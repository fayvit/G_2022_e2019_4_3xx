using FayvitMessageAgregator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FayvitUI
{
    [System.Serializable]
    public class ColorGridMenu : BaseGridMenu
    {
        private Color[] colors;

        private float ultimoClick;
        private int ultimaEscolha;
        [SerializeField] private float doubleClickTime = .25f;


        public Color GetSelectedColor
        {
            get
            {
                AnImageOption[] umas = variableSizeContainer.GetComponentsInChildren<AnImageOption>();
                return umas[SelectedOption].OptionImage.color;
            }
        }

        public void SetActions(System.Action<int> action)
        {
            ThisAction += action;
        }

        public void StartHud(Color[] colors,int selectIndex=0)
        {
            this.ThisAction += ColorActions;
            this.colors = colors;

            if (colors.Length > 0)
                StartHud(colors.Length, ResizeUiType.grid,selectIndex);
            else
                aContainerItem.SetActive(false);

            SetLineRowLength();

        }

        public void ColorActions(int x)
        {
            Debug.Log("ColorActrions");
            Color C = colors[x];

            if (Mathf.Abs(Time.time - ultimoClick) < doubleClickTime && ultimaEscolha == x)
            {
                MessageAgregator<MsgSelectedColorByClick>.Publish(new MsgSelectedColorByClick()
                {
                    C = C
                });
            }
            else
            {
                ChangeSelectionTo(x);

                MessageAgregator<MsgChangeOptionUI>.Publish(new MsgChangeOptionUI()
                {
                    parentOfScrollRect = menuScrollRect.transform.parent.gameObject,
                    selectedOption = SelectedOption
                });
            }

            ultimoClick = Time.time;
            ultimaEscolha = x;
        }

        public override void SetContainerItem(GameObject G, int indice)
        {

            AnImageOption uma = G.GetComponent<AnImageOption>();

            Image I = uma.OptionImage;
            I.color = colors[indice];

            uma.SetarOpcoes(I.sprite, ThisAction);
        }
       
    }
}

public struct MsgSelectedColorByClick:IMessageBase
{
    public Color C;
}
