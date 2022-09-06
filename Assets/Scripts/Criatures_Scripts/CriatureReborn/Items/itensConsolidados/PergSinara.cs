using UnityEngine;
using System.Collections;
using TextBankSpace;
using FayvitMessageAgregator;

namespace Criatures2021
{
    [System.Serializable]
    public class PergaminhoDeSinara : ModifyConsumableAttributeBase
    {

        public PergaminhoDeSinara(int estoque = 1) : base(new ItemFeatures(NameIdItem.pergSinara)
        {
            valor = 1500
        }
            )
        {
            Estoque = estoque;
            TextoDaMensagemInicial = TextBank.RetornaListaDeTextoDoIdioma(TextKey.usarPergaminhoDeSinara).ToArray();
            Particula = GeneralParticles.particulaDoPVpergaminho;
        }

        protected override void AplicaEfeito(int indice)
        {
            PetBase C = MyGlobalController.MainPlayer.Dados.CriaturesAtivos[indice];
            ConsumableAttribute A = C.PetFeat.meusAtributos.PV;

            A = ContaDeSubida(A);

            C.PetFeat.meusAtributos.PV = A;

            EntraNoModoFinalizacao(C);

            MessageAgregator<MsgChangeHP>.Publish(new MsgChangeHP()
            {
                antHp = A.Corrente - 4,
                currentHp = A.Corrente,
                maxHp = A.Maximo,
                target = C
            });
        }
    }
}