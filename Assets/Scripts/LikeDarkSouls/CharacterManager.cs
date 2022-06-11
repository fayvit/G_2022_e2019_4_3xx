using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitBasicTools;
using FayvitMove;
using FayvitCommandReader;
using FayvitMessageAgregator;
using FayvitCam;

namespace FayvitLikeDarkSouls
{
    public class CharacterManager : MonoBehaviour,ICharacterManager
    {
        public CharacterState ThisState { get; private set; } = CharacterState.notStarted;

        [SerializeField] private RollManager roll;
        //[SerializeField] private MotionMoveBehaviour motion;
        [SerializeField] private BasicMove mov;
        [SerializeField] private Controlador inputIndex;
        [SerializeField] private AtkManager atk;
        [SerializeField] private DadosDePersonagem dados;
        [SerializeField] private DamageState estouEmDano = new DamageState();

        private MotionMoveState estado = MotionMoveState.aPasseio;

        private enum MotionMoveState
        {
            aPasseio,
            atacando,
            abertoParaNovoAtk,
            inRoll,
            returnOfRoll,
            inExternalAction,
            estouEmDano,
            inBlock,
            blockDamage
        }

        private ICommandReader CurrentCommander => CommandReader.GetCR(inputIndex);
        public Controlador InputIndex { get => inputIndex; set=> inputIndex = value;  }

        void CreateStMessages()
        {
            StaminaManager st = dados.StManager;

            st.OnChangeStaminaPoints = null;
            st.OnRegenZeroedStamina = null;
            st.OnZeroedStamina = null;

            st.OnChangeStaminaPoints += () => {
                MessageAgregator<MsgChangeST>.Publish(new MsgChangeST()
                {
                    currentSt = st.StaminaPoints,
                    maxSt = st.MaxStaminaPoints,
                    gameObject = gameObject
                });
            };

            st.OnRegenZeroedStamina += () => {
                MessageAgregator<MsgRegenZeroedStamina>.Publish(new MsgRegenZeroedStamina()
                {
                    gameObject = gameObject
                });
            };

            st.OnZeroedStamina += () => {
                MessageAgregator<MsgZeroedStamina>.Publish(new MsgZeroedStamina()
                {
                    gameObject = gameObject
                });
            };
        }

        void Start()
        {
            MessageAgregator<MsgAnimationPointCheck>.AddListener(OnReceivedAnimationCheck);
            //FayvitCommandReaderEventAgregator.AddListener(FayvitCR_EventKey.animationPointCheck, OnReceivedAnimationCheck);
            MessageAgregator<MsgAtkTrigger>.AddListener(OnAtkTrigger);
            //FayvitMoveEventAgregator.AddListener(FayvitMoveEventKey.atkTrigger, OnAtkTrigger);
            MessageAgregator<MsgDamageColliderEnter>.AddListener(OnApplyDamageColliderEnter);
            //EventAgregator.AddListener(EventKey.damageColliderEnter, OnApplyDamageColliderEnter);
            MessageAgregator<MsgRequestDamage>.RemoveListener(OnRequestDamage);
            //EventAgregator.AddListener(EventKey.requestDamage, OnRequestDamage);

            //mov = new BasicMove(new MoveFeatures() { jumpFeat = new JumpFeatures() });
            mov.StartFields(transform);
            estouEmDano.StartFields(mov.Controller);
            //dados = new DadosDePersonagem();
            //atk = new AtkManager();
            //roll = new RollManager();

            dados.StManager.RestartStaminaTimeCount();

            CreateStMessages();
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgAnimationPointCheck>.RemoveListener(OnReceivedAnimationCheck);
            MessageAgregator<MsgAtkTrigger>.RemoveListener(OnAtkTrigger);
            //FayvitCommandReaderEventAgregator.RemoveListener(FayvitCR_EventKey.animationPointCheck, OnReceivedAnimationCheck);
            //FayvitMoveEventAgregator.RemoveListener(FayvitMoveEventKey.atkTrigger, OnAtkTrigger);
            MessageAgregator<MsgDamageColliderEnter>.RemoveListener(OnApplyDamageColliderEnter);
            //EventAgregator.RemoveListener(EventKey.damageColliderEnter, OnApplyDamageColliderEnter);
            MessageAgregator<MsgRequestDamage>.RemoveListener(OnRequestDamage);
            //EventAgregator.RemoveListener(EventKey.requestDamage, OnRequestDamage);
        }

