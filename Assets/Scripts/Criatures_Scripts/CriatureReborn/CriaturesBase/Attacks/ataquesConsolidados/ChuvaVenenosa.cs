using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class ChuvaVenenosa : AeroImpactBase
    {
        public ChuvaVenenosa() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.chuvaVenenosa,
            tipo = PetTypeName.Veneno,
            carac = AttackDiferentialId.colisaoComPow,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 5,
            potenciaCorrente = 34,
            potenciaMaxima = 44,
            potenciaMinima = 24,
            //tempoDeReuso = 8.5f,
            TempoNoDano = 0.5f,
            velocidadeDeGolpe = 28f,
            distanciaDeRepulsao = 65f,
            velocidadeDeRepulsao = 33,
            tempoDeMoveMin = 0.55f,//74
            tempoDeMoveMax = 1.4f,
            tempoDeDestroy = 1.6f,
            custoDeStamina = 60,
            somDoImpacto = FayvitSounds.SoundEffectID.XP_Knock04,
            somDoGolpe = FayvitSounds.SoundEffectID.Evasion1
        }
            )
        {
            carac = new AeroImpactFeatures(
                ImpactParticles.impactoVenenoso,
                AttacksTrails.chuvaVenenosa,
                ToutchingFloor.poeiraAoVento,
                PrepareJump.impulsoVenenoso,
                FinalAeroImpact.MaisAltoQueOAlvo,
                onPrepareSound: FayvitSounds.SoundEffectID.Shot3,
                onTouchGroundSound: FayvitSounds.SoundEffectID.Slash1
                );

        }

        public override void VerificaAplicaStatus(PetBase atacante, PetManager cDoAtacado)
        {
            VerifyApplyPoisonStatus.Verify(atacante, cDoAtacado, this, 2);
        }
    }
}