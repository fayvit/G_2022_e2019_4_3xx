using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Wisks
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("esqueleto/corpo.001/coluna/anteBraco_R/braco_R/mao_R",
                                        new Vector3(0,0,0),
                                        new Vector3(-0.26f,-0,0),.8f),
                Nome = AttackNameId.tapa,
                TaxaDeUso = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.cabecada,
                ModPersonagem = 1.5f,
                NivelDoGolpe = 1,
                Colisor = new Colisor("esqueleto/corpo.001/coluna/pescoco/cabeca",
                                            new Vector3(0,0f,0),
                                            new Vector3(-0.289f,.13f,-0.115f)),
                TaxaDeUso = 0.5f
            },new PetAttackDb()
            {
                NivelDoGolpe = 2,
                ModPersonagem = 0,
                Colisor = new Colisor("esqueleto/corpo.001/",
                                              new Vector3(0,0,1.4f),
                                              new Vector3(-0.365f,0.113f,-0.325f)),
                Nome = AttackNameId.sobreSalto,
                TaxaDeUso = 0.5f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 6,
                ModPersonagem = 16f,
                Colisor = new Colisor("esqueleto"),
                Nome = AttackNameId.dentada,
                TaxaDeUso = 0.5f,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 8,
                Colisor = new Colisor("esqueleto"),
                Nome = AttackNameId.anelDoOlhar,
                TaxaDeUso = 0.5f,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 10,
                Colisor = new Colisor("esqueleto"),
                Nome = AttackNameId.olharEnfraquecedor,
                TaxaDeUso = 0.125f,
                DistanciaEmissora = 0.5f
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Wisks,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Normal },
                        distanciaFundamentadora = 0.02f,
                        meusAtributos = {
                    PV = { Taxa = 0.19f,},
                    PE = { Taxa = 0.22f},
                    Ataque = { Taxa = 0.17f},
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
                        walkSpeed = 5.5f,
                        runSpeed=6.5f,
                        rollSpeed=10,
                        jumpFeat = new JumpFeatures()
                        {
                            jumpHeight = 2f,
                            maxTimeJump = 1,
                            risingSpeed = 5,
                            fallSpeed = 20,
                            inJumpSpeed = 4,
                            verticalDamping = 1.2f
                        }
                    },
                };
            }
        }
    }
}