


namespace Criatures2021
{
    [System.Serializable]
    public class BrasaoDeLaurense : UnusableItem
    {
        public BrasaoDeLaurense(int estoque = 1) : base(new ItemFeatures(NameIdItem.brasaoDeLaurense)
        {
            consumivel = false,
            itemNature = ItemNature.usoParticular,
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
        public BrasaoDeAnanda(int estoque = 1) : base(new ItemFeatures(NameIdItem.brasaoDeAnanda)
        {
            consumivel = false,
            itemNature = ItemNature.usoParticular,
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
        public BrasaoDeBoutjoi(int estoque = 1) : base(new ItemFeatures(NameIdItem.brasaoDeBoutjoi)
        {
            consumivel = false,
            itemNature = ItemNature.usoParticular,
            nosItensRapidos = false,
            valor = 8
        })
        {
            Estoque = estoque;
        }
    }

}