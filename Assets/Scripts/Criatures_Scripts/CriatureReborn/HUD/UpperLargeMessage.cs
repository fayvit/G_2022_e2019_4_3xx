using UnityEngine;
using UnityEngine.UI;
using FayvitMessageAgregator;
using Criatures2021;

namespace Criatures2021Hud
{
    public class UpperLargeMessage : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private Text mainText;

        // Use this for initialization
        void Start()
        {
            MessageAgregator<MsgRequestHideUpperLargeMessage>.AddListener(OnRequestHide);
            MessageAgregator<MsgRequestUpperLargeMessage>.AddListener(OnRequestMessage);
            MessageAgregator<MsgEndOfCaptureAnimate>.AddListener(OnEndCaptureAnimate);
            MessageAgregator<MsgGetChestItem>.AddListener(OnGetChestItem);
        }


        private void OnDestroy()
        {
            MessageAgregator<MsgRequestHideUpperLargeMessage>.RemoveListener(OnRequestHide);
            MessageAgregator<MsgRequestUpperLargeMessage>.RemoveListener(OnRequestMessage);
            MessageAgregator<MsgEndOfCaptureAnimate>.RemoveListener(OnEndCaptureAnimate);
            MessageAgregator<MsgGetChestItem>.RemoveListener(OnGetChestItem);
        }

        private void OnGetChestItem(MsgGetChestItem obj)
        {
            OnRequestMessage(new MsgRequestUpperLargeMessage() { message = obj.message });
        }

        private void OnEndCaptureAnimate(MsgEndOfCaptureAnimate obj)
        {
            OnRequestHide(new MsgRequestHideUpperLargeMessage());
        }

        private void OnRequestMessage(MsgRequestUpperLargeMessage obj)
        {
            container.SetActive(true);
            mainText.text = obj.message;
            mainText.resizeTextForBestFit = obj.useBestFit;

            if (obj.hideTime != default)
            {
                FayvitSupportSingleton.SupportSingleton.Instance.InvokeInSeconds(() =>
                {
                    OnRequestHide(new MsgRequestHideUpperLargeMessage());
                }, obj.hideTime);
            }

        }

        private void OnRequestHide(MsgRequestHideUpperLargeMessage obj)
        {
            container.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    public struct MsgRequestUpperLargeMessage : IMessageBase {
        public string message;
        public bool useBestFit;
        public float hideTime;
    }

    public struct MsgRequestHideUpperLargeMessage : IMessageBase { }
}