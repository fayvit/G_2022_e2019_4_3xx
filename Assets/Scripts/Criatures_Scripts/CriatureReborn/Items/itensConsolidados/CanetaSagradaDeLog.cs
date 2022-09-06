namespace Criatures2021
{
    [System.Serializable]
    public class CanetaSagradaDeLog:UnusableItem
    {
        public CanetaSagradaDeLog(int estoque) : base(new ItemFeatures(NameIdItem.tinteiroSagradoDeLog)
        {
            consumivel = false,
            itemNature = ItemNature.chave,
            nosItensRapidos = false,
            valor = 0

        })
        {
            Estoque = estoque;
        }


    }

}
