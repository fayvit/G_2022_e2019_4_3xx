
using FayvitMessageAgregator;
using UnityEngine;

public struct MsgStartExternalInteraction : IMessageBase { }
public struct MsgFinishExternalInteraction : IMessageBase { }

public struct MsgSendExternalPanelCommand : IMessageBase
{
    public bool confirmButton;
    public bool returnButton;
    public bool extraButton;
    public bool leftChangeButton;
    public bool rightChangeButton;
    public bool pauseMenu;
    public int hChange;
    public int vChange;
    public int leftTrigger;
    public int rightTrigger;
}

public struct MsgRequestExternalMoviment : IMessageBase
{
    public GameObject oMovimentado;
}

public struct MsgChangeToHero : IMessageBase
{
    public GameObject myHero;
    public bool blockReturnCam;
}

