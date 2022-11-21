using FayvitSounds;

namespace Criatures2021
{
    [System.Serializable]
    public class SobreVoo : AeroImpulseBase
    {
        public SobreVoo() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.sobreVoo,
            tipo = PetTypeName.Voador,
            carac = AttackDiferentialId.aeroImpulse,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 7,
            potenciaCorrente = 34,
            potenciaMaxima = 44,
            potenciaMinima = 24,
            //tempoDeReuso = 7.5f,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 30f,
            distanciaDeRepulsao = 65f,
            velocidadeDeRepulsao = 126,
            tempoDeMoveMin = 0,//74
            tempoDeMoveMax = 1f,
            tempoDeDestroy = 1.85f,
            colisorScale = 2,
            custoDeStamina = 60
        }
            )
        {
            carac = new AeroImpulseFeatures()
            {
                noImpacto = ImpactParticles.impactoDeVento,
                nomeTrail = AttacksTrails.umCuboETrail,
                particulaDoInicio = GeneralParticles.subindoSobreVoo,
                somDoInicio = SoundEffectID.Wind1,
                parentearNoOsso = true,
                velocidadeSubindo = 5
            };
        }

       
    }

}