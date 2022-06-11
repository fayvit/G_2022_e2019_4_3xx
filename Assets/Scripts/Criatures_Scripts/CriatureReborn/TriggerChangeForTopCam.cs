using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitCam;
using FayvitBasicTools;

namespace Criatures2021
{
    public class TriggerChangeForTopCam : MonoBehaviour
    {
        [SerializeField] private Transform camPoint;
        
        private CheckPushBlockPuzzle chekable;

        private void Start()
        {
            chekable = GetComponentInParent<CheckPushBlockPuzzle>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !AbstractGameController.Instance.MyKeys.VerificaAutoShift(chekable.GetKey))
            {
                
                float targetHeight = Mathf.Abs(camPoint.position.y- other.transform.position.y);
                float targetDistance = DirectionOnThePlane.InTheUp(camPoint.position, other.transform.position).magnitude;
                CameraApplicator.cam.NewFocusForBasicCam(other.transform, targetHeight, targetDistance);
            }

        }
    }
}