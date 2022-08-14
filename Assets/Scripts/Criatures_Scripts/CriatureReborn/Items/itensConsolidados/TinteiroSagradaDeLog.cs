namespace Criatures2021
{

    [System.Serializable]
    public class TinteiroSagradaDeLog : UnusableItem
    {
        public TinteiroSagradaDeLog(int estoque) : base(new ItemFeatures() {
            consumivel = false,
            itemNature = ItemNature.usoParticular,
            NomeID = NameIdItem.tinteiroSagradoDeLog,
            nosItensRapidos = false,
            valor = 0
        
        })
        {
            Estoque = estoque;
        }


    }

}
