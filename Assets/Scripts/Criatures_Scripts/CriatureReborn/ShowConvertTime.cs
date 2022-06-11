using UnityEngine;
using System.Collections;

public class ShowConvertTime
{
    public enum ShowTimeType { 
        m_s_ms,
        s,
        h_m_s_ms,
    }

    public static string Show(float t, ShowTimeType tipo = ShowTimeType.m_s_ms, bool tiraZero = true)
    {
        string retorno = "";
        float ms = (int)(t * 1000) % 1000;
        float s = ((int)t) % 60;
        float h = ((int)t) / 3600;
        float m = (((int)t) / 60) % 60;

        switch (tipo)
        {
            case ShowTimeType.m_s_ms:
                if (tiraZero)
                {
                    if (m == 0)
                        retorno = s + "s" + ms + "ms";
                    else
                        retorno = m + "m" + s + "s" + ms + "ms";
                }
                else
                    retorno = m + "m" + s + "s" + ms + "ms";

            break;
            case ShowTimeType.h_m_s_ms:
                retorno = h + "h" + m + "min" + s + "s" + ms + "ms";
            break;
            case ShowTimeType.s:
                retorno = (s + 60 * m).ToString();
            break;
        }

        return retorno;
    }
}
