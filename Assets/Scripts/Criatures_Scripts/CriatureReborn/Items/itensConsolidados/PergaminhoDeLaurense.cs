using TextBankSpace;

namespace Criatures2021
{
    [System.Serializable]
    public class PergaminhoDeLaurense: ModifyIntrinsicAtributeBase
    {
        public PergaminhoDeLaurense(int estoque = 1) : base(new ItemFeatures(NameIdItem.pergaminhoDeLaurense)
        {
            valor = 1500
        }
        )
        {
            Estoque = estoque;
            TextoDaMensagemInicial = TextBank.RetornaListaDeTextoDoIdioma(TextKey.usarPergaminhoDeLaurense).ToArray();
            Particula = GeneralParticles.particulaDoAtaquePergaminhoFora;
        }

        protected override void AplicaEfeito(int indice)            
        {
            PetBase C = MyGlobalController.MainPlayer.Dados.CriaturesAtivos[indice];
            IntrinsicAttribute A = C.PetFeat.meusAtributos.Ataque;

            A = ContaDeSubida(A);

            C.PetFeat.meusAtributos.Ataque = A;

            EntraNoModoFinalizacao(C);
        }
    }
}
