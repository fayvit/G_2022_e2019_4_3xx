using UnityEngine;
using Criatures2021;

namespace Criatures2021Hud
{
    public class ShowAttackOption : MonoBehaviour
    {
        [SerializeField] private ShowNewAttackHud showAttack;

        public void InsertAttackDetails(PetAttackBase petAttack,float powerModify)
        {
            showAttack.Start(petAttack, powerModify);
        }
    }
}