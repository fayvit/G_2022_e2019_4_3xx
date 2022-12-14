using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class Espada : AeroImpactBase
    {
        public Espada() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.espada,
            tipo = PetTypeName.Normal,
            carac = AttackDiferentialId.colisaoComPow,
            damageAtribute = DamageBaseAtribute.ataque,
            custoPE = 0,
            potenciaCorrente = 13,
            potenciaMaxima = 19,
            potenciaMinima = 9,
            //tempoDeReuso = 3.5f,
            TempoNoDano = 0.5f,
            velocidadeDeGolpe = 18f,
            distanciaDeRepulsao = 65f,
            velocidadeDeRepulsao = 66,
            tempoDeMoveMin = 0.75f,//74
            tempoDeMoveMax = 1.4f,
            tempoDeDestroy = 1.6f,
            custoDeStamina = 45,
            somDoImpacto = FayvitSounds.SoundEffectID.XP_Knock04,
            //somDoGolpe = FayvitSounds.SoundEffectID.rajadaDeAgua
        }
            )
        {
            carac = new AeroImpactFeatures(
                ImpactParticles.impactoComum,
                AttacksTrails.doisCubos,
                ToutchingFloor.poeiraAoVento,
                PrepareJump.impactoBaixo,
                FinalAeroImpact.AvanceEPareAbaixo,
                onPrepareSound: FayvitSounds.SoundEffectID.Shot1,
                onTouchGroundSound: FayvitSounds.SoundEffectID.Slash1
                );

        }
    }
}