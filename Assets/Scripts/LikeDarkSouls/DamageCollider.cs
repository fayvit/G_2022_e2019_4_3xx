using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;
using System.Collections.Generic;

namespace FayvitLikeDarkSouls
{
    public class DamageCollider : MonoBehaviour
    {
        private List<GameObject> attackeds = new List<GameObject>();

        /*
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }*/

        public void Reestart()
        {
            attackeds.Clear();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!attackeds.Contains(other.gameObject))
            {
                Vector3 varDir = transform.position - other.transform.position;
                MessageAgregator<MsgDamageColliderEnter>.Publish(new MsgDamageColliderEnter()
                {
                    damageColliderGO = gameObject,
                    other = other.gameObject,
                    varDir = varDir
                });
                
                attackeds.Add(other.gameObject);
            }
        }
    }

    public struct MsgDamageColliderEnter : IMessageBase
    {
        public GameObject damageColliderGO;
        public GameObject other;
        public Vector3 varDir;
    }
}