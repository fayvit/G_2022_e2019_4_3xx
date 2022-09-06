using Criatures2021;
using UnityEngine;
using FayvitUI;
using FayvitMessageAgregator;
using System.Collections.Generic;
using FayvitBasicTools;

namespace Criatures2021Hud
{
    public class StatsMenuManager:MonoBehaviour
    {
        [SerializeField] private StatsMenu sMenu;

        private int indiceAtualDoSelecionado;
        private List<PetBase> osPets;
        private MsgSendExternalPanelCommand cmd;

        private void Start()
        {
            MessageAgregator<MsgRequestStatsMenu>.AddListener(OnRequestStatsMenu);
            
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgRequestStatsMenu>.RemoveListener(OnRequestStatsMenu);
            

        }

        private void OnReceiveExternalCommand(MsgSendExternalPanelCommand obj)
        {
            cmd = obj;
        }

        private void OnRequestStatsMenu(MsgRequestStatsMenu obj)
        {
            indiceAtualDoSelecionado = obj.indiceSelecionado;
            FinishThis();
            sMenu.StartHud(obj.petList[obj.indiceSelecionado]);
            osPets = obj.petList;
            cmd = new MsgSendExternalPanelCommand();
            MessageAgregator<MsgSendExternalPanelCommand>.AddListener(OnReceiveExternalCommand);
        }

        private void Update()
        {
            
            int hChange = cmd.hChange + (cmd.rightChangeButton ? 1 : (cmd.leftChangeButton ? -1 : 0));

            if (hChange!=0)
            {
                indiceAtualDoSelecionado = ContadorCiclico.Contar(hChange, indiceAtualDoSelecionado, osPets.Count);
                MessageAgregator<MsgRequestChangeTab>.Publish(new MsgRequestChangeTab()
                {
                    indexOfSelection = indiceAtualDoSelecionado
                });

                sMenu.FinishHud();
                sMenu.StartHud(osPets[indiceAtualDoSelecionado]);
                
            }if (cmd.vChange != 0)
            {

                sMenu.ChangeOption(-cmd.vChange, clamp: true);
            }
            else if (cmd.returnButton)
            {
                FinishThis();
                MessageAgregator<MsgFinishStatsMenu>.Publish();
            }
        }

        void FinishThis()
        {
            sMenu.FinishHud();
            MessageAgregator<MsgSendExternalPanelCommand>.RemoveListener(OnReceiveExternalCommand);
            cmd = new MsgSendExternalPanelCommand();
        }
    }

    public struct MsgFinishStatsMenu : IMessageBase { }

    public struct MsgRequestStatsMenu : IMessageBase
    {
        public List<PetBase> petList;
        public int indiceSelecionado;
    }

    [System.Serializable]
    public class StatsMenu : InteractiveUiBase
    {
        private PetBase targetPet;
        public void StartHud(PetBase targetPet)
        {
            this.targetPet = targetPet;
            int quantidade = targetPet.GerenteDeGolpes.meusGolpes.Count + 1;
            StartHud(quantidade);
        }
        public override void SetContainerItem(GameObject G, int indice)
        {
            ShowPetBase showPet = G.GetComponentInChildren<ShowPetBase>();
            ShowAttackOption atkOpt = G.GetComponentInChildren<ShowAttackOption>();
            if (indice == 0)
            {
                atkOpt.gameObject.SetActive(false);
                showPet.InserirDadosNoPainelPrincipal(targetPet);
            }
            else
            {
                PetAttackBase petAtk = targetPet.GerenteDeGolpes.meusGolpes[indice - 1];
                float mod = targetPet.GerenteDeGolpes.ProcuraGolpeNaLista(targetPet.NomeID, petAtk.Nome).ModPersonagem;
                atkOpt.InsertAttackDetails(petAtk, mod);
                showPet.gameObject.SetActive(false);
            }
        }

        protected override void AfterFinisher()
        {
            
        }
    }
}
