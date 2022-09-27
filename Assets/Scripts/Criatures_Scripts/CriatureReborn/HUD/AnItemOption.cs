
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FayvitUI;
using FayvitMessageAgregator;
using Criatures2021;

namespace Criatures2021Hud
{
    public class AnItemOption : AnOption
    {
        [SerializeField] private Image optionImage;
        [SerializeField] private Text amountTxt;
        [SerializeField] private GameObject speedIcon;
        
        protected GameObject AmountContainer => amountTxt.transform.parent.gameObject;
        public Image OptionImage { get { return optionImage; } set { optionImage = value; } }
        public ItemBase ThisItem { get; private set; }

        private void Start()
        {
            MessageAgregator<MsgItemAmountChange>.AddListener(OnAmountChange);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgItemAmountChange>.RemoveListener(OnAmountChange);
        }

        private void OnAmountChange(MsgItemAmountChange obj)
        {
            if (obj.target == ThisItem)
            {
                amountTxt.text = obj.target.Estoque.ToString();
            }
        }

        public void SetarOpcoes(ItemBase thisItem,System.Action<int> A)
        {
            this.ThisItem = thisItem;
            ThisAction += A;
            if (thisItem.ID == NameIdItem.generico)
            {
                optionImage.gameObject.SetActive(false);
                amountTxt.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log(thisItem.ID);

                optionImage.gameObject.SetActive(true);
                amountTxt.transform.parent.gameObject.SetActive(true);
                optionImage.sprite = ResourcesFolders.GetMiniItem(thisItem.ID);
                amountTxt.text = thisItem.Estoque.ToString();
            }

            speedIcon.SetActive(thisItem.NosItensRapidos);
        }
    }

}
