using UnityEngine;
using System.Collections;
using TextBankSpace;
using System.Collections.Generic;
using FayvitMessageAgregator;

namespace Criatures2021
{
    [System.Serializable]
    public class ItemBase: System.ICloneable
    {
        [SerializeField] private NameIdItem nomeID;
        [SerializeField] private ItemNature itemNature;
        [SerializeField] private bool usavel;
        [SerializeField] private int acumulavel;
        [SerializeField] private int estoque;
        [SerializeField] private int valor;
        [SerializeField] private bool nosItensRapidos;
        

        //private GameObject gAlvoDoItem;
        [System.NonSerialized] private GameObject dono;
        [System.NonSerialized] private List<ItemBase> lista;
        [System.NonSerialized] private AnimateArm animaB;
        //[System.NonSerialized] private FluxoDeRetorno fluxo;
        [System.NonSerialized] private ItemUseState estado = ItemUseState.nulo;

        [System.NonSerialized] private float tempoDecorrido = 0;

        public const float INTERVALO_DO_USO_DE_ITEM = 15;

        public ItemBase(ItemFeatures cont)
        {
            this.nomeID = cont.NomeID;
            this.usavel = cont.consumivel;
            this.acumulavel = cont.acumulavel;
            this.estoque = cont.estoque;
            this.valor = cont.valor;
            this.nosItensRapidos = cont.nosItensRapidos;
            this.itemNature = cont.itemNature;
        }

        public NameIdItem ID
        {
            get { return nomeID; }
        }

        public ItemNature Item_Nature => itemNature;

        public bool NosItensRapidos { get => nosItensRapidos; set => nosItensRapidos = value; }

        public bool Consumivel
        {
            get { return usavel; }
        }

        public int Acumulavel
        {
            get { return acumulavel; }
        }

        public int Estoque
        {
            get { return estoque; }
            set { estoque = value; }
        }

        public int Valor
        {
            get { return valor; }
        }

        public ItemUseState Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        public float TempoDecorrido
        {
            get { return tempoDecorrido; }
            set { tempoDecorrido = value; }
        }

        public GameObject Dono
        {
            get { return dono; }
            set { dono = value; }
        }

        public AnimateArm AnimaB
        {
            get { return animaB; }
            set { animaB = value; }
        }

        public List<ItemBase> Lista
        {
            get => lista;
            set => lista = value;
        }

        public virtual void IniciaUsoDeMenu(GameObject dono,List<ItemBase> lista)
        {
            this.lista = lista;
            this.dono = dono;
        }

        public virtual void RetornaSemUsarComMenu()
        {
            throw new System.NotImplementedException();
            //return false;
        }

        public virtual void IniciaUsoComCriature(GameObject dono, List<ItemBase> lista)
        {
            this.lista = lista;
            this.dono = dono;
        }

        public virtual bool AtualizaUsoComCriature()
        {
            throw new System.NotImplementedException();
        }

        public virtual void IniciaUsoDeHeroi(GameObject dono, List<ItemBase> lista)
        {
            this.lista = lista;
            this.dono = dono;
        }

        public virtual bool AtualizaUsoDeHeroi()
        {
            throw new System.NotImplementedException();
        }

        public static bool RetirarUmItem(
            List<ItemBase> lista,
            ItemBase nomeItem,
            int quantidade = 1
            /*FluxoDeRetorno fluxo = FluxoDeRetorno.heroi*/)
        {
            bool retorno = true;

            for (int i = 0; i < quantidade; i++)
            {
                retorno &= RetirarUmItem(ProcuraItemNaLista(nomeItem.ID,lista), lista/*, fluxo*/);
            }

            return retorno;
        }

        public static bool RetirarUmItem(
            List<ItemBase> lista,
            NameIdItem nomeItem,
            int quantidade = 1
            /*FluxoDeRetorno fluxo = FluxoDeRetorno.heroi*/)
        {
            bool retorno = true;

            for (int i = 0; i < quantidade; i++)
            {
                retorno &= RetirarUmItem(ProcuraItemNaLista(nomeItem,lista), lista/*, fluxo*/);

            }

            return retorno;
        }

        public static ItemBase ProcuraItemNaLista(NameIdItem nome,List<ItemBase> lista)
        {
            ItemBase retorno = new ItemBase(new ItemFeatures(NameIdItem.generico));
            for (int i = lista.Count - 1; i > -1; i--)
            {
                if (lista[i].ID == nome)
                    retorno = lista[i];
            }
            return retorno;

        }

