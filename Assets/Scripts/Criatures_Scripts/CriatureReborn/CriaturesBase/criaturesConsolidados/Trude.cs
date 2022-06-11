using FayvitMove;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Criatures2021
{
    public class Trude : PetBase
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {

            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("metarig/hips/spine/chest/neck/head"),
                Nome = AttackNameId.eletricidade,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.25f,
                AcimaDoChao = -0.1f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.cabecada,
                NivelDoGolpe = 1,
                Colisor = new Colisor("metarig/hips/spine/chest/neck/head",
                                          new Vector3(0.97f,0.12f,0.65f),
                                          new Vector3(-0.49f,0f,-0.3f)),
                TaxaDeUso = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.eletricidadeConcentrada,
                NivelDoGolpe = 2,
                Colisor = new Colisor("metarig/hips/spine/chest/neck/head"),
                DistanciaEmissora = 1f,
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.tempestadeEletrica,
                NivelDoGolpe = 7,
                Colisor = new Colisor("metarig/hips",_deslTrail:new Vector3(0,0.374f,-.25f),colisorScale: 2f),
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.garra,
                NivelDoGolpe = 8,
                Colisor = new Colisor("metarig/hips/spine/chest/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R",
                                          new Vector3(0.97f,0.12f,0.65f),
                                          new Vector3(-0.49f,0f,-0.3f)),
                TaxaDeUso = 0.5f,
                ModPersonagem = 15
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Trude,
                    alturaCamera = .1f,
                    distanciaCamera = 3f,
                    varHeightCamera = 1,
                    //alturaCameraLuta = 6,
                    distanciaCameraLuta = 4.5f,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Eletrico },
                        distanciaFundamentadora = 0.01f,
                        meusAtributos = {
                    PV = { Taxa = 0.19f,},
                    PE = { Taxa = 0.21f},
                    Ataque = { Taxa = 0.22f},
                    Defesa = { Taxa = 0.18f},
                    Poder = { Taxa = 0.2f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Eletrico)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new MoveFeatures()
                    {
                        walkSpeed = 5,
                        runSpeed = 6,
                        rollSpeed = 10,
                        jumpFeat = new JumpFeatures()
                        {
                            jumpHeight = 2f,
                            maxTimeJump = 1,
                            risingSpeed = 5,
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