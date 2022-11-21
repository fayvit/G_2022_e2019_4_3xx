using FayvitSounds;

namespace Criatures2021
{
    [System.Serializable]
    public class TerraVenenosa : SimpleAreaAttack
    {
        public TerraVenenosa() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.terraVenenosa,
            tipo = PetTypeName.Veneno,
            carac = AttackDiferentialId.area,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 6,
            potenciaCorrente = 37,
            potenciaMaxima = 48,    
            potenciaMinima = 26,
            //tempoDeReuso = 5,
            tempoDeMoveMax = 1f,
            tempoDeMoveMin = 0.3f,
            tempoDeDestroy = 2f,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 17,//usando velocidade de golpe para escala da area
            podeNoAr = false,
            ignoreDodge = true,
            custoDeStamina = 45,
            somDoImpacto = SoundEffectID.XP_Knock04,
            somDoGolpe = SoundEffectID.Explosion5
        })
        {
            feat = new AreaFeatures
            {
                noImpacto = ImpactParticles.impactoVenenoso,
                prepara = GeneralParticles.impulsoVenenoso,
                damageParticle = GeneralParticles.particulaTerraVenenosa,
                onGroundSound = SoundEffectID.XP_Ice03,
                trail = AttacksTrails.simpleArea,
                horizontalSpawns = 3,
                radialCount = 7,
                timeToDestroy = 1
            };
        }

        public override void VerificaAplicaStatus(PetBase atacante, PetManager cDoAtacado)
        {
            VerifyApplyPoisonStatus.Verify(atacante, cDoAtacado, this, 3);
        }
    }
}
