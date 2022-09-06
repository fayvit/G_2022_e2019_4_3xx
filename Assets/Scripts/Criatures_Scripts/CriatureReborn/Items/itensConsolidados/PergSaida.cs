namespace Criatures2021
{

    [System.Serializable]
    public class PergSaida : UnusableItem
    {
        public PergSaida(int estoque) : base(new ItemFeatures(NameIdItem.pergSaida)
        {
            consumivel = true,
            itemNature = ItemNature.consumivel,
            nosItensRapidos = true,
            valor = 60
        })
        {
            Estoque = estoque;
        }
    }

}
