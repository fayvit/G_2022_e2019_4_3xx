

using UnityEngine;

namespace Criatures2021
{
    class OnGroundAreaDamageCollider: DamageColliderBase
    {
        void OnTriggerEnter(Collider emQ)
        {
            if (emQ.CompareTag("Criature"))
            {
                PetManager P = emQ.GetComponent<PetManager>();
                if(P && P.Mov.IsGrounded)
                    FuncaoTrigger(emQ);
            }
        }
    }
}