        public static bool RetirarUmItem(
            ItemBase nomeItem,
            List<ItemBase> lista
            /*FluxoDeRetorno fluxo = FluxoDeRetorno.heroi*/)
        {
            
            int indice = lista.IndexOf(nomeItem);
            Debug.Log("NOme do item: "+nomeItem+" : "+nomeItem.ID+" indice "+indice+" : ramanho da lista"+lista.Count);
            if (indice > -1)
                if (lista[indice].Estoque >= 1)
                {
                    lista[indice].Estoque--;
                    //GameController g = GameController.g;

                    Debug.Log("Condição para ser perguntada antes desse metodo [casos de não poder usar, como em luta e etc...]");
                    //if (g.UsarTempoDeItem == UsarTempoDeItem.sempre || (g.UsarTempoDeItem == UsarTempoDeItem.emLuta && g.estaEmLuta))
                    //    gerente.Dados.TempoDoUltimoUsoDeItem = Time.time;

                    MessageAgregator<MsgItemAmountChange>.Publish(new MsgItemAmountChange()
                    {
                        target = lista[indice]
                    });

                    if (lista[indice].Estoque == 0)
                    {
                        Debug.Log("metodo para remover item quando acionado [Parece estar funcionando]");
                        //Debug.Log("Tira daí");
                        //g.FinalizaHuds();
                        //lista.Remove(gerente.Dados.Itens[indice]);
                        lista.RemoveAt(indice);

                        //if (gerente.Dados.itemSai > lista.Count - 1)
                        //    gerente.Dados.itemSai = 0;

                    /*Essa parte ja não fazia nada*/
                        //if (fluxo == FluxoDeRetorno.menuCriature || fluxo == FluxoDeRetorno.menuHeroi)
                        //{
                        //    GameController.g.StartCoroutine(VoltarDosItensQuandoNaoTemMais());
                        //}
                    }
                    return true;
                }

            return false;
        }

        protected void InicializacaoComum(GameObject dono, Transform alvoDoItem)
        {
            Dono = dono;
            MessageAgregator<MsgStartUseItem>.Publish(new MsgStartUseItem
            {
                usuario = dono
            });
            //Manager = GameController.g.Manager;
            TempoDecorrido = 0;

            //Manager.Estado = EstadoDePersonagem.parado;
            //Manager.CriatureAtivo.PararCriatureNoLocal();
            ////Manager.Mov.Animador.PararAnimacao();

            //if (GameController.g.estaEmLuta)
            //    GameController.g.InimigoAtivo.PararCriatureNoLocal();

            AnimaB = new AnimateArm(dono.transform, alvoDoItem);
            

        }

        protected void FecharMensagem()
        {
            Estado = ItemUseState.finalizaUsaItem;

            Debug.Log("pedindo religamento de botoes [em observação]");

            //GameController.g.HudM.MenuDePause.ReligarBotoesDoPainelDeItens();
        }

        //static IEnumerator VoltarDosItensQuandoNaoTemMais()
        //{
        //    yield return new WaitForSecondsRealtime(1f);
        //    //GameController.g.HudM.P_EscolheUsoDeItens.VoltarDosItens();
        //}

        public static string NomeEmLinguas(NameIdItem ID)
        {
            return TextBank.RetornaListaDeTextoDoIdioma(TextKey.listaDeItens)[(int)ID];
        }

        public object Clone()
        {
            return ItemFactory.Get(nomeID, estoque);
        }
    }

    public struct ItemFeatures
    {
        public NameIdItem NomeID;
        public ItemNature itemNature;
        public bool consumivel;
        public int acumulavel;
        public int estoque;
        public int valor;
        public bool nosItensRapidos;

        /// <summary>
        /// Ao construir com o parametro nome, os campos recebem valores padrões
        /// </summary>
        /// <param name="nome">Nome do item a ser construido</param>
        public ItemFeatures(NameIdItem nome)
        {
            this.NomeID = nome;
            this.itemNature = ItemNature.consumivel;
            this.consumivel = true;
            this.acumulavel = 99;
            this.estoque = 1;
            this.valor = 1;
            this.nosItensRapidos = true;
        }
    }

    public struct MsgStartUseItem : IMessageBase
    {
        public GameObject usuario;
    }

    public struct MsgItemAmountChange : IMessageBase
    {
        public ItemBase target;
    }

    public enum ItemUseState
    {
        nulo = -1,
        animandoBraco,
        aplicandoItem,
        selecaoDeItem,
        finalizaUsaItem,
        updateParticular,
        emEspera,
        oneMessageOpened
    }

    public enum ItemNature
    { 
        nula=-1,
        consumivel,
        chave,
        pergGolpe,
        usoParticular
    }

    public enum NameIdItem
    {
        generico = -1,
        maca,
        burguer,
        cartaLuva,
        gasolina,
        aguaTonica,
        regador,
        estrela,
        quartzo,
        adubo,
        seiva,
        inseticida,
        aura,
        repolhoComOvo,
        ventilador,
        pilha,
        geloSeco,
        pergaminhoDeFuga,
        segredo,
        estatuaMisteriosa,
        cristais,
        pergaminhoDePerfeicao,
        antidoto,
        amuletoDaCoragem,
        tonico,
        pergDeRajadaDeAgua,
        pergSaida,
        condecoracaoAlpha,
        pergArmagedom,
        pergSabre,
        pergGosmaDeInseto,
        pergGosmaAcida,
        pergMultiplicar,
        pergVentania,
        pergVentosCortantes,
        pergOlharEnfraquecedor,
        pergOlharMal,
        condecoracaoBeta,
        pergFuracaoDeFolhas,
        explosivos,
        medalhaoDasAguas,
        tinteiroSagradoDeLog,
        pergaminhoDeLaurense,
        pergaminhoDeBoutjoi,
        pergaminhoDeAnanda,
        canetaSagradaDeLog,
        pergSinara,
        pergAlana,
        pergTempestadeEletrica,
        brasaoDeLaurense,
        brasaoDeAnanda,
        brasaoDeBoutjoi,
        pergTurboDeAgua,
        pergHidroBomba,
        pergLaminaDeFolha,
        pergTempesdadeDeFolhas,
        pergBolaDeFogo,
        pergRajadaDeFogo,
        pergTosteAtaque
    }
}