namespace Criatures2021
{
    [System.Serializable]
    public class AmuletoDaCoragem : ItemAntiStatusBase
    {

        public AmuletoDaCoragem(int estoque = 1) : base(new ItemFeatures(NameIdItem.amuletoDaCoragem)
        {
            valor = 30
        }
            )
        {
            Estoque = estoque;
            qualStatusRemover = StatusType.amedrontado;
        }


    }
}