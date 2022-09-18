using UnityEngine;
using FayvitMessageAgregator;

namespace FayvitUI
{
    [System.Serializable]
    public class TextAndImageMenu : InteractiveUiBase
    {
        private Sprite[] imgOptions;
        private System.Action<int> acao;

        public string[] Opcoes { get; private set; }

        public string LabelOfSelectedOption
        {
            get => Opcoes[SelectedOption];
        }        

        public void StartHud(
            System.Action<int> acao,
            string[] txDeOpcoes,
            Sprite[] imgOptions,
            ResizeUiType tipoDeR = ResizeUiType.vertical,
            int selectIndex = 0,
            PivotPosition pivotPos = PivotPosition.@default
            )
        {
            this.imgOptions = imgOptions;
            this.Opcoes = txDeOpcoes;
            this.acao += ActionToDelayButton(acao);

            StartHud(Opcoes.Length, tipoDeR, selectIndex);

            if (pivotPos == PivotPosition.leftUp)
            {
                variableSizeContainer.pivot = Vector2.one;
            }
        }

        public override void SetContainerItem(GameObject G, int indice)
        {
            AnImageAndTextOption uma = G.GetComponent<AnImageAndTextOption>();
            uma.SetarOpcao(acao, Opcoes[indice], imgOptions[indice]);
        }

        protected override void AfterFinisher()
        {
            acao = null;
            //Seria preciso uma finalização especifica??
        }


    }

    public enum PivotPosition
    { 
        @default,
        leftUp
    }
}

