using UnityEngine;
using System.Reflection;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
#endif


namespace FayvitBasicTools
{

    public class BuscadorDeID
    {
        public static void Validate(ref string ID, MonoBehaviour m,string labelID = "ID")
        {
#if UNITY_EDITOR
            Event e = Event.current;
            bool foi = false;
            if (e != null)
            {
                foi = e.commandName == "Duplicate" || e.commandName == "Paste";
            }

            bool ePrefabStage = PrefabStageUtility.GetCurrentPrefabStage() != null;

            if ((ID == "0" || ID == "" || foi) && m.gameObject.scene.name != null && !ePrefabStage)
            {
                Debug.Log("quem foi? meu ID: " + ID + "foi?: " + foi + " : " + m.gameObject.scene.name + " e prefab stage: " + ePrefabStage);

                ID = m.GetInstanceID() + "_" + m.gameObject.scene.name;
                SetUniqueIdProperty(m, ID, labelID);
            }
            else if ((ePrefabStage || m.gameObject.scene.name == null) && (ID != "0" || ID != string.Empty))
            {
                Debug.Log("limpando label do buscador de ID: " + m.gameObject.name + ", PrefabStage: " + ePrefabStage + ", Cena nula: " + (m.gameObject.scene.name == null));
                ID = string.Empty;
                SetUniqueIdProperty(m, ID, labelID);
            }
#endif
        }

        public static string GetUniqueID(GameObject G, string id)
        {
#if UNITY_EDITOR
            PropertyInfo inspectorModeInfo =
        typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

            SerializedObject serializedObject = new SerializedObject(G.transform);
            inspectorModeInfo.SetValue(serializedObject, InspectorMode.Debug, null);

            SerializedProperty localIdProp =
                serializedObject.FindProperty("m_LocalIdentfierInFile");   //note the misspelling!

            int localId = localIdProp.intValue;

            return localId.ToString();
#endif
#pragma warning disable 0162
            return id;
#pragma warning restore 0162
        }

        public static void SetUniqueIdProperty(Object o, string id, string nomeProperty)
        {
#if UNITY_EDITOR

            PropertyInfo inspectorModeInfo =
        typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

            SerializedObject serializedObject = new SerializedObject(o);
            inspectorModeInfo.SetValue(serializedObject, InspectorMode.Debug, null);

            SerializedProperty localIdProp =
                serializedObject.FindProperty(nomeProperty);

            Debug.Log(localIdProp.stringValue + " : " + ((MonoBehaviour)o).name);
            localIdProp.stringValue = id;
#endif
        }


    }
}