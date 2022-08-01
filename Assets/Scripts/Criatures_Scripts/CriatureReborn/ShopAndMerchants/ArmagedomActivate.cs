using UnityEngine;
using Criatures2021Hud;
using FayvitUI;
using FayvitCam;
using FayvitBasicTools;
using TextBankSpace;
using TalkSpace;
using FayvitMessageAgregator;
using FayvitSupportSingleton;

namespace Criatures2021
{
    public class ArmagedomActivate : ButtonActivate
    {
        [SerializeField] private ScheduledTalkManager NPC = null;
        [SerializeField] private ScheduledTalkManager NPCfalasIniciais = null;
        [SerializeField] private Sprite fotoDoNPC;
        [SerializeField] private IndiceDeArmagedoms indiceDesseArmagedom = IndiceDeArmagedoms.daCavernaInicial;
        [SerializeField] private Transform charRef;
        [SerializeField] private string ID;

        private MsgSendExternalPanelCommand commands;
        private CharacterManager manager;
        private fasesDoArmagedom fase = fasesDoArmagedom.emEspera;
        private TextDisplay dispara;
        private PetReplaceManager replace;
        private float tempoDecorrido = 0;
        private string tempString;
        //private string[] t;
        private string[] frasesDeArmagedom = TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDeArmagedom).ToArray();
        private string[] txtDeOpcoes = TextBank.RetornaListaDeTextoDoIdioma(TextKey.menuDeArmagedom).ToArray();
        

        private enum fasesDoArmagedom
        {
            emEspera,
            mensInicial,
            escolhaInicial,
            curando,
            fraseQueAntecedePossoAjudar,
            armagedadosAberto,
            fazendoUmaTroca,
            escolhaDePergaminho,
            vendendoPergaminho,
            oneMessageAberto,
            activeTalk
        }

        private const float TEMPO_DE_CURA = 2.5F;
        void Start()
        {
            
            if (StaticInstanceExistence<DisplayTextManager>.SchelduleExistence(Start, this, () => {
                return DisplayTextManager.instance;
            }))
            { 
                textoDoBotao = TextBank.RetornaFraseDoIdioma(TextKey.textoBaseDeAcao);
                if (StaticInstanceExistence<IGameController>.SchelduleExistence(Start, this, () => { return AbstractGameController.Instance; }))
                {
                    dispara = DisplayTextManager.instance.DisplayText;
                    //t = TextBank.RetornaListaDeTextoDoIdioma(TextKey.primeiroArmagedom).ToArray();
                }
            }
        }

        new void Update()
        {
            base.Update();

            switch (fase)
            {
                case fasesDoArmagedom.mensInicial:
                    CameraApplicator.cam.FocusInPoint(8, 2);
                    //if (dispara.UpdateTexts(commands.confirmButton,commands.returnButton, t, fotoDoNPC)
                    //    ||
                    //    dispara.messageArrayIndex > t.Length - 2
                    //    )
                    if(NPCfalasIniciais.Update(commands.confirmButton,commands.returnButton))
                    {
                        
                        EntraFrasePossoAjudar();
                        LigarMenu();
                    }
                    break;
                case fasesDoArmagedom.escolhaInicial:
                    CameraApplicator.cam.FocusInPoint(8,2/*, -1, true*/);
                    if (!dispara.LendoMensagemAteOCheia(commands.confirmButton))
                    {
                        
                        ContainerBasicMenu.instance.Menu.ChangeOption(-commands.vChange);

                        if(commands.confirmButton)
                            OpcaoEscolhida(ContainerBasicMenu.instance.Menu.SelectedOption);

                    }

                    if (commands.returnButton)
                    {
                        OpcaoEscolhida(txtDeOpcoes.Length - 1);
                    }
                    break;
                case fasesDoArmagedom.curando:

                    tempoDecorrido += Time.deltaTime;
                    if (tempoDecorrido > TEMPO_DE_CURA || commands.confirmButton)
                    {
                        fase = fasesDoArmagedom.fraseQueAntecedePossoAjudar;
                        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                        {
                            sfxId = FayvitSounds.SoundEffectID.Book1
                        });
                        dispara.TurnPanels();
                        dispara.StartShowMessage(frasesDeArmagedom[0], fotoDoNPC);
                    }
                    break;
                case fasesDoArmagedom.fraseQueAntecedePossoAjudar:
                    if (!dispara.LendoMensagemAteOCheia(commands.confirmButton))
                    {
                        if(commands.confirmButton)
                        {
                            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                            {
                                sfxId = FayvitSounds.SoundEffectID.Book1
                            });
                            LigarMenu();
                            EntraFrasePossoAjudar();
                        };

                        
                    }
                    break;
                case fasesDoArmagedom.oneMessageAberto:
                    AbstractGlobalController.Instance.OneMessage.ThisUpdate(commands.confirmButton || commands.returnButton);
                break;
                case fasesDoArmagedom.armagedadosAberto:
                    
