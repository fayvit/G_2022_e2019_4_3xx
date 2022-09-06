using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class VingancaDaTerra : ProjetilBase
    {
        public VingancaDaTerra() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.vingancaDaTerra,
            tipo = PetTypeName.Terra,
            carac = AttackDiferentialId.projetil,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 2,
            potenciaCorrente = 25,
            potenciaMaxima = 29,
            potenciaMinima = 21,
            //tempoDeReuso = 7.5f,
            tempoDeMoveMax = 1,
            tempoDeMoveMin = 0.3f,
            tempoDeDestroy = 2,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 18,
            podeNoAr = true,
            custoDeStamina=50,
            somDoGolpe = FayvitSounds.SoundEffectID.Slash2
        }
            )
        {
            carac = new ProjetilFeatures()
            {
                noImpacto = ImpactParticles.impactoDeTerra,
                tipo = ProjetilType.rigido
            };
        }

    }
}