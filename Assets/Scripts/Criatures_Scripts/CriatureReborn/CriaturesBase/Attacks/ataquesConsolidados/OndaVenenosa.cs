using FayvitSounds;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Criatures2021
{
    [System.Serializable]
    public class OndaVenenosa : ProjetilBase
    {
        public OndaVenenosa() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.ondaVenenosa,
            tipo = PetTypeName.Veneno,
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
            somDoGolpe = SoundEffectID.rajadaDeAgua
        }
        )
        {
            carac = new ProjetilFeatures()
            {
                noImpacto = ImpactParticles.impactoVenenoso,
                tipo = ProjetilType.rigido
            };
        }

        public override void VerificaAplicaStatus(PetBase atacante, PetManager cDoAtacado)
        {
            VerifyApplyPoisonStatus.Verify(atacante, cDoAtacado, this, 2);
        }
    }
}