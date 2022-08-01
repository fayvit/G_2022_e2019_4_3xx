using UnityEngine;
using System.Collections.Generic;
using TextBankSpace;
using FayvitMessageAgregator;

namespace Criatures2021
{
    [System.Serializable]
    public class StatusTemporarioBase
    {
        [SerializeField] private DatesForTemporaryStatus dados;
        [System.NonSerialized] private PetManager cDoAfetado;

        public PetBase OAfetado { get; set; }

        protected Transform Particula { get; private set; }

        public DatesForTemporaryStatus Dados
        {
            get { return dados; }
            set { dados = value; }
        }

        
        public PetManager CDoAfetado { get=>cDoAfetado; set=>cDoAfetado=value; }

        // Use this for initialization
        public virtual void Start()
        {

        }

        // Update is called once per frame
        public virtual void Update()
        {

        }

        public virtual void RecolocaParticula()
        { 
        
        }


        /// <summary>
        /// Função para inserção da particula no criature alvo do status
        /// </summary>
        /// <param name="essaParticula">identificador da particula</param>
        /// <param name="transform">transform o qual a particula será filiada</param>
        protected void ColocaAParticulaEAdicionaEsseStatus(string essaParticula, Transform transform)
        {
            //Debug.LogError("Isso deve ser refeito");
            GameObject G = Resources.Load<GameObject>("particles/" + essaParticula);
            Particula = Object.Instantiate(G,Vector3.zero, Quaternion.identity,transform).transform;
            
            Particula.localPosition = Vector3.zero;
            Particula.localRotation = Quaternion.identity;

            if (ContemStatus(Dados.Tipo, OAfetado) <= -1)
            {
                OAfetado.StatusTemporarios.Add(Dados);
            }
        }

        public static int ContemStatus(StatusType esseStatus, PetBase X)
        {
            int retorno = -1;
            if (X.StatusTemporarios.Count > 0)
            {
                foreach (DatesForTemporaryStatus sT in X.StatusTemporarios)
                {
                    if (sT.Tipo == esseStatus)
                    {
                        retorno = X.StatusTemporarios.IndexOf(sT);
                    }
                }
            }

            return retorno;
        }

        public static void TiraStatusDaLista(StatusType tipo, List<DatesForTemporaryStatus> afetado)
        {
            if (afetado.Count > 0)
            {
                List<DatesForTemporaryStatus> aTirar = new List<DatesForTemporaryStatus>();
                foreach (DatesForTemporaryStatus sT in afetado)
                {
                    if (sT.Tipo == tipo || tipo == StatusType.todos)
                    {
                        aTirar.Add(sT);
                    }
                }

                for (int i = 0; i < aTirar.Count; i++)
                {
                    afetado.Remove(aTirar[i]);
                }
            }
        }

        public virtual void StatusHitUpdater() { }

        public virtual void RetiraParticulaDeStatus()
        {
            if (CDoAfetado != null && Particula != null)
                Object.Destroy(Particula.gameObject);
        }

        public virtual void RetiraComponenteStatus()
        {
            //Debug.Log("Retira componmen status está em observação");

            RetiraParticulaDeStatus();
            
            //if (GameController.g.ContStatus.StatusDoHeroi.Contains(this))
            //    GameController.g.ContStatus.StatusDoHeroi.Remove(this);
            //else if (GameController.g.ContStatus.StatusDoInimigo.Contains(this))
            //    GameController.g.ContStatus.StatusDoInimigo.Remove(this);

            OAfetado.StatusTemporarios.Remove(Dados);

            MessageAgregator<MsgRemoveStatus>.Publish(new MsgRemoveStatus() { status = this, pet = CDoAfetado, petBase = OAfetado });
            Debug.Log("Quando usa o item anti status: " + OAfetado.NomeID);
        }

        public static string NomeEmLinguas(StatusType nome)
        {
            //Debug.LogError("precisa ser feito");

            string[] arr = TextBank.RetornaListaDeTextoDoIdioma(TextKey.nomeStatus).ToArray();
            if (arr.Length > (int)nome)
                return arr[(int)nome];
            else
            {
                Debug.LogError("O array de nomes de status não contem um nome para o ID= " + nome);
                return nome.ToString();// BancoDeTextos.falacoes[heroi.lingua]["listaDeGolpes"][(int)Nome];
            }
        }
    }

    public struct MsgRemoveStatus : IMessageBase
    {
        public StatusTemporarioBase status;
        public PetManager pet;
        public PetBase petBase;
    }
}