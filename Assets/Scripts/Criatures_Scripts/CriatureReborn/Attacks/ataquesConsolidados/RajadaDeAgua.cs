using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class RajadaDeAgua : ProjetilBase
    {

        public RajadaDeAgua() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.rajadaDeAgua,
            tipo = PetTypeName.Agua,
            carac = AttackDiferentialId.projetil,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 2,
            potenciaCorrente = 15,
            potenciaMaxima = 19,
            potenciaMinima = 11,
            //tempoDeReuso = 5,
            tempoDeMoveMax = 1,
            tempoDeMoveMin = 0.3f,
            tempoDeDestroy = 2,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 10,
            custoDeStamina = 40,
            podeNoAr=false,
            somDoImpacto = FayvitSounds.SoundEffectID.XP_Knock04,
            somDoGolpe = FayvitSounds.SoundEffectID.rajadaDeAgua
        }
            )
        {
            carac = new ProjetilFeatures()
            {
                noImpacto = ImpactParticles.impactoDeAgua,
                tipo = ProjetilType.basico
            };
        }
    }
}