using FayvitSupportSingleton;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Assets.Scripts.tentativaDeTerreno
{
    public class RampInTerrain : EditorWindow
    {
        static bool active;

        public static GameObject startPoint1;
        public static GameObject startPoint2;
        public static GameObject endPoint1;
        public static GameObject endPoint2;
        public static Vector2[] V;

        // Open this from Window menu
        [MenuItem("Window/Ramp in Terrain")]
        static void Init()
        {
            var window = (RampInTerrain)EditorWindow.GetWindow(typeof(RampInTerrain));
            window.Show();
        }

        // Listen to scene event
        void OnEnable() => SceneView.duringSceneGui += OnSceneGUI;
        void OnDisable() => SceneView.duringSceneGui -= OnSceneGUI;

        // Receives scene events
        // Use event mouse click for raycasting
        void OnSceneGUI(SceneView view)
        {
            if (!active)
            {
                return;
            }

            if (Event.current.type == EventType.MouseDown)
            {
                
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hit;

                // Spawn cube on hit location
                if (Physics.Raycast(ray, out hit))
                {

                    //Debug.Log("Hit: " + hit.collider.gameObject.name + " : " + hit.triangleIndex);

                    if (hit.collider.gameObject.TryGetComponent(out Terrain t))
                    {
                        Vector3 terrainPosition = hit.point - t.transform.position;
                        Debug.Log(terrainPosition);
                        Vector3 splatPosition = new Vector3(
                            terrainPosition.x / t.terrainData.size.x,
                            0,
                            terrainPosition.z / t.terrainData.size.z);

                        //Debug.Log("Splat: " + splatPosition + " : " + t.terrainData.heightmapResolution);
                        int x = Mathf.FloorToInt(splatPosition.x * t.terrainData.heightmapResolution);
                        int y = Mathf.FloorToInt(splatPosition.z * t.terrainData.heightmapResolution);
                        float f = t.terrainData.GetHeight(x, y);
                        //t.terrainData.SetHeights(x, y, new float[1, 1] { { 1 } });

                        if (startPoint1 == null)
                        {
                            //Debug.Log("tentativa de altura é: " + f + "Os valores de x e y são: " + x + " : " + y);
                            startPoint1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            startPoint1.transform.position = hit.point;
                            V = new Vector2[4];
                            V[0] = new Vector2Int(x, y);    
                        }
                        else if (startPoint2 == null)
                        {
                            startPoint2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            startPoint2.transform.position = hit.point;
                            V[1] = new Vector2Int(x, y);
                        }
                        else if (endPoint1 == null)
                        {
                            endPoint1 = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                            endPoint1.transform.position = hit.point;
                            V[2] = new Vector2Int(x, y);
                        }
                        else if (endPoint2 == null)
                        {
                            endPoint2 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                            endPoint2.transform.position = hit.point;
                            V[3] = new Vector2Int(x, y);
                            Planificar(t);

                            DestroyImmediate(startPoint1);
                            DestroyImmediate(startPoint2);
                            DestroyImmediate(endPoint1);
                            DestroyImmediate(endPoint2);

                        }
                    }



                }
            }

            
        }

        void Planificar(Terrain T)
        {
            //Vector2[] vv = PontosCardeais();
            int rampmaxLength = (int)Mathf.Max(Mathf.Abs(V[0].y - V[2].y), Mathf.Abs(V[0].x - V[2].x), 
                Mathf.Abs(V[1].y - V[3].y), Mathf.Abs(V[1].x - V[3].x));

            for (int i = 0; i <= rampmaxLength; i++)
            {
                Vector2 P1 = (float)i / rampmaxLength * V[2] + (float)(rampmaxLength - i) / rampmaxLength * V[0];
                Vector2 P2 = (float)i / rampmaxLength * V[3] + (float)(rampmaxLength - i) / rampmaxLength * V[1];

                //Debug.Log("P1: " + P1);
                //Debug.Log("P2: " + P2);
                //Debug.Log("rampMax: " + rampmaxLength + " : " + (float)i / rampmaxLength);

                int lineMaxLength = (int)Mathf.Max(Mathf.Abs(P1.x - P2.x), Mathf.Abs(P1.y - P2.y));

                for (int j = 0; j <= lineMaxLength; j++)
                {
                    Vector2 P3 = (float)(lineMaxLength - j) / lineMaxLength * P1 + (float)j / lineMaxLength * P2;
                    //Debug.Log("P3: " + P3);
                    Vector2Int P4 = new Vector2Int(Mathf.FloorToInt(P3.x), Mathf.FloorToInt(P3.y));

                    if (Vector2.Angle(V[1] - V[0], V[2] - V[0]) > Vector2.Angle(V[2] - V[0], P4 - V[0]) &&
                        Vector2.Angle(V[0] - V[1], V[3] - V[1]) > Vector2.Angle(V[3] - V[1], P4 - V[1])
                        )
                    {
                        //InEditorSupportSingleton.Instance.InvokeInRealTime(() =>
                        //{
                            float f1 = T.terrainData.GetHeight(Mathf.FloorToInt(V[0].x), Mathf.FloorToInt(V[0].y));
                            float f2 = T.terrainData.GetHeight(Mathf.FloorToInt(V[3].x), Mathf.FloorToInt(V[3].y));
                            float f = (float)i / rampmaxLength * f2 + (float)(rampmaxLength - i) / rampmaxLength * f1;

                            //Debug.Log("P4: " + P4 + " f " + f + " : " + i+","+ j+ " : " + rampmaxLength);
                            //Debug.Log("P1 height: " + T.terrainData.GetHeight(Mathf.FloorToInt(P1.x), Mathf.FloorToInt(P1.y)));
                            //Debug.Log("P2 height: " + T.terrainData.GetHeight(Mathf.FloorToInt(P2.x), Mathf.FloorToInt(P2.y)));
                            //Debug.Log("height test: "+T.terrainData.heightmapScale);
                            T.terrainData.SetHeights(P4.x, P4.y, new float[1, 1] { { f / T.terrainData.heightmapScale.y  } });
                            //return;
                        //}, (j * i + j) * 10f);
                    }
                }
            }
        }




        // Creates a editor window with button 
        // to toggle raycasting on/off
        void OnGUI()
        {
            if (GUILayout.Button("Enable Raycasting"))
            {
                active = !active;
            }

            GUILayout.Label("Active:" + active);
            EditorGUILayout.ObjectField(startPoint1, typeof(object), true);


        }

    }
}