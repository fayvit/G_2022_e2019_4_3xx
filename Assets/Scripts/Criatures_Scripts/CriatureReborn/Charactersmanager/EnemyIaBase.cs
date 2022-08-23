using UnityEngine;
using System.Collections;
using FayvitMove;
using System.Collections.Generic;
using FayvitMessageAgregator;
using FayvitSupportSingleton;

namespace Criatures2021
{
    [System.Serializable]
    public class EnemyIaBase {

        private Transform transform;
        [SerializeField] private float detectDistance = 5;
        [SerializeField] private float distanceMove = 20;
        [SerializeField] private IaState state = IaState.stand;
        [SerializeField] private float forMoveControlStop = 1;

        [SerializeField]
        private Dictionary<DetectHeroResponse, float> detectResponse = new Dictionary<DetectHeroResponse, float>()
        {
            { DetectHeroResponse.aggresiveTax,.9f },
            { DetectHeroResponse.escapeTax,.05f },
            { DetectHeroResponse.ignoreTax,.05f },
        };

        protected ControlledMoveForCharacter controll;
        protected PetBase myPet;
        private PetManager heroPet;
        private GameObject petOwner;
        private Vector3 movePosition;
        private Vector3 originalPosition;
        

        private float timeCount = 0;        
        private float standTimeToMoveMax = 10;
        private float standTimeToMoveMin = 8;
        private float currenStandTime;

        
        [SerializeField]private float coolDown = 0;
        private bool procurando;
        private const float TEMPO_DE_COOLDOWN = .75f;
        private const float MOD_DISTANCIA_DE_ATAQUE = 14;
        private const float TargetUpdateTax = .5f;

        protected Transform MeuTransform => transform;

        public enum IaState
        { 
            stand,
            standMove,
            rivalElectedWithAgressive,
            stopped,
            atkResponse
        }

        public bool PodeAtualizar { get; set; } = true;

        public Dictionary<DetectHeroResponse, float> DetectResponse => detectResponse;

        public PetManager HeroPet { get => heroPet; }

        public void WaitPetChange(GameObject petOwner)
        {
            
            WaitPetChange(petOwner.GetComponent<CharacterManager>().ActivePet as PetManagerCharacter);
            
        }

        public void WaitPetChange(PetManagerCharacter pet)
        {
            if (pet == heroPet)
            {
                petOwner = pet.T_Dono.gameObject;
                state = IaState.stopped;
                MessageAgregator<MsgChangeToPet>.AddListener(OnChangeToPet);
            }
        }

        private void OnChangeToPet(MsgChangeToPet obj)
        {
            if (obj.dono == petOwner.transform)
            {
                state = IaState.rivalElectedWithAgressive;

                SupportSingleton.Instance.InvokeOnEndFrame(()=> {
                    MessageAgregator<MsgChangeToPet>.RemoveListener(OnChangeToPet);
                });
            }
        }
        
        public virtual void Start(Transform T,PetBase P, ControlledMoveForCharacter controll)
        {
            this.controll = controll;
            myPet = P;
            originalPosition = T.position;
            transform = T;
            SetMovePosition();
            currenStandTime = 0;
        }

        public void ChangeOnAttackResponse(AttackResponse response, GameObject atacante)
        {
            if (response == AttackResponse.aggresive)
            {
                state = IaState.rivalElectedWithAgressive;

                PetManager P = atacante?.GetComponent<PetManager>();

                if (P is PetManagerCharacter)
                {
                    heroPet = P;
                    AproximeEnquantoEspera();
                }
                else
                {
                    ProcuraCriatureDoJogador();
                    AproximeEnquantoEspera();
                }

                MessageAgregator<MsgEnterInAggressiveResponse>.Publish(new MsgEnterInAggressiveResponse()
                {
                    enemyPet = myPet
                });
            }
        }

        void SetMovePosition()
        {
            Vector2 V = Random.insideUnitCircle;
            movePosition = originalPosition + distanceMove*new Vector3(V.x, 0, V.y);
            movePosition = MelhoraInstancia3D.PosEmparedado(movePosition,transform.position);
            
        }

        public void ProcuraCriatureDoJogador()
        {

            if (heroPet == null)
            {
                
                CharacterManager cm = MonoBehaviour.FindObjectOfType<CharacterManager>();

                Debug.Log("entrou na procura: " + cm);
                if (cm)
                {
                    PetManager P = cm.ActivePet;

                    Debug.Log("ActivePet: " + P);

                    if (P)
                        if(P.MeuCriatureBase.PetFeat.meusAtributos.PV.Corrente>0)
                            heroPet = P;
                }
            }

            if (heroPet == null)
            {
                Debug.Log("Reprocurando");
                //GameController.g.StartCoroutine(ProcuraCriatureDoJogadorCo());
                SupportSingleton.Instance.InvokeInSeconds(() =>
                {
                    ProcuraCriatureDoJogador();
                },1);
                procurando = true;
            }
            else
                procurando = false;

        }

