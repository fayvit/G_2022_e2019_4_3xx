using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class ChicoteDeCalda : ImpactBase
    {

        public ChicoteDeCalda() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.chicoteDeCalda,
            tipo = PetTypeName.Normal,
            carac = AttackDiferentialId.colisao,
            damageAtribute = DamageBaseAtribute.ataque,
            custoPE = 0,
            potenciaCorrente = 10,
            potenciaMaxima = 15,
            potenciaMinima = 7,
            //tempoDeReuso = 3.5f,
            TempoNoDano = 0.65f,
            distanciaDeRepulsao = 65f,
            velocidadeDeRepulsao = 53,
            tempoDeMoveMin = 0.5f,//74
            tempoDeMoveMax = 1.2f,
            tempoDeDestroy = 1.4f,
            velocidadeDeGolpe = 20,
            custoDeStamina = 35
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
