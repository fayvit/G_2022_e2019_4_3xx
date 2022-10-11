using UnityEngine;
using System.Collections.Generic;

namespace FayvitCommandReader
{
    public class CommandConverterBase : ICommandConverter
    {
        protected Dictionary<CommandConverterInt, List<int>> dicCommandConverterInt = new Dictionary<CommandConverterInt, List<int>>()
        {
            { CommandConverterInt.jump,new List<int>{0} },
            { CommandConverterInt.dodge,new List<int>{1} },
            { CommandConverterInt.run,new List<int>{2} },
            { CommandConverterInt.criatureChange,new List<int>{5} },
            { CommandConverterInt.itemUse,new List<int>{4} },
            { CommandConverterInt.heroToCriature,new List<int>{8} },
            { CommandConverterInt.camFocus,new List<int>{9} },
            { CommandConverterInt.confirmButton,new List<int>{1,8,7} },
            { CommandConverterInt.returnButton,new List<int>{9,6,3} },
            { CommandConverterInt.humanAction,new List<int>{1} },
            { CommandConverterInt.updateMenu,new List<int>{7} },
            { CommandConverterInt.starterMenuConfirm,new List<int>{7,13} },
            { CommandConverterInt.starterMenuReturn,new List<int>{6,14} },
            { CommandConverterInt.starterMenuExtraKey,new List<int>{0} },
            { CommandConverterInt.starterMenuOption1,new List<int>{10} },
            { CommandConverterInt.starterMenuOption2,new List<int>{11} },
            { CommandConverterInt.menuMoreBlack,new List<int>{8} },
            { CommandConverterInt.menuMoreWhite,new List<int>{9} },
            { CommandConverterInt.menuOpenList,new List<int>{15} },
            { CommandConverterInt.menuMoveToList,new List<int>{16} },
            { CommandConverterInt.pauseMenu,new List<int>{6} },
            { CommandConverterInt.deletarSave,new List<int>{17} },
            { CommandConverterInt.keyDjeyAction,new List<int>{3} },
        };

        protected Dictionary<CommandConverterString, List<string>> dicCommandConverterString 
            = new Dictionary<CommandConverterString, List<string>>()
        {
            { CommandConverterString.camX,new List<string>{"Xcam"} },
            { CommandConverterString.camY,new List<string>{"Ycam"} },
            { CommandConverterString.camZ,new List<string>{"Zcam"} },
            { CommandConverterString.moveH,new List<string>{"horizontal"} },
            { CommandConverterString.moveV,new List<string>{"vertical"} },
            { CommandConverterString.attack,new List<string>{"triggerR"} },
            { CommandConverterString.focusInTheEnemy,new List<string>{"triggerL"} },
            { CommandConverterString.selectAttack_selectCriature,new List<string>{"VDpad"} },
            { CommandConverterString.itemChange,new List<string>{"HDpad"} },
            { CommandConverterString.alternativeV_Change,new List<string>{"VDpad"} },
            { CommandConverterString.alternativeH_Change,new List<string>{"HDpad"} },
            { CommandConverterString.menuChangeH,new List<string>{"horizontal"} },
            { CommandConverterString.menuChangeV,new List<string>{"vertical"} },
            { CommandConverterString.changeLockTargetX,new List<string>{ "changeLockTargetX" } },
            { CommandConverterString.changeLockTargetY,new List<string>{ "changeLockTargetY" } },
        };

        public virtual Dictionary<CommandConverterString, List<string>> DicCommandConverterString => dicCommandConverterString;

        public virtual Dictionary<CommandConverterInt,List<int>> DicCommandConverterInt => dicCommandConverterInt;
    }
}