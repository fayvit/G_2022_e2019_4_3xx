using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FayvitCommandReader;

namespace Assets.ScriptsLadoB.FayvitAdressable
{
    public class CommandsDictionary
    {
        private static Dictionary<KeyCode, InfoCommandTexture> texKeycodeDict = new Dictionary<KeyCode, InfoCommandTexture>()
        {
            { KeyCode.Return, InfoCommandTexture.Enter},
            { KeyCode.Escape,InfoCommandTexture.esc},
            { KeyCode.Delete,InfoCommandTexture.delete},
            { KeyCode.LeftShift,InfoCommandTexture.leftShift},
            { KeyCode.Space,InfoCommandTexture.space},
            { KeyCode.Tab,InfoCommandTexture.tab},
        };

        private static Dictionary<string, InfoCommandTexture> texStringJoyDict = new Dictionary<string, InfoCommandTexture>()
        {
            { "horizontal",InfoCommandTexture.alavancaL},
            { "vertical",InfoCommandTexture.alavancaL},
            { "Xcam",InfoCommandTexture.alavancaR},
            { "Ycam",InfoCommandTexture.alavancaR},
            { "triggerL",InfoCommandTexture.LT},
            { "triggerR",InfoCommandTexture.RT},
            { "triggers",InfoCommandTexture.LT_RT},
            { "HDpad",InfoCommandTexture.dpad_leftRight},
            { "VDpad",InfoCommandTexture.dpad_upDown},
            { "VDpadPos",InfoCommandTexture.dpad_up},
            { "VDpadNeg",InfoCommandTexture.dpad_down},

        };


        public static void SelectShowInfo(
            GameObject imgContainer,
            Image img,
            GameObject txtContainer,
            Text t,
            CommandConverterInt cmd,
            Controlador control)
        {
            if (control == Controlador.teclado)
            {
                KeyboardKeysDict kkd = (KeyboardKeysDict)(((KeyboardCommandReader)CommandReader.GetCR(Controlador.teclado)).CC);
                KeyCode k = kkd.DicKeys[
                kkd.DicCommandConverterInt[cmd][0]][0];
                VerifyKeyboardInfo(k,imgContainer,txtContainer,t,img);
                //Debug.Log(k + " : " + (int)k);

            }
            else if (control == Controlador.joystick1)
            {
                int n = ((JoystickCommandReader)CommandReader.GetCR(Controlador.joystick1)).CC.DicCommandConverterInt[cmd][0];
                imgContainer.SetActive(true);
                txtContainer.SetActive(false);
                //img.sprite = Resources.Load<Sprite>(n.ToString());
                AddressablesResourcesManager.SetSprite((InfoCommandTexture)n, imgContainer, (Sprite s) =>
                {
                    img.sprite = s;
                });
                //img.sprite=

            }
        }

        public static void SelectShowInfo(
            GameObject imgContainer,
            Image img,
            GameObject txtContainer,
            Text t,
            CommandConverterString cmd,
            bool positivo,
            Controlador control)
        {
            if (control == Controlador.teclado)
            {
                ValAxis k = KeyboardKeysDict.dicAxis[
                KeyboardKeysDict.Instance.DicCommandConverterString[cmd][0]][0];

                
                VerifyKeyboardInfo(positivo?k.pos:k.neg, imgContainer, txtContainer, t, img);
                
            }
            else if (control == Controlador.joystick1)
            {
                string n = ((JoystickCommandReader)CommandReader.GetCR(Controlador.joystick1)).CC.DicCommandConverterString[cmd][0];
                imgContainer.SetActive(true);
                txtContainer.SetActive(false);
                n = positivo ? n + "Pos" : n + "Neg";
                //img.sprite = Resources.Load<Sprite>(texStringJoyDict[n].ToString());
                AddressablesResourcesManager.SetSprite(texStringJoyDict[n], imgContainer, (Sprite s) =>
                {
                    img.sprite = s;
                });
                //img.sprite=

            }
        }

        public static void SelectShowInfo(
            GameObject img1Container,
            GameObject img2Container,
            Image img1,
            Image img2,
            GameObject txt1Container,
            GameObject txt2Container,
            Text t1,
            Text t2,
            CommandConverterString cmd,
            Controlador control)
        {
            if (control == Controlador.teclado)
            {
                ValAxis k = KeyboardKeysDict.dicAxis[
                KeyboardKeysDict.Instance.DicCommandConverterString[cmd][0]][0];

                if (k.neg != KeyCode.None)
                {
                    VerifyKeyboardInfo(k.neg, img1Container, txt1Container, t1, img1);
                    VerifyKeyboardInfo(k.pos, img2Container, txt2Container, t2, img2);
                }
                else
                {
                    VerifyKeyboardInfo(k.pos, img1Container, txt1Container, t1, img1);

                    if(img2Container!=null)
                        img2Container.SetActive(false);

                    if(txt2Container)
                        txt2Container.SetActive(false);
                }
            }
            else if (control == Controlador.joystick1)
            {
                string n = ((JoystickCommandReader)CommandReader.GetCR(Controlador.joystick1)).CC.DicCommandConverterString[cmd][0];
                img1Container.SetActive(true);
                txt1Container.SetActive(false);
                if (img2Container != null)
                    img2Container.SetActive(false);

                if (txt2Container)
                    txt2Container.SetActive(false);
                //img1.sprite = Resources.Load<Sprite>(texStringJoyDict[n].ToString());
                AddressablesResourcesManager.SetSprite(texStringJoyDict[n], img1Container, (Sprite s) =>
                {
                    img1.sprite = s;
                });
                //img.sprite=

            }
        }

        static void VerifyKeyboardInfo(KeyCode k,GameObject imgContainer,GameObject txtContainer,Text t,Image img)
        {
            if ((int)k > 96 && (int)k < 123)
            {
                imgContainer.SetActive(false);
                txtContainer.SetActive(true);
                t.text = k.ToString();
            }
            else if ((int)k > 47 && (int)k < 58)
            {
                imgContainer.SetActive(false);
                txtContainer.SetActive(true);
                t.text = ((int)k % 48).ToString();
            }
            else
            {
                imgContainer.SetActive(true);
                txtContainer.SetActive(false);
                //img.sprite = Resources.Load<Sprite>(texKeycodeDict[k].ToString());
                AddressablesResourcesManager.SetSprite(texKeycodeDict[k], imgContainer, (Sprite s) =>
                {
                    img.sprite = s;
                });
            }
        }


    }

    public enum InfoCommandTexture
    { 
        btnA,
        btnB,
        btnX,
        btnY,
        LB,
        RB,
        btnSelect,
        btnStart,
        LS,
        RS,
        Enter,
        esc,
        WASD,
        yuihjk,
        delete,
        leftShift,
        space,
        alavancaL,
        alavancaR,
        LT,
        RT,
        dpad_upDown,
        dpad_up,
        dpad_leftRight,
        dpad_down,
        tab,
        LT_RT
    }
    //public struct KeyTextureDict

    
}