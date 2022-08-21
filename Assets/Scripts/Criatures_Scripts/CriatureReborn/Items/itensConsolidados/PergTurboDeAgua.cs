using System.Collections;
using System.Collections.Generic;
using TextBankSpace;

namespace Criatures2021
{
    [System.Serializable]
    public class PergTurboDeAgua : AttackLearnItem
    {
        public PergTurboDeAgua(int estoque = 1) : base(new ItemFeatures(NameIdItem.pergTurboDeAgua)
        {
            valor = 440,
            itemNature = ItemNature.pergGolpe
        })
        {
            Estoque = estoque;
            TextoDaMensagemInicial = new string[2]
                {
                string.Format(TextBank.RetornaFraseDoIdioma(TextKey.emQuem),ItemBase.NomeEmLinguas(NameIdItem.pergTurboDeAgua)),
                TextBank.RetornaFraseDoIdioma(TextKey.aprendeuGolpe),
                };
            Particula = GeneralParticles.particulaDoPoderPergaminhoFora;

            golpeDoPergaminho = new AttackNameId[1]
            {
            AttackNameId.turboDeAgua
            };
        }
    }


}