using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class RajadaDeFogo : ProjetilBase
    {
        public RajadaDeFogo() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.rajadaDeFogo,
            tipo = PetTypeName.Fogo,
            carac = AttackDiferentialId.projetil,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 3,
            potenciaCorrente = 25,
            potenciaMaxima = 29,
            potenciaMinima = 21,
            //tempoDeReuso = 7.5f,
            tempoDeMoveMax = 1,
            tempoDeMoveMin = 0.3f,
            tempoDeDestroy = 2,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 18,
            custoDeStamina = 50,
            somDoImpacto = FayvitSounds.SoundEffectID.XP_Knock04,
            somDoGolpe = FayvitSounds.SoundEffectID.rajadaDeAgua
        }
        )
        {
            carac = new ProjetilFeatures()
            {
                noImpacto = ImpactParticles.impactoDeFogo,
                tipo = ProjetilType.rigido
            };
        }
    }
}
