using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Cabecu
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {

            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("Armação/base/corpo"),
                Nome = AttackNameId.rajadaDeTerra,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.3f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.chute,
                NivelDoGolpe = 1,
                Colisor = new Colisor("Armação/base/pernaD/peD1/peD2",
                                           new Vector3(0,0f,0),
                                           new Vector3(-0.156f,0.096f,-0.212f)),
                TaxaDeUso = 1f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.vingancaDaTerra,
                NivelDoGolpe = 2,
                Colisor = new Colisor("Armação/base/corpo"),
                TaxaDeUso = 1.25f,
                DistanciaEmissora = 0.25f

            },
            new PetAttackDb()
            {
                Nome = AttackNameId.cortinaDeTerra,
                NivelDoGolpe = 7,
                Colisor = new Colisor(),
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.olharMal,
                NivelDoGolpe = 8,
                Colisor = new Colisor("Armação/base/corpo"),
                TaxaDeUso = 1.25f

            },
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Cabecu,
                    //alturaCamera = 4,
                    //distanciaCamera = 5.5f,
                    //alturaCameraLuta = 6,
                    //distanciaCameraLuta = 4.5f,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Terra },
                        distanciaFundamentadora = 0.01f,
                        meusAtributos = {
                    PV = { Taxa = 0.21f,},
                    PE = { Taxa = 0.21f},
                    Ataque = { Taxa = 0.18f},
                    Defesa = { Taxa = 0.18f},
                    Poder = { Taxa = 0.22f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Terra)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new MoveFeatures()
                    {
                        walkSpeed = 4.7f,
                        runSpeed = 5.6f,
                        rollSpeed = 10,
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