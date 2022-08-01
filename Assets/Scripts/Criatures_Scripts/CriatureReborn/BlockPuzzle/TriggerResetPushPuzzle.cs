using FayvitMessageAgregator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Criatures2021
{
    public class TriggerResetPushPuzzle : MonoBehaviour
    {

        // Start is called before the first frame update
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
                MessageAgregator<MsgResetPushPuzzle>.Publish(new MsgResetPushPuzzle() { sender = gameObject });
            }
        }
    }

    public struct MsgResetPushPuzzle : IMessageBase
    {
        public GameObject sender;
    }
}