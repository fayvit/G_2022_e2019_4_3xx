using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace FayvitUI
{
    [System.Serializable]
    public class GridMenu : BaseGridMenu
    {
        private Sprite[] spritesForGridMenu;

        public void StartHud(System.Action<int> acaoDeFora, Sprite[] sprites)
        {
            ThisAction += ActionToDelayButton(acaoDeFora);
            spritesForGridMenu = sprites;

            if (sprites.Length > 0)
                StartHud(sprites.Length, ResizeUiType.grid);
            else
                aContainerItem.SetActive(false);

            SetLineRowLength();

        }

        public override void SetContainerItem(GameObject G, int indice)
        {
            AnImageOption uma = G.GetComponent<AnImageOption>();

            Sprite S = spritesForGridMenu[indice];

            uma.SetarOpcoes(S, ThisAction);

        }
    }
}
