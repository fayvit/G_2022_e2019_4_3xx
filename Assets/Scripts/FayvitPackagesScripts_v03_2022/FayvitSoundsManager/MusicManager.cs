using UnityEngine;

namespace FayvitSounds
{
    [System.Serializable]
    public class MusicManager : IMusicManager
    {
#pragma warning disable 0649
        [SerializeField] private AudioSource[] audios;
#pragma warning restore 0649
        private int inicia = -1;
        private int termina = -1;

        //private string cenaIniciada = "";
        private bool parando;
        private float volumeAlvo = 0.45f;
        private float volumeBase = 0.5f;
        private const float VELOCIDADE_DE_MUDANCA = 0.25f;

        public MusicaComVolumeConfig ToRememberMusic { get; private set; }

        public MusicaComVolumeConfig CurrentActiveMusic { get; private set; } = new MusicaComVolumeConfig();

        public float ActiveVel { get; set; } = 0.25f;

        public float BaseVolume
        {
            get { return volumeBase; }
            set
            {
                float voltransitorio = volumeAlvo / volumeBase;
                volumeBase = value;
                volumeAlvo = volumeBase * voltransitorio;
                for (int i = 0; i < audios.Length; i++)
                {
                    if (CurrentActiveMusic != null)
                        audios[i].volume = CurrentActiveMusic.Volume * volumeBase;
                }
            }
        }

        public void ResetActiveVel()
        {
            ActiveVel = VELOCIDADE_DE_MUDANCA;
        }

        //public void IniciarMusicaDaCena(DadosDeCena d)
        //{
        //    //DadosDeCena d = SceneDates.GetCurrentSceneDates();

        //    if (d != null)
        //    {
        //        IniciarMusica(d.musicName);
        //    }
        //}

        public void StartRememberedMusic(float vel = -1)
        {
            if (vel <= 0)
                vel = VELOCIDADE_DE_MUDANCA;

            ActiveVel = vel;

            if (ToRememberMusic != null)
            {
                StartMusic(ToRememberMusic.Musica, ToRememberMusic.Volume);
            }
            else
                RestartMusic();
        }

        public void StartMusicRememberingCurrent(AudioClip esseClip, float volumeAlvo = 1, float vel = -1)
        {
            ToRememberMusic = CurrentActiveMusic;
            StartMusic(esseClip, volumeAlvo, vel);
        }

        public void StartMusicRememberingCurrent(NameMusicaComVolumeConfig n, float vel = -1)
        {
            StartMusicRememberingCurrent(n.Musica, n.Volume, vel);
        }

        public void StartMusicRememberingCurrent(MusicaComVolumeConfig n, float vel = -1)
        {
            StartMusicRememberingCurrent(n.Musica, n.Volume);
        }

        public void StartMusicRememberingCurrent(NameMusic esseClip, float volumeAlvo = 1, float vel = -1)
        {
            StartMusicRememberingCurrent(esseClip.ToString(), volumeAlvo); ;
        }

        public void StartMusicRememberingCurrent(string esseClip, float volumeAlvo = 1, float vel = -1)
        {
            StartMusicRememberingCurrent((AudioClip)Resources.Load(esseClip), volumeAlvo);
        }

        public void StartMusicIf(AudioClip esseClip, float volumeAlvo = 1, float vel = -1)
        {
            if(CurrentActiveMusic!=null && CurrentActiveMusic.Musica!=esseClip)
                StartMusic(esseClip, volumeAlvo, vel);
        }

        public void StartMusicIf(NameMusicaComVolumeConfig esseClip, float vel = -1)
        {
            StartMusicIf((AudioClip)Resources.Load(esseClip.Musica.ToString()), esseClip.Volume, vel);
        }

        public void StartMusicIf(NameMusic esseClip, float volumeAlvo = 1, float vel = -1)
        {
            StartMusicIf((AudioClip)Resources.Load(esseClip.ToString()), volumeAlvo, vel);
        }

        public void StartMusic(NameMusicaComVolumeConfig esseClip, float vel = -1)
        {
            StartMusic((AudioClip)Resources.Load(esseClip.Musica.ToString()), esseClip.Volume, vel);
        }

        public void StartMusic(NameMusic esseClip, float volumeAlvo = 1, float vel = -1)
        {
            StartMusic((AudioClip)Resources.Load(esseClip.ToString()), volumeAlvo, vel);
        }

        public void StartMusic(AudioClip esseClip, float volumeAlvo = 1, float vel = -1)
        {
            if (vel <= 0)
            {
                vel = VELOCIDADE_DE_MUDANCA;
            }

            ActiveVel = vel;

            CurrentActiveMusic = new MusicaComVolumeConfig()
            {
                Musica = esseClip,
                Volume = volumeAlvo
            };

            parando = false;
            this.volumeAlvo = volumeAlvo * BaseVolume;
            AudioSource au = audios[0];

            if (au.isPlaying)
            {
                termina = 0;
                inicia = 1;
            }
            else
            {
                termina = 1;
                inicia = 0;
            }

            if (audios[termina].clip == esseClip)
            {
                int temp = inicia;
                inicia = termina;
                termina = temp;
            }
            else
            {
                audios[inicia].volume = 0;
                audios[inicia].clip = esseClip;
                audios[inicia].Play();
            }

        }

        public void StopMusic(float vel = -1)
        {
            if (vel <= 0)
                vel = VELOCIDADE_DE_MUDANCA;

            ActiveVel = vel;
            parando = true;
        }

        /*
        public void PararMusicas()
        {
            parando = true;
        }*/

        public void RestartMusic(bool doZero = false)
        {
            parando = false;

            if (doZero)
            {
                audios[inicia].Stop();
                audios[inicia].Play();
            }
        }

        public void Update()
        {
            //Debug.Log(audios.Length + " : " + parando);
            if (audios.Length > 0)
            {
                if (!parando)
                {
                    if (inicia != -1 && termina != -1)
                    {


                        if (audios[inicia].volume < 0.9f * volumeAlvo)
                            audios[inicia].volume = Mathf.Lerp(audios[inicia].volume, volumeAlvo, Time.fixedDeltaTime * ActiveVel);
                        else
                            audios[inicia].volume = volumeAlvo;

                        if (audios[termina].volume < 0.2f)
                        {
                            audios[termina].volume = 0;
                            audios[termina].Stop();
                        }
                        else
                            audios[termina].volume = Mathf.Lerp(audios[termina].volume, 0, Time.fixedDeltaTime * 3 * ActiveVel);

                    }
                }
                else
                {
                    if (termina != -1)
                        audios[termina].volume = Mathf.Lerp(audios[termina].volume, 0, Time.fixedDeltaTime * 2 * ActiveVel);

                    if (inicia != -1)
                        audios[inicia].volume = Mathf.Lerp(audios[inicia].volume, 0, Time.fixedDeltaTime * 2 * ActiveVel);
                }


            }
        }

        //void MudaPara(string clip, float volume = 1)
        //{
        //    volumeAlvo = volume;
        //    StartMusic((AudioClip)Resources.Load(clip));
        //    cenaIniciada = SceneManager.GetActiveScene().name;
        //}

        public void Start()
        {
            Debug.Log("Adicionar musicas iniciais");
            /*
            if (SceneManager.GetActiveScene().name == "Inicial 1")
                IniciarMusica((AudioClip)Resources.Load(NameMusic.Field2.ToString()));*/
        }
    }
}