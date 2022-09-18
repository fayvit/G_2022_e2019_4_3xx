using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using CriaturesLegado;

namespace Criatures2021
{
    [System.Serializable]
    public class DadosDeJogador:FayvitSave.DadosDeJogador
    {
        [SerializeField, ArrayElementTitle("petId")] private List<PetBase> criaturesAtivos = new List<PetBase>();
        [SerializeField, ArrayElementTitle("petId")] private List<PetBase> criaturesArmagedados = new List<PetBase>();
        [SerializeField, ArrayElementTitle("nomeID")] private List<ItemBase> itens = new List<ItemBase>();
        [SerializeField] private int cristais = 0;

        public int Cristais { get => cristais; }
        public int ItemSai { get; set; } = 0;
        public int MaxCarregaveis { get; set; } = 5;

        public List<ItemBase> Itens { get => itens; set => itens = value; }
        public IndiceDeArmagedoms UltimoArmagedom { get; set; } = IndiceDeArmagedoms.acampamentoDaResistencia;
        public float TempoDoUltimoUsoDeItem { get; set; } = 0;
        public int CriatureSai { get; set; } = 0;

        public List<PetBase> CriaturesAtivos { get=>criaturesAtivos; set=>criaturesAtivos=value; }
        public List<PetBase> CriaturesArmagedados { get=>criaturesArmagedados; set=>criaturesArmagedados=value; }
        public LivroDosCriatures Livro { get; set; } = new LivroDosCriatures();

        /// <summary>
        /// Adiciona o valor amount ao total de cristais, amount deve ser positivo
        /// </summary>
        /// <param name="amount">Um inteiro positivo</param>
        public void AdicionarCristais(int amount)
        {
            cristais += Mathf.Max(amount,0);
        }

        /// <summary>
        /// Remove o valor amount ao total de cristais. Se amount foi maior que o numero de cristais, cristais recebe zero. O valor Amount deve ser positivo
        /// </summary>
        /// <param name="amount">Um inteiro positivo</param>
        public void RemoverCristais(int amount)
        { 
            cristais = Mathf.Max(0,cristais- Mathf.Max(0, amount));
        }


        public void DadosLimpos()
        {
            criaturesAtivos = new List<PetBase>();
            criaturesArmagedados = new List<PetBase>();
            itens = new List<ItemBase>();
        }


