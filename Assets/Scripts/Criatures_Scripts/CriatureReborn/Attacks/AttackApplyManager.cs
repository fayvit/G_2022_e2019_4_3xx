using UnityEngine;
using System.Collections;
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

        public static bool CanStartAttack(PetBase meuCriatureBase)
        {
            PetAtributes A = meuCriatureBase.PetFeat.meusAtributos;
            PetAttackManager ggg = meuCriatureBase.GerenteDeGolpes;
            PetAttackBase gg = ggg.meusGolpes[ggg.golpeEscolhido];

            if (
                //gg.UltimoUso + gg.TempoDeReuso < Time.time 
                meuCriatureBase.StManager.VerifyStaminaAction()
                && A.PE.Corrente >= gg.CustoPE)
            {
                A.PE.Corrente -= gg.CustoPE;
                //gg.UltimoUso = Time.time;
                meuCriatureBase.StManager.ConsumeStamina((uint)gg.CustoDeStamina);

                //AplicadorDeGolpe aplG = gameObject.AddComponent<AplicadorDeGolpe>();

                //aplG.esseGolpe = gg;

                //GameController.g.HudM.AtualizaDadosDaHudVida(false);

                //if (GameController.g.estaEmLuta)
                //    GameController.g.HudM.AtualizaDadosDaHudVida(true);

                // if(!GameController.g.estaEmLuta)
                //   //GameController.g.HudM.AtualizaHudHeroi(meuCriatureBase);

                return true;
            }
            else
                return false;
        }
    }

    public struct MsgFreedonAfterAttack : IMessageBase {
        public GameObject gameObject;
    }
}