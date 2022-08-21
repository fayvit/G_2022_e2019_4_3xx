using System.Collections;
using System.Collections.Generic;
using TextBankSpace;

namespace Criatures2021
{
    [System.Serializable]
    public class PergRajadaDeFogo : AttackLearnItem
    {
        public PergRajadaDeFogo(int estoque = 1) : base(new ItemFeatures(NameIdItem.pergRajadaDeFogo)
        {
            valor = 440,
            itemNature = ItemNature.pergGolpe
        })
        {
            Estoque = estoque;
            TextoDaMensagemInicial = new string[2]
                {
                string.Format(TextBank.RetornaFraseDoIdioma(TextKey.emQuem),ItemBase.NomeEmLinguas(NameIdItem.pergRajadaDeFogo)),
                TextBank.RetornaFraseDoIdioma(TextKey.aprendeuGolpe),
                };
            Particula = GeneralParticles.particulaDoAtaquePergaminhoFora;

            golpeDoPergaminho = new AttackNameId[1]
            {
            AttackNameId.rajadaDeFogo
            };
        }
    }


}