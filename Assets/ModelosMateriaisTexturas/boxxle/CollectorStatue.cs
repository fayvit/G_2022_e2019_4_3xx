using FayvitBasicTools;
using FayvitMessageAgregator;
using System;
using System.Collections;
using TextBankSpace;
using UnityEngine;

namespace Criatures2021
{
    public class CollectorStatue : ButtonActivate
    {
        [SerializeField] private string ID;
        [SerializeField] private int quantidade = 3;
        [SerializeField] private NameIdItem itemCusto = NameIdItem.brasaoDeLaurense;

        private string[] godPuzzleMessages;
        private LocalState state = LocalState.emEspera;
        private MsgSendExternalPanelCommand externalCommand = new MsgSendExternalPanelCommand();
        private enum LocalState
        { 
            emEspera,
            confirmationOpened,
            oneMessageOpened,
        }

        void Start()
        {
            if (StaticInstanceExistence<IGameController>.SchelduleExistence(Start, this, () =>
            {
                return AbstractGameController.Instance;
            }))
            {
                if (AbstractGameController.Instance.MyKeys.VerificaAutoShift(ID))
                    gameObject.SetActive(false);
                else
                    godPuzzleMessages = TextBank.RetornaListaDeTextoDoIdioma(TextKey.godPushPuzzleActivate).ToArray();

                textoDoBotao = TextBank.RetornaListaDeTextoDoIdioma(TextKey.textoBaseDeAcao)[1];
                SempreEstaNoTrigger();
            }
            
            
            
        }

        private void OnReceiveCommands(MsgSendExternalPanelCommand obj)
        {
            externalCommand = obj;
        }

        private void OnValidate()
        {
            BuscadorDeID.Validate(ref ID, this);
        }

        new void Update()
        {
            base.Update();

            switch (state)
            {
                case LocalState.confirmationOpened:
                    AbstractGlobalController.Instance.Confirmation.ThisUpdate(
                        externalCommand.hChange!=0, 
                        externalCommand.confirmButton, 
                        externalCommand.returnButton);
                break;
                case LocalState.oneMessageOpened:
                    AbstractGlobalController.Instance.OneMessage.ThisUpdate(externalCommand.confirmButton||externalCommand.returnButton);
                break;
            }
        }

        private void UsarBrasoes()
        {
            DadosDeJogador dados = MyGlobalController.MainPlayer.Dados;
            ItemBase I = ItemBase.ProcuraItemNaLista(itemCusto,dados.Itens);
            if (I.ID != NameIdItem.generico && I.Estoque >= quantidade)
            {
                ItemBase.RetirarUmItem(dados.Itens, itemCusto, quantidade);
                InstanceSupport.InstancieEDestrua(AttackNameId.teletransporte, transform.position, default, 10);

                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                {
                    sfxId = FayvitSounds.SoundEffectID.Twine
                });

                MessageAgregator<MsgChangeShiftKey>.Publish(new MsgChangeShiftKey()
                {
                    sKey = ID,
                    change = true
                });

                FayvitSupportSingleton.SupportSingleton.Instance.InvokeInSeconds(Finalizar, 1);

                state = LocalState.emEspera;
                Destroy(gameObject);
            }
            else
            {
                state = LocalState.oneMessageOpened;
                
                AbstractGlobalController.Instance.OneMessage.StartMessagePanel(Finalizar,
                    string.Format(godPuzzleMessages[4], quantidade, ItemBase.NomeEmLinguas(itemCusto)));
            }
        }

        private void Finalizar()
        {
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.Book1
            });
            MessageAgregator<MsgSendExternalPanelCommand>.RemoveListener(OnReceiveCommands);
            MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero()
            {
                myHero = MyGlobalController.MainPlayer.gameObject
            });
            state = LocalState.emEspera;
        }

        public override void FuncaoDoBotao()
        {
            SomDoIniciar();
            FluxoDeBotao();
            externalCommand = new MsgSendExternalPanelCommand();
            MessageAgregator<MsgSendExternalPanelCommand>.AddListener(OnReceiveCommands);

            
            Debug.Log(AbstractGlobalController.Instance);
            Debug.Log(" : " + godPuzzleMessages + " : "+ (1 + (((int)itemCusto) % 3)) +" : "+ godPuzzleMessages[1 + (((int)itemCusto) % 3)]);

            AbstractGlobalController.Instance.Confirmation.StartConfirmationPanel(UsarBrasoes, Finalizar,
                string.Format(godPuzzleMessages[0], quantidade, ItemBase.NomeEmLinguas(itemCusto), godPuzzleMessages[1 + (((int)itemCusto) % 3)])
                );

            state = LocalState.confirmationOpened;
        }
    }
}