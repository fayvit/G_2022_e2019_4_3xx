using UnityEngine;
using System.Collections.Generic;

namespace FayvitCommandReader
{
    public interface IKeyDict
    {
        Dictionary<int, List<KeyCode>> DicKeys { get; }
        Dictionary<string, List<ValAxis>> DicAxis { get; }
        
    }

    public interface ICommandConverter
    {
        Dictionary<CommandConverterInt, List<int>> DicCommandConverterInt { get; }
        Dictionary<CommandConverterString, List<string>> DicCommandConverterString { get; }
    }

    public enum CommandConverterInt
    { 
        jump,
        attack,
        itemUse,
        criatureChange,
        run,
        lightAttack,
        dodge,
        heroToCriature,
        camFocus,
        confirmButton,
        returnButton,
        humanAction,
        updateMenu,
        starterMenuConfirm,
        starterMenuReturn,
        starterMenuExtraKey,
        starterMenuOption1,
        starterMenuOption2,
        menuMoreBlack,
        menuMoreWhite,
        menuOpenList,
        menuMoveToList,
        pauseMenu
    }

    public enum CommandConverterString
    { 
        moveH,
        moveV,
        camX,
        camY,
        attack,
        focusInTheEnemy,
        selectAttack_selectCriature,
        itemChange,
        alternativeV_Change,
        alternativeH_Change,
        camZ,
        menuChangeH,
        menuChangeV,
        changeLockTargetX,
        changeLockTargetY
    }
}
/*
 * 0=>Jump
 * 1=>run
 * 2=>dodge
 * 3=>changeCriatures
 * 4=>item
 * 5=>lightAttack
 * 8=>heroToCriature
 * 9=>camFocus
 * 
 * triggerL=>
 * triggerR=>mainSpecialAttack
 */