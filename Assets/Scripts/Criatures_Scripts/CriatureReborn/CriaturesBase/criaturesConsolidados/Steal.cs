using FayvitMove;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Criatures2021
{
    public class Steal :PetBase
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {

            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("metarig/hips/chest"),
                Nome = AttackNameId.eletricidade,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.25f,
                AcimaDoChao = 0.1f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.cabecada,
                NivelDoGolpe = 1,
                Colisor = new Colisor("metarig/hips/chest/neck/head",
                                          new Vector3(0.97f,0.12f,0.65f),
                                          new Vector3(-0.49f,0f,-0.3f)),
                TaxaDeUso = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.eletricidadeConcentrada,
                NivelDoGolpe = 2,
                Colisor = new Colisor("metarig/hips/chest"),
                DistanciaEmissora = 1f,
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.tempestadeEletrica,
                NivelDoGolpe = 7,
                Colisor = new Colisor("metarig/hips",_deslTrail:new Vector3(0,0.374f,-.25f),colisorScale: 2f),
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.impulsoEletrico,
                NivelDoGolpe = 9,
                Colisor = new Colisor("metarig/hips",_deslTrail:new Vector3(0,1,1),_deslColisor:new Vector3(0,0,-1)),
                TaxaDeUso = 1f
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Steal,
                    //alturaCamera = 4,
                    //distanciaCamera = 5.5f,
                    //alturaCameraLuta = 6,
                    //distanciaCameraLuta = 4.5f,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Eletrico },
                        distanciaFundamentadora = 0.01f,
                        meusAtributos = {
                    PV = { Taxa = 0.195f,},
                    PE = { Taxa = 0.205f},
                    Ataque = { Taxa = 0.18f},
                    Defesa = { Taxa = 0.21f},
                    Poder = { Taxa = 0.21f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Eletrico)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new MoveFeatures()
                    {
                        walkSpeed = 5,
                        runSpeed=6,
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
                    }
                };
            }
        }
    }
}