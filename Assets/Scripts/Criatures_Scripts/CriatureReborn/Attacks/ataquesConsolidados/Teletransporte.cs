using UnityEngine;

namespace Criatures2021
{
    [System.Serializable]
    public class Teletransporte : HitNoChaoBase
    {

        public Teletransporte() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.teletransporte,
            tipo = PetTypeName.Psiquico,
            carac = AttackDiferentialId.hitNoChao,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 5,
            potenciaCorrente = 34,
            potenciaMaxima = 44,
            potenciaMinima = 24,
            //tempoDeReuso = 10.5f,
            TempoNoDano = 0.5f,
            velocidadeDeGolpe = 25f,
            distanciaDeRepulsao = 65f,
            velocidadeDeRepulsao = 33,
            tempoDeMoveMin = 2,//74
            tempoDeMoveMax = 0.8f,
            tempoDeDestroy = 2.5f,
            custoDeStamina = 60,
            somDoGolpe = FayvitSounds.SoundEffectID.Twine
        }
            )
        {
            noImpacto = ImpactParticles.impactoComum;
        }


    }
}