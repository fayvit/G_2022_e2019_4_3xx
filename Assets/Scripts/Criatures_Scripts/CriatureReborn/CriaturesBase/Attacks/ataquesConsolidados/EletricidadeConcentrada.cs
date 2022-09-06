using FayvitSounds;
using UnityEngine;

namespace Criatures2021
{
    [System.Serializable]
    public class EletricidadeConcentrada : EletricProjectleBase
    {

        public EletricidadeConcentrada() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.eletricidadeConcentrada,
            tipo = PetTypeName.Eletrico,
            carac = AttackDiferentialId.projetil,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 2,
            potenciaCorrente = 25,
            potenciaMaxima = 29,
            potenciaMinima = 21,
            //tempoDeReuso = 7.5f,
            tempoDeMoveMax = 1,
            tempoDeMoveMin = 0,
            tempoDeDestroy = 2,
            TempoNoDano = 1.05f,
            velocidadeDeGolpe = 30,
            custoDeStamina = 50,
            somDoGolpe = SoundEffectID.Thunder12

        }
            )
        {

        }

        public override void UpdateGolpe(GameObject G, GameObject focado = null)
        {
            carac.posInicial += Vector3.up * 0.25f;
            if (!addView)
            {
                InstanciaEletricidade(G, G.transform.forward, 1);
                addView = true;
            }
        }
    }
}

