namespace Criatures2021
{
    public class RepolhoComOvo : EnergyItemBase
    {
        public RepolhoComOvo(int estoque = 1) : base(new ItemFeatures(NameIdItem.repolhoComOvo)
        {
            valor = 40
        }
            )
        {
            Estoque = estoque;
            recuperaDoTipo = PetTypeName.Gas;
            valorDeRecuperacao = 40;
        }


    }
}