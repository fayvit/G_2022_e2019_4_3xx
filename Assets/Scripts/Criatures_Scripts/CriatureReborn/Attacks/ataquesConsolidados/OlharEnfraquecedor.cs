using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class OlharEnfraquecedor : ProjetilBase
    {

        public OlharEnfraquecedor() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.olharEnfraquecedor,
            tipo = PetTypeName.Normal,
            carac = AttackDiferentialId.projetil,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 4,
            potenciaCorrente = 1,
            potenciaMaxima = 8,
            potenciaMinima = 1,
            //tempoDeReuso = 30f,
            tempoDeMoveMax = 1,
            tempoDeMoveMin = 0.3f,
            tempoDeDestroy = 4,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 10,//18
            podeNoAr = false,
            custoDeStamina = 60,
            somDoImpacto = FayvitSounds.SoundEffectID.XP_Knock04,
            somDoGolpe = FayvitSounds.SoundEffectID.Slash2
        }
            )
        {
            carac = new ProjetilFeatures()
            {
                noImpacto = ImpactParticles.impactoComum,
                tipo = ProjetilType.statusExpansivel
            };
        }

        public override void VerificaAplicaStatus(PetBase atacante, PetManager atacado)
        {

            StatusTemporarioBase S = new Fraco()
            {
                Dados = new DatesForTemporaryStatus()
                {
                    Quantificador = 2,
                    TempoSignificativo = 250,
                    Tipo = StatusType.fraco //trocavel
                },
                CDoAfetado = atacado,
                OAfetado = atacado.MeuCriatureBase
            };

            int num = StatusTemporarioBase.ContemStatus(StatusType.fraco/*trocavel*/, atacado.MeuCriatureBase);

            AddSimpleStatus.InsereStatusSimples(atacado, S, num);

            Debug.Log("enfraqueceu");

        }
    }
}
