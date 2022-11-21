using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class Sabre : ProjetilBase
    {

        public Sabre(AttackNameId n) : base(new PetAttackFeatures()
        {
            nome = n,
            tipo = PetTypeName.Normal,
            carac = AttackDiferentialId.colisao,
            damageAtribute = DamageBaseAtribute.ataque,
            custoPE = 0,
            potenciaCorrente = 33,
            potenciaMaxima = 24,
            potenciaMinima = 15,
            //tempoDeReuso = 5,
            tempoDeMoveMax = 0.75f,
            tempoDeMoveMin = 0.35f,
            tempoDeDestroy = 1.1f,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 1.5f,
            custoDeStamina = 45,
            podeNoAr = false,
            somDoImpacto = FayvitSounds.SoundEffectID.XP_Knock04,
            somDoGolpe = FayvitSounds.SoundEffectID.XP_Ice03
        }
            )
        {
            carac = new ProjetilFeatures()
            {
                noImpacto = ImpactParticles.impactoComum,
                tipo = ProjetilType.basico
            };
        }
    }
}