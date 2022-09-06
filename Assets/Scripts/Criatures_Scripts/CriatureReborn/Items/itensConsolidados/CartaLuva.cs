using UnityEngine;
using System.Collections.Generic;
using FayvitMessageAgregator;
using Criatures2021Hud;
using TextBankSpace;
using FayvitCam;

namespace Criatures2021
{
    [System.Serializable]
    public class CartaLuva : ItemBase
    {
        [System.NonSerialized] private CaptureManager animaCap;
        private bool captura;
        private CaptureTax taxOfcapture = new CaptureTax()
        {
            interpolateIndex = 1,
            maxHp = .5f,
            maxTax = .8f,
            taxOnMaxHp = 0
        };

        [System.Serializable]
        private struct CaptureTax
        {
            public float maxTax;
            public float maxHp;
            public float taxOnMaxHp;
            /// <summary>
            /// usado para saber se a interpolação será quadratica , cubica, linear, raiz e etc...
            /// </summary>
            public float interpolateIndex;
        }

        public CartaLuva(int estoque = 1) : base(new ItemFeatures(NameIdItem.cartaLuva)
        {
            valor = 12
        }
            )
        {
            Estoque = estoque;
        }

        public override void IniciaUsoDeMenu(GameObject dono,List<ItemBase> lista)
        {
            #region estavaAntigamente
            //base.IniciaUsoDeMenu(dono, lista);
            //Estado = ItemUseState.emEspera;

            //Debug.LogError("painel de info na carta luva menu");
            //MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
            //{
            //    message = TextBank.RetornaFraseDoIdioma(TextKey.mensLuta)
            //}) ;
            ////GameController.g.HudM.UmaMensagem.ConstroiPainelUmaMensagem(FecharMensagem, BancoDeTextos.RetornaFraseDoIdioma(ChaveDeTexto.mensLuta));
            #endregion

            MessageAgregator<MsgMenuStartUseItem>.Publish(new MsgMenuStartUseItem()
            {
                response = MsgMenuStartUseItem.StartUseResponse.naoPodeUsar
            });
        }



        public override void IniciaUsoComCriature(GameObject dono, List<ItemBase> lista)
        {
            base.IniciaUsoComCriature(dono, lista);
            IniciaUsoDaCarta();
        }

        public override bool AtualizaUsoComCriature()
        {
            return AtualizaUsoDaCarta();
        }

        public override void IniciaUsoDeHeroi(GameObject dono, List<ItemBase> lista)
        {
            base.IniciaUsoDeHeroi(dono,lista);
            IniciaUsoDaCarta();
        }

        public override bool AtualizaUsoDeHeroi()
        {
            return AtualizaUsoDaCarta();
        }

        bool PodeUsar()
        {
            //(AbstractGlobalController.Instance.Players[0].Manager as CharacterManager).ActivePet.Mov.LockTarget
            //if (GameController.g.estaEmLuta && !GameController.g.ContraTreinador)

            if(FindByOwner.GetEnemy(Dono)&&!Dono.GetComponent<CharacterManager>().ContraTreinador)
                return true;

            return false;
        }

        private void IniciaUsoDaCarta()
        {
            if (PodeUsar())
            {
                
                //Manager = GameController.g.Manager;
                Estado = ItemUseState.animandoBraco;
                RetirarUmItem(Lista, this, 1);
                PetManagerEnemy P = FindByOwner.GetManagerEnemy(Dono) as PetManagerEnemy;

                if (P)
                {
                    CameraApplicator.cam.RemoveMira();
                    InicializacaoComum(Dono, P.transform /*GameController.g.InimigoAtivo.transform*/);
                    P.StopWithRememberedState();
                }
                else
                    Debug.LogError("Uma mensagem para criature não focado");

            }
            else
            {
                Estado = ItemUseState.finalizaUsaItem;

                MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
                {
                    message = TextBank.RetornaFraseDoIdioma(TextKey.mensLuta)
                });

                //Debug.LogError("Uma mensagem de não pode usar");

                if (!FindByOwner.GetManagerEnemy(Dono))
                    MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
                    {
                        message = TextBank.RetornaListaDeTextoDoIdioma(TextKey.mensLuta)[5]
                    });


                //if (!GameController.g.estaEmLuta)
                //    GameController.g.HudM.Painel.AtivarNovaMens(BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.mensLuta)[0], 30, 7);
                //else if (GameController.g.ContraTreinador)
                //    GameController.g.HudM.Painel.AtivarNovaMens(BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.mensLuta)[3], 30, 7);
            }
        }

        bool ContinhaDeCaptura()
        {
            PetBase P = FindByOwner.GetManagerEnemy(Dono).MeuCriatureBase;
            ConsumableAttribute vida = P.PetFeat.meusAtributos.PV;
            float taxaDeVida = ((float)vida.Corrente) / vida.Maximo;
            if (taxaDeVida < taxOfcapture.maxHp)
            {
                taxaDeVida = Mathf.Pow(1-taxaDeVida/taxOfcapture.maxHp, taxOfcapture.interpolateIndex);
                Debug.Log("depois do pow: " + taxaDeVida);
                taxaDeVida = ((1 - taxaDeVida) * taxOfcapture.taxOnMaxHp + taxaDeVida * taxOfcapture.maxTax)/P.captureDificult;
                Debug.Log("depois do interpolate: " + taxaDeVida);
                float sorteio = Random.value;

                Debug.Log("O valor sorteado é: " + sorteio + " e a taxa minima é : " + taxaDeVida);
                if (sorteio < taxaDeVida)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
            



            //return true;
            #region antigo
            //int vida = FindByOwner.GetManagerEnemy(Dono).MeuCriatureBase.PetFeat.meusAtributos.PV.Corrente;

            //bool retorno = false;


            //if (vida == 2)
            //{
            //    float x = Random.value;
            //    if (x > 0.75f)
            //        retorno = true;
            //    else
            //        retorno = false;

            //    Debug.Log("dois pontos de vida: " + x);
            //}

            //if (vida == 1)
            //{
            //    float y = Random.value;
            //    if (y > 0.25f)
            //        retorno = true;
            //    else
            //        retorno = false;
            //}

            //return retorno; 
            #endregion
        }

        private bool AtualizaUsoDaCarta()
        {

            switch (Estado)
            {
                case ItemUseState.animandoBraco:
                    if (!AnimaB.AnimaTroca(true))
                    {
                        captura = ContinhaDeCaptura();
                        Estado = ItemUseState.aplicandoItem;
                        animaCap = new CaptureManager(Dono,captura);
                    }
                    break;
                case ItemUseState.aplicandoItem:
                    if (!animaCap.Update())
                    {
                        if (captura)
                        {

                            Estado = ItemUseState.finalizaUsaItem;
                        }
                        else
                        {
                            MessageAgregator<MsgRequestEndArmsAnimations>.Publish(new MsgRequestEndArmsAnimations()
                            {
                                oAnimado = Dono
                            });

                            
                            (FindByOwner.GetManagerEnemy(Dono) as PetManagerEnemy)
                                .StartAgressiveIa(Dono.GetComponent<CharacterManager>().ActivePet.gameObject);
                            Estado = ItemUseState.finalizaUsaItem;
                        }
                    }
                    break;
                case ItemUseState.finalizaUsaItem:
                    return false;
                //break;
                case ItemUseState.nulo:
                    Debug.Log("O itemManager alcançou estado nulo para " + ID.ToString());
                    break;
            }
            return true;
        }
    }
}