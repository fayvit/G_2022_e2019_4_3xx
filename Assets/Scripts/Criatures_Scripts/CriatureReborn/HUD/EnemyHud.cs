using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FayvitMessageAgregator;
using Criatures2021;

namespace Criatures2021Hud
{
    public class EnemyHud : MonoBehaviour
    {
        [SerializeField] private Text enemyName;
        [SerializeField] private Text levelNumber;
        [SerializeField] private Image hpBar;

        private PetManager owner;

        // Use this for initialization
        void Start()
        {
            MessageAgregator<MsgTargetEnemy>.AddListener(OnTargetEnemy);
            MessageAgregator<MsgUnTargetEnemy>.AddListener(OnUnTargetEnemy);
            MessageAgregator<MsgChangeHP>.AddListener(OnChangeHp);
            MessageAgregator<MsgChangeToHero>.AddListener(OnChangeToHero);
            MessageAgregator<MsgCriatureDefeated>.AddListener(OnCriatureDefeated);
            MessageAgregator<MsgPrepareFinalWithCapture>.AddListener(OnRequestHideHud);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgTargetEnemy>.RemoveListener(OnTargetEnemy);
            MessageAgregator<MsgUnTargetEnemy>.RemoveListener(OnUnTargetEnemy);
            MessageAgregator<MsgChangeHP>.RemoveListener(OnChangeHp);
            MessageAgregator<MsgChangeToHero>.RemoveListener(OnChangeToHero);
            MessageAgregator<MsgCriatureDefeated>.RemoveListener(OnCriatureDefeated);
            MessageAgregator<MsgPrepareFinalWithCapture>.RemoveListener(OnRequestHideHud);
        }

        private void OnUnTargetEnemy(MsgUnTargetEnemy obj)
        {
            enemyName.transform.parent.gameObject.SetActive(false);
        }

        private void OnRequestHideHud(MsgPrepareFinalWithCapture obj)
        {
            if (owner != null)
                if (obj.capturado == owner.gameObject)
                {
                    enemyName.transform.parent.gameObject.SetActive(false);
                }

            if (owner == null)
                Debug.LogError("A captura chama o esconder hud com criature nulo");
        }

        private void OnCriatureDefeated(MsgCriatureDefeated obj)
        {
            if (owner != null)
                if (obj.defeated == owner.gameObject)
                {
                    enemyName.transform.parent.gameObject.SetActive(false);
                }
        }

        private void OnChangeToHero(MsgChangeToHero obj)
        {
            enemyName.transform.parent.gameObject.SetActive(false);
        }

        private void OnChangeHp(MsgChangeHP obj)
        {
            if(owner)
                if (obj.gameObject == owner.gameObject)
                {
                    hpBar.fillAmount = (float)obj.currentHp / obj.maxHp;
                }
        }

        private void OnTargetEnemy(MsgTargetEnemy obj)
        {
            enemyName.transform.parent.gameObject.SetActive(true);

            owner = obj.targetEnemy.GetComponent<PetManager>();
            enemyName.text = owner.MeuCriatureBase.GetNomeEmLinguas;
            levelNumber.text = owner.MeuCriatureBase.G_XP.Nivel.ToString();

            ConsumableAttribute pp = owner.MeuCriatureBase.PetFeat.meusAtributos.PV;
            hpBar.fillAmount = (float)pp.Corrente / pp.Maximo;
            
        }

        // Update is called once per frame
        void Update()
        {
            //if (owner==null && enemyName.transform.parent.gameObject.activeSelf)
            //{
            //    enemyName.transform.parent.gameObject.SetActive(false);
            //}
        }
    }
}