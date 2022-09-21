using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Criatures2021;

namespace Criatures2021Hud
{
    public class ShopShowItemManager : ShowItemHudManager
    {
        [SerializeField] private Text quantidadeJogador;

        //new void Start() { }
        //new void OnDestroy() { }

        public void FillDates(NameIdItem nomeId,int quantidadeDoMercador,int quantidadeDoJogador)
        {
            base.FillDates(new MsgShowItem()
            {
                idItem = nomeId,
                quantidade = quantidadeDoMercador
            },"Você tem:");

            
            numItens.gameObject.SetActive(quantidadeDoMercador>=0);
            this.quantidadeJogador.text = quantidadeDoJogador.ToString();
            labelIntroduction.transform.parent.gameObject.SetActive(true);
        }
    }
}