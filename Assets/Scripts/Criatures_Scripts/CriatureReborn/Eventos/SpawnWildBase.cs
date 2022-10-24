using System.Collections.Generic;
using UnityEngine;

namespace Criatures2021
{
    public class SpawnWildBase : MonoBehaviour
    {
        [SerializeField,ArrayElementTitle("pet")] private SpawnPercent[] spawnables;
        [SerializeField] private int minSpawn = 3;
        [SerializeField] private int maxSpawn = 7;
        [SerializeField] private float spawnOrigimDistance = 5;

        [System.Serializable]
        private class SpawnPercent
        {
            public float tax = 1;
            public PetName pet;
            public int levelMin = 1;
            public int levelMax = 2;
        }

        protected List<PetManager> SpawnPets()
        {
            int spawns = Random.Range(minSpawn, maxSpawn);
            List<PetManager> spawnados = new List<PetManager>();
            for (int i = 0; i < spawns; i++)
            {
                //FayvitSupportSingleton.SupportSingleton.Instance.InvokeOnCountFrame(() =>
                //{
                    float maxSort = SomarTaxas();
                    float sort = Random.Range(0, maxSort);
                    PetBase P = GetSpawn(sort);
                    Vector3 pos = Random.insideUnitCircle;
                    pos = new Vector3(pos.x, 0, pos.y) * Random.Range(0.1f * spawnOrigimDistance, spawnOrigimDistance) + transform.position;
                    spawnados.Add(WildPetInitialize.Initialize(P, pos));
                //}, (uint)(90 * i));
            }

            return spawnados;
        }

        PetBase GetSpawn(float sort)
        {
            float sum = 0;
            for (int i = 0; i < spawnables.Length; i++)
            {
                sum += spawnables[i].tax;
                if (sort <= sum)
                {
                    int nivel = Random.Range(spawnables[i].levelMin, spawnables[i].levelMax);
                    return new PetBase(spawnables[i].pet, nivel);
                }
            }

            return null;
        }

        float SomarTaxas()
        {
            float r = 0;
            for (int i = 0; i < spawnables.Length; i++)
            {
                r += spawnables[i].tax;
            }

            return r;
        }
    }
}