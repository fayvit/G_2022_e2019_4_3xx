
using UnityEngine;
using UnityEngine.UI;
using FayvitUI;
using Criatures2021;
using FayvitMessageAgregator;

namespace Criatures2021Hud
{
    public class A_PetMenuOption:AnOption
    {
        [SerializeField] private Image petImage;
        [SerializeField] private Image statusContainer;
        [SerializeField] private Image statusImage;
        [SerializeField] private Image pvImage;
        [SerializeField] private Image peImage;
        [SerializeField] private Text petName;
        [SerializeField] private Text nivel;
        [SerializeField] private Text PVtext;
        [SerializeField] private Text PEtext;

        private int antHp;
        private int newHp;
        private int antMp;
        private int newMp;
        private int maxHp;
        private int maxMp;
        private float tempoDecorrido=0;
        private float TempoModificandoBar = .5f;
        private bool lerpBarAtivo;
        private PetBase observer;

        private float tempoMudandoStatus = 0;
        private float tempoTotalParaMudarStatus = 1;

        public void Start()
        {
            MessageAgregator<MsgChangeHP>.AddListener(OnChangeHp);
            MessageAgregator<MsgChangeMP>.AddListener(OnChangeMp);
            MessageAgregator<MsgRemoveStatus>.AddListener(OnRemoveStatus);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgChangeHP>.RemoveListener(OnChangeHp);
            MessageAgregator<MsgChangeMP>.RemoveListener(OnChangeMp);
            MessageAgregator<MsgRemoveStatus>.RemoveListener(OnRemoveStatus);
        }

        private void OnRemoveStatus(MsgRemoveStatus obj)
        {
            
            if (obj.petBase == observer)
            {
                if (observer.StatusTemporarios.Count > 0)
                    statusImage.sprite = ResourcesFolders.GetMiniStatus(observer.StatusTemporarios[0].Tipo);
                else
                    statusContainer.gameObject.SetActive(false);
            }
        }

        private void OnChangeHp(MsgChangeHP obj)
        {
            if (obj.target == observer)
            {
                lerpBarAtivo = true;
                tempoDecorrido = 0;
                antHp = obj.antHp;
                newHp = obj.currentHp;
                maxHp = obj.maxHp;
                newMp = antMp;
            }
        }

        private void OnChangeMp(MsgChangeMP obj)
        {
            if (obj.target == observer)
            {
                lerpBarAtivo = true;
                tempoDecorrido = 0;
                antMp = obj.antMp;
                newMp = obj.currentMp;
                maxMp = obj.maxMp;
                newHp = antHp;
            }
        }

        private void Update()
        {
            if (lerpBarAtivo)
            {
                tempoDecorrido += Time.deltaTime;

                pvImage.fillAmount = Mathf.Lerp((float)antHp /maxHp, (float)newHp /maxHp, tempoDecorrido / TempoModificandoBar);
                peImage.fillAmount = Mathf.Lerp((float)antMp /maxMp,(float)newMp/maxMp, tempoDecorrido / TempoModificandoBar);
                PVtext.text = (int)Mathf.Lerp(antHp, newHp, tempoDecorrido / TempoModificandoBar)+" / "+maxHp;
                PEtext.text = (int)Mathf.Lerp(antMp, newMp, tempoDecorrido / TempoModificandoBar) + " / " + maxMp;
            }

            if (observer.StatusTemporarios.Count > 1)
            {
                
                tempoMudandoStatus += Time.deltaTime;
                int x = (int)(observer.StatusTemporarios.Count * tempoMudandoStatus / tempoTotalParaMudarStatus)
                        %   observer.StatusTemporarios.Count;

                //Debug.Log("numero x: " + x);

                statusImage.sprite = ResourcesFolders.GetMiniStatus(observer.StatusTemporarios[x].Tipo);

                if (tempoMudandoStatus > tempoTotalParaMudarStatus)
                    tempoMudandoStatus = 0;
            }
        }

        public void SetarOpcoes(PetBase P,System.Action<int> action)
        {
            ThisAction += action;
            observer = P;
            petImage.sprite = ResourcesFolders.GetMiniPet(P.NomeID);
            if (P.StatusTemporarios.Count > 0)
            {
                statusContainer.gameObject.SetActive(true);
                statusImage.sprite = ResourcesFolders.GetMiniStatus(P.StatusTemporarios[0].Tipo);
            }
            else
                statusContainer.gameObject.SetActive(false);

            PetAtributes A = P.PetFeat.meusAtributos;
            pvImage.fillAmount = (float)A.PV.Corrente / A.PV.Maximo;
            peImage.fillAmount = (float)A.PE.Corrente / A.PE.Maximo;
            petName.text = P.GetNomeEmLinguas;
            nivel.text = InterfaceTextList.txt[InterfaceTextKey.NV] + ": " + P.G_XP.Nivel;
            PVtext.text = InterfaceTextList.txt[InterfaceTextKey.PV] + ": " + A.PV.Corrente + " / " + A.PV.Maximo;
            PEtext.text = InterfaceTextList.txt[InterfaceTextKey.PE] + ": " + A.PE.Corrente + " / " + A.PE.Maximo;
            maxMp = A.PE.Maximo;
            maxHp = A.PV.Maximo;
            antHp = A.PV.Corrente;
            antMp = A.PE.Corrente;
        }
    }
}
