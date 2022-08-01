using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class ParticleOfSubstitution
    {

        public static GameObject InsereParticulaDaLuva(Transform meuHeroi, bool bracoDireito = true)
        {
            Vector3 instancia = SetarInstancia(meuHeroi, bracoDireito);

            GameObject luz = Resources.Load<GameObject>("particles/"+GeneralParticles.particulaLuvaDeGuarde); //GameController.g.El.retorna(DoJogo.particulaLuvaDeGuarde);
            luz = Object.Instantiate(luz, instancia, Quaternion.identity);

            luz.name = "luz";
            return luz;
        }


        public static GameObject InsereParticulaDoRaio(Vector3 posCriature, Transform dono, bool bracoDireito = true)
        {

            Vector3 instancia = SetarInstancia(dono, bracoDireito);
            return InsereParticulaDoRaio(instancia, posCriature);
        }

        public static GameObject InsereParticulaDoRaio(Vector3 posSaida, Vector3 posChegada/*, bool bracoDireito = true*/)
        {
            GameObject raio = Resources.Load<GameObject>("particles/" + GeneralParticles.raioDeLuvaDeGuarde); 
            //GameController.g.El.retorna(DoJogo.raioDeLuvaDeGuarde);

            raio = Object.Instantiate(raio, posSaida, Quaternion.LookRotation(
             posChegada - posSaida
            ));
            raio.name = "raio";
            ParticleSystem P = raio.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = P.main;
            main.startSpeed = main.startSpeed.constant*(posChegada - posSaida).magnitude * 2;
            return raio;
        }

        public static void ParticulaSaiDaLuva(Vector3 X, GeneralParticles oQ = GeneralParticles.replaceParticles)
        {
            GameObject volte = Resources.Load<GameObject>("particles/" + oQ);
            //GameController.g.El.retorna(oQ);
            volte = Object.Instantiate(volte, X, Quaternion.identity) as GameObject;


            //volte.GetComponent<ParticleSystem>().GetComponent<Renderer>().material
            //    = GameController.g.El.materiais[0];
            //volte.GetComponent<ParticleSystem>().startColor = new Color(1, 0.64f, 0, 1);

            Object.Destroy(volte, 2);
        }

        public static void ReduzVelocidadeDoRaio(GameObject raio)
        {
            ParticleSystem P = raio.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = P.main;
            main.startSpeed = Mathf.Lerp(main.startSpeed.constant, 0, 10 * Time.deltaTime);
            main.startLifetime = Mathf.Lerp(main.startLifetime.constant, 0, 10 * Time.deltaTime);
        }

        static Vector3 SetarInstancia(Transform meuHeroi, bool direito)
        {
            string nomeEsqueleto = "metarig";


            //Debug.Log(meuHeroi.Find(nomeEsqueleto + "/hips/spine/chest/shoulder.R"));
            if (direito)
                return meuHeroi
                    .Find(nomeEsqueleto + "/hips/spine/chest/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.02.R")
                        .transform.position;
            else
                return meuHeroi
                .Find(nomeEsqueleto + "/hips/spine/chest/shoulder.L/upper_arm.L/forearm.L/hand.L/palm.02.L")
                    .transform.position;

        }
    }
}
