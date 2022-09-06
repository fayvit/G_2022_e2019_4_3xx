using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class ChicoteDeMao : AeroImpactBase
    {
        public ChicoteDeMao() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.chicoteDeMao,
            tipo = PetTypeName.Normal,
            carac = AttackDiferentialId.colisaoComPow,
            damageAtribute = DamageBaseAtribute.ataque,
            custoPE = 0,
            potenciaCorrente = 10,
            potenciaMaxima = 15,
            potenciaMinima = 7,
            //tempoDeReuso = 3.5f,
            TempoNoDano = 0.5f,
            velocidadeDeGolpe = 28f,
            distanciaDeRepulsao = 65f,
            velocidadeDeRepulsao = 33,
            tempoDeMoveMin = 0.75f,//74
            tempoDeMoveMax = 1.6f,
            tempoDeDestroy = 1.8f,
            custoDeStamina = 35,
            somDoImpacto = FayvitSounds.SoundEffectID.XP_Knock04
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
