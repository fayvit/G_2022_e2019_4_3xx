using UnityEngine;
using System.Collections;

namespace FayvitBasicTools
{
    public class StaticInstanceExistence<T>
    {
        public delegate T GetInstance();
        public static bool SchelduleExistence(System.Action r, MonoBehaviour m, GetInstance instance)
        {
            T g = instance();
            if (g != null)
                return true;
            else
            {
                m.StartCoroutine(Rotina(r));
                return false;
            }
        }

        static IEnumerator Rotina(System.Action r)
        {
            yield return new WaitForSecondsRealtime(0.15f);
            r();
        }
    }
}