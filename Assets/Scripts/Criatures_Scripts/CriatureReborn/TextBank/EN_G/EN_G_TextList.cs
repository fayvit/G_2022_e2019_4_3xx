using System.Collections.Generic;
using UnityEngine;

namespace TextBankSpace
{
    public class EN_G_TextList : TextListBase
    {
        static Dictionary<TextKey, List<string>> txt;
        public static Dictionary<TextKey, List<string>> Txt
        {
            get
            {
                if (txt == null)
                {
                    txt = new Dictionary<TextKey, List<string>>();

                    ColocaTextos(ref txt, KeyTextEN_G.txt);
                    ColocaTextos(ref txt, FallenTextEN_G.txt);
                    ColocaTextos(ref txt, TextosDeBarreirasEN_G.txt);
                    ColocaTextos(ref txt, CustomizationTextsEN_G.txt);
                    //ColocaTextos(ref txt, TextosChaveEmPT_BR.txt);
                    //ColocaTextos(ref txt, TextosDeBarreirasPT_BR.txt);
                    //ColocaTextos(ref txt, TextosDaCavernaInicialPT_BR.txt);
                    //ColocaTextos(ref txt, TextosDeKatidsPT_BR.txt);
                    //ColocaTextos(ref txt, TextosDeMarjanPT_BR.txt);
                    //ColocaTextos(ref txt, TextosDeInfoPT_BR.txt);
                }

                return txt;
            }
        }
    }
}