using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Rocketler
    {
        static List<PetAttackDb > listaDosGolpes = new List<PetAttackDb>()
        {

            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("Esqueleto/hips/spine/chest/upper_arm.R/forearm.R/hand.R",
                                    new Vector3(0,0,0),
                                    new Vector3(-0.621f,-0,0.244f)),
                Nome = AttackNameId.cascalho,
                TaxaDeUso = 1,

                DistanciaEmissora = 0.5f,
                TempoDeInstancia = 0.05f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.cabecada,
                NivelDoGolpe = 1,
                Colisor = new Colisor("Esqueleto/hips/spine/chest/neck/head",
                                                  new Vector3(0,0,-.45f),
                                                  new Vector3(-0.296f,0.401f,0.508f)),
                TaxaDeUso = 1f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.pedregulho,
                NivelDoGolpe = 2,
                Colisor = new Colisor("Esqueleto/hips/spine/chest/upper_arm.R/forearm.R/hand.R"),
                DistanciaEmissora = 1f,
                AcimaDoChao = 0.15f,
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.avalanche,
                NivelDoGolpe = 7,
                Colisor = new Colisor("Esqueleto/hips/spine/chest/neck/head",new Vector3(-0f,0.83F,-0.36f),Vector3.zero,colisorScale:1.5F),
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.olharEnfraquecedor,
                NivelDoGolpe = 8,
                Colisor = new Colisor("Esqueleto/hips/spine/chest/upper_arm.R/forearm.R/hand.R"),
                DistanciaEmissora = 1f,
                AcimaDoChao = 0.15f,
                TaxaDeUso = .125f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.olharMal,
                NivelDoGolpe = 16,
                Colisor = new Colisor("Esqueleto/hips/spine/chest/upper_arm.R/forearm.R/hand.R"),
                DistanciaEmissora = 1f,
                AcimaDoChao = 0.15f,
                TaxaDeUso = .125f
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Rocketler,
                    //alturaCamera = 4,
                    //distanciaCamera = 5.5f,
                    //alturaCameraLuta = 6,
                    //distanciaCameraLuta = 4.5f,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Pedra },
                        distanciaFundamentadora = 0.1f,
                        meusAtributos = {
                    PV = { Taxa = 0.18f,},
                    PE = { Taxa = 0.21f},
                    Ataque = { Taxa = 0.21f},
                    Defesa = { Taxa = 0.22f},
                    Poder = { Taxa = 0.18f}
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
                        rollSpeed=10,
                        runSpeed = 5.65f,
                        jumpFeat = new JumpFeatures()
                        {
                            jumpHeight = 2.8f,
                            maxTimeJump = 1,
                            risingSpeed = 7.5f,
                            fallSpeed = 15,
                            inJumpSpeed = 7,
                            verticalDamping = 1.2f
                        }
                    }
                };
            }
        }
    }
}