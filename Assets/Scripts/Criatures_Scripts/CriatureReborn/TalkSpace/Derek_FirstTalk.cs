using FayvitBasicTools;
using UnityEngine;

namespace TalkSpace
{
    class Derek_FirstTalk: TrainerForFight
    {
        [SerializeField] private ScheduledTalkManager npcDerekParaConversarPrimeiro;

        private LocalState myState = LocalState.standardUpdate;

        private enum LocalState
        { 
            standardUpdate,
            conversandoComDerek
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            npcDerekParaConversarPrimeiro.OnVallidate();
        }

        protected override void OnDefeatedTrainer()
        {
            AbstractGameController.Instance.MyKeys.MudaShift(KeyShift.venceuDerekPrimeiraVez,true);
        }

        protected override void Update()
        {
            if (myState == LocalState.standardUpdate)
                base.Update();
            else
            {
                if (npcDerekParaConversarPrimeiro.Update(Commands.confirmButton, Commands.returnButton))
                {
                    myState = LocalState.standardUpdate;
                    VoltarAoModoPasseio();
                }
            }
        }

        public override void FuncaoDoBotao()
        {
            KeyVar keys = AbstractGameController.Instance.MyKeys;
            if (!keys.VerificaAutoShift(KeyShift.conversouPrimeiroComIan))
            {
                npcDerekParaConversarPrimeiro.Start(gameObject);
                npcDerekParaConversarPrimeiro.IniciaConversa();
                myState = LocalState.conversandoComDerek;
                PreparandoConversa();
            }
            else
            {
                base.FuncaoDoBotao();
            }
            

            keys.MudaShift(KeyShift.conversouPrimeiroComDerek, true);
            

        }
    }
}
