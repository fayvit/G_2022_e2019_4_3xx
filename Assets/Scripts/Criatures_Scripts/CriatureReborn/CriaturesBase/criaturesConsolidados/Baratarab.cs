using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Baratarab
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
        // Golpes aprendidos apenas com pergaminhos
        new PetAttackDb()
            {
                Nome = AttackNameId.ventania,
                NivelDoGolpe = -1,
                Colisor = new Colisor("Armação/Bone.001/Bone/Bone.002"),
                TaxaDeUso = 0.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.ventosCortantes,
                NivelDoGolpe = -1,
                Colisor = new Colisor("Armação/Bone.001/Bone/Bone.002"),
                DistanciaEmissora = 1f,
                AcimaDoChao = 0.15f,
                TaxaDeUso = 0.5f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = -1,
                ModPersonagem = 3,
                Colisor = new Colisor("Armação/Bone.001"),
                Nome = AttackNameId.sobreSalto,
                TaxaDeUso = 0.5f
            },
            /*
            
            fim dos golpes aprendidos apenas com pergaminhos
            
            */
            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("Armação/Bone.001/Bone/Bone.002"),
                Nome = AttackNameId.gosmaDeInseto,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.cabecada,
                NivelDoGolpe = 1,
                Colisor = new Colisor("Armação/Bone.001/Bone/Bone.002",
                                          new Vector3(0,0,0),
                                          new Vector3(-0.283f,0.014f,-0.245f)),
            TaxaDeUso = 0.5f
            },new PetAttackDb()
            {
                NivelDoGolpe = 2,
                ModPersonagem = 0,
                Colisor = new Colisor("Armação/Bone.001/Bone/Bone.002"),
                Nome = AttackNameId.gosmaAcida,
                TaxaDeUso = 1.25f,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.multiplicar,
                NivelDoGolpe = 7,
                Colisor = new Colisor(),
                TaxaDeUso = 1.5f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 8,
                ModPersonagem = 0,
                Colisor = new Colisor("Armação/Bone.001/Bone/Bone.002"),
                Nome = AttackNameId.olharMal,
                TaxaDeUso = .125f,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.sobreVoo,
                NivelDoGolpe = 9,
                Colisor = new Colisor("Armação/Bone.001/Bone/",
                                                      new Vector3(0,0,0),
                                                      new Vector3(-0.163f,0.017f,0.139f)),
                TaxaDeUso = 0.5f
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Baratarab,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Inseto },
                        distanciaFundamentadora = 0.05f,
                        meusAtributos = {
                    PV = { Taxa = 0.21f},
                    PE = { Taxa = 0.21f},
                    Ataque = { Taxa = 0.24f},
                    Defesa = { Taxa = 0.17f},
                    Poder = { Taxa = 0.17f}
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
                        runSpeed = 6,
                        rollSpeed  = 10,
                        jumpFeat = new JumpFeatures()
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