using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class Cascalho : ProjetilBase
    {
        public Cascalho() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.cascalho,
            tipo = PetTypeName.Pedra,
            carac = AttackDiferentialId.projetil,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 1,
            potenciaCorrente = 15,
            potenciaMaxima = 19,
            potenciaMinima = 11,
            //tempoDeReuso = 5,
            tempoDeMoveMax = 1,
            tempoDeMoveMin = 0.3f,
            tempoDeDestroy = 2,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 10,
            custoDeStamina=40
        }
            )
        {
            carac = new ProjetilFeatures()
            {
                noImpacto = ImpactParticles.impactoDePedra,
                tipo = ProjetilType.basico
            };
        }

    }
}