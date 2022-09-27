using Criatures2021;
using FayvitMessageAgregator;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Criatures_Scripts.CriatureReborn.Eventos
{
    public class TriggerChangeKeyDjeyEnterAndExit : MonoBehaviour
    {
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                MessageAgregator<MsgChangeKeyDjeyPermission>.Publish(new MsgChangeKeyDjeyPermission()
                {
                    change = true,
                    gameObject = other.gameObject
                });
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                MessageAgregator<MsgChangeKeyDjeyPermission>.Publish(new MsgChangeKeyDjeyPermission()
                {
                    change = false,
                    gameObject = other.gameObject
                });
            }
            else if (other.gameObject.CompareTag("Criature"))
            {
                KeyDjeyTransportManager k = other.GetComponent<KeyDjeyTransportManager>();

                if (k) k.SairDoKeyDjey();
            }
        }
    }
}