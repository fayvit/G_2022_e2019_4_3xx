using FayvitSounds;


namespace Criatures2021
{
    [System.Serializable]
    public class AtkEmGiro : DelayedCollisionAttack
    {
        public AtkEmGiro() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.ataqueEmGiro,
            tipo = PetTypeName.Normal,
            carac = AttackDiferentialId.colisaoComAtraso,
            damageAtribute = DamageBaseAtribute.ataque,
            custoPE = 3,
            potenciaCorrente = 33,
            potenciaMaxima = 45,
            potenciaMinima = 21,
            //tempoDeReuso = 8.5f,
            TempoNoDano = 1.25f,
            velocidadeDeGolpe = 40f,
            distanciaDeRepulsao = 65f,
            velocidadeDeRepulsao = 66,
            tempoDeMoveMin = 1f,//74
            tempoDeMoveMax = 1.75f,
            tempoDeDestroy = 1.83f,
            custoDeStamina = 60,
            somDoImpacto = SoundEffectID.XP_Knock04,
            somDoGolpe = SoundEffectID.rajadaDeAgua
        })
        {
            feat = new DelayedCollisionFeatures()
            {
                onPrepareSound = SoundEffectID.Collapse3,
                parentearNoOsso = true,
                noImpacto = ImpactParticles.impactoComum,
                prepara = GeneralParticles.preparaImpactoAoChao,
                trail = AttacksTrails.umCuboETrail
            };
        }


    }

}
