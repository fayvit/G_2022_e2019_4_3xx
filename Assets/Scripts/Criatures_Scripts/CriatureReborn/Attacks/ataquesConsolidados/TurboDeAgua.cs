using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Criatures2021
{
    [System.Serializable]
    public class TurboDeAgua : ProjetilBase
    {
        public TurboDeAgua() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.turboDeAgua,
            tipo = PetTypeName.Agua,
            carac = AttackDiferentialId.projetil,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 3,
            potenciaCorrente = 25,
            potenciaMaxima = 29,
            potenciaMinima = 21,
            //tempoDeReuso = 7.5f,
            tempoDeMoveMax = 1.25f,
            tempoDeMoveMin = 0f,
            tempoDeDestroy = 2,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 15,
            custoDeStamina=50,
            somDoImpacto = FayvitSounds.SoundEffectID.XP_Knock04,
            somDoGolpe = FayvitSounds.SoundEffectID.rajadaDeAgua
        }
        )
        {
            carac = new ProjetilFeatures()
            {
                noImpacto = ImpactParticles.impactoDeAgua,
                tipo = ProjetilType.rigido
            };
        }
    }
}