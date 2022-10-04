using Criatures2021Hud;
using FayvitBasicTools;
using FayvitMessageAgregator;
using UnityEngine;
using TextBankSpace;
using Criatures2021;
using FayvitCam;
using FayvitSounds;
using FayvitMove;
using FayvitSupportSingleton;
using Npc2021;

namespace TalkSpace
{
    public class TrainerForFight : ButtonActivate
    {
        
        [SerializeField] private string chaveDaLuta;
        [SerializeField] private bool perguntaParaComecar = true;
        [SerializeField,ArrayElementTitle("pet.petId")] private PetsForBattle[] criaturesDoTreinador;
        [SerializeField] private ScheduledTalkManager npcAntesDaLuta;
        [SerializeField] private ScheduledTalkManager npcAoGanharLuta;
        [SerializeField] private ScheduledTalkManager npcDepoisDaLutaGanha;
        [SerializeField] private TextKey textPerguntaComecar;
        [SerializeField] private ChestItem[] recompensasFimDeLuta;
        [SerializeField] private TipoDeAfastamento tipoDeAfast = TipoDeAfastamento.posNoMapa;
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
        private PetManager myActivePet;

        public string ChaveDaLuta => chaveDaLuta;

        protected MsgSendExternalPanelCommand Commands { get; private set; } = new MsgSendExternalPanelCommand();

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
            conversaDeLutaGanha,
            atividadeExterna,
            showTrainerPet
        }

        public Transform MeuTransform { get => transform.parent; }
        public CharacterManager Manager{ get; set; }

        protected virtual void OnValidate()
        {
            BuscadorDeID.Validate(ref chaveDaLuta, this, "chaveDaLuta");
            if (npcAntesDaLuta!=null)
            {
                npcAntesDaLuta.OnVallidate();
                npcAoGanharLuta.OnVallidate();
                npcDepoisDaLutaGanha.OnVallidate();
            }
        }

        private void Start()
        {
            textoDoBotao = TextBank.RetornaFraseDoIdioma(TextKey.textoBaseDeAcao);
            SempreEstaNoTrigger();

            MessageAgregator<MsgRequestExternalFightStart>.AddListener(OnRequestFightByExternal);
            MessageAgregator<MsgStartRerturnToArmagedom>.AddListener(OnReturnToArmagedom);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgRequestExternalFightStart>.RemoveListener(OnRequestFightByExternal);
            MessageAgregator<MsgStartRerturnToArmagedom>.RemoveListener(OnReturnToArmagedom);
        }

        private void OnReturnToArmagedom(MsgStartRerturnToArmagedom obj)
        {
            if (!AbstractGameController.Instance.MyKeys.VerificaAutoShift(chaveDaLuta) && Manager != null && obj.dono == Manager)
            {
                MeuTransform.GetComponent<CharacterController>().enabled = false;
                MeuTransform.position = npcOriginalPosition;
                MeuTransform.GetComponent<CharacterController>().enabled = true;
                Destroy(GameObject.Find("cilindroEncontro"));
                Destroy(myActivePet.gameObject);
                foreach (var v in criaturesDoTreinador)
                {
                    v.Pet.EstadoPerfeito();
                }
                MessageAgregator<MsgCriatureDefeated>.RemoveListener(OnCriatureDefeated);

            }
        }

        private void OnRequestFightByExternal(MsgRequestExternalFightStart obj)
        {
            if (obj.fighters == transform)
            {
                FuncaoDoBotao();
            }
        }

        protected override void Update()
        {
            base.Update();

            switch (state)
            {
                case ThisState.conversaAntesDaLuta:
                    #region conversa antes da luta
                    if (npcAntesDaLuta.Update(Commands.confirmButton, Commands.returnButton))
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
                    if (!DisplayTextManager.instance.DisplayText.LendoMensagemAteOCheia(Commands.confirmButton))
                    {
                        YesOrNoMenu.instance.Menu.StartHud(YesOrNoResponse,
                            TextBank.RetornaListaDeTextoDoIdioma(TextKey.simOuNao).ToArray());
                        state = ThisState.menuAberto;
                    }
                    #endregion
                    break;
                case ThisState.menuAberto:
                    YesOrNoMenu.instance.Menu.ChangeOption(Commands.vChange);

                    if (Commands.confirmButton)
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
                    if (DisplayTextManager.instance.DisplayText.UpdateTexts(Commands.confirmButton, false, conversa))
                    {
                        MessageAgregator<MsgStartMusic>.Publish(new MsgStartMusic()
                        {
                            nmcvc = musicaDaLuta
                        });

                        animaB = new AnimateArm(MeuTransform, Manager.transform, true);
                        Transform aux = Manager.ActivePet.transform;
                        animaB.PosCriature = aux.position + 3 * aux.forward;

                        state = ThisState.animandoBraco;
                        CameraApplicator.cam.OffCamera();
                    }
                    break;
                #endregion
                case ThisState.animandoBraco:
                    MessageAgregator<ChangeMoveSpeedMessage>.Publish(new ChangeMoveSpeedMessage()
                    {
                        velocity = Vector3.zero,
                        gameObject = gameObject
                    });

                    #region animando braco
                    if (!animaB.AnimaEnvia(criaturesDoTreinador[indiceDoEnviado].Pet, "criatureDeTreinador"))
                    {
                        myActivePet = GameObject.Find("criatureDeTreinador").GetComponent<PetManager>();
                        
                        myActivePet = criaturesDoTreinador[indiceDoEnviado].PrepararInicioDoCriature(myActivePet);
                        
                        

                        PetFeatures P = myActivePet.MeuCriatureBase.PetFeat;
                        MessageAgregator<MsgRequestUpperLargeMessage>.Publish(new MsgRequestUpperLargeMessage()
                        {
                            message = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDaLutaContraTreinador)[4],
                            GetComponent<NpcAppearanceConfiguration>().Sid,
                            P.mNivel.Nivel,
                            P.meusAtributos.PV.Corrente,
                            P.meusAtributos.PE.Corrente,
                            myActivePet.MeuCriatureBase.GetNomeEmLinguas
                            )
                        });
                        CameraApplicator.cam.StartExibitionCam(myActivePet.GetComponent<CharacterController>(),true);
                        tempoDecorrido = 0;
                        state = ThisState.showTrainerPet;
                    }
                    #endregion
                break;

