namespace Criatures2021
{
    [System.Serializable]
    public class Pilha : EnergyItemBase
    {
        public Pilha(int estoque = 1) : base(new ItemFeatures(NameIdItem.pilha)
        {
            valor = 40
        }
            )
        {
            Estoque = estoque;
            recuperaDoTipo = PetTypeName.Eletrico;
            valorDeRecuperacao = 40;
        }


    }
}