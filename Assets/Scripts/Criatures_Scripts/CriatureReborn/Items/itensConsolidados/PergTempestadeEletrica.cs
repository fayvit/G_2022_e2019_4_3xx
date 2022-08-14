using System.Collections;
using System.Collections.Generic;
using TextBankSpace;

namespace Criatures2021
{
    [System.Serializable]
    public class PergTempestadeEletrica : AttackLearnItem
    {
        public PergTempestadeEletrica(int estoque = 1) : base(new ItemFeatures(NameIdItem.pergTempestadeEletrica)
        {
            valor = 440,
            itemNature = ItemNature.pergGolpe
        })
        {
            Estoque = estoque;
            TextoDaMensagemInicial = new string[2]
                {
                string.Format(TextBank.RetornaFraseDoIdioma(TextKey.emQuem),ItemBase.NomeEmLinguas(NameIdItem.pergTempestadeEletrica)),
                TextBank.RetornaFraseDoIdioma(TextKey.aprendeuGolpe),
                };
            Particula = GeneralParticles.particulaDaDefesaPergaminhoFora;

            golpeDoPergaminho = new AttackNameId[1]
            {
            AttackNameId.tempestadeEletrica
            };
        }
    }


}