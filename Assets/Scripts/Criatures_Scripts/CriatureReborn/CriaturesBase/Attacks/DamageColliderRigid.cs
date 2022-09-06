using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class DamageColliderRigid : DamageCollider
    {
        private void OnCollisionEnter(Collision collision)
        {
            FuncaoTrigger(collision.collider);
        }

    }
}