        public void InicializadorDosDados()
        {
            cristais = 100;
            CriaturesArmagedados = new List<PetBase>() {
                new PetBase(PetName.DogMour,5),
                new PetBase(PetName.Trude,5),
                new PetBase(PetName.Galfo,11),
                new PetBase(PetName.Wisks,7),
                new PetBase(PetName.Vampire,13),
                new PetBase(PetName.Rabitler,9),
                new PetBase(PetName.Arpia,9),
                new PetBase(PetName.Marak,11),
                new PetBase(PetName.Estrep,11),
                new PetBase(PetName.Onarac,7),
                new PetBase(PetName.Oderc,7),
                new PetBase(PetName.Nedak,7),
                new PetBase(PetName.Izicuolo,7),
                new PetBase(PetName.FelixCat,7),
                new PetBase(PetName.Nessei,7),
                new PetBase(PetName.Steal,8),
                new PetBase(PetName.Urkan,8),
                new PetBase(PetName.Iruin,7),
                new PetBase(PetName.Cabecu,10),
                new PetBase(PetName.Florest,10),
                new PetBase(PetName.Xuash,10),
                new PetBase(PetName.PolyCharm,10),
                new PetBase(PetName.Flam,9),
                new PetBase(PetName.Cracler,8),
                new PetBase(PetName.Baratarab,10),
                new PetBase(PetName.Babaucu,10),
                new PetBase(PetName.Aladegg,9),
                new PetBase(PetName.Abutre,8),
                new PetBase(PetName.Fajin,7),
                new PetBase(PetName.Rocketler,7),
                new PetBase(PetName.Croc,8),
                new PetBase(PetName.Escorpion,8),
            };

            CriaturesAtivos = new List<PetBase>() {
                new PetBase(PetName.Florest,7),
                new PetBase(PetName.Xuash,11),
                new PetBase(PetName.PolyCharm,7),
                new PetBase(PetName.Vampire,13),
                new PetBase(PetName.Rabitler,9),
                new PetBase(PetName.Arpia,9),
                new PetBase(PetName.Marak,11),
                //new PetBase(PetName.Estrep,11),
                //new PetBase(PetName.Onarac,7),
                //new PetBase(PetName.Oderc,7),
                //new PetBase(PetName.Nedak,7),
                //new PetBase(PetName.Izicuolo,7),
                //new PetBase(PetName.FelixCat,7),
                //new PetBase(PetName.Nessei,7),
                //new PetBase(PetName.Steal,8),
                //new PetBase(PetName.Urkan,8),
                //new PetBase(PetName.Iruin,7),
                //new PetBase(PetName.Cabecu,10),
                //new PetBase(PetName.Florest,10),
                //new PetBase(PetName.Xuash,10),
                //new PetBase(PetName.PolyCharm,10),
                //new PetBase(PetName.Flam,9),
                //new PetBase(PetName.Cracler,8),
                //new PetBase(PetName.Baratarab,10),
                //new PetBase(PetName.Babaucu,10),
                //new PetBase(PetName.Aladegg,9),
                //new PetBase(PetName.Abutre,8),
                //new PetBase(PetName.Fajin,7),
                //new PetBase(PetName.Rocketler,7),
                //new PetBase(PetName.Croc,8),
                //new PetBase(PetName.Escorpion,8),
            };

            foreach (var v in criaturesAtivos)
            {
                Livro.AdicionaVisto(v.NomeID);
                Livro.AdicionaCapturado(v.NomeID);
            }


            //CriaturesAtivos[0].PetFeat.meusAtributos.PE.Corrente = 1;
            //CriaturesAtivos[1].PetFeat.meusAtributos.PV.Corrente = 1;
            //CriaturesAtivos[2].PetFeat.meusAtributos.PE.Corrente = 5;
            //CriaturesAtivos[4].StatusTemporarios.Add(new DatesForTemporaryStatus() { Tipo = StatusType.envenenado });
            //VerifyApplyPoisonStatus.InsereStatus(null, criaturesAtivos[0], new DatesForTemporaryStatus() { Tipo = StatusType.envenenado }, true);
            //AddSimpleStatus.InserindoNovoStatus(null, criaturesAtivos[4],
            //    new Amedrontado()
            //    {
            //        CDoAfetado = null,
            //        Dados = new DatesForTemporaryStatus() { Tipo = StatusType.amedrontado },
            //        OAfetado = criaturesAtivos[4]
            //    }, true);

            //AddSimpleStatus.InserindoNovoStatus(null, criaturesAtivos[0],
            //    new Fraco()
            //    {
            //        CDoAfetado = null,
            //        Dados = new DatesForTemporaryStatus() { Tipo = StatusType.fraco },
            //        OAfetado = criaturesAtivos[0]
            //    }, true);

            //criaturesAtivos[0].GolpesPorAprender.Add(new PetAttackDb() { Nome = AttackNameId.sobreVoo });
            //criaturesAtivos[0].PetFeat.mNivel.ParaProxNivel = criaturesAtivos[0].PetFeat.mNivel.XP + 1;

            //CriaturesArmagedados = new List<PetBase>() {
            //    new PetBase(PetName.Onarac,1),
            //    new PetBase(PetName.Babaucu,3),
            //    new PetBase(PetName.Wisks,2),
            //    new PetBase(PetName.Serpente,3)
            //};
            Itens = new List<ItemBase>()
        {
            ItemFactory.Get(NameIdItem.pergaminhoDePerfeicao,14),
            ItemFactory.Get(NameIdItem.maca,1),
            ItemFactory.Get(NameIdItem.cartaLuva,11),
            ItemFactory.Get(NameIdItem.pergTempestadeEletrica,1),
            ItemFactory.Get(NameIdItem.tonico,10),
            ItemFactory.Get(NameIdItem.amuletoDaCoragem,10),
            //ItemFactory.Get(NameIdItem.antidoto,10),
            //ItemFactory.Get(NameIdItem.tinteiroSagradoDeLog,10),
            //ItemFactory.Get(NameIdItem.ventilador,10),
            //ItemFactory.Get(NameIdItem.gasolina,10),
            //ItemFactory.Get(NameIdItem.regador,10),
            //ItemFactory.Get(NameIdItem.aguaTonica,10),
            //ItemFactory.Get(NameIdItem.inseticida,10),
            //ItemFactory.Get(NameIdItem.seiva,10),
            //ItemFactory.Get(NameIdItem.pilha,10),
            //ItemFactory.Get(NameIdItem.estrela,10),
            //ItemFactory.Get(NameIdItem.pergaminhoDeLaurense,10),
            //ItemFactory.Get(NameIdItem.pergaminhoDeAnanda,10),
            //ItemFactory.Get(NameIdItem.pergaminhoDeBoutjoi,10),
            //ItemFactory.Get(NameIdItem.brasaoDeLaurense,10),
            //ItemFactory.Get(NameIdItem.brasaoDeAnanda,10),
            //ItemFactory.Get(NameIdItem.brasaoDeBoutjoi,10),
            //ItemFactory.Get(NameIdItem.pergVentosCortantes,2),
            //ItemFactory.Get(NameIdItem.pergFuracaoDeFolhas,5),
            //ItemFactory.Get(NameIdItem.pergaminhoDeFuga,10),
            //ItemFactory.Get(NameIdItem.regador,10),
            //ItemFactory.Get(NameIdItem.inseticida,2),
            //ItemFactory.Get(NameIdItem.ventilador,2),
            //ItemFactory.Get(NameIdItem.pergSinara,2),
            //ItemFactory.Get(NameIdItem.pergAlana,1)
        };

        }

