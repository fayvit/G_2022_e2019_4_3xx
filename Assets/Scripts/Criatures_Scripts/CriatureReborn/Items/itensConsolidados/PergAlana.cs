using UnityEngine;
using System.Collections;
using TextBankSpace;
using FayvitMessageAgregator;

namespace Criatures2021
{
    [System.Serializable]
    public class PergAlana : ModifyConsumableAttributeBase
    {

        public PergAlana(int estoque = 1) : base(new ItemFeatures(NameIdItem.pergAlana)
        {
            valor = 1500
        }
            )
        {
            Estoque = estoque;
            TextoDaMensagemInicial = TextBank.RetornaListaDeTextoDoIdioma(TextKey.usarPergaminhoDeAlana).ToArray();
            Particula = GeneralParticles.particulaDoPEpergaminho;
        }

        protected override void AplicaEfeito(int indice)
        {
            PetBase C = MyGlobalController.MainPlayer.Dados.CriaturesAtivos[indice];
            ConsumableAttribute A = C.PetFeat.meusAtributos.PE;

            A = ContaDeSubida(A);

            C.PetFeat.meusAtributos.PE = A;

            EntraNoModoFinalizacao(C);
            MessageAgregator<MsgChangeMP>.Publish(new MsgChangeMP()
            {
                antMp = A.Corrente - 4,
                currentMp = A.Corrente,
                maxMp = A.Maximo,
                target = C
            });
        }
    }
}