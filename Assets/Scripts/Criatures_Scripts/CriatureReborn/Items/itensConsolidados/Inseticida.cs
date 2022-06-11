namespace Criatures2021
{
    public class Inseticida : EnergyItemBase
    {
        public Inseticida(int estoque = 1) : base(new ItemFeatures(NameIdItem.inseticida)
        {
            valor = 40
        }
            )
        {
            Estoque = estoque;
            recuperaDoTipo = PetTypeName.Veneno;
            valorDeRecuperacao = 40;
        }


    }
}