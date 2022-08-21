namespace Criatures2021
{

    [System.Serializable]
    public class TinteiroSagradaDeLog : UnusableItem
    {
        public TinteiroSagradaDeLog(int estoque) : base(new ItemFeatures(NameIdItem.tinteiroSagradoDeLog) {
            consumivel = false,
            itemNature = ItemNature.usoParticular,
            nosItensRapidos = false,
            valor = 0
        
        })
        {
            Estoque = estoque;
        }


    }

}
