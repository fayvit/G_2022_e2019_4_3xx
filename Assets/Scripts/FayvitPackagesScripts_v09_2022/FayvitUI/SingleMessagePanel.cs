using UnityEngine;
using UnityEngine.UI;
using FayvitMessageAgregator;
using FayvitBasicTools;

namespace FayvitUI
{
    public class SingleMessagePanel : MonoBehaviour
    {

        private System.Action onClose;

        [SerializeField] private Text messageText = default;
        [SerializeField] private Text messageButton = default;
        [SerializeField] private Text infoButtonLabel;

        private bool hideCloseSound;

        // Use this for initialization
        public void StartMessagePanel(
            System.Action closeAction,
            string messageText,
            string messageButton = "Ok",
            string infoButtonText = "",
            bool hideOpenSound = false,
            bool hideCloseSound = false
            )
        {
            this.hideCloseSound = hideCloseSound;
            gameObject.SetActive(true);
            this.messageText.text = messageText;
            this.messageButton.text = messageButton;
            onClose = closeAction;
            infoButtonLabel.text = infoButtonText;

            
            MessageAgregator<MsgOpenOneMessage>.Publish(new MsgOpenOneMessage()
            {
                hideSound = hideOpenSound
            });
            
        }

        public void StartMessagePanel(System.Action closeAction)
        {
            gameObject.SetActive(true);
            onClose = closeAction;
        }

        public void ChangeMessageText(string s)
        {
            messageText.text = s;
        }

        public void ChangeButtonText(string s)
        {
            messageButton.text = s;
        }

        public void ChangeMessageAndButtonText(string buttonText, string messageText)
        {
            ChangeMessageText(messageText);
            ChangeButtonText(buttonText);
        }

        public void BtnCallback()
        {
            gameObject.SetActive(false);

            if (onClose != null)
            {
                onClose();
                onClose = null;
            }

        }

        public void ThisUpdate(bool input)
        {
            if (input)
            {
                //if (!hideCloseSound)
                    //MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                    //{
                    //    sfxId = FayvitSounds.SoundEffectID.Book1
                    //});
                MessageAgregator<MsgCloseMessagePanel>.Publish(new MsgCloseMessagePanel() { hideSound = hideCloseSound});
                //EventAgregator.Publish(EventKey.closeMessagePanel);
                BtnCallback();
            }
        }
    }
}

public struct MsgOpenOneMessage : IMessageBase { public bool hideSound; }
//public struct MsgCloseOneMessage : IMessageBase { public bool hide; }