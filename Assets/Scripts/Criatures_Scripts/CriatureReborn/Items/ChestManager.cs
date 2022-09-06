using UnityEngine;
using System.Collections;
using TextBankSpace;
using FayvitBasicTools;
using Criatures2021Hud;
using FayvitMessageAgregator;
using FayvitSupportSingleton;

namespace Criatures2021
{ 
    public class ChestManager : ButtonActivate
    {
        [SerializeField] private string ID = "";
        [SerializeField] private ChestItem[] itemDoBau;
        [SerializeField] private Animator A;

        private int indiceDoItem = 0;
        private string[] textos = TextBank.RetornaListaDeTextoDoIdioma(TextKey.bau).ToArray();
        private FasesDoBau fase = FasesDoBau.emEspera;
        private MsgSendExternalPanelCommand externalCommand = new MsgSendExternalPanelCommand();

        private enum FasesDoBau
        {
            emEspera,
            abrindo,
            lendoOpcoes,
            aberto,
            fechando,
            nula
        }



        // Use this for initialization
        void Start()
        {
            if (StaticInstanceExistence<IGameController>.SchelduleExistence(Start, this, () => {
                return AbstractGameController.Instance;
                }))
            {
                if (AbstractGameController.Instance.MyKeys.VerificaAutoShift(ID))
                    A.Play("bauAberto");

                textoDoBotao = TextBank.RetornaListaDeTextoDoIdioma(TextKey.textoBaseDeAcao)[1];
                SempreEstaNoTrigger();
            }

            MessageAgregator<MsgSendExternalPanelCommand>.AddListener(OnReceiveCommands);
            MessageAgregator<MsgAnimationPointCheck>.AddListener(OnReceiveAnimationPointCheck);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgSendExternalPanelCommand>.RemoveListener(OnReceiveCommands);
            MessageAgregator<MsgAnimationPointCheck>.RemoveListener(OnReceiveAnimationPointCheck);
        }

        private void OnReceiveAnimationPointCheck(MsgAnimationPointCheck obj)
        {
            if (obj.sender == gameObject)
            {
                if (obj.extraInfo == "abrindoBau" && fase==FasesDoBau.abrindo)
                {
                    externalCommand = new MsgSendExternalPanelCommand();
                    VerificaItem();
                    fase = FasesDoBau.aberto;
                    A.Play("bauAberto");

                }
            }
        }

        private void OnReceiveCommands(MsgSendExternalPanelCommand obj)
        {
            if (fase != FasesDoBau.emEspera)
            {
                externalCommand = obj;
            }
        }

        void AcaoDeOpcaoLida()
        {
            QualOpcao(YesOrNoMenu.instance.Menu.SelectedOption);
        }

        void BauAberto()
        {
            if (indiceDoItem + 1 > itemDoBau.Length || AbstractGameController.Instance.MyKeys.VerificaAutoShift(ID))
            {
                MessageAgregator<MsgRequestHideUpperLargeMessage>.Publish();
                MessageAgregator<MsgHideShowItem>.Publish();
                //GameController.g.HudM.Painel.EsconderMensagem();
                //GameController.g.HudM.MostrarItem.DesligarPainel();

                indiceDoItem = 0;
                //fase = FasesDoBau.fechando;

                AbstractGameController.Instance.MyKeys.MudaAutoShift(ID, true);
                //tampa.rotation = Quaternion.LookRotation(transform.forward);
                FinalizarAcaoDeBau();
            }
            else
            {
                VerificaItem();
            }
        }

        private void OnValidate()
        {
            BuscadorDeID.Validate(ref ID, this);
        }

        // Update is called once per frame
        new void Update()
        {

            base.Update();
            switch (fase)
            {
                case FasesDoBau.lendoOpcoes:

                    YesOrNoMenu.instance.Menu.ChangeOption(externalCommand.vChange);

                    if (externalCommand.confirmButton)
                        QualOpcao(YesOrNoMenu.instance.Menu.SelectedOption);

                    // ação de opção lida
                break;
                case FasesDoBau.abrindo:

                    //if (Vector3.Angle(tampa.forward, transform.forward) < 70)
                    //    tampa.Rotate(tampa.right, -75 * Time.deltaTime, Space.World); //(dobradica.position, dobradica.up, 75 * Time.deltaTime);
                    //else
                    //{
                    //    fase = FasesDoBau.aberto;

                    //    //ActionManager.ModificarAcao(GameController.g.transform, BauAberto);

                    //    VerificaItem();

                    //    if (externalCommand.confirmButton || externalCommand.returnButton)
                    //        BauAberto();
                    //}
                break;
                case FasesDoBau.aberto:

                    if (externalCommand.confirmButton || externalCommand.returnButton)
                        BauAberto();
                break;
                case FasesDoBau.fechando:
                    // if (Vector3.Angle(tampa.forward, transform.forward) > 5)
                    //    tampa.Rotate(tampa.right, 75 * Time.deltaTime, Space.World);
                    //else
                    {
                        AbstractGameController.Instance.MyKeys.MudaAutoShift(ID, true);
                        //tampa.rotation = Quaternion.LookRotation(transform.forward);
                        FinalizarAcaoDeBau();
                    }
                break;
            }


        }

