using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class ProjetilFeatures
    {

        [System.NonSerialized] public Vector3 posInicial;
        public ProjetilType tipo;
        public ImpactParticles noImpacto;
    }

    public enum ProjetilType
    {
        basico,
        rigido,
        statusExpansivel,
        direcional
    }
}