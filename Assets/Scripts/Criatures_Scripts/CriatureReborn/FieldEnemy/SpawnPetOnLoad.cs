using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Criatures2021;

public class SpawnPetOnLoad : SpawnWildBase
{
    //[SerializeField] private SpawnPercent[] spawnables;
    //[SerializeField] private int minSpawn = 1;
    //[SerializeField] private int maxSpawn = 2;
    

    //[System.Serializable]
    //private class SpawnPercent
    //{
    //    public float tax = 1;
    //    public PetName pet;
    //    public int levelMin=1;
    //    public int levelMax=2;
    //}

    // Start is called before the first frame update
    void Start()
    {

        SpawnPets();
        Destroy(gameObject);
    }

    

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
