using FayvitMessageAgregator;
using UnityEngine;

namespace Criatures2021
{
    public struct MsgVerifyPoisonDamageMessage:IMessageBase
    {
        public float quantificador;
        public GameObject afetado;
        public PetBase pDoAfetado;
    }

    public struct MsgVerifyPoisonDefeatedPet : IMessageBase
    {
        public PetManager afetado;
        public PetBase pDoAfetado;
    }

    public struct MsgWhoIsTheLoserPet : IMessageBase
    {
        public bool player;
    }
}
