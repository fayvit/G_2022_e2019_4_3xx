using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Criatures2021
{
    public class AnelDoOlharViewers : MonoBehaviour
    {
        [SerializeField] private GameObject startParticle;
        [SerializeField] private GameObject updateParticle;
        [SerializeField] private float timeToSpawnUpdateParticle;

        private float contadorDeTempo = 0;

        // Start is called before the first frame update
        void Start()
        {
            InstanceSupport.InstanciaLigandoE_Destrua(startParticle, transform.position, transform.rotation, 5);
        }

        // Update is called once per frame
        void Update()
        {
            contadorDeTempo += Time.deltaTime;

            if (contadorDeTempo >= timeToSpawnUpdateParticle)
            {
                InstanceSupport.InstanciaLigandoE_Destrua(updateParticle, transform.position, transform.rotation, 5);

                contadorDeTempo = 0;
            }
        }

       
    }
}