using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class SobreSalto : AeroImpactBase
    {
        public SobreSalto() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.sobreSalto,
            tipo = PetTypeName.Normal,
            carac = AttackDiferentialId.colisaoComPow,
            damageAtribute = DamageBaseAtribute.ataque,
            custoPE = 0,
            potenciaCorrente = 25,
            potenciaMaxima = 29,
            potenciaMinima = 21,
            //tempoDeReuso = 6.5f,
            TempoNoDano = 0.5f,
            velocidadeDeGolpe = 28f,
            distanciaDeRepulsao = 65f,
            velocidadeDeRepulsao = 33,
            tempoDeMoveMin = 0.75f,//74
            tempoDeMoveMax = 1.6f,
            tempoDeDestroy = 1.7f,
            colisorScale = 2,
            custoDeStamina = 60,
            somDoImpacto = FayvitSounds.SoundEffectID.XP_Knock04,
            somDoGolpe = FayvitSounds.SoundEffectID.Evasion1
        }
            )
        {
            carac = new AeroImpactFeatures(
                ImpactParticles.impactoComum,
                AttacksTrails.umCuboETrail,
                ToutchingFloor.impactoAoChao,
                PrepareJump.preparaImpactoAoChao,
                FinalAeroImpact.MaisAltoQueOAlvo,
                onPrepareSound: FayvitSounds.SoundEffectID.Shot3,
                onTouchGroundSound: FayvitSounds.SoundEffectID.Slash1
                );

        }
    }
}