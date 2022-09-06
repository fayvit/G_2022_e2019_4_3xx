using System.Collections;
using UnityEngine;
using FayvitMessageAgregator;
using FayvitUI;
using TextBankSpace;
using FayvitCam;
using FayvitBasicTools;
using TalkSpace;
using System;

namespace Criatures2021
{
    public class CamMovimentWithMessageIndex : MonoBehaviour
    {
        [SerializeField] private TextKey key;
        [SerializeField] private string keyForConvert;
        [SerializeField] private Transform alvoDoMovimento;
        [SerializeField] private int index;
        [SerializeField] private float alturaDaCamera=1;
        [SerializeField] private float distanciaDaCamera=6;

        private bool ativo;
        private void OnValidate()
        {
            if (!string.IsNullOrEmpty(keyForConvert))
            {
                key = StringForEnum.GetEnum<TextKey>(keyForConvert);
            }
        }

        // Use this for initialization
        void Start()
        {    
            MessageAgregator<MsgAddTextDisplayIndex>.AddListener(OnTextBoxCommingMessage);
            MessageAgregator<MsgFinishTextDisplay>.AddListener(OnFinishTextDisplay);
            
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgAddTextDisplayIndex>.RemoveListener(OnTextBoxCommingMessage);
            MessageAgregator<MsgFinishTextDisplay>.RemoveListener(OnFinishTextDisplay);
        }

        private void OnFinishTextDisplay(MsgFinishTextDisplay obj)
        {
            if (obj.sender == gameObject)
            {
                ativo = false;
            }
        }

        private void OnTextBoxCommingMessage(MsgAddTextDisplayIndex obj)
        {
            Debug.Log(obj.message + " : " + TextBank.RetornaListaDeTextoDoIdioma(key)[index]);
            if (TextBank.RetornaListaDeTextoDoIdioma(key)[index] == obj.message)
            {
                if (alvoDoMovimento != null)
                {
                    CameraApplicator.cam.StartShowPointCamera(alvoDoMovimento, new SinglePointCameraProperties()
                    {
                        withTime = true,
                        dodgeCam = true,
                        velOrTimeFocus = 1.25f
                    });

                    ativo = true;
                    
                }
            }
            else
                ativo = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (ativo)
            {
                CameraApplicator.cam.FocusInPoint(distanciaDaCamera, alturaDaCamera);
            }
        }
    }
}