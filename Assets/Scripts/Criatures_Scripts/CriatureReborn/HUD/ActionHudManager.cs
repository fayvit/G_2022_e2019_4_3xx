using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FayvitMessageAgregator;

namespace Criatures2021Hud
{
    public class ActionHudManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainObject;
        [SerializeField] private Text infoText;
        [SerializeField] private Text infoCommand;

        private GameObject hudRequest;

        static ActionHudManager instance;

        public static bool Active => instance.mainObject.activeSelf;

        // Use this for initialization
        void Start()
        {
            instance = this;
            MessageAgregator<MsgRequestShowActionHud>.AddListener(OnRequestShow);
            MessageAgregator<MsgRequestHideActionHud>.AddListener(OnRequestHide);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgRequestShowActionHud>.RemoveListener(OnRequestShow);
            MessageAgregator<MsgRequestHideActionHud>.RemoveListener(OnRequestHide);
        }

        private void OnRequestHide(MsgRequestHideActionHud obj)
        {
            if(obj.request == hudRequest)
                mainObject.SetActive(false);
        }

        private void OnRequestShow(MsgRequestShowActionHud obj)
        {
            this.infoCommand.text = obj.infoCommand;
            this.infoText.text = obj.infoText;
            hudRequest = obj.request;

            mainObject.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void InvokeAction()
        {
            MessageAgregator<MsgInvokeActionFromHud>.Publish();
        }
    }

    public struct MsgInvokeActionFromHud : IMessageBase { }

    public struct MsgRequestShowActionHud : IMessageBase
    {
        public string infoText;
        public string infoCommand;
        public GameObject request;
    }

    public struct MsgRequestHideActionHud : IMessageBase {
        public GameObject request;
    }
}
