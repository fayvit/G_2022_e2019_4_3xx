using UnityEngine;
using FayvitSounds;
using FayvitMessageAgregator;
using FayvitBasicTools;

namespace TalkSpace
{
    public class TalkButtonActivate : ButtonActivate
    {
        
        [SerializeField]
        private NameMusicaComVolumeConfig nameMusic = new NameMusicaComVolumeConfig()
        {
            Musica = NameMusic.empty,
            Volume = 1
        };

        private bool inputNext = false;
        private bool inputReturn = false;

        protected TalkManagerBase NPC { get; set; } = new TalkManagerBase();

        // Use this for initialization
        protected void Start()
        {
            MessageAgregator<MsgSendExternalPanelCommand>.AddListener(OnReceiveCommands);
            NPC.Start();

            SempreEstaNoTrigger();
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgSendExternalPanelCommand>.RemoveListener(OnReceiveCommands);
        }

        private void OnReceiveCommands(MsgSendExternalPanelCommand obj)
        {
            if (gameObject.activeSelf)
            {
                inputNext = obj.confirmButton;
                inputReturn = obj.returnButton;
            }
        }

        new protected void Update()
        {
            base.Update();

            if (NPC.Update(inputNext,inputReturn))
            {
                OnFinishTalk();
            }
        }

        protected virtual void OnFinishTalk()
        {
            MessageAgregator<MsgFinishExternalInteraction>.Publish();
            MessageAgregator<MsgReturnRememberedMusic>.Publish();
            AbstractGameController.Instance.MyKeys.MudaShift(KeyShift.sempretrue, true);
            AbstractGameController.Instance.MyKeys.MudaAutoShift(string.Empty, true);
            inputNext = false;
            inputReturn = false;
        }

        void BotaoConversa()
        {
            if (gameObject.activeSelf)
            {
                if (nameMusic.Musica != NameMusic.empty)
                {
                    MessageAgregator<MsgStartMusicWithRecovery>.Publish(new MsgStartMusicWithRecovery()
                    {
                        nmcvc = nameMusic
                    });
                }

                FluxoDeBotao();

                NPC.IniciaConversa();

                OnStartTalk();

                Transform player = AbstractGlobalController.Instance.Players[0].Manager.transform;
                Vector3 dir = transform.parent.position - player.position;
                CharRotateTo.RotateDir(dir, player.gameObject);
                CharRotateTo.RotateDir(-dir, transform.parent.gameObject);
            }
        }

        protected virtual void OnStartTalk()
        {
            MessageAgregator<MsgStartExternalInteraction>.Publish();
        }

        public override void FuncaoDoBotao()
        {
            BotaoConversa();
        }

        public override void SomDoIniciar()
        {
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = SoundEffectID.Decision1
            });
        }
    }    
}

public struct MsgSendExternalPanelCommand : IMessageBase
{
    public bool confirmButton;
    public bool returnButton;
    public bool extraButton;
    public bool leftChangeButton;
    public bool rightChangeButton;
    public bool pauseMenu;
    public int hChange;
    public int vChange;   
    public int leftTrigger;
    public int rightTrigger;
}
public struct MsgStartExternalInteraction : IMessageBase { }
public struct MsgFinishExternalInteraction : IMessageBase { }