using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitBasicTools;

namespace Criatures2021
{
    public class DamageColliderDirectional : DamageColliderBase
    {
        public GameObject alvo;

        private Vector3 dir;
        // Use this for initialization
        void Start()
        {
            Transform T = FindBestTarget.Procure(gameObject, new string[1] { "Criature" });
            if (T)
                alvo = T.gameObject;

            dir = dono.transform.forward;
            QuaternionDeImpacto();
        }

        // Update is called once per frame
        void Update()
        {
            if (alvo)
            {
                float sinal = 1;
                if (Vector3.Angle(dir, alvo.transform.position - transform.position) > 100)
                    sinal = -1;
                dir = Vector3.Slerp(dir, sinal * (alvo.transform.position - transform.position), 2.9f * Time.deltaTime);
                dir = new Vector3(dir.x, 0, dir.z);
                dir.Normalize();
                /*
                Vector3 dir = alvo.transform.position-dono.transform.position;
                dir = Vector3.L(transform.forward,new Vector3(dir.x,0,dir.z)).normalized;*/
                transform.position += velocidadeProjetil * dir * Time.deltaTime;

            }
            else
                transform.position += velocidadeProjetil * transform.forward * Time.deltaTime;
        }

        void OnTriggerEnter(Collider emQ)
        {
            FuncaoTrigger(emQ);
        }
    }
}