                case ThisState.showTrainerPet:
                    tempoDecorrido += Time.deltaTime;
                    if (Commands.confirmButton || tempoDecorrido > 10)
                    {
                        ((PetManagerEnemy)myActivePet).StartAgressiveIa(Manager.ActivePet.gameObject);

                        MessageAgregator<MsgRequestHideUpperLargeMessage>.Publish();

                        MessageAgregator<MsgCriatureDefeated>.AddListener(OnCriatureDefeated);
                        
                        MessageAgregator<MsgRequestChangeToPetByReplace>.Publish(new MsgRequestChangeToPetByReplace()
                        {
                            dono = Manager.gameObject,
                            fluxo = FluxoDeRetorno.criature,
                            lockTarget = myActivePet.transform

                        });
                        ((PetManagerCharacter)Manager.ActivePet).ControlableVsTrainer(myActivePet.transform);

                        state = ThisState.leituraDeLuta;
                    }
                break;
                case ThisState.fraseDeVitoria:
                    if (DisplayTextManager.instance.DisplayText.UpdateTexts(Commands.confirmButton, Commands.returnButton, conversa))
                    {
                        VerificaContinuidadeDaBatalha();
                    }
                break;
                case ThisState.novoJogoDeCamera:
                    if (CameraApplicator.cam.FocusInPoint(height: 3))
                    {
                        Destroy(myActivePet.gameObject);
                        state = ThisState.frasePreInicio; 
                    }
                break;
                case ThisState.fraseDaFinalizacao:
                    #region frase da finalizacao
                    if (npcAoGanharLuta.Update(Commands.confirmButton, Commands.returnButton))
                    {

                        OnDefeatedTrainer();

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
                    if (Commands.confirmButton)
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
                    FayvitSave.SaveDatesManager.SalvarAtualizandoDados(new Criatures2021.CriaturesSaveDates());
                    VoltarAoModoPasseio();
                break;
                case ThisState.conversaDeLutaGanha:
                    if (npcDepoisDaLutaGanha.Update(Commands.confirmButton, Commands.returnButton))
                    {
                        VoltarAoModoPasseio();
                    }
                break;
            }
        }

        //private void OnPlayerChangeToPet(MsgChangeToPet obj)
        //{
        //    Debug.Log("manager: " + Manager+" MyGlobalmainpla: "+MyGlobalController.MainPlayer+" activepet: "+MyGlobalController.MainPlayer.ActivePet);
        //    if (myActivePet.MeuCriatureBase.PetFeat.meusAtributos.PV.Corrente <= 0)
        //    {
        //        SupportSingleton.Instance.InvokeOnCountFrame(() =>
        //        {
        //            MessageAgregator<MsgCriatureDefeated>.Publish(new MsgCriatureDefeated()
        //            {
        //                atacker = Manager.ActivePet.gameObject,
        //                defeated = myActivePet.gameObject,
        //                doDerrotado = myActivePet.MeuCriatureBase
        //            });
        //        },3);
        //        //OnCriatureDefeated(new MsgCriatureDefeated()
        //        //{
        //        //    atacker = Manager.ActivePet.gameObject,
        //        //    defeated = myActivePet.gameObject,
        //        //    doDerrotado = myActivePet.MeuCriatureBase
        //        //});
        //    }
        //}

        protected virtual void OnDefeatedTrainer() { }

        protected void VoltarAoModoPasseio()
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
            //Debug.Log("No trainer" + (obj.defeated == myActivePet.gameObject)
            //    + " : " + (obj.atacker.GetComponent<PetManager>().MeuCriatureBase.PetFeat.meusAtributos.PV.Corrente > 0));
            
            if (obj.defeated == myActivePet.gameObject 
                && Manager.ActivePet.MeuCriatureBase.PetFeat.meusAtributos.PV.Corrente>0)
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
                
                Commands = new MsgSendExternalPanelCommand();
                MessageAgregator<MsgStartExternalInteraction>.Publish();
                MessageAgregator<MsgStartMusic>.Publish(new MsgStartMusic()
                {
                    nmcvc = temaDoTreinadorNoCalorDaBatalha
                });

                SupportSingleton.Instance.InvokeOnEndFrame(() =>
                {
                    MessageAgregator<MsgCriatureDefeated>.RemoveListener(OnCriatureDefeated);
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

                Debug.Log("Verificando continuidade da batalha");

                state = ThisState.novoJogoDeCamera;
            }
            else
            {
                state = ThisState.leituraDeLuta;
                AbstractGlobalController.Instance.FadeV.StartFadeOutWithAction(() => {
                    Destroy(GameObject.Find("cilindroEncontro"));
                    Destroy(myActivePet.gameObject);
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
                    //CameraApplicator.cam.ValoresDeCamera(0, 0, true, false);
                    GameObject G = new GameObject();
                    G.transform.position = 0.5f*(npcOriginalPosition+managerOriginalPosition);
                    G.transform.rotation = Quaternion.LookRotation(Vector3.Cross(transform.forward, Vector3.up));
                    CameraApplicator.cam.NewFocusForBasicCam(G.transform, 3, 5,true,true);
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

        protected virtual void ElementosDoEncontro()
        {
            InsereElementosDoEncontro.EncontroDeTreinador(Manager, MeuTransform,tipoDeAfastamento: tipoDeAfast);
        }

        private void IniciarLutaContraTreinador()
        {
            npcOriginalPosition = MeuTransform.position;

            Manager = MyGlobalController.MainPlayer;
            managerOriginalPosition = Manager.transform.position;

            tempoDecorrido = 0;
            CameraApplicator.cam.OffCamera(); /*StartExibitionCam(transform, 1, true);*/
            ElementosDoEncontro();
            //MessageAgregator<MsgStartMusic>.Publish(new MsgStartMusic()
            //{
            //    nmcvc = musicaDaLuta
            //});
            state = ThisState.animacaoDeEncontro;

            MessageAgregator<MsgStartAnimateArmToFight>.Publish(new MsgStartAnimateArmToFight()
            {
                sender = gameObject
            });
        }

        protected virtual void PreparandoConversa()
        {
            FluxoDeBotao();
            Commands = new MsgSendExternalPanelCommand();
            MessageAgregator<MsgSendExternalPanelCommand>.AddListener(OnReceiveCommands);

            Transform player = MyGlobalController.MainCharTransform;
            Vector3 dir = transform.parent.position - player.position;
            CharRotateTo.RotateDir(dir, player.gameObject);
            CharRotateTo.RotateDir(-dir, MeuTransform.gameObject);
            MessageAgregator<MsgStartMusicWithRecovery>.Publish(new MsgStartMusicWithRecovery()
            {
                nmcvc = temaDoTreinador
            });

            state = ThisState.atividadeExterna;
        }

        public override void FuncaoDoBotao()
        {
            PreparandoConversa();

            if (!AbstractGameController.Instance.MyKeys.VerificaAutoShift(chaveDaLuta))
            {   
                npcAntesDaLuta.Start();
                npcAntesDaLuta.IniciaConversa();
                state = ThisState.conversaAntesDaLuta;
                
                //if (chaveDepoisDeFinalizado != "")
                //    conversa = BancoDeTextos.RetornaListaDeTextoDoIdioma(StringParaEnum.ObterEnum(chaveDepoisDeFinalizado, chaveDaFinalizacao)).ToArray();
            }
            else {
                
                npcDepoisDaLutaGanha.Start();
                npcDepoisDaLutaGanha.IniciaConversa();
                state = ThisState.conversaDeLutaGanha;
            }

            //base.IniciaConversa();
        }

        private void OnReceiveCommands(MsgSendExternalPanelCommand obj)
        {
            Commands = obj;
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

        public PetManager PrepararInicioDoCriature(PetManager cm)
        {
            GameObject G = cm.gameObject;
            PetBase P = cm.MeuCriatureBase;
            MonoBehaviour.Destroy(cm);
            
            cm = G.AddComponent<PetManagerTrainer>();
            cm.MeuCriatureBase = P;
            

            //((PetManagerEnemy)cm).StartAgressiveIa(atacante);

            if (golpeDeInspector)
            {
                cm.MeuCriatureBase.GerenteDeGolpes.meusGolpes = new System.Collections.Generic.List<PetAttackBase>();
                foreach (var v in Pet.GerenteDeGolpes.meusGolpes)
                    cm.MeuCriatureBase.GerenteDeGolpes.meusGolpes.Add(AttackFactory.GetAttack(v.Nome));
            }

            if (pvDeInspector)
            {
                cm.MeuCriatureBase.PetFeat.meusAtributos.PV.Maximo = Pet.PetFeat.meusAtributos.PV.Maximo;
                cm.MeuCriatureBase.PetFeat.meusAtributos.PV.Corrente = Pet.PetFeat.meusAtributos.PV.Corrente;
            }

            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                cm.PararCriatureNoLocal();
            });
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