using System.Collections.Generic;
using FayvitUI;
using FayvitBasicTools;
using TextBankSpace;
using UnityEngine;

namespace Criatures2021Hud
{
    [System.Serializable]
    public class TutoInfoMenuManager
    {
        [SerializeField] private TextAndImageMenu menu;
        [SerializeField] private TutoInfoMessage tutoMessage;

        private List<InfoMessageType> openableMessages;

        public void StartHud()
        {
            openableMessages = new List<InfoMessageType>();
            int num = System.Enum.GetValues(typeof(InfoMessageType)).Length;

            for (int i = 0; i < num; i++)
            {
                string s = ((InfoMessageType)i).ToString()+"open";
                if (AbstractGameController.Instance.MyKeys.VerificaAutoShift(s))
                {
                    openableMessages.Add((InfoMessageType)i);
                }
            }

            int index = menu.SelectedOption;
            menu.StartHud((int x) => { }, GetActiveNames(), GetActiveSprites(), selectIndex: index,pivotPos:PivotPosition.leftUp);
            tutoMessage.FillMessage(openableMessages[menu.SelectedOption],true);
        }

        private string[] GetActiveNames()
        {
            List<string> n = new List<string>();
            for (int i = 0; i < openableMessages.Count; i++)
            {
                TextKey key = StringForEnum.GetEnum<TextKey>(openableMessages[i].ToString() + "Info");
                n.Add(TextBank.RetornaListaDeTextoDoIdioma(key)[0]);
            }

            return n.ToArray();
        }

        private Sprite[] GetActiveSprites()
        {
            List<Sprite> n = new List<Sprite>();
            for (int i = 0; i < openableMessages.Count; i++)
            {
                
                n.Add(ResourcesFolders.GetMiniInfo(openableMessages[i]));
            }

            return n.ToArray();
        }

        public void FinishHud()
        {
            menu.FinishHud();
        }

        public bool Update(int change,bool retorno)
        {
            if (change != 0)
            {
                menu.ChangeOption(change);
                tutoMessage.FillMessage(openableMessages[menu.SelectedOption],true);
            }
            else if (retorno)
                return true;

            return false;
        }
    }
}
