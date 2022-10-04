using Criatures2021;
using UnityEngine;
using UnityEngine.UI;

namespace Criatures2021Hud
{
    public class A_ShopOption : AnItemOption
    {
        [SerializeField] private Text preco;
        public void SetarOpcoes(ItensParaVenda I,string keyCOnt,System.Action<int> A)
        {
            //Debug.Log(MyGameController.Instance.MyKeys.VerificaAutoCont(keyCOnt) + " : " + I.limitado + " : " + I.nomeDoItem);

            ItemBase Ib = ItemFactory.Get(I.nomeDoItem);
            

            this.preco.text = I.precoCustomizado>0?I.precoCustomizado.ToString():Ib.Valor.ToString();
            Debug.Log(I.nomeDoItem + " : " + I.limitado + " : " + I.precoCustomizado);
            

            Ib.Estoque = I.limitado > 0 ? (keyCOnt=="venda"?I.limitado: MyGameController.Instance.MyKeys.VerificaAutoCont(keyCOnt)) : 1;
            Ib.NosItensRapidos = false;
            
            base.SetarOpcoes(Ib, A);

            AmountContainer.SetActive(I.limitado >= 0);
        }
    }
}