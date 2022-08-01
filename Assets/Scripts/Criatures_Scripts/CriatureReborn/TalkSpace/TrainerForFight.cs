using Criatures2021Hud;
using FayvitBasicTools;
using FayvitMessageAgregator;
using UnityEngine;
using TextBankSpace;
using Criatures2021;
using FayvitCam;
using FayvitSounds;

namespace TalkSpace
{
    public class TrainerForFight : ButtonActivate
    {
        
        [SerializeField] private string chaveDaLuta;
        [SerializeField] private bool perguntaParaComecar = true;
        [SerializeField] private PetsForBattle[] criaturesDoTreinador;
        [SerializeField] private ScheduledTalkManager npcAntesDaLuta;
        [SerializeField] private ScheduledTalkManager npcAoGanharLuta;
        [SerializeField] private ScheduledTalkManager npcDepoisDaLutaGanha;
        [SerializeField] private TextKey textPerguntaComecar;
        [SerializeField] private ChestItem[] recompensasFimDeLuta;
        [SerializeField]
        private NameMusicaComVolumeConfig temaDoTreinador = new NameMusicaComVolumeConfig()
        {
            Musica = NameMusic.AsRosasNaoFalam,
            Volume = 1
        };
        [SerializeField]
        private NameMusicaComVolumeConfig musicaDaLuta = new NameMusicaComVolumeConfig()
        {
            Musica = NameMusic.TicoTicoNoFuba_v2,
            Volume = 1
        };
        [SerializeField]
        private NameMusicaComVolumeConfig temaDoTreinadorNoCalorDaBatalha = new NameMusicaComVolumeConfig()
        {
            Musica = NameMusic.CaixaDeFosforo,
            Volume = 1
        };

        private int indiceDoEnviado = 0;
        private float tempoDecorrido = 0;
        private string[] conversa;
        private Vector3 npcOriginalPosition;
        private Vector3 managerOriginalPosition;
        private ThisState state = ThisState.emEspera;
        private AnimateArm animaB;
        private GameObject myActivePet;
        private MsgSendExternalPanelCommand commands = new MsgSendExternalPanelCommand();

        private enum ThisState
        {
            emEspera,
            conversaAntesDaLuta,
            perguntaComecarBatalha,
            animacaoDeEncontro,
            cameraNoTreinador,
            frasePreInicio,
            animandoBraco,
            leituraDeLuta,
            menuAberto,
            novoJogoDeCamera,
            fraseDeVitoria,
            fraseDaFinalizacao,
            finalizacao,
            verificandoMaisItens,
            conversaDeLutaGanha
        }

        public Transform MeuTransform { get => transform.parent; }
        public CharacterManager Manager{ get; set; }

        private void OnValidate()
        {
            BuscadorDeID.Validate(ref chaveDaLuta, this, "chaveDaLuta");
        }

        private void Start()
        {
            textoDoBotao = TextBank.RetornaFraseDoIdioma(TextKey.textoBaseDeAcao);
            SempreEstaNoTrigger();

            MessageAgregator<MsgRequestExternalFightStart>.AddListener(OnRequestFightByExternal);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgRequestExternalFightStart>.RemoveListener(OnRequestFightByExternal);
        }

        private void OnRequestFightByExternal(MsgRequestExternalFightStart obj)
        {
            if (obj.fighters == transform)
            {
                FuncaoDoBotao();
            }
        }

