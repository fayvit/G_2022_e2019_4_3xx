
using UnityEngine;

namespace Criatures2021
{
    [System.Serializable]
    public class BrasaoDeLaurense : UnusableItem
    {
        public BrasaoDeLaurense(int estoque = 1) : base(new ItemFeatures()
        {
            consumivel = false,
            itemNature = ItemNature.usoParticular,
            NomeID = NameIdItem.brasaoDeLaurense,
            nosItensRapidos = false,
            valor = 8
        })
        {
            Estoque = estoque;
        }
    }

    [System.Serializable]
    public class BrasaoDeAnanda : UnusableItem
    {
        public BrasaoDeAnanda(int estoque = 1) : base(new ItemFeatures()
        {
            consumivel = false,
            itemNature = ItemNature.usoParticular,
            NomeID = NameIdItem.brasaoDeAnanda,
            nosItensRapidos = false,
            valor = 8
        })
        {
            Estoque = estoque;
        }
    }

    [System.Serializable]
    public class BrasaoDeBoutjoi : UnusableItem
    {
        public BrasaoDeBoutjoi(int estoque = 1) : base(new ItemFeatures()
        {
            consumivel = false,
            itemNature = ItemNature.usoParticular,
            NomeID = NameIdItem.brasaoDeBoutjoi,
            nosItensRapidos = false,
            valor = 8
        })
        {
            Estoque = estoque;
        }
    }

}