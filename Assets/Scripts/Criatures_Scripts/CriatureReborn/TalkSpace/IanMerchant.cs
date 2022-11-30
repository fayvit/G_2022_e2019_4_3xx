

using Criatures2021Hud;
using FayvitBasicTools;
using FayvitCam;
using FayvitMessageAgregator;
using FayvitSounds;
using TalkSpace;
using TextBankSpace;
using UnityEngine;

namespace Criatures2021
{
    class IanMerchant : MerchantBase
    {
        [SerializeField] private ScheduledTalkManager despedidaDeIan;
        [SerializeField] private NameMusicaComVolumeConfig temaIan = new NameMusicaComVolumeConfig() {
            Musica = NameMusic.choro_N1,
            Volume = 1
        };
        private string[] textosDeIan;
        private float tempoDecorrido = 0;
        private LocalState state = LocalState.emEspera;

        private const float TEMP_COISAS_BOAS = 1;
        private enum LocalState
        {
            emEspera,
            mensInicial,
            escolhaInicial,
            frasePreVenda,
            menuOpened,
            fraseDeBoaCompra,
            fraseInsuficiente,
            particulaDeCoisasBoas,
            fraseFinalDeCompra,
            conversaInterna,
            fraseDeFinalizacao
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            despedidaDeIan.OnVallidate();
        }

        void Start()
        {
            if (StaticInstanceExistence<IGameController>.SchelduleExistence(Start, this, () => { return AbstractGameController.Instance; }))
            {
                KeyVar keyVar = AbstractGameController.Instance.MyKeys;
                

                keyVar.MudaAutoShift(ID, true);

                txtDeOpcoes = TextBank.RetornaListaDeTextoDoIdioma(TextKey.opcoesDeIan).ToArray();
                textosDeIan = TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDeVendaDeIan).ToArray();
                StartBase();
            }

            SempreEstaNoTrigger();
        }

