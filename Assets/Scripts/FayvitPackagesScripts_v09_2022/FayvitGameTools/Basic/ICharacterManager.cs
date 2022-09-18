using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitBasicTools
{
    public interface ICharacterManager
    {
        CharacterState ThisState{get;}
        Transform transform { get; }
    }

    public enum CharacterState
    {
        notStarted = -1,
        onFree,
        stopedWithStoppedCam,
        withPet,
        externalMovement,
        externalPanelOpened,
        activeSingleMessageOpened,
        activeConfirmationOpened,
        stopped,
        NonBlockPanelOpened,
        inDamage,
        withKeyDjey
    }
}
