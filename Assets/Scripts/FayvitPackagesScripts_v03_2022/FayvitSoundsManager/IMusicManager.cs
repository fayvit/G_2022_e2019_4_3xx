using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitSounds
{
    public interface IMusicManager
    {
        MusicaComVolumeConfig ToRememberMusic { get; }

        MusicaComVolumeConfig CurrentActiveMusic { get; }

        float ActiveVel { get; set; }

        float BaseVolume { get; set; }
        void ResetActiveVel();
        void StartRememberedMusic(float vel = -1);
        void StartMusicRememberingCurrent(AudioClip esseClip, float volumeAlvo = 1, float vel = -1);
        void StartMusicRememberingCurrent(NameMusicaComVolumeConfig n, float vel = -1);
        void StartMusicRememberingCurrent(MusicaComVolumeConfig n, float vel = -1);
        void StartMusicRememberingCurrent(NameMusic esseClip, float volumeAlvo = 1, float vel = -1);
        void StartMusicRememberingCurrent(string esseClip, float volumeAlvo = 1, float vel = -1);
        void StartMusic(NameMusicaComVolumeConfig esseClip, float vel = -1);
        void StartMusic(NameMusic esseClip, float volumeAlvo = 1, float vel = -1);
        void StartMusic(AudioClip esseClip, float volumeAlvo = 1, float vel = -1);
        void StopMusic(float vel = -1);
        void RestartMusic(bool doZero = false);
        void Update();
    }

    public enum NameMusic
    {
        empty = 0,
        initialAdventureTheme = 1,
        TicoTicoNoFuba_v1 = 2,
        Noturno_toLoop=3,
        xodoDaBaiana=4,
        Schottish_choro=5,
        TicoTicoNoFuba_v2 = 6,
        AsRosasNaoFalam = 7,
        CaixaDeFosforo = 8,
        Brasileirinho = 9,
        choroChorandoParaPaulinhoNogueira = 10,
        NaqueleTempo = 11,
        choro_N1 = 12,
        Chorrindo = 13
    }

    [System.Serializable]
    public class MusicaComVolumeConfig
    {
        [SerializeField] private AudioClip musica;
        [SerializeField] private float volume = 1;

        public AudioClip Musica
        {
            get { return musica; }
            set { musica = value; }
        }

        public float Volume
        {
            get { return volume; }
            set { volume = value; }
        }
    }

    [System.Serializable]
    public class NameMusicaComVolumeConfig
    {
        [SerializeField] private NameMusic musica = NameMusic.initialAdventureTheme;
        [SerializeField] private float volume = 1;

        public NameMusic Musica
        {
            get { return musica; }
            set { musica = value; }
        }

        public float Volume
        {
            get { return volume; }
            set { volume = value; }
        }
    }
}