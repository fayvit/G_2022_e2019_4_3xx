using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class HidroBomba: AeroImpactBase
    {
        public HidroBomba() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.hidroBomba,
            tipo = PetTypeName.Agua,
            carac = AttackDiferentialId.colisaoComPow,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 5,
            potenciaCorrente = 34,
            potenciaMaxima = 44,
            potenciaMinima = 24,
            //tempoDeReuso = 8.5f,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 20f,
            distanciaDeRepulsao = 65f,
            velocidadeDeRepulsao = 33,
            tempoDeMoveMin = 0.55f,//74
            tempoDeMoveMax = 1.4f,
            tempoDeDestroy = 1.5f,
            custoDeStamina = 60,
            somDoImpacto = FayvitSounds.SoundEffectID.XP_Knock04,
            somDoGolpe = FayvitSounds.SoundEffectID.Evasion1
        }
            )
        {
            carac = new AeroImpactFeatures(
                ImpactParticles.impactoDeAgua,
                AttacksTrails.hidroBomba,
                ToutchingFloor.aguaAoChao,
                PrepareJump.preparaImpactoDeAguaAoChao,
                FinalAeroImpact.MaisAltoQueOAlvo
                );

        }
    }
}