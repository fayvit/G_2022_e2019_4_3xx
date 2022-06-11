using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Rabitler
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
            /*golpes com pergaminhos*/
            new PetAttackDb()
            {
                Nome = AttackNameId.olharEnfraquecedor,
                NivelDoGolpe = -1,
                Colisor = new Colisor("Armação/corpoBase/corpoP/bracoD/maoD"),
                DistanciaEmissora = 1f,
                AcimaDoChao = 0.15f,
                TaxaDeUso = 0.25f
            },
            /***********************************/
            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("Armação/corpoBase/corpoP/bracoD/maoD"),
                Nome = AttackNameId.cascalho,
                TaxaDeUso = 1,

                DistanciaEmissora = 0.5f,
                TempoDeInstancia = 0.05f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.dentada,
                NivelDoGolpe = 1,
                Colisor = new Colisor("Armação/corpoBase/corpoP/pescoco/cabeca",
                                                  new Vector3(0,0,-.45f),
                                                  new Vector3(-0.296f,0.401f,0.508f)),
                TaxaDeUso = 1f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.pedregulho,
                NivelDoGolpe = 2,
                Colisor = new Colisor("Armação/corpoBase/corpoP/bracoD/maoD"),
                DistanciaEmissora = 1f,
                AcimaDoChao = 0.15f,
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.avalanche,
                NivelDoGolpe = 7,
                Colisor = new Colisor("Armação/corpoBase/corpoP/pescoco/cabeca",new Vector3(-0f,0.83F,-0.36f),Vector3.zero,colisorScale:1.5F),
                TaxaDeUso = 1.25f
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Rabitler,
                    //alturaCamera = 4,
                    //distanciaCamera = 5.5f,
                    //alturaCameraLuta = 6,
                    //distanciaCameraLuta = 4.5f,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Pedra },
                        distanciaFundamentadora = 0.1f,
                        meusAtributos = {
                    PV = { Taxa = 0.23f,},
                    PE = { Taxa = 0.16f},
                    Ataque = { Taxa = 0.23f},
                    Defesa = { Taxa = 0.23f},
                    Poder = { Taxa = 0.15f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Pedra)
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
                            jumpHeight = 2.3f,
                            maxTimeJump = 1,
                            risingSpeed = 5.3f,
                            fallSpeed = 18,
                            inJumpSpeed = 5,
                            verticalDamping = 1.2f
                        }
                    }
                };
            }
        }
    }
}