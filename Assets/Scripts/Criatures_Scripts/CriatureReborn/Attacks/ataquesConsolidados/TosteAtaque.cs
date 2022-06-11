using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class TosteAtaque : AeroImpactBase
    {
        public TosteAtaque() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.tosteAtaque,
            tipo = PetTypeName.Fogo,
            carac = AttackDiferentialId.colisaoComPow,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 5,
            potenciaCorrente = 34,
            potenciaMaxima = 44,
            potenciaMinima = 24,
            //tempoDeReuso = 8.5f,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 24f,
            distanciaDeRepulsao = 65f,
            velocidadeDeRepulsao = 33,
            tempoDeMoveMin = 0.75f,//74
            tempoDeMoveMax = 1.6f,
            tempoDeDestroy = 1.7f,
            custoDeStamina = 60,
            somDoImpacto = FayvitSounds.SoundEffectID.XP_Knock04,
            somDoGolpe = FayvitSounds.SoundEffectID.rajadaDeAgua
        }
            )
        {
            carac = new AeroImpactFeatures(
                ImpactParticles.impactoDeFogo,
                AttacksTrails.tosteAtaque,
                ToutchingFloor.impactoDeFogo,
                PrepareJump.preparaImpactoDeFogoAoChao,
                FinalAeroImpact.MaisAltoQueOAlvo,
                onPrepareSound: FayvitSounds.SoundEffectID.Shot3,
                onTouchGroundSound: FayvitSounds.SoundEffectID.Slash1
                );
        }
    }
}
