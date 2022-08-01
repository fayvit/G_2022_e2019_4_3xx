using System.Collections;
using UnityEngine;

namespace Criatures2021
{
    public class SlopeSlipManager
    {
        public static PetAttackBase Slip(GameObject gameObject, RaycastHit hit)
        {
            PetAttackBase petAttack = new PetAttackBase(new PetAttackFeatures()
            {
                potenciaCorrente = 0,
                potenciaMaxima = 0,
                potenciaMinima = 0,
                TempoNoDano = 0.75f,
                distanciaDeRepulsao = 95f,
                velocidadeDeRepulsao = 33,
                tempoDeMoveMin = 0.45f,//74
                tempoDeMoveMax = 0.85f,
                tempoDeDestroy = 1.1f,
                velocidadeDeGolpe = 28,
            });

            DustInTheWind d = gameObject.AddComponent<DustInTheWind>();
            d.tempoDeRepeticao = .1f;
            MonoBehaviour.Destroy(d, petAttack.TempoDeMoveMax);

            petAttack.DirDeREpulsao = (5 * Vector3.down + hit.normal).normalized;

            gameObject.transform.rotation = Quaternion.LookRotation(DirectionOnThePlane.NormalizedInTheUp(hit.normal));

            return petAttack;
        }
    }
}