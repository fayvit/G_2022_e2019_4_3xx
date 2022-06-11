using FayvitBasicTools;
using FayvitMessageAgregator;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Criatures2021
{


    [Serializable]
    public class EletricProjectleBase : PetAttackBase
    {
        [NonSerialized] protected bool addView = false;
        [NonSerialized] protected float tempoDecorrido = 0;
        [NonSerialized] private List<Transform> projeteis = new List<Transform>();

        protected ProjetilFeatures carac = new ProjetilFeatures()
        {
            noImpacto = ImpactParticles.impactoEletrico,
            tipo = ProjetilType.rigido
        };

        public EletricProjectleBase(PetAttackFeatures container) : base(container) { }

        public override void IniciaGolpe(GameObject G)
        {
            addView = false;
            tempoDecorrido = 0;
            carac.posInicial = EmissionPosition.Get(G, this.Nome);
            DirDeREpulsao = G.transform.forward;
            projeteis = new List<Transform>();

            MessageAgregator<MsgRequestAtkAnimation>.Publish(new MsgRequestAtkAnimation()
            {
                gameObject = G,
                nomeAnima = "emissor"
            });

            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = SomDoGolpe
            });
        }

        protected void InstanciaEletricidade(GameObject G, Vector3 paraOnde, float tempoMax = 10)
        {
            PetAttackDb golpeP = PetAttackDb.RetornaGolpePersonagem(G, Nome);

            if (golpeP.TempoDeInstancia > 0)
                carac.posInicial = EmissionPosition.Get(G, Nome);

            GameObject KY = InstanceSupport.InstancieEDestrua(GeneralParticles.raioEletrico, carac.posInicial, TempoDeDestroy, DirDeREpulsao);
            Transform KXY = KY.transform.GetChild(0);
            MonoBehaviour.Destroy(KXY.gameObject, 4.9f);

            KXY.parent = G.transform.Find(golpeP.Colisor.osso).transform;
            KXY.localPosition = Vector3.zero;
            projeteis.Add(KY.transform);
            EletricProjectile proj = KY.transform.GetChild(2).gameObject.AddComponent<EletricProjectile>();
            proj.transform.position += golpeP.DistanciaEmissora * G.transform.forward + golpeP.AcimaDoChao * Vector3.up;
            proj.KXY = KXY;
            proj.criatureAlvo = FindBestTarget.Procure(G, new string[1] { "Criature" }, 350);
            proj.forcaInicial = paraOnde.normalized;
            proj.velocidadeProjetil = 9;
            proj.tempoMax = tempoMax;
            proj.noImpacto = carac.noImpacto;
            proj.dono = G;
            proj.esseGolpe = this;
            addView = true;
        }
    }
}
