using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class Cabecada : ImpactBase
    {

        public Cabecada() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.cabecada,
            tipo = PetTypeName.Normal,
            carac = AttackDiferentialId.colisao,
            damageAtribute = DamageBaseAtribute.ataque,
            custoPE = 0,
            potenciaCorrente = 12,
            potenciaMaxima = 16,
            potenciaMinima = 8,
            //tempoDeReuso = 3.5f,
            TempoNoDano = 0.5f,
            distanciaDeRepulsao = 65f,
            velocidadeDeRepulsao = 33,
            tempoDeMoveMin = 0.25f,//74
            tempoDeMoveMax = 0.5f,
            tempoDeDestroy = .7f,
            velocidadeDeGolpe = 28,
            custoDeStamina = 37
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