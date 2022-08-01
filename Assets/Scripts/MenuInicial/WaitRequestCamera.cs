using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitMessageAgregator;

public class WaitRequestCamera : MonoBehaviour
{
    [SerializeField] private GameObject containerDaCamera;

    // Start is called before the first frame update
    void Start()
    {
        MessageAgregator<MsgRequestCam>.AddListener(OnRequestCam);
    }

    private void OnDestroy()
    {
        MessageAgregator<MsgRequestCam>.RemoveListener(OnRequestCam);
    }

    private void OnRequestCam(MsgRequestCam obj)
    {
        containerDaCamera.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}

public struct MsgRequestCam : IMessageBase { }
