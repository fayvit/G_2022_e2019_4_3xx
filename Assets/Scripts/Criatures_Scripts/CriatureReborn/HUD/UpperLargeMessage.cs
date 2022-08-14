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
            mainText.resizeTextForBestFit = mainText.IsOverflowingVerticle()?true:obj.useBestFit;

            


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
 
 public static class TextExtension
    {
        /// <summary>
        /// Returns true when the Text object contains more lines of text than will fit in the text container vertically
        /// </summary>
        /// <returns></returns>
        public static bool IsOverflowingVerticle(this Text text)
        {
            return LayoutUtility.GetPreferredHeight(text.rectTransform) > GetCalculatedPermissibleHeight(text);
        }

        private static float GetCalculatedPermissibleHeight(Text text)
        {
            //if (cachedCalculatedPermissibleHeight != -1) return cachedCalculatedPermissibleHeight;

            float cachedCalculatedPermissibleHeight = text.gameObject.GetComponent<RectTransform>().rect.height;
            return cachedCalculatedPermissibleHeight;
        }
        //private static float cachedCalculatedPermissibleHeight = -1;

        /// <summary>
        /// Returns true when the Text object contains more character than will fit in the text container horizontally
        /// </summary>
        /// <returns></returns>
        public static bool IsOverflowingHorizontal(this Text text)
        {
            return LayoutUtility.GetPreferredWidth(text.rectTransform) > GetCalculatedPermissibleWidth(text);
        }

        private static float GetCalculatedPermissibleWidth(Text text)
        {
            //if (cachedCalculatedPermissiblWidth != -1) return cachedCalculatedPermissiblWidth;

            float cachedCalculatedPermissiblWidth = text.gameObject.GetComponent<RectTransform>().rect.width;
            return cachedCalculatedPermissiblWidth;
        }
        //private static float cachedCalculatedPermissiblWidth = -1;

    }
}