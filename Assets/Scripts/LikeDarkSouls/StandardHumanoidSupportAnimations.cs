using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitMessageAgregator;

namespace FayvitLikeDarkSouls
{
    public class StandardHumanoidSupportAnimations
    {
        public static void RequestHitAnimation(GameObject G, Vector3 dir)
        {
            Transform transform = G.transform;
            float fDot = Vector3.Dot(transform.forward, dir);
            float rDot = Vector3.Dot(transform.right, dir);

            string animKey = "F1";
            if (fDot > .7 && rDot > 0)
            {
                animKey = "F1";
            }
            else if (fDot < .7 && fDot > -.7 && rDot > 0)
            {
                animKey = "R1";
            }
            else if (fDot < .7 && fDot > -.7 && rDot < 0)
            {
                animKey = "L1";
            }
            else if (fDot < -.7)
            {
                animKey = "B1";
            }
            else
            if (fDot > .7 && rDot < 0)
            {
                animKey = "F2";
            }

            MessageAgregator<MsgDamageAnimate>.Publish(new MsgDamageAnimate()
            {
                G = G,
                animKey = animKey
            });
            //FayvitMoveEventAgregator.Publish(new FayvitMoveEvent(FayvitMoveEventKey.damageAnimate, G, animKey));
        }
    }

    public struct MsgDamageAnimate : IMessageBase {
        public GameObject G;
        public string animKey;
    }
}