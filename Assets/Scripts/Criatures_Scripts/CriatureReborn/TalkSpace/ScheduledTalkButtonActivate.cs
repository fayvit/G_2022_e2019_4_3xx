using UnityEngine;
using System.Collections;
using FayvitBasicTools;
using FayvitMessageAgregator;
using TextBankSpace;

namespace TalkSpace
{
    public class ScheduledTalkButtonActivate : TalkButtonActivate
    {
        [SerializeField] private string ID;

        #region inspector
        [SerializeField] private ScheduledTalkManager esseNpc = null;
        //[SerializeField] private KeyShift[] colocarTrue = null;
        //[SerializeField] private ColocarTrueCondicional[] colocarTrueCondicional = null;
        //[SerializeField] private KeyShift[] condicoesComplementares = null;
        #endregion

        //[System.Serializable]
        //public struct ColocarTrueCondicional
        //{
        //    public KeyShift condicao;
        //    public KeyShift alvo;
        //}

        // Use this for initialization
        new void Start()
        {

            KeyVar myKeys = AbstractGameController.Instance.MyKeys;
            //if (!myKeys.VerificaAutoShift(ID))
            //{
            //    for (int i = 0; i < colocarTrue.Length; i++)
            //    {
            //        myKeys.MudaShift(colocarTrue[i], true);
            //    }
            //}

            //if (colocarTrueCondicional != null)
            //    for (int i = 0; i < colocarTrueCondicional.Length; i++)
            //    {
            //        if (!myKeys.VerificaAutoShift(colocarTrueCondicional[i].condicao))
            //            myKeys.MudaShift(colocarTrueCondicional[i].alvo, true);
            //    }

            //myKeys.MudaAutoShift(ID, true);// Herika buga com esse mudaShift aqui
            myKeys.MudaShift(KeyShift.sempretrue, true);
            myKeys.MudaAutoShift(string.Empty, true);
            textoDoBotao = TextBank.RetornaFraseDoIdioma(TextKey.textoBaseDeAcao);

            NPC = esseNpc;
            esseNpc.GO_Reference = gameObject;
            base.Start();
        }

        private void OnValidate()
        {
            BuscadorDeID.Validate(ref ID, this);
        }

        public override void FuncaoDoBotao()
        {
            SomDoIniciar();
            //if (condicoesComplementares != null)
            //    for (int i = 0; i < condicoesComplementares.Length; i++)
            //        AbstractGameController.Instance.MyKeys.MudaShift(condicoesComplementares[i], true);

            MessageAgregator<MsgChangeShiftKey>.Publish(new MsgChangeShiftKey() { change = true, sKey = ID });
            
            base.FuncaoDoBotao();
        }
    }
}