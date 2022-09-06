using TextBankSpace;

namespace Criatures2021
{
    [System.Serializable]
    public class PergaminhoDeAnanda: ModifyIntrinsicAtributeBase
    {
        public PergaminhoDeAnanda(int estoque = 1) : base(new ItemFeatures(NameIdItem.pergaminhoDeAnanda)
        {
            valor = 1500
        }
        )
        {
            Estoque = estoque;
            TextoDaMensagemInicial = TextBank.RetornaListaDeTextoDoIdioma(TextKey.usarPergaminhoDeAnanda).ToArray();
            Particula = GeneralParticles.particulaDoPoderPergaminhoFora;
        }

        protected override void AplicaEfeito(int indice)            
        {
            PetBase C = MyGlobalController.MainPlayer.Dados.CriaturesAtivos[indice];
            IntrinsicAttribute A = C.PetFeat.meusAtributos.Poder;

            A = ContaDeSubida(A);

            C.PetFeat.meusAtributos.Poder = A;

            EntraNoModoFinalizacao(C);
        }
    }
}
