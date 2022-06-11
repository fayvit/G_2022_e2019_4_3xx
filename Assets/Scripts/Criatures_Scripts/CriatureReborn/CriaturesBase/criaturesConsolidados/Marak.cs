using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Marak
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
        new PetAttackDb()
        {
            NivelDoGolpe = 1,
            Nome = AttackNameId.chifre,
            Colisor = new Colisor("Armação/corpo3/corpo2/corpo1/pescoco/cabeca/",
                                          new Vector3(0,-.5f,0),
                                          new Vector3(0,.97f,1.26f)),
            TaxaDeUso=1
        },
            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                Colisor = new Colisor("Armação/corpo3_R/",
                                         new Vector3(0,0,0.3f),
                                         new Vector3(-0.331f,-0.344f,-0.192f)),
                Nome = AttackNameId.garra,
                TaxaDeUso = .5f,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.sobreSalto,
                NivelDoGolpe = 2,
                Colisor = new Colisor("Armação/corpo3/",
                                         new Vector3(0,-0.71f,.37f),
                                         new Vector3(-0.37f,1.62f,-0.325f)),
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.anelDoOlhar,
                NivelDoGolpe = 7,
                Colisor = new Colisor("Armação/corpo3/corpo2/corpo1/pescoco/cabeca/"),
                DistanciaEmissora = .5f,                
                TaxaDeUso = .5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.olharEnfraquecedor,
                NivelDoGolpe = 8,
                Colisor = new Colisor("Armação/corpo3/corpo2/corpo1/pescoco/cabeca/"),
                TaxaDeUso = .5f,
                DistanciaEmissora=.125f
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Marak,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Normal },
                        distanciaFundamentadora = 0.01f,
                        meusAtributos = {
                    PV = { Taxa = 0.21f,},
                    PE = { Taxa = 0.2f},
                    Ataque = { Taxa = 0.21f},
                    Defesa = { Taxa = 0.18f},
                    Poder = { Taxa = 0.2f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Normal)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new MoveFeatures()
                    {
                        rollSpeed = 10,
                        walkSpeed = 5f,
                        runSpeed = 6f,
                        jumpFeat = new JumpFeatures()
                        {
                            jumpHeight = 2f,
                            maxTimeJump = 1,
                            risingSpeed = 5f,
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