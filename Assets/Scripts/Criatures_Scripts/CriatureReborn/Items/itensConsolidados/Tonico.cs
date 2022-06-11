namespace Criatures2021
{
    [System.Serializable]
    public class Tonico : ItemAntiStatusBase
    {

        public Tonico(int estoque = 1) : base(new ItemFeatures(NameIdItem.tonico)
        {
            valor = 30
        }
            )
        {
            Estoque = estoque;
            qualStatusRemover = StatusType.fraco;
        }


    }
}