using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class InsertImpactView {

        public static void Insert(ImpactParticles noImpacto, Vector3 position, Quaternion qparticles)
        {
            GameObject impacto = Resources.Load<GameObject>("particles/"+noImpacto.ToString());
            impacto = MonoBehaviour.Instantiate(impacto, position, qparticles);

            if (impacto)
                MonoBehaviour.Destroy(impacto, 1.5f);
        }
    }
}