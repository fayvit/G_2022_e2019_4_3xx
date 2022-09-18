using System.Collections;
using UnityEngine;
using FayvitBasicTools;
using Criatures2021;

namespace Assets.Scripts
{
    public class SkyPlanesPositioner : MonoBehaviour
    {
        [SerializeField] private Transform target;

        // Use this for initialization
        void Start()
        {
            if (StaticInstanceExistence<Transform>.SchelduleExistence(Start, this, () => { return MyGlobalController.MainCharTransform; }))
            {
                target = MyGlobalController.MainCharTransform;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (target)
            {
                transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
            }
        }
    }
}