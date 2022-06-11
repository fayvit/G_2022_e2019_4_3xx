using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class TempestadeDeFolhas : AeroImpactBase
    {
        public TempestadeDeFolhas() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.tempestadeDeFolhas,
            tipo = PetTypeName.Planta,
            carac = AttackDiferentialId.colisaoComPow,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 5,
            potenciaCorrente = 34,
            potenciaMaxima = 44,
            potenciaMinima = 24,
            //tempoDeReuso = 8.5f,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 30f,
            distanciaDeRepulsao = 65f,
            velocidadeDeRepulsao = 33,
            tempoDeMoveMin = 0.65f,//74
            tempoDeMoveMax = 1.3f,
            tempoDeDestroy = 1.45f,
            custoDeStamina = 60,
            somDoImpacto = FayvitSounds.SoundEffectID.XP_Knock04,
            somDoGolpe = FayvitSounds.SoundEffectID.rajadaDeAgua
        }
            )
        {
            carac = new AeroImpactFeatures(
                ImpactParticles.impactoDeFolhas,
                AttacksTrails.tempestadeDeFolhas,
                ToutchingFloor.poeiraAoVento,
                PrepareJump.impactoBaixoDeFolhas,
                FinalAeroImpact.AvanceEPareAbaixo,
                onPrepareSound:FayvitSounds.SoundEffectID.Shot1,
                onTouchGroundSound:FayvitSounds.SoundEffectID.Slash1
                );

        }
    }
}
