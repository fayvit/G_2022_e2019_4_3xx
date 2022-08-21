using UnityEngine;
using FayvitMessageAgregator;

namespace FayvitUI
{
    [System.Serializable]
    public class BasicMenu : InteractiveUiBase
    {
        private System.Action<int> acao;

        public string[] Opcoes { get; private set; }

        public string LabelOfSelectedOption
        {
            get =>Opcoes[SelectedOption]; 
        }

        public void StartHud(
            System.Action<int> acao,
            string[] txDeOpcoes,
            ResizeUiType tipoDeR = ResizeUiType.vertical,
            int selectIndex = 0
            )
        {
            this.Opcoes = txDeOpcoes;
            this.acao += ActionToDelayButton(acao);

            #region suprimido
            //this.acao += (int x) =>
            //{
            //    if (!estadoDeAcao)
            //    {
            //        estadoDeAcao = true;

            //        SupportSingleton.Instance.InvokeInRealTime(() =>
            //        {
            //            Debug.Log("Função chamada com delay para destaque do botão");
            //            acao(x);
            //            ChangeSelectionTo(x);
            //            estadoDeAcao = false;
            //        }, .05f);
            //    }
            //};
            #endregion

            StartHud(Opcoes.Length, tipoDeR,selectIndex);
        }

        public override void SetContainerItem(GameObject G, int indice)
        {
            A_MenuOption uma = G.GetComponent<A_MenuOption>();
            uma.SetarOpcao(acao, Opcoes[indice]);
        }

        public void ThisUpdate(int change, bool confirmButton)
        {
            if(change!=0)
                ChangeOption(change);
            else
            if (confirmButton)
                acao(SelectedOption);
        }

        protected override void AfterFinisher()
        {
            acao = null;
            //Seria preciso uma finalização especifica??
        }
    }

    public struct MsgPositiveUiInput : IMessageBase { }
    public struct MsgNegativeUiInput : IMessageBase { }
    
}
