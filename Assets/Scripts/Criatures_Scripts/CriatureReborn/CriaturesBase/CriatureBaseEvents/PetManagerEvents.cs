using FayvitMessageAgregator;
using UnityEngine;

namespace Criatures2021
{
    public struct MsgChangeHP : IMessageBase
    {
        public GameObject gameObject;
        public PetBase target;
        public int currentHp;
        public int antHp;
        public int maxHp;
    }

    public struct MsgChangeMP : IMessageBase
    {
        public GameObject gameObject;
        public PetBase target;
        public int currentMp;
        public int antMp;
        public int maxMp;
    }

    public struct MsgAnEnemyTargetMe : IMessageBase { }
    public struct MsgSendEnemyTargets : IMessageBase
    {
        public PetManager target;
        public PetManager sender;
    }

    public struct MsgRequestNonReturnableDamage : IMessageBase
    {
        public GameObject gameObject;
        public PetAttackBase petAttack;
    }
    public struct MsgCriatureUpdateButtonPress : IMessageBase
    {
        public GameObject dono;
    }
    public struct MsgChangeLevel : IMessageBase
    {
        public GameObject gameObject;
        public int newLevel;
        public int pvCorrente;
        public int pvMax;
        public int peCorrente;
        public int peMaximo;
        public PetAttackDb petAtkDb;
    }

    public struct MsgRequestChangeSelectedPetWithPet : IMessageBase
    {
        public GameObject pet;
    }
    public struct MsgTargetEnemy : IMessageBase
    {
        public Transform targetEnemy;
    }

    public struct MsgUnTargetEnemy : IMessageBase { }

    public struct MsgRequestReplacePet : IMessageBase
    {
        public GameObject dono;
        public Transform lockTarget;
        public bool replaceIndex;
        public int newIndex;
    }
    public struct MsgRequestChangeSelectedItemWithPet : IMessageBase
    {
        public int change;
        public GameObject pet;
    }
    public struct MsgRequestUseItem : IMessageBase
    {
        public GameObject dono;
    }
}