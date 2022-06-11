using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitCommandReader
{
    public class RawCustomAxis
    {

        private static Dictionary<string, float> lerpVal = new Dictionary<string, float>();
        private const float deathVal = 0.2f;

        public static int KeyDownAxis(string esseGatilho, IKeyDict KeyDict, int IndexOfControl)
        {
            int retorno;
            var thisDict = KeyDict.DicAxis[esseGatilho];
            bool pos = false;
            bool neg = false;
            for (int i = 0; i < thisDict.Count; i++)
            {
                pos |= Input.GetKeyDown(KeyDict.DicAxis[esseGatilho][i].pos);
                neg |= Input.GetKeyDown(KeyDict.DicAxis[esseGatilho][i].neg);
            }
            retorno = pos
                ? 1 :
                neg ? -1 : 0;
            if (retorno == 0)
            {
                try
                {
                    float f = Input.GetAxisRaw("joy " + IndexOfControl + " " + esseGatilho);
                    retorno = f > 0 ? 1 : f < 0 ? -1 : 0;
                }
                catch
                {

                }
            }
            return retorno;
        }

        public static int StaticGetAxis(string esseGatilho,IKeyDict KeyDict,int IndexOfControl)
        {
            int retorno;
            var thisDict = KeyDict.DicAxis[esseGatilho];
            bool pos = false;
            bool neg = false;
            for (int i = 0; i < thisDict.Count; i++)
            {
                pos|= Input.GetKey(KeyDict.DicAxis[esseGatilho][i].pos);
                neg |= Input.GetKey(KeyDict.DicAxis[esseGatilho][i].neg);
            }
            retorno = pos
                ? 1 :
                neg ? -1 : 0;
            if (retorno == 0)
            {
                try {
                    float f = Input.GetAxisRaw("joy " + IndexOfControl + " " + esseGatilho);
                    retorno = f > 0 ? 1 : f < 0 ? -1 : 0;
                } catch
                { 
                
                }
            }
            return retorno;
        }

        public static float GetAxis(string oGatilho,int esseControle,IKeyDict KeyDict)
        {
            string esseGatilho = oGatilho + ((Controlador)esseControle).ToString();

            float retorno = 0;
            if (lerpVal.ContainsKey(esseGatilho) )
                retorno = StaticGetAxis(oGatilho,KeyDict,esseControle);
            else
                lerpVal[esseGatilho] = 0;

            if (retorno != 0)
                lerpVal[esseGatilho] = Mathf.Lerp(lerpVal[esseGatilho], retorno, 5 * Time.fixedDeltaTime);
            else if (lerpVal[esseGatilho] > deathVal|| lerpVal[esseGatilho] < -deathVal)
                lerpVal[esseGatilho] = Mathf.Lerp(lerpVal[esseGatilho], retorno, 5 * Time.fixedDeltaTime);
            else
                lerpVal[esseGatilho] = 0;

            retorno = lerpVal[esseGatilho];

            return retorno;
        }

        public static void ZeraLeroVal(string oGatilho, int esseControle)
        {
            string esseGatilho = oGatilho + ((Controlador)esseControle).ToString();

            if (lerpVal.ContainsKey(esseGatilho))
                lerpVal[esseGatilho] = 0;
            else
                lerpVal[esseGatilho] = 0;
        }

        public static void ZeraLerpVal(int esseControle)
        {
            ZeraLeroVal("horizontal", esseControle);
            ZeraLeroVal("vertical", esseControle);
            ZeraLeroVal("triggers", esseControle);
            ZeraLeroVal("Xcam", esseControle);
            ZeraLeroVal("Ycam", esseControle);
            ZeraLeroVal("HDpad", esseControle);
            ZeraLeroVal("VDpad", esseControle);
        }
    }
}