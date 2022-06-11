using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FayvitMessageAgregator;
using Criatures2021;
using TextBankSpace;
using System;

namespace Criatures2021Hud
{
    public class ShowItemHudManager : MonoBehaviour
    {
        [SerializeField] private Text labelIntroduction;
        [SerializeField] private Text numItens;
        [SerializeField] private Text txtNameItem;
        [SerializeField] private Text txtDescriptionItem;
        [SerializeField] private Image imgItem;

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
            labelIntroduction.transform.parent.gameObject.SetActive(false);
        }

        private void OnRequestShowItem(MsgShowItem obj)
        {
            labelIntroduction.transform.parent.gameObject.SetActive(true);
            imgItem.sprite = ResourcesFolders.GetMiniItem(obj.idItem);
            labelIntroduction.text = "Você recebeu";
            numItens.text = obj.quantidade.ToString();
            txtNameItem.text = ItemBase.NomeEmLinguas(obj.idItem);
            txtDescriptionItem.text = TextBank.RetornaListaDeTextoDoIdioma(TextKey.shopInfoItem)[(int)obj.idItem];
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