using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Grewstick
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
        new PetAttackDb()
        {
            NivelDoGolpe = 1,
            Nome = AttackNameId.chifre,
            Colisor = new Colisor("Armature/Bone/Bone.001/Bone.002/Bone.008",
                                          new Vector3(0,-.5f,0),
                                          new Vector3(0,.97f,1.82f)),
            TaxaDeUso=1,
            ModPersonagem = 5
        },
            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                Colisor = new Colisor("Armature/Bone/Bone.001/Bone.001_R/Bone.001_R.001/Bone.001_R.002/Bone.001_R.002_end",
                                         new Vector3(0,0,0.3f),
                                         new Vector3(-0.102f,0.55f,-1.1f)){ ColisorScale = new Vector3(3,3,3)},
                Nome = AttackNameId.garra,
                TaxaDeUso = .5f,
                DistanciaEmissora = 0.5f,
                ModPersonagem = 1
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.sobreSalto,
                NivelDoGolpe = 2,
                Colisor = new Colisor("Armature/Bone",
                                         new Vector3(0,.12f,.62f),
                                         new Vector3(-0.37f,0,-0.325f)),
                TaxaDeUso = 1.25f,
                ModPersonagem = 2
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.anelDoOlhar,
                NivelDoGolpe = 7,
                Colisor = new Colisor("Armature/Bone"),
                DistanciaEmissora = .5f,
                TaxaDeUso = .5f,
                ModPersonagem = 5
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.olharEnfraquecedor,
                NivelDoGolpe = 8,
                Colisor = new Colisor("Armature/Bone/Bone.001/Bone.002/Bone.008"),
                TaxaDeUso = .5f,
                DistanciaEmissora=.125f,
                ModPersonagem = 1
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Grewstick,
                    varHeightCamera = 2,
                    alturaCameraLuta = 15f,
                    distanciaCameraLuta = 10,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Normal },
                        distanciaFundamentadora = 0.01f,
                        meusAtributos = {
                            PV = { Taxa = 0.21f},
                            PE = { Taxa = 0.2f},
                            Ataque = { Taxa = 0.24f},
                            Defesa = { Taxa = 0.2f},
                            Poder = { Taxa = 0.15f}
                        },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Normal)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new MoveFeatures()
                    {
                        rollSpeed = 10,
                        walkSpeed = 5f,
                        runSpeed = 6f,
                        jumpFeat = new JumpFeatures()
                        {
                            jumpHeight = 2f,
                            maxTimeJump = 1,
                            risingSpeed = 5f,
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