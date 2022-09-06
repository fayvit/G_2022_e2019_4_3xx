using FayvitMessageAgregator;

public struct MsgRequestRapidInfo : IMessageBase
{
    public string message;
    public bool useBestSize;
    public int chosenSize;
}