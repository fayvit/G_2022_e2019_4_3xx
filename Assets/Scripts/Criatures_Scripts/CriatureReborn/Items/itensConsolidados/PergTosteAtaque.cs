using System.Collections;
using System.Collections.Generic;
using TextBankSpace;

namespace Criatures2021
{
    [System.Serializable]
    public class PergTosteAtaque : AttackLearnItem
    {
        public PergTosteAtaque(int estoque = 1) : base(new ItemFeatures(NameIdItem.pergTosteAtaque)
        {
            valor = 1220,
            itemNature = ItemNature.pergGolpe
        })
        {
            Estoque = estoque;
            TextoDaMensagemInicial = new string[2]
                {
                string.Format(TextBank.RetornaFraseDoIdioma(TextKey.emQuem),ItemBase.NomeEmLinguas(NameIdItem.pergTosteAtaque)),
                TextBank.RetornaFraseDoIdioma(TextKey.aprendeuGolpe),
                };
            Particula = GeneralParticles.particulaDoAtaquePergaminhoFora;

            golpeDoPergaminho = new AttackNameId[1]
            {
            AttackNameId.tosteAtaque
            };
        }
    }


}