                break;
                case fasesDoArmagedom.activeTalk:
                    if (NPC.Update(commands.confirmButton, commands.returnButton))
                    {
                        LigarMenu();
                        EntraFrasePossoAjudar();
                    }
                break;
            }
        }

        private void OnValidate()
        {
            BuscadorDeID.Validate(ref ID, this);
        }

        void LigarMenu()
        {
            ContainerBasicMenu.instance.SetPercentSizeInTheParent(.65f, .25f, .99f, .75f);
            ContainerBasicMenu.instance.Menu.StartHud(OpcaoEscolhida, txtDeOpcoes);
        }

        void OpcaoEscolhida(int opcao)
        {
            //ActionManager.ModificarAcao(GameController.g.transform, () => { });

            ContainerBasicMenu.instance.Menu.FinishHud();

            switch (opcao)
            {
                case 0:
                    Curar();
                break;
                case 1:
                    CriaturesArmagedados();
                break;
                case 2:
                    IniciarConversar();
                break;
                case 3:
                    VoltarAoJogo();
                break;

            }
        }

        private void IniciarConversar()
        {
            //dispara.OffPanels();
            NPC.Start();
            SomDoIniciar();
            //if (condicoesComplementares != null)
            //    for (int i = 0; i < condicoesComplementares.Length; i++)
            //        AbstractGameController.Instance.MyKeys.MudaShift(condicoesComplementares[i], true);

            MessageAgregator<MsgChangeShiftKey>.Publish(new MsgChangeShiftKey() { change = true, sKey = ID });

            FluxoDeBotao();

            
            NPC.IniciaConversa();

            fase = fasesDoArmagedom.activeTalk;

        }

        //void ComprarPergaminhos()
        //{
        //    dispara.ReligarPaineis();
        //    dispara.Dispara(string.Format(frasesDeArmagedom[8], new MbPergaminhoDeArmagedom().Valor.ToString()), fotoDoNPC);
        //    GameController.g.HudM.Menu_Basico.IniciarHud(EscolhaDeComprarPergaminho,
        //        TextBank.RetornaListaDeTextoDoIdioma(TextKey.simOuNao).ToArray());
        //    fase = fasesDoArmagedom.escolhaDePergaminho;

        //    ActionManager.ModificarAcao(
        //        GameController.g.transform,
        //        () => { EscolhaDeComprarPergaminho(GameController.g.HudM.Menu_Basico.OpcaoEscolhida); }
        //        );
        //}

        //void EscolhaDeComprarPergaminho(int escolha)
        //{
        //    GameController.g.HudM.Menu_Basico.FinalizarHud();
        //    switch (escolha)
        //    {
        //        case 0:
        //            GameController.g.HudM.PainelQuantidades.IniciarEssaHud(PegaUmItem.Retorna(nomeIDitem.pergArmagedom));
        //            break;
        //        case 1:
        //            LigarMenu();
        //            EntraFrasePossoAjudar();
        //            break;
        //    }


        //}

        public void CriaturesArmagedados()
        {

            //ApagarMenu();
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.painelAbrindo
            });
            dispara.OffPanels();
            PetBase[] armagedados = manager.Dados.CriaturesArmagedados.ToArray();
            if (armagedados.Length > 0)
            {
                MessageAgregator<MsgOpenPetList>.Publish(new MsgOpenPetList()
                {
                    armagedom = true,
                    dono = manager
                });

                MessageAgregator<MsgReturnToArmgdMenu>.AddListener(OnReturnToArmagedomMenu);

                //g.HudM.EntraCriatures.IniciarEssaHUD(armagedados, AoEscolherumCriature);
                //GameController.g.HudM.Painel.AtivarNovaMens(frasesDeArmagedom[2], 30);
                fase = fasesDoArmagedom.armagedadosAberto;

                //ActionManager.ModificarAcao(GameController.g.transform, () =>
                //{
                //    AoEscolherumCriature(GameController.g.HudM.EntraCriatures.OpcaoEscolhida);
                //});
            }
            else
            {
                dispara.TurnPanels();
                dispara.StartShowMessage(frasesDeArmagedom[1], fotoDoNPC);
                fase = fasesDoArmagedom.fraseQueAntecedePossoAjudar;
            }
        }

        private void OnReturnToArmagedomMenu(MsgReturnToArmgdMenu obj)
        {
            if (obj.entra < 0)
            {
                LigarMenu();
                EntraFrasePossoAjudar();
            }
            else if (obj.entra >= 0 && obj.sai < 0)
            {
                manager.Dados.CriaturesAtivos.Add(manager.Dados.CriaturesArmagedados[obj.entra]);
                manager.Dados.CriaturesArmagedados.RemoveAt(obj.entra);
                LigarMenu();
                EntraFrasePossoAjudar();
            }
            else if (obj.entra >= 0 && obj.sai == 0)
            {
                

                manager.Dados.CriatureSai = -1;

                PetBase P = manager.Dados.CriaturesArmagedados[obj.entra];
                manager.Dados.CriaturesAtivos[0].EstadoPerfeito();

                manager.Dados.CriaturesArmagedados[obj.entra] = manager.Dados.CriaturesAtivos[0];
                manager.Dados.CriaturesAtivos[0] = P;

                tempString = string.Format(frasesDeArmagedom[6], P.GetNomeEmLinguas, P.PetFeat.mNivel.Nivel,
                   manager.Dados.CriaturesArmagedados[obj.entra].GetNomeEmLinguas,
                   manager.Dados.CriaturesArmagedados[obj.entra].PetFeat.mNivel.Nivel
                   );

                replace = gameObject.AddComponent<PetReplaceManager>();
                replace.StartReplace(manager.transform, manager.ActivePet.transform, FluxoDeRetorno.armagedom,
                    manager.Dados.CriaturesAtivos[0], null
                    );

                

                MessageAgregator<MsgEndReplaceForArmagedom>.AddListener(OnEndReplace);
                //replace = new ReplaceManager(g.Manager, g.Manager.CriatureAtivo.transform, FluxoDeRetorno.armagedom);
                fase = fasesDoArmagedom.fazendoUmaTroca;
            }
            else if (obj.entra >= 0 && obj.sai > 0)
            {
                PetBase P = manager.Dados.CriaturesArmagedados[obj.entra];
                manager.Dados.CriaturesAtivos[obj.sai].EstadoPerfeito();

                manager.Dados.CriaturesArmagedados[obj.entra] = manager.Dados.CriaturesAtivos[obj.sai];
                manager.Dados.CriaturesAtivos[obj.sai] = P;
                    
                tempString = string.Format(frasesDeArmagedom[6], P.GetNomeEmLinguas, P.PetFeat.mNivel.Nivel,
                   manager.Dados.CriaturesArmagedados[obj.entra].GetNomeEmLinguas,
                   manager.Dados.CriaturesArmagedados[obj.entra].PetFeat.mNivel.Nivel
                   );

                AbstractGlobalController.Instance.OneMessage.StartMessagePanel(VoltarDoEntraArmagedado, tempString);

                fase = fasesDoArmagedom.oneMessageAberto;
            }

            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<MsgReturnToArmgdMenu>.RemoveListener(OnReturnToArmagedomMenu);
            });
        }

        public void VoltarDoEntraArmagedado()
        {
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.Decision2
            });
            LigarMenu();
            EntraFrasePossoAjudar();
            //GameController.g.HudM.EntraCriatures.FinalizarHud();
            //GameController.g.HudM.Painel.EsconderMensagem();
        }

        #region suprimido
        //void AoEscolherumCriature(int indice)
        //{


        //    DadosDeJogador dados = manager.Dados;

        //    if (dados.CriaturesAtivos.Count < dados.MaxCarregaveis)
        //    {
        //        PetBase C = dados.CriaturesArmagedados[indice];
        //        AbstractGlobalController.Instance.OneMessage.StartMessagePanel(VoltarDoEntraArmagedado,
        //            string.Format(frasesDeArmagedom[3], C.GetNomeEmLinguas, C.PetFeat.mNivel.Nivel)
        //            );
        //        dados.CriaturesArmagedados.Remove(C);
        //        dados.CriaturesAtivos.Add(C);
        //    }
        //    else
        //    {
        //        PetBase C = dados.CriaturesArmagedados[indice];
        //        Debug.Log(indice);
        //        indiceDoSubstituido = indice;
        //        Debug.Log(indiceDoSubstituido);
        //        AbstractGlobalController.Instance.OneMessage.StartMessagePanel(MostraOsQueSaem,
        //            string.Format(frasesDeArmagedom[4], C.GetNomeEmLinguas, C.PetFeat.mNivel.Nivel)
        //            );
        //        //GameController.g.HudM.EntraCriatures.FinalizarHud();
        //    }

        //}

        //void SubstituiArmagedado(int indice)
        //{

        //    DadosDeJogador dados = manager.Dados;
        //    Debug.Log(indiceDoSubstituido);
        //    PetBase temp = dados.CriaturesArmagedados[indiceDoSubstituido];

        //    dados.CriaturesArmagedados[indiceDoSubstituido] = dados.CriaturesAtivos[indice];
        //    dados.CriaturesAtivos[indice] = temp;

        //    Debug.Log(dados.CriaturesAtivos[indice].NomeID + " : " + dados.CriaturesArmagedados[indiceDoSubstituido].NomeID + " : " + temp.NomeID);

        //    tempString = string.Format(frasesDeArmagedom[6], temp.GetNomeEmLinguas, temp.PetFeat.mNivel.Nivel,
        //            dados.CriaturesArmagedados[indiceDoSubstituido].GetNomeEmLinguas,
        //            dados.CriaturesArmagedados[indiceDoSubstituido].PetFeat.mNivel.Nivel
        //            );

        //    if (indice == 0)
        //    {
        //        dados.CriatureSai = -1;
        //        //g.HudM.EntraCriatures.FinalizarHud();
        //        //GameController.g.HudM.Painel.EsconderMensagem();
        //        replace = gameObject.AddComponent<PetReplaceManager>();
        //        replace.StartReplace(manager.transform, manager.ActivePet.transform, FluxoDeRetorno.armagedom,
        //            dados.CriaturesAtivos[dados.CriatureSai + 1], null
        //            );

        //        MessageAgregator<MsgEndReplaceForArmagedom>.AddListener(OnEndReplace);
        //        //replace = new ReplaceManager(g.Manager, g.Manager.CriatureAtivo.transform, FluxoDeRetorno.armagedom);
        //        fase = fasesDoArmagedom.fazendoUmaTroca;
        //    }
        //    else
        //    {
        //        AbstractGlobalController.Instance.OneMessage.StartMessagePanel(VoltarDoEntraArmagedado, tempString);
        //    }
        //}
        #endregion

        private void OnEndReplace(MsgEndReplaceForArmagedom obj)
        {
            AbstractGlobalController.Instance.OneMessage.StartMessagePanel(() =>
            {
                VoltarDoEntraArmagedado();
                fase = fasesDoArmagedom.escolhaInicial;
            }, tempString);
            CameraApplicator.cam.StartShowPointCamera(transform, new SinglePointCameraProperties()
            {
                velOrTimeFocus = 0f,
                withTime = true
            });
            fase = fasesDoArmagedom.oneMessageAberto;
            manager.Dados.CriatureSai = 0;

            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<MsgEndReplaceForArmagedom>.RemoveListener(OnEndReplace);
            });
        }

        void InstanciaVisaoDeCura()
        {
            

            Vector3 V = manager.ActivePet.transform.position;
            Vector3 V2 = manager.transform.position;
            Vector3 V3 = new Vector3(1, 0, 0);
            Vector3[] Vs = new Vector3[] { V, V2 + V3, V2 + 2 * V3, V2 - V3, V2 - 2 * V3, V2 + 3 * V2, V2 - 3 * V3 };
            GameObject animaVida = ResourcesFolders.GetGeneralElements(GeneralParticles.acaoDeCura1);
            GameObject animaVida2;

            for (int i = 0; i < manager.Dados.CriaturesAtivos.Count; i++)
            {
                if (i < Vs.Length)
                {
                    animaVida2 = Instantiate(animaVida, Vs[i], Quaternion.identity);
                    Destroy(animaVida2, 1);
                }
            }

            Destroy(Instantiate(ResourcesFolders.GetGeneralElements(GeneralParticles.curaDeArmagedom), manager.transform.position, Quaternion.identity), 10);
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.XP_Heal02
            });
        }

        public void Curar()
        {
            //ApagarMenu();
            InstanciaVisaoDeCura();

            manager.Dados.TodosCriaturesPerfeitos();

            tempoDecorrido = 0;
            
            dispara.OffPanels();
            fase = fasesDoArmagedom.curando;
        }

        public void VoltarAoJogo()
        {
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.Book1
            });

            MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero()
            {
                myHero = manager.gameObject
            });

            MessageAgregator<FillTextDisplayMessage>.Publish();
            dispara.OffPanels();
            
            fase = fasesDoArmagedom.emEspera;
            
            MessageAgregator<MsgSendExternalPanelCommand>.RemoveListener(OnReceiveCommands);

            MessageAgregator<MsgReturnGameElmentsHud>.Publish();
        }

        void EntraFrasePossoAjudar()
        {
            dispara.TurnPanels();
            dispara.StartShowMessage(frasesDeArmagedom[9], fotoDoNPC);
            fase = fasesDoArmagedom.escolhaInicial;
            
        }

        public override void SomDoIniciar()
        {
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.Decision1
            });
        }

        public void BotaoArmagedom()
        {
            SomDoIniciar();
            commands = new MsgSendExternalPanelCommand();
            MessageAgregator<MsgSendExternalPanelCommand>.AddListener(OnReceiveCommands);

            FluxoDeBotao();

            CameraApplicator.cam.StartExibitionCam(transform, 1);
            MessageAgregator<MsgHideGameElmentsHud>.Publish();
            
            IndiceDeArmagedoms k = (IndiceDeArmagedoms)AbstractGameController.Instance.MyKeys.VerificaCont(KeyCont.armagedoms);
            k |= indiceDesseArmagedom;
            AbstractGameController.Instance.MyKeys.MudaCont(KeyCont.armagedoms, (int)k);

            dispara.StartTextDisplay();
            manager = MyGlobalController.MainPlayer;
            manager.Dados.UltimoArmagedom = indiceDesseArmagedom;

            CameraApplicator.cam.StartShowPointCamera(transform,new SinglePointCameraProperties() { 
                velOrTimeFocus=0.75f,
                withTime=true
            });

            fase = fasesDoArmagedom.mensInicial;
            NPCfalasIniciais.Start();
            NPCfalasIniciais.IniciaConversa();
            Vector3 dir = charRef.parent.position - manager.transform.position;
            CharRotateTo.RotateDir(dir, manager.gameObject);
            CharRotateTo.RotateDir(-dir, charRef.parent.gameObject);
        }

        private void OnReceiveCommands(MsgSendExternalPanelCommand obj)
        {
            commands = obj;
        }

        public override void FuncaoDoBotao()
        {
            BotaoArmagedom();
        }
    }
}