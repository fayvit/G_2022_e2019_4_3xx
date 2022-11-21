using UnityEngine;
using FayvitMessageAgregator;
using FayvitBasicTools;
using FayvitSounds;

namespace Criatures2021
{
    [System.Serializable]
    public class SimpleAreaAttack : PetAttackBase
    {
        private bool iniciouAnimacao;
        private bool addView;
        private float tempoDecorrido;
        protected AreaFeatures feat;

        public SimpleAreaAttack(PetAttackFeatures C) : base(C) { }

        public override void IniciaGolpe(GameObject G)
        {
            addView = false;
            iniciouAnimacao = false;
            tempoDecorrido = 0;
            MessageAgregator<MsgRequestAtkAnimation>.Publish(new MsgRequestAtkAnimation()
            {
                gameObject = G,
                nomeAnima = "prepara" + Nome
            });

            GameObject instancia = Resources.Load<GameObject>("particles/" + feat.prepara.ToString());

            MonoBehaviour.Destroy(
            MonoBehaviour.Instantiate(instancia, G.transform.position, Quaternion.LookRotation(G.transform.forward)), 5);

            MessageAgregator<MsgRequest3dSound>.Publish(new MsgRequest3dSound()
            {
                sfxId = SomDoGolpe,
                sender = G.transform
            });
        }

        public override void UpdateGolpe(GameObject G, GameObject focado = null)
        {
            if (!iniciouAnimacao)
            {
                tempoDecorrido = TempoDeMoveMin;
                iniciouAnimacao = true;

                FayvitSupportSingleton.SupportSingleton.Instance.InvokeInSeconds(() =>
                {
                    MessageAgregator<MsgRequestAtkAnimation>.Publish(new MsgRequestAtkAnimation()
                    {
                        gameObject = G,
                        nomeAnima = Nome.ToString()
                    });
                }, 0.5F * (TempoDeMoveMax - TempoDeMoveMin));
            }

            tempoDecorrido += Time.deltaTime;

            if (tempoDecorrido > TempoDeMoveMax && !addView)
            {
                addView = true;
                GameObject KY = InstanceSupport.InstancieEDestrua("DamageColliders/" + feat.trail.ToString(),
                    G.transform.position, G.transform.forward,
                    feat.timeToDestroy);

                KY.transform.localScale *= VelocidadeDeGolpe;

                DamageColliderBase dcb = DamageColliderFactory.Get(KY, ProjetilType.area);
                dcb.dono = G;
                dcb.esseGolpe = this;
                dcb.noImpacto = feat.noImpacto;
                dcb.velocidadeProjetil = 0;

                for (int i = 0; i < feat.radialCount; i++)
                    for (int j = 0; j < feat.horizontalSpawns; j++)
                    {
                        Vector3 desl = Quaternion.Euler(0, (float)360 / feat.radialCount * i, 0)
                            * G.transform.forward * (0.5f * VelocidadeDeGolpe / feat.horizontalSpawns * (j + 1));
                        InstanceSupport.InstancieEDestrua("particles/" + feat.damageParticle.ToString(),
                       MelhoraInstancia3D.ProcuraPosNoMapa(G.transform.position + desl) + 0.15f * Vector3.down, desl,
                        5);
                    }

                MessageAgregator<MsgRequest3dSound>.Publish(new MsgRequest3dSound()
                {
                    sfxId = feat.onGroundSound,
                    sender = G.transform
                });
            }
        }
    }

    [System.Serializable]
    public struct AreaFeatures
    {
        public ImpactParticles noImpacto;
        public GeneralParticles prepara;
        public GeneralParticles damageParticle;
        public SoundEffectID onGroundSound;
        public AttacksTrails trail;
        public int horizontalSpawns;
        public int radialCount;
        public float timeToDestroy;
    }
}