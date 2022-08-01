namespace Criatures2021
{
    [System.Serializable]
    public class AguaTonica : EnergyItemBase
    {
        public AguaTonica(int estoque = 1) : base(new ItemFeatures(NameIdItem.aguaTonica)
        {
            valor = 40
        }
            )
        {
            Estoque = estoque;
            recuperaDoTipo = PetTypeName.Agua;
            valorDeRecuperacao = 40;
        }


    }
}