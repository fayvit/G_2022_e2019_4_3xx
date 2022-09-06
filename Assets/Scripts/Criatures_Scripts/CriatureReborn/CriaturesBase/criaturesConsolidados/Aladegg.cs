using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Aladegg
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
        new PetAttackDb()
        {
            NivelDoGolpe = -1,
                Nome = AttackNameId.sabreDeAsa,
                Colisor = new Colisor("esqueleto/corpo",
                                       new Vector3(0.18f,0,0),
                                       new Vector3(-0.867f,-0.585f,-0.26f)),
                DistanciaEmissora = 1.5f,
                AcimaDoChao = 0.25f
        },
            new PetAttackDb()
            {
                Nome = AttackNameId.ventania,
                NivelDoGolpe = 1,
                Colisor = new Colisor("esqueleto/corpo",
                                   new Vector3(0.18f,0,0),
                                   new Vector3(-0.867f,-0.585f,-0.26f)),
                TaxaDeUso = 1f
            },
            //new PetAttackDb()
            //{
            //    Nome = AttackNameId.sobreSalto,
            //    NivelDoGolpe = 2,
            //    Colisor = new Colisor("esqueleto/corpo",
            //                             new Vector3(0,-0.46f,1.4f),
            //                             new Vector3(-0.365f,0.113f,-0.325f)),
            //    TaxaDeUso = 1.25f
            //},
            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("esqueleto/corpo/coxaD/pernaD/peD",
                                          new Vector3(0,0,0),
                                          new Vector3(-0.11f,-0,0.244f)),
                Nome = AttackNameId.chute,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.ventosCortantes,
                NivelDoGolpe = 2,
                Colisor = new Colisor("esqueleto/corpo"),
                DistanciaEmissora = 1f,
                AcimaDoChao = 0.15f,
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.sobreVoo,
                NivelDoGolpe = 7,
                Colisor = new Colisor("esqueleto/corpo/coxaD/pernaD/peD",
                                                   new Vector3(0,0,0),
                                                   new Vector3(-0.11f,-0,0.244f)),
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.olharEnfraquecedor,
                NivelDoGolpe = 8,
                Colisor = new Colisor("esqueleto/corpo"),
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
                    NomeID = PetName.Aladegg,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Voador },
                        distanciaFundamentadora = 0.01f,
                        meusAtributos = {
                     PV = { Taxa = 0.18f,},
                    PE = { Taxa = 0.21f},
                    Ataque = { Taxa = 0.21f},
                    Defesa = { Taxa = 0.18f},
                    Poder = { Taxa = 0.22f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Voador)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new FayvitMove.MoveFeatures()
                    {
                        rollSpeed = 10,
                        walkSpeed = 5f,
                        runSpeed = 6f,
                        jumpFeat = new JumpFeatures()
                        {
                            jumpHeight = 2.3f,
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