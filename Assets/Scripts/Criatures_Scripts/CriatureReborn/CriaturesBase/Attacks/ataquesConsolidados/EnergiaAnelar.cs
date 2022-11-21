using FayvitSounds;
using UnityEngine;

namespace Criatures2021
{
    [System.Serializable]
    public class EnergiaAnelar : ExpansiveAreaAttack
    {
        public EnergiaAnelar() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.energiaAnelar,
            tipo = PetTypeName.Normal,
            carac = AttackDiferentialId.expansiveArea,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 6,
            potenciaCorrente = 24,
            potenciaMaxima = 34,
            potenciaMinima = 14,
            //tempoDeReuso = 5,
            tempoDeMoveMax = 1f,
            tempoDeMoveMin = 0.3f,
            tempoDeDestroy = 2f,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 15,
            distanciaDeRepulsao =66,
            velocidadeDeRepulsao=66,
            podeNoAr = false,
            custoDeStamina = 45,
            somDoImpacto = SoundEffectID.XP_Knock04,
            somDoGolpe = SoundEffectID.Explosion5
        })
        {
            feat = new ExpansiveAreaFeatures
            {
                noImpacto = ImpactParticles.impactoComum,
                prepara = GeneralParticles.preparaImpactoAoChao,
                //damageParticle = GeneralParticles.particulaPedraPartida,
                onGroundSound = SoundEffectID.XP_Ice03,
                trail = AttacksTrails.energiaAnelar,                
                timeToDestroy = 1
            };
        }

        protected override void OnInstantiate(GameObject KY)
        {
            KY.transform.rotation = Quaternion.LookRotation(Vector3.up);
        }
    }
}
