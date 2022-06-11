using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class Amedrontado : StatusTemporarioSimplesBase
    {
        public override void Start()
        {
            if (CDoAfetado != null)
            {
                ColocaAParticulaEAdicionaEsseStatus(StatusParticles.particulasMedo.ToString(), CDoAfetado.transform);
                
                StatusHitUpdater();
            }
        }

        public override void RecolocaParticula()
        {
            ColocaAParticulaEAdicionaEsseStatus(StatusParticles.particulasMedo.ToString(), CDoAfetado.transform);
        }

        public override void StatusHitUpdater()
        {
            int indice = ContemStatus(StatusType.amedrontado, OAfetado);
            CDoAfetado.Mov.ModSpeed *= 
                (CDoAfetado.MeuCriatureBase.StatusTemporarios[indice].Quantificador-1)
                *(float)1 / CDoAfetado.MeuCriatureBase.StatusTemporarios[indice].Quantificador;
        }

        public override void RetiraComponenteStatus()
        {
            int indice = ContemStatus(StatusType.amedrontado, OAfetado);
            CDoAfetado.Mov.ModSpeed *= CDoAfetado.MeuCriatureBase.StatusTemporarios[indice].Quantificador;
            base.RetiraComponenteStatus();
        }
    }
}