using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class On_Racani
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
            new PetAttackDb()
            {
                NivelDoGolpe = -1,
                Nome = AttackNameId.sabreDeEspada,
                Colisor = new Colisor("esqueleto/corpo/"),
                DistanciaEmissora = 2,
                AcimaDoChao = 0.25f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("esqueleto/corpo/anteBracoD/bracoD/maoD",
                                         new Vector3(0,0,0),
                                         new Vector3(-0.335f,-0.202f,0.147f)),
                Nome = AttackNameId.espada,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.chute,
                NivelDoGolpe = 1,
                Colisor = new Colisor("esqueleto/corpo/coxaD/pernaD/calcanharD",
                                          new Vector3(0,0,0),
                                          new Vector3(-0.25f,0.034f,-0.339f)),
                TaxaDeUso = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.sobreSalto,
                NivelDoGolpe = 2,
                Colisor = new Colisor("esqueleto/corpo/",
                                              new Vector3(0,0,1.4f),
                                              new Vector3(0f,0.113f,-0.292f)),
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.anelDoOlhar,
                NivelDoGolpe = 7,
                Colisor = new Colisor("esqueleto/corpo/"),
                DistanciaEmissora = 0.5f,
                TaxaDeUso = 1.25f
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.On_Racani,
                    //alturaCamera = 4,
                    //distanciaCamera = 5.5f,
                    //alturaCameraLuta = 6,
                    //distanciaCameraLuta = 4.5f,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Normal },
                        distanciaFundamentadora = 0.01f,
                        meusAtributos = {
                    PV = { Taxa = 0.195f,},
                    PE = { Taxa = 0.205f},
                    Ataque = { Taxa = 0.21f},
                    Defesa = { Taxa = 0.21f},
                    Poder = { Taxa = 0.18f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Normal)
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