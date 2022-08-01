using System.Collections;
using UnityEngine;
using Criatures2021;

namespace FieldEnemy
{
    public class SpawnPetForVigilance : MonoBehaviour
    {
        [SerializeField] private PetBase spanable;
        [SerializeField] private GameObject vigilanceTrigger;
        [SerializeField] private Transform moveOnDamage;
        [SerializeField] private PetAttackBase petAttack;
        [SerializeField] private float timeInMoveToDamage = .25f;

        // Use this for initialization
        void Start()
        {
            PetManagerEnemy P = (PetManagerEnemy)WildPetInitialize.Initialize(spanable, transform.position);
            FayvitSupportSingleton.SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                P.transform.rotation = transform.rotation;
                P.SetStartVigilance(PetManager.LocalState.stopped);
            });
            PetVigilanceListener pvl= P.gameObject.AddComponent<PetVigilanceListener>();
            pvl.SetVilanceListener(P, vigilanceTrigger, moveOnDamage,timeInMoveToDamage,petAttack);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}