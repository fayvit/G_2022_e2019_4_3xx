using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{

    public class Vampire : PetBase
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
            new PetAttackDb()
            {
                NivelDoGolpe = -1,
                Nome = AttackNameId.sabreDeAsa,
                Colisor = new Colisor("metarig",
                                   new Vector3(0.18f,0,0),
                                   new Vector3(-0.867f,-0.585f,-0.26f)),
                DistanciaEmissora = 1.5f,
                AcimaDoChao = 0.25f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("metarig/hips/spine/chest/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.02.R",
                                          new Vector3(0,0,0),
                                          new Vector3(-0.11f,-0,0.244f)),
                Nome = AttackNameId.garra,
                TaxaDeUso = 0.5f,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.dentada,
                NivelDoGolpe = 1,
                Colisor = new Colisor("metarig/hips/spine/chest/neck/head"),
                TaxaDeUso = 0.65f
            },new PetAttackDb()
            {
                Nome = AttackNameId.ventania,
                NivelDoGolpe = 2,
                Colisor = new Colisor("metarig",
                                   new Vector3(0.18f,0,0),
                                   new Vector3(-0.867f,-0.585f,-0.26f)),
                TaxaDeUso = 1f,
                AcimaDoChao = .25f
            },new PetAttackDb()
            {
                Nome = AttackNameId.ventosCortantes,
                NivelDoGolpe = 3,
                Colisor = new Colisor("metarig"),
                DistanciaEmissora = 1f,
                AcimaDoChao = 0.15f,
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.sobreVoo,
                NivelDoGolpe = 11,
                Colisor = new Colisor("metarig",
                                                   new Vector3(0,0,0),
                                                   new Vector3(-0.11f,-0,0.244f)),
                TaxaDeUso = 1.25f
            },new PetAttackDb()
            {
                Nome = AttackNameId.olharMal,
                NivelDoGolpe = 18,
                Colisor = new Colisor("metarig"),
                DistanciaEmissora = 1f,
                AcimaDoChao = 0.15f,
                TaxaDeUso = 0.125f
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Vampire,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Voador },
                        distanciaFundamentadora = -0.01f,
                        meusAtributos = {
                    PV = { Taxa = 0.19f,},
                    PE = { Taxa = 0.2f},
                    Ataque = { Taxa = 0.19f},
                    Defesa = { Taxa = 0.18f},
                    Poder = { Taxa = 0.24f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Voador)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new MoveFeatures()
                    {
                        walkSpeed = 7,
                        runSpeed = 8,
                        rollSpeed = 12,
                        jumpFeat = new JumpFeatures()
                        {
                            jumpHeight = 2.2f,
                            maxTimeJump = 1,
                            risingSpeed = 5.5f,
                            fallSpeed = 15,
                            inJumpSpeed = 5,
                            verticalDamping = 1.2f
                        }
                    }
                };
            }
        }
    }
}