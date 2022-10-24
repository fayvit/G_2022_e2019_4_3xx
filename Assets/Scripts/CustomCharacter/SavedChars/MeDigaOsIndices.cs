using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomizationSpace
{
    class MeDigaOsIndices
    {
        public static void Diga(CustomizationContainerDates ccd)
        {
            List<CustomizationIdentity> malhas;
            List<CustomizationIdentity> malhasComb;
            List<CustomizationIdentity> texturasE;
            List<ColorAssignements> colorAssign;

            ccd.GetDates(out malhas, out malhasComb, out texturasE, out colorAssign);
            Debug.Log(ccd.Sid);

            
            //foreach (CustomizationIdentity s in malhas)
            //{
            //    Debug.Log("Malhas: " + s.id + " : " + s.contador);
            //    foreach (var v in colorAssign)
            //    {
            //        if (v.id == s.id)
            //        {
            //            for (int i = 0; i < v.coresEditaveis.Length; i++)
            //                Debug.Log("<color=#" + ColorUtility.ToHtmlStringRGB(v.coresEditaveis[i].cor) + "> indice dessa cor é: </color>" 
            //                    + i+" :"+
            //                    v.coresEditaveis[i].coresEditaveis.ColorTargetName+" : "+
            //                    v.coresEditaveis[i].coresEditaveis.indiceDoMaterialAlvo);

            //        }
            //    }
            //    //SimpleChangebleMesh[] scm = sdbc.GetDbMeshWithId(s.id);
            //    //SimpleMesh c2 = GetMeshInListById(s.id);
            //    //Debug.Log(s.id);
            //    //c2.contador = s.contador;
            //    ////Debug.Log("c2: " + c2);
            //    ////Debug.Log("c2.atual: " + c2.atual);
            //    ////Debug.Log("scm"+scm);
            //    ////Debug.Log("scm[s.contador]" + scm[s.contador]);
            //    ////Debug.Log("scm[s.contador].mesh" + scm[s.contador].mesh);
            //    //MudarMesh(ref c2.atual, scm[s.contador].mesh);

            //}
            foreach (CustomizationIdentity s in malhasComb)
            {
                //Debug.Log("MalhasComb: " + s.id + " : " + s.contador);
                //foreach (var v in colorAssign)
                //{
                //    if (v.id == s.id)
                //    {
                //        for (int i = 0; i < v.coresEditaveis.Length; i++)
                //            Debug.Log("<color=#" + ColorUtility.ToHtmlStringRGB(v.coresEditaveis[i].cor) + "> indice dessa cor é: </color>"
                //                + i + " :" +
                //                v.coresEditaveis[i].coresEditaveis.ColorTargetName + " : " +
                //                v.coresEditaveis[i].coresEditaveis.indiceDoMaterialAlvo);

                //    }
                //}
                //CombinedChangebleMesh[] ccm = sdbc.GetCombinedMeshDbWithID(c.id);
                //CombinedMesh c2 = GetCombinedMeshInListById(c.id);
                //c2.contador = c.contador;
                //MudarMesh(ref c2.atual, ccm[c.contador].mesh);
                //c2.contador = c.contador;
            }

            foreach (CustomizationIdentity ct in texturasE)
            {
                foreach (var v in colorAssign)
                { 
                    if(v.id==ct.id)
                        for (int i = 0; i < v.coresEditaveis.Length; i++)
                            Debug.Log("<color=#" + ColorUtility.ToHtmlStringRGB(v.coresEditaveis[i].cor) + "> indice dessa cor é: </color>"
                                + i + " :" +
                                v.coresEditaveis[i].coresEditaveis.ColorTargetName + " : " +
                                v.coresEditaveis[i].coresEditaveis.indiceDoMaterialAlvo);
                }
                //MaskedTexture[] mt = sdbc.GetMaskedTexDbWithId(ct.id);
                //ChangeTextureElementTo(ct.id, mt, ct.contador);
            }
        }
    }
}
