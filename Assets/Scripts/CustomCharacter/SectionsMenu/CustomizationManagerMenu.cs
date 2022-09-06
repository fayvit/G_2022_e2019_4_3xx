using System.Collections.Generic;
using UnityEngine;
using FayvitUI;
using FayvitSupportSingleton;
using FayvitMessageAgregator;
using FayvitEventAgregator;
using FayvitCommandReader;
using FayvitCam;
using FayvitBasicTools;
using UnityEngine.EventSystems;
using TextBankSpace;

namespace CustomizationSpace
{
    public class CustomizationManagerMenu : MonoBehaviour
    {
        [SerializeField] private InputTextManager inputTextManager;
        [SerializeField] private ConfirmationPanel confirmation;
        [SerializeField] private CustomizationMenu cMenu;
        [SerializeField] private SectionCustomizationManager secManager;
        [SerializeField] private SectionCustomizationManager secManagerH_Base;
        [SerializeField] private SectionCustomizationManager secManagerM_Base;
        [SerializeField] private SectionDataBaseContainer sdbc;
        [SerializeField] private SectionDataBaseContainer sdbc_H;
        [SerializeField] private SectionDataBaseContainer sdbc_M;
        [SerializeField] private FlagSectionDataBase menusAtivos = FlagSectionDataBase.@base;
        [SerializeField] private TesteMeshCombiner testMeshCombiner;
        [SerializeField] private GetColorHudManager myGetColor;
        [SerializeField] private ColorGridMenu mySuggestionColors;
        [SerializeField] private GlobalColorMenu globalCM;
        [SerializeField] private BasicMenu globalMenu;
        [SerializeField] private CharDbManager charDbManager;
        [SerializeField] private AudioClip menuMusic;
        [SerializeField] private float velValForGreyScale = 3;
        [SerializeField] private float velValForColor = 3;

        private ColorDbManager cdbManager;
        private EstadoDoMenu estado = EstadoDoMenu.main;
        private RegistroDeCores transitoryReg;

        public enum EstadoDoMenu
        {
            main,
            colorGrid,
            colorCircle,
            globalizationColors,
            globalizationMenu,
            confirmacaoAberta,
            characterSaveChanges
        }

        private EditableElements[] activeEditables;

        [System.Flags]
        private enum FlagSectionDataBase
        {
            @base = SectionDataBase.@base,
            cabelo = SectionDataBase.cabelo,
            queixo = SectionDataBase.queixo,
            globoOcular = 4,
            pupila = 8,
            iris = 16,
            umidade = 32,
            sobrancelha = 64,
            barba = 128,
            torso = 256,
            mao = 512,
            cintura = 1024,
            pernas = 2048,
            botas = 4096,
            particular = 8192,
            nariz = 16384,
            empty = 32768
        }

        public enum EditableType
        {
            control,
            color,
            mesh,
            texture,
            personagemBase,
            conclusao
        }

        public struct EditableElements
        {
            /// <summary>
            /// DataBase ao qual o objeto é membro
            /// </summary>
            public SectionDataBase member;
            /// <summary>
            /// Tipo de editavel. Para ser usado na inserção dos itens do container
            /// </summary>
            public EditableType type;
            public string displayName;
            /// <summary>
            /// Indice da coleção(dataBase) ao qual o elemento pertence
            /// </summary>
            public int outIndex;
            /// <summary>
            /// Indice do elemento dentro do seu objeto. utilizavel no caso de várias cores editaveis, varias subseções e etc..
            /// </summary>
            public int inIndex;
            public SectionDataBase mySDB;

        }

        ICommandReader CurrentCommander
        {
            get => CommandReader.GetCR(AbstractGlobalController.Instance.Control);
        }

        EditableElements[] ActiveEditables
        {
            get
            {
                ChangebleElement changeE = sdbc.GetChangebleElementWithId(SectionDataBase.@base)[0];
                SectionDataBase[] s = changeE.subsection;

                List<EditableElements> retorno = new List<EditableElements>();

                EditableElements thisElement = new EditableElements()
                {
                    mySDB = SectionDataBase.particular,
                    displayName = TextBank.RetornaListaDeTextoDoIdioma(TextKey.customizationParts)[17],//Genero
                    inIndex = 0,
                    outIndex = 0,
                    member = SectionDataBase.particular,
                    type = EditableType.personagemBase
                };

                retorno.Add(thisElement);

                for (int i = 0; i < changeE.coresEditaveis.Length; i++)
                {
                    thisElement = new EditableElements()
                    {
                        mySDB = SectionDataBase.@base,
                        displayName = TextBank.RetornaListaDeTextoDoIdioma(TextKey.customizationParts)[18]/*COr*/ + " " + (i + 1),
                        inIndex = i,
                        outIndex = 0,
                        member = SectionDataBase.@base,
                        type = EditableType.color
                    };

                    retorno.Add(thisElement);
                }

                for (int i = 0; i < s.Length; i++)
                {
                    thisElement = new EditableElements()
                    {
                        mySDB = s[i],
                        displayName = TextBank.RetornaListaDeTextoDoIdioma(TextKey.customizationParts)[(int)s[i]],//+" : "+s[i].ToString(),
                        inIndex = i,
                        outIndex = 0,
                        member = SectionDataBase.@base,
                        type = EditableType.control
                    };
                    retorno.Add(thisElement);
                }

                for (int i = 0; i < s.Length; i++)
                {
                    VerifySubSections(retorno, s[i], 1);
                }

                retorno.Add(new EditableElements()
                {
                    displayName = TextBank.RetornaListaDeTextoDoIdioma(TextKey.customizationParts)[19],
                    type = EditableType.conclusao,
                    member = SectionDataBase.empty,
                    mySDB = SectionDataBase.empty
                });

                return retorno.ToArray();
            }
        }

