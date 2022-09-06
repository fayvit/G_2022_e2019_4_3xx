using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Criatures2021;
using TextBankSpace;
using FayvitUI;
using FayvitBasicTools;
using FayvitSupportSingleton;
using FayvitMessageAgregator;

namespace Criatures2021Hud
{
    public class PauseMenuManager : MonoBehaviour
    {
        [SerializeField] private BasicMenu menuzinho;
        [SerializeField] private PauseItemMenu itemMenu;
        [SerializeField] private PausePetMenu petMenu;
        [SerializeField] private Text infoText;
        [SerializeField] private AnItemOption itemRef;
        [SerializeField] private GameObject container;
        [SerializeField] private PauseTabsManager tabsManager;
        [SerializeField] private PetBookHud petBookHud;
        [SerializeField] private TutoInfoMenuManager infoManager;
        

        private int antSelected = 0;
        private LocalState state = LocalState.emEspera;
        private CharacterManager dono;
        private List<PetBase> petList;
        private List<ItemBase> listaDeItens;
        private MsgSendExternalPanelCommand cmd;

        #region testeAntigo
        //private List<ItemBase> listaDeItens = new List<ItemBase>()
        //{
        //    //ItemFactory.Get(NameIdItem.pergaminhoDePerfeicao,14),
        //    ItemFactory.Get(NameIdItem.maca,1),
        //    ItemFactory.Get(NameIdItem.cartaLuva,1),
        //    ItemFactory.Get(NameIdItem.tonico,10),
        //    ItemFactory.Get(NameIdItem.amuletoDaCoragem,10),
        //    ItemFactory.Get(NameIdItem.antidoto,10),
        //    ItemFactory.Get(NameIdItem.aura,10),
        //    ItemFactory.Get(NameIdItem.ventilador,10),
        //    ItemFactory.Get(NameIdItem.cartaLuva,1),
        //    ItemFactory.Get(NameIdItem.tonico,10),
        //    ItemFactory.Get(NameIdItem.amuletoDaCoragem,10),
        //    ItemFactory.Get(NameIdItem.antidoto,10),
        //    ItemFactory.Get(NameIdItem.aura,10),
        //    ItemFactory.Get(NameIdItem.ventilador,10),
        //    ItemFactory.Get(NameIdItem.cartaLuva,1),
        //    ItemFactory.Get(NameIdItem.tonico,10),
        //    ItemFactory.Get(NameIdItem.amuletoDaCoragem,10),
        //    ItemFactory.Get(NameIdItem.antidoto,10),
        //    ItemFactory.Get(NameIdItem.aura,10),
        //    ItemFactory.Get(NameIdItem.ventilador,10),
        //    ItemFactory.Get(NameIdItem.cartaLuva,1),
        //    ItemFactory.Get(NameIdItem.tonico,10),
        //    ItemFactory.Get(NameIdItem.amuletoDaCoragem,10),
        //    ItemFactory.Get(NameIdItem.antidoto,10),
        //    ItemFactory.Get(NameIdItem.aura,10),
        //    ItemFactory.Get(NameIdItem.ventilador,10),
        //    //ItemFactory.Get(NameIdItem.pergVentosCortantes,2),
        //    //ItemFactory.Get(NameIdItem.pergFuracaoDeFolhas,5),
        //    //ItemFactory.Get(NameIdItem.pergaminhoDeFuga,10),
        //    //ItemFactory.Get(NameIdItem.regador,10),
        //    //ItemFactory.Get(NameIdItem.inseticida,2),
        //    //ItemFactory.Get(NameIdItem.ventilador,2),
        //    //ItemFactory.Get(NameIdItem.pergSinara,2),
        //    //ItemFactory.Get(NameIdItem.pergAlana,1)
        //};

        //private List<PetBase> petList = new List<PetBase>()
        //{
        //    new PetBase(PetName.Xuash,11),
        //    new PetBase(PetName.Escorpion,10),
        //    new PetBase(PetName.Serpente,7),
        //    new PetBase(PetName.PolyCharm,12),
        //    new PetBase(PetName.Flam,9),
        //    new PetBase(PetName.Estrep,11),
        //    new PetBase(PetName.Nessei,8),
        //};
        #endregion

