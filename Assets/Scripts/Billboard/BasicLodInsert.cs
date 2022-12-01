using System.Collections;
using UnityEngine;



public class BasicLodInsert
{
    public static void Insert(GameObject G, float lodTax)
    {
        FayvitSupportSingleton.SupportSingleton.Instance.InvokeInSeconds(() =>
        {
            if (G != null)
            {
                LODGroup L = G.GetComponent<LODGroup>();
                if (L == null)
                {
                    L = G.AddComponent<LODGroup>();

                    Renderer[] rs = G.GetComponentsInChildren<Renderer>();


                    LOD l = new LOD(lodTax, rs);
                    L.SetLODs(new LOD[1] { l });
                }
            }
        }, 3);
    }
}
