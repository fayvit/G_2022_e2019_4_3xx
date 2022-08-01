using System.Collections;
using UnityEngine;

namespace Criatures2021
{
    public class PetManagerTrainer : PetManagerEnemy
    {
        protected override void OnCriatureDefeated(MsgCriatureDefeated obj) {
            if (obj.defeated == gameObject)
                State = LocalState.defeated;
        }
    }
}