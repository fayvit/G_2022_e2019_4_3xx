using UnityEngine;
using System.Collections.Generic;
using FayvitCam;
using FayvitCommandReader;
using FayvitBasicTools;
using FayvitMessageAgregator;
using FayvitSupportSingleton;
using Criatures2021Hud;
using TextBankSpace;
using FayvitMove;
using System;

namespace Criatures2021
{
    public class PetManagerCharacter : PetManager
    {

        [SerializeField] private CharacterManager tDono;

        private bool inControll;

        public CharacterManager T_Dono { get => tDono; set => tDono = value; }

        private ICommandReader CurrentCommander
        {
            get => CommandReader.GetCR(AbstractGlobalController.Instance.Control);
        }

        protected override void Start()
        {
            base.Start();
            MessageAgregator<MsgChangeToHero>.AddListener(OnChangeToHero);
            MessageAgregator<MsgChangeToPet>.AddListener(OnChangeToPet);
            MessageAgregator<MsgStartUseItem>.AddListener(OnStartUseItem);
            MessageAgregator<MsgStartReplacePet>.AddListener(OnStartReplacePet);
            MessageAgregator<MsgPetEnterInAttackLearn>.AddListener(OnEnterInAttackLearn);
            MessageAgregator<MsgEndNewAttackHud>.AddListener(OnEndAttackhud);
            MessageAgregator<MsgRequestNonReturnableDamage>.AddListener(OnRequestNonReturnableDamage);
            MessageAgregator<MsgSlopeSlip>.AddListener(OnSlopeSlip);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            MessageAgregator<MsgChangeToHero>.RemoveListener(OnChangeToHero);
            MessageAgregator<MsgChangeToPet>.RemoveListener(OnChangeToPet);
            MessageAgregator<MsgStartUseItem>.RemoveListener(OnStartUseItem);
            MessageAgregator<MsgStartReplacePet>.RemoveListener(OnStartReplacePet);
            MessageAgregator<MsgPetEnterInAttackLearn>.RemoveListener(OnEnterInAttackLearn);
            MessageAgregator<MsgEndNewAttackHud>.RemoveListener(OnEndAttackhud);
            MessageAgregator<MsgRequestNonReturnableDamage>.RemoveListener(OnRequestNonReturnableDamage);
            MessageAgregator<MsgSlopeSlip>.RemoveListener(OnSlopeSlip);
            MeuCriatureBase.StManager.OnRegenZeroedStamina = null;
        }

        private void OnSlopeSlip(MsgSlopeSlip obj)
        {
            if (obj.slipped == gameObject)
            {
                PetAttackBase petAttack =  SlopeSlipManager.Slip(gameObject,obj.hit);

                MessageAgregator<MsgEnterInDamageState>.Publish(new MsgEnterInDamageState()
                {
                    oAtacado = gameObject,
                    golpe = petAttack,
                    atacante = null
                });
            }
        }

        private void OnChangeToHero(MsgChangeToHero obj)
        {
            if (obj.myHero == tDono.gameObject)
            {
                inControll = false;
                State = LocalState.following;
            }
        }

        private void OnRequestNonReturnableDamage(MsgRequestNonReturnableDamage obj)
        {
            if (obj.gameObject == gameObject)
            {
                OnEnterInDamageState(new MsgEnterInDamageState()
                {
                    golpe = obj.petAttack,
                    oAtacado = obj.gameObject
                });

                State = LocalState.nonReturnableDamage;
            }
        }

        private void OnEndAttackhud(MsgEndNewAttackHud obj)
        {
            if (obj.fluxo == FluxoDeRetorno.criature && obj.oAprendiz == MeuCriatureBase)
            {
                State = LocalState.onFree;
            }
        }

        private void OnEnterInAttackLearn(MsgPetEnterInAttackLearn obj)
        {
            if (obj.dono.transform == tDono)
            {
                State = LocalState.NonBlockPanelOpened;
            }
        }

