using Criatures2021Hud;
using FayvitBasicTools;
using FayvitMessageAgregator;
using FayvitSounds;
using UnityEngine;
using TextBankSpace;


namespace Criatures2021
{
    public abstract class SimpleBarrierBase: EventoComGolpe
    {
        [SerializeField] private int indiceDaMensagem = 0;
        [SerializeField] private float tempoDeEfetivaAcao = 2.5f;
        [SerializeField] private SoundEffectID somDaFinalizacao;
        [SerializeField] private SoundEffectID vinhetaDaFinalizacao = SoundEffectID.vinhetinhaMedia;

        private BarrierEventsState estado = BarrierEventsState.emEspera;
        private bool jaIniciaou = false;
        private float tempoDecorrido = 0;

        protected MsgSendExternalPanelCommand externalCommand = new MsgSendExternalPanelCommand();

        public SoundEffectID VinhetaDaFinalizacao => vinhetaDaFinalizacao;
        public SoundEffectID SomDaFinalizacao => somDaFinalizacao;

        protected enum BarrierEventsState
        {
            emEspera,
            mensAberta,
            ativou,
            barrasDescendo,
            apresentaFinalizaAcao
        }

        public float TempoDeEfetivaAcao
        {
            get { return tempoDeEfetivaAcao; }
        }

        protected float TempoDecorrido
        {
            get { return tempoDecorrido; }
            set { tempoDecorrido = value; }
        }

        protected bool JaIniciaou
        {
            get { return jaIniciaou; }
            set { jaIniciaou = value; }
        }

        protected BarrierEventsState Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        protected void Start()
        {
            textoDoBotao = TextBank.RetornaListaDeTextoDoIdioma(TextKey.textoBaseDeAcao)[1];
            if (StaticInstanceExistence<IGameController>.SchelduleExistence(Start, this, () => { return AbstractGameController.Instance; }))
            {
                if (AbstractGameController.Instance.MyKeys.VerificaAutoShift(Chave))
                    gameObject.SetActive(false);

                JaIniciaou = true;

                MessageAgregator<MsgSendExternalPanelCommand>.AddListener(OnReceiveCommands);
            }


            SempreEstaNoTrigger();
            
        }

        protected void OnDestroy()
        {
            MessageAgregator<MsgSendExternalPanelCommand>.RemoveListener(OnReceiveCommands);
        }

        private void OnReceiveCommands(MsgSendExternalPanelCommand obj)
        {
            if (Estado != BarrierEventsState.emEspera)
            {
                externalCommand = obj;
            }
        }

        public override void FuncaoDoBotao()
        {
            BotaoInfo();
        }

        public override void DisparaEvento(AttackNameId nomeDoGolpe)
        {
            //Debug.Log(nomeDoGolpe+" : "+ GameController.g.MyKeys.VerificaAutoShift(Chave)+" : "+
            //GameController.g.MyKeys.VerificaAutoShift(ChaveEspecial));

            if (EsseGolpeAtiva(nomeDoGolpe))
                estado = BarrierEventsState.ativou;



            if (estado == BarrierEventsState.ativou)
            {
                FluxoDeBotao();
                //acaoEfetivada.SetActive(true);
                EfetivadorDaAcao();
                tempoDecorrido = 0;
                AbstractGameController.Instance.MyKeys.MudaAutoShift(Chave, true);
            }
        }
        public void BotaoInfo()
        {
            SomDoIniciar();
            FluxoDeBotao();
            externalCommand = new MsgSendExternalPanelCommand();
            MessageAgregator<MsgRequestUpperLargeMessage>.Publish(new MsgRequestUpperLargeMessage()
            {
                message = TextBank.RetornaListaDeTextoDoIdioma(TextKey.barreirasDeGolpes)[indiceDaMensagem]
            });
            MessageAgregator<MsgStartExternalInteraction>.Publish();


            estado = BarrierEventsState.mensAberta;
        }

        public void AcaoDeMensAberta()
        {
            Estado = BarrierEventsState.emEspera;
            MessageAgregator<MsgRequestHideUpperLargeMessage>.Publish();
            //GameController.g.HudM.Painel.EsconderMensagem();

            VoltarAoFLuxoDeJogo();

        }
        protected void VoltarAoFLuxoDeJogo()
        {
            MessageAgregator<MsgFinishExternalInteraction>.Publish();
        }


        protected abstract void EfetivadorDaAcao();
    }
}
