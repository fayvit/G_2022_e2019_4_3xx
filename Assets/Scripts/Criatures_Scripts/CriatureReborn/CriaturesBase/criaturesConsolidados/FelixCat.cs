using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class FelixCat
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {

            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 10,
                Colisor = new Colisor("metarig/hips/"),
                Nome = AttackNameId.psicose,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.garra,
                NivelDoGolpe = 1,
                Colisor = new Colisor("metarig/hips/spine/chest/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.02.R"),
                TaxaDeUso = 0.5f,
                ModPersonagem = 25
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.bolaPsiquica,
                NivelDoGolpe = 2,
                ModPersonagem=10,
                Colisor = new Colisor("metarig/hips"),
                DistanciaEmissora = 1f,
                AcimaDoChao = 0.15f,
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.teletransporte,
                NivelDoGolpe = 7,
                ModPersonagem=10,
                Colisor = new Colisor(),
                TaxaDeUso = 1.25f
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.FelixCat,
                    //alturaCamera = 4,
                    //distanciaCamera = 5.5f,
                    //alturaCameraLuta = 6,
                    //distanciaCameraLuta = 4.5f,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Psiquico },
                        distanciaFundamentadora = 0.03f,
                        meusAtributos = {
                    PV = { Taxa = 0.185f,},
                    PE = { Taxa = 0.185f},
                    Ataque = { Taxa = 0.23f},
                    Defesa = { Taxa = 0.19f},
                    Poder = { Taxa = 0.21f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Psiquico)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new MoveFeatures()
                    {
                        walkSpeed = 4.65f,
                        rollSpeed = 10,
                        runSpeed = 5.65f,
                        jumpFeat = new JumpFeatures()
                        {
                            jumpHeight = 1.8f,
                            maxTimeJump = 1,
                            risingSpeed = 4.7f,
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