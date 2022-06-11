namespace Criatures2021
{
    public class Regador : EnergyItemBase
    {
        public Regador(int estoque = 1) : base(new ItemFeatures(NameIdItem.regador)
        {
            valor = 40
        }
            )
        {
            Estoque = estoque;
            recuperaDoTipo = PetTypeName.Planta;
            valorDeRecuperacao = 40;
        }


    }
}