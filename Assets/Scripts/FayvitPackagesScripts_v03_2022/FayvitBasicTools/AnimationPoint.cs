using FayvitMessageAgregator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPoint : StateMachineBehaviour
{
    [SerializeField] private float animationPoint = 0.889f;
    [SerializeField] private string extraInfo = "";
    bool requestSend;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        requestSend = true;
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (requestSend && stateInfo.normalizedTime > animationPoint)
        {
            requestSend = false;

            MessageAgregator<MsgAnimationPointCheck>.Publish(new MsgAnimationPointCheck()
            {
                sender = animator.gameObject,
                animationClipName = animator.GetCurrentAnimatorClipInfo(layerIndex)[0].clip.name,
                extraInfo = extraInfo
            });
            //EventAgregator.Publish(new StandardSendGameEvent(animator.gameObject,EventKey.animationPointCheck,
            //    animator.GetCurrentAnimatorClipInfo(layerIndex)[0].clip.name,extraInfo));

            Debug.Log(animator.GetCurrentAnimatorClipInfo(layerIndex)[0].clip.name + " : " + extraInfo);
        }
        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }
}

public struct MsgAnimationPointCheck : IMessageBase
{
    public GameObject sender;
    public string animationClipName;
    public string extraInfo;
}