        void VerifiqueDetectHeroPet()
        {
            if (heroPet == null)
            {
                heroPet = MonoBehaviour.FindObjectOfType<CharacterManager>().ActivePet;
            }

            if (Vector3.SqrMagnitude(heroPet.transform.position - transform.position) < detectDistance * detectDistance)
            {
                int s = SorteieComPeso.Sorteie(
                    new float[3] {
                        detectResponse[DetectHeroResponse.escapeTax],
                        detectResponse[DetectHeroResponse.aggresiveTax],
                        detectResponse[DetectHeroResponse.ignoreTax]
                    });

                DetectHeroResponse d = (DetectHeroResponse)s;

                if (d != DetectHeroResponse.ignoreTax)
                { 
                
                }
            }
        }

        public void Update()
        {
            switch (state)
            {
                case IaState.stand:
                    
                    timeCount += Time.deltaTime;
                    if (timeCount >currenStandTime)
                    {
                        SetMovePosition();
                        controll.ModificarOndeChegar(movePosition);
                        state = IaState.standMove;
                    }

                    controll.Mov.MoveApplicator(Vector3.zero);
                break;
                case IaState.stopped:
                    controll.Mov.MoveApplicator(Vector3.zero);
                break;
                case IaState.standMove:
                    if (controll.UpdatePosition(forMoveControlStop))
                    {
                        timeCount = 0;
                        state = IaState.stand;
                        currenStandTime = Random.Range(standTimeToMoveMin, standTimeToMoveMax);
                    }
                break;
                case IaState.rivalElectedWithAgressive:
                    AplicaIaDeAtaque();
                    myPet.StManager.StaminaRegen(false);
                break;
                case IaState.atkResponse:
                    UpdateAtkResponse();
                break;
            }
        }

        public void RequestJump(Vector3 dir)
        {
            controll.Mov.MoveApplicator(dir, startJump: true, pressJump: true);
        }

        protected void EnterInAtkResponse() { state = IaState.atkResponse; }
        protected void ExitOutAtkResponse() { state = IaState.rivalElectedWithAgressive; }

        protected virtual void UpdateAtkResponse()
        { 
        
        }

        void VerifiqueSigaOuAtaque(PetAttackBase GB, PetAtributes A)
        {
            if ((heroPet.transform.position - transform.position).magnitude
                >
                MOD_DISTANCIA_DE_ATAQUE *
                (GB.TempoDeMoveMax - GB.TempoDeMoveMin)
                )
            {
                Debug.Log("verificação de status retirada [Refazer]");

                //int numStatus = StatusTemporarioBase.ContemStatus(StatusType.amedrontado, myPet);
                //if (numStatus > -1)
                //{
                //    siga.ModVelocidade = 1 / (float)meuCriature.MeuCriatureBase.StatusTemporarios[numStatus].Quantificador;
                //}
                //else
                //    siga.ModVelocidade = 1;

                timeCount += Time.deltaTime;
                if (timeCount > TargetUpdateTax )
                {
                    timeCount = 0;
                    controll.ModificarOndeChegar(heroPet.transform.position);
                }

                if (controll.UpdatePosition(2.5f))
                {

                }
            }
            else
            {
                
                controll.Mov.MoveApplicator(Vector3.zero);
                if(myPet.StManager.VerifyStaminaAction())
                    Disparador(GB, A);
            }
        }

        void Disparador(PetAttackBase GB, PetAtributes A)
        {
            
            coolDown = TEMPO_DE_COOLDOWN;

            Vector3 olhe = heroPet.transform.position
                - transform.position;
            olhe = new Vector3(olhe.x, 0, olhe.z);
            transform.rotation = Quaternion.LookRotation(olhe);

            if (GB.CustoPE <= A.PE.Corrente)
            {
                AplicaGolpe();
            }
            else
            {
                ProcureColisao();
            }
            
        }

        void ProcureColisao()
        {
            PetAttackManager gg = myPet.GerenteDeGolpes;
            bool foi = false;
            for (int i = 0; i < gg.meusGolpes.Count; i++)
            {
                if (gg.meusGolpes[i].CustoPE == 0)
                {
                    foi = true;
                    gg.golpeEscolhido = i;
                }
            }

            if (foi)
            {
                coolDown = 0;
            }
            else
            {
                myPet.PetFeat.meusAtributos.PE.AumentaAoMaximo();
            }
        }

        void BugDaListaVazia()
        {
            myPet.GerenteDeGolpes.meusGolpes.AddRange(
                myPet.GolpesAtivos(
                    myPet.PetFeat.mNivel.Nivel, myPet.GerenteDeGolpes.listaDeGolpes.ToArray()));
        }

