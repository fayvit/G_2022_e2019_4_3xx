namespace Criatures2021
{
    [System.Serializable]
    public class Antidoto : ItemAntiStatusBase
    {

        public Antidoto(int estoque = 1) : base(new ItemFeatures(NameIdItem.antidoto)
        {
            valor = 30
        }
            )
        {
            Estoque = estoque;
            qualStatusRemover = StatusType.envenenado;
        }


    }
}