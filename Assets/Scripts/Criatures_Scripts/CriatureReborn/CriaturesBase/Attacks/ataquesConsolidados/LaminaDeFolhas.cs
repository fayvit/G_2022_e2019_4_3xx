using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class LaminaDeFolhas : ProjetilBase
    {

        public LaminaDeFolhas() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.laminaDeFolha,
            tipo = PetTypeName.Planta,
            carac = AttackDiferentialId.projetil,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 2,
            potenciaCorrente = 15,
            potenciaMaxima = 19,
            potenciaMinima = 11,
            //tempoDeReuso = 5,
            tempoDeMoveMax = 1,
            tempoDeMoveMin = 0.3f,
            tempoDeDestroy = 2,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 18,
            podeNoAr = true,
            custoDeStamina = 40,
            somDoImpacto = FayvitSounds.SoundEffectID.XP_Knock04,
            somDoGolpe = FayvitSounds.SoundEffectID.Slash2
        }
            )
        {
            carac = new ProjetilFeatures()
            {
                noImpacto = ImpactParticles.impactoDeFolhas,
                tipo = ProjetilType.basico
            };
        }
    }
}