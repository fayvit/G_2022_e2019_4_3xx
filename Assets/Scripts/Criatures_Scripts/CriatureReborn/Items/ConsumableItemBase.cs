using UnityEngine;
using FayvitMessageAgregator;
using FayvitSupportSingleton;
using System.Collections.Generic;
using Criatures2021Hud;
using TextBankSpace;
using FayvitBasicTools;

namespace Criatures2021
{
    [System.Serializable]
    public class ConsumableItemBase : ItemBase
    {
        [System.NonSerialized] protected PetManager CriatureAlvoDoItem;
        protected bool confirmarRetorno;
        protected bool eMenu;
        private const float TEMPO_DE_ANIMA_CURA_1 = 1.5f;

        public ConsumableItemBase(ItemFeatures C) : base(C) { }

        

        public override void IniciaUsoDeMenu(GameObject dono,List<ItemBase> lista)
        {
            Dono = dono;
            Lista = lista;
            eMenu = true;

            MessageAgregator<MsgMenuStartUseItem>.Publish(new MsgMenuStartUseItem()
            {
                response = MsgMenuStartUseItem.StartUseResponse.escolhaEmQuemUsar
            });

            MessageAgregator<MsgChoseItemTarget>.AddListener(OnChosenTarget);
        }

        private void OnChosenTarget(MsgChoseItemTarget obj)
        {
            EscolhiEmQuemUsar(obj.indexOfTarget);
            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<MsgChoseItemTarget>.RemoveListener(OnChosenTarget);
            });
        }

        public override void RetornaSemUsarComMenu()
        {
            MessageAgregator<MsgChoseItemTarget>.RemoveListener(OnChosenTarget);
        }

        void VerificaTemMaisParaUsar()
        {
            Debug.LogError("Verifica tem mais pra usar removido");

            //GameController g = GameController.g;
            //if (g.Manager.Dados.TemItem(ID) <= 0)
            //    g.HudM.P_EscolheUsoDeItens.VoltarDosItens();
        }

        protected virtual void EscolhiEmQuemUsar(int indice)
        {
            throw new System.NotImplementedException();
        }

        protected void EscolhiEmQuemUsar(
            int indice,
            bool vaiUsar,
            bool tipoCerto,
            int valor = 0,
            int corrente = 0,
            int maximo = 0,
            PetTypeName recuperaDoTipo = PetTypeName.nulo)
        {
            //CharacterManager manager = GameController.g.Manager;
            //CriatureBase C = manager.Dados.CriaturesAtivos[indice];

            PetBase pb = Dono.GetComponent<CharacterManager>().Dados.CriaturesAtivos[indice];

            if (vaiUsar && tipoCerto)
            {
                if (Consumivel)
                    RetirarUmItem(Lista, this, 1);

                AcaoDoItemConsumivel(indice);

                //Debug.LogError("Aplicação do item com menu não implementado");


                ItemBase refi = ProcuraItemNaLista(ID, Lista);

                if(!confirmarRetorno)
                    MessageAgregator<MsgUsingQuantitativeItem>.Publish(new MsgUsingQuantitativeItem()
                    {
                        temMaisParausar = refi.ID == ID && refi.Estoque > 0
                    });
                

                //ItemQuantitativo.AplicacaoDoItemComMenu(manager, C, valor, VerificaTemMaisParaUsar);

            }
            else if (!tipoCerto)
            {
                MessageAgregator<MsgNotUseItem>.Publish(new MsgNotUseItem()
                {
                    notMessage = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.itens)[3], TypeNameInLanguages.Get(recuperaDoTipo))
                });

                //Debug.LogError("Mensagem de item não implementado");
                //MensDeUsoDeItem.MensNaoTemOTipo(recuperaDoTipo.ToString());
            }

            else if (corrente <= 0)
            {
                MessageAgregator<MsgNotUseItem>.Publish(new MsgNotUseItem()
                {
                    notMessage = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.itens)[2], pb.GetNomeEmLinguas)
                });
                //MensDeUsoDeItem.MensDeMorto(C.NomeEmLinguas);
            }
            else if (corrente >= maximo)
            {

                MessageAgregator<MsgNotUseItem>.Publish(new MsgNotUseItem()
                {
                    notMessage = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.itens)[9], pb.GetNomeEmLinguas)
                });

                //MensDeUsoDeItem.MensDeNaoPrecisaDesseItem(C.NomeEmLinguas);
            }
        }


        protected void IniciaUsoDesseItem(GameObject dono, bool podeUsar, bool temTipo = true, PetTypeName nomeDoTipo = PetTypeName.nulo)
        {
            //Manager = GameController.g.Manager;
            CriatureAlvoDoItem = dono.GetComponent<CharacterManager>().ActivePet;
            Transform pet = CriatureAlvoDoItem.transform;
            if (podeUsar && temTipo && RetirarUmItem(Lista, this, 1))
            {
                //GameController.g.HudM.ModoCriature(false);
                InicializacaoComum(dono, pet);
                Estado = ItemUseState.animandoBraco;
            }
            else
            {
                Estado = ItemUseState.finalizaUsaItem;
                if (!temTipo)
                {
                    Debug.LogError("Uma mensagem de não tem tipo");

                    MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
                    {
                        message = string.Format(TextBank.RetornaFraseDoIdioma(TextKey.itens),
                            "<color = orange>"+nomeDoTipo+"</color>")
                    });

                    //GameController.g.HudM.Painel.AtivarNovaMens(string.Format(
                    //   BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.itens)[3], nomeDoTipo), 30, 5);
                }
                else if (!podeUsar)
                {
                    //Feito
                    //Debug.LogError("Uma mensagem de não pode usar");

                    MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
                    {
                        message = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.mensLuta)[2],
                        CriatureAlvoDoItem.MeuCriatureBase.GetNomeEmLinguas
                    )
                    });
                    //GameController.g.HudM.Painel.AtivarNovaMens(string.Format(
                    //BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.mensLuta)[2],
                    //    CriatureAlvoDoItem.MeuCriatureBase.NomeEmLinguas), 30, 5);
                }
            }
        }

        protected bool AtualizaUsoDesseItem(GeneralParticles particula,FayvitSounds.SoundEffectID sfxId)
        {
            switch (Estado)
            {
                case ItemUseState.animandoBraco:
                    if (!AnimaB.AnimaTroca(true))
                    {
                        Estado = ItemUseState.aplicandoItem;

                        MessageAgregator<MsgRequestEndArmsAnimations>.Publish(new MsgRequestEndArmsAnimations()
                        {
                            oAnimado = Dono
                        });

                        //Manager.Mov.Animador.ResetaTroca();

                        //Debug.LogError("Tudo aqui pode ser substituido por um evento enviado ao manager");
                        InstanceSupport.InstancieEDestrua(particula, AnimaB.PosCriature, 1);
                        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                        {
                            sfxId = sfxId
                        });
                        //AuxiliarDeInstancia.InstancieEDestrua(particula, CriatureAlvoDoItem.transform.position, 1);
                        AcaoDoItemConsumivel(0);


                        //GameController.g.HudM.AtualizeImagemDeAtivos();
                        //GameController.g.HudM.AtualizaDadosDaHudVida(false);
                    }
                    break;
                case ItemUseState.aplicandoItem:
                    TempoDecorrido += Time.deltaTime;
                    if (TempoDecorrido > TEMPO_DE_ANIMA_CURA_1)
                    {

                        //GameController.g.HudM.AtualizaHudHeroi(CriatureAlvoDoItem.MeuCriatureBase);
                        Estado = ItemUseState.finalizaUsaItem;
                        return false;
                    }
                    break;
                case ItemUseState.finalizaUsaItem:
                    return false;
                //break;
                case ItemUseState.nulo:
                    Debug.Log("alcançou estado nulo para " + ID.ToString());
                break;
            }
            return true;
        }

        public virtual void AcaoDoItemConsumivel(int indice)
        {
            throw new System.NotImplementedException();
        }
    }

    public struct MsgMenuStartUseItem : IMessageBase
    {
        public enum StartUseResponse
        {
            usar,
            escolhaEmQuemUsar,
            naoPodeUsar
        }

        public StartUseResponse response;
    }

    public struct MsgChoseItemTarget : IMessageBase
    {
        public int indexOfTarget;
    }

}