        new void Update()
        {
            base.Update();

            switch (state)
            {
                case LocalState.mensInicial:
                    CameraApplicator.cam.FocusInPoint(8, 2/*, -1, true*/);
                    if (NPCfalasIniciais.Update(commands.confirmButton, commands.returnButton))
                    {
                        EntraFrasePossoAjudar();
                        LigarMenu();
                    }
                break;
                case LocalState.escolhaInicial:
                    EscolhaInicial();
                break;
                case LocalState.frasePreVenda:
                    if (!dispara.LendoMensagemAteOCheia(commands.confirmButton||commands.returnButton))
                    {
                        ContainerBasicMenu.instance.Menu.StartHud(ComprarOuNaoComprar,
                            TextBank.RetornaListaDeTextoDoIdioma(TextKey.simOuNao).ToArray()
                            );
                        state = LocalState.menuOpened;
                        
                    }
                break;
                case LocalState.menuOpened:
                    ContainerBasicMenu.instance.Menu.ThisUpdate(-commands.vChange, commands.confirmButton);

                    if (!commands.confirmButton && commands.returnButton)
                    {
                        ContainerBasicMenu.instance.Menu.FinishHud();
                        VoltarAoJogo();
                    }
                break;
                case LocalState.fraseInsuficiente:
                    if (!dispara.LendoMensagemAteOCheia(commands.confirmButton || commands.returnButton))
                    {
                        if (commands.confirmButton)
                        {
                            dispara.OffPanels();
                            EntraNasEscolhas();
                        }
                    }
                    break;
                case LocalState.fraseDeBoaCompra:
                    if (!dispara.LendoMensagemAteOCheia(commands.confirmButton||commands.returnButton))
                    {
                        if (commands.confirmButton)
                        {
                            InstanceSupport.InstancieEDestrua(GeneralParticles.particulaDaDefesaPergaminhoFora, transform.position, 5);

                            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                            {
                                sfxId = FayvitSounds.SoundEffectID.XP_Heal02
                            });

                            dispara.OffPanels();
                            tempoDecorrido = 0;
                            state = LocalState.particulaDeCoisasBoas;
                        }
                    }
                    break;
                case LocalState.particulaDeCoisasBoas:
                    tempoDecorrido += Time.deltaTime;
                    if (tempoDecorrido > TEMP_COISAS_BOAS)
                    {
                        dispara.StartTextDisplay();
                        dispara.StartShowMessage(textosDeIan[4], fotoDoNPC);
                        state = LocalState.fraseFinalDeCompra;
                        MessageAgregator<MsgShowItem>.Publish(new MsgShowItem()
                        {
                            idItem = !indice1 ? NameIdItem.pergSinara : NameIdItem.pergAlana,
                            quantidade = 1
                        });
                        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                        {
                            sfxId = FayvitSounds.SoundEffectID.VinhetinhaCompletaComFim
                        });
                    }
                    break;
                case LocalState.fraseFinalDeCompra:
                    if (!dispara.LendoMensagemAteOCheia(commands.confirmButton || commands.returnButton))
                    {

                        if (commands.confirmButton)
                        {
                            MessageAgregator<MsgHideShowItem>.Publish();
                            dispara.OffPanels();
                            EntraNasEscolhas();
                        }
                    }
                break;
                case LocalState.conversaInterna:
                    if (NPC.Update(commands.confirmButton, commands.returnButton))
                    {
                        EntraNasEscolhas();
                    }
                break;
                case LocalState.fraseDeFinalizacao:
                    if (despedidaDeIan.Update(commands.confirmButton, commands.returnButton))
                    {
                        VoltarAoJogo();
                    }
                break;
            }
        }

        public override void VoltarAoJogo()
        {
            MessageAgregator<MsgReturnRememberedMusic>.Publish();
            state = LocalState.emEspera;
            base.VoltarAoJogo();
        }

        void ComprarOuNaoComprar(int indice)
        {
            ContainerBasicMenu.instance.Menu.FinishHud();
            
            KeyVar keys = AbstractGameController.Instance.MyKeys;
            DadosDeJogador dados = MyGlobalController.MainPlayer.Dados;

            dispara.OffPanels();


            int val = !indice1
                ? (int)Mathf.Pow(2, keys.VerificaCont(KeyCont.pergSinaraComprados))
                : (int)Mathf.Pow(2, keys.VerificaCont(KeyCont.pergAlanaComprados));

            switch (indice)
            {
                case 0:
                    if (dados.TemItem(NameIdItem.tinteiroSagradoDeLog) >= val && dados.Cristais >= 100 * val)
                    {
                        dados.RemoverCristais(100 * val);
                        MessageAgregator<MsgChangeCristalCount>.Publish(new MsgChangeCristalCount()
                        {
                            newCristalCount = dados.Cristais
                        });
                        ItemBase.RetirarUmItem(dados.Itens, NameIdItem.tinteiroSagradoDeLog, val);
                        dados.AdicionaItem(indice1 ? NameIdItem.pergAlana : NameIdItem.pergSinara);
                        dispara.TurnPanels();
                        dispara.StartShowMessage(textosDeIan[3], fotoDoNPC);
                        state = LocalState.fraseDeBoaCompra;
                        keys.SomaCont(indice1 ? KeyCont.pergAlanaComprados : KeyCont.pergSinaraComprados, 1);
                    }
                    else
                    {
                        dispara.TurnPanels();
                        dispara.StartShowMessage(textosDeIan[2], fotoDoNPC);
                        state = LocalState.fraseInsuficiente;
                    }
                    break;
                case 1:
                    EntraNasEscolhas();
                break;
            }
        }

        void EntraNasEscolhas()
        {
            EntraFrasePossoAjudar();
            LigarMenu();
        }

        void EntraFrasePossoAjudar()
        {
            dispara.TurnPanels();
            dispara.StartShowMessage(textosDeIan[0], fotoDoNPC);
            state = LocalState.escolhaInicial;
        }

        public override void FuncaoDoBotao()
        {
            MessageAgregator<MsgStartMusicWithRecovery>.Publish(new MsgStartMusicWithRecovery()
            {
                nmcvc = temaIan
            });
            state = LocalState.mensInicial;
            BaseStartMerchant();
        }

        protected override void OpcaoEscolhida(int x)
        {

            switch (x)
            {
                case 0:
                case 1:
                    indice1 = x == 0 ? false : true;                    
                    
                    dispara.StartTextDisplay();
                    dispara.StartShowMessage(
                       string.Format(textosDeIan[1], RetornaArgumentosPreVenda()), fotoDoNPC);
                    state = LocalState.frasePreVenda;
                break;
                case 2:
                    state = LocalState.conversaInterna;
                    NPC.Start(gameObject);
                    NPC.IniciaConversa();
                break;
                case 3:
                    PreFinal();
                    break;
            }

            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = SoundEffectID.Book1
            });

            ContainerBasicMenu.instance.Menu.FinishHud();
        }
        void PreFinal()
        {
            state = LocalState.fraseDeFinalizacao;

            despedidaDeIan.Start(gameObject);
            despedidaDeIan.IniciaConversa();
            
        }

        bool indice1;
        string[] RetornaArgumentosPreVenda()
        {
            string[] retorno = new string[3];
            KeyVar keys = AbstractGameController.Instance.MyKeys;
            retorno[0] = !indice1 ? ItemBase.NomeEmLinguas(NameIdItem.pergSinara) : ItemBase.NomeEmLinguas(NameIdItem.pergAlana);
            retorno[1] = !indice1
                ? Mathf.Pow(2, keys.VerificaCont(KeyCont.pergSinaraComprados)).ToString()
                : Mathf.Pow(2, keys.VerificaCont(KeyCont.pergAlanaComprados)).ToString();
            retorno[2] = !indice1
                ? (100 * Mathf.Pow(2, keys.VerificaCont(KeyCont.pergSinaraComprados))).ToString()
                : (100 * Mathf.Pow(2, keys.VerificaCont(KeyCont.pergAlanaComprados))).ToString();
            return retorno;
        }

    }
}
