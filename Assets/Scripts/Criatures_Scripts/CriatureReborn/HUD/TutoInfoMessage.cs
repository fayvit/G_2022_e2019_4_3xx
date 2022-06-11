using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TextBankSpace;
using FayvitBasicTools;
using FayvitMessageAgregator;
using FayvitSounds;

namespace Criatures2021Hud
{
    public class TutoInfoMessage : MonoBehaviour
    {
        [SerializeField] private Image imgInfo;
        [SerializeField] private Text titleInfo;
        [SerializeField] private Text shortDescription;
        [SerializeField] private Text LargeDescription;
        [SerializeField] private GameObject container;

        private bool eMenu;

        private MsgSendExternalPanelCommand command = new MsgSendExternalPanelCommand();

        private void Start()
        {
            MessageAgregator<MsgRequestOpenInfoMessage>.AddListener(OnRequestOpen);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgRequestOpenInfoMessage>.RemoveListener(OnRequestOpen);
        }

        private void OnRequestOpen(MsgRequestOpenInfoMessage obj)
        {
            AbstractGameController.Instance.MyKeys.MudaAutoShift(obj.info.ToString() + "open", true);
            MessageAgregator<MsgStartExternalInteraction>.Publish();
            MessageAgregator<MsgSendExternalPanelCommand>.AddListener(OnReceiveExternalCommand);

            container.SetActive(true);
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx() { sfxId = SoundEffectID.painelAbrindo });
            FillMessage(obj.info,false);
        }

        private void OnReceiveExternalCommand(MsgSendExternalPanelCommand obj)
        {
            command = obj;
        }

        public void FillMessage(InfoMessageType info,bool eMenu)
        {
            this.eMenu = eMenu;

            command = new MsgSendExternalPanelCommand();
            TextKey key = StringForEnum.GetEnum<TextKey>(info.ToString() + "Info");
            titleInfo.text = TextBank.RetornaListaDeTextoDoIdioma(key)[0];
            shortDescription.text = TextBank.RetornaListaDeTextoDoIdioma(key)[1];
            LargeDescription.text = TextBank.RetornaListaDeTextoDoIdioma(key)[2];
            imgInfo.sprite = ResourcesFolders.GetMiniInfo(info);
        }

        public void BtnOk()
        {
            container.SetActive(false);
            MessageAgregator<MsgCloseInfoMessage>.Publish();
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx() { sfxId = SoundEffectID.Book1 });
        }

        private void Update()
        {
            if (container.activeSelf && (command.confirmButton||command.returnButton)&&!eMenu)
            {
                BtnOk();
                command = new MsgSendExternalPanelCommand();
            }
        }
    }

    public enum InfoMessageType
    {
        dodge,
        targetLock,
        alternancia,
        mudaGolpe,
        atacar
    }

    public struct MsgRequestOpenInfoMessage : IMessageBase
    {
        public InfoMessageType info;
    }

    public struct MsgCloseInfoMessage : IMessageBase { }
}