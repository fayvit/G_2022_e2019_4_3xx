using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class MacaItem:RecoveryItem
    {
        public MacaItem(int estoque = 1) : base(new ItemFeatures(NameIdItem.maca)
        {
            valor = 40
        }
                )
        {
            Estoque = estoque;
            valorDeRecuperacao = 40;
        }
    }
}