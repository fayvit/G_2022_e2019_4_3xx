using FayvitSounds;

namespace Criatures2021
{
    [System.Serializable]
    public class PedraPartida : SimpleAreaAttack
    {
        public PedraPartida() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.pedraPartida,
            tipo = PetTypeName.Pedra,
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
            velocidadeDeGolpe = 15,//usando velocidade de golpe para escala da area
            podeNoAr = false,
            ignoreDodge=true,
            custoDeStamina = 45,
            somDoImpacto = SoundEffectID.XP_Knock04,
            somDoGolpe = SoundEffectID.Explosion5
        })
        {
            feat = new AreaFeatures
            {
                noImpacto = ImpactParticles.impactoDePedra,
                prepara = GeneralParticles.preparaImpactoAoChao,
                damageParticle = GeneralParticles.particulaPedraPartida,
                onGroundSound = SoundEffectID.XP131_Earth03,
                trail = AttacksTrails.simpleArea,
                horizontalSpawns = 3,
                radialCount = 10,
                timeToDestroy=1
            };
        }
    }   
}
