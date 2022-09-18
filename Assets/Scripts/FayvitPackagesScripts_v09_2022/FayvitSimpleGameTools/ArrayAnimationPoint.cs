using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitBasicTools
{
    public class ArrayAnimationPoint : StateMachineBehaviour
    {
        [SerializeField] private AnimationPoint[] points;

        [System.Serializable]
        private class AnimationPoint
        {
            public float animationPoint = 0.889f;
            public string extraInfo = "";
            [HideInInspector] public bool requestSend;
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i].requestSend = true;
            }

            base.OnStateEnter(animator, stateInfo, layerIndex);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].requestSend && stateInfo.normalizedTime > points[i].animationPoint)
                {
                    points[i].requestSend = false;
                    FayvitMessageAgregator.MessageAgregator<MsgAnimationPointCheck>.Publish(new MsgAnimationPointCheck()
                    {
                        sender = animator.gameObject,
                        extraInfo = points[i].extraInfo,
                        animationClipName = animator.GetCurrentAnimatorClipInfo(layerIndex)[0].clip.name
                    });

                    Debug.Log(animator.GetCurrentAnimatorClipInfo(layerIndex)[0].clip.name + " : " + points[i].extraInfo);
                }
            }
            base.OnStateUpdate(animator, stateInfo, layerIndex);
        }
    }
}