namespace Criatures2021
{
    public class Quartzo : EnergyItemBase
    {
        public Quartzo(int estoque = 1) : base(new ItemFeatures(NameIdItem.quartzo)
        {
            valor = 40
        }
            )
        {
            Estoque = estoque;
            recuperaDoTipo = PetTypeName.Pedra;
            valorDeRecuperacao = 40;
        }


    }
}