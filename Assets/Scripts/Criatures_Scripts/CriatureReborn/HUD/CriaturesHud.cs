using Criatures2021;
using FayvitBasicTools;
using FayvitMove;
using FayvitMessageAgregator;
using UnityEngine;
using UnityEngine.UI;

namespace Criatures2021Hud
{
    public class CriaturesHud : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Image hpBar;
        [SerializeField] private Image mpBar;
        [SerializeField] private Image stBar;
        [SerializeField] private Text hpLabel;
        [SerializeField] private Text hpNumber;
        [SerializeField] private Text mpLabel;
        [SerializeField] private Text mpNumber;
        [SerializeField] private Text nameCriature;
        [SerializeField] private Text labelLevel;
        [SerializeField] private Text numberLevel;
        [SerializeField] private Image stZeroed;
        [SerializeField] private PiscarBarra pisca;


        private GameObject dono;
        private bool piscando;

        private void Start()
        {
            pisca = new PiscarBarra(stZeroed);
            //MessageAgregator<MsgStartHud>.AddListener(OnStartCriatureHud);
            MessageAgregator<MsgChangeToPet>.AddListener(OnStartCriatureHud);
            MessageAgregator<MsgChangeToHero>.AddListener(OnChangeToHero);
            MessageAgregator<MsgChangeHP>.AddListener(OnChangeHp);
            MessageAgregator<MsgChangeMP>.AddListener(OnChangeMp);
            MessageAgregator<MsgChangeST>.AddListener(OnChangeSt);
            MessageAgregator<MsgZeroedStamina>.AddListener(OnZeroedStamina);
            MessageAgregator<MsgRegenZeroedStamina>.AddListener(OnRegenZeroedStamina);
            MessageAgregator<MsgChangeLevel>.AddListener(OnChangeLvel);
        }

        private void OnDestroy()
        {

            //MessageAgregator<MsgStartHud>.AddListener(OnStartCriatureHud);
            MessageAgregator<MsgChangeToPet>.RemoveListener(OnStartCriatureHud);
            MessageAgregator<MsgChangeToHero>.RemoveListener(OnChangeToHero);
            MessageAgregator<MsgChangeHP>.RemoveListener(OnChangeHp);
            MessageAgregator<MsgChangeMP>.RemoveListener(OnChangeMp);
            MessageAgregator<MsgChangeST>.RemoveListener(OnChangeSt);
            MessageAgregator<MsgZeroedStamina>.RemoveListener(OnZeroedStamina);
            MessageAgregator<MsgRegenZeroedStamina>.RemoveListener(OnRegenZeroedStamina);
            MessageAgregator<MsgChangeLevel>.RemoveListener(OnChangeLvel);

        }

        private void OnChangeLvel(MsgChangeLevel obj)
        {
            if (obj.gameObject== dono)
            {
                numberLevel.text = obj.newLevel.ToString();
                hpNumber.text = obj.pvCorrente + " / " + obj.pvMax;
                mpNumber.text = obj.peCorrente + " / " + obj.peMaximo;
                hpBar.fillAmount = (float)obj.pvCorrente / obj.pvMax;
                mpBar.fillAmount = (float)obj.peCorrente / obj.peMaximo;
            }
        }

        private void OnChangeToHero(MsgChangeToHero obj)
        {
            root.SetActive(false);
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

        private void OnChangeHp(MsgChangeHP obj)
        {
            if (obj.gameObject == dono)
            {
                hpBar.fillAmount = (float)obj.currentHp / obj.maxHp;
                hpNumber.text = obj.currentHp + " / " + obj.maxHp; 
            }
        }

        private void OnChangeSt(MsgChangeST obj)
        {
            if (obj.gameObject == dono)
            {
                stBar.fillAmount = (float)obj.currentSt / obj.maxSt;
            }
        }

        private void OnChangeMp(MsgChangeMP obj)
        {
            if (obj.gameObject == dono)
            {
                mpBar.fillAmount = (float)obj.currentMp / obj.maxMp;
                mpNumber.text = obj.currentMp + " / " + obj.maxMp;
            }
        }

        private void OnStartCriatureHud(MsgChangeToPet obj)
        {
            dono = obj.oCriature;

            nameCriature.text = obj.name;
            numberLevel.text = obj.level.ToString();
            mpBar.fillAmount = (float)obj.currentMp / obj.maxMp;
            mpNumber.text = obj.currentMp + " / " + obj.maxMp;
            stBar.fillAmount = (float)obj.currentSt / obj.maxSt;
            hpBar.fillAmount = (float)obj.currentHp / obj.maxHp;
            hpNumber.text = obj.currentHp + " / " + obj.maxHp;

            root.SetActive(true);
        }

        private void Update()
        {
            if (piscando)
                pisca.PiscarSemTempo();
        }
    }
}

//public struct MsgStartHud : IMessageBase
//{
//    public GameObject oCriature;
//    public int currentSt;
//    public int maxSt;
//    public int currentHp;
//    public int maxHp;
//    public int currentMp;
//    public int maxMp;
//    public string name;
//    public string level;
//}



public struct MsgChangeST : IMessageBase
{
    public GameObject gameObject;
    public int currentSt;
    public int maxSt;
}