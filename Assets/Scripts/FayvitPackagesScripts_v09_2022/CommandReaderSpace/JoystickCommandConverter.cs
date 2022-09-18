using UnityEngine;
using System.Collections.Generic;

namespace FayvitCommandReader
{
    public class JoystickCommandConverter : CommandConverterBase
    {
        public override Dictionary<CommandConverterInt, List<int>> DicCommandConverterInt
        {
            get {
                Dictionary<CommandConverterInt, List<int>> baseDic = new Dictionary<CommandConverterInt, List<int>>(base.dicCommandConverterInt);
                baseDic[CommandConverterInt.starterMenuConfirm] = new List<int> { 0 };
                baseDic[CommandConverterInt.starterMenuReturn] = new List<int> { 1 };
                baseDic[CommandConverterInt.starterMenuExtraKey] = new List<int> { 2 };
                baseDic[CommandConverterInt.starterMenuOption1] = new List<int> { 4 };
                baseDic[CommandConverterInt.starterMenuOption2] = new List<int> { 5 };
                baseDic[CommandConverterInt.menuMoreBlack] = new List<int> { 2 };
                baseDic[CommandConverterInt.menuMoreWhite] = new List<int> { 3 };
                baseDic[CommandConverterInt.confirmButton] = new List<int> { 1 };
                baseDic[CommandConverterInt.returnButton] = new List<int> { 3 };
                baseDic[CommandConverterInt.menuOpenList] = new List<int> { 2 };
                baseDic[CommandConverterInt.menuMoveToList] = new List<int> { 3 };
                baseDic[CommandConverterInt.updateMenu] = new List<int> { 6 };
                baseDic[CommandConverterInt.pauseMenu] = new List<int> { 7 };
                baseDic[CommandConverterInt.deletarSave] = new List<int> { 6 };


                return baseDic;
            }
        }

        public override Dictionary<CommandConverterString, List<string>> DicCommandConverterString
        {
            get {
                Dictionary<CommandConverterString, List<string>> baseDic
                        = new Dictionary<CommandConverterString, List<string>>(dicCommandConverterString);
                baseDic[CommandConverterString.camZ] = new List<string> {"triggers"};
                baseDic[CommandConverterString.menuChangeH] = new List<string> { "horizontal", "HDpad" };
                baseDic[CommandConverterString.menuChangeV] = new List<string> { "vertical", "VDpad" };
                baseDic[CommandConverterString.changeLockTargetX] = new List<string> { "Xcam" };
                baseDic[CommandConverterString.changeLockTargetY] = new List<string> { "Ycam" };

                return baseDic;
            }
        }
    }

    public class AndroidCommandConverter : CommandConverterBase
    { 
    
    }
}