using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class BombaDeGas : ProjetilBase
    {
        public BombaDeGas() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.bombaDeGas,
            tipo = PetTypeName.Gas,
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
            custoDeStamina = 40
        }
            )
        {
            carac = new ProjetilFeatures()
            {
                noImpacto = ImpactParticles.impactoDeGas,
                tipo = ProjetilType.basico
            };
        }

    }
}