namespace Criatures2021
{
    [System.Serializable]
    public class Aura : EnergyItemBase
    {
        public Aura(int estoque = 1) : base(new ItemFeatures(NameIdItem.aura)
        {
            valor = 40
        }
            )
        {
            Estoque = estoque;
            recuperaDoTipo = PetTypeName.Psiquico;
            valorDeRecuperacao = 40;
        }


    }
}