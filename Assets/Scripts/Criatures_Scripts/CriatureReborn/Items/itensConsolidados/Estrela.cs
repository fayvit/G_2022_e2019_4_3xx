namespace Criatures2021
{
    [System.Serializable]
    public class Estrela : EnergyItemBase
    {
        public Estrela(int estoque = 1) : base(new ItemFeatures(NameIdItem.estrela)
        {
            valor = 40
        }
            )
        {
            Estoque = estoque;
            recuperaDoTipo = PetTypeName.Normal;
            valorDeRecuperacao = 40;
        }


    }
}