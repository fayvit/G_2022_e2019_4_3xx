using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitCam;
using FayvitMessageAgregator;

namespace Criatures2021
{
    public class TriggerReturnToStandardCam : MonoBehaviour
    {
        // Start is called before the first frame update
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.tag + " : " + CameraApplicator.cam.Style);
            if (other.CompareTag("Player") && CameraApplicator.cam.Style == CameraApplicator.EstiloDeCamera.basic)
            {
                SetHeroCamera.Set(other.transform);
            }
        }
    }
}