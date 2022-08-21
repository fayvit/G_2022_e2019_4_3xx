using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Criatures2021;

namespace Criatures2021Hud
{
    public class ShopHudManager : MonoBehaviour
    {
        [SerializeField] private Text cristaisCount;
        [SerializeField] private GameObject container;
        [SerializeField] private ShopShowItemManager showItem;
        [SerializeField] private ShowNewAttackHud showAttack;        
        [SerializeField] private GridShopItemMenu shopMenu;

        private bool venda;
        private string IDdoChop;
        private List<ItemBase> itensDoJogador;
        private ItensParaVenda[] osItensParaVenda;

        public static ShopHudManager instance;

        public int SelectedOption => shopMenu.SelectedOption;

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
        }

        internal void StartHudVenda(System.Action<int> onChoiseSale, int cristais, List<ItemBase> itens)
        {
            this.itensDoJogador = itens;
            osItensParaVenda = new ItensParaVenda[itens.Count];
            for (int i = 0; i < itens.Count; i++)
            {
                osItensParaVenda[i] = new ItensParaVenda()
                {
                    limitado = itens[i].Estoque,
                    nomeDoItem = itens[i].ID,
                    precoCustomizado = itens[i].Valor / 2
                };
            }

            container.SetActive(true);
            cristaisCount.text = cristais.ToString();
            shopMenu.StartHud("venda", onChoiseSale, osItensParaVenda);
            venda = true;
            ColocaInformacoesDeItens(0);


        }

        public void StartHudCompra(
            string ID,
            System.Action<int> acao,
            int cristalCount,
            ItensParaVenda[] osItensParaVenda,
            List<ItemBase> itensDoJogador,
            int selecionado=0)
        {
            IDdoChop = ID;
            this.itensDoJogador = itensDoJogador;
            this.osItensParaVenda = osItensParaVenda;

            container.SetActive(true);
            cristaisCount.text = cristalCount.ToString();
            shopMenu.StartHud(ID,acao, osItensParaVenda,selecionado:selecionado);
            ColocaInformacoesDeItens(0);

        }

        void ColocaInformacoesDeItens(int indice)
        {
            ItemBase I = ItemBase.ProcuraItemNaLista(osItensParaVenda[indice].nomeDoItem, itensDoJogador);
            showItem.FillDates(osItensParaVenda[indice].nomeDoItem, 
                venda? osItensParaVenda[indice].limitado:MyGameController.Instance.MyKeys.VerificaAutoCont(IDdoChop+"item"+indice)
                , I.ID == NameIdItem.generico ? 0 : I.Estoque);

            ItemBase Ib = ItemFactory.Get(osItensParaVenda[indice].nomeDoItem);
            //showAttack.EndHud();

            Debug.Log("caso seja item para aprender ataque alterar aqui: "+Ib.Item_Nature);
            if (Ib.Item_Nature == ItemNature.pergGolpe)
                showAttack.Start(AttackFactory.GetAttack(((AttackLearnItem)Ib).GolpeDoPergaminho[0]), 0);
            else
                showAttack.EndHud();
        }

        internal void ChangeOption(int vChange,int hChange)
        {
            shopMenu.ChangeOption(vChange,hChange);
            ColocaInformacoesDeItens(shopMenu.SelectedOption);
        }

        public void ChangeInteractiveButtons(bool interactive)
        {
            shopMenu.ChangeInteractiveButtons(interactive);
        }

        public void FinishHud()
        {
            container.SetActive(false);
            shopMenu.FinishHud();
            //showItem.FinishHud();
            showAttack.EndHud();
        }

        
    }
}