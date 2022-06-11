using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class Fraco : StatusTemporarioSimplesBase
    {
        public override void Start()
        {
            if (CDoAfetado != null)
                ColocaAParticulaEAdicionaEsseStatus(StatusParticles.particulasFraco.ToString(), CDoAfetado.transform);
        }

        public override void RecolocaParticula()
        {
            ColocaAParticulaEAdicionaEsseStatus(StatusParticles.particulasFraco.ToString(), CDoAfetado.transform);
        }
    }
}