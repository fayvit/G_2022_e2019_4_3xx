using Criatures2021Hud;
using FayvitBasicTools;
using FayvitMessageAgregator;
using UnityEngine;
using TextBankSpace;
using Criatures2021;
using FayvitCam;

namespace TalkSpace
{
    public class TrainerForFight : ButtonActivate
    {
        
        [SerializeField] private string chaveDaLuta;
        [SerializeField] private bool perguntaParaComecar = true;
        [SerializeField] private PetsForBattle[] criaturesDoTreinador;
        [SerializeField] private ScheduledTalkManager npcAntesDaLuta;
        [SerializeField] private TextKey textPerguntaComecar;

        private int indiceDoEnviado = 0;
        private float tempoDecorrido = 0;
        private string[] conversa;
        private ThisState state = ThisState.emEspera;
        private AnimateArm animaB;
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
            menuAberto
        }

        public Transform MeuTransform { get => transform.parent; }
        public CharacterManager Manager{ get; set; }

        private void OnValidate()
        {
            BuscadorDeID.Validate(ref chaveDaLuta, this, "chaveDaLuta");
        }

        private void Start()
        {
            SempreEstaNoTrigger();
        }

        new void Update()
        {
            base.Update();

            switch (state)
            {
                case ThisState.conversaAntesDaLuta:
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
                        }
                        else
                        { 
                        
                        }
                    }
                break;
                case ThisState.perguntaComecarBatalha:
                    if (!DisplayTextManager.instance.DisplayText.LendoMensagemAteOCheia(commands.confirmButton))
                    {
                        YesOrNoMenu.instance.Menu.StartHud(YesOrNoResponse,
                            TextBank.RetornaListaDeTextoDoIdioma(TextKey.simOuNao).ToArray());
                        state = ThisState.menuAberto;
                    }
                break;
                case ThisState.menuAberto:
                    YesOrNoMenu.instance.Menu.ChangeOption(commands.vChange);

                    if (commands.confirmButton)
                        YesOrNoResponse(YesOrNoMenu.instance.Menu.SelectedOption);
                break;
                case ThisState.animacaoDeEncontro:
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
                break;
                case ThisState.cameraNoTreinador:
                    if (CameraApplicator.cam.FocusInPoint(height: 3))
                    {
                        
                        DisplayTextManager.instance.DisplayText.StartTextDisplay();
                        conversa = TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDaLutaContraTreinador).ToArray();
                        conversa = new string[2] {
                        string.Format(conversa[0],criaturesDoTreinador.Length), conversa[1] };
                        state = ThisState.frasePreInicio;
                    }
                break;
                case ThisState.frasePreInicio:
                    if (DisplayTextManager.instance.DisplayText.UpdateTexts(commands.confirmButton,false,conversa))
                    {
                        animaB = new AnimateArm(MeuTransform, Manager.transform, true);
                        Transform aux = Manager.ActivePet.transform;
                        animaB.PosCriature = aux.position + 3 * aux.forward;

                        state = ThisState.animandoBraco;
                        CameraApplicator.cam.OffCamera();
                    }
                    break;
                case ThisState.animandoBraco:
                    if (!animaB.AnimaEnvia(criaturesDoTreinador[indiceDoEnviado].Pet, "criatureDeTreinador"))
                    {
                        PetManager enemy = GameObject.Find("criatureDeTreinador").GetComponent<PetManager>();
                        //GameController.g.EncontroAgoraCom(
                        criaturesDoTreinador[indiceDoEnviado].PrepararInicioDoCriature(enemy,Manager.ActivePet.gameObject);
                        //, true, nomeDoTreinador);

                        ((PetManagerCharacter)Manager.ActivePet).ControlableVsTrainer(enemy.transform);
                        state = ThisState.leituraDeLuta;
                    }
                break;
                case ThisState.leituraDeLuta:
                    //if (GameController.g.InimigoAtivo == null)
                    //{
                    //    indiceDoEnviado++;
                    //    if (indiceDoEnviado < criaturesDoTreinador.Length)
                    //    {
                    //        conversa = BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.frasesDaLutaContraTreinador).ToArray();
                    //        conversa = new string[2] { conversa[2], conversa[3] };
                    //        disparaT.IniciarDisparadorDeTextos();
                    //        AplicadorDeCamera.cam.InicializaCameraExibicionista(MeuTransform, 1, true);
                    //        estadoInterno = EstadoInterno.novoJogoDeCamera;
                    //    }
                    //    else
                    //    {
                    //        AplicadorDeCamera.cam.InicializaCameraExibicionista(MeuTransform, 1, true);
                    //        conversa = StringParaEnum.SetarConversaOriginal(chaveDaFinalizacaoString, ref chaveDaFinalizacao);
                    //        disparaT.IniciarDisparadorDeTextos();
                    //        estadoInterno = EstadoInterno.fraseDaFinalizacao;
                    //    }
                    //}
                break;
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
                break;
            }

            YesOrNoMenu.instance.Menu.FinishHud();
        }

        private void IniciarLutaContraTreinador()
        {
            Manager = AbstractGlobalController.Instance.Players[0].Manager.transform.GetComponent<CharacterManager>();
            tempoDecorrido = 0;
            CameraApplicator.cam.StartExibitionCam(transform, 1, true);
            InsereElementosDoEncontro.EncontroDeTreinador(Manager, MeuTransform);
            state = ThisState.animacaoDeEncontro;
        }

        public override void FuncaoDoBotao()
        {
            if (!AbstractGameController.Instance.MyKeys.VerificaAutoShift(chaveDaLuta))
            {
                FluxoDeBotao();
                npcAntesDaLuta.Start();
                npcAntesDaLuta.IniciaConversa();
                state = ThisState.conversaAntesDaLuta;
                commands = new MsgSendExternalPanelCommand();
                MessageAgregator<MsgSendExternalPanelCommand>.AddListener(OnReceiveCommands);
                //if (chaveDepoisDeFinalizado != "")
                //    conversa = BancoDeTextos.RetornaListaDeTextoDoIdioma(StringParaEnum.ObterEnum(chaveDepoisDeFinalizado, chaveDaFinalizacao)).ToArray();
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
            ((PetManagerEnemy)cm).StartAgressiveIa(atacante);

            if (golpeDeInspector)
                cm.MeuCriatureBase.GerenteDeGolpes.meusGolpes = Pet.GerenteDeGolpes.meusGolpes;

            if (pvDeInspector)
            {
                cm.MeuCriatureBase.PetFeat.meusAtributos.PV.Maximo = Pet.PetFeat.meusAtributos.PV.Maximo;
                cm.MeuCriatureBase.PetFeat.meusAtributos.PV.Corrente = Pet.PetFeat.meusAtributos.PV.Corrente;
            }

            //GameController.g.EncontroAgoraCom(cm);
            return cm;
        }
    }
}