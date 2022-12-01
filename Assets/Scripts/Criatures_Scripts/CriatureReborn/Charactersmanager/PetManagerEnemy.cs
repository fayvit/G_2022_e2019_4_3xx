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
            MessageAgregator<MsgRequestCpuRoll>.AddListener(OnRequestCpuRoll);
            MessageAgregator<MsgRequestAtkResponse>.AddListener(OnRequestJump);
            MessageAgregator<MsgClearPetTarget>.AddListener(OnRequestClearTarget);


            SupportSingleton.Instance.InvokeInSeconds(VerifiqueChao, Random.Range(1f,5f));

        }

        void VerifiqueChao()
        {
            if (this != null)
            {
                if (transform.position.y < -100)
                {
                    BasicCriatures.PoolPets_ddepoisDoBugDaUnity.instance.DisablePetGO(gameObject, MeuCriatureBase.NomeID);
                    //OnDestroy();
                }
                    //Destroy(gameObject);


                SupportSingleton.Instance.InvokeInSeconds(VerifiqueChao, Random.Range(1f, 5f));
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            MessageAgregator<MsgClearNegativeStatus>.Publish(new MsgClearNegativeStatus()
            {
                target = MeuCriatureBase
            });
            MessageAgregator<MsgEnemyRequestAttack>.RemoveListener(OnRequestAttack);
            MessageAgregator<MsgPlayerPetDefeated>.RemoveListener(OnPlayerDefeated);
            MessageAgregator<MsgStartUseItem>.RemoveListener(OnPlayerPetStartUseItem);
            MessageAgregator<MsgStartReplacePet>.RemoveListener(OnStartReplacePet);
            MessageAgregator<MsgTargetEnemy>.RemoveListener(OnTargetEnemy);
            MessageAgregator<MsgRequestCpuRoll>.RemoveListener(OnRequestCpuRoll);
            MessageAgregator<MsgRequestAtkResponse>.RemoveListener(OnRequestJump);
            MessageAgregator<MsgClearPetTarget>.RemoveListener(OnRequestClearTarget);

            enemyIa.OnDestroy();
        }

        private void OnRequestClearTarget(MsgClearPetTarget obj)
        {
            enemyIa.RequestClearTarget(obj.owner);
        }

        private void OnRequestJump(MsgRequestAtkResponse obj)
        {
            if (obj.sender == gameObject && State == LocalState.onFree)
            {
                obj.acao?.Invoke();
            }
        }

        private void OnRequestCpuRoll(MsgRequestCpuRoll obj)
        {
            if (obj.sender == gameObject && State==LocalState.onFree)
            {
                Roll.Start(obj.dir, gameObject);
                StartDodge();
            }
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
            AddStoppedUntilChageToPet(obj.dono);
        }

        private void OnPlayerPetStartUseItem(MsgStartUseItem obj)
        {
            enemyIa.WaitPetChange(obj.usuario);
            AddStoppedUntilChageToPet(obj.usuario);
        }

        private void AddStoppedUntilChageToPet(GameObject dono)
        {
            if (enemyIa != null && enemyIa.PetOwner && dono == enemyIa.PetOwner)
            {
                PararCriatureNoLocal();
                MessageAgregator<MsgChangeToPet>.AddListener(OnChangeToPet);
            }
        }

        private void OnChangeToPet(MsgChangeToPet obj)
        {
            Debug.Log("Change to pet do enemy");
            Debug.Log(enemyIa);
            Debug.Log(enemyIa.PetOwner);
            Debug.Log(enemyIa.PetOwner+" : "+obj.dono);



            if (enemyIa != null && enemyIa.PetOwner != null && obj.dono.gameObject == enemyIa.PetOwner)
            {
                State = LocalState.onFree;
                SupportSingleton.Instance.InvokeOnEndFrame(() =>
                {
                    MessageAgregator<MsgChangeToPet>.RemoveListener(OnChangeToPet);
                });
            }
            else
            {
                SupportSingleton.Instance.InvokeOnEndFrame(() =>
                {
                    MessageAgregator<MsgChangeToPet>.RemoveListener(OnChangeToPet);
                });
            }

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

            //Debug.Log("é um trainer: "+obj.defeated.GetComponent<PetManagerTrainer>());
            //Debug.Log("enemyIa hero pet: " + enemyIa.HeroPet);
            if (obj.defeated == gameObject)
            {
                SupportSingleton.Instance.InvokeInSeconds(() =>
                {
                    GameObject G = Resources.Load<GameObject>("particles/" + ImpactParticles.defeatedParticles);
                    Destroy(
                    Instantiate(G, transform.position, Quaternion.identity), 5);

                    BasicCriatures.PoolPets_ddepoisDoBugDaUnity.instance.DisablePetGO(gameObject, MeuCriatureBase.NomeID);

                    //Destroy(gameObject);
                }, 4);
            }
            //else if (obj.defeated.GetComponent<PetManagerTrainer>()
            //    && obj.defeated.GetComponent<PetManagerTrainer>().enemyIa.HeroPet == enemyIa.HeroPet)
            //{
            //    //Debug.Log("chamou o eait channge to pet");
            //    enemyIa.WaitPetChange(enemyIa.HeroPet as PetManagerCharacter);
            //}
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

        public void ChangeIaType(EnemyIaBase ia)
        {
            enemyIa = ia;
            enemyIa.Start(transform, MeuCriatureBase, Controll);
        }
    }

    public struct MsgNewSeen : IMessageBase
    {
        public PetName name;
    }

    public struct MsgClearPetTarget : IMessageBase
    {
        public GameObject owner;
    }
}