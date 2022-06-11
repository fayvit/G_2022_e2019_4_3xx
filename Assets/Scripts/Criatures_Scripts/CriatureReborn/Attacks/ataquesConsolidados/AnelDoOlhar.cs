using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Criatures2021
{
    [System.Serializable]
    public class AnelDoOlhar : ProjetilBase
    {
        public AnelDoOlhar() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.anelDoOlhar,
            tipo = PetTypeName.Normal,
            carac = AttackDiferentialId.projetil,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 6,
            potenciaCorrente = 38,
            potenciaMaxima = 48,
            potenciaMinima = 28,
            //tempoDeReuso = 8.5f,
            tempoDeMoveMax = 1,
            tempoDeMoveMin = 0.3f,
            tempoDeDestroy = 2,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 18,
            custoDeStamina = 50,
            somDoImpacto = FayvitSounds.SoundEffectID.XP_Knock04,
            somDoGolpe = FayvitSounds.SoundEffectID.Slash2
        }
        )
        {
            carac = new ProjetilFeatures()
            {
                noImpacto = ImpactParticles.impactoComum,
                tipo = ProjetilType.rigido
            };
        }
    }
}