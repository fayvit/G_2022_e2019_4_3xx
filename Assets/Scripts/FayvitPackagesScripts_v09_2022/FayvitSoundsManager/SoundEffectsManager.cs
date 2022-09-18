using UnityEngine;
using System.Collections.Generic;

namespace FayvitSounds
{
    [System.Serializable]
    public class SoundEffectsManager : ISoundEffectsManager
    {

#pragma warning disable 0649
        [SerializeField] private AudioSource[] audios;
        [SerializeField] private AudioSource[] sources3d;
#pragma warning restore 0649
        private List<AudioSource> ativos = new List<AudioSource>();

        public float BaseVolume { get; set; } = 0.95f;

        public void Instantiate3dSound(Transform T, AudioClip som, float spartial = 1)
        {
            Instantiate3dSound(T.position, som, spartial);
        }

        public void Instantiate3dSound(Vector3 V, AudioClip som, float spartial = 1)
        {
            AudioSource a = RetornaMelhorCandidato(sources3d);
            a.clip = som;
            a.volume = BaseVolume;
            a.spatialBlend = spartial;
            a.transform.position = V;
            a.Play();
            //GameObject G = new GameObject();
            //G.name = "3d sound for :" + som;
            //AudioSource a = //(AudioSource)MonoBehaviour.Instantiate(audios[0], T.position, Quaternion.identity);
            //G.AddComponent<AudioSource>();
            //G.transform.position = V;
            ////MonoBehaviour.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), a.transform.position, Quaternion.identity);
            //a.clip = som;
            //a.volume = BaseVolume;
            //a.spatialBlend = spartial;
            //a.enabled = true;
            //a.Play();
            

            ////Debug.Log(a.clip.length + " : " + a.clip.name+" : "+a.clip.length+" : "+a.gameObject.name);

            //MonoBehaviour.Destroy(G, 2 * a.clip.length);
        }

        public void Instantiate3dSound(Transform T, SoundEffectID som, float spartial = 1)
        {
            Instantiate3dSound(T, (AudioClip)Resources.Load(som.ToString()), spartial);
        }

        public void PlaySfx(SoundEffectID s)
        {
            PlaySfx(s.ToString());
        }

        public void PlaySfx(string s)
        {
            PlaySfx((AudioClip)Resources.Load(s));
        }

        public void PlaySfx(AudioClip s)
        {
            AudioSource a = RetornaMelhorCandidato(audios);

            a.clip = s;
            a.volume = BaseVolume;
            a.Play();
        }

        AudioSource RetornaMelhorCandidato(AudioSource[] audios)
        {
            VerificaAudioAtivo(audios);
            for (int i = 0; i < audios.Length; i++)
            {
                if (!ativos.Contains(audios[i]))
                {
                    ativos.Add(audios[i]);
                    return audios[i];
                }
            }

            return ativos[0];
        }

        void VerificaAudioAtivo(AudioSource[] audios)
        {
            for (int i = 0; i < audios.Length; i++)
            {
                if (!audios[i].isPlaying)
                {
                    ativos.Remove(audios[i]);
                }
            }
        }
    }
}