using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Quaster
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 3,
                Colisor = new Colisor("Armature/Bone/Bone.001/Bone.001_R/Bone.001_R.001/Bone.001_R.002",
                                        new Vector3(0,0,0),
                                        new Vector3(-0.26f,-0,0),.8f),
                Nome = AttackNameId.tapa,
                TaxaDeUso = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.cabecada,
                ModPersonagem = 8,
                NivelDoGolpe = 1,
                Colisor = new Colisor("Armature/Bone/Bone.001/Bone.002/Bone.003",
                                            new Vector3(0,0f,0),
                                            new Vector3(-0.289f,.13f,-0.115f)),
                TaxaDeUso = 0.5f
            },new PetAttackDb()
            {
                NivelDoGolpe = 2,
                ModPersonagem = 5,
                Colisor = new Colisor("Armature/Bone",
                                              new Vector3(0,0,1.4f),
                                              new Vector3(-0.365f,0.113f,-0.325f)),
                Nome = AttackNameId.sobreSalto,
                TaxaDeUso = 0.5f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 6,
                ModPersonagem = 20,
                Colisor = new Colisor("Armature",_deslColisor:new Vector3(0,-1,.33f)),
                Nome = AttackNameId.dentada,
                TaxaDeUso = 0.5f,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 8,
                Colisor = new Colisor("Armature"),
                Nome = AttackNameId.anelDoOlhar,
                TaxaDeUso = 0.5f,
                DistanciaEmissora = 0.5f,
                ModPersonagem=5
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 10,
                Colisor = new Colisor("Armature"),
                Nome = AttackNameId.olharEnfraquecedor,
                TaxaDeUso = 0.125f,
                DistanciaEmissora = 0.5f,
                ModPersonagem=1
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Quaster,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Normal },
                        distanciaFundamentadora = 0.02f,
                        meusAtributos = {
                    PV = { Taxa = 0.19f,},
                    PE = { Taxa = 0.22f},
                    Ataque = { Taxa = 0.17f},
                    Defesa = { Taxa = 0.2f},
                    Poder = { Taxa = 0.22f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Normal)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new MoveFeatures()
                    {
                        walkSpeed = 6f,
                        runSpeed=7f,
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
                    },
                };
            }
        }
    }
}