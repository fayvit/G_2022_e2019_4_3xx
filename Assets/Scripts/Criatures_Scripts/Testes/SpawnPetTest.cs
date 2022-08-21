using System.Collections;
using UnityEngine;

namespace Criatures2021
{
    public class SpawnPetTest : MonoBehaviour
    {
        [SerializeField] private PetName pet;
        [SerializeField] private int nivel;
        [SerializeField] private PetAttackBase[] atksCustomizados;
        [SerializeField] private EnemyIaPercent iaPercent;
        //[SerializeField] ConsumableAttribute pv;
        //[SerializeField] ConsumableAttribute pe;
        //[SerializeField] IntrinsicAttribute ataque;
        //[SerializeField] IntrinsicAttribute defesa;
        //[SerializeField] IntrinsicAttribute poder;

        // Use this for initialization
        void Start()
        {
            PetManager P =  WildPetInitialize.Initialize(pet, nivel,transform.position);
            if (atksCustomizados != null && atksCustomizados.Length > 0)
            {
                P.MeuCriatureBase.GerenteDeGolpes.meusGolpes = new System.Collections.Generic.List<PetAttackBase>();
                P.MeuCriatureBase.GerenteDeGolpes.meusGolpes.AddRange(atksCustomizados);
            }

            ((PetManagerEnemy)P).ChangeIaType(iaPercent);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}