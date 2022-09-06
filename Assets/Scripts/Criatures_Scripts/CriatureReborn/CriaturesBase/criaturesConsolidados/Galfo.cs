using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Galfo
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
        // Golpes aprendidos apenas com pergaminhos
        new PetAttackDb()
            {
                Nome = AttackNameId.ventania,
                NivelDoGolpe = -1,
                Colisor = new Colisor("Armature/spine"),
                TaxaDeUso = 0.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.ventosCortantes,
                NivelDoGolpe = -1,
                Colisor = new Colisor("Armature/spine"),
                DistanciaEmissora = 1f,
                AcimaDoChao = 0.15f,
                TaxaDeUso = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.sobreVoo,
                NivelDoGolpe = -1,
                Colisor = new Colisor("Armature/spine/thigh.R/shin.R/foot.R"),
                TaxaDeUso = 0.5f
            },
            /*
            
            fim dos golpes aprendidos apenas com pergaminhos
            
            */
            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("Armature/spine/thigh.R/shin.R/foot.R"),
                Nome = AttackNameId.chute,
                TaxaDeUso = .5f,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 8,
                Colisor = new Colisor("Armature/spine"),
                Nome = AttackNameId.gosmaDeInseto,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.cabecada,
                NivelDoGolpe = 1,
                Colisor = new Colisor("Armature/spine/spine.001/spine.002/spine.003/neck/neck.001/head"),
            TaxaDeUso = 0.5f
            },new PetAttackDb()
            {
                NivelDoGolpe = 2,
                ModPersonagem = 8,
                Colisor = new Colisor("Armature/spine"),
                Nome = AttackNameId.gosmaAcida,
                TaxaDeUso = 1.25f,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.multiplicar,
                NivelDoGolpe = 7,
                Colisor = new Colisor(),
                TaxaDeUso = 1f,
                ModPersonagem=8
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 11,
                ModPersonagem = 0,
                Colisor = new Colisor("Armature/spine"),
                Nome = AttackNameId.olharMal,
                TaxaDeUso = .125f,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 13,
                ModPersonagem = 9,
                Colisor = new Colisor("Armature/spine/spine.001/spine.002/spine.003/upper_arm.R/forearm.R/hand.R/hand.R_end"),
                Nome = AttackNameId.garra,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.5f
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Galfo,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Inseto },
                        distanciaFundamentadora = 0.05f,
                        meusAtributos = {
                    PV = { Taxa = 0.21f},
                    PE = { Taxa = 0.21f},
                    Ataque = { Taxa = 0.22f},
                    Defesa = { Taxa = 0.17f},
                    Poder = { Taxa = 0.19f}
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
                        rollSpeed = 10,
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