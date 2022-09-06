using FayvitMessageAgregator;

public struct MsgRequestUpperLargeMessage : IMessageBase
{
    public string message;
    public bool useBestFit;
    public float hideTime;
}

public struct MsgRequestHideUpperLargeMessage : IMessageBase { }