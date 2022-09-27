using Criatures2021;
using FayvitMessageAgregator;
using UnityEngine;

namespace Assets.Scripts.Criatures_Scripts.CriatureReborn.Eventos
{
    public class TriggerChangeKeyDjey : MonoBehaviour
    {
        [SerializeField] private bool change;  

        private void OnTriggerEnter(Collider other)
        {
            
            if (other.gameObject.CompareTag("Player"))
            {
                MessageAgregator<MsgChangeKeyDjeyPermission>.Publish(new MsgChangeKeyDjeyPermission()
                {
                    change = change,
                    gameObject = other.gameObject
                });
            }
            else if (other.gameObject.CompareTag("Criature")&&change==false)
            {
                KeyDjeyTransportManager k = other.GetComponent<KeyDjeyTransportManager>();

                if(k) k.SairDoKeyDjey();
            }
        }
    }
}