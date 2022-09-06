using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;
using FayvitBasicTools;
using FayvitSounds;

namespace Criatures2021
{
    [System.Serializable]
    public class SobreVoo : PetAttackBase
    {
        private ImpactFeatures carac = new ImpactFeatures()
        {
            noImpacto = ImpactParticles.impactoDeVento,
            nomeTrail = AttacksTrails.umCuboETrail,
            parentearNoOsso = true
        };
        private bool addView = false;
        private bool procurouAlvo = false;
        private float tempoDecorrido = 0;
        private float velocidadeSubindo = 5;
        [System.NonSerialized] private CharacterController controle;
        [System.NonSerialized] private Transform alvoProcurado;
        [System.NonSerialized] private Vector3 posInicial;

        public SobreVoo() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.sobreVoo,
            tipo = PetTypeName.Voador,
            carac = AttackDiferentialId.colisaoComPow,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 7,
            potenciaCorrente = 34,
            potenciaMaxima = 44,
            potenciaMinima = 24,
            //tempoDeReuso = 7.5f,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 30f,
            distanciaDeRepulsao = 65f,
            velocidadeDeRepulsao = 126,
            tempoDeMoveMin = 0,//74
            tempoDeMoveMax = 1f,
            tempoDeDestroy = 1.85f,
            colisorScale = 2,
            custoDeStamina = 60
        }
            )
        {

        }

        public override void IniciaGolpe(GameObject G)
        {
            addView = false;
            tempoDecorrido = 0;
            procurouAlvo = false;
            posInicial = G.transform.position;
            DirDeREpulsao = G.transform.forward;

            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = SoundEffectID.Wind1
            });

            MessageAgregator<MsgRequestAtkAnimation>.Publish(new MsgRequestAtkAnimation()
            {
                gameObject = G,
                nomeAnima = Nome.ToString()
            });
            //AnimadorCriature.AnimaAtaque(G, Nome.ToString());

            MonoBehaviour.Destroy(
            MonoBehaviour.Instantiate(
                    Resources.Load<GameObject>("particles/" + GeneralParticles.subindoSobreVoo.ToString())/*GameController.g.El.retorna("subindoSobreVoo")*/,
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
                controle.Move(Vector3.up * velocidadeSubindo *controleDifVel* Time.deltaTime);
            }

            if (tempoDecorrido > TempoDeMoveMax && !procurouAlvo)
            {
                if (!procurouAlvo)
                {
                    if (focado)
                        alvoProcurado = focado.transform;
                    else
                        alvoProcurado = FindBestTarget.Procure(G, new string[1] { "Criature" },100);
                }
                //alvoProcurado = CriaturesPerto.procureUmBomAlvo(G, 100);

                if (alvoProcurado)
                {
                    if (Vector3.SqrMagnitude(
                        Vector3.ProjectOnPlane(G.transform.position - alvoProcurado.position, Vector3.up)) < 25)
                    {
                        alvoProcurado = null;
                    }

                    G.transform.LookAt(alvoProcurado, Vector3.up);
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

                if (alvoProcurado )
                {
                    float dot = Vector3.Dot(G.transform.forward, DirectionOnThePlane.NormalizedInTheUp(alvoProcurado.position - G.transform.position));
                    //Debug.Log("Dot: " + dot);
                    if (Vector3.ProjectOnPlane(posInicial - alvoProcurado.position, Vector3.up).sqrMagnitude > 25 && dot>.75f)
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

}