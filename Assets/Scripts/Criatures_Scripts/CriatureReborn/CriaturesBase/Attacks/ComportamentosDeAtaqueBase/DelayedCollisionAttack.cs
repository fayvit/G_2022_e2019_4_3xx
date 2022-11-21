using FayvitBasicTools;
using FayvitMessageAgregator;
using UnityEngine;
using FayvitSounds;

namespace Criatures2021
{
    [System.Serializable]
    public class DelayedCollisionAttack : PetAttackBase
    {
        protected DelayedCollisionFeatures feat;
        private StandardImpactUpdate aImpacto;
        private bool iniciouAnimacao;

        public DelayedCollisionAttack(PetAttackFeatures C) : base(C) { }

        public override void IniciaGolpe(GameObject G)
        {
            MessageAgregator<MsgVerifyEventMessage>.AddListener(OnInvokeEventMessage);
            //tempoDecorrido = 0;
            iniciouAnimacao = false;
            if (aImpacto == null)
                aImpacto = new StandardImpactUpdate();

            aImpacto.ReiniciaAtualizadorDeImpactos();
            DirDeREpulsao = G.transform.forward;

            MessageAgregator<MsgRequestAtkAnimation>.Publish(new MsgRequestAtkAnimation()
            {
                gameObject = G,
                nomeAnima = "prepara" + Nome
            });

            //AnimadorCriature.AnimaAtaque(G, "emissor");

            GameObject instancia = Resources.Load<GameObject>("particles/" + feat.prepara.ToString());

            MessageAgregator<MsgRequest3dSound>.Publish(new MsgRequest3dSound()
            {
                sfxId = feat.onPrepareSound,
                sender = G.transform
            });
            //GameController.g.El.retorna(carac.prepara.ToString());

            MonoBehaviour.Destroy(
            MonoBehaviour.Instantiate(instancia, G.transform.position, Quaternion.LookRotation(G.transform.forward)), 5);
        }

        public override void UpdateGolpe(GameObject G, GameObject focado = null)
        {
            //tempoDecorrido += Time.deltaTime;
            if (!iniciouAnimacao)
            {
                iniciouAnimacao = true;
                MessageAgregator<MsgRequestAtkAnimation>.Publish(new MsgRequestAtkAnimation()
                {
                    gameObject = G,
                    nomeAnima = Nome.ToString()
                });
            }

            aImpacto.ImpactoAtivo(G, this, feat, focado);
        }

        public override void FinalizaEspecificoDoGolpe()
        {
            aImpacto.Interromper(this);
            FayvitSupportSingleton.SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<MsgVerifyEventMessage>.RemoveListener(OnInvokeEventMessage);
            });
        }

        public override void InterromperGolpe(GameObject executor)
        {
            FinalizaEspecificoDoGolpe();
        }

        private void OnInvokeEventMessage(MsgVerifyEventMessage obj)
        {
            if (obj.atk == this)
            {
                FinalizaEspecificoDoGolpe();
            }
        }

    }

    [System.Serializable]
    public struct DelayedCollisionFeatures:IImpactFeatures
    {
        public ImpactParticles noImpacto;
        public AttacksTrails trail;
        public GeneralParticles prepara;
        public SoundEffectID onPrepareSound;
        public bool parentearNoOsso;

        public ImpactParticles NoImpacto => noImpacto;

        public AttacksTrails NomeTrail => trail;

        public bool ParentearNoOsso => parentearNoOsso;
    }
}