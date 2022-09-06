using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class GosmaAcida : ProjetilBase
    {

        public GosmaAcida() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.gosmaAcida,
            tipo = PetTypeName.Inseto,
            carac = AttackDiferentialId.projetil,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 3,
            potenciaCorrente = 25,
            potenciaMaxima = 29,
            potenciaMinima = 21,
            //tempoDeReuso = 7.5f,
            tempoDeMoveMax = 1,
            tempoDeMoveMin = 0.15f,
            tempoDeDestroy = 2,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 10,
            podeNoAr = false,
            custoDeStamina = 50,
            somDoImpacto = FayvitSounds.SoundEffectID.XP_Knock04,
            somDoGolpe = FayvitSounds.SoundEffectID.Slash2
        }
            )
        {
            carac = new ProjetilFeatures()
            {
                noImpacto = ImpactParticles.impactoDeGosmaAcida,
                tipo = ProjetilType.basico
            };
        }
    }
}
