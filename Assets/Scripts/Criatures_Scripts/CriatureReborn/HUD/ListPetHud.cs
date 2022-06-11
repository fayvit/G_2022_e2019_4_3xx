using UnityEngine;
using System.Collections;
using FayvitUI;
using Criatures2021;
using FayvitMessageAgregator;
using FayvitSupportSingleton;

namespace Criatures2021Hud
{
    [System.Serializable]
    public class ListPetHud : InteractiveUiBase
    {
        private PetBase[] listaDeCriatures;
        private System.Action<int> aoClique;
        private bool podeMudar = true;
        private bool armagedom = false;
        private bool estadoDeAcao = false;

        public bool PodeMudar
        {
            get { return podeMudar; }
            set { podeMudar = value; }
        }

        public void StartHud(PetBase[] listaDeCriatures, System.Action<int> AoEscolherUmCriature, bool armagedom = false)
        {
            this.armagedom = armagedom;
            this.listaDeCriatures = listaDeCriatures;
            PodeMudar = true;
            aoClique += (int x) => { 
                if (PodeMudar && !estadoDeAcao)
                {
                    estadoDeAcao = true;
                    ChangeSelectionTo(x);

                    SupportSingleton.Instance.InvokeInRealTime(() =>
                    {
                        Debug.Log("Função chamada com delay para destaque do botão");
                        AoEscolherUmCriature(x);
                        estadoDeAcao = false;
                        Debug.Log("A opção escolhida é: " + SelectedOption);
                    }, .05f);
                }
            };


            StartHud(listaDeCriatures.Length);

            //ActionManager.ModificarAcao(GameController.g.transform, AcaoDeOpcaoEscolhida);
        }

        public void AcaoDeOpcaoEscolhida()
        {
            variableSizeContainer.GetChild(SelectedOption + 1).GetComponent<AnOption>().InvokeAction();
            podeMudar = false;
            Debug.Log("A opção escolhida é: " + SelectedOption);
        }

        public void Update(int val,bool selectOption)
        {
            if (PodeMudar)
            {
                ChangeOption(val);
                if (selectOption)
                    aoClique(SelectedOption);
            }
        }

        public override void SetContainerItem(GameObject G, int indice)
        {
            G.GetComponent<OptionPetForListHud>().SetarCriature(listaDeCriatures[indice], aoClique, armagedom);
        }

        protected override void AfterFinisher()
        {
            aoClique = null;
        }
    }
}