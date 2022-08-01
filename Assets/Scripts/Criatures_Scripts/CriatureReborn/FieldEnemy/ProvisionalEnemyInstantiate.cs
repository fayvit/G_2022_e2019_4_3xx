using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Criatures2021
{
    public class ProvisionalEnemyInstantiate : MonoBehaviour
    {
        [SerializeField] private List<PetManager> emCampo = new List<PetManager>();
        [SerializeField] private float timeToSpawn = 7.5f;
        public int maxCriatures = 5;
        public Vector2 xVertePosition = new Vector2(450, 470);
        public Vector2 zVertePosition = new Vector2(510, 530);

        private float tempoDecorrido = 0;

        // Use this for initialization
        void Start()
        {
            for(int i=0;i<maxCriatures;i++)
                Spawn();
        }

        void Spawn()
        {
            List<PetName> opcoes = new List<PetName>()
                {
                    PetName.Aladegg,
                    PetName.Arpia,
                    PetName.Babaucu,
                    PetName.Batler,
                    PetName.Florest,
                    PetName.Iruin,
                    PetName.Marak,
                    PetName.PolyCharm,
                    PetName.Urkan,
                    PetName.Xuash,
                    PetName.Flam,
                    PetName.Escorpion,
                    PetName.Steal,
                    PetName.DogMour,
                    PetName.Cabecu,
                    PetName.Rocketler,
                    PetName.Serpente,
                    PetName.Nessei,
                    PetName.Abutre,
                    PetName.Estrep,
                    PetName.Baratarab,
                    PetName.Cracler,
                    PetName.Fajin,
                    PetName.FelixCat,
                    PetName.Vampire,
                    PetName.Wisks,
                    PetName.Izicuolo
                };
            Vector3 V = new Vector3(
                    Random.Range(xVertePosition.x, xVertePosition.y),
                    1,
                    Random.Range(zVertePosition.x, zVertePosition.y)
                    );
            int x = Random.Range(0, opcoes.Count);
            //emCampo.Add(WildPetInitialize.Initialize(PetName.Rocketler, 11, V));
            emCampo.Add(WildPetInitialize.Initialize(opcoes[x], 11, V));

            #region umTesteEspecifico
            //emCampo.Add(WildPetInitialize.Initialize(PetName.Babaucu, 5, V));
            //V = new Vector3(
            //        Random.Range(450, 470),
            //        1,
            //        Random.Range(510, 530)
            //        );
            //emCampo.Add(WildPetInitialize.Initialize(PetName.Marak, 10, V));
            //V = new Vector3(
            //        Random.Range(450, 470),
            //        1,
            //        Random.Range(510, 530)
            //        );
            //emCampo.Add(WildPetInitialize.Initialize(PetName.Onarac, 15, V));
            //V = new Vector3(
            //        Random.Range(450, 470),
            //        1,
            //        Random.Range(510, 530)
            //        );
            //emCampo.Add(WildPetInitialize.Initialize(PetName.Wisks, 20, V));
            #endregion
        }

        // Update is called once per frame
        void Update()
        {
            tempoDecorrido += Time.deltaTime;
            if (tempoDecorrido > timeToSpawn)
            {
                List<int> retirar = new List<int>();
                for (int i = 0; i < emCampo.Count; i++)
                {
                    if (emCampo[i] == null)
                        retirar.Add(i);
                }

                for (int i = retirar.Count; i > 0; i--)
                    emCampo.RemoveAt(retirar[i - 1]);

                if ( emCampo.Count < maxCriatures)
                {
                    Spawn();
                }

                tempoDecorrido = 0;
            }
        }
    }
}