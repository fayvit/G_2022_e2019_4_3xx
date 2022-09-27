using Criatures2021;
using System.Collections;
using UnityEngine;
using FayvitMessageAgregator;


public class DarkScreenDamageForLastGroundedPosition : DarkenScreenDamageBase
{
    protected override void Posicione()
    {
        MessageAgregator<MsgRequestLastGroundedPosition>.Publish(new MsgRequestLastGroundedPosition()
        {
            who = Moved,
            petChangePosition=true
        });
    }
}

public struct MsgRequestLastGroundedPosition:IMessageBase
{
    public Transform who;
    public bool petChangePosition;
}

