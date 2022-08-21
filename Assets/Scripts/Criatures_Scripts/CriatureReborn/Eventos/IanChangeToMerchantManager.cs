using UnityEngine;
using FayvitBasicTools;
using FayvitMessageAgregator;
using FayvitSupportSingleton;

namespace Criatures2021
{
    class IanChangeToMerchantManager:MonoBehaviour
    {
        [SerializeField] private MonoBehaviour merchant;
        [SerializeField] private MonoBehaviour firstTalk;

        private void Start()
        {
            if (StaticInstanceExistence<IGameController>.SchelduleExistence(Start, this, () =>
            {
                return AbstractGameController.Instance;
            }))
            {
                KeyVar myKeys = AbstractGameController.Instance.MyKeys;
                
                if (myKeys.VerificaAutoShift(KeyShift.entregouCanetaDeIan))
                {
                    MudarManager();
                    Destroy(this);
                }
                else
                    MessageAgregator<MsgGetLogPen>.AddListener(OnGetLogPen);
            }
        }

        private void OnGetLogPen(MsgGetLogPen obj)
        {
            MudarManager();
            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<MsgGetLogPen>.RemoveListener(OnGetLogPen);
            });
        }

        void MudarManager()
        {
            merchant.enabled = true;
            Destroy(firstTalk);
        }
    }

    public struct MsgGetLogPen : IMessageBase { }

}
