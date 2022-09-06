using UnityEngine;
using System.Collections;
using FayvitUI;
using System;
using UnityEngine.UI;
using FayvitSupportSingleton;
using FayvitMessageAgregator;

namespace CustomizationSpace
{
    public class A_CustomizationOption : AnOption, ICustomizationMenuBase
    {
        [SerializeField] private Text txtDaOpcao;
        [SerializeField] private GameObject controlButtonsContainer;
        [SerializeField] private Image viewColorImg;

        private SectionCustomizationManager scm;

        private Action<int, int> horizontalChange;
        private Action<int> returnAction;

        void Start()
        {
            MessageAgregator<MsgSendApplyColorToHud>.AddListener(OnReceiveApplyColor);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgSendApplyColorToHud>.RemoveListener(OnReceiveApplyColor);
        }

        private void OnReceiveApplyColor(MsgSendApplyColorToHud obj)
        {
            if (transform.GetSiblingIndex() - 1 == obj.indice)
            {
                viewColorImg.color = obj.cor;
            }
        }

        public virtual void SetOptions(
            Action<int> mainAction,
            Action<int> returnAction,
            Action<int, int> horizontalChange,
            CustomizationManagerMenu.EditableElements opcao,
            SectionCustomizationManager scm
            )
        {

            this.scm = scm;
            ThisAction += mainAction;
            this.horizontalChange += horizontalChange;
            this.returnAction += returnAction;
            txtDaOpcao.text = opcao.displayName;

            switch (opcao.type)
            {
                case CustomizationManagerMenu.EditableType.color:
                    SupportSingleton.Instance.InvokeOnEndFrame(() =>
                    {
                        viewColorImg.transform.parent.gameObject.SetActive(true);

                        Color C = scm.GetColorOfMember(opcao.member, opcao.inIndex);
                        C.a = 1;
                        viewColorImg.color = C;
                    });
                    break;
                case CustomizationManagerMenu.EditableType.mesh:
                case CustomizationManagerMenu.EditableType.texture:
                    controlButtonsContainer.SetActive(true);
                    break;
            }
        }

        public void HorizontalChange(int change)
        {
            horizontalChange?.Invoke(change, transform.GetSiblingIndex() - 1);
        }

        public void ReturnInvoke()
        {
            returnAction?.Invoke(transform.GetSiblingIndex() - 1);
        }

    }
}