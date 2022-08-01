using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Criatures2021;

public class SpawnPetOnLoad : MonoBehaviour
{
    [SerializeField] private SpawnPercent[] spawnables;
    [SerializeField] private int minSpawn = 1;
    [SerializeField] private int maxSpawn = 2;
    [SerializeField] private float spawnDistance=5;

    [System.Serializable]
    private class SpawnPercent
    {
        public float tax = 1;
        public PetName pet;
        public int levelMin=1;
        public int levelMax=2;
    }

    // Start is called before the first frame update
    void Start()
    {
        int spawns = Random.Range(minSpawn, maxSpawn);

        for (int i = 0; i < spawns; i++)
        {
            float maxSort = SomarTaxas();
            float sort = Random.Range(0, maxSort);
            PetBase P = GetSpawn(sort);
            Vector3 pos = Random.insideUnitCircle;
            pos = new Vector3(pos.x, 0, pos.y) * Random.Range(0.1f * spawnDistance, spawnDistance) + transform.position;
            WildPetInitialize.Initialize(P, pos);
        }

        Destroy(gameObject);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
