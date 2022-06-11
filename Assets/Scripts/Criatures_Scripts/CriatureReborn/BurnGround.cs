using UnityEngine;
using System.Collections;

namespace Criatures2021
{    
    public class BurnGround : MonoBehaviour
    {
        private float tempoQueimando;

        public float tempoMin = 5;
        public float tempoMax = 10;

        public PetTypeName tipoImune = PetTypeName.Fogo;
        public ImpactParticles noImpacto = ImpactParticles.impactoDeFogo;

        void Start()
        {
            tempoQueimando = Random.Range(5, 10);

            Destroy(gameObject, tempoQueimando);
        }

        void OnTriggerEnter(Collider emQ)
        {
            if (emQ.tag == "Criature" || emQ.tag == "Player")
            {
                bool dano = true;
                bool deFogo = false;
                if (emQ.tag == "Criature")
                {
                    PetTypeName[] Tipos = emQ.transform.GetComponent<PetManager>().MeuCriatureBase.PetFeat.meusTipos;
                    for (int i = 0; i < Tipos.Length; i++)
                    {
                        if (Tipos[i] == tipoImune)
                        {
                            dano = false;
                            deFogo = true;
                        }
                    }
                }
                else
                    dano = false;

                if (!deFogo)
                {
                    GameObject G = Resources.Load<GameObject>("particles/" + noImpacto.ToString());//GameController.g.El.retorna(noImpacto);
                    G = (GameObject)Instantiate(G, emQ.transform.position, Quaternion.identity);

                    Destroy(G, 0.75f);
                }

                if (dano)
                {
                    DamageManager.VerificaDano(emQ.gameObject, emQ.gameObject, new PetAttackBase(new PetAttackFeatures()));
                    //Dano.VerificaDano(emQ.gameObject, emQ.gameObject, new GolpeBase(new ContainerDeCaracteristicasDeGolpe()));
                }

                if (!deFogo)
                    Destroy(gameObject);
            }
        }
    }
}