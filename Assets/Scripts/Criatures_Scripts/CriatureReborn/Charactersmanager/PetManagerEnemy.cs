using UnityEngine;
using System.Collections;
using FayvitMove;
using FayvitSupportSingleton;
using System.Collections.Generic;
using FayvitMessageAgregator;

namespace Criatures2021
{
    public class PetManagerEnemy : PetManager
    {
        [SerializeField]private EnemyIaBase enemyIa = new EnemyIaBase();
        [SerializeField]
        private Dictionary<AttackResponse, float> atkResponseTax = new Dictionary<AttackResponse, float>()
        {
            { AttackResponse.aggresive,.95f},
            { AttackResponse.escapeTax,.05f}
        };

        private LocalState rememberedState;
        private bool visto;

        protected override void Start()
        {
            Controll = new ControlledMoveForCharacter(transform);
            Controll.SetCustomMove(MeuCriatureBase.MovFeat);

            State = LocalState.onFree;
            enemyIa.Start(transform,MeuCriatureBase,Controll);
            base.Start();

            MessageAgregator<MsgEnemyRequestAttack>.AddListener(OnRequestAttack);
            MessageAgregator<MsgPlayerPetDefeated>.AddListener(OnPlayerDefeated);
            MessageAgregator<MsgStartUseItem>.AddListener(OnPlayerPetStartUseItem);
            MessageAgregator<MsgStartReplacePet>.AddListener(OnStartReplacePet);
            MessageAgregator<MsgTargetEnemy>.AddListener(OnTargetEnemy);


        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            MessageAgregator<MsgEnemyRequestAttack>.RemoveListener(OnRequestAttack);
            MessageAgregator<MsgPlayerPetDefeated>.RemoveListener(OnPlayerDefeated);
            MessageAgregator<MsgStartUseItem>.RemoveListener(OnPlayerPetStartUseItem);
            MessageAgregator<MsgStartReplacePet>.RemoveListener(OnStartReplacePet);
            MessageAgregator<MsgTargetEnemy>.RemoveListener(OnTargetEnemy);
        }

        private void OnTargetEnemy(MsgTargetEnemy obj)
        {
            if (obj.targetEnemy==transform && !visto)
            {
                visto = true;
                MessageAgregator<MsgNewSeen>.Publish(new MsgNewSeen()
                {
                    name = MeuCriatureBase.NomeID
                });
            }
        }

        

        private void OnStartReplacePet(MsgStartReplacePet obj)
        {
            enemyIa.WaitPetChange(obj.dono);
        }

        private void OnPlayerPetStartUseItem(MsgStartUseItem obj)
        {
            enemyIa.WaitPetChange(obj.usuario);
        }

        private void OnPlayerDefeated(MsgPlayerPetDefeated obj)
        {
            enemyIa.WaitPetChange(obj.pet);
        }

        private void OnRequestAttack(MsgEnemyRequestAttack obj)
        {
            if (obj.gameObject == gameObject)
            {
                EfetiveApplyAttack(obj.atk,enemyIa.HeroPet.gameObject);
            }
        }

        protected override void OnEnterInDamageState(MsgEnterInDamageState obj)
        {
            base.OnEnterInDamageState(obj);

            if (obj.oAtacado == gameObject)
            {
                enemyIa.OnStartDamageState(atkResponseTax,obj.atacante);
            }
        }

        protected override void OnRequestEnemiesTargets(MsgAnEnemyTargetMe obj)
        {
            MessageAgregator<MsgSendEnemyTargets>.Publish(new MsgSendEnemyTargets()
            {
                target = enemyIa.HeroPet,
                sender = this
            });
        }

        protected override void OnCriatureDefeated(MsgCriatureDefeated obj)
        {
            base.OnCriatureDefeated(obj);

            Debug.Log("é um trainer: "+obj.defeated.GetComponent<PetManagerTrainer>());
            Debug.Log("enemyIa hero pet: " + enemyIa.HeroPet);
            if (obj.defeated == gameObject)
            {
                SupportSingleton.Instance.InvokeInSeconds(() =>
                {
                    GameObject G = Resources.Load<GameObject>("particles/" + ImpactParticles.defeatedParticles);
                    Destroy(
                    Instantiate(G, transform.position, Quaternion.identity), 5);

                    Destroy(gameObject);
                }, 4);
            }
            else if (obj.defeated.GetComponent<PetManagerTrainer>()
                && obj.defeated.GetComponent<PetManagerTrainer>().enemyIa.HeroPet==enemyIa.HeroPet)
            {
                Debug.Log("chamou o eait channge to pet");
                enemyIa.WaitPetChange(enemyIa.HeroPet as PetManagerCharacter);
            }
            //else if (obj.atacker == gameObject)
            //{ 
            
            //}
        }

        protected override void Update()
        {
            switch (State)
            {
                case LocalState.onFree:
                    enemyIa.Update();
                break;
                case LocalState.atk:
                    if (AtkApply.UpdateAttack())
                    {
                        State = LocalState.onFree;
                    }
                break;
                case LocalState.inDamage:
                    if (DamageState.Update())
                    {
                        EndDamageState();
                        enemyIa.OnEndDamageState();
                        State = LocalState.onFree;
                    }
                break;
            }

            base.Update();
        }

        public void StopWithRememberedState()
        {
            rememberedState = State;
            State = LocalState.stopped;
        }

        public void ReturnRememberedState()
        {
            State = rememberedState;
        }

        public void StartAgressiveIa(GameObject atacante)
        {
            enemyIa.Start(transform, MeuCriatureBase, Controll);
            enemyIa.ChangeOnAttackResponse(AttackResponse.aggresive, atacante);
            State = LocalState.onFree;
        }

        public void SetStartVigilance(LocalState s = LocalState.onFree)
        {
            State = s;
        }
    }

    public struct MsgNewSeen : IMessageBase
    {
        public PetName name;
    }
}