using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class SimulateLevelPass {
        public void SimulaPassaNivel(GerenciadorDeExperiencia gXP, PetAtributes A, int ateONivel = -1)
        {
            if (ateONivel < 0)
                ateONivel = 99;

            for (int i = 0; i < ateONivel; i++)
            {
                if (gXP.VerificaPassaNivel())
                {
                    gXP.AplicaPassaNivel();
                    UpDeNivel.calculaUpDeNivel(gXP.Nivel, A);
                }
                gXP.XP = gXP.ParaProxNivel + 1;
                Debug.Log(gXP.Nivel + " : " + gXP.XP + "/" + gXP.ParaProxNivel + " : " + gXP.UltimoPassaNivel
                          + " : " + gXP.CalculaPassaNivelInicial(gXP.Nivel, true));
            }
        }
    }
}