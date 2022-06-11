using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;
using FayvitBasicTools;

namespace Criatures2021
{

    [System.Serializable]
    public class ProjetilBase : PetAttackBase
    {
        private bool addView = false;

        protected ProjetilFeatures carac = new ProjetilFeatures()
        {
            noImpacto = ImpactParticles.impactoComum,
            tipo = ProjetilType.basico
        };

        protected bool AnimaEmissor { get; set; } = true;

        public ProjetilBase(PetAttackFeatures C) : base(C) { }

        public override void IniciaGolpe(GameObject G)
        {
            addView = false;            

            carac.posInicial = EmissionPosition.Get(G,Nome);

            DirDeREpulsao = G.transform.forward;

            string animacao = "emissor";
            if (!AnimaEmissor)
                animacao = Nome.ToString();

            MessageAgregator<MsgRequestAtkAnimation>.Publish(new MsgRequestAtkAnimation()
            {
                gameObject = G,
                nomeAnima = animacao
            });

            MessageAgregator<MsgRequest3dSound>.Publish(new MsgRequest3dSound()
            {
                sender = G.transform,
                sfxId = SomDoGolpe
            });

        }

        public override void UpdateGolpe(GameObject G,GameObject focado = null)
        {
            if (!addView)
            {
                addView = true;
                ProjetilApply.AplicaProjetil(G, this, carac);
            }
        }
    }

}