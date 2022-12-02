using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitMove;
using FayvitMessageAgregator;
using FayvitSupportSingleton;

namespace TalkSpace
{
    public class MoveTalkableNPC : MonoBehaviour
    {
        [SerializeField] private float minStoppedTime = 1;
        [SerializeField] private float maxStopeedTime = 10;
        //[SerializeField] private bool iniciarControlle;
        [SerializeField] private ControlledMoveForCharacter controle;
        [SerializeField] private MovePointsForNPC currentPoints;
        private float targetTime = 100;
        private float tempoDecorrido = 0;
        private Transform target;

        private LocalState state = LocalState.stopped;
        private LocalState rememberedState = LocalState.stopped;
        private enum LocalState
        {
            stopped,
            walk,
            talk,
            endingTalk
        }
        // Start is called before the first frame update
        void Start()
        {
            //if (iniciarControlle)
                IniciarControlle(transform.parent);

            MessageAgregator<MsgDesyncStandardAnimation>.AddListener(OnDesync);
            MessageAgregator<MsgStartTextDisplay>.AddListener(OnStartTalk);
            MessageAgregator<MsgFinishTextDisplay>.AddListener(OnFinishTalk);

        }

        private void OnDestroy()
        {
            MessageAgregator<MsgDesyncStandardAnimation>.RemoveListener(OnDesync);
            MessageAgregator<MsgStartTextDisplay>.RemoveListener(OnStartTalk);
            MessageAgregator<MsgFinishTextDisplay>.RemoveListener(OnFinishTalk);
        }

        private void OnFinishTalk(MsgFinishTextDisplay obj)
        {
            if (obj.sender == gameObject)
            {
                if (rememberedState == LocalState.walk)
                {
                    state = LocalState.endingTalk;
                    targetTime = Random.Range(1, 1 + .1f * maxStopeedTime);
                    SupportSingleton.Instance.InvokeInSeconds(() =>
                    {
                        state = rememberedState;
                    }, targetTime);
                }
                else
                    state = rememberedState;
                
            }
        }

        private void OnStartTalk(MsgStartTextDisplay obj)
        {
            if (obj.sender == gameObject)
            {
                rememberedState = state;
                state = LocalState.talk;
            }
        }

        private void OnDesync(MsgDesyncStandardAnimation obj)
        {
            if (obj.gameObject.transform == transform.parent)
            {
                IniciarControlle(obj.gameObject.transform);
            }
        }

        void IniciarControlle(Transform T)
        {
            controle.StartFields(T);
            //SupportSingleton.Instance.InvokeOnEndFrame(() =>
            //{

            IniciarCaminhadaComNovoPonto();
            //});
        }

        void IniciarCaminhadaComNovoPonto()
        {
            target = currentPoints.UmPontoSorteado;
            currentPoints = target.GetComponent<MovePointsForNPC>();
            controle.ModificarOndeChegar(target.position);
            state = LocalState.walk;
        }

        // Update is called once per frame
        void Update()
        {
            switch (state)
            {
                case LocalState.walk:
                    if (controle.UpdatePosition())
                    {
                        state = LocalState.stopped;
                        tempoDecorrido = 0;
                        targetTime = Random.Range(minStoppedTime, maxStopeedTime);
                    }
                    break;
                case LocalState.stopped:
                    tempoDecorrido += Time.deltaTime;
                    if (tempoDecorrido > targetTime)
                    {
                        IniciarCaminhadaComNovoPonto();
                    }
                break;

            }
        }
    }
}