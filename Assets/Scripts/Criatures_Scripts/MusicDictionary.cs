using FayvitLoadScene;
using FayvitSounds;
using System.Collections.Generic;

namespace Criatures2021
{
    public class MusicDictionary
    {
        public static Dictionary<NomesCenas, NameMusicaComVolumeConfig> musicDict = new Dictionary<NomesCenas, NameMusicaComVolumeConfig>()
        {
            { NomesCenas.nula,new NameMusicaComVolumeConfig()
            {
                Musica = NameMusic.choroChorandoParaPaulinhoNogueira
            } },
            { 
            NomesCenas.cavernaAcampamentoKatids, new NameMusicaComVolumeConfig()
            { 
                Musica = NameMusic.NaqueleTempo 
            }
            }
        };

        public static NameMusicaComVolumeConfig GetSceneMusic(NomesCenas n)
        {
            if (musicDict.ContainsKey(n))
                return musicDict[n];
            else
                return musicDict[NomesCenas.nula];  
        }
    }
}