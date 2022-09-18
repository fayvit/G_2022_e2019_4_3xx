using FayvitMessageAgregator;
using UnityEngine;

namespace FayvitUI
{
    public struct ConfirmationPanelBtnYesMessage : IMessageBase { }
    public struct ConfirmationPanelBtnNoMessage : IMessageBase { }
    public struct MsgConfirmationPanelChangeOption : IMessageBase { public bool hideSound; }
    public struct MsgCloseMessagePanel : IMessageBase { public bool hideSound; }
    public struct MsgAddTextDisplayIndex : IMessageBase { public string message; }
    public struct FillingTextDisplayMessage : IMessageBase { }
    public struct FillTextDisplayMessage : IMessageBase { }
    public struct TextBoxGoingMessage : IMessageBase {}
    public struct TextBoxCommingMessage : IMessageBase {}
    public struct MsgChangeOptionUI : IMessageBase {
        public GameObject parentOfScrollRect;
        public int selectedOption;
    }
    

}
