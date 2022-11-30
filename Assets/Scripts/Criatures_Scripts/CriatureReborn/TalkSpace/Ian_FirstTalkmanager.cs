using Criatures2021;
using FayvitBasicTools;
using FayvitMessageAgregator;
using FayvitSupportSingleton;
using System;
using TextBankSpace;
using UnityEngine;

namespace TalkSpace
{
    class Ian_FirstTalkmanager : TalkButtonActivate
    {
        [SerializeField] private string ID;
        [SerializeField] private ScheduledTalkManager npcIanInicial;
        [SerializeField] private ScheduledTalkManager npcIanParaEntregarCaneta;
        [SerializeField] private ScheduledTalkManager ianFalouPrimeiroComDerek;

        //private LocalState state = LocalState.emEspera;

        //private enum LocalState
        //{ 
        //    emEspera,
        //    conversaComum
        //}

        new void Start()
        {
            
            KeyVar myKeys = AbstractGameController.Instance.MyKeys;
            
            myKeys.MudaShift(KeyShift.sempretrue, true);
            myKeys.MudaAutoShift(string.Empty, true);
            textoDoBotao = TextBank.RetornaFraseDoIdioma(TextKey.textoBaseDeAcao);

            base.NPC = npcIanInicial;
            npcIanInicial.GO_Reference = gameObject;

            base.Start();
        }

        private void OnValidate()
        {
            BuscadorDeID.Validate(ref ID, this);
            npcIanInicial.OnVallidate();
            npcIanParaEntregarCaneta.OnVallidate();
            ianFalouPrimeiroComDerek.OnVallidate();
        }

        //protected override void Update()
        //{
        //    switch (state)
        //    {
        //        case LocalState.emEspera:
        //        case LocalState.conversaComum:
        //            base.Update();
        //        break;
        //    }
        //}



        public override void FuncaoDoBotao()
        {
            SomDoIniciar();

            MessageAgregator<MsgChangeShiftKey>.Publish(new MsgChangeShiftKey() { change = true, sKey = ID });

            KeyVar keys = AbstractGameController.Instance.MyKeys;

            keys.MudaShift(KeyShift.conversouPrimeiroComIan, true);

            //if(true
            if (keys.VerificaAutoShift(KeyShift.conversouPrimeiroComDerek)
                &&
                keys.VerificaAutoShift(KeyShift.venceuDerekPrimeiraVez)
                &&
                !keys.VerificaAutoShift(KeyShift.entregouCanetaDeIan)
                )
            {
                NPC = npcIanParaEntregarCaneta;
                
                MessageAgregator<MsgChangeShiftKey>.Publish(new MsgChangeShiftKey() { change = true, key = KeyShift.entregouCanetaDeIan });
                MessageAgregator<MsgFinishTextDisplay>.AddListener(OnFinishTextDisplay);

            }
            else if (keys.VerificaAutoShift(KeyShift.conversouPrimeiroComDerek) && !keys.VerificaAutoShift(KeyShift.venceuDerekPrimeiraVez))
            {
                NPC = ianFalouPrimeiroComDerek;
            }
            else
            {
                NPC = npcIanInicial;

            }

            NPC.Start(gameObject);
            NPC.IniciaConversa();


            base.FuncaoDoBotao();
        }

        private void OnFinishTextDisplay(MsgFinishTextDisplay obj)
        {
            //Debug.Log("os elementos do evento: "+obj.sender + " : " + gameObject + " : " + obj.talkKey);
            if (obj.sender == gameObject && obj.talkKey == TextKey.IanComCaneta)
            {
                MessageAgregator<MsgGetLogPen>.Publish();
                SupportSingleton.Instance.InvokeOnEndFrame(() =>
                {
                    MessageAgregator<MsgFinishTextDisplay>.RemoveListener(OnFinishTextDisplay);
                });
                OnFinishTalk();
            }
        }
    }
}
