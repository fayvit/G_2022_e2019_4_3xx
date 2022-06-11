using FayvitMessageAgregator;
using UnityEngine;

namespace FayvitUI
{
    public struct ConfirmationPanelBtnYesMessage : IMessageBase { }
    public struct ConfirmationPanelBtnNoMessage : IMessageBase { }
    public struct MsgCloseMessagePanel : IMessageBase { }
    public struct FillingTextDisplayMessage : IMessageBase { }
    public struct FillTextDisplayMessage : IMessageBase { }
    public struct TextBoxGoingMessage : IMessageBase { }
    public struct TextBoxCommingMessage : IMessageBase { }
    public struct MsgChangeOptionUI : IMessageBase {
        public GameObject parentOfScrollRect;
        public int selectedOption;
    }
    

}
