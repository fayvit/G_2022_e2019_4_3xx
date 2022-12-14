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
            CDoAfetado.GetComponent<PetManager>().Mov.ModSpeed *= 
                (OAfetado.StatusTemporarios[indice].Quantificador-1)
                *(float)1 / OAfetado.StatusTemporarios[indice].Quantificador;
        }

        public override void RetiraComponenteStatus()
        {
            if (CDoAfetado != null)
            {
                int indice = ContemStatus(StatusType.amedrontado, OAfetado);
                CDoAfetado.GetComponent<PetManager>().Mov.ModSpeed *= OAfetado.StatusTemporarios[indice].Quantificador;
            }
            base.RetiraComponenteStatus();
        }
    }
}