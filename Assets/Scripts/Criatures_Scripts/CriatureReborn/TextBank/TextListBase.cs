using UnityEngine;
using System.Collections.Generic;

namespace TextBankSpace
{
    public class TextListBase
    {
        protected static void ColocaTextos(ref Dictionary<TextKey, List<string>> retorno, Dictionary<TextKey, List<string>> inserir)
        {
            foreach (TextKey k in inserir.Keys)
            {
                retorno[k] = inserir[k];
            }
        }
    }
}