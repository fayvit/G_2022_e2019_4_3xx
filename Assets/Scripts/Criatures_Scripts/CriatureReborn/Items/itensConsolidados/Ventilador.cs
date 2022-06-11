namespace Criatures2021
{
    public class Ventilador : EnergyItemBase
    {
        public Ventilador(int estoque = 1) : base(new ItemFeatures(NameIdItem.ventilador)
        {
            valor = 40
        }
            )
        {
            Estoque = estoque;
            recuperaDoTipo = PetTypeName.Voador;
            valorDeRecuperacao = 40;
        }


    }
}