using TextBankSpace;

namespace Criatures2021
{
    [System.Serializable]
    public class PergaminhoDeBoutjoi: ModifyIntrinsicAtributeBase
    {
        public PergaminhoDeBoutjoi(int estoque = 1) : base(new ItemFeatures(NameIdItem.pergaminhoDeBoutjoi)
        {
            valor = 1500
        }
        )
        {
            Estoque = estoque;
            TextoDaMensagemInicial = TextBank.RetornaListaDeTextoDoIdioma(TextKey.usarPergaminhoDeBoutjoi).ToArray();
        }

        protected override void AplicaEfeito(int indice)            
        {
            PetBase C = MyGlobalController.MainPlayer.Dados.CriaturesAtivos[indice];
            IntrinsicAttribute A = C.PetFeat.meusAtributos.Ataque;

            A = ContaDeSubida(A);

            C.PetFeat.meusAtributos.Defesa = A;

            EntraNoModoFinalizacao(C);
        }
    }
}