        private void OnAtkTrigger(MsgAtkTrigger obj)
        {
            if (obj.dono == gameObject)
            {
                dados.StManager.ConsumeStamina(40);
            }
        }

        private void OnApplyDamageColliderEnter(MsgDamageColliderEnter obj)
        {
            //GameObject oDanado = obj.MySendObjects[1] as GameObject;
            //GameObject arma = obj.MySendObjects[0] as GameObject;

            if (obj.other.transform.IsChildOf(gameObject.transform) && obj.damageColliderGO != gameObject)
            {
                MessageAgregator<MsgRequestDamage>.Publish(new MsgRequestDamage()
                {
                    oDanado = obj.damageColliderGO,
                    amountDamage = 100,
                    damageDir = obj.varDir
                });
                //EventAgregator.Publish(new GameEvent(EventKey.requestDamage, obj.damageColliderGO, 100,obj.varDir));
            }
        }

        private void OnReceivedAnimationCheck(MsgAnimationPointCheck obj)
        {
            if (obj.sender == gameObject)
            {
                string s = obj.extraInfo;
                switch (s)
                {
                    case "posRollAttack":
                        estado = MotionMoveState.aPasseio;
                        break;
                    case "effects":
#if UNITY_STANDALONE
                        //GamePad.SetVibration(PlayerIndex.One, 0, .25f);
#endif
                        MessageAgregator<RequestShakeCamMessage>.Publish(new RequestShakeCamMessage()
                        {
                            numShake = 3,
                            shakeAngle = .25f,
                            shakeAxis = ShakeAxis.y
                        });
                        //FayvitCamEventAgregator.Publish(
                        //    new FayvitCamEvent(FayvitCamEventKey.requestShakeCamera, gameObject, ShakeAxis.y, 3, .25f));
                    break;
                    case "endVibration":
#if UNITY_STANDALONE
                        //GamePad.SetVibration(PlayerIndex.One, 0, 0);
#endif
                    break;
                    case "OpenNewAtk":
                        atk.OpenNewAtk = true;
                    break;
                    case "VerifyNewAtk":
                        atk.VerifyEndAtk();
                    break;
                    case "endSword":
                        if (atk.EndAnimation())
                            MessageAgregator<MsgEndAtk>.Publish(new MsgEndAtk()
                            {
                                sender = gameObject
                            });
                            //FayvitMoveEventAgregator.Publish(new FayvitMoveEvent(FayvitMoveEventKey.endAttack, gameObject));
                    break;
                }

                atk.OnReceivedAnimationPoint(obj);

            }
        }

        private void OnRequestDamage(MsgRequestDamage obj)
        {
            

            if (obj.oDanado == gameObject)
            {
                Vector3 dirAfastamento = obj.damageDir;
                uint amountDamage = (uint)obj.amountDamage;

                dirAfastamento = new Vector3(dirAfastamento.x, 0, dirAfastamento.z);

                dirAfastamento = dirAfastamento.normalized;

                if (estado == MotionMoveState.inBlock && Vector3.Dot(dirAfastamento, transform.forward) > -.75f)
                {
                    System.Action animationAction = () => {
                        MessageAgregator<MsgBlockHit>.Publish(new MsgBlockHit()
                        {
                            gameObject = gameObject
                        });
                        //FayvitMoveEventAgregator.Publish(new FayvitMoveEvent(FayvitMoveEventKey.blockHit, gameObject));
                    };

                    VerifyDamageThread(amountDamage, dirAfastamento, "BlockHit", MotionMoveState.blockDamage, animationAction);//BlockVerifyDamage(amountDamage, dirAfastamento);
                }
                else if (estado != MotionMoveState.inRoll && !roll.ImunneRoll)
                {
                    System.Action animationAction = () => {
                        StandardHumanoidSupportAnimations.
                            RequestHitAnimation(gameObject, dirAfastamento);
                    };

                    VerifyDamageThread(amountDamage, dirAfastamento, "Damage", MotionMoveState.estouEmDano, animationAction);
                }
            }
        }

        void VerifyDamageThread(uint damageAmount, Vector3 dirAfast, string particle, MotionMoveState nextState, System.Action AnimationAction)
        {
            dados.ApplyDamage(damageAmount);
            GameObject G = Resources.Load<GameObject>(particle);
            Destroy(
                Instantiate(G, transform.position + 1.5f * Vector3.up, Quaternion.identity), 1
                );

            if (dados.LifePoints > 0)
            {
                estouEmDano.Start(-dirAfast);
                estado = nextState;

                AnimationAction();
            }
            else
            {
                Debug.Log("Death_aFazer");
                MessageAgregator<MsgDeathAnimate>.Publish(new MsgDeathAnimate()
                {
                    gameObject = gameObject
                });
                //FayvitMoveEventAgregator.Publish(new FayvitMoveEvent(FayvitMoveEventKey.deathAnimate, gameObject));
            }
        }

