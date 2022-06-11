using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FayvitMessageAgregator;
using Criatures2021;
using System;

namespace Criatures2021Hud
{
    public class HudChangebleGameElements : MonoBehaviour
    {
        [SerializeField] private Image criatureImg;
        [SerializeField] private Image itemImg;
        [SerializeField] private Image attackImg;
        [SerializeField] private Image containerDeInfoText;
        [SerializeField] private Text infoText;
        [SerializeField] private Text itemCount;

        private float contTime = 0;
        private float timeTohide = 2;


        // Use this for initialization
        void Start()
        {
            MessageAgregator<MsgChangeSelectedAttack>.AddListener(OnChangeAttack);
            MessageAgregator<MsgChangeSelectedPet>.AddListener(OnChangeSelectedPet);
            MessageAgregator<MsgChangeSelectedItem>.AddListener(OnChangeSelectedItem);
            MessageAgregator<MsgChangeToHero>.AddListener(OnChangeToHero);
            MessageAgregator<MsgChangeToPet>.AddListener(OnChangeToPet);
            MessageAgregator<MsgStartGameElementsHud>.AddListener(OnStartHud);
            MessageAgregator<MsgHideGameElmentsHud>.AddListener(OnRequestHide);
            MessageAgregator<MsgReturnGameElmentsHud>.AddListener(OnRequestReturn);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgChangeSelectedAttack>.RemoveListener(OnChangeAttack);
            MessageAgregator<MsgChangeSelectedPet>.RemoveListener(OnChangeSelectedPet);
            MessageAgregator<MsgChangeSelectedItem>.RemoveListener(OnChangeSelectedItem);
            MessageAgregator<MsgChangeToHero>.RemoveListener(OnChangeToHero);
            MessageAgregator<MsgChangeToPet>.RemoveListener(OnChangeToPet);
            MessageAgregator<MsgStartGameElementsHud>.RemoveListener(OnStartHud);
            MessageAgregator<MsgHideGameElmentsHud>.RemoveListener(OnRequestHide);
            MessageAgregator<MsgReturnGameElmentsHud>.RemoveListener(OnRequestReturn);
        }

        private void OnRequestReturn(MsgReturnGameElmentsHud obj)
        {
            criatureImg.transform.parent.parent.gameObject.SetActive(true);
        }

        private void OnRequestHide(MsgHideGameElmentsHud obj)
        {
            criatureImg.transform.parent.parent.gameObject.SetActive(false);
        }

        private void OnStartHud(MsgStartGameElementsHud obj)
        {
            criatureImg.transform.parent.parent.gameObject.SetActive(true);

            if (obj.petname!=PetName.nulo)
                OnChangeSelectedPet(new MsgChangeSelectedPet() { petname = obj.petname });
            else
                criatureImg.transform.parent.gameObject.SetActive(false);

            if (obj.nameItem != NameIdItem.generico)
                OnChangeSelectedItem(new MsgChangeSelectedItem() { 
                    nameItem = obj.nameItem ,
                    quantidade = obj.countItem
                });
            else
                itemImg.transform.parent.gameObject.SetActive(false);

            OnChangeToHero(new MsgChangeToHero());
        }

        private void OnChangeToHero(MsgChangeToHero obj)
        {
            attackImg.transform.parent.gameObject.SetActive(false);
            //criatureImg.transform.parent.parent.gameObject.SetActive(false);
        }

        private void OnChangeToPet(MsgChangeToPet obj)
        {
            //criatureImg.transform.parent.parent.gameObject.SetActive(true);

            if (obj.numCriatures <= 1)
            {
                criatureImg.gameObject.SetActive(false);
            }
            else
            {
                if (obj.petToGoOut != PetName.nulo)
                {
                    criatureImg.gameObject.SetActive(true);
                    criatureImg.sprite = ResourcesFolders.GetMiniPet(obj.petToGoOut);
                    //= Resources.Load<Sprite>("miniCriatures/" + obj.petToGoOut.ToString());
                }
            }

            attackImg.transform.parent.gameObject.SetActive(true);
            attackImg.sprite = ResourcesFolders.GetMiniAttack(obj.atkSelected);
            
            //= Resources.Load<Sprite>("miniGolpes/" + obj.atkSelected.ToString());
        }



        private void OnChangeSelectedItem(MsgChangeSelectedItem obj)
        {
            if (obj.nameItem != NameIdItem.generico)
            {
                itemImg.transform.parent.gameObject.SetActive(true);
                itemCount.text = obj.quantidade.ToString();
                itemImg.sprite = ResourcesFolders.GetMiniItem(obj.nameItem);
                    //= Resources.Load<Sprite>("miniItens/" + obj.nameItem.ToString());
                ActiveInfoText(ItemBase.NomeEmLinguas(obj.nameItem));
            }else
                itemImg.transform.parent.gameObject.SetActive(false);
        }

        private void OnChangeSelectedPet(MsgChangeSelectedPet obj)
        {
            criatureImg.transform.parent.gameObject.SetActive(true);
            criatureImg.sprite = ResourcesFolders.GetMiniPet(obj.petname);
                
                //= Resources.Load<Sprite>("miniCriatures/" + obj.petname.ToString());
            ActiveInfoText(PetBase.NomeEmLinguas(obj.petname));
        }

        private void OnChangeAttack(MsgChangeSelectedAttack obj)
        {
            attackImg.sprite = ResourcesFolders.GetMiniAttack(obj.attackName);
                //= Resources.Load<Sprite>("miniGolpes/" + obj.attackName.ToString());
            ActiveInfoText(PetAttackBase.NomeEmLinguas(obj.attackName));
        }

        private void ActiveInfoText(string s)
        {
            infoText.text = s;
            containerDeInfoText.gameObject.SetActive(true);
            contTime = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (containerDeInfoText.gameObject.activeSelf)
            {
                contTime += Time.deltaTime;
                if (contTime > timeTohide)
                {
                    containerDeInfoText.gameObject.SetActive(false);
                }
            }
        }
    }
}

public struct MsgChangeSelectedPet : IMessageBase {
    public PetName petname;
    public bool temGolpePorAprender;
}
public struct MsgChangeSelectedItem : IMessageBase {
    public NameIdItem nameItem;
    public int quantidade;
}
public struct MsgChangeSelectedAttack : IMessageBase {
    public AttackNameId attackName;
}
public struct MsgStartGameElementsHud : IMessageBase {
    public PetName petname;
    public NameIdItem nameItem;
    public int countItem;
    public int countCristals;
    public bool temGolpePorAprender;
}

public struct MsgHideGameElmentsHud : IMessageBase { }
public struct MsgReturnGameElmentsHud : IMessageBase { }