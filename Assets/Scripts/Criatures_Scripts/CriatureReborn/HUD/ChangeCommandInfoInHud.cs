using FayvitCommandReader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.ScriptsLadoB.FayvitAdressable;
using FayvitBasicTools;
using FayvitMessageAgregator;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif




public class ChangeCommandInfoInHud : MonoBehaviour
{
    [SerializeField] private CommandConverterInt commandIntName;
    [SerializeField] private CommandConverterString commandName;
    [SerializeField] private GameObject txt1Container;
    [SerializeField] private GameObject img1container;
    [SerializeField] private Text t1;
    [SerializeField] private Image img1;
    [SerializeField] private GameObject txt2Container;
    [SerializeField] private GameObject img2container;
    [SerializeField] private Text t2;
    [SerializeField] private Image img2;
    [SerializeField] private PosOrNeg sinal;
    [SerializeField] private TypeOfCommandConverter type;

    private enum PosOrNeg
    {
        positivo,
        negativo
    }

    private enum TypeOfCommandConverter
    { 
        intButton,
        stringPositeveAndNegative,
        stringPositeveOrNegative
    }

    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(ChangeCommandInfoInHud))]
    public class ChangeCommandInfoInHudEditor : Editor
    {
        private SerializedProperty commandIntName;
        private SerializedProperty commandName;
        private SerializedProperty txt1Container;
        private SerializedProperty img1container;
        private SerializedProperty t1;
        private SerializedProperty img1;
        private SerializedProperty txt2Container;
        private SerializedProperty img2container;
        private SerializedProperty t2;
        private SerializedProperty img2;
        private SerializedProperty sinal;
        private SerializedProperty type;

        private void OnEnable()
        {
            commandIntName = serializedObject.FindProperty("commandIntName");
            commandName = serializedObject.FindProperty("commandName");
            txt1Container = serializedObject.FindProperty("txt1Container");
            img1container = serializedObject.FindProperty("img1container");
            t1 = serializedObject.FindProperty("t1");
            img1 = serializedObject.FindProperty("img1");
            txt2Container = serializedObject.FindProperty("txt2Container");
            img2container = serializedObject.FindProperty("img2container");
            t2 = serializedObject.FindProperty("t2");
            img2 = serializedObject.FindProperty("img2");
            sinal = serializedObject.FindProperty("sinal");
            type = serializedObject.FindProperty("type");


        }

        void Basic()
        {
           
            EditorGUILayout.PropertyField(t1);
            EditorGUILayout.PropertyField(txt1Container);
            
            EditorGUILayout.PropertyField(img1);
            EditorGUILayout.PropertyField(img1container);
            
        }


        public override void OnInspectorGUI()
        {

            type.intValue = GUILayout.SelectionGrid(type.intValue, new string[3] { "int Button", "PosNeg AND", "PosNeg OR" }, 3);


            switch (type.intValue)
            {
                case 0:
                    EditorGUILayout.PropertyField(commandIntName);
                    Basic();
                    break;
                case 1:

                    EditorGUILayout.PropertyField(commandName);

                    Basic();

                    
                    EditorGUILayout.PropertyField(t2);
                    EditorGUILayout.PropertyField(txt2Container);
                    
                    EditorGUILayout.PropertyField(img2);
                    EditorGUILayout.PropertyField(img2container);
                    
                    break;
                case 2:
                    EditorGUILayout.PropertyField(commandName);

                    Basic();

                    EditorGUILayout.PropertyField(sinal);
                    break;

            }
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif 
    #endregion

    void ColocarInfo()
    {
        switch (type)
        {
            case TypeOfCommandConverter.intButton:
                CommandsDictionary.SelectShowInfo(
                    img1container, img1, txt1Container, t1, commandIntName, AbstractGlobalController.Instance.Control);
            break;
            case TypeOfCommandConverter.stringPositeveAndNegative:
                CommandsDictionary.SelectShowInfo(img1container,img2container, img1,img2, txt1Container,txt2Container,
                    t1,t2, commandName, AbstractGlobalController.Instance.Control);
            break;
            case TypeOfCommandConverter.stringPositeveOrNegative:
                CommandsDictionary.SelectShowInfo(
                img1container, img1, txt1Container, t1, commandName,sinal==PosOrNeg.positivo, AbstractGlobalController.Instance.Control);
            break;
        }
    }

    private void OnEnable()
    {
        if (StaticInstanceExistence<IGlobalController>.SchelduleExistence(
            OnEnable, this, () => { return AbstractGlobalController.Instance; }))
        {
            ColocarInfo();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        MessageAgregator<MsgChangeHardwareControler>.AddListener(OnChangeController);
    }

    private void OnDestroy()
    {
        MessageAgregator<MsgChangeHardwareControler>.RemoveListener(OnChangeController);
    }

    private void OnChangeController(MsgChangeHardwareControler obj)
    {
        ColocarInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
