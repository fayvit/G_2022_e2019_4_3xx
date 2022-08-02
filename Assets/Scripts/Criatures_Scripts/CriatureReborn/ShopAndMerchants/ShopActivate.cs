using FayvitCam;
using System.Collections;
using TextBankSpace;
using UnityEngine;

namespace Criatures2021
{
    public class ShopActivate : MerchantBase
    {
        [SerializeField] private TextKey keyFrasesDoShoping = TextKey.frasesDeShoping;
        [SerializeField,ArrayElementTitle("nomeDoItem")] private ItensParaVenda[] itensParaVenda;

        private LocalState state = LocalState.emEspera;
        private string[] frasesDeShoping;
        private enum LocalState
        { 
            emEspera,
            mensIniciais,
            escolhaInicial,
            talkActivate
        }
        // Use this for initialization
        void Start()
        {
            txtDeOpcoes = TextBank.RetornaListaDeTextoDoIdioma(TextKey.comprarOuVender).ToArray();
            frasesDeShoping = TextBank.RetornaListaDeTextoDoIdioma(keyFrasesDoShoping).ToArray();
            StartBase();
        }
        void EntraFrasePossoAjudar()
        {
            dispara.TurnPanels();
            dispara.StartShowMessage(frasesDeShoping[5], fotoDoNPC);
            state = LocalState.escolhaInicial;
        }


        // Update is called once per frame
        new void Update()
        {
            base.Update();
            switch (state)
            {
                case LocalState.mensIniciais:
                    CameraApplicator.cam.FocusInPoint(8, 2);
                    if (NPCfalasIniciais.Update(commands.confirmButton, commands.returnButton))
                    {

                        EntraFrasePossoAjudar();
                        LigarMenu();
                    }
                break;
                case LocalState.escolhaInicial:
                    EscolhaInicial();
                break;
                case LocalState.talkActivate:
                    if (NPC.Update(commands.confirmButton, commands.returnButton))
                    {
                        LigarMenu();
                        EntraFrasePossoAjudar();
                    }
                break;
            }
        }

        public override void FuncaoDoBotao()
        {
            BotaoShop();
        }

        public override void VoltarAoJogo()
        {
            state = LocalState.emEspera;
            base.VoltarAoJogo();
        }

        void BotaoShop()
        {
            BaseStartMerchant();
            state = LocalState.mensIniciais;
        }

        protected override void IniciarConversar()
        {
            base.IniciarConversar();
            state = LocalState.talkActivate;
        }



        protected override void OpcaoEscolhida(int x)
        {
            ContainerBasicMenu.instance.Menu.FinishHud();

            switch (x)
            {
                case 0:
                break;
                case 1:
                break;
                case 2:
                    IniciarConversar();
                break;
                case 3:
                    VoltarAoJogo();
                break;
            }
        }
    }

    [System.Serializable]
    public struct ItensParaVenda
    {
        public NameIdItem nomeDoItem;
        public int limitado;
        public int precoCustomizado;
    }
}