        /// <summary>
        /// Verifica se um dos criatures no vetor de criatures ativos tem golpes por aprender
        /// </summary>
        /// <returns></returns>
        public bool TemGolpesPorAprender()
        {
            bool retorno = false;
            for (int i = 0; i < criaturesAtivos.Count; i++)
            {
                retorno |= criaturesAtivos[i].GolpesPorAprender.Count > 0;
            }

            return retorno;
        }

        /// <summary>
        /// Verifica se um criature est� com golpes por aprender
        /// </summary>
        /// <param name="qualIndice">Indice do criature no vetor de criatures ativos</param>
        /// <returns></returns>
        public bool TemGolpesPorAprender(int qualIndice)
        {
           return CriaturesAtivos.Count>0? criaturesAtivos[qualIndice].GolpesPorAprender.Count > 0:false;
        }

        public PetBase PrimeiroComGolpePorAprender()
        {
             
            for (int i = 0; i < criaturesAtivos.Count; i++)
                if (criaturesAtivos[i].GolpesPorAprender.Count > 0)
                    return criaturesAtivos[i];

            return PetFactory.GetPet(PetName.nulo); 
        }

        public bool TemAlgumPetAtivoVivo()
        {
            for (int i = 0; i < criaturesAtivos.Count; i++)
            {
                if (criaturesAtivos[i].PetFeat.meusAtributos.PV.Corrente > 0)
                    return true;
            }

            return false;
        }

        public int TemItem(NameIdItem nome)
        {
            int tanto = 0;
            for (int i = 0; i < Itens.Count; i++)
            {
                if (Itens[i].ID == nome)
                    tanto += Itens[i].Estoque;
            }

            return tanto;
        }

        public void AdicionaItem(NameIdItem nomeItem, int quantidade)
        {
            if (nomeItem != NameIdItem.cristais)
            {
                for (int i = 0; i < quantidade; i++)
                {
                    AdicionaItem(nomeItem);
                }
            }
            else
            {
                cristais += quantidade;
            }
        }

        public void AdicionaItem(NameIdItem nomeItem)
        {
            ItemBase I = ItemFactory.Get(nomeItem);
            bool foi = false;
            if (I.Acumulavel > 1)
            {

                int ondeTem = -1;
                for (int i = 0; i < Itens.Count; i++)
                {
                    if (Itens[i].ID == I.ID)
                    {
                        if (Itens[i].Estoque < Itens[i].Acumulavel)
                        {
                            if (!foi)
                            {
                                ondeTem = i;
                                foi = true;
                            }
                        }
                    }
                }

                if (foi)
                {
                    Itens[ondeTem].Estoque++;
                }
                else
                {
                    Itens.Add(ItemFactory.Get(nomeItem));
                }
            }
            else
            {
                Itens.Add(ItemFactory.Get(nomeItem));
            }
        }

        public void TodosCriaturesPerfeitos()
        {
            for (int i = 0; i < criaturesAtivos.Count; i++)
            {
                criaturesAtivos[i].EstadoPerfeito();
            }
        }
    }
}