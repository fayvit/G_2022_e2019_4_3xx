using UnityEngine;
using System.Collections.Generic;
using FayvitMove;
using FayvitMessageAgregator;

namespace Criatures2021
{
    [System.Serializable]
    public class PetBase
    {
        [SerializeField] private PetName petId;
        [SerializeField] private PetFeatures petFeat;
        [SerializeField] private PetAttackManager atkManager;
        [SerializeField] private MoveFeatures movFeat;
        [SerializeField] private List<DatesForTemporaryStatus> statusTemporarios = new List<DatesForTemporaryStatus>();
        [SerializeField] private StaminaManager stManager;

        public List<PetAttackDb> GolpesPorAprender { get; private set; } = new List<PetAttackDb>();

        public float alturaCamera = .25f;
        public float distanciaCamera = 5.5f;
        public float varHeightCamera = .25f;
        public float alturaCameraLuta = 1f;
        public float distanciaCameraLuta = 6f;
        //public float varHeightFightcamera = 

        public PetBase() { }
        public PetBase(PetName X, int nivel = 1)
        {
            PetBase Y = PetFactory.GetPet(X);
            petFeat = Y.petFeat;
            atkManager = Y.GerenteDeGolpes;
            movFeat = Y.MovFeat;
            alturaCamera = Y.alturaCamera;
            distanciaCamera = Y.distanciaCamera;
            varHeightCamera = Y.varHeightCamera;
            alturaCameraLuta = Y.alturaCameraLuta;
            distanciaCameraLuta = Y.distanciaCameraLuta;
            petId = X;

            if (nivel > 1)
                PetFeat.IncrementaNivel(nivel);
            

            atkManager.meusGolpes = new List<PetAttackBase>();
            atkManager.meusGolpes.AddRange(GolpesAtivos(nivel, atkManager.listaDeGolpes.ToArray()));


            VerificaSomaDeTaxas();

            if (StManager == null)
                stManager = new StaminaManager();
        }

        void VerificaSomaDeTaxas()
        {
            PetAtributes a = petFeat.meusAtributos;
            float comoAssim = a.PV.Taxa + a.PE.Taxa + a.Ataque.Taxa + a.Defesa.Taxa + a.Poder.Taxa;
            if (comoAssim != 1)
            {
                Debug.Log("O criature " + petId.ToString() + " não tem a soma das taxas igual a 1: " + comoAssim);
            }
        }

        public PetAttackBase[] GolpesAtivos(int nivel, PetAttackDb[] listaGolpes)
        {
            List<PetAttackDb> L = new List<PetAttackDb>();
            int i = 0;
            //int N = -1;
            while (i < listaGolpes.Length)
            {
                if (listaGolpes[i].NivelDoGolpe <= nivel && listaGolpes[i].NivelDoGolpe > -1)
                {
                    if (L.Count < 4)
                        L.Add(listaGolpes[i]);
                    else
                    {
                        L[0] = L[1];
                        L[1] = L[2];
                        L[2] = L[3];
                        L[3] = listaGolpes[i];
                    }
                }
                i++;
            }

            PetAttackBase[] Y = new PetAttackBase[L.Count];
            for (i = 0; i < L.Count; i++)
            {
                Y[i] = AttackFactory.GetAttack(L[i].Nome);

            }
            return Y;
        }

        public static void EnergiaEVidaCheia(PetBase C)
        {
            PetAtributes A = C.petFeat.meusAtributos;
            A.PV.Corrente = A.PV.Maximo;
            A.PE.Corrente = A.PE.Maximo;
        }

        public void EstadoPerfeito()
        {
            EnergiaEVidaCheia(this);
            //int num = statusTemporarios.Count - 1;
            //for (int i = num; i >= 0; i--)
            //{
            //    Debug.LogError("Parte para ser substituida por um evento");



            //    //int num2 = GameController.g.ContStatus.StatusDoHeroi.Count - 1;
            //    //for (int j = num2; j >= 0; j--)
            //    //    if (statusTemporarios.IndexOf(GameController.g.ContStatus.StatusDoHeroi[j].Dados) == i)
            //    //        GameController.g.ContStatus.StatusDoHeroi[j].RetiraComponenteStatus();
            //}

            statusTemporarios.Clear();

            MessageAgregator<MsgClearNegativeStatus>.Publish(new MsgClearNegativeStatus()
            {
                target = this
            });
        }

