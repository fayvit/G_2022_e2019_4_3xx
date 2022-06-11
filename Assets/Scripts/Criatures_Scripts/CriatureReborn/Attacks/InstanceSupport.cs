using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class InstanceSupport
    {
        public static GameObject InstancieEDestrua(AttackNameId nomeGolpe,
                                                    Vector3 posInicial,
                                                    Vector3 forwardInicial,
                                                    float tempoDeGolpe)
        {
            return InstancieEDestrua("Attacks/" + nomeGolpe.ToString(), posInicial, forwardInicial, tempoDeGolpe);
        }

        public static GameObject InstancieEDestrua(GeneralParticles nomeParticles,
                                                    Vector3 posInicial,
                                                    float tempoDeGolpe,
                                                    Vector3 forwardInicial=default
                                                    )
        {
            if (forwardInicial == default)
                forwardInicial = Vector3.forward;

            return InstancieEDestrua("particles/" + nomeParticles.ToString(), posInicial, forwardInicial, tempoDeGolpe);
        }

        private static GameObject InstancieEDestrua(string nome,
                                                    Vector3 posInicial,
                                                    Vector3 forwardInicial,
                                                    float tempoDeGolpe)
        {
            GameObject golpeX = Resources.Load<GameObject>(nome);

            golpeX = Object.Instantiate(golpeX, posInicial, Quaternion.LookRotation(forwardInicial));

            Object.Destroy(golpeX, tempoDeGolpe);

            return golpeX;
        }

        public static GameObject InstanciaLigandoE_Destrua(GameObject G, Vector3 position, Quaternion rotation, float tempoDaDestruicao)
        {
            GameObject Gg = InstancieLigando(G, position, rotation);
            MonoBehaviour.Destroy(Gg, tempoDaDestruicao);
            return Gg;
        }

        public static GameObject InstancieLigando(GameObject G, Vector3 position, Quaternion rotation)
        {
            GameObject Gg = MonoBehaviour.Instantiate(G, position, rotation);
            Gg.SetActive(true);
            return Gg;
        }

        //private static GameObject InstancieEDestrua(string nome, Vector3 posInicial, float tempoDeGolpe)
        //{
        //    return InstancieEDestrua(nome, posInicial, Vector3.forward, tempoDeGolpe);
        //}

        ////public static GameObject InstancieEDestrua(DoJogo nome, Vector3 posInicial, float tempoDeGolpe)
        ////{
        ////    return InstancieEDestrua(nome.ToString(), posInicial, tempoDeGolpe);
        ////}

        ////public static GameObject InstancieEDestrua(DoJogo nome, Vector3 posInicial, Vector3 forwardInicial, float tempoDeGolpe)
        ////{
        ////    return InstancieEDestrua(nome.ToString(), posInicial, forwardInicial, tempoDeGolpe);
        ////}
    }
}