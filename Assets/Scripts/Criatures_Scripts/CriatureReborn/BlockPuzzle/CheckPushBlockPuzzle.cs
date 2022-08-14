using UnityEngine;
using System.Collections.Generic;
using FayvitBasicTools;
using FayvitMessageAgregator;
using FayvitSupportSingleton;
using FayvitCam;
using FayvitSounds;

namespace Criatures2021
{
    public class CheckPushBlockPuzzle:MonoBehaviour
    {
        [SerializeField] private List<CheckableGameObject> observados = new List<CheckableGameObject>();
        [SerializeField] private GameObject bau;
        [SerializeField] private GameObject particulaDoBau;
        [SerializeField] private SoundEffectID vinhetaDaAparicao = SoundEffectID.vinhetinhaMedia;
        [SerializeField] private SoundEffectID somDaParticula = SoundEffectID.XP_Earth02;
        [SerializeField] private string ID;

        [System.Serializable]
        private struct CheckableGameObject
        {
            public GameObject G;
            public bool check;
        }

        public string GetKey => ID;
        private MsgSendExternalPanelCommand commands;
        private bool listenCommands=false;

        private void Start()
        {
            if (StaticInstanceExistence<IGameController>.SchelduleExistence(Start, this, () =>
            {
                return AbstractGameController.Instance;
            }))
            {
                if (AbstractGameController.Instance.MyKeys.VerificaAutoShift(ID))
                {
                    DisablePuzzle();
                }

                MessageAgregator<MsgSendBlockCheck>.AddListener(OnReceiveBlockCheck);
                MessageAgregator<MsgChangeCheck>.AddListener(OnChangeCheck);
                MessageAgregator<MsgResetPushPuzzle>.AddListener(OnRequestReset);
            }

            //MessageAgregator<MsgFinishEdition>.AddListener(XXX);
        }

        //private void XXX(MsgFinishEdition obj)
        //{
        //    CheckProcedure();
        //}

        private void OnDestroy()
        {
            MessageAgregator<MsgSendBlockCheck>.RemoveListener(OnReceiveBlockCheck);
            MessageAgregator<MsgChangeCheck>.RemoveListener(OnChangeCheck);
            MessageAgregator<MsgResetPushPuzzle>.RemoveListener(OnRequestReset);
        }

