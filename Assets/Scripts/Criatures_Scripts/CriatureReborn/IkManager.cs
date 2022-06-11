using UnityEngine;

namespace Criatures2021
{
    public class IkManager
    {
        private Animator A;
        private IkManagerState state = IkManagerState.semIk;
        private Transform[] ikPositions;

        public void Start(Animator A)
        {
            this.A = A;
        }
        public enum IkManagerState
        { 
            semIk,
            maosDeEmpurrando
        }
        public void ChangeState(IkManagerState newState,Transform[] ikPositions)
        {
            this.ikPositions = ikPositions;
            state = newState;
        }

        public void Update()
        {
            switch (state)
            {
                case IkManagerState.maosDeEmpurrando:
                    A.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    A.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);

                    A.SetIKPosition(AvatarIKGoal.LeftHand, ikPositions[0].position);
                    A.SetIKPosition(AvatarIKGoal.RightHand, ikPositions[1].position);
                    
                break;
            }
        }
    }
}
