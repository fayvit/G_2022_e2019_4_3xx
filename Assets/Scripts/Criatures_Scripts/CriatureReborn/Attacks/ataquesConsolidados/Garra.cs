using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class Garra : ImpactBase
    {

        public Garra() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.garra,
            tipo = PetTypeName.Normal,
            carac = AttackDiferentialId.colisao,
            damageAtribute = DamageBaseAtribute.ataque,
            custoPE = 0,
            potenciaCorrente = 10,
            potenciaMaxima = 15,
            potenciaMinima = 7,
            //tempoDeReuso = 3.5f,
            TempoNoDano = 0.5f,
            velocidadeDeGolpe = 20f,
            distanciaDeRepulsao = 65f,
            velocidadeDeRepulsao = 33,
            tempoDeMoveMin = 0.3f,//74
            tempoDeMoveMax = .75f,//0.55f,
            tempoDeDestroy = 1.2f,
            custoDeStamina = 35
        }
        )
        {
            carac = new ImpactFeatures()
            {
                noImpacto = ImpactParticles.impactoComum,
                nomeTrail = AttacksTrails.colisorParaGarra,
                parentearNoOsso = true
            };
        }
    }
}
