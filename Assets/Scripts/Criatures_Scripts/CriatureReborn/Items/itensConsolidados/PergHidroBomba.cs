using System.Collections;
using System.Collections.Generic;
using TextBankSpace;

namespace Criatures2021
{
    [System.Serializable]
    public class PergHidroBomba : AttackLearnItem
    {
        public PergHidroBomba(int estoque = 1) : base(new ItemFeatures(NameIdItem.pergHidroBomba)
        {
            valor = 1220,
            itemNature = ItemNature.pergGolpe
        })
        {
            Estoque = estoque;
            TextoDaMensagemInicial = new string[2]
                {
                string.Format(TextBank.RetornaFraseDoIdioma(TextKey.emQuem),ItemBase.NomeEmLinguas(NameIdItem.pergHidroBomba)),
                TextBank.RetornaFraseDoIdioma(TextKey.aprendeuGolpe),
                };
            Particula = GeneralParticles.particulaDoPoderPergaminhoFora;

            golpeDoPergaminho = new AttackNameId[1]
            {
            AttackNameId.hidroBomba
            };
        }
    }


}