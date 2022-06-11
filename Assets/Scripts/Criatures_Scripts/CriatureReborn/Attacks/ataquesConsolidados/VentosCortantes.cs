using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class VentosCortantes : ProjetilBase
    {
        public VentosCortantes() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.ventosCortantes,
            tipo = PetTypeName.Voador,
            carac = AttackDiferentialId.projetil,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 3,
            potenciaCorrente = 25,
            potenciaMaxima = 29,
            potenciaMinima = 21,
            //tempoDeReuso = 7.5f,
            tempoDeMoveMax = 1,
            tempoDeMoveMin = 0.3f,
            tempoDeDestroy = 2,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 18,
            podeNoAr = true,
            custoDeStamina = 50,
            somDoImpacto = FayvitSounds.SoundEffectID.XP_Knock04,
            somDoGolpe = FayvitSounds.SoundEffectID.rajadaDeAgua
        }
            )
        {
            carac = new ProjetilFeatures()
            {
                noImpacto = ImpactParticles.impactoDeVento,
                tipo = ProjetilType.rigido
            };
        }
    }
}