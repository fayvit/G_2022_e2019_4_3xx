using Criatures2021;
using UnityEngine;

namespace TalkSpace
{
    public class IaFightForTrainerGround : FightForTrainerGround
    {
        [SerializeField] private EnemyIaPercent[] iaPercent;

        protected override void SetParticularEnemyDetails(PetManager myActivePet,int index)
        {
            if (iaPercent.Length > index)
            {
                ((PetManagerEnemy)myActivePet).ChangeIaType(iaPercent[index]);
            }
        }
    }
}