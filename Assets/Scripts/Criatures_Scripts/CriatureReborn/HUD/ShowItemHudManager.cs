using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FayvitMessageAgregator;
using Criatures2021;
using TextBankSpace;

namespace Criatures2021Hud
{
    public class ShowItemHudManager : MonoBehaviour
    {
        
        [SerializeField] private Text txtNameItem;
        [SerializeField] private Text txtDescriptionItem;
        [SerializeField] private Image imgItem;

        [SerializeField] protected Text numItens;
        [SerializeField] protected Text labelIntroduction;

        // Use this for initialization
        void Start()
        {
            MessageAgregator<MsgShowItem>.AddListener(OnRequestShowItem);
            MessageAgregator<MsgHideShowItem>.AddListener(OnRequestHideItem);
            MessageAgregator<MsgGetChestItem>.AddListener(OnGetChestItem);
        }

        private void OnDestroy()
        { 

            MessageAgregator<MsgGetChestItem>.RemoveListener(OnGetChestItem);
            MessageAgregator<MsgShowItem>.RemoveListener(OnRequestShowItem);
            MessageAgregator<MsgHideShowItem>.RemoveListener(OnRequestHideItem);
        }

        private void OnGetChestItem(MsgGetChestItem obj)
        {
            OnRequestShowItem(new MsgShowItem() { idItem=obj.nameItem,quantidade=obj.quantidade});
        }

        private void OnRequestHideItem(MsgHideShowItem obj)
        {
            FinishHud();
        }

        protected virtual void FillDates(MsgShowItem obj,string customlabel)
        {
            imgItem.sprite = ResourcesFolders.GetMiniItem(obj.idItem);
            numItens.SetText(obj.quantidade.ToString());
            txtNameItem.SetText(ItemBase.NomeEmLinguas(obj.idItem));
            txtDescriptionItem.SetText(TextBank.RetornaListaDeTextoDoIdioma(TextKey.shopInfoItem)[(int)obj.idItem]);
            labelIntroduction.SetText(customlabel);
        }

        private void OnRequestShowItem(MsgShowItem obj)
        {
            labelIntroduction.transform.parent.gameObject.SetActive(true);

            FillDates(obj, "Você recebeu");
        }

        public void FinishHud()
        {
            labelIntroduction.transform.parent.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    public struct MsgShowItem : IMessageBase
    {
        public NameIdItem idItem;
        public int quantidade;
    }

    public struct MsgHideShowItem : IMessageBase { }
}