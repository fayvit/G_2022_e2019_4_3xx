using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class Bastao : ImpactBase
    {

        public Bastao() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.bastao,
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
            tempoDeMoveMin = 0.25f,//74
            tempoDeMoveMax = 0.5f,
            tempoDeDestroy = 1f,
            velocidadeDeGolpe = 28,
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