        private void OnUiChange(MsgChangeOptionUI obj)
        {

            if (cMenu.GetTransformContainer.gameObject == obj.parentOfScrollRect)
            {
                MessageAgregator<MsgChangeMenuDb>.Publish(new MsgChangeMenuDb()
                {
                    sdb = activeEditables[obj.selectedOption].mySDB
                });
            }
            else
            {
                if (cdbManager != null)
                {
                    if (obj.parentOfScrollRect == cdbManager.PaiDoGrid)
                    {
                        secManager.ApplyColor(cdbManager.CurrentGridColor);
                    }
                }
                else if (obj.parentOfScrollRect == mySuggestionColors.GetTransformContainer.gameObject)
                {
                    secManager.ApplyColor(mySuggestionColors.GetSelectedColor);
                }
            }
        }

        private void OnChangeInGlobalMessage(ChangeInGlobalColorMessage obj)
        {
            Debug.Log(obj.optionType + " : " + obj.check);

            if (obj.optionType == 1)
            {
                secManager.ApplyColor(globalCM.GetSloteColor(obj.registro));
            }
            else
            {
                if (obj.optionType == 0 && obj.check)
                {
                    secManager.ApplyColor(globalCM.ChangedColor);
                    //secManager.ApplyColor(globalCM.GetSloteColor(obj.registro));
                }
                else
                    secManager.ApplyColor(globalCM.RememberedColor);
            }
        }

        private void OnToggleGlobalColor(ToggleGlobalColorMessage obj)
        {
            secManager.ChangeColorRegIfEqual(obj.indexOfGlobal, globalCM.RememberedColor, globalCM.GetSloteColor(obj.indexOfGlobal));
        }

