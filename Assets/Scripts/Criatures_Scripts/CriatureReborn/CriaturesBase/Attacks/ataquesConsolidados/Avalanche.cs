using UnityEngine;

namespace Criatures2021
{
    [System.Serializable]
    public class Avalanche : AeroImpactBase
    {


        public Avalanche() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.avalanche,
            tipo = PetTypeName.Pedra,
            carac = AttackDiferentialId.colisaoComPow,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 3,
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
            custoDeStamina=60
        }
            )
        {
            carac = new AeroImpactFeatures(
                ImpactParticles.impactoDePedra,
                AttacksTrails.avalanche,
                ToutchingFloor.impactoDePedraAoChao,
                PrepareJump.preparaImpactoAoChao,
                FinalAeroImpact.MaisAltoQueOAlvo,
                onPrepareSound: FayvitSounds.SoundEffectID.Shot3,
                onTouchGroundSound: FayvitSounds.SoundEffectID.Slash1
                );

        }


    }
}