using Criatures2021Hud;
using FayvitMessageAgregator;
using System.Collections;
using UnityEngine;

namespace Criatures2021
{
    [System.Serializable]
    public class ModifyConsumableAttributeBase : ModifyPetItemBase
    {
        public ModifyConsumableAttributeBase(ItemFeatures cont) : base(cont) { confirmarRetorno = true; }

        protected ConsumableAttribute ContaDeSubida(ConsumableAttribute A)
        {
            A = new ConsumableAttribute(
                    A.Corrente,
                    A.Taxa,
                    A.Maximo + 4,
                    A.ModMaximo
                    );
            A.Corrente = A.Maximo;
            return A;
        }

        protected override void EntraNoModoFinalizacao(PetBase C)
        {
            if (eMenu)
            {
                ItemBase refi = ProcuraItemNaLista(ID, Lista);
                MessageAgregator<MsgUsingQuantitativeItem>.Publish(new MsgUsingQuantitativeItem()
                {
                    confirmarRetorno = true,
                    mensagemDeRetorno = string.Format(
                        TextoDaMensagemInicial[1],
                        C.GetNomeEmLinguas, C.G_XP.Nivel),
                    temMaisParausar = refi.ID == ID && refi.Estoque > 0
                });
            }
            else
            {
                base.EntraNoModoFinalizacao(C);
            }
        }

    }
}