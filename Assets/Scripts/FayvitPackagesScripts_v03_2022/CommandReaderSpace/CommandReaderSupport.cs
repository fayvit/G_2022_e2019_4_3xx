using UnityEngine;
using System.Collections.Generic;
using FayvitSupportSingleton;

namespace FayvitCommandReader
{
    public abstract class CommandReaderSupport
    {

        public abstract ICommandConverter CC{get;}
        public abstract ICommandReader CR { get; }
        private static Dictionary<string, bool> zerados = new Dictionary<string, bool>();
        private Dictionary<CommandConverterInt, TravarQuadro> travaQuadro = new Dictionary<CommandConverterInt, TravarQuadro>();

        private enum TravarQuadro
        { 
            down,
            up,
            livre
        }

        public static bool VerificaUsoDesseControle(ICommandReader c)
        {
            bool retorno = false;
            //Input.ResetInputAxes();
            if (c.GetAxis("horizontal") != 0 ||
                c.GetAxis("vertical") != 0 ||
                c.GetAxis("triggers") != 0 ||
                c.GetAxis("Xcam") != 0 ||
                c.GetAxis("Ycam") != 0 ||
                c.GetAxis("HDpad") != 0 ||
                c.GetAxis("VDpad") != 0)
            {
                //Debug.Log(c is KeyboardCommandReader ? "teclado" : "joystick");
                //Debug.Log(c.GetAxis("horizontal") + " horizontal<=" +
                //c.GetAxis("vertical") + " vertical<=" +
                //c.GetAxis("triggers") + " triggers<=" +
                //c.GetAxis("Xcam") + " Xcam <=" +
                //c.GetAxis("Ycam") + " Yccam<=" +
                //c.GetAxis("HDpad") + " HDpad<=" +
                //c.GetAxis("VDpad") + "VDpad");
                retorno = true;
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    if (c.GetButton(i))
                    {
                        //Debug.Log(c is KeyboardCommandReader ? "teclado" :(c is JoystickCommandReader? "joystick": "que isso?")+"botão "+i);
                        retorno = true;
                    }
                }
            }
            return retorno;
        }

        protected static int VerificaValorSeZerado(string esseGatilho, float val, float valTolerance)
        {
            int retorno = 0;
            if (!zerados.ContainsKey(esseGatilho))
                zerados[esseGatilho] = true;

            if (zerados[esseGatilho])
            {
                if (val != 0)
                {
                    zerados[esseGatilho] = false;

                }

                if (val > 0)
                    retorno = 1;
                else if (val < 0)
                    retorno = -1;

            }
            else
            {

                retorno = 0;
                if (val > -valTolerance && val < valTolerance)
                    zerados[esseGatilho] = true;

            }

            return retorno;
        }

        protected static Vector3 VetorDirecao(float h, float v)
        {
            Vector3 forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);

            forward.y = 0;
            forward = forward.normalized;

            Vector3 right = new Vector3(forward.z, 0, -forward.x);

            return (h * right + v * forward);
        }

        public bool GetButton(string nameButton,ICommandReader c)
        {
            return c.GetButton(KeyStringDict.GetIntForString(nameButton));
        }

        public bool GetButtonDown(string nameButton, ICommandReader c)
        {
            return c.GetButtonDown(KeyStringDict.GetIntForString(nameButton));
        }

        public bool GetButtonUp(string nameButton, ICommandReader c)
        {
            return c.GetButtonUp(KeyStringDict.GetIntForString(nameButton));
        }

        public bool GetButton(CommandConverterInt cci)
        {
            bool retorno = false;
            foreach (var v in CC.DicCommandConverterInt[cci])
                retorno |= CR.GetButton(v);

            return retorno;
        }

        public bool GetButtonDown(CommandConverterInt cci,bool travaQuadro = false)
        {
            bool retorno = false;
            foreach (var v in CC.DicCommandConverterInt[cci])
                retorno |= CR.GetButtonDown(v);

            bool verificaTrava = !VerificaTravarQuadro(cci, TravarQuadro.down);

            if (travaQuadro && retorno)
            {
                retorno = verificaTrava;
            }

            return retorno;
        }

        public bool GetButtonUp(CommandConverterInt cci,bool travaQuadro = false)
        {
            bool retorno = false;
            foreach (var v in CC.DicCommandConverterInt[cci])
                retorno |= CR.GetButtonUp(v);

            bool verificaTrava = !VerificaTravarQuadro(cci, TravarQuadro.up);

            if (travaQuadro && retorno)
            {
                retorno = verificaTrava;
            }

            return retorno;
        }

        bool VerificaTravarQuadro(CommandConverterInt cci,TravarQuadro tr)
        {
            bool retorno = false;
            if (this.travaQuadro.ContainsKey(cci))
            {
                retorno = (this.travaQuadro[cci] == tr);
            }
            else
                retorno = false;

            this.travaQuadro[cci] = tr;

            SupportSingleton.Instance.InvokeOnEndFrame(() => {
                this.travaQuadro[cci] = TravarQuadro.livre;
            });

            return retorno;
        }

        public float GetAxis(CommandConverterString ccs)
        {
            float retorno = 0;
            foreach (var v in CC.DicCommandConverterString[ccs])
                retorno = Mathf.Clamp(retorno+CR.GetAxis(v),-1,1);
            return retorno;
        }

        public int GetIntTriggerDown(CommandConverterString ccs)
        {
            int retorno = 0;
            foreach (var v in CC.DicCommandConverterString[ccs])
                retorno = Mathf.Clamp(retorno + CR.GetIntTriggerDown(v), -1, 1);
            return retorno;
        }

    }
}