        private enum LocalState
        { 
            emEspera,
            mudandoItens,
            mudandoPets,
            menuzinhoItens,
            escolhendoEmQuemUsar,
            oneMessageOpened,
            menuzinhoPets,
            mudandoLivro,
            mudandoInfos
        }

        // Start is called before the first frame update
        void Start()
        {
            MessageAgregator<MsgRequestPauseMenu>.AddListener(OnRequestPauseMenu);
            MessageAgregator<MsgUsingQuantitativeItem>.AddListener(OnUseQuantitativeItem);
            MessageAgregator<MsgNotUseItem>.AddListener(OnNegateUseItem);
            MessageAgregator<MsgActionTabMenu>.AddListener(OnActionTab);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgRequestPauseMenu>.RemoveListener(OnRequestPauseMenu);
            MessageAgregator<MsgUsingQuantitativeItem>.RemoveListener(OnUseQuantitativeItem);
            MessageAgregator<MsgNotUseItem>.RemoveListener(OnNegateUseItem);
            MessageAgregator<MsgActionTabMenu>.RemoveListener(OnActionTab);

        }

        #region ActionTabs
        private void FinalizarAbas()
        {
            petBookHud.FinishHud();
            menuzinho.FinishHud();
            infoManager.FinishHud();
            itemMenu.GetTransformContainer.parent.gameObject.SetActive(false);
        }

        private void OnActionTab(MsgActionTabMenu obj)
        {
            FinalizarAbas();

            switch (obj.index)
            {
                case 0:
                    
                    itemMenu.GetTransformContainer.parent.gameObject.SetActive(true);
                    //RetornaParaMudandoItens();
                    itemMenu.FinishHud();
                    petMenu.FinishHud();

                    MessageAgregator<MsgChangeOptionUI>.RemoveListener(OnChangeUI);
                    MessageAgregator<MsgSendExternalPanelCommand>.RemoveListener(OnReceiveExternalCommand);

                    OnRequestPauseMenu(new MsgRequestPauseMenu()
                    {
                        dono = dono
                    });
                    
                break;
                case 1:
                    
                    state = LocalState.mudandoLivro;
                    petBookHud.StartHud(dono.Dados.Livro);
                break;
                case 2:
                    state = LocalState.mudandoInfos;
                    infoManager.StartHud();
                break;
            }
        }
        #endregion

        #region useItem

        private void OnNegateUseItem(MsgNotUseItem obj)
        {
            AbstractGlobalController.Instance.OneMessage.StartMessagePanel(() =>
            {
                listaDeItens[itemMenu.SelectedOption].IniciaUsoDeMenu(dono.gameObject, listaDeItens);
                state = LocalState.escolhendoEmQuemUsar;
            },obj.notMessage);

            state = LocalState.oneMessageOpened;
        }

        void UsouQuantitativeItem(bool temMaisPraUsar)
        {
            
            state = LocalState.emEspera;

            SupportSingleton.Instance.InvokeInSeconds(() =>
            {
                if (temMaisPraUsar)
                {
                    listaDeItens[itemMenu.SelectedOption].IniciaUsoDeMenu(dono.gameObject, listaDeItens);
                    state = LocalState.escolhendoEmQuemUsar;
                    //MessageAgregator<>
                }
                else
                {
                    RetornarDoEscolhaEmQuemUsar();
                }
            }, .5f);
        }

        private void OnUseQuantitativeItem(MsgUsingQuantitativeItem obj)
        {
            TuinManager.RequestDecreaseTuin();
            if (obj.confirmarRetorno)
            {
                state = LocalState.oneMessageOpened;
                AbstractGlobalController.Instance.OneMessage.StartMessagePanel(() =>
                {
                    UsouQuantitativeItem(obj.temMaisParausar);
                },obj.mensagemDeRetorno);
            }
            else
            {
                UsouQuantitativeItem(obj.temMaisParausar);
            }
            
        }

        void PosicionarItemRef(int indice)
        {
            A_PetMenuOption opts = petMenu.GetTransformContainer.GetComponentsInChildren<A_PetMenuOption>()[indice];
            RectTransform rt = opts.GetComponent<RectTransform>();
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);