        new void Update()
        {
            base.Update();

            switch (state)
            {
                case ThisState.conversaAntesDaLuta:
                    #region conversa antes da luta
                    if (npcAntesDaLuta.Update(commands.confirmButton, commands.returnButton))
                    {

                        DisplayTextManager.instance.DisplayText.OffPanels();

                        if (perguntaParaComecar)
                        {
                            DisplayTextManager.instance.DisplayText.TurnPanels();

                            DisplayTextManager.instance.DisplayText.StartShowMessage(
                                TextBank.RetornaFraseDoIdioma(textPerguntaComecar)
                                );

                            state = ThisState.perguntaComecarBatalha;
                            MessageAgregator<MsgStartMusic>.Publish(new MsgStartMusic()
                            {
                                nmcvc = temaDoTreinadorNoCalorDaBatalha
                            });
                        }
                        else
                        {
                            IniciarLutaContraTreinador();
                        }
                    }
                    #endregion
                break;
                case ThisState.perguntaComecarBatalha:
                    #region pergunta para iniciar batalha
                    if (!DisplayTextManager.instance.DisplayText.LendoMensagemAteOCheia(commands.confirmButton))
                    {
                        YesOrNoMenu.instance.Menu.StartHud(YesOrNoResponse,
                            TextBank.RetornaListaDeTextoDoIdioma(TextKey.simOuNao).ToArray());
                        state = ThisState.menuAberto;
                    }
                    #endregion
                break;
                case ThisState.menuAberto:
                    YesOrNoMenu.instance.Menu.ChangeOption(commands.vChange);

                    if (commands.confirmButton)
                        YesOrNoResponse(YesOrNoMenu.instance.Menu.SelectedOption);
                break;
                case ThisState.animacaoDeEncontro:
                    #region animacao de encontro
                    tempoDecorrido += Time.deltaTime;
                    if (tempoDecorrido > 0.5F)
                    {
                        CameraApplicator.cam.StartShowPointCamera(MeuTransform, new SinglePointCameraProperties()
                        {
                            withTime = true,
                            dodgeCam = true,
                            velOrTimeFocus = .5f
                        });
                        state = ThisState.cameraNoTreinador;
                    }
                    #endregion
                break;
                case ThisState.cameraNoTreinador:
                    #region camera no treinador
                    if (CameraApplicator.cam.FocusInPoint(height: 3))
                    {
                        
                        DisplayTextManager.instance.DisplayText.StartTextDisplay();
                        conversa = TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDaLutaContraTreinador).ToArray();
                        conversa = new string[2] {
                        string.Format(conversa[0],criaturesDoTreinador.Length), conversa[1] };
                        state = ThisState.frasePreInicio;
                    }
                    #endregion
                break;
                case ThisState.frasePreInicio:
                    #region frasePreInicio
                    if (DisplayTextManager.instance.DisplayText.UpdateTexts(commands.confirmButton,false,conversa))
                    {
                        animaB = new AnimateArm(MeuTransform, Manager.transform, true);
                        Transform aux = Manager.ActivePet.transform;
                        animaB.PosCriature = aux.position + 3 * aux.forward;

                        state = ThisState.animandoBraco;
                        CameraApplicator.cam.OffCamera();
                    }
                    break;
                #endregion
                case ThisState.animandoBraco:
                    FayvitMessageAgregator.MessageAgregator<FayvitMove.ChangeMoveSpeedMessage>.Publish(new FayvitMove.ChangeMoveSpeedMessage()
                    {
                        velocity = Vector3.zero,
                        gameObject = gameObject
                    });
                    #region animando braco
                    if (!animaB.AnimaEnvia(criaturesDoTreinador[indiceDoEnviado].Pet, "criatureDeTreinador"))
                    {
                        PetManager enemy = GameObject.Find("criatureDeTreinador").GetComponent<PetManager>();
                        //GameController.g.EncontroAgoraCom(
                        criaturesDoTreinador[indiceDoEnviado].PrepararInicioDoCriature(enemy,Manager.ActivePet.gameObject);
                        //, true, nomeDoTreinador);
                        myActivePet = enemy.gameObject;

                        MessageAgregator<MsgCriatureDefeated>.AddListener(OnCriatureDefeated);
                        MessageAgregator<MsgRequestChangeToPetByReplace>.Publish(new MsgRequestChangeToPetByReplace()
                        {
                            dono = Manager.gameObject,
                            fluxo = FluxoDeRetorno.criature
                        });

                        ((PetManagerCharacter)Manager.ActivePet).ControlableVsTrainer(enemy.transform);

                        
                        state = ThisState.leituraDeLuta;
                    }
                    #endregion
                break;
                case ThisState.fraseDeVitoria:
                    if (DisplayTextManager.instance.DisplayText.UpdateTexts(commands.confirmButton, commands.returnButton, conversa))
                    {
                        VerificaContinuidadeDaBatalha();
                    }
                break;
                case ThisState.novoJogoDeCamera:
                    if (CameraApplicator.cam.FocusInPoint(height: 3))
                    {
                        Destroy(myActivePet);
                        state = ThisState.frasePreInicio; 
                    }
                break;
                case ThisState.fraseDaFinalizacao:
                    #region frase da finalizacao
                    if (npcAoGanharLuta.Update(commands.confirmButton, commands.returnButton))
                    {
                        if (recompensasFimDeLuta.Length <= 0)
                        {
                            state = ThisState.finalizacao;
                        }
                        else
                        {
                            conversa = TextBank.RetornaListaDeTextoDoIdioma(TextKey.bau).ToArray();
                            indiceDoEnviado = 0;
                            VerificaItem();
                            state = ThisState.verificandoMaisItens;
                        }
                    }
                    #endregion
                break;
                case ThisState.verificandoMaisItens:
                    #region verificando mais itens
                    if (commands.confirmButton)
                    {
                        if (indiceDoEnviado + 1 > recompensasFimDeLuta.Length)
                        {
                            MessageAgregator<MsgRequestHideUpperLargeMessage>.Publish();
                            MessageAgregator<MsgHideShowItem>.Publish();

                            state = ThisState.finalizacao;
                        }
                        else
                        {
                            VerificaItem();
                        }
                        
                    }
                    #endregion
                break;
                case ThisState.finalizacao:
                    
                    AbstractGameController.Instance.MyKeys.MudaAutoShift(chaveDaLuta, true);
                    FayvitSave.SaveDatesManager.SalvarAtualizandoDados(new Criatures2021.SaveDates());
                    VoltarAoModoPasseio();
                break;
                case ThisState.conversaDeLutaGanha:
                    if (npcDepoisDaLutaGanha.Update(commands.confirmButton, commands.returnButton))
                    {
                        VoltarAoModoPasseio();
                    }
                break;
            }
        }