        void CameraFreedonReads()
        {
            bool focarCamera = CurrentCommander.GetButtonDown(CommandConverterInt.camFocus);
            if (focarCamera)
            {
                if (mov.LockTarget)
                {
                    mov.LockTarget = null;

                    CameraApplicator.cam.RetornarParaCameraDirecional();
                }
                else
                {
                    string[] tags = new string[2] { "Vermelho", "Roxo" };
                    //List<GameObject> osPerto = FindBestTarget.ProximosDeMim(gameObject, tags);

                    mov.LockTarget = FindBestTarget.Procure(gameObject, tags);
                    //CameraAplicator.cam.SetarInimigosProximosParaFoco(osPerto);

                    if (mov.LockTarget)
                    {
                        CameraApplicator.cam.StartFightCam(transform, mov.LockTarget);
                    }
                }
            }

            CameraApplicator.cam.ValoresDeCamera(
                CurrentCommander.GetAxis("Xcam"),
                CurrentCommander.GetAxis("Ycam"),
                focarCamera,
                mov.Controller.velocity.sqrMagnitude > .1f
                );
        }

        void KombatFreedonReads()
        {

            if (dados.StManager.VerifyStaminaAction())
            {
                if (CurrentCommander.GetIntTriggerDown("triggerR") > 0)
                {
                    //dados.StManager.ConsumeStamina(40);
                    estado = MotionMoveState.atacando;
                    atk.StartAttack(gameObject);

                }
                else

                if (CurrentCommander.GetButtonDown(1))
                {
                    dados.StManager.ConsumeStamina(25);
                    if (roll.Start(CurrentCommander.DirectionalVector(), gameObject))
                        estado = MotionMoveState.inRoll;
                }
            }
        }

        void MoveFreedonReads()
        {
            Vector3 dir = CameraApplicator.cam.SmoothCamDirectionalVector(
                    CurrentCommander.GetAxis("horizontal"),
                    CurrentCommander.GetAxis("vertical")
                    );

            if (CurrentCommander.GetButtonDown(4))
            {
                estado = MotionMoveState.inBlock;
                MessageAgregator<MsgEnterInBlock>.Publish(new MsgEnterInBlock()
                {
                    gameObject = gameObject
                });
            }

            bool run = CurrentCommander.GetButton(2) && dados.StManager.VerifyStaminaAction();

            atk.RunTime = run ? atk.RunTime + Time.deltaTime : 0;

            mov.MoveApplicator(mov.LockTarget ? CurrentCommander.DirectionalVector() : dir,
                run,
                CurrentCommander.GetButtonDown(0),
                CurrentCommander.GetButton(0)
                );
        }

        void FreedonState()
        {
            MoveFreedonReads();

            CameraFreedonReads();

            KombatFreedonReads();


            dados.StManager.StaminaRegen(atk.RunTime != 0 || CurrentCommander.GetButton(2));
        }

        void TestNewAtk()
        {
            /*
            if (Input.GetKeyDown(KeyCode.Space) && state.IndexAtk!=2)
            {
                if (state.IndexAtk == 1)
                    state.IndexAtk = 7;
                else if (state.IndexAtk == 7)
                    state.IndexAtk = 2;

                state.Atk();
                estado = MotionMoveState.atacando;
            }*/
        }

        void InRollState()
        {

            if (roll.Update())
            {
                if (roll.RequestAttack)
                {
                    if (dados.StManager.VerifyStaminaAction())
                    {
                        estado = MotionMoveState.inExternalAction;
                        MessageAgregator<MsgPosRollAtk>.Publish(new MsgPosRollAtk()
                        {
                            sender = gameObject
                        });
                        //FayvitMoveEventAgregator.Publish(new FayvitMoveEvent(FayvitMoveEventKey.posRollAttack, gameObject));
                        dados.StManager.ConsumeStamina(40);
                    }
                    else
                        estado = MotionMoveState.returnOfRoll;
                }
                else
                    estado = MotionMoveState.returnOfRoll;

                mov.MoveApplicator(Vector3.zero);
            }
            else
            {
                if (CurrentCommander.GetButtonDown(5))
                    roll.RequestAttack = true;
                mov.MoveApplicator(roll.DirOfRoll, true);
            }
        }

