using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class StatusTemporarioSimplesBase : StatusTemporarioBase
    {
        private float tempoAcumulado = 0;
        public override void Update()
        {
            tempoAcumulado += Time.deltaTime;

            if (tempoAcumulado >= Dados.TempoSignificativo || OAfetado.PetFeat.meusAtributos.PV.Corrente <= 0)
            {
                RetiraComponenteStatus();
            }
        }
    }

    public enum StatusParticles
    {
        particulasFraco,
        particulasMedo,
        particulasEnvenenado
    }
}