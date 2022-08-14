using FayvitCam;
using FayvitBasicTools;
using System.Collections;
using TextBankSpace;
using UnityEngine;
using Criatures2021Hud;
using FayvitMessageAgregator;

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
            talkActivate,
            fraseDeCompra,
            menuDeCompra,
            oneMessageOpened,
            itemAmountOpened,
            fraseDeVenda,
            menuDeVenda
        }
        // Use this for initialization
        void Start()
        {
            if (StaticInstanceExistence<IGameController>.SchelduleExistence(Start, this, () => { return AbstractGameController.Instance; }))
            {
                KeyVar keyVar = AbstractGameController.Instance.MyKeys;
                for (int i = 0; i < itensParaVenda.Length; i++)
                {
                    if (itensParaVenda[i].limitado >= 0 && !keyVar.VerificaAutoShift(ID))
                    {
                        keyVar.MudaAutoCont(ID + "item" + i,
                            itensParaVenda[i].limitado > 0 ? itensParaVenda[i].limitado : -1);
                    }

                    itensParaVenda[i].limitado = keyVar.VerificaAutoCont(ID + "item" + i);

                }

                keyVar.MudaAutoShift(ID, true);

                txtDeOpcoes = TextBank.RetornaListaDeTextoDoIdioma(TextKey.comprarOuVender).ToArray();
                frasesDeShoping = TextBank.RetornaListaDeTextoDoIdioma(keyFrasesDoShoping).ToArray();
                StartBase();
            }
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
                case LocalState.fraseDeCompra:
                    if (!dispara.LendoMensagemAteOCheia(commands.confirmButton))
                    {
                        DadosDeJogador dados = MyGlobalController.MainPlayer.Dados;
                        ShopHudManager.instance.StartHudCompra(ID,OnChoisePurchase, dados.Cristais, itensParaVenda, dados.Itens);
                        state = LocalState.menuDeCompra;
                    }
                break;
                case LocalState.fraseDeVenda:
                    if (!dispara.LendoMensagemAteOCheia(commands.confirmButton))
                    {
                        DadosDeJogador dados = MyGlobalController.MainPlayer.Dados;
                        ShopHudManager.instance.StartHudVenda(OnChoiseSale, dados.Cristais,dados.Itens);
                        state = LocalState.menuDeVenda;
                    }
                break;
                case LocalState.menuDeVenda:
                    if (commands.confirmButton)
                        OnChoiseSale(ShopHudManager.instance.SelectedOption);
                    else if (commands.vChange != 0 || commands.hChange != 0)
                        ShopHudManager.instance.ChangeOption(commands.vChange, commands.hChange);
                    else if (commands.returnButton)
                    {
                        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                        {
                            sfxId = FayvitSounds.SoundEffectID.Book1
                        });
                        ShopHudManager.instance.FinishHud();
                        LigarMenu();
                        EntraFrasePossoAjudar();
                    }
                    break;
                case LocalState.menuDeCompra:
                    if (commands.confirmButton)
                        OnChoisePurchase(ShopHudManager.instance.SelectedOption);
                    else if (commands.vChange != 0 || commands.hChange != 0)
                        ShopHudManager.instance.ChangeOption(commands.vChange, commands.hChange);
                    else if (commands.returnButton)
                    {
                        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                        {
                            sfxId = FayvitSounds.SoundEffectID.Book1
                        });
                        ShopHudManager.instance.FinishHud();
                        LigarMenu();
                        EntraFrasePossoAjudar();
                    }
                break;
                case LocalState.oneMessageOpened:
                    AbstractGlobalController.Instance.OneMessage.ThisUpdate(commands.confirmButton || commands.returnButton);                    
                break;
                case LocalState.itemAmountOpened:
                    AmountHudManager.instance.UpdateHud(commands.confirmButton, commands.returnButton, commands.vChange, commands.hChange);
                break;
            }
        }

        private void OnChoiseSale(int obj)
        {
            ItemBase Ib = MyGlobalController.MainPlayer.Dados.Itens[obj];
            ShopHudManager.instance.ChangeInteractiveButtons(false);

            if (Ib.Item_Nature == ItemNature.chave||Ib.Valor<=0)
            {

                state = LocalState.oneMessageOpened;

                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                {
                    sfxId = FayvitSounds.SoundEffectID.Book1
                });
                AbstractGlobalController.Instance.OneMessage.StartMessagePanel(() =>
                {
                    MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                    {
                        sfxId = FayvitSounds.SoundEffectID.Book1
                    });

                    state = LocalState.menuDeVenda;
                },frasesDeShoping[11]);
            }
            else
            {

                int cristais = MyGlobalController.MainPlayer.Dados.Cristais;
                Sprite S = ResourcesFolders.GetMiniItem(Ib.ID);

                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                {
                    sfxId = FayvitSounds.SoundEffectID.Decision1
                });

                

                AmountHudManager.instance.StartHud(
                        Ib.Valor/2,
                        Ib.Estoque,
                        int.MaxValue,
                        S,
                        OnChoiseAmountSell,()=> {
                            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                            {
                                sfxId = FayvitSounds.SoundEffectID.Book1
                            });
                            ShopHudManager.instance.ChangeInteractiveButtons(true);
                            AmountHudManager.instance.FinishHud();
                            state = LocalState.menuDeVenda;
                        });

                state = LocalState.itemAmountOpened;
            }
        }

        private void OnChoiseAmountSell(int amount, int val)
        {
            AmountHudManager.instance.FinishHud();

            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.Shop
            });

            DadosDeJogador dados = MyGlobalController.MainPlayer.Dados;

            ItemBase.RetirarUmItem(dados.Itens, dados.Itens[ShopHudManager.instance.SelectedOption].ID, amount);

            dados.AdicionarCristais(val);

            MessageAgregator<MsgChangeCristalCount>.Publish(new MsgChangeCristalCount()
            {
                newCristalCount = dados.Cristais
            });

            if (dados.Itens.Count > 0)
            {
                state = LocalState.menuDeVenda;
                
                ShopHudManager.instance.ChangeInteractiveButtons(true);
                int x = dados.Itens.Count> ShopHudManager.instance.SelectedOption? ShopHudManager.instance.SelectedOption:0;
                ShopHudManager.instance.FinishHud();
                ShopHudManager.instance.StartHudVenda(OnChoiseSale, dados.Cristais, dados.Itens);
            }
            else
            {
                ShopHudManager.instance.FinishHud();
                LigarMenu();
                EntraFrasePossoAjudar();
            }
        }

        private void OnChoisePurchase(int obj)
        {
            KeyVar k = AbstractGameController.Instance.MyKeys;
            ItensParaVenda I = itensParaVenda[obj];
            ItemBase Ib = ItemFactory.Get(I.nomeDoItem, I.limitado > 0 ? k.VerificaAutoCont(ID + "item" + obj) : 99);
            
            int cristais = MyGlobalController.MainPlayer.Dados.Cristais;

            bool estaNoEstoque = (k.VerificaAutoCont(ID + "item" + obj) <= I.limitado
                    && k.VerificaAutoCont(ID + "item" + obj) > 0) || I.limitado <= 0;

            int preco = I.precoCustomizado > 0 ? I.precoCustomizado :  Ib.Valor;
            bool temDinheiro = cristais >= preco;
            bool maisQueUm = cristais / preco > 1;

            if (estaNoEstoque && temDinheiro && maisQueUm)
            {
                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                {
                    sfxId = FayvitSounds.SoundEffectID.Decision1
                });

                ShopHudManager.instance.ChangeInteractiveButtons(false);
                Sprite S = ResourcesFolders.GetMiniItem(I.nomeDoItem);

                AmountHudManager.instance.StartHud(
                    I.precoCustomizado > 0 ? I.precoCustomizado : Ib.Valor,
                    Ib.Estoque,
                    cristais,
                    S,
                    OnChoiseAmountBuy,()=> {
                        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                        {
                            sfxId = FayvitSounds.SoundEffectID.Book1
                        });

                        ShopHudManager.instance.ChangeInteractiveButtons(true);
                        AmountHudManager.instance.FinishHud();
                        state = LocalState.menuDeCompra;
                    });

                state = LocalState.itemAmountOpened;
            }
            else if (!estaNoEstoque)
            {
                AbrirMensagemSimples(frasesDeShoping[6]);
            }
            else if (!temDinheiro)
            {
                AbrirMensagemSimples(frasesDeShoping[10]);
            }
            else if (!maisQueUm)
            {
                OnChoiseAmountBuy(1, preco);
            }
        }

        void AbrirMensagemSimples(string frase)
        {
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.painelAbrindo
            });

            ShopHudManager.instance.ChangeInteractiveButtons(false);
            state = LocalState.oneMessageOpened;
            AbstractGlobalController.Instance.OneMessage.StartMessagePanel(() =>
            {
                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                {
                    sfxId = FayvitSounds.SoundEffectID.Book1
                });

                ShopHudManager.instance.ChangeInteractiveButtons(true);
                state = LocalState.menuDeCompra;
            },
            frase
            );
        }

        private void OnChoiseAmountBuy(int amount,int value)
        {
            AmountHudManager.instance.FinishHud();

            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.Shop
            });

            DadosDeJogador dados = MyGlobalController.MainPlayer.Dados;
            dados.AdicionaItem(itensParaVenda[ShopHudManager.instance.SelectedOption].nomeDoItem, amount);
            dados.RemoverCristais(value);

            MessageAgregator<MsgChangeCristalCount>.Publish(new MsgChangeCristalCount()
            {
                newCristalCount = dados.Cristais
            });

            RemoveItemDoEstoque(amount);

            state = LocalState.menuDeCompra;
            ShopHudManager.instance.ChangeInteractiveButtons(true);
            int x = ShopHudManager.instance.SelectedOption;

            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.Book1
            });

            ShopHudManager.instance.FinishHud();
            ShopHudManager.instance.StartHudCompra(ID,OnChoisePurchase, dados.Cristais, itensParaVenda, dados.Itens,selecionado:x);

        }

        private void RemoveItemDoEstoque(int value)
        {
            int x = ShopHudManager.instance.SelectedOption;
            if (itensParaVenda[x].limitado > 0)
            {
                MyGameController.Instance.MyKeys.SomaAutoCont(ID + "item" + x, -value);
            }
        }

        public override void FuncaoDoBotao()
        {
            BotaoShop();
        }

        public override void VoltarAoJogo()
        {
            //MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            //{
            //    sfxId = FayvitSounds.SoundEffectID.Book1
            //});

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
                    IniciarComprarVender(LocalState.fraseDeCompra,frasesDeShoping[0]);
                break;
                case 1:
                    if (MyGlobalController.MainPlayer.Dados.Itens.Count>0)
                        IniciarComprarVender(LocalState.fraseDeVenda, frasesDeShoping[1]);
                    else
                    {
                        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                        {
                            sfxId = FayvitSounds.SoundEffectID.painelAbrindo
                        });

                        state = LocalState.oneMessageOpened;
                        AbstractGlobalController.Instance.OneMessage.StartMessagePanel(() =>
                        {
                            LigarMenu();
                            EntraFrasePossoAjudar();
                        }, frasesDeShoping[12]);
                    }
                break;
                case 2:
                    IniciarConversar();
                break;
                case 3:
                    VoltarAoJogo();
                break;
            }
        }

        private void IniciarComprarVender(LocalState state,string frase)
        {
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.Decision1
            });

            dispara.TurnPanels();
            dispara.StartShowMessage(frase/*frasesDeShoping[0]*/, fotoDoNPC);
            this.state = state;// LocalState.fraseDeCompra;
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