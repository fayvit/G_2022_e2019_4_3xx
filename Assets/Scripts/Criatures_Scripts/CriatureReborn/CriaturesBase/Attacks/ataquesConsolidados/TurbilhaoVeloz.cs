using FayvitSounds;


namespace Criatures2021
{
    [System.Serializable]
    public class TurbilhaoVeloz : DelayedCollisionAttack
    {
        public TurbilhaoVeloz() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.turbilhaoVeloz,
            tipo = PetTypeName.Voador,
            carac = AttackDiferentialId. colisaoComAtraso,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 3,
            potenciaCorrente = 15,
            potenciaMaxima = 19,
            potenciaMinima = 11,
            //tempoDeReuso = 8.5f,
            TempoNoDano = 1.25f,
            velocidadeDeGolpe = 25f,
            distanciaDeRepulsao = 65f,
            velocidadeDeRepulsao = 66,
            tempoDeMoveMin = 1f,//74
            tempoDeMoveMax = 2f,
            tempoDeDestroy = 2.03f,
            custoDeStamina = 60,
            somDoImpacto = SoundEffectID.XP_Knock04,
            somDoGolpe = SoundEffectID.rajadaDeAgua
        })
        {
            feat = new DelayedCollisionFeatures()
            {
                onPrepareSound = SoundEffectID.Collapse3,
                parentearNoOsso = true,
                noImpacto = ImpactParticles.impactoDeVento,
                prepara = GeneralParticles.preparaTurbilhaoVeloz,
                trail = AttacksTrails.turbilhaoVeloz
            };
        }

        
    }

}
