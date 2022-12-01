﻿using Criatures2021.BasicCriatures;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FayvitBasicTools;

namespace Criatures2021
{
    
    public class SpawnWithDistance : SpawnWildBase
    {
        [SerializeField] private float spawnDistance = 30;
        [SerializeField] private float destroyDistance = 40;
        [SerializeField] private List<PetManager> emCampo = new List<PetManager>();
        private CharacterManager manager;
        private bool petsSpawnados;

        // Use this for initialization
        void Start()
        {
           if(StaticInstanceExistence<PoolPets_ddepoisDoBugDaUnity>.SchelduleExistence(Start,this,()=> {
               return PoolPets_ddepoisDoBugDaUnity.instance;
           }))
            InvokeRepeating("VerificarSpawn", 0, Random.Range(2.5f,3.5f));
        }

        void VerificarSpawn()
        {
            manager = MyGlobalController.MainPlayer;
            if (manager)
            {
                if (Vector3.Distance(manager.transform.position, transform.position) < spawnDistance && !petsSpawnados)
                {
                    petsSpawnados = true;
                    emCampo = SpawnPets();
                }
                else if (Vector3.Distance(manager.transform.position, transform.position) > destroyDistance && petsSpawnados)
                {
                    foreach (var v in emCampo.ToList())
                        if (v != null)
                        {
                            PoolPets_ddepoisDoBugDaUnity.instance.DisablePetGO(v.gameObject, v.MeuCriatureBase.NomeID);
                        }

                    emCampo.Clear();
                    petsSpawnados = false;

                }
            }
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}