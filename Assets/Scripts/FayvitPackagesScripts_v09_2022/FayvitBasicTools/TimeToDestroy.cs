using System.Collections;
using UnityEngine;

namespace FayvitBasicTools
{
    public class TimeToDestroy : MonoBehaviour
    {
        [SerializeField] private float time;
        // Use this for initialization
        void Start()
        {
            Destroy(gameObject, time);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}