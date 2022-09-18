using UnityEngine;
using System.Collections.Generic;

namespace FayvitCommandReader
{
    public class KeyboardKeysDict : CommandConverterBase, IKeyDict
    {
        private static KeyboardKeysDict instance;
        public static KeyboardKeysDict Instance
        {
            get
            {
                if (instance == null)
                    instance = new KeyboardKeysDict();

                return instance;
            }
        }

        private KeyboardKeysDict() { }

        public static Dictionary<int, List<KeyCode>> dicKeys = new Dictionary<int, List<KeyCode>>
        {
            { 0,new List<KeyCode>{KeyCode.Space} },
            { 1,new List<KeyCode>{KeyCode.L} },
            { 2,new List<KeyCode>{KeyCode.LeftShift} },
            { 3,new List<KeyCode>{KeyCode.O} },
            { 4,new List<KeyCode>{KeyCode.F} },
            { 5,new List<KeyCode>{KeyCode.R,KeyCode.I } },
            { 6,new List<KeyCode>{KeyCode.Escape} },
            { 7,new List<KeyCode>{KeyCode.Return} },
            { 8,new List<KeyCode>{KeyCode.E} },
            { 9,new List<KeyCode>{KeyCode.Q,KeyCode.LeftAlt} },
            { 10,new List<KeyCode>{KeyCode.Alpha1} },
            { 11,new List<KeyCode>{KeyCode.Alpha2} },
            { 12,new List<KeyCode>{KeyCode.Mouse0} },
            { 13,new List<KeyCode>{KeyCode.Mouse1} },
            { 14,new List<KeyCode>{KeyCode.Mouse2} },
            { 15,new List<KeyCode>{KeyCode.Alpha3} },
            { 16,new List<KeyCode>{KeyCode.Alpha4} },
            { 17,new List<KeyCode>{KeyCode.Delete} },

        };

        public static readonly Dictionary<string, List<ValAxis>> dicAxis = new Dictionary<string, List<ValAxis>>
        {
            { "horizontal", new List <ValAxis>{new ValAxis(KeyCode.D,KeyCode.A) } },
            { "vertical",new List <ValAxis>{new ValAxis(KeyCode.W,KeyCode.S)} },
            { "Xcam",new List <ValAxis>{new ValAxis(KeyCode.K,KeyCode.H)} },
            { "Ycam",new List <ValAxis>{new ValAxis(KeyCode.J,KeyCode.U)} },
            { "HDpad",new List <ValAxis>{new ValAxis(KeyCode.Alpha2,KeyCode.Alpha1),new ValAxis(KeyCode.RightArrow,KeyCode.LeftArrow) } },
            { "VDpad",new List <ValAxis>{new ValAxis(KeyCode.Alpha4,KeyCode.Alpha3),new ValAxis(KeyCode.UpArrow,KeyCode.DownArrow) } },
            { "triggerL",new List <ValAxis>{new ValAxis(KeyCode.Tab,KeyCode.None) } },
            { "triggerR",new List <ValAxis>{new ValAxis(KeyCode.P,KeyCode.None) } },
            { "triggers",new List <ValAxis>{new ValAxis(KeyCode.P,KeyCode.Y) } },
            { "Zcam",new List <ValAxis>{new ValAxis(KeyCode.Y,KeyCode.I)} },
            { "changeLockTargetX",new List <ValAxis>{new ValAxis(KeyCode.K,KeyCode.H)} },
            { "changeLockTargetY",new List <ValAxis>{new ValAxis(KeyCode.J,KeyCode.U)} },
        };


        

        //public Dictionary<CommandConverterInt, int> dicCommandConverterInt = new Dictionary<CommandConverterInt, int>()
        //{
        //    { CommandConverterInt.jump,0},
        //    { CommandConverterInt.dodge,1},
        //    { CommandConverterInt.run,2},
        //    { CommandConverterInt.criatureChange,3},
        //    { CommandConverterInt.itemUse,4},
        //    { CommandConverterInt.lightAttack,5},
        //    { CommandConverterInt.heroToCriature,8},
        //    { CommandConverterInt.camFocus,9}
        //};

        //public Dictionary<CommandConverterString, string> dicCommandConverterString = new Dictionary<CommandConverterString, string>()
        //{
        //    { CommandConverterString.camX,"Xcam"},
        //    { CommandConverterString.camY,"Ycam"},
        //    { CommandConverterString.moveH,"horizontal"},
        //    { CommandConverterString.moveV,"vertical"},
        //    { CommandConverterString.attack,"triggerR"},
        //    { CommandConverterString.focusInTheEnemy,"triggerL"},
        //};

        //public Dictionary<CommandConverterString, string> DicCommandConverterString => dicCommandConverterString;

        //public Dictionary<CommandConverterInt, int> DicCommandConverterInt => dicCommandConverterInt;

        public Dictionary<int, List<KeyCode>> DicKeys => dicKeys;

        public Dictionary<string, List<ValAxis>> DicAxis => dicAxis;
    }
}
