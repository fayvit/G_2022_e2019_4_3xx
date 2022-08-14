using FayvitMessageAgregator;
using System.Collections.Generic;
using UnityEngine;
using Criatures2021Hud;
using TextBankSpace;
using FayvitBasicTools;
using FayvitSupportSingleton;

namespace Criatures2021
{
    [System.Serializable]
    public class ModifyPetItemBase : ConsumableItemBase
    {
        private int opcaoEscolhida = -1;
        //    private FluxoDeRetorno fluxo;
        private string[] textoDaMensInicial;//ChaveDeTexto chave = ChaveDeTexto.usarPergaminhoDeLaurense;
        private GeneralParticles particula = GeneralParticles.particulaDoAtaquePergaminhoFora;
        [System.NonSerialized]private MsgSendExternalPanelCommand commands;
        private const float TEMPO_DE_ANIMA_PARTICULAS = 1;

        protected MsgSendExternalPanelCommand Commands => commands;
        public string[] TextoDaMensagemInicial
        {
            get { return textoDaMensInicial; }
            protected set { textoDaMensInicial = value; }
        }

        public GeneralParticles Particula
        {
            get { return particula; }
            protected set { particula = value; }
        }

        public ModifyPetItemBase(ItemFeatures cont) : base(cont) { }

        protected override void EscolhiEmQuemUsar(int indice)
        {

            PetAtributes A = Dono.GetComponent<CharacterManager>().Dados.CriaturesAtivos[indice].PetFeat.meusAtributos;

            EscolhiEmQuemUsar(indice, A.PV.Corrente > 0, true);
        }

        public override void AcaoDoItemConsumivel(int indice)
        {
            AplicaEfeito(indice);
            //if (!GameController.g.estaEmLuta)
            //    GameController.g.Salvador.SalvarAgora();
        }

