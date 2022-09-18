using UnityEngine;
using System.Collections;

namespace FayvitSounds
{
    public interface ISoundEffectsManager
    {
        float BaseVolume { get; set; }
        void Instantiate3dSound(Transform T, AudioClip som, float spartial = 1);
        void Instantiate3dSound(Transform T, SoundEffectID som, float spartial = 1);
        void PlaySfx(SoundEffectID s);
        void PlaySfx(string s);
        void PlaySfx(AudioClip s);

    }

    public enum SoundEffectID
    {
        empty = 0,
        Evasion1 = 1,
        XP_Knock04 = 2,
        XP_Swing03 = 3,
        Wind1 = 4,
        XP_Swing04 = 5,
        rajadaDeAgua = 6,
        Shot1 = 7,
        Slash2 = 8,
        Slash1 = 9,
        Shot3 = 10,
        VinhetinhaCurta=11,
        VinhetinhaCompletaComFim=12,
        vinhetinhaMedia=13,
        painelAbrindo = 14,
        paraBau = 15,
        Cursor1 = 16,
        Book1 = 17,
        Decision2 = 18,
        tuin_1ponto3=19,
        tuimParaNivel=20,
        Decision1 = 21,
        Twine = 22,
        Thunder12 = 23,
        Attack2 = 24,
        Collapse1 = 25,
        XP_Earth02 = 26,
        XP_Ice03 = 27,
        XP_Heal02 = 28,
        XP_Heal01 = 29,
        XP_Heal07 = 30,
        Darkness8 = 31,
        encontro = 32,
        Monster3 = 33,
        Shop = 34,
        XP010_System10 = 35,
        keyDjeySound = 36,
        Fire6 = 37,
        finalisaAcaoPositiva = 38,
        ItemImportante = 39
    }
}
