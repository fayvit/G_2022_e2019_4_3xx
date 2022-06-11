using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;
using FayvitBasicTools;

namespace Criatures2021
{
    [System.Serializable]
    public class AeroImpactBase : PetAttackBase
    {
        protected AeroImpactFeatures carac;
        [System.NonSerialized] protected AeroImpactUpdater aImpacto = new AeroImpactUpdater();
        public AeroImpactBase(PetAttackFeatures C) : base(C) { }

        public override void IniciaGolpe(GameObject G)
        {

            if (aImpacto == null)
                aImpacto = new AeroImpactUpdater();

            aImpacto.ReiniciaAtualizadorDeImpactos(G);
            DirDeREpulsao = G.transform.forward;

            MessageAgregator<MsgRequestAtkAnimation>.Publish(new MsgRequestAtkAnimation()
            {
                gameObject = G,
                nomeAnima = "emissor"
            });
            //AnimadorCriature.AnimaAtaque(G, "emissor");
            
            GameObject instancia = Resources.Load<GameObject>("particles/" + carac.prepara.ToString());

            MessageAgregator<MsgRequest3dSound>.Publish(new MsgRequest3dSound()
            {
                sfxId = carac.onPrepareSound,
                sender = G.transform
            });
                //GameController.g.El.retorna(carac.prepara.ToString());
            MonoBehaviour.Destroy(
            MonoBehaviour.Instantiate(instancia, G.transform.position, Quaternion.LookRotation(G.transform.forward)), 5);
        }

        public override void UpdateGolpe(GameObject G,GameObject focado = null)
        {
            aImpacto.ImpactoAtivo(G, this, carac);
        }

        public override void FinalizaEspecificoDoGolpe()
        {
            aImpacto.DestruirAo();
        }

    }
}