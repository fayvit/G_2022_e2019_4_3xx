using Criatures2021;
using FayvitMessageAgregator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Criatures2021Hud
{
    public class CommandInfoManager : MonoBehaviour
    {
        [SerializeField] private GameObject esquivarContainer;
        [SerializeField] private GameObject escolherAlvoContainer;
        [SerializeField] private Text txtAlternancia;
        [SerializeField] private GameObject[] allHeroFamilly;

        // Start is called before the first frame update
        void Start()
        {
            
            MessageAgregator<MsgChangeToHero>.AddListener(OnChangeToHero);
            MessageAgregator<MsgChangeToPet>.AddListener(OnChangeToPet);
            MessageAgregator<MsgStartGameElementsHud>.AddListener(OnStartHud);
        }

        private void OnDestroy()
        {
            
            MessageAgregator<MsgChangeToHero>.RemoveListener(OnChangeToHero);
            MessageAgregator<MsgChangeToPet>.RemoveListener(OnChangeToPet);
            MessageAgregator<MsgStartGameElementsHud>.RemoveListener(OnStartHud);
        }

        private void OnStartHud(MsgStartGameElementsHud obj)
        {
            foreach (var v in allHeroFamilly)
                v.SetActive(true);
        }

        private void OnChangeToPet(MsgChangeToPet obj)
        {
            esquivarContainer.SetActive(true);
            escolherAlvoContainer.SetActive(true);
            txtAlternancia.text = TextBankSpace.TextBank.textosDeInterface[TextBankSpace.TextBank.linguaChave][InterfaceTextKey.aoHeroi];
        }

        private void OnChangeToHero(MsgChangeToHero obj)
        {
            esquivarContainer.SetActive(false);
            escolherAlvoContainer.SetActive(false);
            txtAlternancia.text = TextBankSpace.TextBank.textosDeInterface[TextBankSpace.TextBank.linguaChave][InterfaceTextKey.aoCriature];
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}