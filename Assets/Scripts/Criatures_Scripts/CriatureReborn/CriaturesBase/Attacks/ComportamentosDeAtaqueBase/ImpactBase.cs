using UnityEngine;
using FayvitMessageAgregator;

namespace Criatures2021
{
    [System.Serializable]
    public class ImpactBase : PetAttackBase
    {
        [System.NonSerialized] private StandardImpactUpdate aImpacto;
        protected ImpactFeatures carac = new ImpactFeatures()
        {
            noImpacto = ImpactParticles.impactoComum,
            nomeTrail = AttacksTrails.tresCubos,
            parentearNoOsso = true
        };

        public ImpactBase(PetAttackFeatures C) : base(C) { }

        public override void IniciaGolpe(GameObject G)
        {
            aImpacto = new StandardImpactUpdate();
            aImpacto.ReiniciaAtualizadorDeImpactos();
            //AnimadorCriature.AnimaAtaque(G, Nome.ToString());
            MessageAgregator<MsgRequestAtkAnimation>.Publish(new MsgRequestAtkAnimation() { 
                nomeAnima = Nome.ToString(),
                gameObject = G
            });
        }

        public override void UpdateGolpe(GameObject G,GameObject focado=null)
        {
            aImpacto.ImpactoAtivo(G, this, carac,focado);
        }
    }

    public struct MsgRequestAtkAnimation : IMessageBase
    {
        public GameObject gameObject;
        public string nomeAnima;
    }
}