        public void InicioComum()
        {
            MessageAgregator<MsgStartUseItem>.Publish(new MsgStartUseItem
            {
                usuario = Dono
            });
            MessageAgregator<MsgRequestUpperLargeMessage>.Publish(new MsgRequestUpperLargeMessage()
            {
                message = TextoDaMensagemInicial[0]
            });
            MessageAgregator<MsgStartExternalInteraction>.Publish();
            MessageAgregator<MsgSendExternalPanelCommand>.AddListener(OnReceiveCommands);
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.painelAbrindo
            });
            commands = new MsgSendExternalPanelCommand();
            ContainerBasicMenu.instance.SetPercentSizeInTheParent(.6f,.4f,.95f,.95f);
            ContainerBasicMenu.instance.Menu.StartHud(OpcaoEscolhida, NomesDosCriaturesAtivos());
            //GameController.g.HudM.Painel.AtivarNovaMens(textoDaMensInicial[0], 25);
            //GameController.g.HudM.Menu_Basico.IniciarHud(OpcaoEscolhida, NomesDosCriaturesAtivos());
            Estado = ItemUseState.selecaoDeItem;

        }

        

        private void OnReceiveCommands(MsgSendExternalPanelCommand obj)
        {
            commands = obj;
        }

        public override void IniciaUsoDeHeroi(GameObject dono, List<ItemBase> lista)
        {
            this.Dono = dono;
            this.Lista = lista;
            InicioComum();
            eMenu = false;
        }

        //public override void IniciaUsoDeHeroi(GameObject dono)
        //{
        //    GameController.g.Manager.Estado = EstadoDePersonagem.parado;
        //    InicioComum();
        //}

        public override bool AtualizaUsoComCriature()
        {
            return AtualizaUsoDoPergaminho();
        }

        public override bool AtualizaUsoDeHeroi()
        {
            return AtualizaUsoDoPergaminho();
        }

        protected virtual bool AtualizaUsoDoPergaminho()
        {
            Debug.Log(Estado);
            switch (Estado)
            {
                case ItemUseState.selecaoDeItem:
                    ContainerBasicMenu.instance.Menu.ChangeOption(-commands.vChange);
                    if (commands.confirmButton)
                    {
                        OpcaoEscolhida(ContainerBasicMenu.instance.Menu.SelectedOption);
                    }
                    else if (commands.returnButton)
                    {
                        ContainerBasicMenu.instance.Menu.FinishHud();
                        MessageAgregator<MsgRequestHideUpperLargeMessage>.Publish();
                        Finaliza();
                    }

                    #region Original
                    //GameController.g.HudM.Menu_Basico.MudarOpcao();
                    //if (GameController.g.CommandR.DisparaCancel())
                    //{
                    //    ActionManager.ModificarAcao(GameController.g.transform, null);
                    //    GameController.g.HudM.Painel.EsconderMensagem();
                    //    GameController.g.HudM.Menu_Basico.FinalizarHud();
                    //    Estado = ItemUseState.finalizaUsaItem;
                    //}
                    //else
                    // if (GameController.g.CommandR.DisparaAcao())
                    //{
                    //    OpcaoEscolhida(GameController.g.HudM.Menu_Basico.OpcaoEscolhida);
                    //} 
                    #endregion
                break;
                case ItemUseState.animandoBraco:
                    if (!AnimaB.AnimaTroca(true))
                    {
                        MessageAgregator<MsgRequestEndArmsAnimations>.Publish(new MsgRequestEndArmsAnimations()
                        {
                            oAnimado = Dono
                        });

                        Estado = ItemUseState.aplicandoItem;
                        InstaciarParticulaComSom();

                        //AplicaEfeito(opcaoEscolhida);

                        #region Original
                        //Estado = ItemUseState.aplicandoItem;
                        //Manager.Mov.Animador.ResetaTroca();
                        //AuxiliarDeInstancia.InstancieEDestrua(Particula,
                        //    GameController.g.Manager.CriatureAtivo.transform.position, 1);

                        //AplicaEfeito(GameController.g.Manager.Dados.CriaturesAtivos[opcaoEscolhida]); 
                        #endregion
                    }
                    break;
                case ItemUseState.aplicandoItem:
                    TempoDecorrido += Time.deltaTime;
                    if (TempoDecorrido > TEMPO_DE_ANIMA_PARTICULAS)
                    {
                        AplicaEfeito(opcaoEscolhida);
                    }
                break;
                case ItemUseState.oneMessageOpened:
                    MyGlobalController.Instance.OneMessage.ThisUpdate(commands.confirmButton || commands.returnButton);
                break;
                case ItemUseState.finalizaUsaItem:
                    return false;
            }
            return true;
        }

        public override void IniciaUsoComCriature(GameObject dono, List<ItemBase> lista)
        {
            eMenu = false;
            CharacterManager manager = dono.GetComponent<CharacterManager>();
            this.Dono = dono;
            this.Lista = lista;
            if (manager.ContraTreinador)
            {
                MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
                {
                    message = TextBank.RetornaFraseDoIdioma(TextKey.mensLuta)
                });
            }
            else
            {
                InicioComum();
            }

        }

        //public override void IniciaUsoComCriature(GameObject dono)
        //{
        //    //fluxo = FluxoDeRetorno.heroi;
        //    //if (GameController.g.estaEmLuta)
        //    //{
        //    //    GameController.g.HudM.Painel.AtivarNovaMens(BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.mensLuta)[0], 25, 2);
        //    //    Estado = ItemUseState.finalizaUsaItem;
        //    //}
        //    //else
        //    //{
        //    //    GameController.g.Manager.CriatureAtivo.Estado = CreatureManager.CreatureState.parado;
        //    //    InicioComum();
        //    //}

        //}

        protected string[] NomesDosCriaturesAtivos()
        {
            List<string> nomes = new List<string>();
            List<PetBase> meusCriatures = Dono.GetComponent<CharacterManager>().Dados.CriaturesAtivos;
            for (int i = 0; i < meusCriatures.Count; i++)
            {
                nomes.Add(meusCriatures[i].GetNomeEmLinguas + "\t Lv: " + meusCriatures[i].G_XP.Nivel);
            }

            return nomes.ToArray();
        }

        void InstaciarParticulaComSom()
        {
            
            InstanceSupport.InstancieEDestrua(Particula,
                                Dono.transform.position, 1);

            SupportSingleton.Instance.InvokeInSeconds(() =>
            {
                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                {
                    sfxId = FayvitSounds.SoundEffectID.XP_Heal02
                });
            }, .125f);
        }

        protected virtual void OpcaoEscolhida(int escolha)
        {
            commands = new MsgSendExternalPanelCommand();
            CharacterManager manager = Dono.GetComponent<CharacterManager>();
            if (Consumivel)
                RetirarUmItem(manager.Dados.Itens, this, 1);

            //GameController.g.HudM.Menu_Basico.FinalizarHud();
            //GameController.g.HudM.Painel.EsconderMensagem();
            opcaoEscolhida = escolha;

            if (escolha == 0)
            {
                InicializacaoComum(manager.gameObject, manager.ActivePet.transform);
                Estado = ItemUseState.animandoBraco;
            }
            else
            {
                TempoDecorrido = 0;
                InstaciarParticulaComSom();
                Estado = ItemUseState.aplicandoItem;
            }

            MessageAgregator<MsgRequestHideUpperLargeMessage>.Publish();
            ContainerBasicMenu.instance.Menu.FinishHud();
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.Book1
            });
        }

        protected virtual void AplicaEfeito(int indice) { }

        protected void Finaliza()
        {
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.Book1
            });
            MessageAgregator<MsgSendExternalPanelCommand>.RemoveListener(OnReceiveCommands);
            Estado = ItemUseState.finalizaUsaItem;
            
        }

        protected virtual void EntraNoModoFinalizacao(PetBase C)
        {

            Estado = ItemUseState.emEspera;
            
            SupportSingleton.Instance.InvokeInSeconds(() =>
            {
                MessageAgregator<MsgStartExternalInteraction>.Publish();
                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                {
                    sfxId = FayvitSounds.SoundEffectID.painelAbrindo
                });
                Estado = ItemUseState.oneMessageOpened;
                MyGlobalController.Instance.OneMessage.StartMessagePanel(Finaliza, string.Format(
                textoDaMensInicial[1],
                C.GetNomeEmLinguas, C.G_XP.Nivel));
            }, 1);

            #region Original
            //if (GameController.g.HudM.MenuDePause.EmPause)
            //{
            //    Finaliza();
            //}
            //else
            //    GameController.g.StartCoroutine(MensComAtraso(C)); 
            #endregion
        }

        #region Suprimido
        //System.Collections.IEnumerator MensComAtraso(PetBase C)
        //{
        //    yield return new WaitForSeconds(1f);
        //    MyGlobalController.Instance.OneMessage.StartMessagePanel(Finaliza, string.Format(
        //    textoDaMensInicial[1],
        //    C.GetNomeEmLinguas, C.G_XP.Nivel));

        //    //GameController.g.HudM.UmaMensagem.ConstroiPainelUmaMensagem(Finaliza,
        //    //    string.Format(
        //    //    textoDaMensInicial[1],
        //    //    C.NomeEmLinguas, C.G_XP.Nivel));
        //} 
        #endregion
    }

}