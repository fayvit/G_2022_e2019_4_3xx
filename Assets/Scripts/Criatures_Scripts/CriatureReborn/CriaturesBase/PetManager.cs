using FayvitBasicTools;
using FayvitMessageAgregator;
using FayvitMove;
using FayvitSounds;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Criatures2021
{
    public class PetManager : MonoBehaviour
    {

        [SerializeField] private LocalState state = LocalState.following;
        [SerializeField] private PetBase meuCriatureBase;
        [SerializeField] private ControlledMoveForCharacter controll;
        [SerializeField] private RollManager roll;
        [SerializeField] private DamageState damageState;
        [SerializeField] private float targetUpdateTax = 1;

        protected float timeCount = 0;

        protected float TargetUpdateTax => targetUpdateTax;
        protected RollManager Roll => roll;

        public LocalState State { get=>state; protected set=>state=value; }

        public enum LocalState
        {
            following,
            onFree,
            stopped,
            atk,
            inDamage, 
            defeated,
            inDodge,
            returnOfRoll,
            NonBlockPanelOpened,
            externalPanelOpened,
            nonReturnableDamage,
            externalJumpRequest
        }

        public PetBase MeuCriatureBase
        {
            get => meuCriatureBase;
            set
            {
                //Debug.Log("Setado para: " + name);
                meuCriatureBase = value;
                Controll = new ControlledMoveForCharacter(transform);
                Controll.SetCustomMove(meuCriatureBase.MovFeat);
            }
        }

        public BasicMove Mov
        {
            get
            {
                if (Controll == null)
                    SetaMov();
                return Controll.Mov;
            }
        }

        protected ControlledMoveForCharacter Controll { get => controll; set => controll = value; }
        protected AttackApplyManager AtkApply { get; set; }
        protected DamageState DamageState { get => damageState; set => damageState = value; }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            AtkApply = new AttackApplyManager(gameObject);
            DamageState = new DamageState(transform);

            //Debug.Log("Criado para: " + name);
            //Controll = new ControlledMoveForCharacter(transform);
            //Controll.SetCustomMove(meuCriatureBase.MovFeat);

            if (roll == null)
                roll = new RollManager();

            MessageAgregator<MsgEnterInDamageState>.AddListener(OnEnterInDamageState);
            MessageAgregator<AnimateStartJumpMessage>.AddListener(OnStartJump);
            MessageAgregator<MsgCriatureDefeated>.AddListener(OnCriatureDefeated);
            MessageAgregator<MsgAnEnemyTargetMe>.AddListener(OnRequestEnemiesTargets);
            //MessageAgregator<MsgSendNewStatus>.AddListener(OnReceiveNewStatus);
            //MessageAgregator<MsgSendUpdateStatus>.AddListener(OnReceiveUpdateStatus);
            //MessageAgregator<MsgRemoveStatus>.AddListener(OnRemoveStatus);
        }

        protected virtual void OnDestroy()
        {
            MessageAgregator<MsgEnterInDamageState>.RemoveListener(OnEnterInDamageState);
            MessageAgregator<AnimateStartJumpMessage>.RemoveListener(OnStartJump);
            MessageAgregator<MsgCriatureDefeated>.RemoveListener(OnCriatureDefeated);
            MessageAgregator<MsgAnEnemyTargetMe>.RemoveListener(OnRequestEnemiesTargets);
            //MessageAgregator<MsgSendNewStatus>.RemoveListener(OnReceiveNewStatus);
            //MessageAgregator<MsgSendUpdateStatus>.RemoveListener(OnReceiveUpdateStatus);
            //MessageAgregator<MsgRemoveStatus>.RemoveListener(OnRemoveStatus);
        }

        //private void OnRemoveStatus(MsgRemoveStatus obj)
        //{
        //    if (obj.pet == this)
        //    {
        //        switch (obj.status.Dados.Tipo)
        //        {
        //            case StatusType.amedrontado:
        //                controll.Mov.ModSpeed = 1;
        //            break;
        //        }
        //    }
        //}

        //private void OnReceiveUpdateStatus(MsgSendUpdateStatus obj)
        //{
        //    if (obj.receiver == this)
        //    {
        //        int i = StatusTemporarioBase.ContemStatus(obj.S.Dados.Tipo, MeuCriatureBase);
        //        if (i > -1)
        //        {
        //            switch (obj.S.Dados.Tipo)
        //            {
        //                case StatusType.amedrontado:
        //                    controll.Mov.ModSpeed = 1 / (meuCriatureBase.StatusTemporarios[i].Quantificador+1);
        //                break;
        //            }
        //        }
        //    }
        //}

        //private void OnReceiveNewStatus(MsgSendNewStatus obj)
        //{
        //    if (obj.receiver == this)
        //    {
        //        switch (obj.S.Dados.Tipo)
        //        {
        //            case StatusType.amedrontado:
        //                controll.Mov.ModSpeed = 1 / (obj.S.Dados.Quantificador+1);
        //            break;
        //        }
                
        //    }
        //}

        protected virtual void OnRequestEnemiesTargets(MsgAnEnemyTargetMe obj)
        {
            MessageAgregator<MsgSendEnemyTargets>.Publish(new MsgSendEnemyTargets()
            {
                target = null,
                sender  =this
            });
        }

        protected virtual void OnCriatureDefeated(MsgCriatureDefeated obj){
            if (obj.defeated == gameObject)
                state = LocalState.defeated;
        }

        private void OnStartJump(AnimateStartJumpMessage obj)
        {
            if (obj.gameObject == gameObject)
            {
                MessageAgregator<MsgRequest3dSound>.Publish(new MsgRequest3dSound()
                {
                    sender = transform,
                    sfxId = SoundEffectID.Evasion1
                }) ;
            }
        }

        protected virtual void OnEnterInDamageState(MsgEnterInDamageState obj)
        {
            if (obj.oAtacado == gameObject)
            {
                DamageState.StartDamageState(obj.golpe);
                state = LocalState.inDamage;

                ConsumableAttribute PV = MeuCriatureBase.PetFeat.meusAtributos.PV;

                MessageAgregator<MsgChangeHP>.Publish(new MsgChangeHP()
                {
                    currentHp = PV.Corrente,
                    maxHp = PV.Maximo,
                    gameObject = gameObject
                });
            }
        }

        protected virtual void ReiniciarModulos()
        {
            EndDamageState();
            MessageAgregator<MsgFreedonAfterAttack>.Publish(new MsgFreedonAfterAttack() { gameObject = gameObject });
        }

        void SetaMov()
        {
            Controll = new ControlledMoveForCharacter(transform);
            Controll.SetCustomMove(meuCriatureBase.MovFeat);
          
        }

        protected void EfetiveApplyAttack(PetAttackBase gg,GameObject focado)
        {
            PetAttackDb petDb = MeuCriatureBase.GerenteDeGolpes.ProcuraGolpeNaLista(MeuCriatureBase.NomeID, gg.Nome);
            AtkApply.StartAttack(gg, petDb.TempoDeInstancia,focado);
            state = LocalState.atk;
            MessageAgregator<MsgChangeMP>.Publish(new MsgChangeMP()
            {
                gameObject = gameObject,
                currentMp = meuCriatureBase.PetFeat.meusAtributos.PE.Corrente,
                maxMp = meuCriatureBase.PetFeat.meusAtributos.PE.Maximo
            });
        }

        protected void AplicaGolpe(GameObject focado)
        {
            
            PetAttackBase gg = meuCriatureBase.GerenteDeGolpes.meusGolpes[meuCriatureBase.GerenteDeGolpes.golpeEscolhido];

            Debug.Log("no chão: " + Controll.Mov.IsGrounded);

            if (Controll.Mov.IsGrounded || gg.PodeNoAr)
            {
                if (AttackApplyManager.CanStartAttack(MeuCriatureBase,OnEmptyStaminaInAttack,OnNotHavingPE))
                {
                    EfetiveApplyAttack(gg,focado);
                }
            }
        }

        protected virtual void OnNotHavingPE() { }

        protected virtual void OnEmptyStaminaInAttack() { }

        // Update is called once per frame
        protected virtual void Update() {
            switch (state)
            {
                case LocalState.defeated:
                    Controll.Mov.ApplicableGravity = true;
                    Controll.Mov.MoveApplicator(Vector3.zero);
                break;
                case LocalState.stopped:
                    Controll.Mov.MoveApplicator(Vector3.zero);
                break;
                case LocalState.inDodge:
                    InRollState();
                break;
                case LocalState.returnOfRoll:
                    if (Roll.ReturnTime())
                        EndRollState();
                break;
            }
        }

        protected void StartDodge()
        {
            gameObject.layer = 2;
            Controll.Mov.ApplicableGravity = false;
            GameObject G = Resources.Load<GameObject>("particles/" + GeneralParticles.rollParticles.ToString());
            Destroy(Instantiate(G, transform.position, Quaternion.identity, transform), 3);
            Controll.Mov.UseRollSpeed = true;
            MeuCriatureBase.StManager.ConsumeStamina(20);
            State = LocalState.inDodge;
        }

        protected virtual void EndRollState()
        {
            gameObject.layer = 0;
            Controll.Mov.ApplicableGravity = true;
            Controll.Mov.UseRollSpeed = false;
            Controll.Mov.MoveApplicator(Vector3.zero);
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

        // eram comandos para o android mas já há comandos melhores para isso
        //public void ComandoDeAtacar()
        //{
        //    if (state == LocalState.onFree || state == LocalState.inFight)
        //        AplicaGolpe();
        //}

        //public void IniciaPulo()
        //{
        //    if (!meuCriatureBase.Mov.caracPulo.estouPulando && (state == LocalState.onFree || state == LocalState.inFight))
        //        mov._Pulo.IniciaAplicaPulo();
        //}

        public void PararCriatureNoLocal()
        {
            ReiniciarModulos();
            state = LocalState.stopped;
        }

        protected virtual void EndDamageState()
        {
            MessageAgregator<MsgEndDamageState>.Publish(new MsgEndDamageState() { gameObject = gameObject });
        }
    }

    public struct MsgEndDamageState : IMessageBase {
        public GameObject gameObject;
    }
}