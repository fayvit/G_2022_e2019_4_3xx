using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Uiritibucu
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
        new PetAttackDb()
        {
            NivelDoGolpe = 1,
            ModPersonagem = 28,
            Colisor = new Colisor("esqueleto/corpo/corpo_R/corpo_R.001/corpo_R.002",
                                        new Vector3(0,-0.16f,-1.12f),
                                        new Vector3(0,.447f,1.357f)),
            Nome = AttackNameId.chicoteDeMao,
            TaxaDeUso = 0.5f
        },
            new PetAttackDb()
            {
                Nome = AttackNameId.dentada,
                ModPersonagem = 10,
                NivelDoGolpe = 1,
                Colisor = new Colisor("esqueleto/corpo/",
                                                new Vector3(0,0,0.3f),
                                           new Vector3(-0.2f,0f,0.723f)),
                TaxaDeUso = 0.5f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 2,
                ModPersonagem = 8,
                Colisor = new Colisor("esqueleto/corpo/",
                                              new Vector3(0,0,0f),
                                              new Vector3(0.181f,0f,0.075f)),
                Nome = AttackNameId.sobreSalto,
                TaxaDeUso = 0.5f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 7,
                Colisor = new Colisor("esqueleto"),
                Nome = AttackNameId.anelDoOlhar,
                TaxaDeUso = 0.5f,
                DistanciaEmissora = 0.5f,
                ModPersonagem = 5
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 8,
                Colisor = new Colisor("esqueleto"),
                Nome = AttackNameId.olharMal,
                TaxaDeUso = 0.125f,
                DistanciaEmissora = 0.5f,
                ModPersonagem=1,
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 12,
                Colisor = new Colisor("esqueleto"),
                Nome = AttackNameId.ataqueEmGiro,
                TaxaDeUso = 1.125f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 16,
                Colisor = new Colisor("esqueleto"),
                Nome = AttackNameId.olharEnfraquecedor,
                TaxaDeUso = 0.125f,
                DistanciaEmissora = 0.5f,
                ModPersonagem=7,
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Uiritibucu,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Normal },
                        distanciaFundamentadora = 0.02f,
                        meusAtributos = {
                    PV = { Taxa = 0.19f},
                    PE = { Taxa = 0.17f},
                    Ataque = { Taxa = 0.22f},
                    Defesa = { Taxa = 0.2f},
                    Poder = { Taxa = 0.22f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Normal)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new MoveFeatures()
                    {
                        rollSpeed = 9,
                        walkSpeed = 5.5f,
                        runSpeed = 7.5f,
                        jumpFeat = new JumpFeatures()
                        {
                            jumpHeight = 3f,
                            maxTimeJump = 1,
                            risingSpeed = 8f,
                            fallSpeed = 20,
                            inJumpSpeed = 5,
                            verticalDamping = 1.2f
                        }
                    }
                };
            }
        }
    }
}