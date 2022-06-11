using System.Collections;
using UnityEngine;
using FayvitMessageAgregator;
using TextBankSpace;
using FayvitBasicTools;

namespace TalkSpace
{
    public class StartTalkShiftKey : MonoBehaviour
    {
        [SerializeField] private TextKey tKey;
        [SerializeField] private string changeString;
        [SerializeField] private bool stringKeyChangeVal;
        [SerializeField] private KeyShift changeKeyShift = KeyShift.sempretrue;
        [SerializeField] private bool keyChangeVal;
        // Use this for initialization
        void Start()
        {
            MessageAgregator<MsgStartTextDisplay>.AddListener(OnStartTextDisplay);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgStartTextDisplay>.RemoveListener(OnStartTextDisplay);
        }

        private void OnStartTextDisplay(MsgStartTextDisplay obj)
        {
            if (obj.sender == gameObject)
            {
                if (obj.talkKey == tKey)
                {
                    KeyVar MyKeys = AbstractGameController.Instance.MyKeys;
                    if (changeKeyShift != KeyShift.sempretrue)
                        MyKeys.MudaShift(changeKeyShift, keyChangeVal);

                    if (!string.IsNullOrEmpty(changeString))
                        MyKeys.MudaAutoShift(changeString, stringKeyChangeVal);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}