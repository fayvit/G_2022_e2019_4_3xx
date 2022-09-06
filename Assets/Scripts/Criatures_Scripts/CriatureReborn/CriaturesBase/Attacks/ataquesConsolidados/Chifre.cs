using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Criatures2021
{
    [System.Serializable]
    public class Chifre : ImpactBase
    {
        public Chifre():base(new PetAttackFeatures() {
            nome = AttackNameId.chifre,
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
            tempoDeMoveMin = 0.45f,//74
            tempoDeMoveMax = 0.85f,
            tempoDeDestroy = 1.1f,
            velocidadeDeGolpe = 28,
            custoDeStamina = 35
        })
        {
            carac = new ImpactFeatures()
            {
                noImpacto = ImpactParticles.impactoComum,
                nomeTrail = AttacksTrails.umCuboETrail,
                parentearNoOsso = false
            };
        }
    }
}