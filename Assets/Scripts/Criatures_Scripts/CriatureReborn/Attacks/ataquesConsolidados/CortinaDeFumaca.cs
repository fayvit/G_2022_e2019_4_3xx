using UnityEngine;

namespace Criatures2021
{
    [System.Serializable]
    public class CortinaDeFumaca : HitNoChaoBase
    {

        public CortinaDeFumaca() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.cortinaDeFumaca,
            tipo = PetTypeName.Gas,
            carac = AttackDiferentialId.hitNoChao,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 3,
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

            noImpacto = ImpactParticles.impactoDeGas;

        }


    }
}