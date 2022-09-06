using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using FayvitSupportSingleton;
using FayvitMessageAgregator;
using FayvitBasicTools;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FayvitUI
{
    public abstract class InteractiveUiBase
    {
        [SerializeField] protected GameObject aContainerItem;
        [SerializeField] protected RectTransform variableSizeContainer;
        [SerializeField] protected ScrollRect menuScrollRect;
        [SerializeField] protected Sprite selectedSprite;
        [SerializeField] protected Sprite standardSprite;
        [SerializeField] protected Color selectedColor = Color.gray;
        [SerializeField] protected Color standardColor = Color.white;
        [SerializeField] private bool colorModify = true;
        [SerializeField] private bool spriteModify = false;

        #region Editor_Space
#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(InteractiveUiBase), true)]
        public class InteractiveUiBaseEditor : PropertyDrawer
        {

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                if (property.isExpanded)
                {
                    SerializedProperty colorModify = property.FindPropertyRelative("colorModify");
                    SerializedProperty spriteModify = property.FindPropertyRelative("spriteModify");

                    if (colorModify.boolValue && spriteModify.boolValue)
                        return 270;
                    else
                    return 205;
                }
                else
                    return 20;
            }

            public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
            {
                
                EditorGUI.BeginProperty(pos, label, prop);

                SerializedProperty sItem = prop.FindPropertyRelative("aContainerItem");
                SerializedProperty varSize = prop.FindPropertyRelative("variableSizeContainer");
                SerializedProperty menuScrollRect = prop.FindPropertyRelative("menuScrollRect");
                SerializedProperty selectedSprite = prop.FindPropertyRelative("selectedSprite");
                SerializedProperty standardSprite = prop.FindPropertyRelative("standardSprite");
                SerializedProperty selectedColor = prop.FindPropertyRelative("selectedColor");
                SerializedProperty standardColor = prop.FindPropertyRelative("standardColor");
                SerializedProperty colorModify = prop.FindPropertyRelative("colorModify");
                SerializedProperty spriteModify = prop.FindPropertyRelative("spriteModify");

                int highlightOption = 0;
                if (colorModify.boolValue && spriteModify.boolValue)
                    highlightOption = 2;
                else if (spriteModify.boolValue)
                    highlightOption = 1;

                
                EditorGUI.PropertyField(new Rect(pos.position, new Vector2(pos.width, 18)), prop, label);

                if (prop.isExpanded)
                {
                    EditorGUI.indentLevel = 1;                 

                    DesenharPadroes(pos, sItem, varSize, menuScrollRect);

                    if (CheckHighlightStyle(highlightOption, pos, colorModify, spriteModify))
                    {
                        
                    }

                    VerifyColorHighlight(colorModify,selectedColor, standardColor,pos);
                    VerifySpriteHighlight(colorModify,spriteModify, selectedSprite, standardSprite, pos);

                }
         
                EditorGUI.EndProperty();

            }

            void VerifySpriteHighlight(
                SerializedProperty colorModify,
                SerializedProperty spriteModify,
                SerializedProperty selectedSprite,
                SerializedProperty standardSprite,
                Rect pos
                )
            {
                
                if (spriteModify.boolValue)
                {
                    int yDesl = colorModify.boolValue ? 65 : 0;

                    BaseHighlightModifiers(pos, "Standard Sprite", "Selected Sprite", yDesl, selectedSprite, standardSprite);
                }
            }

            void VerifyColorHighlight(
                SerializedProperty colorModify,
                SerializedProperty selectedColor,
                SerializedProperty standardColor,
                Rect pos
                )
            {
                
                if (colorModify.boolValue)
                {
                    BaseHighlightModifiers(pos, "Standard Color", "Selected Color",0, selectedColor, standardColor);
                }
            }

            void BaseHighlightModifiers(
                Rect pos,string labelStandard,string labelSelected,int yDesl,
                SerializedProperty selectedProp,
                SerializedProperty standardProp
                )
            {
                Color backgroundColor = EditorGUIUtility.isProSkin
                        ? new Color32(56, 56, 56, 255)
                        : new Color32(194, 194, 194, 255);

                backgroundColor *= .75f;

                EditorGUI.DrawRect(new Rect(pos.position - (140+yDesl) * Vector2.down,
                new Vector2(pos.width, 60)), backgroundColor);

                GUI.Box(new Rect(pos.position - (150+yDesl) * Vector2.down + 5 * Vector2.right,
                new Vector2(.97f * pos.width, 40)), "");

                EditorGUI.LabelField(new Rect(pos.position - (150+yDesl) * Vector2.down, new Vector2(.45f * pos.width, 18)), labelSelected);
                EditorGUI.LabelField(new Rect(pos.position - (150+yDesl) * Vector2.down + .5f * pos.width * Vector2.right,
                    new Vector2(.45f * pos.width, 18)), labelStandard);

                EditorGUI.PropertyField(new Rect(pos.position - (165+yDesl) * Vector2.down, new Vector2(.45f * pos.width, 18)),
                        selectedProp, new GUIContent(""));
                EditorGUI.PropertyField(new Rect(pos.position - (165+yDesl) * Vector2.down + .5f * pos.width * Vector2.right,
                    new Vector2(.45f * pos.width, 18)),
                    standardProp, new GUIContent(""));
            }

            bool CheckHighlightStyle(int highlightOption,Rect pos, SerializedProperty colorModify, SerializedProperty spriteModify)
            {
                GUIStyle newStyle = new GUIStyle(GUI.skin.label);
                newStyle.normal.textColor = new Color(.52f, .52f, .52f);
                newStyle.alignment = TextAnchor.MiddleCenter;
                EditorGUI.LabelField(new Rect(pos.position - 90 * Vector2.down, new Vector2(pos.width, 18)),
                    "Highlight Style", newStyle);

                int ant = highlightOption;
                highlightOption = GUI.SelectionGrid(
                    new Rect(pos.position - 110 * Vector2.down + .05f * pos.width * Vector2.right,
                    new Vector2(.9f * pos.width, 22)),
                     highlightOption, new string[3] { "Color", "Sprite", "Color and Sprite" }, 3);
                    

                switch (highlightOption)
                {
                    case 0:
                        colorModify.boolValue = true;
                        spriteModify.boolValue = false;
                        break;
                    case 1:
                        colorModify.boolValue = false;
                        spriteModify.boolValue = true;
                        break;
                    case 2:
                        colorModify.boolValue = true;
                        spriteModify.boolValue = true;
                        break;
                }

                return ant != highlightOption;
               // return highlightOption;
            }

            void DesenharPadroes(Rect pos,
            SerializedProperty sItem,
            SerializedProperty varSize,
            SerializedProperty menuScrollRect)
            {

                
                EditorGUI.PropertyField(new Rect(pos.position - 20 * Vector2.down, new Vector2(pos.width, 18)), sItem);
                EditorGUI.PropertyField(new Rect(pos.position - 40 * Vector2.down, new Vector2(pos.width, 18)), varSize);
                EditorGUI.PropertyField(new Rect(pos.position - 60 * Vector2.down, new Vector2(pos.width, 18)), menuScrollRect);

            }
        }
#endif
        #endregion

        public virtual Transform GetTransformContainer { get => menuScrollRect.transform.parent; }
        public abstract void SetContainerItem(GameObject G, int indice);
        protected abstract void AfterFinisher();

        private float contadorDeTempo = 0;
        private const float TEMPO_DE_SCROLL = .25F;
        private bool emEstadoDeAcao;

        public int SelectedOption { get; private set; } = 0;

        public bool IsActive
        {
            get { return menuScrollRect.transform.parent.gameObject.activeSelf; }
        }

        protected void StartHud(int quantidade, ResizeUiType tipo = ResizeUiType.vertical, int selectIndex = 0)
        {   
            VerifySprites();
            SelectedOption = selectIndex;
            variableSizeContainer.parent.parent.gameObject.SetActive(true);

            aContainerItem.SetActive(true);

            if (tipo == ResizeUiType.vertical)
                ResizeUI.InVertical(variableSizeContainer, aContainerItem, quantidade);
            else if (tipo == ResizeUiType.grid)
                ResizeUI.InGrid(variableSizeContainer, aContainerItem, quantidade);
            else if (tipo == ResizeUiType.horizontal)
                ResizeUI.InHorizontal(variableSizeContainer, aContainerItem, quantidade);

            for (int i = 0; i < quantidade; i++)
            {
                GameObject G = ParentingInTheHUD.Parenting(aContainerItem, variableSizeContainer);
                SetContainerItem(G, i);

                G.name += i.ToString();
                AnOption An = G.GetComponent<AnOption>();
                if (i == SelectedOption)
                {
                    HighlightSelected(An);
                }
                else
                {
                    RemoveHighlightFromSelected(An);
                }
            }

            aContainerItem.SetActive(false);

            if (selectIndex == 0)
            {
                if (menuScrollRect != null)
                    if (menuScrollRect.verticalScrollbar)
                        menuScrollRect.verticalScrollbar.value = 1;

                if (menuScrollRect != null)
                    if (menuScrollRect.horizontalScrollbar)
                        menuScrollRect.horizontalScrollbar.value = 0;
                ScheduleScrollPos();
            }
            else
                SupportSingleton.Instance.InvokeOnEndFrame(() =>
                {
                    ChangeSelectionTo(selectIndex,ignoreSound:true);
                });

        }

        protected System.Action<int> ActionToDelayButton(System.Action<int> action)
        {
            return (int x) =>
            {
                if (!emEstadoDeAcao)
                {
                    emEstadoDeAcao = true;

                    SupportSingleton.Instance.InvokeInRealTime(() =>
                    {
                        Debug.Log("Função chamada com delay para destaque do botão");
                        action(x);
                        ChangeSelectionTo(x);
                        emEstadoDeAcao = false;
                    }, .05f);
                }
            };
        }

        #region PrivateMethodRegion
        private void VerifySprites()
        {
            if (selectedSprite == null)
            {
                selectedSprite = aContainerItem.GetComponent<Image>().sprite;
            }

            if (standardSprite == null)
                standardSprite = aContainerItem.GetComponent<Image>().sprite;
        }

        private void ScheduleScrollPos()
        {
            SupportSingleton.Instance.StartCoroutine(ScrollPos());
        }

        private void UpdateHighlight(AnOption[] umaS)
        {
            for (int i = 0; i < umaS.Length; i++)
            {
                if (i == SelectedOption)
                {
                    HighlightSelected(umaS[i]);
                }
                else
                {
                    RemoveHighlightFromSelected(umaS[i]);
                }
            }
        }

        private void FixScroll(AnOption[] umaS, int rowCellCount)
        {
            contadorDeTempo = 0;
            SupportSingleton.Instance.StartCoroutine(MoveScroll(umaS, rowCellCount));

        }

        private void RemoveAllHighlights(AnOption[] umaS)
        {
            for (int i = 0; i < umaS.Length; i++)
            {
                RemoveHighlightFromSelected(umaS[i]);
            }
        }

        private IEnumerator ScrollPos()
        {
            yield return new WaitForEndOfFrame();//new WaitForSecondsRealtime(0.01f);

            if (menuScrollRect != null)
                if (menuScrollRect.verticalScrollbar)
                {
                    menuScrollRect.verticalScrollbar.value = 1;
                }


            if (menuScrollRect != null)
                if (menuScrollRect.horizontalScrollbar)
                    menuScrollRect.horizontalScrollbar.value = 0;

            yield return new WaitForEndOfFrame();

            if (menuScrollRect != null)
                if (menuScrollRect.verticalScrollbar)
                {

                    if (menuScrollRect.verticalScrollbar.value != 1)
                        ScheduleScrollPos();

                }


            if (menuScrollRect != null)
                if (menuScrollRect.horizontalScrollbar)
                    if (menuScrollRect.horizontalScrollbar.value != 0)
                        ScheduleScrollPos();


        }
        #endregion


        #region publicMethodRegion

        public void ChangeInteractiveButtons(bool interactable)
        {
            Button[] bs = variableSizeContainer.GetComponentsInChildren<Button>();
            foreach(Button b in bs)
                b.interactable = interactable;
        }

        public void RemoveHighlights()
        {
            AnOption[] umaS = variableSizeContainer.GetComponentsInChildren<AnOption>();
            RemoveAllHighlights(umaS);
        }

        public void HighlightSelectedOption()
        {
            AnOption uma = variableSizeContainer.transform.GetChild(SelectedOption + 1).GetComponent<AnOption>();
            HighlightSelected(uma);
        }

        public void ChangeSelectionTo(int qual,int rowCellCount=-1,bool ignoreSound=false)
        {
            AnOption[] umaS = variableSizeContainer.GetComponentsInChildren<AnOption>();
            RemoveAllHighlights(umaS);
            SelectiAnOption(qual);
            
            float destiny = CalcScrollDestiny(umaS.Length, rowCellCount);

            Scrollbar s = GetActiveScrollBar();

            if (s != null)
                s.value = destiny;

            if(!ignoreSound)
            MessageAgregator<MsgChangeOptionUI>.Publish(new MsgChangeOptionUI()
            {
                parentOfScrollRect = menuScrollRect.transform.parent.gameObject,
                selectedOption = SelectedOption
            });

        }

        public void ChangeOptionWithVal(int quanto, int rowCellCount = -1,bool clamp=false)
        {
            if (quanto != 0)
            {
                AnOption[] umaS = variableSizeContainer.GetComponentsInChildren<AnOption>();
                int antOption = SelectedOption;
                if (!clamp)
                    SelectedOption = ContadorCiclico.Contar(quanto, SelectedOption, umaS.Length);
                else
                    SelectedOption = Mathf.Clamp(SelectedOption+quanto, 0, umaS.Length-1);
                #region removida
                //if (quanto > 0)
                //{
                //    if (SelectedOption + quanto < umaS.Length)
                //        SelectedOption += quanto;
                //    else
                //        SelectedOption = 0;
                //}
                //else if (quanto < 0)
                //{
                //    if (SelectedOption + quanto >= 0)
                //        SelectedOption += quanto;
                //    else
                //        SelectedOption = umaS.Length - 1;
                //}
                #endregion

                UpdateHighlight(umaS);

                if (menuScrollRect != null)
                    if (menuScrollRect.verticalScrollbar || menuScrollRect.horizontalScrollbar)
                    {

                        FixScroll(umaS, rowCellCount);
                    }
                    else
                    {
                        Debug.Log("erro scroll 2");
                    }

                else
                    Debug.Log("erro no scrool");

                if (antOption != SelectedOption)
                    MessageAgregator<MsgChangeOptionUI>.Publish(new MsgChangeOptionUI()
                    {
                        parentOfScrollRect = menuScrollRect.transform.parent.gameObject,
                        selectedOption = SelectedOption
                    });
                //EventAgregator.Publish(
                //    new GameEvent(EventKey.UiDeOpcoesChange, menuScrollRect.transform.parent.gameObject, SelectedOption));
            }
        }

        public void SelectiAnOption(int qual)
        {
            if (variableSizeContainer.childCount > qual + 1)
            {
                SelectedOption = qual;
                AnOption uma = variableSizeContainer.GetChild(qual + 1).GetComponent<AnOption>();
                HighlightSelected(uma);
            }
        }

        public void FinishHud(int starter = 1)
        {
            BeforeFinisher();

            //Debug.Log(variableSizeContainer.transform.childCount);

            for (int i = starter; i < variableSizeContainer.transform.childCount; i++)
            {
                //Debug.Log(variableSizeContainer.GetChild(i).name);

                MonoBehaviour.Destroy(variableSizeContainer.GetChild(i).gameObject);
            }

            variableSizeContainer.parent.parent.gameObject.SetActive(false);

            AfterFinisher();
        }
        #endregion


        #region virtualRegion
        public virtual void RemoveHighlightFromSelected(AnOption uma)
        {
            if (colorModify)
                uma.SpriteDoItem.color = standardColor;
            else
                uma.SpriteDoItem.color = Color.white;

            uma.SpriteDoItem.sprite = standardSprite;
        }

        public virtual void HighlightSelected(AnOption uma)
        {

            if (colorModify)
                uma.SpriteDoItem.color = selectedColor;
            else
                uma.SpriteDoItem.color = Color.white;

            if (spriteModify)
                uma.SpriteDoItem.sprite = selectedSprite;
            else
                uma.SpriteDoItem.sprite = standardSprite;



        }

        public virtual void ChangeOption(int val,bool clamp=false)
        {
            //ChangeOptionWithVal(UpdateChangeOption());
            ChangeOptionWithVal(val,clamp:clamp);
        }

        public virtual void ChangeOption_H(int val/*,bool negativar = false*/)
        {

            //  ChangeOptionWithVal((negativar ? -1 : 1) * UpdateChangeOption(false));
            ChangeOptionWithVal(val);
        }

        private float CalcScrollDestiny(int numChilds,int colCellCount)
        {
            int val = (colCellCount == -1) ? numChilds : Mathf.CeilToInt((float)numChilds / colCellCount);
            int opc = SelectedOption / ((colCellCount == -1) ? 1 : colCellCount);

            float retorno = Mathf.Clamp((float)(val - opc - 1) / Mathf.Max(val - 1, 1), 0, 1);


            return E_Horizontal()?1-retorno: retorno;
        }

        bool E_Horizontal()
        {
            return menuScrollRect.horizontalScrollbar != null;
        }

        Scrollbar GetActiveScrollBar()
        {
            Scrollbar s = null;
            if (menuScrollRect != null)
            {
                if (menuScrollRect.verticalScrollbar != null)
                    s = menuScrollRect.verticalScrollbar;
                else if (menuScrollRect.horizontalScrollbar != null)
                    s = menuScrollRect.horizontalScrollbar;
            }
            return s;
        }

        protected virtual IEnumerator MoveScroll(AnOption[] umaS, int rowCellCount)
        {

            yield return new WaitForSecondsRealtime(0.01f);
            yield return new WaitForEndOfFrame();

            contadorDeTempo += Time.fixedDeltaTime;

            float destiny = CalcScrollDestiny(umaS.Length,rowCellCount);

            Scrollbar s = GetActiveScrollBar();
            

            if (s != null)
            {
                s.value = Mathf.Lerp(s.value,
                    destiny, contadorDeTempo / TEMPO_DE_SCROLL);

                if (s.value != destiny)
                    SupportSingleton.Instance.StartCoroutine(MoveScroll(umaS, rowCellCount));

            }

            //GlobalController.g.StartCoroutine(MovendoScroll(umaS, rowCellCount));
        }

        protected virtual IEnumerator MoveScroll_H(AnOption[] umaS, int rowCellCount)
        {
            yield return new WaitForEndOfFrame();//new WaitForSecondsRealtime(0.01f);
            int val = (rowCellCount == -1) ? umaS.Length : Mathf.CeilToInt((float)umaS.Length / rowCellCount);
            int opc = SelectedOption / ((rowCellCount == -1) ? 1 : rowCellCount);

            contadorDeTempo += Time.fixedDeltaTime;
            float destiny = 1 - Mathf.Clamp((float)(val - opc - 1) / Mathf.Max(val - 1, 1), 0, 1);

            menuScrollRect.horizontalScrollbar.value = Mathf.Lerp(menuScrollRect.horizontalScrollbar.value,
                destiny, contadorDeTempo / TEMPO_DE_SCROLL);


            if (menuScrollRect.horizontalScrollbar.value != destiny)
                SupportSingleton.Instance.StartCoroutine(MoveScroll_H(umaS, rowCellCount));


            // GlobalController.g.StartCoroutine(MovendoScroll(umaS, rowCellCount));
        }

        protected virtual void BeforeFinisher() { }


        #endregion

        public static int UpdateChangeOption(bool vertical = true)
        {
            int quanto = 0;
            Debug.LogWarning("Fazer verifica mudar Opcao");
            if (vertical)
            {
                /*
                quanto = -CommandReader.ValorDeGatilhos("VDpad", 1);

                if (quanto == 0)
                    quanto = -CommandReader.ValorDeGatilhos("vertical", -1);*/

            }
            else
            {
                /*
                quanto = CommandReader.ValorDeGatilhos("HDpad", 1);

                if (quanto == 0)
                    quanto = -CommandReader.ValorDeGatilhos("horizontal", -1);*/

            }

            return quanto;
        }
        
    }
}