using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Criatures2021.BasicCriatures
{
    public class PoolPets_ddepoisDoBugDaUnity : MonoBehaviour
    {

        #region CustomEditor
#if UNITY_EDITOR
        [CustomEditor(typeof(PoolPets_ddepoisDoBugDaUnity))]
        public class PoolPetsEditor : Editor
        {
            bool ativosExpandido;
            bool inativosExpandido;
            Dictionary<PetName, bool> ativosExpandidos = new Dictionary<PetName, bool>();
            Dictionary<PetName, bool> inativosExpandidos = new Dictionary<PetName, bool>();
            //SerializedProperty ativos;
            //SerializedProperty inativos;

            //void OnEnable()
            //{
            //	ativos = (target as PoolPets_ddepoisDoBugDaUnity).ativos;
            //	inativos = serializedObject.FindProperty("inativos");
            //}

            void DesenharPropriedade<T>(ref bool ativosExpandido, Dictionary<PetName, T> ativos, Dictionary<PetName, bool> ativosExpandidos, string labelname) where T : ICollection<GameObject>
            {
                EditorGUILayout.BeginVertical();

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Expandir", GUILayout.Width(0.25f * Screen.width), GUILayout.MaxWidth(100)))
                {
                    ativosExpandido = !ativosExpandido;
                }

                GUILayout.Label(labelname);
                GUILayout.Label(ativosExpandido.ToString());
                EditorGUILayout.EndHorizontal();
                if (ativosExpandido && ativos != null)
                {
                    foreach (var v in ativos)
                    {
                        GUIStyle skin = new GUIStyle(GUI.skin.box);
                        skin.normal.textColor = Color.yellow;
                        EditorGUILayout.BeginHorizontal();
                        //EditorGUI.indentLevel = 2;
                        GUILayout.Space(20);

                        bool val;
                        if (!ativosExpandidos.TryGetValue(v.Key, out val))
                        {
                            ativosExpandidos.Add(v.Key, false);
                        }

                        if (GUILayout.Button(val ? "Contrair" : "Expandir", GUILayout.Width(0.25f * Screen.width), GUILayout.MaxWidth(100)))
                        {


                            ativosExpandidos[v.Key] = !val;
                        }
                        GUILayout.Box(v.Key.ToString(), skin, GUILayout.Width(.25f * Screen.width), GUILayout.MaxWidth(150));
                        GUILayout.Label("tamanho: " + v.Value.Count);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginVertical();

                        //bool valb = false;
                        ativosExpandidos.TryGetValue(v.Key, out bool valb);
                        if (valb)
                        {
                            foreach (var w in v.Value)
                            {
                                EditorGUILayout.BeginHorizontal();
                                //EditorGUI.indentLevel = 3;
                                GUILayout.Space(40);
                                //GUILayout.Box(w.name);
                                EditorGUILayout.ObjectField(v.Value.ToList().IndexOf(w) + " : ", w.transform, typeof(Transform), true);

                                EditorGUILayout.EndHorizontal();
                            }
                        }
                        EditorGUILayout.EndVertical();


                    }

                    EditorGUI.indentLevel = 1;
                }
                //if (inativos.isExpanded)
                //{

                //}
                EditorGUILayout.EndVertical();
            }

            public override void OnInspectorGUI()
            {
                Repaint();
                serializedObject.Update();

                Dictionary<PetName, HashSet<GameObject>> ativos = (target as PoolPets_ddepoisDoBugDaUnity).ativos;
                Dictionary<PetName, HashSet<GameObject>> inativos = (target as PoolPets_ddepoisDoBugDaUnity).inativos;


                DesenharPropriedade(ref ativosExpandido, ativos, ativosExpandidos, "ativos");
                DesenharPropriedade(ref inativosExpandido, inativos, inativosExpandidos, "inativos");


                serializedObject.ApplyModifiedProperties();
            }
        }
#endif 
        #endregion

        public Dictionary<PetName, HashSet<GameObject>> ativos = new Dictionary<PetName, HashSet<GameObject>>();
        public Dictionary<PetName, HashSet<GameObject>> inativos = new Dictionary<PetName, HashSet<GameObject>>();

        public static PoolPets_ddepoisDoBugDaUnity instance;

        void Start()
        {

            instance = this;
        }

        private void OnDestroy()
        {
            foreach (var v in ativos)
                foreach (var w in v.Value)
                    Destroy(w);


            foreach (var v in inativos)
                foreach (var w in v.Value)
                    Destroy(w);
        }

        public GameObject GetPetGO(PetName p)
        {
            if (inativos.ContainsKey(p) && inativos[p].Count > 0)
            {
                GameObject G = inativos[p].ToList()[0];
                inativos[p].Remove(G);
                if (ativos.ContainsKey(p))
                {
                    ativos[p].Add(G);
                }
                else
                    ativos.Add(p, new HashSet<GameObject> { G });

                G.SetActive(true);
                return G;
            }
            else
            {
                GameObject G = Instantiate(Resources.Load<GameObject>("Criatures/" + p.ToString()));
                if (ativos.ContainsKey(p))
                    ativos[p].Add(G);
                else
                    ativos.Add(p, new HashSet<GameObject> { G });
                return G;
            }
        }

        public void DisablePetGO(GameObject G, PetName p)
        {
            ativos[p].Remove(G);
            if (inativos.ContainsKey(p))
                inativos[p].Add(G);
            else
                inativos.Add(p, new HashSet<GameObject> { G });
            G.SetActive(false);
        }
    }
}