        void AttackingState()
        {
            bool atkInput = CurrentCommander.GetIntTriggerDown("triggerR") > 0;
            if (!dados.StManager.VerifyStaminaAction())
                atkInput = false;

            if (atk.UpdateAttack(atkInput))
            {
                estado = MotionMoveState.aPasseio;
            }

            CurrentCommander.DirectionalVector();


            mov.MoveApplicator(Vector3.zero);
        }

        void BlockState()
        {
            Vector3 dir = CameraApplicator.cam.SmoothCamDirectionalVector(
                        CurrentCommander.GetAxis("horizontal"),
                        CurrentCommander.GetAxis("vertical")
                        );

            mov.MoveApplicator(mov.LockTarget ? CurrentCommander.DirectionalVector() : dir,
               false,
               false,
               false
               );

            CameraFreedonReads();

            if (CurrentCommander.GetButtonUp(4))
            {
                estado = MotionMoveState.aPasseio;
                MessageAgregator<MsgExitInBlock>.Publish(new MsgExitInBlock()
                {
                    gameObject = gameObject
                });
                //FayvitMoveEventAgregator.Publish(new FayvitMoveEvent(FayvitMoveEventKey.exitInBlock, gameObject));
            }

            dados.StManager.StaminaRegen(false, true);
        }

        void Update()
        {

            switch (estado)
            {
                case MotionMoveState.aPasseio:
                    //inputIndex = ChangeController.ControllerInUse(inputIndex);

                    FreedonState();

                    break;
                case MotionMoveState.abertoParaNovoAtk:
                    TestNewAtk();
                    break;
                case MotionMoveState.inRoll:
                    InRollState();
                    break;
                case MotionMoveState.returnOfRoll:

                    mov.MoveApplicator(Vector3.zero);
                    CurrentCommander.DirectionalVector();
                    if (roll.ReturnTime())
                        estado = MotionMoveState.aPasseio;
                    break;
                case MotionMoveState.atacando:

                    AttackingState();
                    break;
                case MotionMoveState.estouEmDano:
                    if (estouEmDano.Update())
                    {
                        estado = MotionMoveState.aPasseio;
                    }

                    CurrentCommander.DirectionalVector();
                    break;
                case MotionMoveState.inBlock:
                    BlockState();
                    break;
                case MotionMoveState.blockDamage:
                    if (estouEmDano.Update())
                    {
                        if (CurrentCommander.GetButton(4))
                        {
                            MessageAgregator<MsgEnterInBlock>.Publish(new MsgEnterInBlock()
                            {
                                gameObject = gameObject
                            });
                            //FayvitMoveEventAgregator.Publish(new FayvitMoveEvent(FayvitMoveEventKey.enterInBlock, gameObject));
                            estado = MotionMoveState.inBlock;
                        }
                        else
                        {
                            MessageAgregator<MsgExitInBlock>.Publish(new MsgExitInBlock()
                            {
                                gameObject = gameObject
                            });
                            //FayvitMoveEventAgregator.Publish(new FayvitMoveEvent(FayvitMoveEventKey.exitInBlock, gameObject));
                            estado = MotionMoveState.aPasseio;
                        }
                    }

                    CurrentCommander.DirectionalVector();
                    break;
            }

        }


        void FootL()
        {

        }

        void FootR()
        {

        }

        void Hit()
        {
            //      estado = MotionMoveBoltState.abertoParaNovoAtk;
        }

        void EndAtkAnimation()
        {
            //      estado = MotionMoveBoltState.aPasseio;
        }
    }

    public struct MsgEndAtk : IMessageBase
    {
        public GameObject sender;
    }

    public struct MsgPosRollAtk : IMessageBase
    {
        public GameObject sender;
    }

    public struct MsgBlockHit : IMessageBase
    {
        public GameObject gameObject;
    }

    public struct MsgDeathAnimate : IMessageBase
    {
        public GameObject gameObject;
    }

    public struct MsgEnterInBlock : IMessageBase
    {
        public GameObject gameObject;
    }

    public struct MsgExitInBlock : IMessageBase
    {
        public GameObject gameObject;
    }

    public struct MsgRequestDamage : IMessageBase
    {
        public GameObject oDanado;
        public Vector3 damageDir;
        public int amountDamage;
    }
}