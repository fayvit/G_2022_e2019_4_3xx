using FayvitMessageAgregator;
using FayvitSupportSingleton;
using UnityEngine;

namespace Criatures2021
{
    public class CheckableBlock : MonoBehaviour
    {
        private void Start()
        {
            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<MsgSendBlockCheck>.Publish(new MsgSendBlockCheck()
                {
                    observed = gameObject
                });
            });
        }
    }
}