            itemRef.GetComponent<PositionLerp>().StartRePos(opts.transform, -0.63f * (corners[2] - corners[1]), .5f);
        }

        private void ItemMenuAction(int x)
        {
            AnItemOption opts = itemMenu.GetTransformContainer.GetComponentsInChildren<AnItemOption>()[x];
            itemMenu.ChangeSelectionTo(x);
            itemMenu.ChangeInteractiveButtons(false);
            petMenu.ChangeInteractiveButtons(false);
            menuzinho.GetTransformContainer.position = opts.transform.position;
            menuzinho.StartHud(MenuzinhoItemAction,
                TextBank.RetornaListaDeTextoDoIdioma(TextKey.menuUsoDeItem).ToArray()
                );
            state = LocalState.menuzinhoItens;
        }

        void OnStartUseItemResponse(MsgMenuStartUseItem obj)
        {
            switch (obj.response)
            {
                case MsgMenuStartUseItem.StartUseResponse.escolhaEmQuemUsar:
                    itemRef.gameObject.SetActive(true);
                    ItemBase iRef = listaDeItens[itemMenu.SelectedOption];
                    itemRef.SetarOpcoes(iRef, null);
                    state = LocalState.escolhendoEmQuemUsar;
                    petMenu.FinishHud();
                    petMenu.StatHud(EscolhiEmQuemUsarItem, petList, petMenu.SelectedOption);
                    SupportSingleton.Instance.InvokeOnEndFrame(() =>
                    {
                        PosicionarItemRef(petMenu.SelectedOption);
                    });
                    break;
                case MsgMenuStartUseItem.StartUseResponse.naoPodeUsar:
                    AbstractGlobalController.Instance.OneMessage.StartMessagePanel(() =>
                    {
                        RetornaParaMudandoItens();
                    },
                    TextBank.RetornaListaDeTextoDoIdioma(TextKey.itens)[10]);

                    state = LocalState.oneMessageOpened;
                    break;
                case MsgMenuStartUseItem.StartUseResponse.usar:

                    break;
            }

            SupportSingleton.Instance.InvokeOnCountFrame(() =>
            {
                MessageAgregator<MsgMenuStartUseItem>.RemoveListener(OnStartUseItemResponse);
            });
        }

        private void MenuzinhoItemAction(int x)
        {
            switch (x)
            {
                case 0:
                    state = LocalState.emEspera;
                    MessageAgregator<MsgMenuStartUseItem>.AddListener(OnStartUseItemResponse);
                    SupportSingleton.Instance.InvokeOnEndFrame(() =>
                    {
                        listaDeItens[itemMenu.SelectedOption].IniciaUsoDeMenu(dono.gameObject, listaDeItens);
                    });
                    break;
                case 1:
                    listaDeItens[itemMenu.SelectedOption].NosItensRapidos = !listaDeItens[itemMenu.SelectedOption].NosItensRapidos;
                    string message = listaDeItens[itemMenu.SelectedOption].NosItensRapidos ?
                        TextBank.RetornaFraseDoIdioma(TextKey.mudouItemRapido) :
                        TextBank.RetornaListaDeTextoDoIdioma(TextKey.mudouItemRapido)[1];
                    message = string.Format(message, ItemBase.NomeEmLinguas(listaDeItens[itemMenu.SelectedOption].ID));
                    AbstractGlobalController.Instance.OneMessage.StartMessagePanel(() => {
                        RetornaParaMudandoItens();
                        ReinicieMenusConsiderandoSelecionado();
                    }, message);
                    state = LocalState.oneMessageOpened;
                    break;
                case 2:
                    MessageAgregator<MsgRequestOpenDiscartHud>.Publish(new MsgRequestOpenDiscartHud()
                    {
                        descartavel = listaDeItens[itemMenu.SelectedOption]
                    });
                    MessageAgregator<MsgFinishDiscartHud>.AddListener(OnFinishDiscartMenu);

                    state = LocalState.emEspera;
                    break;
                case 3:
                    RetornaParaMudandoItens();
                    break;
            }

            menuzinho.FinishHud();
        }

