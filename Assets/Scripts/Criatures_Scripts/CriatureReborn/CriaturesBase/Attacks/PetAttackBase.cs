using UnityEngine;
using System.Collections;
using FayvitSounds;
using TextBankSpace;

namespace Criatures2021
{
    [System.Serializable]
    public class NullPetAttack : PetAttackBase
    {
        public NullPetAttack(PetAttackFeatures container):base(container){ }

        public override void IniciaGolpe(GameObject G)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateGolpe(GameObject G,GameObject focado =null)
        {
            throw new System.NotImplementedException();
        }

        public override void VerificaAplicaStatus(PetBase atacante, PetManager atacado)
        {
            throw new System.NotImplementedException();
        }
    }

    [System.Serializable]
    public class PetAttackBase
    {
        [SerializeField] private PetAttackFeatures container;
        [System.NonSerialized] private Vector3 dirDeREpulsao = Vector3.zero;

        public virtual void IniciaGolpe(GameObject G) { }

        public virtual void UpdateGolpe(GameObject G, GameObject focado = null) { }

        public virtual void VerificaAplicaStatus(PetBase atacante, PetManager atacado) { }


        public PetAttackBase(PetAttackFeatures container)
        {
            this.container = container;
        }

        public float ColisorScale
        {
            get { return container.colisorScale; }
        }

        public AttackDiferentialId Caracteristica
        {
            get
            {
                return container.carac;
            }
        }

        public DamageBaseAtribute DamageAtribute => container.damageAtribute;

        public int CustoPE
        {
            get
            {
                return container.custoPE;
            }
        }

        public int ModCorrente
        {
            get
            {
                return container.modCorrente;
            }

            set
            {
                container.modCorrente = value;
            }
        }

        //public float ModTempoDeReuso
        //{
        //    get
        //    {
        //        return container.modTempoDeReuso;
        //    }

        //    set
        //    {
        //        container.modTempoDeReuso = value;
        //    }
        //}

        public AttackNameId Nome
        {
            get
            {
                return container.nome;
            }
        }

        public int PotenciaCorrente
        {
            get
            {
                int retorno = container.potenciaCorrente + container.modCorrente;

                if (retorno > container.potenciaMaxima)
                    retorno = container.potenciaMaxima;
                else if (retorno < container.potenciaMinima)
                    retorno = container.potenciaMinima;

                return retorno;
            }
        }

        public float DistanciaDeRepulsao
        {
            get
            {
                return container.distanciaDeRepulsao;
            }
        }

        public float VelocidadeDeRepulsao
        {
            get { return container.velocidadeDeRepulsao; }
        }

        public float VelocidadeDeGolpe
        {
            get { return container.velocidadeDeGolpe; }
        }

        public float TempoDeDestroy
        {
            get
            {
                return container.tempoDeDestroy;
            }
        }

        public float TempoDeMoveMax
        {
            get
            {
                return container.tempoDeMoveMax;
            }
        }

        public float TempoDeMoveMin
        {
            get
            {
                return container.tempoDeMoveMin;
            }
        }

        //public float TempoDeReuso
        //{
        //    get
        //    {
        //        return container.tempoDeReuso;
        //    }
        //}

        public float TempoNoDano
        {
            get
            {
                return container.TempoNoDano;
            }
        }

        public PetTypeName Tipo
        {
            get
            {
                return container.tipo;
            }
        }

        public float CustoDeStamina
        {
            get
            {
                return container.custoDeStamina;
            }

            set
            {
                container.custoDeStamina = value;
            }
        }

        public Vector3 DirDeREpulsao
        {
            get
            {
                return dirDeREpulsao;
            }

            set
            {
                dirDeREpulsao = value;
            }
        }

        public bool PodeNoAr
        {
            get
            {
                return container.podeNoAr;
            }
        }

        public SoundEffectID SomDoGolpe
        {
            get { return container.somDoGolpe; }
            protected set { container.somDoGolpe = value; }
        }

        public SoundEffectID SomDoImpacto
        {
            get { return container.somDoImpacto; }
            protected set { container.somDoImpacto = value; }
        }

        public virtual void InterromperGolpe(GameObject executor)
        {

        }

        public virtual void FinalizaEspecificoDoGolpe()
        {

        }

        public string GetNomeEmLinguas => NomeEmLinguas();

        public string NomeEmLinguas()
        {
            string[] arr = TextBank.RetornaListaDeTextoDoIdioma(TextKey.nomesDosGolpes).ToArray();
            if (arr.Length > (int)Nome)
                return arr[(int)Nome];
            else
            {
                Debug.LogError("O array de nomes de golpes não contem um nome para o ID= " + Nome);
                return Nome.ToString();// BancoDeTextos.falacoes[heroi.lingua]["listaDeGolpes"][(int)Nome];
            }
        }

        public static string NomeEmLinguas(AttackNameId nome)
        {
            string[] arr = TextBank.RetornaListaDeTextoDoIdioma(TextKey.nomesDosGolpes).ToArray();
            if (arr.Length > (int)nome)
                return arr[(int)nome];
            else
            {
                Debug.LogError("O array de nomes de golpes não contem um nome para o ID= " + nome);
                return nome.ToString();// BancoDeTextos.falacoes[heroi.lingua]["listaDeGolpes"][(int)Nome];
            }
        }
    }

}