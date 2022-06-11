using UnityEngine;
using FayvitBasicTools;
using TextBankSpace;
using FayvitMessageAgregator;

namespace TalkSpace
{
    [System.Serializable]
    public class ScheduledTalkManager : TalkManagerBase
    {
        #region inspector
        [SerializeField] private FalasAgendaveis[] falas = null;
        #endregion

        private int ultimoIndice = -1;

        [System.Serializable]
        private class FalasAgendaveis
        {
            [SerializeField] private string stringCondicionalDaConversa;
            [SerializeField] private KeyShift chaveCondicionalDaConversa=KeyShift.sempretrue;            
            [SerializeField] private TextKey chaveDeTextoDaConversa;
            [SerializeField] private int repetir = 0;

            public string StringCondicionalDaConversa { 
                get => stringCondicionalDaConversa; 
                set => stringCondicionalDaConversa = value; 
            }

            public KeyShift ChaveCondicionalDaConversa
            {
                
                get { return chaveCondicionalDaConversa; }
                set { chaveCondicionalDaConversa = value; }
            }

            public TextKey ChaveDeTextoDaConversa
            {
                get { return chaveDeTextoDaConversa; }
                set { chaveDeTextoDaConversa = value; }
            }

            public int Repetir { get { return repetir; } set { repetir = value; } }
        }

        void VerificaQualFala()
        {
            KeyVar myKeys = AbstractGameController.Instance.MyKeys;

            Debug.Log("ultimo indice no inicio: " + ultimoIndice);


            int indiceFinal = ultimoIndice > 0 ? Mathf.Min(ultimoIndice, falas.Length) : falas.Length;


            for (int i = 0; i < indiceFinal; i++)
            {
                if (myKeys.VerificaAutoShift(falas[i].ChaveCondicionalDaConversa)
                    &&
                    myKeys.VerificaAutoShift(falas[i].StringCondicionalDaConversa)
                    )
                {

                    conversa = TextBank.RetornaListaDeTextoDoIdioma(falas[i].ChaveDeTextoDaConversa).ToArray();
                    ultimoIndice = i;
                }
            }

            Debug.Log(indiceFinal + " : " + ultimoIndice);

            if (falas[ultimoIndice].Repetir >= 0)
            {
                string kCont = falas[ultimoIndice].ChaveCondicionalDaConversa.ToString()+ falas[ultimoIndice].StringCondicionalDaConversa;

                myKeys.SomaAutoCont(kCont, 1);
                if (falas[ultimoIndice].Repetir < myKeys.VerificaAutoCont(kCont))
                {
                    if(falas[ultimoIndice].ChaveCondicionalDaConversa!=KeyShift.sempretrue)
                        myKeys.MudaShift(falas[ultimoIndice].ChaveCondicionalDaConversa, false);

                    if (!string.IsNullOrEmpty(falas[ultimoIndice].StringCondicionalDaConversa))
                        myKeys.MudaAutoShift(falas[ultimoIndice].StringCondicionalDaConversa, false);

                }

            }

        }

        override public void IniciaConversa()
        {
            VerificaQualFala();
            base.IniciaConversa();
        }
    }
}