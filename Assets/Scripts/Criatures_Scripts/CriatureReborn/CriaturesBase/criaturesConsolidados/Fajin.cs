using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Fajin
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
        new PetAttackDb()
        {
            NivelDoGolpe = -1,
            Nome = AttackNameId.sabreDeAsa,
            Colisor = new Colisor("Esqueleto/corpo1/corpo2/corpo3/pescoco/cabeca"),
            DistanciaEmissora = 2
        },
            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("Esqueleto/corpo1/corpo2/corpo3/pescoco/cabeca",
                                    new Vector3(0,0,0),
                                    new Vector3(-0.621f,-0,0.244f)),
                Nome = AttackNameId.bico,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.ventania,
                NivelDoGolpe = 1,
                Colisor = new Colisor("Esqueleto/corpo1/corpo2/corpo3/pescoco/cabeca",
                                   new Vector3(0.18f,0,0),
                                   new Vector3(-0.867f,-0.585f,-0.26f)),
                TaxaDeUso = 1f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.ventosCortantes,
                NivelDoGolpe = 2,
                Colisor = new Colisor("Esqueleto/corpo1/corpo2/corpo3/pescoco/cabeca"),
                DistanciaEmissora = 1f,
                AcimaDoChao = 0.15f,
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.sobreVoo,
                NivelDoGolpe = 7,
                Colisor = new Colisor("Esqueleto"),
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.olharEnfraquecedor,
                NivelDoGolpe = 9,
                Colisor = new Colisor("Esqueleto/corpo1/corpo2/corpo3/pescoco/cabeca",
                                   new Vector3(0.18f,0,0),
                                   new Vector3(-0.867f,-0.585f,-0.26f)),
                TaxaDeUso = .25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.anelDoOlhar,
                NivelDoGolpe = 11,
                Colisor = new Colisor("Esqueleto/corpo1/corpo2/corpo3/pescoco/cabeca",
                                   new Vector3(0.18f,0,0),
                                   new Vector3(-0.867f,-0.585f,-0.26f)),
                TaxaDeUso = .75f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 12,
                Colisor = new Colisor("Esqueleto/corpo1",
                                                  new Vector3(0,0,0),
                                                  new Vector3(-0.6f,-0,0)),
                Nome = AttackNameId.turbilhaoVeloz,
                TaxaDeUso = 1f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 14,
                Colisor = new Colisor("Esqueleto/corpo1/corpo2/corpo3/pescoco/cabeca"),
                Nome = AttackNameId.sabreDeAsa,
                TaxaDeUso = 1f,
                DistanciaEmissora = 1f,
                AcimaDoChao = 0.15f
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Fajin,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Voador },
                        distanciaFundamentadora = 0.01f,
                        meusAtributos = {
                    PV = { Taxa = 0.21f},
                    PE = { Taxa = 0.21f},
                    Ataque = { Taxa = 0.17f},
                    Defesa = { Taxa = 0.17f},
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
                        rollSpeed = 12,
                        walkSpeed = 6.2f,
                        runSpeed = 7.4f,
                        jumpFeat = new JumpFeatures()
                        {
                            jumpHeight = 2.1f,
                            maxTimeJump = 1,
                            risingSpeed = 5.4f,
                            fallSpeed = 15,
                            inJumpSpeed = 4.9f,
                            verticalDamping = 1.2f
                        }
                    }
                };
            }
        }
    }
}