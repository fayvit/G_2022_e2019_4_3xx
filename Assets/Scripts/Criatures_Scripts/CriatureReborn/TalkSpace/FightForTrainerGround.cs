using UnityEngine;
using Criatures2021;

namespace TalkSpace
{
    public class FightForTrainerGround: TrainerForFight
    {
        [SerializeField] private Transform centerOfBattle;

        protected override void ElementosDoEncontro()
        {
            InsereElementosDoEncontro.EncontroDeTreinador(Manager, MeuTransform, centerOfBattle.position);
            Manager.ActivePet.transform.position = centerOfBattle.position - 3 * Manager.transform.forward;
        }
    }
}