        private void OnSelectGlobalColor(SelectGlobalColorMessage obj)
        {
            transitoryReg = obj.indexOfGlobal;
            estado = EstadoDoMenu.globalizationMenu;

            EndGlobalColorMenu();
            globalMenu.StartHud(OpcoesDoGlobalizationMenu,
                new string[3] {
                TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDoCustomization)[2]
                /*"Selecionar a cor e não usar registro"*/, 
                /*"Selecionar a cor e usar o registro"*/
                TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDoCustomization)[3]
                , /*"Voltar para o menu anterior"*/ 
            TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDoCustomization)[4]
                });

            EditableElements target = activeEditables[cMenu.SelectedOption];

            Debug.Log(target.member + " : " + target.inIndex);

            ColorContainerStruct ccs = secManager.GetColorAssignById(target.member).coresEditaveis[target.inIndex];
            if (ccs.coresEditaveis.registro == obj.indexOfGlobal)
            {
                globalMenu.ChangeSelectionTo(1);
            }


        }

        private void OpcoesDoGlobalizationMenu(int x)
        {
            switch (x)
            {
                case 0:
                    estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
                    secManager.ChangeColorReg(RegistroDeCores.registravel);
                    break;
                case 1:
                    estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
                    secManager.ChangeColorReg(transitoryReg);
                    break;
                case 2:
                    globalCM.StartHud(globalCM.RememberedColor, secManager.GuardOriginalColor.cor, secManager.VerifyColorReg(), secManager.GetTargetColorReg);
                    StartGlobalColorMenu();
                    break;
            }

            globalMenu.FinishHud();
        }

        private void OnButtonMakeGlobal(ButtonMakeGlobalMessage obj)
        {
            secManager.ChangeRegColor(globalCM.RememberedColor, obj.indexOfGlobal);
            globalCM.SetGlobalViewInTheIndex(obj.indexOfGlobal);
        }

        private void OnChangeColorPicker(IGameEvent e)
        {
            Color C = (Color)e.MySendObjects[0];
            secManager.ApplyColor(C);
        }

        int GetInListDbIndex(List<EditableElements> L, SectionDataBase sdb)
        {
            int retorno = -1;
            for (int i = 1; i < L.Count; i++)
            {
                Debug.Log(sdbc.GetChangebleElementWithId(L[i].member) + " : " + L[i].member + " : " + L[i].type);

                ChangebleElement changeE = sdbc.GetChangebleElementWithId(L[i].member)[L[i].outIndex];

                if (changeE.subsection.Length > 0)
                {
                    if (changeE.subsection[L[i].inIndex] == sdb)
                    {
                        retorno = i;
                    }
                }
            }

            return retorno;
        }

        string DeepView(int deep)
        {
            string s = string.Empty;
            for (int i = 0; i < deep; i++)
            {
                s += " - ";
            }

            return s;
        }

        bool VerifyActiveDbView(SectionDataBase sdb)
        {
            return menusAtivos.HasFlag(StringForEnum.GetEnum<FlagSectionDataBase>(sdb.ToString()));
        }

        void VerifySubSections(List<EditableElements> L, SectionDataBase target, int deep)
        {
            if (!VerifyActiveDbView(target))
                return;


            int dbIndex = secManager.GetActiveIndexOf(target);

            if (dbIndex == -1)
            {
                Debug.Log("Não encontrada referencia no sectionManager para : " + target);
                return;
            }

            int inListIndex = GetInListDbIndex(L, target);

            ChangebleElement changeE = sdbc.GetChangebleElementWithId(target)[dbIndex];


            List<EditableElements> forInsert = new List<EditableElements>();
            if (changeE is SimpleChangebleMesh || changeE is CombinedChangebleMesh)
            {
                forInsert.Add(new EditableElements()
                {
                    mySDB = target,
                    displayName = DeepView(deep) + TextBank.RetornaListaDeTextoDoIdioma(TextKey.customizationParts)[21],//"Type",
                    inIndex = 0,
                    outIndex = dbIndex,
                    member = target,
                    type = EditableType.mesh
                });
            }
            else if (changeE is MaskedTexture)
            {
                forInsert.Add(new EditableElements()
                {
                    mySDB = target,
                    displayName = DeepView(deep) + TextBank.RetornaListaDeTextoDoIdioma(TextKey.customizationParts)[22],//"Detail",
                    inIndex = 0,
                    outIndex = dbIndex,
                    member = target,
                    type = EditableType.texture
                });
            }

            int contCores = 0;
            for (int i = 0; i < changeE.coresEditaveis.Length; i++)
            {
                if (changeE.coresEditaveis[i].registro != RegistroDeCores.skin)
                {
                    contCores++;
                    forInsert.Add(new EditableElements()
                    {
                        mySDB = target,
                        displayName = DeepView(deep) + TextBank.RetornaListaDeTextoDoIdioma(TextKey.customizationParts)[18]/*"Cor "*/ + " " + contCores,
                        inIndex = i,
                        outIndex = dbIndex,
                        member = target,
                        type = EditableType.color
                    });
                }
            }

            SectionDataBase[] s = changeE.subsection;

            for (int i = 0; i < s.Length; i++)
            {
                forInsert.Add(new EditableElements()
                {
                    mySDB = s[i],
                    displayName = DeepView(deep) + " " + s[i].ToString(),
                    inIndex = i,
                    outIndex = 0,
                    member = target,
                    type = EditableType.control
                });
            }

            L.InsertRange(inListIndex + 1, forInsert);

            if (s.Length > 0)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    VerifySubSections(L, s[i], deep + 1);
                }
            }
        }

        string[] GetActiveOption(EditableElements[] elements)
        {
            string[] retorno = new string[elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                retorno[i] = elements[i].displayName;
            }

            return retorno;
        }

        int GetElementIndexOf(EditableElements e)
        {
            int retorno = -1;
            for (int i = 0; i < activeEditables.Length; i++)
            {
                EditableElements elegible = activeEditables[i];

                if (elegible.type == e.type
                    && elegible.member == e.member
                    && elegible.inIndex == e.inIndex)
                {
                    retorno = i;
                }

            }

            return retorno;
        }

        private void ChangeAction(int change, int index)
        {
            EditableElements target = activeEditables[index];

            if (target.type == EditableType.control)
            {
                if (change != 0)
                {

                    SectionDataBase sdb = sdbc.GetChangebleElementWithId(target.member)[target.outIndex].subsection[target.inIndex];
                    FlagSectionDataBase flag = StringForEnum.GetEnum<FlagSectionDataBase>(sdb.ToString());

                    if (change > 0 && !menusAtivos.HasFlag(flag))
                    {
                        MessageAgregator<MsgNegativeUiInput>.Publish();
                        menusAtivos |= flag;

                    }
                    else if (change < 0 && menusAtivos.HasFlag(flag))
                    {
                        MessageAgregator<MsgNegativeUiInput>.Publish();
                        menusAtivos &= ~flag;
                    }

                    RestartMenu(target);

                }
            }
            else if (target.type == EditableType.mesh || target.type == EditableType.texture)
            {

                if (change != 0)
                {
                    ChangeElementMainAction(target, change);
                    MessageAgregator<MsgNegativeUiInput>.Publish();
                }
            }
        }

        EditableElements GetEditableElementBySdb(SectionDataBase sdb)
        {
            EditableElements retorno = new EditableElements();
            for (int i = 0; i < activeEditables.Length; i++)
            {
                if (activeEditables[i].mySDB == sdb && activeEditables[i].type == EditableType.control)
                {
                    retorno = activeEditables[i];
                }
            }

            return retorno;
        }

        void EscapeAction(int index)
        {
            EditableElements target = activeEditables[index];

            if (target.type != EditableType.control && target.member != SectionDataBase.@base)
            {
                FlagSectionDataBase flag = StringForEnum.GetEnum<FlagSectionDataBase>(target.member.ToString());
                if (menusAtivos.HasFlag(flag))
                {
                    menusAtivos &= ~flag;

                    RestartMenu(GetEditableElementBySdb(target.member));
                }
            }
            else if (target.type == EditableType.control)
            {
                SectionDataBase sdb = sdbc.GetChangebleElementWithId(target.member)[target.outIndex].subsection[target.inIndex];
                FlagSectionDataBase flag = StringForEnum.GetEnum<FlagSectionDataBase>(sdb.ToString());
                if (menusAtivos.HasFlag(flag))
                {
                    menusAtivos &= ~flag;

                    RestartMenu(target);
                }
                else if (target.member != SectionDataBase.@base)
                    EscapeAction(index - 1);
            }
        }

        void ChangeBaseCharacter(bool masculino)
        {
            if (masculino)
            {
                secManager = secManagerH_Base;
                sdbc = sdbc_H;
            }
            else
            {
                secManager = secManagerM_Base;
                sdbc = sdbc_M;
            }

            secManagerH_Base.gameObject.SetActive(masculino);
            secManagerM_Base.gameObject.SetActive(!masculino);
        }

        private void MainAction(int index)
        {

            EditableElements target = activeEditables[index];

            if (target.type == EditableType.control)
            {
                ControlMainAction(target);

            }
            else if (target.type == EditableType.mesh || target.type == EditableType.texture)
            {
                ChangeElementMainAction(target);
            }
            else if (target.type == EditableType.color)
            {
                cMenu.ChangeInteractiveButtons(false);
                ColorContainerStruct ccs = secManager.GetColorAssignById(target.member).coresEditaveis[target.inIndex];

                secManager.StartChangeColor(target.member, target.inIndex, ccs);
                ChangeColorMainAction(ccs);
                estado = EstadoDoMenu.colorGrid;
            }
            else if (target.type == EditableType.personagemBase)
            {
                if (secManager == secManagerH_Base)
                {
                    ChangeBaseCharacter(false);
                    DirectionalCamera cDir = CameraApplicator.cam.Cdir;
                    //CameraAplicator.cam.FocusBasicCam(secManager.transform, 0.2f, .7f);
                    CameraApplicator.cam.Cdir.VarVerticalHeightPoint = .7f;
                    SupportSingleton.Instance.InvokeOnEndFrame(() =>
                    {

                        secManager.SetColorsByAssign(secManagerH_Base.ColorAssign);
                    });
                }
                else if (secManager == secManagerM_Base)
                {
                    ChangeBaseCharacter(true);
                    DirectionalCamera cDir = CameraApplicator.cam.Cdir;
                    //CameraAplicator.cam.FocusBasicCam(secManager.transform, 0.2f, .7f);
                    CameraApplicator.cam.Cdir.VarVerticalHeightPoint = .7f;
                    SupportSingleton.Instance.InvokeOnEndFrame(() =>
                    {
                        CameraApplicator.cam.Cdir.VarVerticalHeightPoint = .7f;
                        secManager.SetColorsByAssign(secManagerM_Base.ColorAssign);
                    });
                }
            }
            else if (target.type == EditableType.conclusao)
            {
                cMenu.ChangeInteractiveButtons(false);
                estado = EstadoDoMenu.confirmacaoAberta;
                confirmation.StartConfirmationPanel(
                    () =>
                    {

                        cMenu.FinishHud();
                        //ProvisionalStartGame.InitProvisionalStartGame(
                        //    testMeshCombiner, secManager
                        //    );
                        //FayvitLikeDarkSouls.StartGameWizard.InitProvisionalStartGame(
                        //    testMeshCombiner, secManager
                        //    );

                        MessageAgregator<MsgFinishCustomizationMenu>.Publish(new MsgFinishCustomizationMenu()
                        {
                            secManager = secManager,
                            tCombiner = testMeshCombiner
                        });
                        Destroy(gameObject);
                    },
                    () =>
                    {
                        estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
                    },
                    TextBank.RetornaFraseDoIdioma(TextKey.frasesDoCustomization),//"Iniciar o jogo com esse personagem?",
                    hideSelections: true
                    );
            }
        }

        private void ChangeColorMainAction(ColorContainerStruct ccs)
        {

            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                Color[] C = DbColors.ColorsByDb(ccs.coresEditaveis.registro);
                int selectIndex = DbColors.GetApproximateColorIndex(C, ccs.cor);
                mySuggestionColors.StartHud(C, selectIndex);
                mySuggestionColors.SetActions(mySuggestionColors.ColorActions);
                secManager.ApplyColor(C[selectIndex]);

                SupportSingleton.Instance.InvokeOnEndFrame(() =>
                {
                    MessageAgregator<MsgOpenColorMenu>.Publish(new MsgOpenColorMenu() { reg = ccs.coresEditaveis.registro });
                });

            });
        }

        void ChangeElementMainAction(EditableElements target, int changeVal = 1)
        {
            ChangebleElement[] ce = sdbc.GetChangebleElementWithId(target.member);

            if (ce[0] is SimpleChangebleMesh)
            {
                secManager.TrocaMesh(target.member, ce as SimpleChangebleMesh[], changeVal);
            }
            else if (ce[0] is CombinedChangebleMesh)
            {
                CombinedChangebleMesh[] ccm = ce as CombinedChangebleMesh[];
                CombinedChangebleMesh[] doComb = sdbc.GetCombinedMeshDbWithID(ccm[0].combinedWithDb);
                secManager.ChangeCombinedMesh(target.member, ccm, doComb, changeVal);
            }
            else if (ce[0] is MaskedTexture)
            {
                secManager.ChangeTextureElement(target.member, ce as MaskedTexture[], changeVal);
            }

            RestartMenu(target);
        }

        void ControlMainAction(EditableElements target)
        {
            SectionDataBase sdb = sdbc.GetChangebleElementWithId(target.member)[target.outIndex].subsection[target.inIndex];
            FlagSectionDataBase flag = StringForEnum.GetEnum<FlagSectionDataBase>(sdb.ToString());

            if (menusAtivos.HasFlag(flag))
            {
                menusAtivos &= ~flag;
            }
            else
            {
                menusAtivos |= flag;
            }

            RestartMenu(target);
        }

        void RestartMenu(EditableElements target)
        {
            activeEditables = ActiveEditables;

            cMenu.FinishHud();
            int indexOfElement = GetElementIndexOf(target);
            cMenu.StartHud(secManager, MainAction, ChangeAction, EscapeAction, activeEditables, selectIndex: indexOfElement);
        }

        // Use this for initialization
        void Start()
        {

            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<MsgStartMusic>.Publish(new MsgStartMusic()
                {
                    clip = menuMusic
                });

                charDbManager.StartFields(confirmation, inputTextManager);
            });

            //menusAtivos &= ~FlagSectionDataBase.pupila;

            if (secManager is null)
                secManager = FindObjectOfType<SectionCustomizationManager>();

            if (sdbc is null)
                sdbc = FindObjectOfType<SectionDataBaseContainer>();

            //string[] StartOptions = GetActiveOption(ActiveEditables);
            activeEditables = ActiveEditables;
            cMenu.StartHud(secManager, MainAction, ChangeAction, EscapeAction, activeEditables);

            SupportSingleton.Instance.InvokeOnEndFrame(() =>// acontecia do evento ser publicado antes da camera estar pronta
            {
                MessageAgregator<MsgChangeMenuDb>.Publish(new MsgChangeMenuDb()
                {
                    sdb = activeEditables[0].mySDB
                });
            });

            MessageAgregator<MsgApplyColor>.AddListener(OnApplyColor);
            MessageAgregator<MsgChangeOptionUI>.AddListener(OnUiChange);
            MessageAgregator<MsgSelectedColorByClick>.AddListener(OnSelectColorByClick);
            MessageAgregator<MsgFinishEdition>.AddListener(OnFinishEdition);
        }

        private void OnApplyColor(MsgApplyColor obj)
        {
            MessageAgregator<MsgSendApplyColorToHud>.Publish(new MsgSendApplyColorToHud()
            {
                cor = obj.c,
                indice = cMenu.SelectedOption
            });
        }

        private void OnFinishEdition(MsgFinishEdition obj)
        {
            gameObject.SetActive(false);
            secManager.gameObject.SetActive(false);
        }

        void OnSelectColorByClick(MsgSelectedColorByClick msg)
        {
            secManager.ApplyColor(msg.C);

            secManager.EndChangeColor(true);

            FinishColorGrid();

            estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgChangeOptionUI>.RemoveListener(OnUiChange);
            MessageAgregator<MsgSelectedColorByClick>.RemoveListener(OnSelectColorByClick);
            MessageAgregator<MsgFinishEdition>.RemoveListener(OnFinishEdition);
            MessageAgregator<MsgApplyColor>.RemoveListener(OnApplyColor);
        }

        bool Jolt
        {
            get
            {

                if (GameJolt.API.GameJoltAPI.Instance != null)
                    if (GameJolt.API.GameJoltAPI.Instance.CurrentUser != null)
                        return true;



                return false;
            }
        }

        void MainStateMod()
        {
            MainStateNew();
            //if (Jolt)
            //{
            //    MainStateNew();
            //}
            //else
            //    MainStateOriginal();
        }

        void MainStateNew()
        {
            int change = -CurrentCommander.GetIntTriggerDown(CommandConverterString.menuChangeV);
            int hChange = CurrentCommander.GetIntTriggerDown(CommandConverterString.menuChangeH);


            cMenu.ChangeOption(change);

            ChangeAction(hChange, cMenu.SelectedOption);

            if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuConfirm))
            {
                MessageAgregator<MsgNegativeUiInput>.Publish();
                MainAction(cMenu.SelectedOption);
            }
            else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuReturn))
            {
                MessageAgregator<MsgNegativeUiInput>.Publish();
                EscapeAction(cMenu.SelectedOption);
            }
            else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuOption1))
            {
                #region saveInTheArrayConfirmation
                estado = EstadoDoMenu.confirmacaoAberta;
                confirmation.StartConfirmationPanel(() =>
                {
                    if (Jolt)
                    {
                        //MyJoltSpace.SaveAndLoadInJolt.SaveMember(secManager.GetCustomDates());
                        //MessageAgregator<MyJoltSpace.MsgFinishTrySaveInJolt>.AddListener(OnFinishTrySave);
                    }
                    else
                    {
                        ToSaveCustomizationContainer.Instance.Save(secManager.GetCustomDates());
                        estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
                    }

                }, () =>
                {
                    estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
                }, TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDoCustomization)[1], hideSelections: true);
                #endregion
            }
            else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuOption2))
            {
                if (Jolt) { }
                //StartCharactersSavedMenu(MyJoltSpace.SaveDatesForJolt.instance.ccds.ccds);
                else
                {
                    ToSaveCustomizationContainer.Instance.Load();
                    StartCharactersSavedMenu(ToSaveCustomizationContainer.Instance.ccds);
                }
                MessageAgregator<MsgEnterInListOptions>.Publish();


            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
#if UNITY_EDITOR
                #region loadOfTheArrrayConfirmation
                estado = EstadoDoMenu.confirmacaoAberta;
                confirmation.StartConfirmationPanel(() =>
                {

                    ToSaveCustomizationContainer.Instance.Load();
                    List<CustomizationContainerDates> lccd = CustomizationSavedChars.listaDePersonagens;

                    if (lccd != null && lccd.Count > 0)
                    {
                        int i = Random.Range(0, lccd.Count);
                        CustomizationContainerDates ccd = lccd[i];
                        if (ccd.PersBase == PersonagemBase.masculino)
                        {
                            ChangeBaseCharacter(true);
                        }
                        else
                            ChangeBaseCharacter(false);

                        secManager.SetCustomDates(ccd);

                    }
                    else
                    {
                        Debug.Log(lccd);
                        Debug.Log(lccd.Count);
                    }

                    secManager.gameObject.SetActive(false);
                    SupportSingleton.Instance.InvokeOnEndFrame(() =>
                    {
                        secManager.gameObject.SetActive(true);
                    });
                    estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
                }, () =>
                {
                    estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
                }, "Deseja tentar carregar um pernogem do vetor de personagens?", hideSelections: true);
                #endregion
#endif
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
#if UNITY_EDITOR
                #region CreateAddList
                if (Jolt) { }
                //StartCharactersSavedMenu(MyJoltSpace.SaveDatesForJolt.instance.ccds.ccds);
                else
                {
                    ToSaveCustomizationContainer.Instance.Load();
                    StartCharactersSavedMenu(SumCharLists.Lccd);
                }
                MessageAgregator<MsgEnterInListOptions>.Publish();
                #endregion
#endif
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
#if UNITY_EDITOR
                #region SaveAddList
                estado = EstadoDoMenu.confirmacaoAberta;
                confirmation.StartConfirmationPanel(() =>
                {
                    SumCharLists.SaveSumChars();
                    estado = EstadoDoMenu.main;
                }, () =>
                {
                    estado = EstadoDoMenu.main;
                }, "<Color=yellow>Atenção</color>\n Você está usando a opção de salvar no arquivo somando com os personagens do arquivo base." +
                "Está certo que quer fazer isso?", hideSelections: true);

                #endregion
#endif
            }

            else if (Input.GetKeyDown(KeyCode.Alpha8))
            {
#if UNITY_EDITOR
                #region salvandoBlocosDeTexto
                estado = EstadoDoMenu.confirmacaoAberta;
                confirmation.StartConfirmationPanel(() =>
                {
                    SumCharLists.SalvarBlocoDeNotas();
                    estado = EstadoDoMenu.main;
                }, () =>
                {
                    estado = EstadoDoMenu.main;
                }, "<Color=yellow>Atenção</color>\n Você está salvando um bloco de notas com os bytes em JSOn dos personagens na área de trabalho." +
                "Está certo que quer fazer isso?", hideSelections: true);

                #endregion
#endif
            }
        }

        //void OnFinishTrySave(MyJoltSpace.MsgFinishTrySaveInJolt j)
        //{
        //    estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
        //    SupportSingleton.Instance.InvokeOnEndFrame(() =>
        //    {
        //        MessageAgregator<MyJoltSpace.MsgFinishTrySaveInJolt>.RemoveListener(OnFinishTrySave);
        //    });
        //}

        void MainStateOriginal()
        {
            int change = -CurrentCommander.GetIntTriggerDown(CommandConverterString.menuChangeV);
            int hChange = CurrentCommander.GetIntTriggerDown(CommandConverterString.menuChangeH);


            cMenu.ChangeOption(change);

            ChangeAction(hChange, cMenu.SelectedOption);

            if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuConfirm))
            {
                MessageAgregator<MsgNegativeUiInput>.Publish();
                MainAction(cMenu.SelectedOption);
            }
            else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuReturn))
            {
                MessageAgregator<MsgNegativeUiInput>.Publish();
                EscapeAction(cMenu.SelectedOption);
            }
            else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuOption1))
            {
                #region SloteSaveCOnfirmation
                estado = EstadoDoMenu.confirmacaoAberta;
                confirmation.StartConfirmationPanel(() =>
                {
                    CustomizationContainerDates ccd = secManager.GetCustomDates();
                    ccd.Save();
                    estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
                }, () =>
                {
                    estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
                }, "Deseja salvar esse personagem no slote de salvamento único?", hideSelections: true);
                #endregion
            }
            else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuOption2))
            {
                #region sloteLoadConfirmation
                estado = EstadoDoMenu.confirmacaoAberta;
                confirmation.StartConfirmationPanel(() =>
                {
                    CustomizationContainerDates ccd = new CustomizationContainerDates();
                    ccd.Load();
                    secManager.SetCustomDates(ccd);
                    estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
                }, () =>
                {
                    estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
                }, "Deseja carregar o personagem do slote de salvamento único?", hideSelections: true);
                #endregion
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                #region combineMeshConfirmation
                estado = EstadoDoMenu.confirmacaoAberta;
                confirmation.StartConfirmationPanel(() =>
                {
                    bool generoMasculino = secManager == secManagerH_Base;
                    testMeshCombiner.StartCombiner(secManager);
                    estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
                }, () =>
                {
                    estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
                }, "Combinar malhas?", hideSelections: true);
                #endregion
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                #region saveInTheArrayConfirmation
                estado = EstadoDoMenu.confirmacaoAberta;
                confirmation.StartConfirmationPanel(() =>
                {
                    ToSaveCustomizationContainer.Instance.Save(secManager.GetCustomDates());
                    estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
                }, () =>
                {
                    estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
                }, "Deseja salvar esse pernogem no vetor de personagens?", hideSelections: true);
                #endregion
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                #region loadOfTheArrrayConfirmation
                estado = EstadoDoMenu.confirmacaoAberta;
                confirmation.StartConfirmationPanel(() =>
                {

                    ToSaveCustomizationContainer.Instance.Load();
                    List<CustomizationContainerDates> lccd = new List<CustomizationContainerDates>();

                    lccd.AddRange(ToSaveCustomizationContainer.Instance.ccds.ToArray());

                    foreach (var v in ListToSaveCustomizationContainer.Instance.dccd)
                    {
                        lccd.AddRange(v.Value.ToArray());
                    }

                    if (lccd != null && lccd.Count > 0)
                    {
                        int i = Random.Range(0, lccd.Count);
                        CustomizationContainerDates ccd = lccd[i];
                        if (ccd.PersBase == PersonagemBase.masculino)
                        {
                            ChangeBaseCharacter(true);
                        }
                        else
                            ChangeBaseCharacter(false);

                        secManager.SetCustomDates(ccd);

                    }
                    else
                    {
                        Debug.Log(lccd);
                        Debug.Log(lccd.Count);
                    }

                    secManager.gameObject.SetActive(false);
                    SupportSingleton.Instance.InvokeOnEndFrame(() =>
                    {
                        secManager.gameObject.SetActive(true);
                    });
                    estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
                }, () =>
                {
                    estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
                }, "Deseja tentar carregar um pernogem do vetor de personagens?", hideSelections: true);
                #endregion
            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                ToSaveCustomizationContainer.Instance.Load();
                StartCharactersSavedMenu(ToSaveCustomizationContainer.Instance.ccds);
            }
        }

        void StartCharactersSavedMenu(List<CustomizationContainerDates> lccd, int indice = 0)
        {
            if (lccd.Count > 0)
            {

                MessageAgregator<MsgRequestUpdateViewChar>.AddListener(OnRequestUpdateViewChar);
                MessageAgregator<MsgFinishCharDbManager>.AddListener(OnFinishCharDbManager);

                cMenu.FinishHud();
                charDbManager.StartCharStandardVector(lccd, indice);
                ChangeBaseCharacter(lccd[indice].PersBase == PersonagemBase.masculino);
                secManager.SetCustomDates(lccd[indice]);

                estado = EstadoDoMenu.characterSaveChanges;
            }
        }

        private void OnFinishCharDbManager(MsgFinishCharDbManager obj)
        {
            cMenu.StartHud(secManager, MainAction, ChangeAction, EscapeAction, activeEditables);

            estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);

            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<MsgRequestUpdateViewChar>.RemoveListener(OnRequestUpdateViewChar);
                MessageAgregator<MsgFinishCharDbManager>.RemoveListener(OnFinishCharDbManager);
            });
        }

        private void OnRequestUpdateViewChar(MsgRequestUpdateViewChar obj)
        {
            ChangeBaseCharacter(obj.ccd.PersBase == PersonagemBase.masculino);

            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                secManager.SetCustomDates(obj.ccd);
            });
        }

        bool ColorState()
        {
            bool foi = false;
            bool effective = false;

            int x = CurrentCommander.GetIntTriggerDown(CommandConverterString.menuChangeH);
            int y = CurrentCommander.GetIntTriggerDown(CommandConverterString.menuChangeV);

            mySuggestionColors.ChangeOption(y, x);

            if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuReturn))
            {
                effective = false;
                foi = true;
            }
            else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuConfirm))
            {
                effective = true;
                foi = true;
            }
            else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuOption1))
            {

                Color C = mySuggestionColors.GetSelectedColor;

                FinishColorGrid();

                ChangeToColorCircle(C);
            }
            else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuOption2) && secManager.GetTargetColorReg != RegistroDeCores.skin)
            {
                globalCM.StartHud(mySuggestionColors.GetSelectedColor, secManager.GuardOriginalColor.cor, secManager.VerifyColorReg(), secManager.GetTargetColorReg);
                StartGlobalColorMenu();
                FinishColorGrid();
            }

            if (foi)
            {
                secManager.EndChangeColor(effective);

                FinishColorGrid();

                estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
            }
            return true;
        }

        private void FinishColorGrid()
        {
            mySuggestionColors.FinishHud();
        }

        private void ChangeToColorCircle(Color C)
        {
            myGetColor.transform.parent.gameObject.SetActive(true);
            estado = EstadoDoMenu.colorCircle;

            EventAgregator.AddListener(EventKey.changeColorPicker, OnChangeColorPicker);

            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                myGetColor.SetColor(C);

                EditableElements target = activeEditables[cMenu.SelectedOption];

                MessageAgregator<MsgOpenColorMenu>.Publish(new MsgOpenColorMenu() { reg = secManager.GetTargetColorReg });

            });
        }

        void StartGlobalColorMenu()
        {

            estado = EstadoDoMenu.globalizationColors;
            MessageAgregator<ChangeInGlobalColorMessage>.AddListener(OnChangeInGlobalMessage);
            MessageAgregator<ButtonMakeGlobalMessage>.AddListener(OnButtonMakeGlobal);
            MessageAgregator<SelectGlobalColorMessage>.AddListener(OnSelectGlobalColor);
            MessageAgregator<ToggleGlobalColorMessage>.AddListener(OnToggleGlobalColor);
        }

        private bool ColorCircleState()
        {
            bool foi = false;
            bool effective = false;

            Vector2 V = new Vector2(
                CurrentCommander.GetAxis(CommandConverterString.menuChangeH),
                CurrentCommander.GetAxis(CommandConverterString.menuChangeV)
                );

            float val = (CurrentCommander.GetButton(CommandConverterInt.menuMoreBlack) ? 1 : 0)
                + (CurrentCommander.GetButton(CommandConverterInt.menuMoreWhite) ? -1 : 0);
            val *= Time.deltaTime * velValForGreyScale;

            myGetColor.MoveMark(V * velValForColor, val);

            if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuReturn))
            {
                effective = false;
                foi = true;
            }
            else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuConfirm))
            {
                effective = true;
                foi = true;
            }
            else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuConfirm))
            {
                EndColorCircle();
                ChangeToColorGrid();

            }
            else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuOption2) && secManager.GetTargetColorReg != RegistroDeCores.skin)
            {
                EndColorCircle();
                globalCM.StartHud(myGetColor.CurrentColor, secManager.GuardOriginalColor.cor, secManager.VerifyColorReg(), secManager.GetTargetColorReg);
                StartGlobalColorMenu();
            }

            if (foi)
            {
                EndColorCircle();
                secManager.EndChangeColor(effective);
                estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
            }



            return true;
        }

        void EndColorCircle()
        {
            myGetColor.transform.parent.gameObject.SetActive(false);
            EventAgregator.RemoveListener(EventKey.changeColorPicker, OnChangeColorPicker);
        }

        private void EndGlobalColorMenu()
        {
            globalCM.FinishHud();

            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<ChangeInGlobalColorMessage>.RemoveListener(OnChangeInGlobalMessage);
                MessageAgregator<ButtonMakeGlobalMessage>.RemoveListener(OnButtonMakeGlobal);
                MessageAgregator<SelectGlobalColorMessage>.RemoveListener(OnSelectGlobalColor);
                MessageAgregator<ToggleGlobalColorMessage>.RemoveListener(OnToggleGlobalColor);
            });
        }

        private bool GlobalizationColorsState()
        {
            globalCM.ChangeOption(
                CurrentCommander.GetIntTriggerDown(CommandConverterString.menuChangeH),
                -CurrentCommander.GetIntTriggerDown(CommandConverterString.menuChangeV)
                );

            if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuReturn))
            {
                secManager.EndChangeColor(false);
                estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
                EndGlobalColorMenu();
            }
            else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuConfirm))
            {
                globalCM.InvokeSelectedAction();
            }
            else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuOption1))
            {
                EndGlobalColorMenu();
                ChangeToColorGrid();

            }
            else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuOption2))
            {

                ChangeToColorCircle(globalCM.RememberedColor);
                EndGlobalColorMenu();
            }
            else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuExtraKey))
            {
                EndGlobalColorMenu();
                estado = EstadoDoMenu.main; cMenu.ChangeInteractiveButtons(true);
            }


            return true;
        }

        void ChangeToColorGrid()
        {
            EditableElements target = activeEditables[cMenu.SelectedOption];
            ColorContainerStruct ccs = secManager.GetColorAssignById(target.member).coresEditaveis[target.inIndex];
            ChangeColorMainAction(ccs);
            estado = EstadoDoMenu.colorGrid;
        }

        private bool GlobalizationMenuState()
        {
            int val = -CurrentCommander.GetIntTriggerDown(CommandConverterString.menuChangeV);
            globalMenu.ChangeOption(val);

            if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuConfirm))
            {
                OpcoesDoGlobalizationMenu(globalMenu.SelectedOption);
            }

            return false;
        }



        // Update is called once per frame
        void Update()
        {

            //if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuConfirm))
            //{
            //    MonoBehaviour[] ms = FindObjectsOfType<MonoBehaviour>();

            //    foreach (var m in ms)
            //        Debug.Log(m.name+" : "+m.GetType());

            //    Debug.Log(ms.Length);
            //}

            switch (estado)
            {
                case EstadoDoMenu.main: MainStateMod(); break;
                case EstadoDoMenu.colorGrid: ColorState(); break;
                case EstadoDoMenu.globalizationColors: GlobalizationColorsState(); break;
                case EstadoDoMenu.colorCircle: ColorCircleState(); break;
                case EstadoDoMenu.globalizationMenu: GlobalizationMenuState(); break;
                case EstadoDoMenu.confirmacaoAberta: ConfirmationOpened(); break;
                case EstadoDoMenu.characterSaveChanges: charDbManager.UpdateDb(); break;
            };



        }


        private bool ConfirmationOpened()
        {
            if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuReturn))
            {
                confirmation.BtnNo();
            }
            else if (CurrentCommander.GetButtonDown(CommandConverterInt.starterMenuConfirm))
            {
                confirmation.BtnYes();
            }

            return false;
        }

        public void DoubleClickCharDb(BaseEventData bed)
        {
            PointerEventData ped = bed as PointerEventData;

            if (ped.clickCount == 2)
            {
                charDbManager.DoubleClickInMenu(/*ped.pointerPress*/);
            }
        }
    }

    [System.Serializable]
    public class CustomizationMenu : InteractiveUiBase
    {
        private SectionCustomizationManager secManager;
        private CustomizationManagerMenu.EditableElements[] opcoes;
        private System.Action<int, int> changeAction;
        private System.Action<int> returnAction;
        private bool estadoDeAcao = false;

        protected System.Action<int> MainAction { get; private set; }

        public void StartHud(
            SectionCustomizationManager secManager,
            System.Action<int> mainAction,
            System.Action<int, int> changeAction,
            System.Action<int> returnAction,
            CustomizationManagerMenu.EditableElements[] dasOpcoes,
            ResizeUiType tipoDeR = ResizeUiType.vertical,
            int selectIndex = 0
            )
        {
            this.secManager = secManager;
            opcoes = dasOpcoes;

            this.returnAction += returnAction;
            this.changeAction += changeAction;

            MainAction += (x) =>
            {
                if (!estadoDeAcao)
                {
                    estadoDeAcao = true;
                    ChangeSelectionTo(x);


                    SupportSingleton.Instance.InvokeInRealTime(() =>
                    {
                        Debug.Log("Função chamada com delay para destaque do botão");
                        mainAction(x);
                        estadoDeAcao = false;
                    }, .05f);
                }
            };
            StartHud(opcoes.Length, tipoDeR, selectIndex);
        }

        public override void SetContainerItem(GameObject G, int indice)
        {
            A_CustomizationOption uma = G.GetComponent<A_CustomizationOption>();
            uma.SetOptions(MainAction, returnAction, changeAction, opcoes[indice], secManager);
        }

        protected override void AfterFinisher()
        {
            MainAction = null;
            changeAction = null;
            returnAction = null;
            //Seria preciso uma finalização especifica??
        }
    }

    public struct MsgSendApplyColorToHud : IMessageBase
    {
        public Color cor;
        public int indice;
    }

    public struct MsgFinishCustomizationMenu : IMessageBase
    {
        public TesteMeshCombiner tCombiner;
        public SectionCustomizationManager secManager;
    }
}