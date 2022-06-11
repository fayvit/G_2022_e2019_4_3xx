using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


//Credits:
//Original hard-coded solution by NathanJSmith: https://answers.unity.com/questions/1538195/unity-lod-billboard-asset-example.html?childToView=1692072#answer-1692072
//Cutomization to script by BarShiftGames on above link, and below link
//Inspiration/Feedback/More coding optimization by SomeGuy22 : https://answers.unity.com/questions/1692296/unable-to-save-generatedrendered-texture2d-to-use.html

#if UNITY_EDITOR
public class GenerateBillboard : ScriptableWizard
{
    [Header("")]
    [Tooltip("This should be a Nature/Speedtree billboard material")]
    public Material m_material;
    [Tooltip("How much to pinch the mesh at a certain point, to cut off extra pixels off the mesh that won't be needed.")]
    [Range(0, 1)]
    public float topWidth = 1;
    [Tooltip("How much to pinch the mesh at a certain point, to cut off extra pixels off the mesh that won't be needed.")]
    [Range(0, 1)]
    public float midWidth = 1;
    [Tooltip("How much to pinch the mesh at a certain point, to cut off extra pixels off the mesh that won't be needed.")]
    [Range(0, 1)]
    public float botWidth = 1;

    [SerializeField] private bool autoPreencherTamanhos = true;
    [Tooltip("Units in height of the object, roughly, this can be fine-tuned later on the final asset")]
    public float objectHeight = 0;
    [Tooltip("Units in width of the object, roughly, this can be fine-tuned later on the final asset")]
    public float objectWidth = 0;
    [Tooltip("Usually negative and small, to make it sit in the ground slightly, can be modifed on final asset")]
    public float bottomOffset = 0;

    [Tooltip("The amount of rows in the texture atlas")]
    [Min(1)]
    public int atlasRowImageCount = 3;
    [Tooltip("The amount of columns in the texture atlas")]
    [Min(1)]
    public int atlasColumnImageCount = 3;
    [Tooltip("The total number of images to bake, ALSO decides how many angles to view from")]
    [Min(1)]
    public int totalImageCount = 8;

    [Header("-")]
    [Tooltip("This dictates the rotational center of the render for the billboard, and what is rotated to get different angles.\nThis also checks once for an object with named \"BillboardCameraArm\"")]
    public GameObject toRotateCamera;
    [Tooltip("This should be child of toRotateCamera, and on the local +x axis from it, facing center with a complete view of the object")]
    public Camera renderCamera;

    [Header("Dimensios of atlas")]
    public int atlasPixelWidth = 1024;
    public int atlasPixelHeight = 1024;

    [Header("Optional renderer to set in")]
    public BillboardRenderer optionalBillboardRenderer;
    [SerializeField] private GameObject objectToRender;
    [Range(0, 3)] public float colorVar = .01f;

    private bool doOnce = true;
    private bool checkArmOnce = true;
    private bool preencheuTamanho = false;
    private Dictionary<GameObject, int> layerDic = new Dictionary<GameObject, int>();

    void ColocarRenderLayer(GameObject G)
    {
        layerDic.Add(G, G.layer);
        G.layer = 11;
        for (int i = 0; i < G.transform.childCount; i++)
            ColocarRenderLayer(G.transform.GetChild(i).gameObject);

        //layer
    }

    void DevolveLayers()
    {
        foreach (var v in layerDic)
            v.Key.layer = v.Value;
    }

