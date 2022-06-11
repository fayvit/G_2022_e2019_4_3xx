namespace Criatures2021
{
    public class Adubo : EnergyItemBase
    {
        public Adubo(int estoque = 1) : base(new ItemFeatures(NameIdItem.adubo)
        {
            valor = 40
        }
            )
        {
            Estoque = estoque;
            recuperaDoTipo = PetTypeName.Terra;
            valorDeRecuperacao = 40;
        }


    }
}