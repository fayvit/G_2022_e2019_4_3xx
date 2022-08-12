using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class ItemFactory
    {
        public static ItemBase Get(NameIdItem nomeItem, int estoque = 1)
        {
            ItemBase retorno = new ItemBase(new ItemFeatures());
            switch (nomeItem)
            {
                case NameIdItem.maca:
                    retorno = new MacaItem(estoque);
                break;
                case NameIdItem.cartaLuva:
                    retorno = new CartaLuva(estoque);
                break;
                case NameIdItem.gasolina:
                    retorno = new Gasolina(estoque);
                break;
                case NameIdItem.aguaTonica:
                    retorno = new AguaTonica(estoque);
                break;
                case NameIdItem.aura:
                    retorno = new Aura(estoque);
                break;
                case NameIdItem.regador:
                    retorno = new Regador(estoque);
                break;
                case NameIdItem.ventilador:
                    retorno = new Ventilador(estoque);
                break;
                case NameIdItem.inseticida:
                    retorno = new Inseticida(estoque);
                break;
                case NameIdItem.pilha:
                    retorno = new Pilha(estoque);
                break;
                case NameIdItem.estrela:
                    retorno = new Estrela(estoque);
                break;
                case NameIdItem.seiva:
                    retorno = new Seiva(estoque);
                break;
                case NameIdItem.quartzo:
                    retorno = new Quartzo(estoque);
                break;
                case NameIdItem.adubo:
                    retorno = new Adubo(estoque);
                break;
                case NameIdItem.repolhoComOvo:
                    retorno = new RepolhoComOvo(estoque);
                break;
                case NameIdItem.pergTempestadeEletrica:
                    retorno = new PergTempestadeEletrica(estoque);
                break;
                //case nomeIDitem.pergArmagedom:
                //    retorno = new MbPergaminhoDeArmagedom(estoque);
                //    break;
                //case nomeIDitem.pergaminhoDePerfeicao:
                //    retorno = new MbPergaminhoDePerfeicao(estoque);
                //    break;
                //case nomeIDitem.pergaminhoDeFuga:
                //    retorno = new MbPergaminhoDeFuga(estoque);
                //    break;
                //case nomeIDitem.tinteiroSagradoDeLog:
                //    retorno = new TinteiroSagradaDeLog(estoque);
                //    break;
                //case nomeIDitem.pergaminhoDeLaurense:
                //    retorno = new PergaminhoDeLaurense(estoque);
                //    break;
                //case nomeIDitem.pergaminhoDeAnanda:
                //    retorno = new PergaminhoDeAnanda(estoque);
                //    break;
                //case nomeIDitem.pergaminhoDeBoutjoi:
                //    retorno = new PergaminhoDeBoutjoi(estoque);
                //    break;
                //case nomeIDitem.canetaSagradaDeLog:
                //    retorno = new CanetaSagradaDeLog(estoque);
                //    break;
                //case nomeIDitem.pergSinara:
                //    retorno = new PergaminhoDeSinara(estoque);
                //    break;
                //case nomeIDitem.pergAlana:
                //    retorno = new PergaminhoDeAlana(estoque);
                //    break;
                //case nomeIDitem.pergSabre:
                //    retorno = new PergDeSabre(estoque);
                //    break;
                //case nomeIDitem.pergMultiplicar:
                //    retorno = new PergDoMultiplicar(estoque);
                //    break;
                case NameIdItem.antidoto:
                    retorno = new Antidoto(estoque);
                    break;
                case NameIdItem.amuletoDaCoragem:
                    retorno = new AmuletoDaCoragem(estoque);
                    break;
                case NameIdItem.tonico:
                    retorno = new Tonico(estoque);
                    break;
                    //case nomeIDitem.pergOlharEnfraquecedor:
                    //    retorno = new PergOlharEnfraquecedor(estoque);
                    //    break;
                    //case nomeIDitem.pergOlharMal:
                    //    retorno = new PergOlharMal(estoque);
                    //    break;
                    //case nomeIDitem.pergFuracaoDeFolhas:
                    //    retorno = new PergFuracaoDeFolhas(estoque);
                    //    break;
                    //case nomeIDitem.pergVentosCortantes:
                    //    retorno = new PergVentosCortantes(estoque);
                    //    break;
                    //case nomeIDitem.pergGosmaAcida:
                    //    retorno = new PergGosmaAcida(estoque);
                    //    break;
            }
            return retorno;
        }
    }

}