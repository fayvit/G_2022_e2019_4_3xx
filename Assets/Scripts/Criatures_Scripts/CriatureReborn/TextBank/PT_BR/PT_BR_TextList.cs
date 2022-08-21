using UnityEngine;
using System.Collections.Generic;

namespace TextBankSpace
{
    public class PT_BR_TextList: TextListBase
    {
        static Dictionary<TextKey, List<string>> txt;
        public static Dictionary<TextKey, List<string>> Txt
        {
            get
            {
                if (txt == null)
                {
                    txt = new Dictionary<TextKey, List<string>>();

                    ColocaTextos(ref txt, PT_BR_TextosDeMenus.txt);
                    ColocaTextos(ref txt, KeyTextPT_BR.txt);
                    ColocaTextos(ref txt, FallenTextPT_BR.txt);
                    ColocaTextos(ref txt, TextosDeBarreirasPT_BR.txt);
                    ColocaTextos(ref txt, CustomizationTextsPT_BR.txt);
                    ColocaTextos(ref txt, TutoInfoPT_BR.txt);
                    ColocaTextos(ref txt, PT_BR_TextosIniciais.txt);
                    ColocaTextos(ref txt, PT_BR_TextosDeKatids.txt);
                    ColocaTextos(ref txt, PT_BR_TextosDeMarjan.txt);
                    ColocaTextos(ref txt, PT_BR_TextosDosDevotos.txt);
                }

                return txt;
            }
        }

    }
}