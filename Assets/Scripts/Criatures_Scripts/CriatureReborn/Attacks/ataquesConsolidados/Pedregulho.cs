using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class Pedregulho : ProjetilBase
    {

        public Pedregulho() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.pedregulho,
            tipo = PetTypeName.Pedra,
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
            custoDeStamina = 50
        }
            )
        {
            carac = new ProjetilFeatures()
            {
                noImpacto = ImpactParticles.impactoDePedra,
                tipo = ProjetilType.rigido
            };
        }

    }
}