        private void OnRequestReset(MsgResetPushPuzzle obj)
        {
            if (HierarchyTools.EstaNaHierarquia(transform, obj.sender.transform) &&
                !AbstractGameController.Instance.MyKeys.VerificaAutoShift(GetKey)
                &&
                !MyGlobalController.MainPlayer.ContraTreinador
                )
            {
                commands = new MsgSendExternalPanelCommand();
                MessageAgregator<MsgStartExternalInteraction>.Publish();
                MessageAgregator<MsgSendExternalPanelCommand>.AddListener(OnReceiveCommand);
                listenCommands = true;

                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx() { sfxId = SoundEffectID.Decision1 });

                AbstractGlobalController.Instance.Confirmation.StartConfirmationPanel(ResetPuzzle, () =>
                {
                    MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero()
                    {
                        blockReturnCam = true,
                        myHero = MyGlobalController.MainPlayer.gameObject
                    });

                    listenCommands = false;
                    MessageAgregator<MsgSendExternalPanelCommand>.RemoveListener(OnReceiveCommand);
                    MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx() { sfxId = SoundEffectID.Book1 });

                }, "Deseja reiniciar o Puzzle?");

                
                commands = new MsgSendExternalPanelCommand();
                
            }
        }

        private void OnReceiveCommand(MsgSendExternalPanelCommand obj)
        {
            commands = obj;
        }

        private void ResetPuzzle()
        {
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx() { sfxId = SoundEffectID.Decision2 });
            listenCommands = false;
            AbstractGlobalController.Instance.FadeV.StartFadeOutWithAction(OnFadeOutReset,.5f);
            MessageAgregator<MsgSendExternalPanelCommand>.RemoveListener(OnReceiveCommand);
        }

        private void OnFadeOutReset()
        {
            Debug.Log(MyGlobalController.MainPlayer.transform);
            Debug.Log(GetComponentInChildren<TriggerReturnToStandardCam>().transform);
            CharacterController cc = MyGlobalController.MainPlayer.GetComponent<CharacterController>();
            
            cc.enabled = false;
            cc.transform.position
                = MelhoraInstancia3D.ProcuraPosNoMapa(
                GetComponentInChildren<TriggerReturnToStandardCam>().transform.position,1);
            cc.enabled = true;

            for (int i=0;i<observados.Count;i++)
            {
                observados[i] = new CheckableGameObject()
                {
                    check = false,
                    G = observados[i].G,
                };
            }
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx() { sfxId = SoundEffectID.Twine });
            MessageAgregator<MsgPushBlockReturnToStartPosition>.Publish(new MsgPushBlockReturnToStartPosition()
            {
                sender = gameObject
            });

            AbstractGlobalController.Instance.FadeV.StartFadeInWithAction(OnFadeInReset,.37f);
        }

        private void Update()
        {
            if (listenCommands)
            {
                //Debug.Log(commands.hChange + " : " + commands.vChange + " : " + commands.confirmButton + ":" + commands.returnButton);
                AbstractGlobalController.Instance.Confirmation
                    .ThisUpdate(commands.hChange!=0, commands.confirmButton, commands.returnButton);
            }
        }

        private void OnFadeInReset()
        {
            MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero()
            {
                myHero = MyGlobalController.MainCharTransform.gameObject
            });
        }

        private void OnValidate()
        {
            BuscadorDeID.Validate(ref ID, this);
        }

        void DisablePuzzle()
        {
            if (observados.Count > 0)
            {
                var v = FindObjectsOfType<PushBlockPuzzle>();
                List<PushBlockPuzzle> meusMeninos = new List<PushBlockPuzzle>();
                foreach (var vv in v)
                    if (HierarchyTools.EstaNaHierarquia(transform, vv.transform))
                        meusMeninos.Add(vv);

                for (int i = 0; i < observados.Count; i++)
                {
                    meusMeninos[i].transform.position = observados[i].G.transform.position + Vector3.up;
                    meusMeninos[i].enabled = false;
                }

                bau.SetActive(true);
            }
            else
                SupportSingleton.Instance.InvokeInSeconds(() =>
                {
                    DisablePuzzle();
                },.25f);

        }

        private void OnChangeCheck(MsgChangeCheck obj)
        {
            int index = IndexOf(obj.checkable);
            if (index>-1)
            {
                observados[index] = new CheckableGameObject()
                {
                    G = obj.checkable,
                    check = obj.check
                };

                if (TodosChecados())
                {
                    CheckProcedure();
                }
            }
        }

        void CheckProcedure()
        {

            AbstractGameController.Instance.MyKeys.MudaAutoShift(ID, true);

            MessageAgregator<MsgRequestExternalMoviment>.Publish(new MsgRequestExternalMoviment()
            {
                oMovimentado = MyGlobalController.MainCharTransform.gameObject
            });
            MessageAgregator<MsgRequestFadeOut>.Publish(new MsgRequestFadeOut()
            {
                darkenTime = .25f
            });
            MessageAgregator<FadeOutComplete>.AddListener(OnFadeOutComplete);
        }

        private void OnFadeOutComplete(FadeOutComplete obj)
        {
            CameraApplicator.cam.NewFocusForBasicCam(bau.transform, 6, 10, true);
            MessageAgregator<MsgRequestFadeIn>.Publish(new MsgRequestFadeIn()
            {
                lightenTime = .5f
            });
            MessageAgregator<FadeInComplete>.AddListener(OnFadeInComplete);
            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<FadeOutComplete>.RemoveListener(OnFadeOutComplete);
            });
        }

        private void OnFadeInComplete(FadeInComplete obj)
        {
            particulaDoBau.SetActive(true);
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = somDaParticula
            });
            SupportSingleton.Instance.InvokeInSeconds(() =>
            {
                bau.SetActive(true);
            }, .25f);
            SupportSingleton.Instance.InvokeInSeconds(() =>
            {
                DisablePuzzle();
                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                {
                    sfxId = vinhetaDaAparicao
                });
            }, .75f);
            SupportSingleton.Instance.InvokeInSeconds(() =>
            {
                MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero()
                {
                    myHero = MyGlobalController.MainCharTransform.gameObject
                });
            }, 1.75f);
            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<FadeInComplete>.RemoveListener(OnFadeInComplete);
            });
        }

        private bool TodosChecados()
        {
            bool retorno = true;
            foreach (var v in observados)
                retorno &= v.check;
            return retorno;
        }

        private int IndexOf(GameObject G)
        {
            int index = -1;
            for(int i=0;i<observados.Count;i++)
            {
                if (observados[i].G == G)
                    index = i;

            }
            return index;
        }

        private bool TemEsse(GameObject G)
        {
            foreach (var v in observados)
                if (v.G == G)
                    return true;

            return false;
        }

        private void OnReceiveBlockCheck(MsgSendBlockCheck obj)
        {
            
            if (HierarchyTools.EstaNaHierarquia( transform, obj.observed.transform)
                &&!TemEsse(obj.observed)
                )
            {
                observados.Add(new CheckableGameObject()
                {
                    G = obj.observed,
                    check = false
                });
            }
            
        }
    }

    public struct MsgSendBlockCheck : IMessageBase
    {
        public GameObject observed;
    }

    public struct MsgChangeCheck : IMessageBase
    {
        public GameObject checkable;
        public bool check;
    }

    public struct MsgPushBlockReturnToStartPosition : IMessageBase
    {
        public GameObject sender;
    }
}