        void ReinicieMenusConsiderandoSelecionado()
        {
            int selecionado = itemMenu.SelectedOption;
            itemMenu.FinishHud();
            if (listaDeItens.Count >= selecionado)
                itemMenu.StartHud(ItemMenuAction, listaDeItens.ToArray(), selecionado);
            else
                itemMenu.StartHud(ItemMenuAction, listaDeItens.ToArray());
        }

        private void OnFinishDiscartMenu(MsgFinishDiscartHud obj)
        {
            if (obj.discartResult)
            {
                SupportSingleton.Instance.InvokeOnEndFrame(() =>
                {
                    ItemBase.RetirarUmItem(listaDeItens, listaDeItens[itemMenu.SelectedOption].ID, obj.amount);
                    RetornaParaMudandoItens();
                    ReinicieMenusConsiderandoSelecionado();
                });

                menuzinho.FinishHud();
            }
            else
            {
                RetornaParaMudandoItens();
            }

            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<MsgFinishDiscartHud>.RemoveListener(OnFinishDiscartMenu);
            });
        }

        void RetornaParaMudandoItens()
        {
            state = LocalState.mudandoItens;
            itemMenu.ChangeInteractiveButtons(true);
            petMenu.ChangeInteractiveButtons(true);
        }

        void RetornarDoEscolhaEmQuemUsar()
        {
            petMenu.RemoveHighlights();
            itemRef.gameObject.SetActive(false);
            ReinicieMenusConsiderandoSelecionado();
            RetornaParaMudandoItens();
        }

        void EscolhiEmQuemUsarItem(int quem)
        {
            state = LocalState.emEspera;
            MessageAgregator<MsgChoseItemTarget>.Publish(new MsgChoseItemTarget()
            {
                indexOfTarget = quem
            });

        }
        #endregion

        #region ManagerMainMenu
        void FinishMenu()
        {
            MessageAgregator<MsgChangeOptionUI>.RemoveListener(OnChangeUI);
            MessageAgregator<MsgSendExternalPanelCommand>.RemoveListener(OnReceiveExternalCommand);
            container.SetActive(false);
            tabsManager.FinishTabMenu();

            MessageAgregator<MsgFinishPauseMenu>.Publish(new MsgFinishPauseMenu()
            {
                dono = dono
            });

            cmd = new MsgSendExternalPanelCommand();
            petMenu.FinishHud();
            itemMenu.FinishHud();
            menuzinho.FinishHud();
            itemRef.gameObject.SetActive(false);
            state = LocalState.emEspera;
        }

        private void OnReceiveExternalCommand(MsgSendExternalPanelCommand obj)
        {
            cmd = obj;
        }

        private void OnRequestPauseMenu(MsgRequestPauseMenu obj)
        {
            tabsManager.ChangeToDefaulState();
            FinalizarAbas();
            itemMenu.GetTransformContainer.parent.gameObject.SetActive(true);
            
            container.SetActive(true);
            dono = obj.dono;
            listaDeItens = dono.Dados.Itens;
            petList = dono.Dados.CriaturesAtivos;
            cmd = new MsgSendExternalPanelCommand();
            MessageAgregator<MsgChangeOptionUI>.AddListener(OnChangeUI);
            MessageAgregator<MsgSendExternalPanelCommand>.AddListener(OnReceiveExternalCommand);

            petMenu.StatHud(PetsMenuAction, petList);
            itemMenu.StartHud(ItemMenuAction, listaDeItens.ToArray());
            if (listaDeItens.Count > 0)
            {
                RetornaParaMudandoItens();
                petMenu.RemoveHighlights();
            }
            else
            {
                state = LocalState.mudandoPets;
                infoText.text = "";
            }

            itemMenu.GetTransformContainer.gameObject.SetActive(true);
        }

        private void OnChangeUI(MsgChangeOptionUI obj)
        {
            if (obj.parentOfScrollRect == itemMenu.GetTransformContainer.gameObject)
            {
                int columns = itemMenu.ColCellCount();
                //Debug.Log(antSelected + " : " + itemMenu.SelectedOption + " : " + columns);

                if ((antSelected % columns == columns - 1 && itemMenu.SelectedOption % columns == 0)
                    ||
                    antSelected==listaDeItens.Count-1 && itemMenu.SelectedOption%listaDeItens.Count==0)
                {
                    itemMenu.ChangeSelectionTo(antSelected);
                    itemMenu.RemoveHighlights();
                    petMenu.ChangeSelectionTo(petMenu.SelectedOption);
                    state = LocalState.mudandoPets;
                    infoText.text = "";
                }
                else
                {
                    infoText.text = "<color=yellow>" + ItemBase.NomeEmLinguas(listaDeItens[obj.selectedOption].ID) + "</color>\r\n" +
                    TextBank.RetornaListaDeTextoDoIdioma(TextKey.shopInfoItem)[(int)listaDeItens[obj.selectedOption].ID];
                }
            }
            else if (obj.parentOfScrollRect == petMenu.GetTransformContainer.gameObject)
            {
                if (state == LocalState.escolhendoEmQuemUsar)
                {
                    PosicionarItemRef(petMenu.SelectedOption);
                }
            }
        }
        #endregion

        #region petMenu
        private void PetsMenuAction(int selectedOption)
        {
            A_PetMenuOption opts = petMenu.GetTransformContainer.GetComponentsInChildren<A_PetMenuOption>()[selectedOption];
            //itemMenu.ChangeSelectionTo();
            itemMenu.ChangeInteractiveButtons(false);
            petMenu.ChangeInteractiveButtons(false);
            menuzinho.GetTransformContainer.position = opts.transform.position;
            List<string> l = new List<string>(TextBank.RetornaListaDeTextoDoIdioma(TextKey.menuPets));
            l.Reverse();
            menuzinho.StartHud(MenuzinhoPetsAction, l.ToArray());
            state = LocalState.menuzinhoPets;
        }

        private void MenuzinhoPetsAction(int obj)
        {
            switch (obj)
            {
                case 0:
                    tabsManager.ReestartTabsManager(GetPetsSprites(),SelectPetTabAction,petMenu.SelectedOption);
                    state = LocalState.emEspera;
                    MessageAgregator<MsgRequestStatsMenu>.Publish(new MsgRequestStatsMenu()
                    {
                        indiceSelecionado = petMenu.SelectedOption,
                        petList = petList
                    });
                    MessageAgregator<MsgSendExternalPanelCommand>.RemoveListener(OnReceiveExternalCommand);
                    MessageAgregator<MsgFinishStatsMenu>.AddListener(OnFinishStatsMenu);
                break;
                case 1://voltar
                    RetornaParaMudandoPets();
                break;
            }

            menuzinho.FinishHud();
        }

        private void OnFinishStatsMenu(MsgFinishStatsMenu obj)
        {
            cmd = new MsgSendExternalPanelCommand();
            RetornaParaMudandoPets();
            MessageAgregator<MsgSendExternalPanelCommand>.AddListener(OnReceiveExternalCommand);
            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<MsgFinishStatsMenu>.RemoveListener(OnFinishStatsMenu);
            });
        }

        private void RetornaParaMudandoPets()
        {
            state = LocalState.mudandoPets;
            itemMenu.ChangeInteractiveButtons(true);
            petMenu.ChangeInteractiveButtons(true);
        }

        private void SelectPetTabAction(int x)
        {
            MessageAgregator<MsgRequestStatsMenu>.Publish(new MsgRequestStatsMenu()
            {
                indiceSelecionado = x,
                petList = petList
            });
        }

        Sprite[] GetPetsSprites()
        {
            Sprite[] s = new Sprite[petList.Count];

            for (int i = 0; i < s.Length; i++)
                s[i] = ResourcesFolders.GetMiniPet(petList[i].NomeID);

            return s;
        }

        #endregion


        // Update is called once per frame
        void Update()
        {
            if (cmd.pauseMenu && state!=LocalState.oneMessageOpened)
            {
                FinishMenu();
            }else
                switch (state)
                {
                    case LocalState.mudandoItens:
                        #region mudandoItens
                        antSelected = itemMenu.SelectedOption;
                        itemMenu.ChangeOption(cmd.vChange, cmd.hChange);

                        if (cmd.confirmButton)
                        {
                            #region tentativa
                            //AnItemOption opts = itemMenu.GetTransformContainer.GetComponentsInChildren<AnItemOption>()[itemMenu.SelectedOption];
                            //RectTransform rt = menuzinho.GetTransformContainer.GetComponent<RectTransform>();
                            //RectTransform rtt = opts.GetComponent<RectTransform>();
                            //Vector4 v = FayvitUiUtility.GetMinMaxPositionInTheCanvas(rtt);
                            ////float xStart = rtt.anchorMin.x;
                            ////float yEnd = 1 - rtt.anchorMin.y;
                            ////float xEnd = rtt.anchorMax.x;
                            ////float yStart = 1-rtt.anchorMax.y;
                            //Debug.Log(v);
                            //rt.anchorMin = new Vector2(v.x, v.y);
                            //rt.anchorMax = new Vector2(v.z, v.w);//0.5f * Vector2.one;
                            //rt.anchoredPosition = Vector2.zero;
                            //rt.offsetMin = Vector2.zero;//new Vector2(xStart * rtt.rect.width, yStart * rtt.rect.height);
                            //rt.offsetMax = Vector2.zero;
                            #endregion
                            ItemMenuAction(itemMenu.SelectedOption);
                        }
                        else
                            tabsManager.VerifyChangeTab(cmd.rightChangeButton, cmd.leftChangeButton);
                        #endregion
                        break;
                    case LocalState.mudandoPets:
                        #region mudandoPets
                        petMenu.ChangeOption(-cmd.vChange);
                        if (cmd.hChange < 0 && listaDeItens.Count>0)
                        {
                            state = LocalState.mudandoItens;
                            petMenu.RemoveHighlights();
                            itemMenu.ChangeSelectionTo(antSelected);
                        }else if (cmd.confirmButton)
                        {
                            PetsMenuAction(petMenu.SelectedOption);
                        }else 
                            tabsManager.VerifyChangeTab(cmd.rightChangeButton, cmd.leftChangeButton);
                        #endregion
                        break;
                    case LocalState.menuzinhoItens:
                        #region menuzinhoItens
                        menuzinho.ChangeOption(-cmd.vChange);
                        if (cmd.returnButton)
                        {
                            menuzinho.FinishHud();
                            RetornaParaMudandoItens();
                        }
                        else if (cmd.confirmButton)
                        {
                            MenuzinhoItemAction(menuzinho.SelectedOption);
                        }
                        #endregion
                        break;
                    case LocalState.menuzinhoPets:
                        #region menuzinhoPets
                        menuzinho.ChangeOption(-cmd.vChange);
                        if (cmd.returnButton)
                        {
                            menuzinho.FinishHud();
                            RetornaParaMudandoItens();
                        }
                        else if (cmd.confirmButton)
                        {
                            MenuzinhoPetsAction(menuzinho.SelectedOption);
                        }
                        #endregion
                        break;
                    case LocalState.escolhendoEmQuemUsar:
                        #region EscolhendoEmQuemUsar
                        petMenu.ChangeOption(-cmd.vChange);
                        if (cmd.confirmButton)
                        {
                            EscolhiEmQuemUsarItem(petMenu.SelectedOption);
                        }
                        else if (cmd.returnButton)
                        {
                            listaDeItens[itemMenu.SelectedOption].RetornaSemUsarComMenu();
                            RetornarDoEscolhaEmQuemUsar();
                        }
                        #endregion
                        break;
                    case LocalState.mudandoLivro:
                        petBookHud.Update(-cmd.vChange, false, false);
                        tabsManager.VerifyChangeTab(cmd.rightChangeButton, cmd.leftChangeButton);
                    break;
                    case LocalState.mudandoInfos:
                        infoManager.Update(-cmd.vChange, false);
                        tabsManager.VerifyChangeTab(cmd.rightChangeButton, cmd.leftChangeButton);
                    break;
                    case LocalState.oneMessageOpened:

                        if (cmd.confirmButton || cmd.returnButton)
                            AbstractGlobalController.Instance.OneMessage.ThisUpdate(true);

                    break;
                }
        }
    }

    public struct MsgUsingQuantitativeItem : IMessageBase
    {
        public bool temMaisParausar;
        public bool confirmarRetorno;
        public string mensagemDeRetorno;
    }

    public struct MsgNotUseItem : IMessageBase
    {
        public string notMessage;
    }
}