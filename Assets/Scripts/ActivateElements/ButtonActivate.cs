using UnityEngine;
using System.Collections;
using FayvitBasicTools;
using FayvitMessageAgregator;

    public abstract class ButtonActivate : MonoBehaviour
    {
        //[SerializeField] protected GameObject btn;
        [SerializeField] private bool debug;
        [SerializeField] protected float distanciaParaAcionar = 1.5f;
        protected string textoDoBotao = "";
        private bool estaNoTrigger = false;

        

        //public GameObject Btn { get { return btn; } }

        // Use this for initialization
        void Start()
        {

        }

        protected void FluxoDeBotao()
        {
            MessageAgregator<MsgStartExternalInteraction>.Publish();
            Update();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (debug)
            {
                Debug.Log(AbstractGlobalController.Instance);
                Debug.Log(AbstractGlobalController.Instance.Players);
                Debug.Log(AbstractGlobalController.Instance.Players.Count);
                Debug.Log(Vector3.Distance(AbstractGlobalController.Instance.Players[0].Manager.transform.position, transform.position));
                Debug.Log(estaNoTrigger);
                Debug.Log(AbstractGlobalController.Instance.Players[0].Manager.ThisState);

            }

            if (AbstractGlobalController.Instance!=null 
                && AbstractGlobalController.Instance.Players!=null 
                && AbstractGlobalController.Instance.Players.Count > 0
                )
                
                    if (gameObject!=null
                        &&
                        Vector3.Distance(
                            AbstractGlobalController.Instance.Players[0].Manager.transform.position, transform.position) 
                                < distanciaParaAcionar
                        &&
                        estaNoTrigger
                        &&
                        AbstractGlobalController.Instance.Players[0].Manager.ThisState == CharacterState.onFree
                        &&
                        ActionManager.Instance.PodeVisualizarEste(this)
                        //  &&
                        // GameController.g.EmEstadoDeAcao()
                        &&
                        gameObject.activeSelf
                        )
                    {
                        MessageAgregator<MsgRequestShowActionHud>.Publish(
                            new MsgRequestShowActionHud() { 
                            infoCommand = "L",
                            infoText = textoDoBotao,
                            request = gameObject
                            }
                            );
                        //btn.SetActive(true);
                    }
                    else
                    {
                    MessageAgregator<MsgRequestHideActionHud>.Publish(new MsgRequestHideActionHud()
                    {
                        request = gameObject
                    });
                        /*
                        if (Vector3.Distance(GameController.g.Manager.transform.position, transform.position) >= distanciaParaAcionar)
                        {*/
                        //btn.SetActive(false);
                        /*  }else
                          if (ActionManager.TransformDeActionE(transform))
                          {

                              btn.SetActive(true);
                          }*/

                    }

        }
        protected void SempreEstaNoTrigger()
        {
            estaNoTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                estaNoTrigger = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                estaNoTrigger = false;
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.tag == "Player")
            {
                estaNoTrigger = true;
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.tag == "Player")
            {
                estaNoTrigger = false;
            }
        }

        public virtual void SomDoIniciar()
        {
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.painelAbrindo
            });
        }

        /*
        public virtual void SomDoIniciar()
        {
            EventAgregator.Publish(new StandardSendStringEvent(gameObject, SoundEffectID.Decision1.ToString(), EventKey.disparaSom));
        }*/

        public abstract void FuncaoDoBotao();
    }

    public struct MsgRequestShowActionHud : IMessageBase
    {
        public string infoText;
        public string infoCommand;
        public GameObject request;
    }

public struct MsgRequestHideActionHud : IMessageBase
{
    public GameObject request;
}

//}