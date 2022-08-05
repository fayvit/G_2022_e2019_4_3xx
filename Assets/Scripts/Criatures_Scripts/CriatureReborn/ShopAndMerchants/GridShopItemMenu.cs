using UnityEngine;
using FayvitUI;
using Criatures2021;
using System.Collections.Generic;

namespace Criatures2021Hud
{
    [System.Serializable]
    public class GridShopItemMenu : BaseGridMenu
    {

        private ItensParaVenda[] itensForMenu;

        private string IddoShop;
        //private List<ItemBase> itensDoJogador;



        public void StartHud(string IddoShop,System.Action<int> acaoDeFora, ItensParaVenda[] paraVenda/*, List<ItemBase> itensDoJogador*/,int selecionado = 0)
        {
            //this.itensDoJogador = itensDoJogador;
            this.IddoShop = IddoShop;
            ThisAction += acaoDeFora;
            itensForMenu = paraVenda;

            if (paraVenda.Length > 0)
                StartHud(paraVenda.Length, ResizeUiType.grid, selectIndex: selecionado);
            else
                aContainerItem.SetActive(false);
            variableSizeContainer.pivot = Vector2.one;
            

            SetLineRowLength();


        }

        public override void SetContainerItem(GameObject G, int indice)
        {
            A_ShopOption uma = G.GetComponent<A_ShopOption>();

            Sprite S = ResourcesFolders.GetMiniItem(itensForMenu[indice].nomeDoItem);
            //ItemBase.ProcuraItemNaLista(itensForMenu[indice].nomeDoItem, itensDoJogador);


            uma.SetarOpcoes(itensForMenu[indice],IddoShop=="venda"?IddoShop: IddoShop+"item"+indice,ThisAction);

        }
    }
}

