namespace Criatures2021
{
    [System.Serializable]
    public class Seiva : EnergyItemBase
    {
        public Seiva(int estoque = 1) : base(new ItemFeatures(NameIdItem.seiva)
        {
            valor = 40
        }
            )
        {
            Estoque = estoque;
            recuperaDoTipo = PetTypeName.Inseto;
            valorDeRecuperacao = 40;
        }


    }
}