using UnityEngine;
using System.Collections.Generic;
using FayvitMessageAgregator;

namespace Criatures2021
{
    public class UseItemManager:MonoBehaviour
    {
        private GameObject dono;
        private FluxoDeRetorno fluxo;

        private ItemBase esseItem;
        private bool retorno = false;

        public bool EstouUsandoItem { get; private set; } = false;

        // Use this for initialization
        public void StartFields(GameObject dono, List<ItemBase> lista, int indexOfUse,FluxoDeRetorno fluxo)
        {
            EstouUsandoItem = true;
            this.dono = dono;

            esseItem = (ItemBase)lista[indexOfUse].Clone();
            this.fluxo = fluxo;

            switch (fluxo)
            {
                case FluxoDeRetorno.criature:
                    esseItem.IniciaUsoComCriature(dono,lista);
                    break;
                case FluxoDeRetorno.heroi:
                    esseItem.IniciaUsoDeHeroi(dono,lista);
                break;
                case FluxoDeRetorno.menuHeroi:
                case FluxoDeRetorno.menuCriature:
                    esseItem.IniciaUsoDeMenu(dono,lista);
                break;
            }
        }

        // Update is called once per frame
        public void Update()
        {
            if (EstouUsandoItem)
            {
                switch (fluxo)
                {
                    case FluxoDeRetorno.criature:
                        retorno = !esseItem.AtualizaUsoComCriature();
                        break;
                    case FluxoDeRetorno.heroi:
                        retorno = !esseItem.AtualizaUsoDeHeroi();
                    break;
                    //case FluxoDeRetorno.menuHeroi:
                    //case FluxoDeRetorno.menuCriature:
                    //    retorno = !esseItem.AtualizaUsoDeMenu();// parece desnecessario
                    //break;
                }

                if (retorno)
                {
                    FindByOwner.GetHeroActivePet(dono).gameObject.tag = "Criature";
                    switch (fluxo)
                    {
                        case FluxoDeRetorno.criature:
                            
                            MessageAgregator<MsgRequestChangeToPetByReplace>.Publish(new MsgRequestChangeToPetByReplace()
                            {
                                dono = dono,
                                fluxo = FluxoDeRetorno.criature
                            });
                        break;
                        case FluxoDeRetorno.heroi:
                            MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero()
                            {
                                myHero = dono
                            });
                        break;
                        case FluxoDeRetorno.menuHeroi:
                        case FluxoDeRetorno.menuCriature:
                            //GameController.g.HudM.PauseM.FinalizaUsoDeItemComMenu();
                        break;
                    }

                    EstouUsandoItem = false;

                }
            }
            else
                Destroy(this);
        }

        public void FinalizaUsaItemDeFora()
        {
            retorno = false;
            EstouUsandoItem = false;
            fluxo = FluxoDeRetorno.heroi;
        }
    }
}