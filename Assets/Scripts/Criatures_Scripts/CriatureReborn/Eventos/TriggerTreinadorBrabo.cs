using Criatures2021;
using FayvitBasicTools;
using FayvitMessageAgregator;
using System.Collections;
using TalkSpace;
using UnityEngine;

namespace Eventos
{
    public class TriggerTreinadorBrabo : MonoBehaviour
    {
        
        [SerializeField] private Transform heroTargetPosition;
        [SerializeField] private TrainerForFight trainer;

        private bool iniciouLuta;

        // Use this for initialization
        void Start()
        {

        }

        void EfetivarAcao()
        {


            MessageAgregator<MsgStartExternalInteraction>.Publish();
            AbstractGlobalController.Instance.FadeV.StartFadeOutWithAction(() =>
            {
                CharacterController controle = MyGlobalController.MainPlayer.GetComponent<CharacterController>();
                controle.enabled = false;
                controle.transform.position = MelhoraInstancia3D.ProcuraPosNoMapa(heroTargetPosition.position);
                AbstractGlobalController.Instance.FadeV.StartFadeInWithAction(() =>
                {
                    controle.enabled = true;
                    MessageAgregator<MsgRequestExternalFightStart>.Publish(new MsgRequestExternalFightStart()
                    {
                        fighters = trainer.transform
                    });
                });
            });

        }

        private void OnTriggerEnter(Collider other)
        {
            if (!AbstractGameController.Instance.MyKeys.VerificaAutoShift(trainer.ChaveDaLuta))
            {
                if (other.CompareTag("Player"))
                {
                    //Debug.Log("Player no trigger");
                    if (MyGlobalController.MainPlayer.ThisState == CharacterState.onFree)
                        EfetivarAcao();
                }
                else if (other.CompareTag("Criature"))
                {
                    if (!iniciouLuta)
                    {
                        KeyDjeyTransportManager kTransport = other.GetComponent<KeyDjeyTransportManager>();
                        if (kTransport)
                        {
                            kTransport.SairDoKeyDjey(returnState: CharacterState.stopped, melhorarPosicaoDoPet: false);
                            EfetivarAcao();
                        }
                    }
                }
            }
        }
    }
}