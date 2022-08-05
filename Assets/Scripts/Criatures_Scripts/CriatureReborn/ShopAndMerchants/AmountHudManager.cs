using FayvitBasicTools;
using System.Collections;
using TextBankSpace;
using UnityEngine;
using UnityEngine.UI;

namespace Criatures2021Hud
{
    public class AmountHudManager : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private InputField input;
        [SerializeField] private Image imgOfItem;
        [SerializeField] private Text txtCost;

        private int value = 1;
        private int maxValueLimit = 100;
        private int costPerUnit = 1;
        private int maxCost = 1000;
        private LocalState state = LocalState.onUpdate;

        private System.Action<int,int> action;
        private System.Action onCancel;

        public static AmountHudManager instance;

        private enum LocalState
        { 
            onUpdate,
            oneMessageOpened
        }

        // Use this for initialization
        void Start()
        {
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool Limit(int x)
        {
            if (maxValueLimit > 0)
                return x <= maxValueLimit;
            else
                return true;
        }

        void ChangeValue(int change)
        {
            if (value + change > 0 && Limit(value + change) && (value + change) * costPerUnit <= maxCost)
            {
                value += change;
            }
            else if (value + change <= 0)
            {
                value = 1;
            }
            else if (!Limit(value + change) && maxValueLimit * costPerUnit <= maxCost)
            {
                value = maxValueLimit;
            }
            else if ((value + change) * costPerUnit > maxCost)
            {
                value = maxCost / costPerUnit;
            }

            input.text = value.ToString();
            txtCost.text = (value * costPerUnit).ToString();
        }

        void ChangeValue_b(int change)
        {
            if (value + change > 0 && Limit(value + change) && (value + change) * costPerUnit <= maxCost)
            {
                value += change;
                
            }
            else if (value + change <= 0)
            {
                value = 1;
                state = LocalState.oneMessageOpened;
                AbstractGlobalController.Instance.OneMessage.StartMessagePanel(() =>
                {
                    
                    state = LocalState.onUpdate;
                }, 
                TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDeShoping)[7]);
            }
            else if (!Limit(value + change)&& (value + change) * costPerUnit <= maxCost)
            {
                state = LocalState.oneMessageOpened;
                value = maxValueLimit;
                AbstractGlobalController.Instance.OneMessage.StartMessagePanel(() =>
                {
                    state = LocalState.onUpdate;
                    
                },
                string.Format(
                        TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDeShoping)[8], maxValueLimit));
            } else if ((value + change) * costPerUnit > maxCost)
            {
                state = LocalState.oneMessageOpened;
                value = maxCost / costPerUnit;
                AbstractGlobalController.Instance.OneMessage.StartMessagePanel(() =>
                {
                    state = LocalState.onUpdate;
                },
                string.Format(
                            TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDeShoping)[9], maxCost/costPerUnit));
            }

            input.text = value.ToString();
            txtCost.text = (value * costPerUnit).ToString();
        }

        public void StartHud(int costPerUnit,int limit,int maxCost,Sprite imgOfItem,System.Action<int,int> A,System.Action cancelAction)
        {
            container.SetActive(true);

            value = 1;
            maxValueLimit = limit;
            action += A;
            onCancel += cancelAction;

            this.costPerUnit = costPerUnit;
            this.imgOfItem.sprite = imgOfItem;
            this.maxCost = maxCost;

            input.text = value.ToString();
            txtCost.text = (value * costPerUnit).ToString();   
        }

        public void UpdateHud(bool confirm,bool retorno,int vchange,int hchange)
        {
            switch (state)
            {
                case LocalState.onUpdate:

                    if (hchange > 0)
                        ButtonPlusOne();
                    else if (hchange < 0)
                        ButtonMinusOne();
                    else if (vchange > 0)
                        ButtonPlusTen();
                    else if (vchange < 0)
                        ButtonMinusTen();
                    else if (confirm)
                    {
                        if (value * costPerUnit <= maxCost)
                            action(value, value * costPerUnit);
                        else
                        {
                            state = LocalState.oneMessageOpened;
                        }
                    }
                    else if (retorno)
                        onCancel();
                    

                break;
                case LocalState.oneMessageOpened:
                    AbstractGlobalController.Instance.OneMessage.ThisUpdate(confirm || retorno);
                break;
            }
        }

        public void ButtonPlusOne()
        {
            ChangeValue(1);
        }

        public void ButtonPlusTen()
        {
            ChangeValue(10);
        }

        public void ButtonMinusOne()
        {
            ChangeValue(-1);
        }
        public void ButtonMinusTen()
        {
            ChangeValue(-10);
        }

        public void OnChangeInputField(string s)
        {
            int x;
            int.TryParse(s, out x);
            if (x != 0)
                value = x;
            input.text = value.ToString();
            txtCost.text = (value * costPerUnit).ToString();
        }

        public void FinishHud()
        {
            container.SetActive(false);
            action = null;
            onCancel = null;
        }

    }
}