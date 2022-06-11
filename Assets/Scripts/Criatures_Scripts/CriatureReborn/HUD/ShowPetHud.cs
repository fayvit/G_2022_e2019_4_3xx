using UnityEngine;
using Criatures2021;
using FayvitMessageAgregator;

namespace Criatures2021Hud
{
    [System.Serializable]
    public class ShowPetHud : ShowPetBase
    {
        [SerializeField] private GameObject goContainer;

        private void Start()
        {
            MessageAgregator<MsgInsertCaptureInfos>.AddListener(OnRequestCaptureInfos);
            MessageAgregator<MsgHideShowPetHud>.AddListener(OnRequestHide);
            MessageAgregator<MsgEndOfCaptureAnimate>.AddListener(OnEndCaptureAnimate);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgInsertCaptureInfos>.RemoveListener(OnRequestCaptureInfos);
            MessageAgregator<MsgHideShowPetHud>.RemoveListener(OnRequestHide);
            MessageAgregator<MsgEndOfCaptureAnimate>.RemoveListener(OnEndCaptureAnimate);
        }

        private void OnRequestHide(MsgHideShowPetHud obj)
        {
            goContainer.SetActive(false);
        }

        private void OnEndCaptureAnimate(MsgEndOfCaptureAnimate obj)
        {
            goContainer.SetActive(false);
        }

        private void OnRequestCaptureInfos(MsgInsertCaptureInfos obj)
        {
            goContainer.SetActive(true);
            InserirDadosNoPainelPrincipal(obj.pet);
        }
    }

        

    public struct MsgHideShowPetHud : IMessageBase { }
}