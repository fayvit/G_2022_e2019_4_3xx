using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Xuash : PetBase
    {

        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
            new PetAttackDb()
            {
                Nome = AttackNameId.tapa,
                NivelDoGolpe = 1,
                Colisor = new Colisor("Armação",
                                    new Vector3(0,.35f,-.77f),
                                    new Vector3(0,-1.036f,1.021f)),
                TaxaDeUso = 0.5f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("Armação/Tronco/pescoco/Cabeca/BocaD"),
                Nome = AttackNameId.rajadaDeAgua,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 2,
                ModPersonagem = 0,
                Colisor = new Colisor("Armação/Tronco/pescoco/Cabeca/BocaD"),
                Nome = AttackNameId.turboDeAgua,
                TaxaDeUso = 1.25f,
                DistanciaEmissora = 0.5f
            },new PetAttackDb()
            {
                NivelDoGolpe = 7,
                ModPersonagem = 0,
                Colisor = new Colisor("Armação/Tronco",
                                                  new Vector3(0,0,0),
                                                  new Vector3(-0.26f,-0,0)),
                Nome = AttackNameId.hidroBomba,
                TaxaDeUso = 1.25f
            },new PetAttackDb()
            {
                NivelDoGolpe = 9,
                Colisor = new Colisor("Armação/Tronco",
                                                  new Vector3(0,0,0),
                                                  new Vector3(-0.26f,-0,0)),
                Nome = AttackNameId.impulsoAquatico,
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 12,
                ModPersonagem = 0,
                Colisor = new Colisor("Armação/Tronco/pescoco/Cabeca/"),
                Nome = AttackNameId.cabecada,
                TaxaDeUso = .25f,
                DistanciaEmissora = 0.5f
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Xuash,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Agua },
                        distanciaFundamentadora = -0.01f,
                        meusAtributos = {
                            PV = { Taxa = 0.19f,},
                            PE = { Taxa = 0.21f},
                            Ataque = { Taxa = 0.19f},
                            Defesa = { Taxa = 0.22f},
                            Poder = { Taxa = 0.19f}
                        },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Agua)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new MoveFeatures()
                    {
                        rollSpeed = 12,
                        walkSpeed = 5,
                        runSpeed = 6,
                        jumpFeat = new JumpFeatures()
                        { 
                         fallSpeed = 20,
                         jumpHeight = 2,
                         maxTimeJump=1,
                         risingSpeed = 5,
                         inJumpSpeed=4
                        }
                    }
                };
            }
        }
    }
}