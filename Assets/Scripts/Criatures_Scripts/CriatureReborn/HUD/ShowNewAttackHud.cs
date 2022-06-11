using UnityEngine;
using UnityEngine.UI;
using Criatures2021;

namespace Criatures2021Hud
{
    [System.Serializable]
    public class ShowNewAttackHud 
    {
        [SerializeField] private Text attackName;
        [SerializeField] private Text txtTypeName;
        [SerializeField] private Text txtPower;
        [SerializeField] private Text txtPeCost;
        [SerializeField] private Text txtStCost;
        [SerializeField] private Image imgAttack;

        // Use this for initialization
        public void Start(PetAttackBase petAtk,float powerModify)
        {
            attackName.text = petAtk.NomeEmLinguas();
            txtPower.text = ((int)(petAtk.PotenciaCorrente+powerModify)).ToString();
            txtPeCost.text = petAtk.CustoPE.ToString();
            txtStCost.text = petAtk.CustoDeStamina.ToString();
            txtTypeName.text = TypeNameInLanguages.Get(petAtk.Tipo);
            imgAttack.sprite = ResourcesFolders.GetMiniAttack(petAtk.Nome);

            txtPeCost.transform.parent.gameObject.SetActive(true);
        }

        // Update is called once per frame
        public void EndHud()
        {
            txtPeCost.transform.parent.gameObject.SetActive(false);
        }
    }
}