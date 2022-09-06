using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class DamageCollider : DamageColliderBase
    {
        Vector3 forwardInicial;

        void Start()
        {
            forwardInicial = transform.forward;
            QuaternionDeImpacto();

        }

        // Update is called once per frame
        void Update()
        {

            transform.position += velocidadeProjetil * forwardInicial * Time.deltaTime;
        }

        void OnTriggerEnter(Collider emQ)
        {
            //Debug.Log(emQ.name + " : " + emQ.gameObject.name + " : " + dono+"trigger");

            FuncaoTrigger(emQ);
        }
    }

}