        private void VoltarAoModoPasseio()
        {
            if (Manager == null)
            {
                Manager = MyGlobalController.MainPlayer;
            }
            Manager.ContraTreinador = false;
            state = ThisState.emEspera;
            MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero()
            {
                myHero = Manager.gameObject
            });
            MessageAgregator<MsgReturnRememberedMusic>.Publish();

            MessageAgregator<MsgSendExternalPanelCommand>.RemoveListener(OnReceiveCommands);
        }

        private void VerificaItem()
        {
            ChestItem ii = recompensasFimDeLuta[indiceDoEnviado];
            MessageAgregator<MsgGetChestItem>.Publish(new MsgGetChestItem()
            {
                message = string.Format(conversa[3], ii.Quantidade, ItemBase.NomeEmLinguas(ii.Item)),
                nameItem = ii.Item,
                quantidade = ii.Quantidade
            });

            indiceDoEnviado++;
        }

        private void OnCriatureDefeated(MsgCriatureDefeated obj)
        {
            if (obj.defeated ==myActivePet)
            {
                conversa = new string[1] { string.Format(
                        TextBank.RetornaFraseDoIdioma(TextKey.apresentaFim),
                        Manager.ActivePet.MeuCriatureBase.GetNomeEmLinguas,
                        obj.doDerrotado.PetFeat.meusAtributos.PV.Maximo,
                        2*obj.doDerrotado.PetFeat.meusAtributos.PV.Maximo,
                        obj.doDerrotado.GetNomeEmLinguas
                        )};

                Manager.ActivePet.PararCriatureNoLocal();
                DisplayTextManager.instance.DisplayText.StartTextDisplay();
                state = ThisState.fraseDeVitoria;
                
                commands = new MsgSendExternalPanelCommand();
                MessageAgregator<MsgStartExternalInteraction>.Publish();
                MessageAgregator<MsgStartMusic>.Publish(new MsgStartMusic()
                {
                    nmcvc = temaDoTreinadorNoCalorDaBatalha
                });

            }
        }

        private void VerificaContinuidadeDaBatalha()
        {
            indiceDoEnviado++;
            if (indiceDoEnviado < criaturesDoTreinador.Length)
            {
                
                conversa = TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDaLutaContraTreinador).ToArray();
                conversa = new string[2] { conversa[2], conversa[3] };
                DisplayTextManager.instance.DisplayText.StartTextDisplay();

                CameraApplicator.cam.StartShowPointCamera(MeuTransform, new SinglePointCameraProperties()
                {
                    withTime = true,
                    dodgeCam = true,
                    velOrTimeFocus = .5f
                });

                state = ThisState.novoJogoDeCamera;
            }
            else
            {
                state = ThisState.leituraDeLuta;
                AbstractGlobalController.Instance.FadeV.StartFadeOutWithAction(() => {
                    Destroy(GameObject.Find("cilindroEncontro"));
                    Destroy(myActivePet);
                    MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero()
                    {
                        myHero = Manager.gameObject
                    });
                    MessageAgregator<MsgStartExternalInteraction>.Publish();
                    Manager.GetComponent<CharacterController>().enabled = false;
                    Manager.transform.position = managerOriginalPosition;
                    Manager.GetComponent<CharacterController>().enabled = true;
                    MeuTransform.GetComponent<CharacterController>().enabled = false;
                    MeuTransform.position = npcOriginalPosition;
                    MeuTransform.GetComponent<CharacterController>().enabled = true;

                    AbstractGlobalController.Instance.FadeV.StartFadeInWithAction(() =>
                    {

                        //AplicadorDeCamera.cam.InicializaCameraExibicionista(MeuTransform, 1, true);

                        DisplayTextManager.instance.DisplayText.StartTextDisplay();
                        npcAoGanharLuta.Start();
                        npcAoGanharLuta.IniciaConversa();
                        state = ThisState.fraseDaFinalizacao;
                    }, .75f);
                },1);
                
            }
        }

        private void YesOrNoResponse(int obj)
        {
            switch (obj)
            { 
                case 0:
                    IniciarLutaContraTreinador();
                break;
                case 1:
                    VoltarAoModoPasseio();
                break;
            }

            YesOrNoMenu.instance.Menu.FinishHud();
            DisplayTextManager.instance.DisplayText.OffPanels();
        }

        private void IniciarLutaContraTreinador()
        {
            npcOriginalPosition = MeuTransform.position;

            Manager = MyGlobalController.MainPlayer;
            managerOriginalPosition = Manager.transform.position;

            tempoDecorrido = 0;
            CameraApplicator.cam.OffCamera(); /*StartExibitionCam(transform, 1, true);*/
            InsereElementosDoEncontro.EncontroDeTreinador(Manager, MeuTransform);
            MessageAgregator<MsgStartMusic>.Publish(new MsgStartMusic()
            {
                nmcvc = musicaDaLuta
            });
            state = ThisState.animacaoDeEncontro;

            MessageAgregator<MsgStartAnimateArmToFight>.Publish(new MsgStartAnimateArmToFight()
            {
                sender = gameObject
            });
        }

        public override void FuncaoDoBotao()
        {
            FluxoDeBotao();
            commands = new MsgSendExternalPanelCommand();
            MessageAgregator<MsgSendExternalPanelCommand>.AddListener(OnReceiveCommands);

            Transform player = MyGlobalController.MainCharTransform;
            Vector3 dir = transform.parent.position - player.position;
            CharRotateTo.RotateDir(dir, player.gameObject);
            CharRotateTo.RotateDir(-dir, MeuTransform.gameObject);
            MessageAgregator<MsgStartMusicWithRecovery>.Publish(new MsgStartMusicWithRecovery()
            {
                nmcvc = temaDoTreinador
            });

            if (!AbstractGameController.Instance.MyKeys.VerificaAutoShift(chaveDaLuta))
            {   
                npcAntesDaLuta.Start();
                npcAntesDaLuta.IniciaConversa();
                state = ThisState.conversaAntesDaLuta;
                
                //if (chaveDepoisDeFinalizado != "")
                //    conversa = BancoDeTextos.RetornaListaDeTextoDoIdioma(StringParaEnum.ObterEnum(chaveDepoisDeFinalizado, chaveDaFinalizacao)).ToArray();
            }
            else {
                FluxoDeBotao();
                npcDepoisDaLutaGanha.Start();
                npcDepoisDaLutaGanha.IniciaConversa();
                state = ThisState.conversaDeLutaGanha;
            }

            //base.IniciaConversa();
        }

        private void OnReceiveCommands(MsgSendExternalPanelCommand obj)
        {
            commands = obj;
        }
    }

    [System.Serializable]
    public struct PetsForBattle
    {
        [SerializeField] private PetBase pet;
        [SerializeField] private bool golpeDeInspector;
        [SerializeField] private bool pvDeInspector;

        public PetBase Pet
        {
            get { return pet; }
        }

        public PetManager PrepararInicioDoCriature(PetManager cm,GameObject atacante)
        {
            GameObject G = cm.gameObject;
            PetBase P = cm.MeuCriatureBase;
            MonoBehaviour.Destroy(cm);
            
            cm = G.AddComponent<PetManagerTrainer>();
            cm.MeuCriatureBase = P;

            ((PetManagerEnemy)cm).StartAgressiveIa(atacante);

            if (golpeDeInspector)
                cm.MeuCriatureBase.GerenteDeGolpes.meusGolpes = Pet.GerenteDeGolpes.meusGolpes;

            if (pvDeInspector)
            {
                cm.MeuCriatureBase.PetFeat.meusAtributos.PV.Maximo = Pet.PetFeat.meusAtributos.PV.Maximo;
                cm.MeuCriatureBase.PetFeat.meusAtributos.PV.Corrente = Pet.PetFeat.meusAtributos.PV.Corrente;
            }

            

            ///*O groundCheck tinha problemas por ser inserido no personagem com escala variando durante o animateArm*/
            //cm.Mov.RecalculeGroundCheck();

            //GameController.g.EncontroAgoraCom(cm);
            return cm;
        }
    }

    public struct MsgRequestExternalFightStart : IMessageBase {
        public Transform fighters;
    }

   
}

public struct MsgStartAnimateArmToFight : IMessageBase
{
    public GameObject sender;
}