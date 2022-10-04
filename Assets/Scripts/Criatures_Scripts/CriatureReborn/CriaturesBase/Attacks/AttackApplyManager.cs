using UnityEngine;
using FayvitMessageAgregator;

namespace Criatures2021
{
    public class AttackApplyManager
    {
        private float tempoDecorrido = 0;
        private PetAttackBase esseGolpe;
        private GameObject gameObject;
        private GameObject focado;

        public AttackApplyManager(GameObject G)
        {
            gameObject = G;
        }

        public void StartAttack(PetAttackBase esseGolpe,float tempoDeInstancia,GameObject focado)
        {
            this.focado = focado;
            this.esseGolpe = esseGolpe;
            tempoDecorrido = 0.0f;
            esseGolpe.IniciaGolpe(gameObject);
            tempoDecorrido -= tempoDeInstancia;

            MessageAgregator<MsgInvokeStartAtk>.Publish(new MsgInvokeStartAtk()
            {
                atacado = focado,
                atacante = gameObject,
                atk = esseGolpe
            });
            //GolpePersonagem.RetornaGolpePersonagem(gameObject, esseGolpe.Nome).TempoDeInstancia;
            //gerente = GetComponent<CreatureManager>();
            //ParaliseNoTempo();
        }

        public bool UpdateAttack()
        {
            tempoDecorrido += Time.deltaTime;

            if (tempoDecorrido > esseGolpe.TempoDeMoveMin /*&& gerente.Estado == CreatureManager.CreatureState.aplicandoGolpe*/)
            {
                esseGolpe.UpdateGolpe(gameObject,focado);
            }
            //else if (gerente.Estado == CreatureManager.CreatureState.emDano)
            //{
            //    FinalizaGolpe();
            //}

            if (tempoDecorrido > esseGolpe.TempoDeMoveMax /*&& !retornou*/)
            {
                if (esseGolpe.Caracteristica == AttackDiferentialId.projetil)
                {
                    MessageAgregator<MsgFreedonAfterAttack>.Publish(new MsgFreedonAfterAttack() { gameObject = gameObject });
                    return true;
                    //LiberaDoAtacando();
                    //Destroy(this, 2);
                }
                else if (tempoDecorrido > esseGolpe.TempoDeDestroy)
                {
                    esseGolpe.FinalizaEspecificoDoGolpe();
                    MessageAgregator<MsgFreedonAfterAttack>.Publish(new MsgFreedonAfterAttack() { gameObject = gameObject });
                    return true;
                }
            }

            return false;
        }

        public static bool CanStartAttack(PetBase meuCriatureBase,System.Action onEmptyStamina=null,System.Action onNotHavingPE=null)
        {
            PetAtributes A = meuCriatureBase.PetFeat.meusAtributos;
            PetAttackManager ggg = meuCriatureBase.GerenteDeGolpes;
            PetAttackBase gg = ggg.meusGolpes[ggg.golpeEscolhido];

            if (
                meuCriatureBase.StManager.VerifyStaminaAction()
                && A.PE.Corrente >= gg.CustoPE)
            {
                A.PE.Corrente -= gg.CustoPE;

                meuCriatureBase.StManager.ConsumeStamina((uint)gg.CustoDeStamina);

                return true;
            }
            else
            {
                if (!meuCriatureBase.StManager.VerifyStaminaAction())
                    onEmptyStamina?.Invoke();
                else if (A.PE.Corrente < gg.CustoPE)
                    onNotHavingPE?.Invoke();

                return false;
            }
        }

        public void InterruptAtk()
        {
            esseGolpe.InterromperGolpe(gameObject);
        }
    }

    public struct MsgFreedonAfterAttack : IMessageBase {
        public GameObject gameObject;
    }

    public struct MsgInvokeStartAtk : IMessageBase {
        public GameObject atacante;
        public GameObject atacado;
        public PetAttackBase atk;
    }
}