        private void OnStartReplacePet(MsgStartReplacePet obj)
        {
            if (obj.dono == tDono.gameObject)
            {
                State = LocalState.stopped;
            }
        }

        private void OnStartUseItem(MsgStartUseItem obj)
        {
            if (obj.usuario == tDono.gameObject)
            {
                State = LocalState.stopped;
            }
        }

        void VerificaLevelUp()
        {
            GerenciadorDeExperiencia gXp = MeuCriatureBase.PetFeat.mNivel;

            if (gXp.VerificaPassaNivel())
            {
                gXp.AplicaPassaNivel();


                GameObject G = Instantiate(ResourcesFolders.GetGeneralElements(GeneralElements.passouDeNivel), transform.position, Quaternion.identity);
                DamageViewer d = G.GetComponentInChildren<DamageViewer>();
                d.dano = "Nivel " + gXp.Nivel;
                d.atacado = transform;

                Destroy(G, 5);

                PetAtributes P = MeuCriatureBase.PetFeat.meusAtributos;
                PetUpLevel.CalculeUpLevel(gXp.Nivel, P);

                PetAttackDb gp = MeuCriatureBase.GerenteDeGolpes.VerificaGolpeDoNivel(
                    MeuCriatureBase.NomeID, gXp.Nivel
                    );

                if (gp.Nome != AttackNameId.nulo && !MeuCriatureBase.GerenteDeGolpes.TemEsseGolpe(gp.Nome))
                {
                    MeuCriatureBase.GolpesPorAprender.Add(gp);
                }
                else if (gp.Nome != AttackNameId.nulo)
                    gp = new PetAttackDb();

                MessageAgregator<MsgChangeLevel>.Publish(new MsgChangeLevel()
                {
                    newLevel = gXp.Nivel,
                    gameObject = gameObject,
                    peCorrente = P.PE.Corrente,
                    peMaximo = P.PE.Maximo,
                    pvCorrente = P.PV.Corrente,
                    pvMax = P.PV.Maximo,
                    petAtkDb = gp
                });

                TuinManager.RequestTripleTuin();

                MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
                {
                    message = string.Format(
                    TextBank.RetornaFraseDoIdioma(TextKey.passouDeNivel), MeuCriatureBase.GetNomeEmLinguas,
                    gXp.Nivel.ToString())
                });

            }
        }

        protected override void OnCriatureDefeated(MsgCriatureDefeated obj)
        {
            base.OnCriatureDefeated(obj);

            if (obj.atacker == gameObject && obj.defeated.layer!=10)
            {
                GerenciadorDeExperiencia gXp = MeuCriatureBase.PetFeat.mNivel;
                gXp.XP += (tDono.ContraTreinador ? 2 : 1)*(int)((float)obj.doDerrotado.PetFeat.meusAtributos.PV.Maximo / 2);

                VerificaLevelUp();

                if (!tDono.ContraTreinador)
                    IamTarget.StaticStart(this, () => { MessageAgregator<MsgReturnRememberedMusic>.Publish(); });
                
                if(!tDono.ContraTreinador)
                    MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
                    {
                        message = string.Format(
                        TextBank.RetornaFraseDoIdioma(TextKey.apresentaFim),
                        MeuCriatureBase.GetNomeEmLinguas,
                        (int)((float)obj.doDerrotado.PetFeat.meusAtributos.PV.Maximo / 2),
                        obj.doDerrotado.PetFeat.meusAtributos.PV.Maximo,
                        obj.doDerrotado.GetNomeEmLinguas
                        ),
                        useBestSize=true
                    });
            }

            if (Controll.Mov.LockTarget!=null  && obj.defeated == Controll.Mov.LockTarget.gameObject)
            {
                SupportSingleton.Instance.InvokeInSeconds(() =>
                {
                    VerificaFocarInimigo();
                },1);
            }