        void AplicaGolpe()
        {

            transform.rotation = Quaternion.LookRotation(
                Vector3.ProjectOnPlane(
                    heroPet.transform.position - transform.position,
                    Vector3.up
                    )
                );

            if (AttackApplyManager.CanStartAttack(myPet))
            {
                coolDown = 0;

                MessageAgregator<MsgEnemyRequestAttack>.Publish(new MsgEnemyRequestAttack()
                {
                    gameObject = transform.gameObject,
                    atk = myPet.GerenteDeGolpes.meusGolpes[myPet.GerenteDeGolpes.golpeEscolhido]
                });

                SorteiaAtk();
            }
        }

        public void SorteiaAtk()
        {
            myPet.GerenteDeGolpes.golpeEscolhido = SorteadorDeGolpes.Sorteia(
                        myPet.NomeID, myPet.GerenteDeGolpes);
        }

        public void OnStartDamageState(Dictionary<AttackResponse,float> atkResponseTax,GameObject atacante)
        {
            AttackResponse s = (AttackResponse)SorteieComPeso.Sorteie(new float[2]
                    {
                        atkResponseTax[AttackResponse.escapeTax],
                        atkResponseTax[AttackResponse.aggresive]
                    });

            ChangeOnAttackResponse(s, atacante);

            SorteiaAtk();

            PodeAtualizar = false;

            coolDown = 0;

            controll.Mov._JumpM.StartFall();
            
        }

        public void OnEndDamageState()
        {
            PodeAtualizar = true;
        }

        void AproximeEnquantoEspera()
        {
            if (heroPet != null)
            {
                Vector3 instancia = heroPet.transform.position + 7 * ((transform.position - heroPet.transform.position).normalized);

                instancia = MelhoraInstancia3D.PosEmparedado(instancia, heroPet.transform.position);

                instancia = MelhoraInstancia3D.ProcuraPosNoMapa(instancia);

                controll.ModificarOndeChegar(instancia);
            }
            else
                Debug.Log("heroPet nulo");
        }

        protected void AplicaIaDeAtaque()
        {
            if (heroPet && heroPet.MeuCriatureBase.PetFeat.meusAtributos.PV.Corrente <= 0)
            {
                WaitPetChange(heroPet as PetManagerCharacter);
            }
                
            if (PodeAtualizar)
                coolDown += Time.deltaTime;


            PetAtributes A = myPet.PetFeat.meusAtributos;
            PetAttackManager gg = myPet.GerenteDeGolpes;

            if (heroPet
                &&
                A.PV.Corrente > 0
                &&
                coolDown > TEMPO_DE_COOLDOWN
                &&
                gg.meusGolpes.Count > 0
                &&
                PodeAtualizar
                )
            {

                PetAttackBase GB = gg.meusGolpes[gg.golpeEscolhido];

                if (GB.Caracteristica == AttackDiferentialId.colisao || GB.Caracteristica == AttackDiferentialId.colisaoComPow)
                {
                    {

                        VerifiqueSigaOuAtaque(GB, A);
                    }
                }
                else
                {
                    controll.Mov.MoveApplicator(Vector3.zero);

                    if(myPet.StManager.VerifyStaminaAction())
                        Disparador(GB, A);
                }
            }
            else if (gg.meusGolpes.Count <= 0)
            {
                controll.Mov.MoveApplicator(Vector3.zero);
                Debug.Log("lista de golpes vazia. POr que??? nivel" + myPet.PetFeat.mNivel.Nivel);
                BugDaListaVazia();
            }
            else if (A.PV.Corrente <= 0)
            {
                controll.Mov.MoveApplicator(Vector3.zero);
                //siga.PareAgora();
            }
            else if (coolDown < TEMPO_DE_COOLDOWN && heroPet!=null)
            {
                if (controll.Mov.IsGrounded)
                {
                    AproximeEnquantoEspera();

                    if (controll.UpdatePosition(2.5f)) { }
                }
                else
                {
                    controll.Mov.MoveApplicator(Vector3.zero);
                }
            }
            else if (!heroPet && !procurando)
            {
                ProcuraCriatureDoJogador();
                controll.Mov.MoveApplicator(Vector3.zero);
            }


        }
    }

    

    public enum AttackResponse
    {
        escapeTax,
        aggresive
    }

    public enum DetectHeroResponse
    {
        escapeTax,
        aggresiveTax,
        ignoreTax
    }

    public struct MsgDetectHero : IMessageBase
    {
        public DetectHeroResponse detectMessage;
        public Transform detector;
    }

    public struct MsgEnemyRequestAttack : IMessageBase
    {
        public GameObject gameObject;
        public PetAttackBase atk;
    }

    public struct MsgEnterInAggressiveResponse : IMessageBase {
        public PetBase enemyPet;
    }
}