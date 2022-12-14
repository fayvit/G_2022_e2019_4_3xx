using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class ItemFactory
    {
        public static ItemBase Get(NameIdItem nomeItem, int estoque = 1)
        {
            ItemBase retorno = new ItemBase(new ItemFeatures(NameIdItem.generico) { nosItensRapidos = false }) ;
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
                case NameIdItem.pergaminhoDePerfeicao:
                    retorno = new PergPerfeicao(estoque);
                break;
                //case nomeIDitem.pergaminhoDeFuga:
                //    retorno = new MbPergaminhoDeFuga(estoque);
                //    break;
                case NameIdItem.tinteiroSagradoDeLog:
                    retorno = new TinteiroSagradaDeLog(estoque);
                break;
                case NameIdItem.pergaminhoDeLaurense:
                    retorno = new PergaminhoDeLaurense(estoque);
                break;
                case NameIdItem.pergaminhoDeAnanda:
                    retorno = new PergaminhoDeAnanda(estoque);
                break;
                case NameIdItem.pergaminhoDeBoutjoi:
                    retorno = new PergaminhoDeBoutjoi(estoque);
                break;
                case NameIdItem.canetaSagradaDeLog:
                    retorno = new CanetaSagradaDeLog(estoque);
                break;
                case NameIdItem.pergSaida:
                    retorno = new PergSaida(estoque);
                break;
                case NameIdItem.pergSinara:
                    retorno = new PergaminhoDeSinara(estoque);
                    break;
                case NameIdItem.pergAlana:
                    retorno = new PergAlana(estoque);
                break;
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
                case NameIdItem.brasaoDeLaurense:
                    retorno = new BrasaoDeLaurense(estoque);
                break;
                case NameIdItem.brasaoDeAnanda:
                    retorno = new BrasaoDeAnanda(estoque);
                break;
                case NameIdItem.brasaoDeBoutjoi:
                    retorno = new BrasaoDeBoutjoi(estoque);
                break;
                case NameIdItem.pergDeRajadaDeAgua:
                    retorno = new PergRajadaDeAgua(estoque);
                break;
                case NameIdItem.pergTurboDeAgua:
                    retorno = new PergTurboDeAgua(estoque);
                break;
                case NameIdItem.pergHidroBomba:
                    retorno = new PergHidroBomba(estoque);
                break;
                case NameIdItem.pergBolaDeFogo:
                    retorno = new PergBolaDeFogo(estoque);
                    break;
                case NameIdItem.pergRajadaDeFogo:
                    retorno = new PergRajadaDeFogo(estoque);
                    break;
                case NameIdItem.pergTosteAtaque:
                    retorno = new PergTosteAtaque(estoque);
                break;
                //case nomeIDitem.pergOlharEnfraquecedor:
                //    retorno = new PergOlharEnfraquecedor(estoque);
                //    break;
                //case nomeIDitem.pergOlharMal:
                //    retorno = new PergOlharMal(estoque);
                //    break;
                case NameIdItem.pergLaminaDeFolha:
                    retorno = new PergLaminaDeFolha(estoque);
                break;
                case NameIdItem.pergFuracaoDeFolhas:
                    retorno = new PergFuracaoDeFolhas(estoque);
                break;
                case NameIdItem.pergTempesdadeDeFolhas:
                    retorno = new PergTempestadeDeFolhas(estoque);
                break;
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