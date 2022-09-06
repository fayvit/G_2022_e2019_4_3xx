using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FayvitBasicTools;
using FayvitMove;
using FayvitMessageAgregator;
using System;

namespace FayvitLikeDarkSouls
{
    public class LikeDsHud : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Image hpBar;
        [SerializeField] private Image stBar;
        [SerializeField] private Image stZeroed;
        [SerializeField] private PiscarBarra pisca;

        private GameObject dono;
        private bool piscando;

        private void Start()
        {
            pisca = new PiscarBarra(stZeroed);
            //MessageAgregator<MsgStartHud>.AddListener(OnStartCriatureHud);

            MessageAgregator<MsgStartDsHud>.AddListener(OnStartDsHud);
            //MessageAgregator<MsgChangeHP>.AddListener(OnChangeHp);
            MessageAgregator<MsgChangeST>.AddListener(OnChangeSt);
            MessageAgregator<MsgZeroedStamina>.AddListener(OnZeroedStamina);
            MessageAgregator<MsgRegenZeroedStamina>.AddListener(OnRegenZeroedStamina);

        }

        private void OnDestroy()
        {

            MessageAgregator<MsgStartDsHud>.RemoveListener(OnStartDsHud);
            //MessageAgregator<MsgChangeHP>.RemoveListener(OnChangeHp);
            MessageAgregator<MsgChangeST>.RemoveListener(OnChangeSt);
            MessageAgregator<MsgZeroedStamina>.RemoveListener(OnZeroedStamina);
            MessageAgregator<MsgRegenZeroedStamina>.RemoveListener(OnRegenZeroedStamina);

        }

        private void OnStartDsHud(MsgStartDsHud obj)
        {
            dono = obj.dono;
            root.SetActive(true);
        }

        private void OnRegenZeroedStamina(MsgRegenZeroedStamina obj)
        {
            if (obj.gameObject == dono)
            {
                piscando = false;
                pisca.SetOpacityZero();
            }
        }

        private void OnZeroedStamina(MsgZeroedStamina obj)
        {
            if (obj.gameObject == dono)
            {
                piscando = true;
            }
        }

        //private void OnChangeHp(MsgChangeHP obj)
        //{
        //    if (obj.gameObject == dono)
        //    {
        //        hpBar.fillAmount = (float)obj.currentHp / obj.maxHp;
        //        //hpNumber.text = obj.currentHp + " / " + obj.maxHp;
        //    }
        //}

        private void OnChangeSt(MsgChangeST obj)
        {
            //Debug.Log(dono + " : " + obj.gameObject);

            if (obj.gameObject == dono)
            {
                stBar.fillAmount = (float)obj.currentSt / obj.maxSt;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (piscando)
                pisca.PiscarSemTempo();
        }
    }

    public struct MsgStartDsHud : IMessageBase
    { 
        public GameObject dono;
    }
}