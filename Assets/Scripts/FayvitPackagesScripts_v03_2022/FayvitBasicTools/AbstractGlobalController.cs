using FayvitCommandReader;
using FayvitMessageAgregator;
using FayvitSounds;
using FayvitUI;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitBasicTools
{
    public abstract class AbstractGlobalController : MonoBehaviour,IGlobalController
    {
        [SerializeField] private ConfirmationPanel confirmP;
        [SerializeField] private SingleMessagePanel singleP;
        [SerializeField] private MyFadeView myFade;
        [SerializeField] private SoundEffectsManager sfx;
        [SerializeField] private MusicManager music;

        private static AbstractGlobalController instance;

        public static IGlobalController Instance => instance;

        public List<IPlayersInGameDb> Players { get; set; }

        public Controlador Control { get; private set; } = Controlador.teclado;

        public ConfirmationPanel Confirmation => confirmP;

        public SingleMessagePanel OneMessage => singleP;

        public IFadeView FadeV => myFade;

        public IMusicManager Music { get => music; }

        public float MusicVolume { get => music.BaseVolume; set => music.BaseVolume=value; }
        public float SfxVolume { get => sfx.BaseVolume; set => sfx.BaseVolume=value; }
        public bool EmTeste { get; set; }

        protected SoundEffectsManager Sfx => sfx;

        public Vector3 InitialGamePosition => new Vector3(0, 0, 0);

        protected virtual void Awake()
        {
            AbstractGlobalController[] gg = FindObjectsOfType<AbstractGlobalController>();

            if (gg.Length > 1)
            {
                Destroy(gameObject);
                return;
            }


            transform.parent = null;

            DontDestroyOnLoad(gameObject);

            instance = this;

        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            if (Players == null)
                Players = new List<IPlayersInGameDb>();

            MessageAgregator<MsgChangeMusicIfNew>.AddListener(OnRequestChangeMusicIfNew);            
            MessageAgregator<MsgRequestSfx>.AddListener(OnRequestSfx);
            MessageAgregator<MsgStartMusic>.AddListener(OnRequestStartMusic);
            MessageAgregator<MsgStopMusic>.AddListener(OnRequestStopMusic);
            MessageAgregator<MsgStartMusicWithRecovery>.AddListener(OnRequestStartMusicWithRecovery);
            MessageAgregator<MsgReturnRememberedMusic>.AddListener(OnRequestReturnRememberedMusic);
            MessageAgregator<MsgRequest3dSound>.AddListener(OnRequest3dSound);
            MessageAgregator<MsgRequestFadeIn>.AddListener(OnRequestFadeIn);
            MessageAgregator<MsgRequestFadeOut>.AddListener(OnRequestFadeOut);

        }

        protected virtual void OnDestroy()
        {
            MessageAgregator<MsgChangeMusicIfNew>.RemoveListener(OnRequestChangeMusicIfNew);
            MessageAgregator<MsgRequestSfx>.RemoveListener(OnRequestSfx);
            MessageAgregator<MsgStartMusic>.RemoveListener(OnRequestStartMusic);
            MessageAgregator<MsgStopMusic>.RemoveListener(OnRequestStopMusic);
            MessageAgregator<MsgStartMusicWithRecovery>.RemoveListener(OnRequestStartMusicWithRecovery);
            MessageAgregator<MsgReturnRememberedMusic>.RemoveListener(OnRequestReturnRememberedMusic);
            MessageAgregator<MsgRequest3dSound>.RemoveListener(OnRequest3dSound);
            MessageAgregator<MsgRequestFadeIn>.RemoveListener(OnRequestFadeIn);
            MessageAgregator<MsgRequestFadeOut>.RemoveListener(OnRequestFadeOut);

        }

        private void OnRequestChangeMusicIfNew(MsgChangeMusicIfNew obj)
        {
            float vol = 1;
            if (obj.changeVolume)
                vol = obj.volumeVal;

            if (obj.nmcvc != null)
                music.StartMusicIf(obj.nmcvc);
            else if (obj.clip != null)
                music.StartMusicIf(obj.clip, vol);
            else if (obj.nameMusic != NameMusic.empty)
                music.StartMusicIf(obj.nameMusic, vol);
        }

        private void OnRequestFadeOut(MsgRequestFadeOut obj)
        {
            myFade.StartFadeOut(obj.color, obj.darkenTime);
        }

        private void OnRequestFadeIn(MsgRequestFadeIn obj)
        {
            myFade.StartFadeIn(obj.color, obj.lightenTime);
        }

        private void OnRequest3dSound(MsgRequest3dSound obj)
        {
            float spartial = .75f;
            if (obj.spartialVal!=default)
                spartial = obj.spartialVal;

            if (obj.clip != null)
                sfx.Instantiate3dSound(obj.sender, obj.clip, spartial);
            else if (obj.sfxId!=SoundEffectID.empty)
                sfx.Instantiate3dSound(obj.sender,obj.sfxId,spartial);
        }

        private void OnRequestReturnRememberedMusic(MsgReturnRememberedMusic obj)
        {
            if (!obj.changeVel)
                music.StartRememberedMusic();
            else
                music.StartRememberedMusic(obj.velVal);
        }

        private void OnRequestStartMusicWithRecovery(MsgStartMusicWithRecovery obj)
        {
            if (obj.changeVel)
                music.StartMusicRememberingCurrent(obj.nmcvc, obj.velVal);
            else
                music.StartMusicRememberingCurrent(obj.nmcvc);
        }

        private void OnRequestStopMusic(MsgStopMusic obj)
        {
            float vel = 0.25f;

            if (obj.velOfStopMusic > 0)
                vel = obj.velOfStopMusic;

            music.StopMusic(vel);
        }

        private void OnRequestStartMusic(MsgStartMusic obj)
        {
            float vol = 1;
            if (obj.changeVolume)
                vol = obj.volumeVal;

            if (obj.nmcvc != null)
                music.StartMusic(obj.nmcvc);
            else if(obj.clip!=null)
                music.StartMusic(obj.clip, vol);
            else if (obj.nameMusic != NameMusic.empty)
                music.StartMusic(obj.nameMusic, vol);

        }

        private void OnRequestSfx(MsgRequestSfx obj)
        {
            if (obj.clip!=null)
            {
                sfx.PlaySfx(obj.clip);
            }
            else if (!string.IsNullOrEmpty(obj.sfxName))
            {
                sfx.PlaySfx(obj.sfxName);
            }
            else
            if (obj.sfxId!=SoundEffectID.empty)
            {
                sfx.PlaySfx(obj.sfxId);
            }
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            Controlador antControl = Control;
            Control = ChangeController.ControllerInUse(Control);
            if (antControl != Control)
            {
                MessageAgregator<MsgChangeHardwareControler>.Publish(new MsgChangeHardwareControler()
                {
                    newControler = Control
                });
            }
            music.Update();
        }
    }

    public struct MsgRequestSfx : IMessageBase
    {
        public AudioClip clip;
        public SoundEffectID sfxId;
        public string sfxName;
    }
    public struct MsgChangeMusicIfNew : IMessageBase
    {
        public bool changeVolume;
        public float volumeVal;
        public NameMusic nameMusic;
        public AudioClip clip;
        public NameMusicaComVolumeConfig nmcvc;
    }
    public struct MsgStartMusic:IMessageBase
    {
        public bool changeVolume;
        public float volumeVal;
        public NameMusic nameMusic;
        public AudioClip clip;
        public NameMusicaComVolumeConfig nmcvc;
    }
    public struct MsgStopMusic : IMessageBase
    {
        public float velOfStopMusic;
    }
    public struct MsgStartMusicWithRecovery : IMessageBase
    {
        public bool changeVel;
        public float velVal;
        public NameMusicaComVolumeConfig nmcvc;
    }
    public struct MsgReturnRememberedMusic : IMessageBase 
    {
        public bool changeVel;
        public float velVal;
    }
    public struct MsgRequest3dSound : IMessageBase
    {
        public AudioClip clip;
        public SoundEffectID sfxId;
        public Transform sender;     
        public float spartialVal;
    }

    public struct MsgRequestFadeOut : IMessageBase
    {
        public float darkenTime;
        public Color color;
    }

    public struct MsgRequestFadeIn : IMessageBase
    {
        public float lightenTime;
        public Color color;
    }
}