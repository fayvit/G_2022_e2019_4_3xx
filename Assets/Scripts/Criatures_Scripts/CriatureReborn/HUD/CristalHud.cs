using FayvitMessageAgregator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Criatures2021Hud
{
    public class CristalHud : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private Text txtCristalCount;
        
        // Start is called before the first frame update
        void Start()
        {
            MessageAgregator<MsgStartGameElementsHud>.AddListener(OnStartHud);
            MessageAgregator<MsgChangeCristalCount>.AddListener(OnChangeCristalCount);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgStartGameElementsHud>.RemoveListener(OnStartHud);
            MessageAgregator<MsgChangeCristalCount>.RemoveListener(OnChangeCristalCount);
        }

        private void OnChangeCristalCount(MsgChangeCristalCount obj)
        {
            txtCristalCount.text = obj.newCristalCount.ToString();
        }

        private void OnStartHud(MsgStartGameElementsHud obj)
        {
            container.SetActive(true);
            txtCristalCount.text = "x " + obj.countCristals;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    public struct MsgChangeCristalCount : IMessageBase
    {
        public int newCristalCount;
    }
}