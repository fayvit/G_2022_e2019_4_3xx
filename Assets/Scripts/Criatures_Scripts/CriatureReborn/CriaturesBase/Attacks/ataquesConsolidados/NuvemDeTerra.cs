using FayvitSounds;

namespace Criatures2021
{
    [System.Serializable]
    public class NuvemDeTerra : AeroImpulseBase
    {
        public NuvemDeTerra() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.nuvemDeTerra,
            tipo = PetTypeName.Terra,
            carac = AttackDiferentialId.aeroImpulse,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 6,
            potenciaCorrente = 34,
            potenciaMaxima = 44,
            potenciaMinima = 24,
            //tempoDeReuso = 7.5f,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 15f,
            distanciaDeRepulsao = 65f,
            velocidadeDeRepulsao = 126,
            tempoDeMoveMin = 0,//74
            tempoDeMoveMax = .33f,
            tempoDeDestroy = 1.58f,
            colisorScale = 2,
            custoDeStamina = 60
        }
            )
        {
            carac = new AeroImpulseFeatures()
            {
                noImpacto = ImpactParticles.impactoDeTerra,
                nomeTrail = AttacksTrails.nuvemDeTerra,
                particulaDoInicio = GeneralParticles.preparaNuvemDeTerra,
                somDoInicio = SoundEffectID.Wind11,
                parentearNoOsso = true,
                velocidadeSubindo = 15
            };
        }


    }

}
