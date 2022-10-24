using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Xosh : PetBase
    {

        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
            new PetAttackDb()
            {
                Nome = AttackNameId.tapa,
                NivelDoGolpe = 1,
                Colisor = new Colisor("metarig",
                                    new Vector3(0,.35f,-.77f),
                                    new Vector3(0,-1.036f,1.021f)),
                TaxaDeUso = 0.5f,
                ModPersonagem = 5
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 5,
                Colisor = new Colisor("metarig/spine/spine.001/spine.002/spine.003/spine.004/neck/head/bocaInferior"),
                Nome = AttackNameId.rajadaDeAgua,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 2,
                ModPersonagem = 5,
                Colisor = new Colisor("metarig/spine/spine.001/spine.002/spine.003/spine.004/neck/head/bocaInferior"),
                Nome = AttackNameId.turboDeAgua,
                TaxaDeUso = 1.25f,
                DistanciaEmissora = 0.5f
            },new PetAttackDb()
            {
                NivelDoGolpe = 7,
                ModPersonagem = 7,
                Colisor = new Colisor("metarig/spine",
                                                  new Vector3(0,0,0),
                                                  new Vector3(-0.26f,-0,0)),
                Nome = AttackNameId.hidroBomba,
                TaxaDeUso = 1.25f
            },new PetAttackDb()
            {
                NivelDoGolpe = 9,
                ModPersonagem = 7,
                Colisor = new Colisor("metarig/spine",
                                                  new Vector3(0,0,0),
                                                  new Vector3(-0.26f,-0,0)),
                Nome = AttackNameId.impulsoAquatico,
                TaxaDeUso = 1.25f
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Xosh,
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