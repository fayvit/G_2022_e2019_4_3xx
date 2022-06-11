using UnityEngine;
using FayvitMessageAgregator;
using FayvitBasicTools;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class HitNoChaoBase : PetAttackBase
    {
        private bool addView = false;
        private float tempoDecorrido = 0;
        protected ImpactParticles noImpacto = ImpactParticles.impactoComum;
        [System.NonSerialized] private CharacterController controle;

        public HitNoChaoBase(PetAttackFeatures C) : base(C) { }

        public override void IniciaGolpe(GameObject G)
        {
            addView = false;
            tempoDecorrido = 0;
            DirDeREpulsao = G.transform.forward;

            ApareceDesaparece(false, G);

            InstanceSupport.InstancieEDestrua(Nome, G.transform.position, G.transform.forward, 10);

            //MonoBehaviour.Destroy(
            //    MonoBehaviour.Instantiate(
            //    Resources.Load<GameObject>(Nome.ToString()),
            //    G.transform.position,
            //    Quaternion.identity),
            //    10);

        }

        public override void UpdateGolpe(GameObject G, GameObject focado = null)
        {
            tempoDecorrido += Time.deltaTime;
            if (focado == null)
            {
                Transform T = FindBestTarget.Procure(G, new string[1] { "Criature" });
                focado = T?.gameObject;
            }
            ApareceComHitNoChao(G,focado);
        }


        void ApareceDesaparece(bool aparecer, GameObject G)
        {
            SkinnedMeshRenderer[] skinned = G.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer sk in skinned)
            {
                sk.enabled = aparecer;
            }

            if (!controle)
                controle = G.GetComponent<CharacterController>();

            controle.enabled = aparecer;

            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = SomDoGolpe
            });
        }

        void ApareceComHitNoChao(GameObject gameObject,GameObject focado)
        {
            if (!addView)
            {
                addView = true;
                Transform alvo = focado != null ? focado.transform : null;
                //CreatureManager aAlvo = gameObject.name == "CriatureAtivo"
                //    ?
                //    GameController.g.InimigoAtivo
                //    :
                //    GameController.g.Manager.CriatureAtivo;

                //Transform alvo = aAlvo != null ? aAlvo.transform : null;

                PetManager aAlvo=null;
                if (alvo!=null)
                     aAlvo = alvo.GetComponent<PetManager>();

                Vector3 volta = gameObject.transform.position;

                if (aAlvo != null)
                {
                    volta = alvo.position;
                    DirDeREpulsao = Vector3.ProjectOnPlane(alvo.position - gameObject.transform.position, Vector3.up).normalized;

                    bool b = aAlvo.Mov.IsGrounded;
                    bool c = alvo.GetComponent<CharacterController>().enabled;

                    InsertImpactView.Insert(noImpacto, alvo.position, Quaternion.identity);
                    //MonoBehaviour.Destroy(
                    //    MonoBehaviour.Instantiate(
                    //    Resources.Load<GameObject>(noImpacto.ToString()),
                    //    alvo.position,
                    //    Quaternion.identity),
                    //    10);

                    Debug.Log("hitNoChao: " + b + " : " + c);
                    if (b && c)
                        DamageManager.VerificaDano(aAlvo.gameObject, gameObject, this);

                }

                InstanceSupport.InstancieEDestrua(Nome, volta,default, 10);
                //MonoBehaviour.Destroy(
                //    MonoBehaviour.Instantiate(
                //    Resources.Load<GameObject>(Nome.ToString()),
                //    volta,
                //    Quaternion.identity),
                //    10);

                gameObject.transform.position = MelhoraInstancia3D.ProcuraPosNoMapa(volta);// new melhoraPos().novaPos(volta);
                ApareceDesaparece(true, gameObject);
            }
        }
    }
}