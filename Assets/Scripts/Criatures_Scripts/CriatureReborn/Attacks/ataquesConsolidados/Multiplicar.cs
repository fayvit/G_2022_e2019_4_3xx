using UnityEngine;
using FayvitBasicTools;
using FayvitMessageAgregator;

namespace Criatures2021
{
    [System.Serializable]
    public class Multiplicar : PetAttackBase
    {
        [System.NonSerialized] private CharacterController controle;
        private ImpactFeatures carac;
        private PetAttackDb gP;
        private float tempoDecorrido = 0;
        private bool addView = false;


        public Multiplicar() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.multiplicar,
            tipo = PetTypeName.Inseto,
            carac = AttackDiferentialId.projetil,
            damageAtribute = DamageBaseAtribute.poder,
            custoPE = 3,
            potenciaCorrente = 15,
            potenciaMaxima = 19,
            potenciaMinima = 11,
            //tempoDeReuso = 10f,
            tempoDeMoveMax = 1,
            tempoDeMoveMin = 0.1f,
            tempoDeDestroy = 12,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 18,
            podeNoAr = true,
            custoDeStamina = 60,
            somDoGolpe = FayvitSounds.SoundEffectID.Attack2
        }
            )
        {
            carac = new ImpactFeatures()
            {
                noImpacto = ImpactParticles.impactoDeGosma
            };
        }

        public override void IniciaGolpe(GameObject G)
        {
            tempoDecorrido = 0;
            addView = false;

            gP = PetAttackDb.RetornaGolpePersonagem(G, Nome);
        }

        public override void UpdateGolpe(GameObject G, GameObject focado = null)
        {
            tempoDecorrido += Time.deltaTime;
            MultiplicarInsetos(G);
        }

        void MultiplicarInsetos(GameObject G)
        {
            if (!addView && tempoDecorrido > gP.TempoDeInstancia)
            {
                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                {
                    sfxId = SomDoGolpe
                });
                PetManager C = G.GetComponent<PetManager>();
                GameObject G2 = ResourcesFolders.GetPet(C.MeuCriatureBase.NomeID);// GameController.g.El.retorna(C.MeuCriatureBase.NomeID);
                Vector3 pos = Vector3.zero;
                Transform alvo = FindBestTarget.Procure(G, 450,new string[1] { "Criature"});

                if (alvo)
                    G.transform.rotation = Quaternion.LookRotation(
                        Vector3.ProjectOnPlane(alvo.position - G.transform.position, Vector3.up)
                        );

                for (int i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                            pos = G.transform.position + G.transform.forward + 2 * G.transform.right;

                            break;
                        case 1:

                            pos = G.transform.position + G.transform.forward - 2 * G.transform.right;

                            break;
                        case 2:
                            pos = G.transform.position - G.transform.forward + 3 * G.transform.right;

                            break;
                        case 3:
                            pos = G.transform.position - G.transform.forward - 3 * G.transform.right;
                            break;
                    }

                    G2 = MonoBehaviour.Instantiate(G2, MelhoraInstancia3D.ProcuraPosNoMapa(pos), G.transform.rotation) as GameObject;
                    if (i == 0)
                    {
                        G2.layer = 10;
                        G2.tag = "Untagged";
                        MultiplyedBehaviour c = G2.AddComponent<MultiplyedBehaviour>();
                        c.Pet = C;
                        c.direcaoMovimento = pos - G.transform.position;
                        c.velocidadeProjetil = C.Mov.WalkSpeed;
                        c.dono = G;
                        c.esseGolpe = this;
                        c.tempoDestroy = Mathf.Max(0.95f * TempoDeDestroy - TempoDeMoveMin, 1);
                        CapsuleCollider caps = G2.AddComponent<CapsuleCollider>();
                        G2.AddComponent<Rigidbody>();
                        caps.isTrigger = true;
                        if (!controle)
                            controle = G.GetComponent<CharacterController>();
                        caps.radius = 2 * controle.radius;
                        caps.height = controle.height;
                        caps.center = controle.center;
                        //					print(procureUmBomAlvo(450));
                        c.alvo = alvo;
                    }
                    else
                    {
                        MultiplyedBehaviour mC = G2.GetComponent<MultiplyedBehaviour>();
                        mC.Pet = C;
                        mC.direcaoMovimento = pos - G.transform.position;
                        mC.esseGolpe = this;
                    }

                    InsertImpactView.Insert(carac.noImpacto, pos, Quaternion.identity);
                }
                addView = true;
            }
        }
    }
}