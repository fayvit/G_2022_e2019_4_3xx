using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Izicuolo
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
            new PetAttackDb()
            {
                NivelDoGolpe = -1,
                Nome = AttackNameId.sabreDeBastao,
                Colisor = new Colisor("Armação.001/Corpo/"),
                AcimaDoChao = 0.1f,
                DistanciaEmissora = 2
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("Armação.001/Corpo/braco_R/punho_R/punho_R.001",
                                         new Vector3(0,0,0),
                                         new Vector3(0.382f,-0.192f,0.509f)),
                Nome = AttackNameId.bastao,
                TaxaDeUso = 0.5f,
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.gosmaDeInseto,
                NivelDoGolpe = 1,
                Colisor = new Colisor("Armação.001/Corpo/",
                                                new Vector3(0,0,0.3f),
                                           new Vector3(-0.2f,0f,0.723f)),
                TaxaDeUso = 0.65f,
                AcimaDoChao = 0.1f
            },new PetAttackDb()
            {
                Nome = AttackNameId.gosmaAcida,
                NivelDoGolpe = 2,
                Colisor = new Colisor("Armação.001/Corpo/",
                                   new Vector3(0.18f,0,0),
                                   new Vector3(-0.867f,-0.585f,-0.26f)),
                TaxaDeUso = 1f,
                AcimaDoChao = 0.1f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.multiplicar,
                NivelDoGolpe = 7,
                Colisor = new Colisor(),
                TaxaDeUso = 1.25f
            },new PetAttackDb()
            {
                Nome = AttackNameId.olharMal,
                NivelDoGolpe = 8,
                Colisor = new Colisor("Armação.001/Corpo/",
                                   new Vector3(0.18f,0,0),
                                   new Vector3(-0.867f,-0.585f,-0.26f)),
                TaxaDeUso = .125f,
                AcimaDoChao = 0.1f
            },
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Izicuolo,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Inseto },
                        distanciaFundamentadora = -0.01f,
                        meusAtributos = {
                    PV = { Taxa = 0.19f,},
                    PE = { Taxa = 0.21f},
                    Ataque = { Taxa = 0.19f},
                    Defesa = { Taxa = 0.19f},
                    Poder = { Taxa = 0.22f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Inseto)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new MoveFeatures()
                    {
                        walkSpeed = 4.5f,
                        runSpeed = 5.5f,
                        rollSpeed = 9.75f,
                         jumpFeat= new JumpFeatures()
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