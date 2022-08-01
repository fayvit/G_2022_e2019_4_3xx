using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitMessageAgregator;
using FayvitBasicTools;
using TalkSpace;
using FayvitLoadScene;
using ChangeScene;
using Spawns;

namespace Criatures2021
{
    public class TriggerSaidaDaCavernaLutaComCaesar : MonoBehaviour
    {
        [SerializeField] private string observedID;
        [SerializeField] private Transform caesar;
        [SerializeField] private Transform heroTargetPosition;
        [SerializeField] private NomesCenas[] cenasParaCarregar;
        [SerializeField] private SpawnID spawnId;

        private bool iniciouLoad;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //Debug.Log("Player no trigger");
                if(MyGlobalController.MainPlayer.ThisState==CharacterState.onFree)
                if (AbstractGameController.Instance.MyKeys.VerificaAutoShift(observedID) )
                {
                        if (!iniciouLoad)
                        {
                            iniciouLoad = true;
                            MudeCena.MudarCena(cenasParaCarregar, spawnId, other.transform);
                        }
                }
                else
                {
                    MessageAgregator<MsgStartExternalInteraction>.Publish();
                    AbstractGlobalController.Instance.FadeV.StartFadeOutWithAction(() => {
                        CharacterController controle = other.GetComponent<CharacterController>();
                        controle.enabled = false;
                        controle.transform.position = heroTargetPosition.position;
                        AbstractGlobalController.Instance.FadeV.StartFadeInWithAction(() =>
                        {
                            controle.enabled = true;
                            MessageAgregator<MsgRequestExternalFightStart>.Publish(new MsgRequestExternalFightStart()
                            {
                                fighters=caesar
                            });
                        });
                    });
                }
            }
            else if (other.CompareTag("Criature"))
            {
                if (!iniciouLoad)
                {
                    MessageAgregator<MsgBlockPetAdvanceInTrigger>.Publish(new MsgBlockPetAdvanceInTrigger()
                    {
                        pet = other.gameObject
                    });
                }
            }
        }
    }

    public struct MsgBlockPetAdvanceInTrigger : IMessageBase
    {
        public GameObject pet;
    }
}