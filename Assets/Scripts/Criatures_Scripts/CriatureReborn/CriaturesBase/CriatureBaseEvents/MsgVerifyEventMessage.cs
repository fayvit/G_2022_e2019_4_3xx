using FayvitMessageAgregator;
using UnityEngine;

namespace Criatures2021
{
    public struct MsgVerifyEventMessage:IMessageBase
    {
        public GameObject atacado;
        public PetAttackBase atk;
    }
}
