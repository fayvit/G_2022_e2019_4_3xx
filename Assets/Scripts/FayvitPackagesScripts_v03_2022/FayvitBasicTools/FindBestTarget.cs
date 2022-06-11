using FayvitCam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitBasicTools
{
    public class FindBestTarget : MonoBehaviour
    {
        public static List<GameObject> MaisProximosQue(Transform origim,List<GameObject> encontraveis, float distance = 40)
        {
            List<GameObject> inimigosPerto = new List<GameObject>();

            foreach (GameObject encontravel in encontraveis)
            {
                if (encontravel != origim.gameObject)
                {
                    Vector3 c = encontravel.transform.position;
                    c = c - origim.position;



                    if (c.sqrMagnitude < Mathf.Pow(distance, 2))
                        inimigosPerto.Add(encontravel);
                }
            }

            return inimigosPerto;

        }
        public static Transform InDirectionOfBase(Transform origim, Vector3 direction, Transform baseT, string[] tags, float distancia = 40)
        {
            Transform retorno = baseT;

            List<GameObject> inimigosPerto = MaisProximosQue(origim, EncontraveisComTag(tags), distancia);
            if (inimigosPerto.Count > 0)
            {
                GameObject oMelhor=null;
                Vector3 camPlaneForward = DirectionOnThePlane.NormalizedInTheUp(Camera.main.transform.forward);

                foreach (var inimigo in inimigosPerto)
                {
                    Vector3 V = inimigo.transform.position - baseT.position;
                    Vector3 inCamDirection = DirectionOnThePlane.NormalizedInTheUp(inimigo.transform.position - Camera.main.transform.position);
                    
                    if (Vector3.Dot(V.normalized, direction.normalized) > 0 && Vector3.Dot(camPlaneForward,inCamDirection)>.1f)
                        if (oMelhor == null)
                        {
                            oMelhor = inimigo;
                        }
                        else
                        {
                            if (Vector3.SqrMagnitude(V) < Vector3.SqrMagnitude(oMelhor.transform.position - baseT.position))
                                oMelhor = inimigo;
                        }
                    
                }

                retorno = oMelhor ? oMelhor.transform:retorno;
            }
            


            return retorno;
        }

        public static Transform Procure(GameObject esseObjeto, string[] tags, float distancia = 40)
        {
            return Procure(esseObjeto, EncontraveisComTag(tags), distancia);
        }

        public static Transform Procure(GameObject esseObjeto, float distancia, params string[] tags)
        {
            return Procure(esseObjeto, EncontraveisComTag(tags), distancia);
        }

        public static Transform Procure(GameObject esseObjeto, params string[] tags)
        {
            return Procure(esseObjeto, EncontraveisComTag(tags), 40);
        }

        public static Transform Procure(
            GameObject esseObjeto,
            List<GameObject> encontraveisV,
            float distancia = 40,
            bool melhorVisaoDaCamera = false)
        {
            //Vector3 vendo;
            Vector3 c;


            GameObject alvo = null;

            
            Transform T = esseObjeto.transform;
            List<GameObject> inimigosPerto = MaisProximosQue(T, encontraveisV, distancia);


            #region suprimido transformado em função
            //foreach (GameObject encontravel in encontraveisV)
            //{
            //    if (encontravel != esseObjeto)
            //    {
            //        c = encontravel.transform.position;
            //        vendo = c - T.position;



            //        if (vendo.sqrMagnitude < Mathf.Pow(distancia, 2))
            //            inimigosPerto.Add(encontravel);
            //    }
            //}
            #endregion



            if (inimigosPerto.Count != 0)
            {
                GameObject deMelhorVisao = null;
                GameObject maisPerto = null;
                Transform camT = melhorVisaoDaCamera ? CameraApplicator.cam.transform : T;
                float targetDist = melhorVisaoDaCamera ? -0.25f : 0.5f;

                foreach (GameObject criature in inimigosPerto)
                {
                    c = criature.transform.position;

                    maisPerto = maisPerto != null
                        ?
                            (c - T.position).sqrMagnitude
                            <
                            (maisPerto.transform.position - T.position).sqrMagnitude
                            ?
                            criature
                            :
                            maisPerto
                            : criature;

                    deMelhorVisao = deMelhorVisao == null
                        ?
                            criature
                            :
                            Vector3.Dot((c - camT.position).normalized,
                                         camT.forward)
                            >
                            Vector3.Dot(
                                (deMelhorVisao.transform.position - camT.position)
                                .normalized,
                                camT.forward
                                )
                            ?
                            criature
                            :
                            deMelhorVisao;
                }



                if (deMelhorVisao == maisPerto
                   &&
                   Vector3.Dot(
                    (deMelhorVisao.transform.position - T.position).normalized,
                    camT.forward) > 0)
                {
                    alvo = Vector3.Dot((maisPerto.transform.position -
                                        T.position).normalized,
                                       camT.forward) > targetDist
                        ? deMelhorVisao : null;
                }
                else
                {
                    if ((maisPerto.transform.position - T.position)
                       .sqrMagnitude < 25 &&
                       Vector3.Dot((maisPerto.transform.position -
                                 T.position).normalized,
                                T.forward) > targetDist
                       )
                        alvo = maisPerto;
                    else
                        alvo = Vector3.Dot((deMelhorVisao.transform.position -
                                            T.position).normalized,
                                           camT.forward) > targetDist
                            ? deMelhorVisao : null;
                }
            }

            //procurouAlvo = true;


            //AttackHelp(alvo, T);

            return alvo != null ? alvo.transform : null;
        }

        #region suprimido
        //private static void AttackHelp(GameObject alvo, Transform T)
        //{

        //    Vector3 gira;
        //    if (alvo != null)
        //    {
        //        gira = alvo.transform.position - T.position;

        //        gira.y = 0;
        //        T.rotation = Quaternion.LookRotation(gira);

        //    }
        //}
        #endregion

        private static List<GameObject> EncontraveisComTag(string[] tags)
        {
            List<GameObject> encontraveis = new List<GameObject>();
            for (int i = 0; i < tags.Length; i++)
            {
                encontraveis.AddRange(GameObject.FindGameObjectsWithTag(tags[i]));
            }

            return encontraveis;
        }

        public static List<GameObject> ProximosDoPonto(Vector3 pontoDeProximidade, string[] tags, float distancia = 40)
        {
            List<GameObject> retorno = new List<GameObject>();
            GameObject[] Gs = EncontraveisComTag(tags).ToArray();

            foreach (GameObject G in Gs)
            {
                if (Vector3.Distance(G.transform.position, pontoDeProximidade) < distancia &&
                   Vector3.Distance(G.transform.position, pontoDeProximidade) > 0 /*&&
               G.GetComponent<GerenciadorDeCriature>().MeuCriatureBase.CaracCriature.meusAtributos.PV.Corrente > 0*/)
                {
                    retorno.Add(G);
                }
            }
            return retorno;
        }

        public static List<GameObject> RemoveEu(List<GameObject> osPerto, GameObject eu)
        {
            bool remove = false;

            foreach (GameObject G in osPerto)
            {

                if (G == eu)
                    remove = true;
            }

            if (remove)
                osPerto.Remove(eu);

            return osPerto;
        }

        public static List<GameObject> ProximosDeMim(GameObject eu, string[] tags, float distancia = 40)
        {
            return RemoveEu(ProximosDoPonto(eu.transform.position, tags, distancia), eu);
        }
    }
}
