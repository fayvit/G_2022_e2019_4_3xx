namespace Criatures2021
{
    public class Gasolina : EnergyItemBase
    {
        public Gasolina(int estoque = 1) : base(new ItemFeatures(NameIdItem.gasolina)
        {
            valor = 40
        }
            )
        {
            Estoque = estoque;
            recuperaDoTipo = PetTypeName.Fogo;
            valorDeRecuperacao = 40;
        }


    }
}