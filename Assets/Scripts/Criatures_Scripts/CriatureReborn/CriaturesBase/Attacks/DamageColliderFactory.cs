using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class DamageColliderFactory
    {
        public static DamageColliderBase Get(GameObject G, ProjetilType t)
        {
            switch (t)
            {
                case ProjetilType.basico:
                    return G.AddComponent<DamageCollider>();
                case ProjetilType.rigido:
                    return G.AddComponent<DamageColliderRigid>();
                case ProjetilType.statusExpansivel:
                    return G.AddComponent<ExpansiveStatusCollider>();
                case ProjetilType.direcional:
                    return G.AddComponent<DamageColliderDirectional>();
                case ProjetilType.area:
                    return G.AddComponent<OnGroundAreaDamageCollider>();
                default:
                    return null;

            }
            //return t switch
            //{
            //    ProjetilType.basico => G.AddComponent<DamageCollider>(),
            //    ProjetilType.rigido => G.AddComponent<DamageColliderRigid>(),
            //    _ => null
            //};
        }
    }
}