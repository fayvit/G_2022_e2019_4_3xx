using FayvitMessageAgregator;
using FayvitSupportSingleton;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace CustomizationSpace
{
    [System.Serializable]
    public class TesteMeshCombiner
    {
        [SerializeField] private SectionCustomizationManager myBase;
        [SerializeField] private SectionCustomizationManager myMBase;
        [SerializeField] private SectionCustomizationManager myHBase;
        [SerializeField] private float deltaVertex;

        private SectionCustomizationManager target;
        private GameObject parentObj;
        private GameObject gameObject;// target GameObject
        private List<Vector3> vertices = new List<Vector3>();
        private List<Vector3> weldVertices = new List<Vector3>();
        private List<Vector2> uvs = new List<Vector2>();
        private List<Vector3> normals = new List<Vector3>();
        //private List<Vector4> tangents = new List<Vector4>();
        private List<List<int>> triangles = new List<List<int>>();
        private List<Material> materials = new List<Material>();
        private List<BoneWeight> boneWeights = new List<BoneWeight>();

        private int vertCount;

        private SkinnedMeshRenderer mR;

        public GameObject StartCombiner(CustomizationContainerDates ccd, GameObject baseM, GameObject baseH, string checkKey = "")
        {
            if (ccd.PersBase == PersonagemBase.feminino)
                myMBase = baseM.GetComponent<SectionCustomizationManager>();
            else
                myHBase = baseH.GetComponent<SectionCustomizationManager>();

            return StartCombiner(ccd, checkKey);
        }

        public GameObject StartCombiner(CustomizationContainerDates ccd, string checkKey = "")
        {
            ClearFields();

            if (ccd.PersBase == PersonagemBase.feminino)
                myBase = myMBase;
            else
                myBase = myHBase;

            target = MonoBehaviour.Instantiate(myBase, myBase.transform.position, myBase.transform.rotation);
            target.gameObject.SetActive(true);

            InEditorSupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
            //Debug.Log("pos first combination singleton");
            target.SetCustomDates(ccd);

                InEditorSupportSingleton.Instance.InvokeOnEndFrame(() =>
                {
                    //Debug.Log("second");
                    Transform T = CombinerReallyStart();


                    MessageAgregator<MsgCombinationComplete>.Publish(new MsgCombinationComplete()
                    {
                        combined = T,
                        checkKey = checkKey
                    });


                });
            });

            return target.gameObject;
        }

        private void ClearFields()
        {
            triangles.Clear();
            uvs.Clear();
            materials.Clear();
            boneWeights.Clear();
            normals.Clear();
            vertices.Clear();
            weldVertices.Clear();
            vertCount = 0;
        }

        public GameObject StartCombiner(SectionCustomizationManager source, string checkKey = "")
        {
            //if (!source.masculino)
            //    myBase = myMBase;
            //else
            //    myBase = myHBase;



            CustomizationContainerDates ccd = source.GetCustomDates();

            return StartCombiner(ccd, checkKey);
        }

        void VerifyWeldVertex(SkinnedMeshRenderer meshR, Transform child)
        {
            if (meshR != null && meshR.CompareTag("WeldVertex"))
            {
                foreach (var V in meshR.sharedMesh.vertices)
                {

                    weldVertices.Add(child.TransformPoint(V) - parentObj.transform.position);
                }

                //Debug.Log(weldVertices.Count + " são weld vertex");
            }
        }

        void GuardMaterialsAndSetTriangles(SkinnedMeshRenderer meshR)
        {
            foreach (var mat in meshR.sharedMaterials)
            {

                if (!materials.Contains(mat))
                {
                    materials.Add(mat);
                    triangles.Add(new List<int>());
                }
            }
        }

        void GuardBoneInfos(SkinnedMeshRenderer meshR, Transform child)
        {
            for (int i = 0; i < meshR.sharedMesh.vertices.Length; i++)
            {
                vertices.Add(child.TransformPoint(meshR.sharedMesh.vertices[i]) - parentObj.transform.position);

                if (meshR.sharedMesh.boneWeights.Length > i)
                    boneWeights.Add(meshR.sharedMesh.boneWeights[i]);
                else
                    boneWeights.Add(new BoneWeight() { boneIndex0 = 0, weight0 = 1 });
            }
        }

        Transform CombinerReallyStart()
        {

            //GameObject paiDeTodos = new GameObject("PaiDeTodos");
            //paiDeTodos.transform.position = target.transform.position;

            //Debug.Log("o nome pe combinado");
            gameObject = new GameObject("Combinado ***");

            //gameObject.transform.SetParent(paiDeTodos.transform);
            //gameObject.transform.localPosition = Vector3.zero;


            gameObject.transform.position = target.transform.position;
            mR = gameObject.AddComponent<SkinnedMeshRenderer>();

            parentObj = target.gameObject;

            //Debug.Log("Really starting combine Mesh");

            Transform[] children = parentObj.GetComponentsInChildren<Transform>();

            SkinnedMeshRenderer meshRr = null;

            foreach (var child in children)
            {
                SkinnedMeshRenderer meshR = child.GetComponent<SkinnedMeshRenderer>();

                VerifyWeldVertex(meshR, child);


                if (meshR != null
                    && !meshR.gameObject.CompareTag("skCabelo")
                    && !meshR.gameObject.CompareTag("WeldVertex")
                    && !(meshR.gameObject.layer == 9)
                    )
                {

                    meshRr = meshR;
                    GuardMaterialsAndSetTriangles(meshR);

                    GuardBoneInfos(meshR, child);


                    for (int i = 0; i < meshR.sharedMesh.subMeshCount; i++)
                    {

                        int triIndex = GetTrianglesIndex(meshR.sharedMaterials[i]);

                        int[] tris = meshR.sharedMesh.GetTriangles(i);

                        for (int t = 0; t < tris.Length; t++)
                        {
                            triangles[triIndex].Add(vertCount + tris[t]);
                        }
                    }

                    //tangents.AddRange(meshR.sharedMesh.tangents);

                    uvs.AddRange(meshR.sharedMesh.uv);
                    vertCount = vertices.Count;

                    int q = normals.Count;
                    normals.AddRange(meshR.sharedMesh.normals);
                    for (int i = q; i < normals.Count; i++)
                    {
                        normals[i] = child.TransformPoint(normals[i]) - parentObj.transform.position;
                    }

                }
            }

            Mesh mesh = new Mesh();

            if (vertices.Count > 65535)
                mesh.indexFormat = IndexFormat.UInt32;

            mesh.subMeshCount = triangles.Count;
            mesh.vertices = vertices.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.boneWeights = boneWeights.ToArray();

            mesh.normals = normals.ToArray();

            for (int i = 0; i < triangles.Count; i++)
            {
                mesh.SetTriangles(triangles[i].ToArray(), i);
            }

            mR.bones = meshRr.bones;
            mR.rootBone = meshRr.bones[0];

            var bindPoses = new Matrix4x4[mR.bones.Length];

            for (int i = 0; i < bindPoses.Length; i++)
            {
                bindPoses[i] = mR.bones[i].worldToLocalMatrix * gameObject.transform.localToWorldMatrix;
            }

            mesh.bindposes = bindPoses;

            mR.sharedMaterials = materials.ToArray();

            RemoveDuplicateVertices(mesh);

            #region tentativasDaqui
            //mesh.tangents = tangents.ToArray();

            //mesh.GetSubMesh(0).;

            //mesh.CombineMeshes(lm.ToArray(),);
            //MeshWelder mw = new MeshWelder(mesh);

            //mw.Weld();

            //mesh = mw.GetMesh;
            #endregion

            mR.sharedMesh = mesh;



            foreach (var child in children)
            {
                SkinnedMeshRenderer meshR = child.GetComponent<SkinnedMeshRenderer>();

                if (meshR != null && !meshR.gameObject.CompareTag("skCabelo") && !(meshR.gameObject.layer == 9))
                {
                    if (Application.isPlaying)
                        MonoBehaviour.Destroy(meshR.gameObject);
                    else
                        MonoBehaviour.DestroyImmediate(meshR.gameObject);
                }
            }

            
            for (int i = 0; i < parentObj.transform.childCount; i++)
                if (!parentObj.transform.GetChild(i).gameObject.activeSelf)
                    MonoBehaviour.Destroy(parentObj.transform.GetChild(i).gameObject);
            //Transform metarig = target.transform.Find("metarig");
            //children = parentObj.GetComponentsInChildren<Transform>();
            //foreach (var child in children)
            //    if (child.name != "metarig" &&!FayvitBasicTools.HierarchyTools.EstaNaHierarquia(metarig,child)&&child!=parentObj.transform)
            //        MonoBehaviour.Destroy(child.gameObject);

            gameObject.transform.SetParent(parentObj.transform);
            MonoBehaviour.Destroy(parentObj.GetComponent<SectionDataBaseContainer>());
            MonoBehaviour.Destroy(parentObj.GetComponent<SectionCustomizationManager>());
            parentObj.GetComponent<Animator>().enabled = true;
            parentObj.SetActive(false);
            InEditorSupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                parentObj.SetActive(true);
            });

            return mR.transform.parent;
        }

        class MeshDates
        {
            public List<Vector3> lV = new List<Vector3>();
            public List<Vector2> lUV = new List<Vector2>();
            public List<BoneWeight> lB = new List<BoneWeight>();
            public List<List<int>> lT = new List<List<int>>();

        }

        void RemoveDuplicateVertices(Mesh mesh)
        {
            MeshDates M = new MeshDates();

            M.lV.AddRange(mesh.vertices);
            M.lUV.AddRange(mesh.uv);
            M.lB.AddRange(mesh.boneWeights);
            M.lT.AddRange(triangles);


            #region umaOutraTentativa
            //bool repita;
            //do
            //{

            //    repita = VerifiqueIgualdade(M);
            //} while (repita);
            #endregion

            //SubMeshDescriptor smd = mesh.GetSubMesh(3);
            //int max = smd.indexStart + smd.indexCount;

            for (int i = 0; i < M.lV.Count; i++)
            {
                for (int j = i + 1; j < M.lV.Count; j++)
                {
                    if (M.lV[i] == M.lV[j])
                    {
                        foreach (var V in weldVertices)
                        {
                            if (Vector3.SqrMagnitude(V - M.lV[i]) <= deltaVertex)
                            {
                                //if (weldVertices.Contains(M.lV[i]))
                                //if (!EstaoNaMesmaSub(mesh, i, j))


                                SubstituaNosTriangulos(i, j, M);
                                M.lUV[j] = M.lUV[i];

                            }
                        }

                    }
                }
            }


            //Debug.Log("numero de vertices: " + M.lV.Count+" numero de ossos: "+M.lB.Count);
            mesh.vertices = M.lV.ToArray();
            mesh.boneWeights = M.lB.ToArray();

            for (int i = 0; i < M.lT.Count; i++)
            {
                mesh.SetTriangles(M.lT[i].ToArray(), i);
            }

        }

        #region tentativas
        //bool EstaoNaMesmaSub(Mesh m,int i, int j)
        //{
        //    for (int I = 0; I < m.subMeshCount; I++)
        //    {
        //        SubMeshDescriptor smd = m.GetSubMesh(I);
        //        int end = smd.indexStart + smd.indexCount;
        //        if (
        //            smd.indexStart <= i 
        //            && i < end 
        //            && smd.indexStart <= j 
        //            && j < end)
        //        {
        //            Debug.Log("Os indices: " + i + " e " + j + " estão na sub: " + I);
        //            return true;
        //        }

        //    }
        //    return false;
        //}
        #endregion

        void SubstituaNosTriangulos(int i, int j, MeshDates M)
        {

            for (int I = 0; I < M.lT.Count; I++)
            {
                for (int J = 0; J < M.lT[I].Count; J++)
                {
                    if (M.lT[I][J] == j)
                    {
                        M.lT[I][J] = i;
                    }
                }
            }
        }



        bool VerifiqueIgualdade(MeshDates M)
        {
            for (int i = 0; i < M.lV.Count; i++)
            {
                for (int j = 0; j < M.lV.Count; j++)
                {
                    if (i != j)
                    {
                        if (M.lV[i] == M.lV[j])
                        {
                            M.lV.RemoveAt(j);
                            M.lB.RemoveAt(j);

                            ReplaceTrianglesID(M, i, j);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        void ReplaceTrianglesID(MeshDates M, int i, int j)
        {
            for (int I = 0; I < M.lT.Count; I++)
            {
                for (int J = 0; J < M.lT[I].Count; J++)
                {
                    if (M.lT[I][J] == j)
                        M.lT[I][J] = i;
                }
            }
        }

        Transform[] MetarigToArray(GameObject toArray, Transform[] reference, string ObjectName, string rigName)
        {
            Transform T = toArray.transform;
            List<Transform> tt = new List<Transform>();

            foreach (var tRef in reference)
            {
                Transform finded = T.Find(CaminhoDoFilho(tRef, ObjectName, rigName));

                if (finded is null)
                {
                    Debug.Log("Encontrei um nulo");
                }
                else
                {
                    tt.Add(finded);
                }
            }

            return tt.ToArray();

        }

        string CaminhoDoFilho(Transform tRef, string objectName, string metaRigName)
        {
            string s = tRef.name;
            Transform otoTref = tRef;
            while (otoTref.parent != null)
            {
                otoTref = otoTref.parent;

                if (otoTref.name != objectName && otoTref.name != metaRigName)
                    s = otoTref.name + "/" + s;
            }

            Debug.Log(s);

            return s;
        }

        int GetTrianglesIndex(Material material)
        {
            int index = 0;

            for (int i = 0; i < materials.Count; i++)
            {
                if (material == materials[i])
                {
                    index = i;
                    break;
                }
            }

            return index;
        }
    }

    public static class UmaStaticClass
    {
        private class Vertices
        {
            List<Vector3> verts = null;
            List<Vector2> uv1 = null;
            List<Vector2> uv2 = null;
            List<Vector2> uv3 = null;
            List<Vector2> uv4 = null;
            List<Vector3> normals = null;
            List<Vector4> tangents = null;
            List<Color32> colors = null;
            List<BoneWeight> boneWeights = null;

            public Vertices()
            {
                verts = new List<Vector3>();
            }
            public Vertices(Mesh aMesh)
            {
                verts = CreateList(aMesh.vertices);
                uv1 = CreateList(aMesh.uv);
                uv2 = CreateList(aMesh.uv2);
                uv3 = CreateList(aMesh.uv3);
                uv4 = CreateList(aMesh.uv4);
                normals = CreateList(aMesh.normals);
                tangents = CreateList(aMesh.tangents);
                colors = CreateList(aMesh.colors32);
                boneWeights = CreateList(aMesh.boneWeights);
            }

            private List<T> CreateList<T>(T[] aSource)
            {
                if (aSource == null || aSource.Length == 0)
                    return null;
                return new List<T>(aSource);
            }
            private void Copy<T>(ref List<T> aDest, List<T> aSource, int aIndex)
            {
                if (aSource == null)
                    return;
                if (aDest == null)
                    aDest = new List<T>();
                aDest.Add(aSource[aIndex]);
            }
            public int Add(Vertices aOther, int aIndex)
            {
                int i = verts.Count;
                Copy(ref verts, aOther.verts, aIndex);
                Copy(ref uv1, aOther.uv1, aIndex);
                Copy(ref uv2, aOther.uv2, aIndex);
                Copy(ref uv3, aOther.uv3, aIndex);
                Copy(ref uv4, aOther.uv4, aIndex);
                Copy(ref normals, aOther.normals, aIndex);
                Copy(ref tangents, aOther.tangents, aIndex);
                Copy(ref colors, aOther.colors, aIndex);
                Copy(ref boneWeights, aOther.boneWeights, aIndex);
                return i;
            }
            public void AssignTo(Mesh aTarget)
            {
                if (verts.Count > 65535)
                    aTarget.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
                aTarget.SetVertices(verts);
                if (uv1 != null) aTarget.SetUVs(0, uv1);
                if (uv2 != null) aTarget.SetUVs(1, uv2);
                if (uv3 != null) aTarget.SetUVs(2, uv3);
                if (uv4 != null) aTarget.SetUVs(3, uv4);
                if (normals != null) aTarget.SetNormals(normals);
                if (tangents != null) aTarget.SetTangents(tangents);
                if (colors != null) aTarget.SetColors(colors);
                if (boneWeights != null) aTarget.boneWeights = boneWeights.ToArray();
            }
        }

        public static Mesh GetSubmesh(this Mesh aMesh, int aSubMeshIndex)
        {
            if (aSubMeshIndex < 0 || aSubMeshIndex >= aMesh.subMeshCount)
                return null;
            int[] indices = aMesh.GetTriangles(aSubMeshIndex);
            Vertices source = new Vertices(aMesh);
            Vertices dest = new Vertices();
            Dictionary<int, int> map = new Dictionary<int, int>();
            int[] newIndices = new int[indices.Length];
            for (int i = 0; i < indices.Length; i++)
            {
                int o = indices[i];
                int n;
                if (!map.TryGetValue(o, out n))
                {
                    n = dest.Add(source, o);
                    map.Add(o, n);
                }
                newIndices[i] = n;
            }
            Mesh m = new Mesh();
            dest.AssignTo(m);
            m.triangles = newIndices;
            return m;
        }
    }

    public struct MsgCombinationComplete : IMessageBase
    {
        public Transform combined;
        public string checkKey;
    }
}