using UnityEngine;
using System.Collections.Generic;

namespace Criatures2021
{
    public class Iruin:PetBase
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("Esqueleto/gomo1/cabeca"),
                Nome = AttackNameId.gosmaDeInseto,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.chicoteDeCalda,
                NivelDoGolpe = 1,
                Colisor = new Colisor("Esqueleto/gomo2/gomo3/rabo",
                                          new Vector3(0,0,0),
                                          new Vector3(-0.444f,0,0f),2),
            TaxaDeUso = 0.5f
            },new PetAttackDb()
            {
                NivelDoGolpe = 2,
                ModPersonagem = 0,
                Colisor = new Colisor("Esqueleto/gomo1/cabeca"),
                Nome = AttackNameId.gosmaAcida,
                TaxaDeUso = 1.25f,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 7,
                Colisor = new Colisor(),
                Nome = AttackNameId.multiplicar,
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 8,
                ModPersonagem = 0,
                Colisor = new Colisor("Esqueleto/gomo1/cabeca"),
                Nome = AttackNameId.olharEnfraquecedor,
                TaxaDeUso = .1f,
                DistanciaEmissora = 0.125f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 10,
                ModPersonagem = 0,
                Colisor = new Colisor("Esqueleto/gomo1/"),
                Nome = AttackNameId.deslizamentoNaGosma,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.125f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 12,
                Colisor = new Colisor("Esqueleto/gomo1/cabeca"),
                Nome = AttackNameId.cabecada,
                TaxaDeUso = .5f,
            },
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Iruin,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Inseto },
                        distanciaFundamentadora = 0.01f,
                        meusAtributos = {
                    PV = { Taxa = 0.19f},
                    PE = { Taxa = 0.2f},
                    Ataque = { Taxa = 0.17f},
                    Defesa = { Taxa = 0.22f},
                    Poder = { Taxa = 0.22f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Inseto)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new FayvitMove.MoveFeatures()
                    {
                        walkSpeed = 4.5f,
                        runSpeed=6,
                        rollSpeed=10,
                        
                        jumpFeat = new FayvitMove.JumpFeatures()
                        {
                            jumpHeight = 1.8f,
                            maxTimeJump = 1,
                            risingSpeed = 5,
                            fallSpeed = 20,
                            inJumpSpeed = 3.8f,
                            verticalDamping = 1.2f
                        }
                    }
                };
            }
        }
    }
}