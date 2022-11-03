using FayvitSounds;


namespace Criatures2021
{
    [System.Serializable]
    public class ImpulsoEletrico : DelayedCollisionAttack
    {
        public ImpulsoEletrico() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.impulsoEletrico,
            tipo = PetTypeName.Eletrico,
            carac = AttackDiferentialId.colisaoComAtraso,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 3,
            potenciaCorrente = 15,
            potenciaMaxima = 19,
            potenciaMinima = 11,
            //tempoDeReuso = 8.5f,
            TempoNoDano = 1.25f,
            velocidadeDeGolpe = 20f,
            distanciaDeRepulsao = 65f,
            velocidadeDeRepulsao = 66,
            tempoDeMoveMin = 1f,//74
            tempoDeMoveMax = 2.5f,
            tempoDeDestroy = 2.53f,
            custoDeStamina = 60,
            somDoImpacto = SoundEffectID.XP_Knock04,
            somDoGolpe = SoundEffectID.Thunder11
        })
        {
            feat = new DelayedCollisionFeatures()
            {
                onPrepareSound = SoundEffectID.Collapse3,
                noImpacto = ImpactParticles.impactoEletrico,
                prepara = GeneralParticles.preparaImpulsoEletrico,
                trail = AttacksTrails.impulsoEletrico
            };
        }

        
    }

}
