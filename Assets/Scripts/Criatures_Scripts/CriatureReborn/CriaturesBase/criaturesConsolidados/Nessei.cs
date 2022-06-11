using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Nessei
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
        new PetAttackDb()
            {
                NivelDoGolpe = -1,
                ModPersonagem = 0.1f,
                Colisor = new Colisor("esqueleto/centro/c1/c2/c3/cabeca/bocaB"),
                Nome = AttackNameId.olharMal,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.5f,
                TempoDeInstancia = 0.25f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0.1f,
                Colisor = new Colisor("esqueleto/centro/c1/c2/c3/cabeca/bocaB"),
                Nome = AttackNameId.rajadaDeAgua,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.5f,
                TempoDeInstancia = 0.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.chicoteDeCalda,
                NivelDoGolpe = 1,
                ModPersonagem = 0.2f,
                Colisor = new Colisor("esqueleto/centroReverso/r1/r2/r3/rabo",
                                                  new Vector3(0,0f,0),
                                                  new Vector3(-0.093f,0.135f,-0.37f)),
                TaxaDeUso = 0.5f
            },new PetAttackDb()
            {
                NivelDoGolpe = 2,
                ModPersonagem = 0.3f,
                Colisor = new Colisor("esqueleto/centro/c1/c2/c3/cabeca/bocaB"),
                Nome = AttackNameId.turboDeAgua,
                TaxaDeUso = 1.25f,
                DistanciaEmissora = 0.5f,
                TempoDeInstancia = 0.15f
            },new PetAttackDb()
            {
                NivelDoGolpe = 7,
                ModPersonagem = 0.3f,
                Colisor = new Colisor("esqueleto/centroReverso",
                                                            new Vector3(0,-.24f,0),
                                                            new Vector3(0,0,0),1.5f),
                Nome = AttackNameId.hidroBomba,
                TaxaDeUso = 1.25f
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    alturaCameraLuta=1.5f,
                    distanciaCameraLuta = 5f,
                    NomeID = PetName.Nessei,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Agua },
                        distanciaFundamentadora = 0.11f,
                        meusAtributos = {
                    PV = { Taxa = 0.19f,},
                    PE = { Taxa = 0.21f},
                    Ataque = { Taxa = 0.26f},
                    Defesa = { Taxa = 0.17f},
                    Poder = { Taxa = 0.17f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Agua)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new MoveFeatures()
                    {
                        walkSpeed = 5,
                        runSpeed = 6,
                        rollSpeed = 10,
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