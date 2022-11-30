using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Criatures2021.BasicCriatures
{
	public class PoolPets_ddepoisDoBugDaUnity : MonoBehaviour
	{
#if UNITY_EDITOR
		[CustomEditor(typeof(PoolPets_ddepoisDoBugDaUnity))]
		public class PoolPetsEditor : Editor
		{
			bool ativosExpandido;
			bool inativosExpandido;
			//SerializedProperty ativos;
			//SerializedProperty inativos;

			//void OnEnable()
			//{
			//	ativos = (target as PoolPets_ddepoisDoBugDaUnity).ativos;
			//	inativos = serializedObject.FindProperty("inativos");
			//}

			public override void OnInspectorGUI()
            {
				Dictionary<PetName, List<GameObject>> ativos = (target as PoolPets_ddepoisDoBugDaUnity).ativos;
				Dictionary<PetName, List<GameObject>> inativos = (target as PoolPets_ddepoisDoBugDaUnity).inativos;
				
				serializedObject.Update();
				EditorGUILayout.BeginVertical();

				EditorGUILayout.BeginHorizontal();

				if (GUILayout.Button("Expandir", GUILayout.MaxWidth(.25f * Screen.width)))
				{
					ativosExpandido = !ativosExpandido;
				}
				
				GUILayout.Label("ativos");
				GUILayout.Label(ativosExpandido.ToString());
				EditorGUILayout.EndHorizontal();
                if (ativosExpandido && ativos!=null)
                {
                    foreach (var v in ativos)
                    {
                        EditorGUI.indentLevel = 2;
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Box(v.Key.ToString());
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUI.indentLevel = 1;
                }
                //if (inativos.isExpanded)
                //{

                //}
                EditorGUILayout.EndVertical();

				serializedObject.ApplyModifiedProperties();
            }
        }
#endif

		public Dictionary<PetName, List<GameObject>> ativos;
		public Dictionary<PetName, List<GameObject>> inativos;

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
				GameObject G = inativos[p][0];
				inativos[p].Remove(G);
				if (ativos.ContainsKey(p))
				{
					ativos[p].Add(G);
				}
				else
					ativos.Add(p, new List<GameObject> { G });

				return G;
			}
			else
			{
				GameObject G = Instantiate(Resources.Load<GameObject>("Criatures/" + p.ToString()));
				ativos.Add(p, new List<GameObject> { G });
				return G;
			}
		}

		public void DisablePetGO(GameObject G, PetName p)
		{
			ativos[p].Remove(G);
			if (inativos.ContainsKey(p))
				inativos[p].Add(G);
			else
				inativos.Add(p, new List<GameObject> { G });
		}
	}
}