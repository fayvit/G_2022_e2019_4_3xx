using System.Collections;
using UnityEngine;
using FayvitMessageAgregator;
using FayvitCam;
using FayvitUI;
using FayvitBasicTools;
using TalkSpace;
using TextBankSpace;

namespace Criatures2021
{
    public abstract class MerchantBase : ButtonActivate
    {
        [SerializeField] private Transform charRef;

        [SerializeField] protected string ID;
        [SerializeField] protected ScheduledTalkManager NPC = null;
        [SerializeField] protected ScheduledTalkManager NPCfalasIniciais = null;
        [SerializeField] protected Sprite fotoDoNPC;

        protected CharacterManager manager;
        protected MsgSendExternalPanelCommand commands;
        protected TextDisplay dispara;
        protected string[] txtDeOpcoes;

        protected abstract void OpcaoEscolhida(int x);

        protected void StartBase()
        {
            if (StaticInstanceExistence<DisplayTextManager>.SchelduleExistence(StartBase, this, () => {
                return DisplayTextManager.instance;
            }))
            {
                textoDoBotao = TextBank.RetornaFraseDoIdioma(TextKey.textoBaseDeAcao);
                if (StaticInstanceExistence<IGameController>.SchelduleExistence(StartBase, this, () => { return AbstractGameController.Instance; }))
                {
                    dispara = DisplayTextManager.instance.DisplayText;
                    //t = TextBank.RetornaListaDeTextoDoIdioma(TextKey.primeiroArmagedom).ToArray();
                }
            }
        }
        protected virtual void OnValidate()
        {
            BuscadorDeID.Validate(ref ID, this);
            NPC.OnVallidate();
            NPCfalasIniciais.OnVallidate();
        }

        protected void EscolhaInicial()
        {
            CameraApplicator.cam.FocusInPoint(8, 2/*, -1, true*/);
            if (!dispara.LendoMensagemAteOCheia(commands.confirmButton))
            {

                ContainerBasicMenu.instance.Menu.ChangeOption(-commands.vChange);

                if (commands.confirmButton)
                    OpcaoEscolhida(ContainerBasicMenu.instance.Menu.SelectedOption);

            }

            if (commands.returnButton)
            {
                OpcaoEscolhida(txtDeOpcoes.Length - 1);
            }
        }
        protected virtual void IniciarConversar()
        {
            
            NPC.Start();
            SomDoIniciar();            

            MessageAgregator<MsgChangeShiftKey>.Publish(new MsgChangeShiftKey() { change = true, sKey = ID });

            FluxoDeBotao();

            NPC.IniciaConversa();

        }

        protected void LigarMenu()
        {
            ContainerBasicMenu.instance.SetPercentSizeInTheParent(.65f, .25f, .99f, .75f);
            ContainerBasicMenu.instance.Menu.StartHud(OpcaoEscolhida, txtDeOpcoes);
        }

        private void OnReceiveCommands(MsgSendExternalPanelCommand obj)
        {
            commands = obj;
        }

        public virtual void VoltarAoJogo()
        {
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.Book1
            });

            MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero()
            {
                myHero = manager.gameObject
            });

            MessageAgregator<FillTextDisplayMessage>.Publish();
            dispara.OffPanels();

            MessageAgregator<MsgSendExternalPanelCommand>.RemoveListener(OnReceiveCommands);

            MessageAgregator<MsgReturnGameElmentsHud>.Publish();
        }


        protected void BaseStartMerchant()
        {
            SomDoIniciar();
            commands = new MsgSendExternalPanelCommand();
            MessageAgregator<MsgSendExternalPanelCommand>.AddListener(OnReceiveCommands);

            FluxoDeBotao();

            CameraApplicator.cam.StartExibitionCam(transform, 1);
            MessageAgregator<MsgHideGameElmentsHud>.Publish();

            dispara.StartTextDisplay();
            manager = MyGlobalController.MainPlayer;

            CameraApplicator.cam.StartShowPointCamera(transform, new SinglePointCameraProperties()
            {
                velOrTimeFocus = 0.75f,
                withTime = true
            });

            Vector3 dir = charRef.parent.position - manager.transform.position;
            CharRotateTo.RotateDir(dir, manager.gameObject);
            CharRotateTo.RotateDir(-dir, charRef.parent.gameObject);

            NPCfalasIniciais.Start();
            NPCfalasIniciais.IniciaConversa();
        }
    }
}