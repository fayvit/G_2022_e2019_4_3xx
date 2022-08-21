using UnityEngine;
using FayvitBasicTools;
using TextBankSpace;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TalkSpace
{
    [System.Serializable]
    public class ScheduledTalkManager : TalkManagerBase
    {
        #region inspector
        [SerializeField] private FalasAgendaveis[] falas = null;
        #endregion

        private int ultimoIndice = -1;

        
        
        [System.Serializable]
        private class FalasAgendaveis
        {
            #region Editor_Space
#if UNITY_EDITOR

            [CustomPropertyDrawer(typeof(FalasAgendaveis), true)]
            public class FalasAgendaveisEditor : PropertyDrawer
            {
                public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
                {
                    if (prop.isExpanded)
                    {
                        SerializedProperty useStringCond = prop.FindPropertyRelative("useStringCondicional");
                        SerializedProperty useKeyShiftProp = prop.FindPropertyRelative("useKeyshift");
                        SerializedProperty useStringForKeyTextProp = prop.FindPropertyRelative("useStringForKeyText");
                        int sum = 0;
                        if (useStringCond.boolValue)
                            sum += 20;
                        if (useKeyShiftProp.boolValue)
                            sum += 20;
                        if (useStringForKeyTextProp.boolValue)
                            sum += 20;
                        //SerializedProperty colorModify = property.FindPropertyRelative("colorModify");
                        //SerializedProperty spriteModify = property.FindPropertyRelative("spriteModify");

                        //if (colorModify.boolValue && spriteModify.boolValue)
                        //    return 270;
                        //else
                        return 120 + sum;
                    }
                    else
                        return 20;
                }

                public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
                {
                    
                    SerializedProperty useStringCond = prop.FindPropertyRelative("useStringCondicional");
                    SerializedProperty useKeyShiftProp = prop.FindPropertyRelative("useKeyshift");
                    SerializedProperty useStringForKeyTextProp = prop.FindPropertyRelative("useStringForKeyText");
                    SerializedProperty stringCondConv = prop.FindPropertyRelative("stringCondicionalDaConversa");
                    SerializedProperty chaveCondConv = prop.FindPropertyRelative("chaveCondicionalDaConversa");
                    SerializedProperty stringParaconverter = prop.FindPropertyRelative("stringParaConverterEmChaveDeTextoPraConversa");
                    SerializedProperty chaveDaConversa = prop.FindPropertyRelative("chaveDeTextoDaConversa");
                    SerializedProperty repetirProp = prop.FindPropertyRelative("repetir");

                    label.text = ((TextKey)chaveDaConversa.enumValueIndex).ToString();

                    EditorGUI.BeginProperty(pos, label, prop);
                    EditorGUI.PropertyField(new Rect(pos.position, new Vector2(pos.width, 18)), prop, label);

                    if (prop.isExpanded)
                    {
                        EditorGUI.indentLevel = 3;
                        DesenharBools(pos, useStringCond, useKeyShiftProp, useStringForKeyTextProp);
                        DesenharCondicionais(pos, useStringCond, useKeyShiftProp, useStringForKeyTextProp, stringCondConv, chaveCondConv, stringParaconverter);
                        DesenharPadroes(pos, chaveDaConversa, repetirProp);
                    }

                    EditorGUI.EndProperty();
                }

                private void DesenharCondicionais(Rect pos,
                    SerializedProperty useStringCond,
                    SerializedProperty useKeyShiftProp,
                    SerializedProperty useStringForKeyTextProp,
                    SerializedProperty stringCondConv,
                    SerializedProperty chaveCondConv,
                    SerializedProperty stringParaconverter)
                {
                    int carga = 0;
                    if (useStringCond.boolValue)
                    {
                        EditorGUI.PropertyField(new Rect(pos.position - 120 * Vector2.down, new Vector2(pos.width, 18)), stringCondConv);
                        carga += 20;
                    }

                    if (useKeyShiftProp.boolValue)
                    {
                        EditorGUI.PropertyField(new Rect(pos.position - (120 + carga) * Vector2.down, new Vector2(pos.width, 18)), chaveCondConv);
                        carga += 20;
                    }

                    if (useStringForKeyTextProp.boolValue)
                        EditorGUI.PropertyField(new Rect(pos.position - (120 + carga) * Vector2.down, new Vector2(pos.width, 18)), stringParaconverter);
                }

                void DesenharBools(Rect xPos, SerializedProperty useStringCond, SerializedProperty useKeyShiftProp, SerializedProperty useStringForKeyTextProp)
                {
                    //Color backgroundColor = EditorGUIUtility.isProSkin
                    //    ? new Color32(56, 56, 56, 255)
                    //    : new Color32(194, 194, 194, 255);

                    //backgroundColor *= .75f;
                    Rect pos = new Rect(xPos.x + 30, xPos.y, xPos.width - 30, xPos.height);
                    GUIStyle newStyle = new GUIStyle(GUI.skin.button);
                    //GUI.SelectionGrid(new Rect(pos.position - 80 * Vector2.down, new Vector2(pos.size.x, 0.1f * pos.size.y)), 0, new string[2] { "a", "b" }, 2);

                    //EditorGUI.DrawRect(new Rect(pos.position - 100 * Vector2.down, new Vector2(pos.size.x, 0.2f * pos.size.y)), backgroundColor);

                    GUI.Box(new Rect(pos.position + 20 * Vector2.up/* - (150) * Vector2.down + 5 * Vector2.right*/,
                new Vector2(pos.width, 55)), "");


                    useStringCond.boolValue = GUI.Toggle(new Rect(pos.position - 30 * Vector2.down
                        + .02f * pos.size.x * Vector2.right
                        , new Vector2(0.31f * pos.size.x, 40)), useStringCond.boolValue, "string\n cond", newStyle);
                    useKeyShiftProp.boolValue = GUI.Toggle(new Rect(pos.position - 30 * Vector2.down
                        + 0.35f * pos.size.x * Vector2.right
                        , new Vector2(0.31f * pos.size.x, 40)), useKeyShiftProp.boolValue, "KeyShift\n cond", newStyle);
                    useStringForKeyTextProp.boolValue = GUI.Toggle(new Rect(pos.position - 30 * Vector2.down
                        + 0.68f * pos.size.x * Vector2.right
                        , new Vector2(0.31f * pos.size.x, 40)), useStringForKeyTextProp.boolValue, "string\n TextKey", newStyle);

                }

                void DesenharPadroes(Rect pos, SerializedProperty chaveDaConversa, SerializedProperty repetirProp)
                {
                    EditorGUI.PropertyField(new Rect(pos.position - 80 * Vector2.down, new Vector2(pos.width, 18)), chaveDaConversa);
                    EditorGUI.PropertyField(new Rect(pos.position - 100 * Vector2.down, new Vector2(pos.width, 18)), repetirProp);
                }
            }
#endif
            #endregion

            [SerializeField] private bool useStringCondicional;
            [SerializeField] private bool useKeyshift;
            [SerializeField] private bool useStringForKeyText;
            [SerializeField] private string stringCondicionalDaConversa;
            [SerializeField] private KeyShift chaveCondicionalDaConversa=KeyShift.sempretrue;
            [SerializeField] private string stringParaConverterEmChaveDeTextoPraConversa;
            [SerializeField] private TextKey chaveDeTextoDaConversa;
            [SerializeField] private int repetir = 0;

            public string StringCondicionalDaConversa { 
                get => stringCondicionalDaConversa; 
                set => stringCondicionalDaConversa = value; 
            }

            public KeyShift ChaveCondicionalDaConversa
            {
                
                get { return chaveCondicionalDaConversa; }
                set { chaveCondicionalDaConversa = value; }
            }

            public TextKey ChaveDeTextoDaConversa
            {
                get { return chaveDeTextoDaConversa; }
                set { chaveDeTextoDaConversa = value; }
            }

            public int Repetir { get { return repetir; } set { repetir = value; } }

            public void OnVallidate()
            {
                if (!string.IsNullOrEmpty(stringParaConverterEmChaveDeTextoPraConversa))
                {
                    chaveDeTextoDaConversa = StringForEnum.GetEnum(stringParaConverterEmChaveDeTextoPraConversa,TextKey.bomDia);
                }

                if (!useKeyshift)
                    chaveCondicionalDaConversa = KeyShift.sempretrue;
            }
        }

        public void OnVallidate()
        {
            foreach (var v in falas)
                v.OnVallidate();
        }

        void VerificaQualFala()
        {
            KeyVar myKeys = AbstractGameController.Instance.MyKeys;

            Debug.Log("ultimo indice no inicio: " + ultimoIndice);


            int indiceFinal = ultimoIndice > 0 ? Mathf.Min(ultimoIndice, falas.Length) : falas.Length;


            for (int i = 0; i < indiceFinal; i++)
            {
                if (myKeys.VerificaAutoShift(falas[i].ChaveCondicionalDaConversa)
                    &&
                    myKeys.VerificaAutoShift(falas[i].StringCondicionalDaConversa)
                    )
                {
                    Debug.Log("eu passo aqui?"+ falas[i].ChaveDeTextoDaConversa);
                    ChangeTalkKey(falas[i].ChaveDeTextoDaConversa);
                    conversa = TextBank.RetornaListaDeTextoDoIdioma(falas[i].ChaveDeTextoDaConversa).ToArray();
                    ultimoIndice = i;
                }
            }

            Debug.Log(indiceFinal + " : " + ultimoIndice);

            if (falas[ultimoIndice].Repetir >= 0)
            {
                string kCont = falas[ultimoIndice].ChaveCondicionalDaConversa.ToString()+ falas[ultimoIndice].StringCondicionalDaConversa;

                myKeys.SomaAutoCont(kCont, 1);
                if (falas[ultimoIndice].Repetir < myKeys.VerificaAutoCont(kCont))
                {
                    if(falas[ultimoIndice].ChaveCondicionalDaConversa!=KeyShift.sempretrue)
                        myKeys.MudaShift(falas[ultimoIndice].ChaveCondicionalDaConversa, false);

                    if (!string.IsNullOrEmpty(falas[ultimoIndice].StringCondicionalDaConversa))
                        myKeys.MudaAutoShift(falas[ultimoIndice].StringCondicionalDaConversa, false);

                }

            }

        }

        override public void IniciaConversa()
        {
            VerificaQualFala();
            base.IniciaConversa();
        }
    }
}