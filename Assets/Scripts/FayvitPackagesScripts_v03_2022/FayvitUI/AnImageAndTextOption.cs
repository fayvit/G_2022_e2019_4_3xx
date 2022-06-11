using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace FayvitUI
{
    public class AnImageAndTextOption : AnOption
    {
        [SerializeField] private Text optionText;
        [SerializeField] private Image optionImg;

        protected Text OptionText
        {
            get { return optionText; }
            set { optionText = value; }
        }

        public virtual void SetarOpcao(System.Action<int> optionAction, string optionText,Sprite imgOption)
        {
            ThisAction += optionAction;
            OptionText.text = optionText;
            optionImg.sprite = imgOption;
        }
    }
}