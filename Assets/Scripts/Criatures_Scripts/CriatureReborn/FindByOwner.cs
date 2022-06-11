using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class FindByOwner : MonoBehaviour
    {
        public static Transform GetEnemy(GameObject owner)
        {
            return owner.GetComponent<CharacterManager>().ActivePet.Mov.LockTarget;
        }

        public static PetManager GetManagerEnemy(GameObject owner)
        {
            if (owner.GetComponent<CharacterManager>().ActivePet.Mov.LockTarget != null)
                return owner.GetComponent<CharacterManager>().ActivePet.Mov.LockTarget.GetComponent<PetManager>();
            else
                return null;
        }

        public static PetManager GetHeroActivePet(GameObject owner)
        {
            return owner.GetComponent<CharacterManager>().ActivePet;
        }

        public static PetManager GetAnyHeroActivePet()
        {
            return GameObject.FindWithTag("Player").GetComponent<CharacterManager>().ActivePet;
        }

    }
}