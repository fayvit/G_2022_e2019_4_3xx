using UnityEngine;
using Criatures2021;
using FayvitMessageAgregator;
using FayvitBasicTools;

namespace FieldEnemy
{
    public class PetVigilanceListener : MonoBehaviour
    {
        [SerializeField] private float timeInMoveToDamage = .25f;
        private PetAttackBase petAttack = new PetAttackBase(new PetAttackFeatures()
        {
            potenciaCorrente = 0,
            potenciaMaxima = 0,
            potenciaMinima = 0,
            TempoNoDano = 0.75f,
            distanciaDeRepulsao = 95f,
            velocidadeDeRepulsao = 33,
            tempoDeMoveMin = 0.45f,//74
            tempoDeMoveMax = 0.85f,
            tempoDeDestroy = 1.1f,
            velocidadeDeGolpe = 28,
        });

        private GameObject vigilanceTrigger;
        private PetManager pet;
        private LocalState state = LocalState.inVigilance;
        private Transform moveOnDamage;
        private Transform player;
        private Vector3 startPosition;
        private float tempoDecorrido = 0;

        

        public enum LocalState
        { 
            inVigilance,
            suspenseVigilance,
            vigilanceAttack,
            returnToPosition
        }

        public void SetVilanceListener(PetManager pet, GameObject vigilanceTrigger, Transform moveOnDamage, float timeInMoveToDamage, PetAttackBase petAttack)
        {
            this.pet = pet;
            this.vigilanceTrigger = vigilanceTrigger;
            this.moveOnDamage = moveOnDamage;
            this.petAttack = petAttack;
            this.timeInMoveToDamage = timeInMoveToDamage;
        }

        // Use this for initialization
        void Start()
        {
            MessageAgregator<MsgPlayerInVigilanceTrigger>.AddListener(OnPlayerInVigilanceTrigger);
            MessageAgregator<MsgEnterInAggressiveResponse>.AddListener(OnEnterInAggressiveResponse);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgPlayerInVigilanceTrigger>.RemoveListener(OnPlayerInVigilanceTrigger);
            MessageAgregator<MsgEnterInAggressiveResponse>.RemoveListener(OnEnterInAggressiveResponse);
        }

        private void OnEnterInAggressiveResponse(MsgEnterInAggressiveResponse obj)
        {
            if (obj.enemyPet == pet.MeuCriatureBase)
            {
                state = LocalState.suspenseVigilance;
            }
        }

        private void OnPlayerInVigilanceTrigger(MsgPlayerInVigilanceTrigger obj)
        {
            if (obj.trigger == vigilanceTrigger && state==LocalState.inVigilance)
            {
                pet.Mov.Controller.enabled = false;
                tempoDecorrido = 0;
                player = obj.player;
                startPosition = pet.transform.position;
                state = LocalState.vigilanceAttack;

                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                {
                    sfxId = petAttack.SomDoGolpe
                });
            }
        }

        // Update is called once per frame
        void Update()
        {
            switch (state)
            {
                case LocalState.vigilanceAttack:
                    tempoDecorrido += Time.deltaTime;
                    pet.transform.position = Vector3.Lerp(startPosition, player.position, tempoDecorrido / timeInMoveToDamage);
                    if (tempoDecorrido >= timeInMoveToDamage)
                    {
                        petAttack.DirDeREpulsao = -(player.position - moveOnDamage.position).normalized;
                        PetManagerCharacter P = player.GetComponent<PetManagerCharacter>();
                        if (P)
                        {
                            MessageAgregator<MsgEnterInDamageState>.Publish(new MsgEnterInDamageState()
                            {
                                oAtacado = P.gameObject,
                                golpe = petAttack,
                                atacante = gameObject
                            });
                        }else
                        //Debug.Log(petAttack + " : " + player + " : " + moveOnDamage);

                        
                        MessageAgregator<MsgRequestHumanDamage>.Publish(new MsgRequestHumanDamage()
                        {
                            esseGolpe = petAttack,
                            gameObject = player.gameObject,
                            autoReturnToMove=true
                        });

                        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                        {
                            sfxId = petAttack.SomDoImpacto
                        });

                        InsertImpactView.Insert(ImpactParticles.impactoComum, player.position, Quaternion.identity);

                        state = LocalState.returnToPosition;
                        tempoDecorrido = 0;
                    }
                break;
                case LocalState.returnToPosition:
                    tempoDecorrido += Time.deltaTime;
                    pet.transform.position = Vector3.Lerp(transform.position, startPosition, tempoDecorrido / timeInMoveToDamage);
                    if (tempoDecorrido >= timeInMoveToDamage)
                    {
                        pet.Mov.Controller.enabled = true;
                        state = LocalState.inVigilance;
                        tempoDecorrido = 0;
                    }
                break;
            }
        }
    }

    public struct MsgPlayerInVigilanceTrigger : IMessageBase
    {
        public GameObject trigger;
        public Transform player;
    }
}