    void CameraSetup(int tanto)
    {
        //grab the main camera and mess with it for rendering the object - make sure orthographic
        //cam = new GameObject().AddComponent<Camera>();

        Camera cam2 = renderCamera;

        cam2.orthographic = true;
        cam2.cullingMask = (int)Mathf.Pow(2, 11);
        //cam2.clearFlags = CameraClearFlags.SolidColor;
        //render to screen rect area equal to out image size
        float rw = atlasPixelWidth / atlasColumnImageCount;
        rw /= Screen.width;
        float rh = atlasPixelHeight / atlasRowImageCount;
        rh /= Screen.height;
        cam2.rect = new Rect(0, 0, rw, rh);
        //**manually set the background color
        //cam2.backgroundColor = new Color(1, 0, .75f, 0);

        //grab size of object to render - place/size camera to fit
        Bounds bb = objectToRender.GetComponent<Renderer>().bounds;

        //place camera looking at centre of object - and backwards down the z-axis from it

        Vector3[] visoes = new Vector3[totalImageCount];
        Vector3 refV = -Vector3.right;
        for (int i = 0; i < totalImageCount; i++)
        {
            visoes[i] = refV;
            refV = Quaternion.Euler(0, -360 / totalImageCount, 0)*refV;
            refV.Normalize();
            //Debug.Log(refV);
        }
        //{
        //    -Vector3.right,
        //    -(Vector3.right+Vector3.forward).normalized,
        //    -Vector3.forward,
        //    -(Vector3.forward-Vector3.right).normalized,
        //    Vector3.right,
        //    (Vector3.forward+Vector3.right).normalized,
        //    Vector3.forward,
        //    (Vector3.forward-Vector3.right).normalized
        //};

        cam2.transform.position = bb.center - 1.3f * (bb.extents.z + bb.extents.x) * visoes[tanto];
        cam2.transform.LookAt(objectToRender.transform);
        cam2.transform.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(cam2.transform.forward, Vector3.up));
        cam2.transform.position.Set(cam2.transform.position.x, cam2.transform.position.y, -1.0f + (bb.min.z * 2.0f));
        //make clip planes fairly optimal and enclose whole mesh
        cam2.nearClipPlane = 0.5f;
        cam2.farClipPlane = Mathf.Abs(cam2.transform.position.z) + 10.0f + bb.max.z;
        //set camera size to just cover entire mesh
        cam2.orthographicSize = 1.01f * Mathf.Max((bb.max.y - bb.min.y) / 2.0f, (bb.max.x - bb.min.x) / 2.0f);
        cam2.transform.position.Set(cam2.transform.position.x, cam2.orthographicSize * 0.05f, cam2.transform.position.y);

    }


    void OnWizardUpdate()
    {
        //string helpString = "";
        bool isValid = (m_material != null && objectHeight != 0 && objectWidth != 0 && renderCamera != null && toRotateCamera != null);

        if (doOnce)
        {
            //this will get activated once
            doOnce = false;
            toRotateCamera = GameObject.Find("BillboardCameraArm");

        }

        if (toRotateCamera != null && checkArmOnce)
        {
            //this will check for a camera under toRotateCamera once
            checkArmOnce = false;
            Camera cam = toRotateCamera.GetComponentInChildren<Camera>();
            if (cam != null) { renderCamera = cam; }
        }

        if (objectToRender && !preencheuTamanho && autoPreencherTamanhos)
        {
            Bounds bb = objectToRender.GetComponent<Renderer>().bounds;
            objectHeight = 1.01f * (bb.max.y - bb.min.y) / 2.0f;
            objectWidth = 1.01f * (bb.max.x - bb.min.x) / 2.0f;
        }
    }


    void OnWizardCreate()
    {
        //function to execute on submit

        BillboardAsset billboard = new BillboardAsset();

        billboard.material = m_material;
        Vector4[] texCoords = new Vector4[totalImageCount];

        ushort[] indices = new ushort[12];
        Vector2[] vertices = new Vector2[6];

        //make texture to save at end
        var texture = new Texture2D(atlasPixelWidth, atlasPixelHeight);
        //make render texture to copy to texture and assign it to camera
        //renderCamera.targetTexture = RenderTexture.GetTemporary(atlasPixelWidth / atlasColumnImageCount, atlasPixelHeight / atlasRowImageCount, 16);
        renderCamera.targetTexture = RenderTexture.GetTemporary(atlasPixelWidth, atlasPixelHeight, 16);
        var renderTex = renderCamera.targetTexture;
        renderCamera.targetTexture = renderTex;

        //reset rotation, but camera should be on local +x axis from rotating object
        //toRotateCamera.transform.eulerAngles = Vector3.zero;
        int imageAt = 0;
        ColocarRenderLayer(objectToRender);
        for (int j = 0; j < atlasRowImageCount; j++)
        {
            for (int i = 0; i < atlasColumnImageCount; i++)
            {
                //i is x, j is y
                if (imageAt < totalImageCount)
                {
                    CameraSetup(imageAt);
                    //atla them left-right, top-bottom, 0,0 is bottom left
                    float xRatio = (float)i / atlasColumnImageCount;
                    float yRatio = (float)(atlasRowImageCount - j - 1) / atlasRowImageCount;

                    //starts at viewing from +x, and rotates camera clockwise around object, uses amount of vertices set (later down) to tell how many angles to view from
                    texCoords[imageAt].Set(xRatio, yRatio, 1f / atlasColumnImageCount, 1f / atlasRowImageCount);
                    imageAt++;

                    //set rect of where to render texture to
                    renderCamera.rect = new Rect(xRatio, yRatio, 1f / atlasColumnImageCount, 1f / atlasRowImageCount);
                    renderCamera.Render();

                    //read pixels on rec
                    //Rect rec = new Rect(xRatio * atlasPixelWidth, yRatio * atlasPixelHeight, (float)1 / atlasColumnImageCount * atlasPixelWidth, (float)1 / atlasRowImageCount * atlasPixelHeight);
                    //texture.ReadPixels(rec, i / atlasColumnImageCount * atlasPixelWidth, (atlasRowImageCount - j - 1) / atlasRowImageCount * atlasPixelHeight);

                    //toRotateCamera.transform.eulerAngles -= Vector3.up * (360 / totalImageCount);
                }
            }
        }


        toRotateCamera.transform.eulerAngles = Vector3.zero;
        renderCamera.rect = new Rect(0, 0, 1, 1);

        RenderTexture pastActive = RenderTexture.active;
        RenderTexture.active = renderTex;
        texture.ReadPixels(new Rect(0, 0, atlasPixelWidth, atlasPixelHeight), 0, 0);
        RenderTexture.active = pastActive;

        texture.Apply();

        //turn all pixels == background-color to transparent

        Color bCol = renderCamera.backgroundColor;
        Color alpha = new Vector4(0, 0, 0, 0);
        alpha.a = 0.0f;
        bool foi = false;
        for (int y = 0; y < atlasPixelHeight; y++)
        {
            for (int x = 0; x < atlasPixelWidth; x++)
            {
                Color c = texture.GetPixel(x, y);
                //**check for difference
                //if (c.r != bCol.r || c.g != bCol.g || c.b != bCol.b)
                //if(Vector3.Distance(new Vector3(c.r,c.g,c.b),new Vector3(bCol.r,bCol.g,bCol.b))>0.1f)
                if (Mathf.Abs(c.r - bCol.r) + Mathf.Abs(c.b - bCol.b) + Mathf.Abs(c.g - bCol.g) > colorVar && c.a != 0)
                    texture.SetPixel(x, y, new Vector4(c.r, c.g, c.b, 1));
                else
                {
                    if (!foi)
                    {
                        Debug.Log(c);
                        foi = true;
                    }
                    texture.SetPixel(x, y, alpha);

                }
            }
        }
        texture.Apply();





        //texCoords[0].Set(0.230981f, 0.33333302f, 0.230981f, -0.33333302f);
        //texCoords[1].Set(0.230981f, 0.66666603f, 0.230981f, -0.33333302f);
        //texCoords[2].Set(0.33333302f, 0.0f, 0.33333302f, 0.23098099f);
        //texCoords[3].Set(0.564314f, 0.23098099f, 0.23098099f, -0.33333302f);
        //texCoords[4].Set(0.564314f, 0.564314f, 0.23098099f, -0.33333403f);
        //texCoords[5].Set(0.66666603f, 0.0f, 0.33333302f, 0.23098099f);
        //texCoords[6].Set(0.89764804f, 0.23098099f, 0.230982f, -0.33333302f);
        //texCoords[7].Set(0.89764804f, 0.564314f, 0.230982f, -0.33333403f);

        //make basic box out of four trinagles, to be able to pinch the top/bottom/middle to cut extra transparent pixels
        //still not sure how this works but it connects vertices to make the mesh
        indices[0] = 4;
        indices[1] = 3;
        indices[2] = 0;
        indices[3] = 1;
        indices[4] = 4;
        indices[5] = 0;
        indices[6] = 5;
        indices[7] = 4;
        indices[8] = 1;
        indices[9] = 2;
        indices[10] = 5;
        indices[11] = 1;

        //set vertices positions on mesh
        vertices[0].Set(-botWidth / 2 + 0.5f, 0);
        vertices[1].Set(-midWidth / 2 + 0.5f, 0.5f);
        vertices[2].Set(-topWidth / 2 + 0.5f, 1);
        vertices[3].Set(botWidth / 2 + 0.5f, 0);
        vertices[4].Set(midWidth / 2 + 0.5f, 0.5f);
        vertices[5].Set(topWidth / 2 + 0.5f, 1);

        //assign data
        billboard.SetImageTexCoords(texCoords);
        billboard.SetIndices(indices);
        billboard.SetVertices(vertices);

        billboard.width = objectWidth;
        billboard.height = objectHeight;
        billboard.bottom = bottomOffset;

        //save assets
        string path;
        int nameLength = AssetDatabase.GetAssetPath(m_material).Length;
        //take out ".mat" prefix
        path = AssetDatabase.GetAssetPath(m_material).Substring(0, nameLength - 4) + ".asset";
        AssetDatabase.CreateAsset(billboard, path);
        path = AssetDatabase.GetAssetPath(m_material).Substring(0, nameLength - 4) + ".png";
        byte[] byteArray = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, byteArray);
        Debug.Log("BILLBOARD ASSET COMPLETED: File saved to " + path + ",\n if pressing save in editor breaks billboard, manually assign texture to material");

        DevolveLayers();

        if (optionalBillboardRenderer != null)
        {
            optionalBillboardRenderer.billboard = billboard;
        }

        //cleanup / qol things
        RenderTexture.ReleaseTemporary(renderTex);
        renderCamera.targetTexture = null;
        m_material.SetTexture("_MainTex", texture);

        AssetDatabase.Refresh();
    }

    [MenuItem("Window/Rendering/Generate Billboard of Object %j")]
    static void MakeBillboard()
    {
        ScriptableWizard.DisplayWizard<GenerateBillboard>(
            "Make Billboard from object", "Create");
    }
}
#endif

