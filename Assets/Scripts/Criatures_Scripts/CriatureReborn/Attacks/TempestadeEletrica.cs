using UnityEngine;
using System.Collections;
using FayvitSounds;

namespace Criatures2021
{
    [System.Serializable]
    public class TempestadeEletrica : AeroImpactBase
    {


        public TempestadeEletrica() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.tempestadeEletrica,
            tipo = PetTypeName.Eletrico,
            carac = AttackDiferentialId.colisaoComPow,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 3,
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
            somDoGolpe = SoundEffectID.Thunder12
        }
            )
        {
            carac = new AeroImpactFeatures(
                ImpactParticles.impactoEletrico,
                AttacksTrails.tempestadeEletrica,
                ToutchingFloor.eletricidadeAoChao,
                PrepareJump.eletricidadeAoChao,
                FinalAeroImpact.MaisAltoQueOAlvo,
                parentearNoOsso:  false,
                onPrepareSound: SoundEffectID.Shot3,
                onTouchGroundSound: SoundEffectID.Slash1
                )
                ;

        }


    }
}