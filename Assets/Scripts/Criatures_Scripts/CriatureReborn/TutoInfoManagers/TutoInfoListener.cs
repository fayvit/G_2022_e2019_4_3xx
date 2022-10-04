using UnityEngine;
using FayvitMessageAgregator;
using Criatures2021Hud;

namespace Criatures2021
{

    public class TutoInfoListener:MonoBehaviour
    {
        private GameObject heroGameObject;
        public void Start()
        {
            MessageAgregator<MsgRequestDodgeInfoEvent>.AddListener(OnRequestDodgeInfoEvent);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgRequestDodgeInfoEvent>.RemoveListener(OnRequestDodgeInfoEvent);
        }

        private void OnRequestDodgeInfoEvent(MsgRequestDodgeInfoEvent obj)
        {
            heroGameObject = obj.heroGameObject;
            MessageAgregator<MsgRequestOpenInfoMessage>.Publish(new MsgRequestOpenInfoMessage()
            {
                info = InfoMessageType.dodge
            });
            MessageAgregator<MsgCloseInfoMessage>.AddListener(OnCloseInfoMessage);
        }

        private void OnCloseInfoMessage(MsgCloseInfoMessage obj)
        {
            MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero()
            {
                myHero = heroGameObject
            });
        }
    }
}
