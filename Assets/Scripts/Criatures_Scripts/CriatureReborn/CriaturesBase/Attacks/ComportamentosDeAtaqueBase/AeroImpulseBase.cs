using FayvitBasicTools;
using FayvitMessageAgregator;
using FayvitSounds;
using UnityEngine;

namespace Criatures2021
{   
    [System.Serializable]
    public class AeroImpulseBase : PetAttackBase
    {
        protected AeroImpulseFeatures carac;
        
        private bool addView = false;
        private bool procurouAlvo = false;
        private float tempoDecorrido = 0;
        
        [System.NonSerialized] private CharacterController controle;
        [System.NonSerialized] private Transform alvoProcurado;

        [System.NonSerialized] private Vector3 posInicial;
        public AeroImpulseBase(PetAttackFeatures C) : base(C) { }

        public override void IniciaGolpe(GameObject G)
        {
            addView = false;
            tempoDecorrido = 0;
            procurouAlvo = false;
            posInicial = G.transform.position;
            DirDeREpulsao = G.transform.forward;

            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId =carac.somDoInicio
            });

            MessageAgregator<MsgRequestAtkAnimation>.Publish(new MsgRequestAtkAnimation()
            {
                gameObject = G,
                nomeAnima = Nome.ToString()
            });
            //AnimadorCriature.AnimaAtaque(G, Nome.ToString());

            MonoBehaviour.Destroy(
            MonoBehaviour.Instantiate(
                    Resources.Load<GameObject>("particles/" + carac.particulaDoInicio.ToString())/*GameController.g.El.retorna("subindoSobreVoo")*/,
                    G.transform.position,
                Quaternion.identity), 0.5f);


        }

        public override void UpdateGolpe(GameObject G, GameObject focado = null)
        {
            tempoDecorrido += Time.deltaTime;
            if (tempoDecorrido < TempoDeMoveMax)
            {
                if (!controle)
                    controle = G.GetComponent<CharacterController>();

                float controleDifVel = Mathf.Max(controle.height, 1);
                controle.Move(Vector3.up * carac.velocidadeSubindo * controleDifVel * Time.deltaTime);
            }

            if (tempoDecorrido > TempoDeMoveMax && !procurouAlvo)
            {
                if (!procurouAlvo)
                {
                    if (focado)
                        alvoProcurado = focado.transform;
                    else
                        alvoProcurado = FindBestTarget.Procure(G, 100, "Criature");
                }
                //alvoProcurado = CriaturesPerto.procureUmBomAlvo(G, 100);

                if (alvoProcurado)
                {
                    if (Vector3.SqrMagnitude(
                        Vector3.ProjectOnPlane(G.transform.position - alvoProcurado.position, Vector3.up)) < 25)
                    {
                        alvoProcurado = null;
                    }

                    G.transform.rotation 
                        = Quaternion.LookRotation(DirectionOnThePlane.InTheUp(alvoProcurado.position-G.transform.position));

                    //G.transform.LookAt(alvoProcurado, Vector3.up);
                }

                procurouAlvo = true;

                Debug.Log("Sobre voo, alvo procurado: " + alvoProcurado);
            }

            if (tempoDecorrido > TempoDeMoveMax && tempoDecorrido < TempoDeDestroy)
            {
                //if (!procurouAlvo)
                //   alvoProcurado = CriaturesPerto.procureUmBomAlvo(G, 100);

                if (!addView)
                {
                    MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                    {
                        sfxId = SomDoGolpe
                    });
                    AttackColliders.AdicionaOColisor(G, this, carac, tempoDecorrido);
                    addView = true;
                }

                Vector3 dir = G.transform.forward + 0.5f * Vector3.down;

                if (alvoProcurado)
                {
                    float dot = Vector3.Dot(G.transform.forward, DirectionOnThePlane.NormalizedInTheUp(alvoProcurado.position - G.transform.position));
                    //Debug.Log("Dot: " + dot);
                    if (Vector3.ProjectOnPlane(posInicial - alvoProcurado.position, Vector3.up).sqrMagnitude > 25 && dot > .75f)
                        dir = alvoProcurado.position - G.transform.position + 0.5f * Vector3.down;
                }

                dir.Normalize();

                controle.Move(dir * VelocidadeDeGolpe * Time.deltaTime);
            }
        }

        public override void FinalizaEspecificoDoGolpe()
        {

        }
    }

    [System.Serializable]
    public struct AeroImpulseFeatures:IImpactFeatures
    {
        public ImpactParticles noImpacto;
        public AttacksTrails nomeTrail;
        public GeneralParticles particulaDoInicio;
        public SoundEffectID somDoInicio;
        public bool parentearNoOsso;
        public float velocidadeSubindo;


        public ImpactParticles NoImpacto => noImpacto;

        public AttacksTrails NomeTrail => nomeTrail;

        public bool ParentearNoOsso => parentearNoOsso;
    }

    public interface IImpactFeatures
    {
        ImpactParticles NoImpacto { get; }
        AttacksTrails NomeTrail { get; }
        bool ParentearNoOsso { get; }
    }



}