        /*
        public bool SouOMesmoQue(PetBase C)
        {
            bool retorno = false;

            if (C.NomeID == nome
                &&
                C.petFeat.mNivel.Nivel==petFeat.mNivel.Nivel
                &&
                C.petFeat.mNivel.XP == petFeat.mNivel.XP
                &&
                C.petFeat.meusAtributos.PV.Corrente == petFeat.meusAtributos.PV.Corrente
                &&
                C.petFeat.meusAtributos.PV.Maximo == petFeat.meusAtributos.PV.Maximo
                &&
                C.petFeat.meusAtributos.PE.Corrente == petFeat.meusAtributos.PE.Corrente
                &&
                C.petFeat.meusAtributos.PE.Maximo == petFeat.meusAtributos.PE.Maximo
                )
            retorno = true;
            return retorno;
        }*/

        public object Clone()
        {
            PetBase retorno =
            new PetBase()
            {
                NomeID = NomeID,
                alturaCamera = alturaCamera,
                distanciaCamera = distanciaCamera,
                alturaCameraLuta = alturaCameraLuta,
                distanciaCameraLuta = distanciaCameraLuta,
                petFeat = new PetFeatures()
                {
                    meusTipos = (PetTypeName[])petFeat.meusTipos.Clone(),
                    distanciaFundamentadora = petFeat.distanciaFundamentadora,
                    meusAtributos = {
                    PV = { Taxa = petFeat.meusAtributos.PV.Taxa},
                    PE = { Taxa = petFeat.meusAtributos.PE.Taxa},
                    Ataque = { Taxa = petFeat.meusAtributos.Ataque.Taxa},
                    Defesa = { Taxa = petFeat.meusAtributos.Defesa.Taxa},
                    Poder = { Taxa = petFeat.meusAtributos.Poder.Taxa }
                    },
                    contraTipos = petFeat.contraTipos
                },
                GerenteDeGolpes = new PetAttackManager()
                {
                    listaDeGolpes = atkManager.listaDeGolpes,
                },
                MovFeat = new MoveFeatures()
                {
                    walkSpeed = movFeat.walkSpeed,
                    runSpeed = movFeat.runSpeed,
                    rotAlways = movFeat.rotAlways,
                    rollSpeed = movFeat.rollSpeed,
                    jumpFeat = new JumpFeatures()
                    { 
                        fallSpeed = movFeat.jumpFeat.fallSpeed,
                        horizontalDamping = movFeat.jumpFeat.horizontalDamping,
                        initialImpulse = movFeat.jumpFeat.initialImpulse,
                        inJumpSpeed = movFeat.jumpFeat.inJumpSpeed,
                        jumpHeight = movFeat.jumpFeat.jumpHeight,
                        maxTimeJump = movFeat.jumpFeat.maxTimeJump,
                        minTimeJump = movFeat.jumpFeat.minTimeJump,
                        risingSpeed = movFeat.jumpFeat.risingSpeed,
                        verticalDamping = movFeat.jumpFeat.verticalDamping
                    }
                }
            };
            return retorno;
        }

        public PetBase ClonePetBase()
        {
            return Clone() as PetBase;
        }

        public StaminaManager StManager => stManager;

        public PetFeatures PetFeat
        {
            get { return petFeat; }
            set { petFeat = value; }
        }

        public PetAttackManager GerenteDeGolpes
        {
            get { return atkManager; }
            set { atkManager = value; }
        }

        public MoveFeatures MovFeat
        {
            get { return movFeat; }
            set { movFeat = value; }
        }

        public PetName NomeID
        {
            get { return petId; }
            set { petId = value; }
        }

        public List<DatesForTemporaryStatus> StatusTemporarios
        {
            get { return statusTemporarios; }
            set { statusTemporarios = value; }
        }

        public IGerenciadorDeExperiencia G_XP
        {
            get { return petFeat.mNivel; }
        }

        public string GetNomeEmLinguas
        {
            get { return NomeID.ToString(); }
        }

        public static string NomeEmLinguas(PetName p)
        {
            return p.ToString();
        }
    }

    public struct MsgClearNegativeStatus : IMessageBase
    {
        public PetBase target;
    }
}