using FayvitMessageAgregator;
using System.Collections;
using UnityEngine;
using Criatures2021;

namespace FieldEnemy
{
    public class TriggerForPetVigilance : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                MessageAgregator<MsgPlayerInVigilanceTrigger>.Publish(new MsgPlayerInVigilanceTrigger()
                {
                    player = other.transform,
                    trigger = gameObject
                });
            }
            else if (other.CompareTag("Criature"))
            {
                PetManagerCharacter P = other.GetComponent<PetManagerCharacter>();
                if (P != null)
                {
                    MessageAgregator<MsgPlayerInVigilanceTrigger>.Publish(new MsgPlayerInVigilanceTrigger()
                    {
                        player = other.transform,
                        trigger = gameObject
                    });
                }
            }
        }
    }
}