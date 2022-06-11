using Criatures2021;
using FayvitMessageAgregator;
using System;
using System.Collections;
using System.Collections.Generic;
using TextBankSpace;
using UnityEngine;
using UnityEngine.UI;

namespace Criatures2021Hud
{
    public class DescartarHud : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private Image imgItem;
        [SerializeField] private Text quantidadeItem;
        [SerializeField] private Text pergunta;
        [SerializeField] private Text infoText;
        [SerializeField] private InputField inputNumber;

        private float tempoDecorrido = 0;
        private ItemBase oDescartavel;
        private LocalState state = LocalState.emEspera;
        private InfoTextState infoState = InfoTextState.emEspera;
        private MsgSendExternalPanelCommand cmd;

        private const float TEMPO_CLAREANDO = .5F;
        private const float TEMPO_MOSTRANDO = 1.5F;
        private const float TEMPO_ESCURECENDO = .75F;

        private enum LocalState
        { 
            emEspera,
            emMudanca
        }

        private enum InfoTextState
        { 
            emEspera,
            clareando,
            mostrando,
            escurecendo
        }

        // Start is called before the first frame update
        void Start()
        {
            MessageAgregator<MsgRequestOpenDiscartHud>.AddListener(OnRequestOpen);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgRequestOpenDiscartHud>.RemoveListener(OnRequestOpen);
        }

        private void OnRequestOpen(MsgRequestOpenDiscartHud obj)
        {
            container.SetActive(true);
            oDescartavel = obj.descartavel;
            imgItem.sprite = ResourcesFolders.GetMiniItem(obj.descartavel.ID);
            quantidadeItem.text = oDescartavel.Estoque.ToString();
            pergunta.text = string.Format(TextBank.RetornaFraseDoIdioma(TextKey.MenuDescartavel),
                ItemBase.NomeEmLinguas(oDescartavel.ID));

            Color C = infoText.color;
            inputNumber.text = "1";
            infoText.color = new Color(C.r, C.g, C.b, 0);
            infoState = InfoTextState.emEspera;
            state = LocalState.emMudanca;

            MessageAgregator<MsgSendExternalPanelCommand>.AddListener(OnReceiveCommand);

        }

        private void OnReceiveCommand(MsgSendExternalPanelCommand obj)
        {
            cmd = obj;
        }

        void ChangeInputVal(int val)
        {
            int thisVal = int.Parse(inputNumber.text);
            if (thisVal + val <= 0)
            {
                StartInfoMessage(TextBank.RetornaListaDeTextoDoIdioma(TextKey.MenuDescartavel)[1]);
                thisVal = 1;
            }
            else if (thisVal + val > oDescartavel.Estoque)
            {
                StartInfoMessage(
                    string.Format(
                    TextBank.RetornaListaDeTextoDoIdioma(TextKey.MenuDescartavel)[2],
                    oDescartavel.Estoque));
                
                thisVal = oDescartavel.Estoque;
            }
            else
            {
                thisVal += val;
            }

            inputNumber.text = thisVal.ToString();

            
        }

        void StartInfoMessage(string message)
        {
            Color C = infoText.color;
            infoText.color = new Color(C.r, C.g, C.b, 0);
            infoText.text = message;
            infoState = InfoTextState.clareando;
            tempoDecorrido = 0;
        }

        void UpdateInfoMessage()
        {
            if(infoState!=InfoTextState.emEspera)
                tempoDecorrido += Time.deltaTime;

            switch (infoState)
            {
                case InfoTextState.clareando:
                    Color C = infoText.color;
                    infoText.color = new Color(C.r, C.g, C.b, Mathf.Lerp(0,1,tempoDecorrido/TEMPO_CLAREANDO));
                    if (tempoDecorrido >= TEMPO_CLAREANDO)
                    {
                        tempoDecorrido = 0;
                        infoState = InfoTextState.mostrando;
                    }
                break;
                case InfoTextState.mostrando:
                    if (tempoDecorrido >= TEMPO_MOSTRANDO)
                    {
                        tempoDecorrido = 0;
                        infoState = InfoTextState.escurecendo;
                    }
                break;
                case InfoTextState.escurecendo:
                    Color Cc = infoText.color;
                    infoText.color = new Color(Cc.r, Cc.g, Cc.b, Mathf.Lerp(1, 0, tempoDecorrido / TEMPO_ESCURECENDO));
                    if (tempoDecorrido >= TEMPO_ESCURECENDO)
                    {
                        tempoDecorrido = 0;
                        infoState = InfoTextState.emEspera;
                    }
                break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            UpdateInfoMessage();

            switch (state)
            {
                case LocalState.emMudanca:
                    if (cmd.hChange<0)
                    {
                        ChangeInputVal(-1);
                    }
                    else if (cmd.hChange>0)
                    {
                        ChangeInputVal(1);
                    }
                    else if (cmd.vChange<0)
                    {
                        ChangeInputVal(-10);
                    }
                    else if (cmd.vChange>0)
                    {
                        ChangeInputVal(10);
                    }
                    else if (cmd.confirmButton)
                    {
                        BtnConfirm();
                    }
                    else if (cmd.returnButton)
                    {
                        BtnCancel();
                    }
                break;
            }
        }

        void FinishHud()
        {
            container.SetActive(false);
            state = LocalState.emEspera;
            infoState = InfoTextState.emEspera;
        }

        public void BtnPlus1()
        {
            ChangeInputVal(1);
        }

        public void BtnPlus10()
        {
            ChangeInputVal(10);
        }

        public void BtnMinus1()
        {
            ChangeInputVal(-1);
        }
        public void BtnMinus10()
        {
            ChangeInputVal(-10);
        }

        public void BtnConfirm()
        {
            MessageAgregator<MsgFinishDiscartHud>.Publish(new MsgFinishDiscartHud()
            {
                amount = int.Parse(inputNumber.text),
                discartResult = true
            });
            FinishHud();
        }

        public void BtnCancel()
        {
            MessageAgregator<MsgFinishDiscartHud>.Publish(new MsgFinishDiscartHud()
            {
                discartResult = false
            });
            FinishHud();
        }
    }

    public struct MsgRequestOpenDiscartHud : IMessageBase
    {
        public ItemBase descartavel;
    }

    public struct MsgFinishDiscartHud : IMessageBase {
        public bool discartResult;
        public int amount;
    }
}