        void VerificaItem()
        {
            if (AbstractGameController.Instance.MyKeys.VerificaAutoShift(ID))
            {
                MessageAgregator<MsgRequestUpperLargeMessage>.Publish(new MsgRequestUpperLargeMessage()
                {
                    message = textos[1]
                });
                //GameController.g.HudM.Painel.AtivarNovaMens(textos[1], 25);
            }
            else
            {
                ChestItem ii = itemDoBau[indiceDoItem];
                MessageAgregator<MsgGetChestItem>.Publish(new MsgGetChestItem()
                {
                    message = string.Format(textos[2], ii.Quantidade, ItemBase.NomeEmLinguas(ii.Item)),
                    nameItem = ii.Item,
                    quantidade = ii.Quantidade
                });

                indiceDoItem++;

                //MessageAgregator<MsgRequestUpperLargeMessage>.Publish(new MsgRequestUpperLargeMessage()
                //{
                //    message = string.Format(textos[2], ii.Quantidade, ItemBase.NomeEmLinguas(ii.Item))
                //});


                //GameController.g.HudM.Painel.AtivarNovaMens(string.Format(textos[2], ii.Quantidade, ItemBase.NomeEmLinguas(ii.Item)), 25);
                //GameController.g.HudM.MostrarItem.IniciarPainel(ii.Item, ii.Quantidade);
                //GameController.g.Manager.Dados.AdicionaItem(ii.Item, ii.Quantidade);



                //MessageAgregator<MsgShowItem>.Publish(new MsgShowItem()
                //{
                //    idItem = ii.Item,
                //    quantidade = ii.Quantidade
                //});
            }
        }

        void QualOpcao(int qual)
        {
            switch (qual)
            {
                case 0://sim
                    fase = FasesDoBau.abrindo;
                    A.Play("abrindoBau");
                    MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                    {
                        sfxId = FayvitSounds.SoundEffectID.paraBau
                    });
                break;
                case 1://nao

                    FinalizarAcaoDeBau();
                break;
            }

            YesOrNoMenu.instance.Menu.FinishHud();
            //GameController.g.HudM.Menu_Basico.FinalizarHud();

            MessageAgregator<MsgRequestHideUpperLargeMessage>.Publish();
            //GameController.g.HudM.Painel.EsconderMensagem();
        }

        void FinalizarAcaoDeBau()
        {
            fase = FasesDoBau.emEspera;

            //GameController.g.Manager.AoHeroi();
            MessageAgregator<MsgFinishExternalInteraction>.Publish();


            Debug.Log("Uma chamada de save[em observação]");
            FayvitSave.SaveDatesManager.SalvarAtualizandoDados(new Criatures2021.CriaturesSaveDates());
            //GameController.g.Salvador.SalvarAgora();
        }

        public override void FuncaoDoBotao()
        {
            SomDoIniciar();
            FluxoDeBotao();

            //commandR = GameController.g.CommandR;
            //ActionManager.Instance.ModificarAcao(this, AcaoDeOpcaoLida);

            if (AbstractGameController.Instance.MyKeys.VerificaAutoShift(ID))
            {
                fase = FasesDoBau.aberto;
                VerificaItem();
                externalCommand = new MsgSendExternalPanelCommand(); 
                
            }
            else
            {
                fase = FasesDoBau.nula;

                SupportSingleton.Instance.InvokeOnCountFrame(() =>
                {
                    fase = FasesDoBau.lendoOpcoes;
                }, 2);

                MessageAgregator<MsgRequestUpperLargeMessage>.Publish(new MsgRequestUpperLargeMessage() { message = textos[0] });
                
                //GameController.g.HudM.Painel.AtivarNovaMens(textos[0], 25);

                YesOrNoMenu.instance.Menu.StartHud(QualOpcao,
                    TextBank.RetornaListaDeTextoDoIdioma(TextKey.simOuNao).ToArray()
                );
            }

            MessageAgregator<MsgStartExternalInteraction>.Publish();
        }


    }

    [System.Serializable]
    public class ChestItem
    {
        [SerializeField] private NameIdItem item;
        [SerializeField] private int quantidade;

        public NameIdItem Item
        {
            get { return item; }
            set { item = value; }
        }

        public int Quantidade
        {
            get { return quantidade; }
            set { quantidade = value; }
        }
    }

    public struct MsgGetChestItem : IMessageBase
    {
        public NameIdItem nameItem;
        public int quantidade;
        public string message;
    }
}
