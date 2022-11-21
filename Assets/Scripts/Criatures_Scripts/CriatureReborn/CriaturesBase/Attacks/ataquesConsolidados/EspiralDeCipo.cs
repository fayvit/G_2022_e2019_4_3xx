using FayvitSounds;

namespace Criatures2021
{
    [System.Serializable]
    public class EspiralDeCipo : AeroImpulseBase
    {
        public EspiralDeCipo() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.espiralDeCipo,
            tipo = PetTypeName.Planta,
            carac = AttackDiferentialId.aeroImpulse,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 6,
            potenciaCorrente = 34,
            potenciaMaxima = 44,
            potenciaMinima = 24,
            //tempoDeReuso = 7.5f,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 20f,
            distanciaDeRepulsao = 65f,
            velocidadeDeRepulsao = 126,
            tempoDeMoveMin = 0,//74
            tempoDeMoveMax = .38f,
            tempoDeDestroy = 1.68f,
            colisorScale = 2,
            custoDeStamina = 60
        }
            )
        {
            carac = new AeroImpulseFeatures()
            {
                noImpacto = ImpactParticles.impactoDeFolhas,
                nomeTrail = AttacksTrails.espiralDeCipo,
                particulaDoInicio = GeneralParticles.preparaEspiralDeCipo,
                somDoInicio = SoundEffectID.Wind11,
                parentearNoOsso = true,
                velocidadeSubindo = 12
            };
        }


    }

}
