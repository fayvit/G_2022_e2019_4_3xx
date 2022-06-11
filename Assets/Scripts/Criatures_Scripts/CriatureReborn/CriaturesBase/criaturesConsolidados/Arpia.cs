using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Arpia
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
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Arpia,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Voador },
                        distanciaFundamentadora = 0.01f,
                        meusAtributos = {
                    PV = { Taxa = 0.18f},
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
                        rollSpeed = 12,
                        walkSpeed = 6.2f,
                        runSpeed = 7.4f,
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