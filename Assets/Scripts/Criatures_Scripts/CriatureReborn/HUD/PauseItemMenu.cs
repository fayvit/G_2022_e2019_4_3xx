using UnityEngine;
using FayvitUI;
using Criatures2021;

namespace Criatures2021Hud
{
    [System.Serializable]
    public class PauseItemMenu : BaseGridMenu
    {

        private ItemBase[] itensForMenu;

        public void StartHud(System.Action<int> acaoDeFora, ItemBase[] sprites,int selecionado = 0)
        {
            ThisAction += acaoDeFora;
            itensForMenu = sprites;

            if (sprites.Length > 0)
                StartHud(sprites.Length, ResizeUiType.grid,selectIndex: selecionado);
            else
                aContainerItem.SetActive(false);
            variableSizeContainer.pivot = Vector2.one;
            //variableSizeContainer.anchoredPosition = new Vector2(0, -0.5f*variableSizeContainer.sizeDelta.y);

            //Rect r = variableSizeContainer.rect;
            //Debug.Log(r + " : " + r.yMin + " : " + r.yMax + " : " + r.top);

            SetLineRowLength();

            
        }

        public override void SetContainerItem(GameObject G, int indice)
        {
            AnItemOption uma = G.GetComponent<AnItemOption>();

            Sprite S = ResourcesFolders.GetMiniItem(itensForMenu[indice].ID);

            uma.SetarOpcoes(itensForMenu[indice], ThisAction);

        }
    }
}

