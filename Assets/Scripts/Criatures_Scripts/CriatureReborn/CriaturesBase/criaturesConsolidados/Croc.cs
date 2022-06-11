using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Croc
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {

            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("metarig/hips/spine/chest/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R",
                                                  new Vector3(0f,0.13f,1.26f),
                                                  new Vector3(-0.0776f,0.022f,-0.07f)),
                Nome = AttackNameId.cascalho,
                TaxaDeUso = 1,

                DistanciaEmissora = 0.5f,
                TempoDeInstancia = 0.05f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.tapa,
                NivelDoGolpe = 1,
                Colisor = new Colisor("metarig/hips/spine/chest/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R",
                                                  new Vector3(0,0,-.45f),
                                                  new Vector3(-0.296f,0.401f,0.508f)),
                TaxaDeUso = 1f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.pedregulho,
                NivelDoGolpe = 2,
                Colisor = new Colisor("metarig/hips/spine/chest/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R"),
                DistanciaEmissora = 1f,
                AcimaDoChao = 0.15f,
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.avalanche,
                NivelDoGolpe = 7,
                Colisor = new Colisor("metarig/hips/spine/chest/neck/head",new Vector3(-0f,0.83F,-0.36f),Vector3.zero,colisorScale:1.5F),
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.olharEnfraquecedor,
                NivelDoGolpe = 8,
                Colisor = new Colisor("metarig/hips/spine/chest/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R"),
                DistanciaEmissora = 1f,
                AcimaDoChao = 0.15f,
                TaxaDeUso = 0.25f
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Croc,
                    //alturaCamera = 4,
                    //distanciaCamera = 5.5f,
                    //alturaCameraLuta = 6,
                    //distanciaCameraLuta = 4.5f,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Pedra },
                        distanciaFundamentadora = 0.1f,
                        meusAtributos = {
                    PV = { Taxa = 0.21f,},
                    PE = { Taxa = 0.15f},
                    Ataque = { Taxa = 0.24f},
                    Defesa = { Taxa = 0.24f},
                    Poder = { Taxa = 0.16f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Pedra)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new MoveFeatures()
                    {
                        walkSpeed = 4.65f,
                        rollSpeed = 10,
                        runSpeed = 5.65f,
                        jumpFeat = new JumpFeatures()
                        {
                            jumpHeight = 1.8f,
                            maxTimeJump = 1,
                            risingSpeed = 4.7f,
                            fallSpeed = 20,
                            inJumpSpeed = 4,
                            verticalDamping = 1.2f
                        }
                    }
                };
            }
        }
    }
}