using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class Tapa : ImpactBase
    {

        public Tapa() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.tapa,
            tipo = PetTypeName.Normal,
            carac = AttackDiferentialId.colisao,
            damageAtribute = DamageBaseAtribute.ataque,
            custoPE = 0,
            potenciaCorrente = 10,
            potenciaMaxima = 15,
            potenciaMinima = 5,
            //tempoDeReuso = 3.5f,
            TempoNoDano = 0.5f,
            distanciaDeRepulsao = 65f,
            velocidadeDeRepulsao = 33,
            tempoDeMoveMin = 0.74f,//74
            tempoDeMoveMax = 1.2f,
            tempoDeDestroy = 1.4f,
            velocidadeDeGolpe = 25,
            custoDeStamina=35
        }
        )
        {
            carac = new ImpactFeatures()
            {
                noImpacto = ImpactParticles.impactoComum,
                nomeTrail = AttacksTrails.umCuboETrail,
                parentearNoOsso = true
            };
        }
    }
}