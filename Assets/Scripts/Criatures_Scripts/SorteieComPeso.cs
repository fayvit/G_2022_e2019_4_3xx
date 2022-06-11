using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class SorteieComPeso
    {
        public static int Sorteie(float[] pesos)
        {
            float sum = 0;
            foreach (var f in pesos)
            {
                sum += f;
            }

            float ff = Random.Range(0, sum);
            sum = 0;

            for (int i = 0; i < pesos.Length; i++)
            {
                sum += pesos[i];
                if (sum >= ff)
                    return i;
            }

            return 0;
        }
    }
}