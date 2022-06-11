using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitBasicTools
{
    public class ContadorCiclico
    {
        public static int Contar(int val, int current, int max)
        {
            if (current + val < max && current + val >= 0)
                return current + val;
            else if (current + val >= max)
                return 0;
            else if (current + val < 0)
                return max - 1;
            else return current;

        }

        public static int ContarComResto(int val, int current, int max)
        {
            if (current + val < max && current + val >= 0)
                return current + val;
            else if (current + val >= max || current + val < 0)
            {
                int r = (current + val) % max;
                if (r >= 0)
                    return r;
                else
                    return max + r;
            }

            else return current;

        }
    }
}