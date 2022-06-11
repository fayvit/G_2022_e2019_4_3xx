using UnityEngine;
using System.Collections;
using FayvitUI;
using UnityEngine.UI;
using Criatures2021;
using FayvitMessageAgregator;

namespace Criatures2021Hud
{
    public class OptionPetForListHud : AnOption
    {
        [SerializeField] private Image imgDoCriature;
        [SerializeField] private Text txtPVnum;
        [SerializeField] private Text txtPEnum;
        [SerializeField] private Text nomeCriature;
        [SerializeField] private Text txtNivelNum;
        [SerializeField] private Text txtListaDeStatus;

        private bool armagedom = false;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetarCriature(PetBase C, System.Action<int> ao, bool armagedom = false)
        {
            this.armagedom = armagedom;
            ThisAction += ao;

            PetAtributes A = C.PetFeat.meusAtributos;

            imgDoCriature.sprite = ResourcesFolders.GetMiniPet(C.NomeID);
                //GameController.g.El.RetornaMini(C.NomeID);
            nomeCriature.text = C.GetNomeEmLinguas;
            txtNivelNum.text = C.PetFeat.mNivel.Nivel.ToString();
            txtPVnum.text = A.PV.Corrente + " / " + A.PV.Maximo;
            txtPEnum.text = A.PE.Corrente + " / " + A.PE.Maximo;
            txtListaDeStatus.text = "";

            if (A.PV.Corrente <= 0)
            {
                Text[] txtS = GetComponentsInChildren<Text>();

                for (int i = 1; i < txtS.Length - 2; i++)
                    txtS[i].color = Color.gray;

                txtS[0].color = new Color(1, 1, 0.75f);

                txtListaDeStatus.text = "derrotado";
            }
            else
                txtListaDeStatus.text = "preparado";
        }
    }
}