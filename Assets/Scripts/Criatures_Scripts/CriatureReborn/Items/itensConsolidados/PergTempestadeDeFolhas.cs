using System.Collections;
using System.Collections.Generic;
using TextBankSpace;

namespace Criatures2021
{
    [System.Serializable]
    public class PergTempestadeDeFolhas : AttackLearnItem
    {
        public PergTempestadeDeFolhas(int estoque = 1) : base(new ItemFeatures(NameIdItem.pergTempesdadeDeFolhas)
        {
            valor = 1220,
            itemNature = ItemNature.pergGolpe
        })
        {
            Estoque = estoque;
            TextoDaMensagemInicial = new string[2]
                {
                string.Format(TextBank.RetornaFraseDoIdioma(TextKey.emQuem),ItemBase.NomeEmLinguas(NameIdItem.pergTempesdadeDeFolhas)),
                TextBank.RetornaFraseDoIdioma(TextKey.aprendeuGolpe),
                };
            Particula = GeneralParticles.particulaDoPoderPergaminhoFora;

            golpeDoPergaminho = new AttackNameId[1]
            {
            AttackNameId.tempestadeDeFolhas
            };
        }
    }


}