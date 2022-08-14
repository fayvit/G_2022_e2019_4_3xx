

using Criatures2021Hud;
using FayvitMessageAgregator;

namespace Criatures2021
{
    [System.Serializable]
    public class ModifyIntrinsicAtributeBase:ModifyPetItemBase
    {
        public ModifyIntrinsicAtributeBase(ItemFeatures cont) : base(cont) { confirmarRetorno = true; }

        protected IntrinsicAttribute ContaDeSubida(IntrinsicAttribute A)
        {
            if ((A.Minimo + A.Maximo) / 2 + 1 > 5)
            {
                A = new IntrinsicAttribute(
                    A.Corrente + 1,
                    A.Taxa,
                    A.Maximo + 1,
                    A.Minimo + 1,
                    A.ModCorrente,
                    A.ModMaximo
                    );

            }
            else
            {
                //Debug.Log("<5");
                A = new IntrinsicAttribute(
                    A.Corrente + 1,
                    A.Taxa,
                    A.Maximo + 2,
                    1,
                    A.ModCorrente,
                    A.ModMaximo
                    );

            }
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
            }else
            {
                base.EntraNoModoFinalizacao(C);
            }
        }
    }
}
