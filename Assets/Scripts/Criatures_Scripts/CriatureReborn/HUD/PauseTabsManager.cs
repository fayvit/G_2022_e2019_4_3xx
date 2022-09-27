using UnityEngine;
using FayvitUI;
using FayvitMessageAgregator;

namespace Criatures2021Hud
{
    public class PauseTabsManager : MonoBehaviour
    {
        [SerializeField] private PauseTabMenu tabMenu;
        [SerializeField] private Sprite[] basicSprites;

        // Use this for initialization
        void Start()
        {
            MessageAgregator<MsgRequestChangeTab>.AddListener(OnRequestChangeTab);
            MessageAgregator<MsgFinishStatsMenu>.AddListener(OnFinishStatsMenu);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgRequestChangeTab>.RemoveListener(OnRequestChangeTab);
            MessageAgregator<MsgFinishStatsMenu>.RemoveListener(OnFinishStatsMenu);
        }

        private void OnFinishStatsMenu(MsgFinishStatsMenu obj)
        {
            ChangeToDefaulState();
        }

        public void ChangeToDefaulState()
        {
            tabMenu.FinishHud();
            tabMenu.StartHud(basicSprites, (int x) => {
                MessageAgregator<MsgActionTabMenu>.Publish(new MsgActionTabMenu()
                {
                    index = x
                });
            });
        }

        public void ChangeInteractiveButtons(bool b)
        {
            tabMenu.ChangeInteractiveButtons(b);
        }


        private void OnRequestChangeTab(MsgRequestChangeTab obj)
        {
            tabMenu.ChangeSelectionTo(obj.indexOfSelection);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ReestartTabsManager(Sprite[] dasAbas,System.Action<int> action,int indexOfSelection)
        {
            tabMenu.FinishHud();
            tabMenu.StartHud(dasAbas,action,indexOfSelection);
        }

        internal void FinishTabMenu()
        {
            tabMenu.FinishHud();
        }

        public void VerifyChangeTab(bool plus, bool minus)
        {
            if (plus || minus)
            {
                tabMenu.ChangeOption(plus ? 1 : minus ? -1 : 0);
                MessageAgregator<MsgActionTabMenu>.Publish(new MsgActionTabMenu()
                {
                    index = tabMenu.SelectedOption
                });
            }
        }
    }

    [System.Serializable]
    public class PauseTabMenu : InteractiveUiBase
    {
        [SerializeField] private Sprite[] sprites;

        private System.Action<int> action;

        public void StartHud(Sprite[] sprites,System.Action<int> action,int indexOfSelection=0)
        {
            this.action += ActionToDelayButton(action);
            this.sprites = sprites;
            StartHud(sprites.Length, ResizeUiType.horizontal,indexOfSelection);
            variableSizeContainer.pivot = Vector3.zero;
        }

        public override void SetContainerItem(GameObject G, int indice)
        {
            AnImageOption option = G.GetComponent<AnImageOption>();
            option.SetarOpcoes(sprites[indice],action);
            
        }

        protected override void AfterFinisher()
        {
            action = null;
        }
    }

    public struct MsgRequestChangeTab : IMessageBase
    {
        public int indexOfSelection;
    }

    public struct MsgActionTabMenu : IMessageBase {
        public int index;
    }
}