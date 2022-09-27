using UnityEngine;
using Criatures2021;
using System.Collections.Generic;
using FayvitUI;
using FayvitSupportSingleton;

namespace Criatures2021Hud
{
    [System.Serializable]
    public class PetMenu : InteractiveUiBase
    {
        private System.Action<int> localAction;
        private List<PetBase> pets;

        public void StartHud(System.Action<int> thisAction, List<PetBase> pets,int selectedOption = 0)
        {
            localAction += ActionToDelayButton(thisAction);

            this.pets = pets;

            StartHud(pets.Count,selectIndex: selectedOption);
        }

        public override void SetContainerItem(GameObject G, int indice)
        {
            G.GetComponent<A_PetMenuOption>().SetarOpcoes(pets[indice], localAction);
        }

        protected override void AfterFinisher()
        {
            localAction = null;
        }
    }
}