            if (obj.defeated == gameObject)
            {
                CameraApplicator.cam.RemoveMira();
                //Controll.Mov.LockTarget = null;
                //State = LocalState.defeated;
                MessageAgregator<MsgPlayerPetDefeated>.Publish(new MsgPlayerPetDefeated()
                {
                    dono = tDono.gameObject,
                    pet = this
                });
            }
        }

        private void OnChangeToPet(MsgChangeToPet obj)
        {
            if (obj.dono == tDono.transform)
            {

                StaminaManager st = MeuCriatureBase.StManager;

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
                inControll = true;
                State = LocalState.onFree;

                Transform T = CameraApplicator.cam.transform;
                Vector3 pos = T.position;
                Quaternion Q = T.rotation;

                


                if (CameraApplicator.cam.Cdir.TargetIs(transform) && Mov.LockTarget == null)
                {
                    Debug.Log("pensando na camera: "+MeuCriatureBase.varHeightCamera);

                    CameraApplicator.cam.FocusForDirectionalCam(
                        transform,
                        MeuCriatureBase.alturaCamera,
                        MeuCriatureBase.distanciaCamera,
                        MeuCriatureBase.varHeightCamera
                    );
                    
                    T.position = pos;
                    T.rotation = Q;

                    Vector3 V = transform.InverseTransformDirection(
                        Vector3.ProjectOnPlane(
                        T.forward, Vector3.up));


                    CameraApplicator.cam.RetornarParaCameraDirecional(V);
                }
                else if (CameraApplicator.cam.Cdir.TargetIs(transform) && Mov.LockTarget != null)
                {
                    ColocarMiraNoInimigo();
                    //CameraApplicator.cam.StartFightCam(
                    //    transform,
                    //    MeuCriatureBase.alturaCameraLuta,
                    //    MeuCriatureBase.distanciaCameraLuta,                        
                    //    Mov.LockTarget);
                    //MessageAgregator<MsgTargetEnemy>.Publish(new MsgTargetEnemy()
                    //{
                    //    targetEnemy = Mov.LockTarget
                    //});
                }
                else if(obj.lockTarget != null)
                {
                    Debug.Log("colocando target: " + obj.lockTarget + " : " + CameraApplicator.cam.Cdir.TargetIs(transform));

                    CameraApplicator.cam.FocusForDirectionalCam(
                        transform,
                        MeuCriatureBase.alturaCamera,
                        MeuCriatureBase.distanciaCamera,
                        MeuCriatureBase.varHeightCamera
                    );

                    Mov.LockTarget = obj.lockTarget;
                    ColocarMiraNoInimigo();

                }else
                    CameraApplicator.cam.FocusForDirectionalCam(
                        transform,
                        MeuCriatureBase.alturaCamera,
                        MeuCriatureBase.distanciaCamera,
                        MeuCriatureBase.varHeightCamera
                    );

                PetFeatures P = MeuCriatureBase.PetFeat;
            }
        }

        void MoreStatusCommands()
        {
            int selectAttackInput = CurrentCommander.GetIntTriggerDown(CommandConverterString.selectAttack_selectCriature);

            if (selectAttackInput < 0)
            {
                int x = MeuCriatureBase.GerenteDeGolpes.golpeEscolhido;
                x = ContadorCiclico.Contar(1, x, MeuCriatureBase.GerenteDeGolpes.meusGolpes.Count);
                MeuCriatureBase.GerenteDeGolpes.golpeEscolhido = x;

                MessageAgregator<MsgChangeSelectedAttack>.Publish(new MsgChangeSelectedAttack()
                {
                    attackName = MeuCriatureBase.GerenteDeGolpes.meusGolpes[x].Nome
                });
            }
            else if (selectAttackInput > 0)
            {
                MessageAgregator<MsgRequestChangeSelectedPetWithPet>.Publish(new MsgRequestChangeSelectedPetWithPet()
                {
                    pet = gameObject
                });
            }
        }

        Vector3 BasicMoveCommands()
        {
            Vector3 V = CameraApplicator.cam.SmoothCamDirectionalVector(
                        CurrentCommander.GetAxis(CommandConverterString.moveH),
                        CurrentCommander.GetAxis(CommandConverterString.moveV)
                        );
            bool run = CurrentCommander.GetButton(CommandConverterInt.run) && MeuCriatureBase.StManager.VerifyStaminaAction();
            bool startJump = CurrentCommander.GetButtonDown(CommandConverterInt.jump);
            bool pressJump = CurrentCommander.GetButton(CommandConverterInt.jump);
            Controll.Mov.MoveApplicator(V, run, startJump, pressJump);

            MeuCriatureBase.StManager.StaminaRegen(run);

            return V;
        }

        void BaseAction()
        {
            switch (State)
            {
                case LocalState.NonBlockPanelOpened:
                    BasicMoveCommands();
                break;
                case LocalState.following:
                    timeCount += Time.deltaTime;
                    if (timeCount > TargetUpdateTax && tDono != null)
                    {
                        timeCount = 0;
                        Controll.ModificarOndeChegar(tDono.transform.position);
                    }

                    if (Controll.UpdatePosition(2.5f))
                    {

                    }

                    MeuCriatureBase.StManager.StaminaRegen(false);
                break;
                case LocalState.onFree:
                    Vector3 V = BasicMoveCommands();

                    int itemChange = CurrentCommander.GetIntTriggerDown(CommandConverterString.itemChange);



                    if (Mov.LockTarget != null)
                        VerificaMudaAlvo();

                    if (CurrentCommander.GetIntTriggerDown(CommandConverterString.attack) > 0)
                        AplicaGolpe(Mov.LockTarget == null ? null : Mov.LockTarget.gameObject);
                    else if (CurrentCommander.GetIntTriggerDown(CommandConverterString.focusInTheEnemy) > 0)
                    {
                        VerificaFocarInimigo();
                    }
                    else if (itemChange != 0)
                    {
                        MessageAgregator<MsgRequestChangeSelectedItemWithPet>.Publish(new MsgRequestChangeSelectedItemWithPet()
                        {
                            pet = gameObject,
                            change = itemChange
                        });
                    }
                    else if (Controll.Mov.IsGrounded)
                    {
                        bool heroToPet = CurrentCommander.GetButtonDown(CommandConverterInt.heroToCriature, true);
                        if ( heroToPet&& !tDono.ContraTreinador)
                        {
                            if (Mov.LockTarget)
                            {
                                Mov.LockTarget = null;
                                CameraApplicator.cam.RemoveMira();
                            }
                            inControll = false;
                            State = LocalState.following;
                            Controll.Mov.MoveApplicator(Vector3.zero);
                            MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero() { myHero = tDono.gameObject });
                        }
                        else if (heroToPet && tDono.ContraTreinador)
                        {
                            MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
                            {
                                message = TextBank.RetornaListaDeTextoDoIdioma(TextKey.mensLuta)[6]
                            });
                        }
                        else if (CurrentCommander.GetButtonDown(CommandConverterInt.dodge))
                        {
                            if (MeuCriatureBase.StManager.VerifyStaminaAction() && Roll.Start(V, gameObject))
                            {
                                gameObject.layer = 2;
                                Controll.Mov.ApplicableGravity = false;
                                GameObject G = Resources.Load<GameObject>("particles/" + GeneralParticles.rollParticles.ToString());
                                Destroy(Instantiate(G, transform.position, Quaternion.identity, transform), 3);
                                Controll.Mov.UseRollSpeed = true;
                                MeuCriatureBase.StManager.ConsumeStamina(20);
                                State = LocalState.inDodge;
                            }
                        }
                        else if (CurrentCommander.GetButtonDown(CommandConverterInt.criatureChange))
                        {
                            MessageAgregator<MsgRequestReplacePet>.Publish(new MsgRequestReplacePet()
                            {
                                dono = tDono.gameObject,
                                lockTarget = Mov.LockTarget
                            });
                        }
                        else if (CurrentCommander.GetButtonDown(CommandConverterInt.itemUse))
                        {
                            MessageAgregator<MsgRequestUseItem>.Publish(new MsgRequestUseItem()
                            {
                                dono = tDono.gameObject
                            });
                        }
                        else if (CurrentCommander.GetButtonDown(CommandConverterInt.updateMenu))
                        {
                            MessageAgregator<MsgCriatureUpdateButtonPress>.Publish(new MsgCriatureUpdateButtonPress()
                            {
                                dono = tDono.gameObject
                            });
                        }

                    }

                    MoreStatusCommands();
                break;
                case LocalState.atk:
                    if (AtkApply.UpdateAttack())
                    {
                        State = inControll? LocalState.onFree:LocalState.following;
                    }

                    MoreStatusCommands();
                break;
                case LocalState.inDamage:
                    if (DamageState.Update())
                    {
                        EndDamageState();
                        State = inControll ? LocalState.onFree : LocalState.following;
                    }
                break;
                case LocalState.inDodge:
                    InRollState();
                break;
                case LocalState.returnOfRoll:
                    if (Roll.ReturnTime())
                        EndRollState();
                break;
                case LocalState.nonReturnableDamage:
                    if (DamageState.Update())
                    {
                        EndDamageState();
                        State = LocalState.stopped;
                    }
                break;
            }
        }

        private void VerificaMudaAlvo()
        {
            int intMouseX = CurrentCommander.GetIntTriggerDown(CommandConverterString.changeLockTargetX);
            int intMouseY = CurrentCommander.GetIntTriggerDown(CommandConverterString.changeLockTargetY);

            Transform candidato = Mov.LockTarget;

            if (intMouseX != 0)
            {
                Vector3 right = Camera.main.transform.right;
                right *= intMouseX;
                candidato = FindBestTarget.InDirectionOfBase(transform, right, Mov.LockTarget, new string[1] { "Criature" });
            }

            if (intMouseY != 0)
            {
                Vector3 forward = Camera.main.transform.forward;
                forward *= intMouseY;
                candidato = FindBestTarget.InDirectionOfBase(transform, forward, Mov.LockTarget, new string[1] { "Criature" });
            }

            if (candidato != Mov.LockTarget)
            {
                Mov.LockTarget = candidato;
                ColocarMiraNoInimigo();
            }

        }

        void EndRollState()
        {
            gameObject.layer = 0;
            Controll.Mov.ApplicableGravity = true;
            Controll.Mov.UseRollSpeed = false;
            Controll.Mov.MoveApplicator(Vector3.zero);
            CurrentCommander.DirectionalVector();
            State = LocalState.onFree;
        }

        void InRollState()
        {

            if (Roll.Update())
            {
                if (Roll.RequestAttack)
                {
                    if (MeuCriatureBase.StManager.VerifyStaminaAction())
                    {
                        //estado = MotionMoveState.inExternalAction;
                        //FayvitMoveEventAgregator.Publish(new FayvitMoveEvent(FayvitMoveEventKey.posRollAttack, gameObject));
                        MeuCriatureBase.StManager.ConsumeStamina(40);
                    }
                    else
                        State = LocalState.returnOfRoll;
                }
                else
                    State = LocalState.returnOfRoll;

                Controll.Mov.MoveApplicator(Vector3.zero);
            }
            else
            {
                //if (CurrentCommander.GetButtonDown(5))
                //    Roll.RequestAttack = true;

                Controll.Mov.MoveApplicator(Roll.DirOfRoll, true);
            }
        }

        void VerificaFocarInimigo()
        {
            if (Mov.LockTarget)
            {
                Mov.LockTarget = null;
                Vector3 V = transform.InverseTransformDirection(
                    Vector3.ProjectOnPlane(
                    CameraApplicator.cam.transform.forward,Vector3.up));
                CameraApplicator.cam.RetornarParaCameraDirecional(V);
                MessageAgregator<MsgUnTargetEnemy>.Publish();
            }
            else
            {
                string[] tags = new string[1] { "Criature" };
                List<GameObject> osPerto = FindBestTarget.ProximosDeMim(gameObject, tags);

                RemoveDefeated(osPerto);
                Mov.LockTarget = FindBestTarget.Procure(gameObject, osPerto,20,true);
                //CameraAplicator.cam.SetarInimigosProximosParaFoco(osPerto);

                if (Mov.LockTarget)
                {
                    ColocarMiraNoInimigo();
                }
            }
        }

        private void ColocarMiraNoInimigo()
        {
            //CameraApplicator.cam.StartFightCam(transform, Mov.LockTarget);
            CameraApplicator.cam.StartFightCam(
                transform,
                MeuCriatureBase.alturaCameraLuta,
                MeuCriatureBase.distanciaCameraLuta,
                Mov.LockTarget);
            MessageAgregator<MsgTargetEnemy>.Publish(new MsgTargetEnemy()
            {
                targetEnemy = Mov.LockTarget
            });
        }

        private void RemoveDefeated(List<GameObject> osPerto)
        {
            int count = osPerto.Count;
            for (int i = count; i > 0; i--)
            {
                if (osPerto[i - 1].GetComponent<PetManager>().State == LocalState.defeated)
                {
                    osPerto.RemoveAt(i - 1);
                }
            }
        }

        void CamAction()
        {
            if (State != LocalState.following)
            {
                Vector2 V = new Vector2(
                    CurrentCommander.GetAxis(CommandConverterString.camX),
                    CurrentCommander.GetAxis(CommandConverterString.camY)
                    );

                bool focar = CurrentCommander.GetButtonDown(CommandConverterInt.camFocus);
                CameraApplicator.cam.ValoresDeCamera(V.x, V.y, focar, Controll.Mov.Controller.velocity.sqrMagnitude > .1f);
            }
        }

        protected override void Update()
        {
            BaseAction();
            CamAction();

            base.Update();
        }

        public void ControlableVsTrainer(Transform lockTarget)
        {
            State = LocalState.onFree;
            Controll.Mov.LockTarget = lockTarget;

            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                ColocarMiraNoInimigo();
            });
        }

    }

    public struct MsgCriatureUpdateButtonPress : IMessageBase {
        public GameObject dono;
    }
    public struct MsgChangeLevel : IMessageBase {
        public GameObject gameObject;
        public int newLevel;
        public int pvCorrente;
        public int pvMax;
        public int peCorrente;
        public int peMaximo;
        public PetAttackDb petAtkDb;
    }
    public struct MsgChangeToHero : IMessageBase {
        public GameObject myHero;
        public bool blockReturnCam;
    }

    public struct MsgRequestChangeSelectedPetWithPet : IMessageBase {
        public GameObject pet;
    }
    public struct MsgTargetEnemy : IMessageBase {
        public Transform targetEnemy;
    }

    public struct MsgUnTargetEnemy : IMessageBase{ }

    public struct MsgRequestReplacePet : IMessageBase {
        public GameObject dono;
        public Transform lockTarget;
        public bool replaceIndex;
        public int newIndex;
    }
    public struct MsgRequestChangeSelectedItemWithPet : IMessageBase
    {
        public int change;
        public GameObject pet;
    }
    public struct MsgRequestUseItem : IMessageBase
    {
        public GameObject dono;
    }
    public struct MsgPlayerPetDefeated : IMessageBase {
        public GameObject dono;
        public PetManagerCharacter pet;
    }
    public struct MsgAnEnemyTargetMe : IMessageBase { }
    public struct MsgSendEnemyTargets :IMessageBase{
        public PetManager target;
        public PetManager sender;
    }

    public struct MsgRequestNonReturnableDamage : IMessageBase
    {
        public GameObject gameObject;
        public PetAttackBase petAttack;
    }
}