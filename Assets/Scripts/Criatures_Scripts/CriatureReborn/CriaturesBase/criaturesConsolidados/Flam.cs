using UnityEngine;
using System.Collections.Generic;

namespace Criatures2021
{
    public class Flam
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {

            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("Esqueleto/Bone/Bone.001/Bone.002/Bone.003/Bone.004/Bone.005"),
                Nome = AttackNameId.bolaDeFogo,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.garra,
                NivelDoGolpe = 1,
                Colisor = new Colisor("Esqueleto/Bone/Bone.001/Bone.002/Bone.002_R/Bone.002_R.001/Bone.002_R.002",
                                         new Vector3(0.18f,0,0),
                                         new Vector3(-0.525f,-0.057f,-0.015f)),
                TaxaDeUso = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.rajadaDeFogo,
                NivelDoGolpe = 2,
                Colisor = new Colisor("Esqueleto/Bone/Bone.001/Bone.002/Bone.003/Bone.004/Bone.005"),
                DistanciaEmissora = 1f,
                AcimaDoChao = 0.15f,
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.tosteAtaque,
                NivelDoGolpe = 7,
                Colisor = new Colisor("Esqueleto/Bone",
                                                   new Vector3(0f,0,0),
                                                   new Vector3(0f,0f,-0f)),
                TaxaDeUso = 1.25f
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Flam,
                    //alturaCamera = 4,
                    //distanciaCamera = 5.5f,
                    //alturaCameraLuta = 6,
                    //distanciaCameraLuta = 4.5f,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Fogo },
                        distanciaFundamentadora = 0.01f,
                        meusAtributos = {
                    PV = { Taxa = 0.18f,},
                    PE = { Taxa = 0.22f},
                    Ataque = { Taxa = 0.25f},
                    Defesa = { Taxa = 0.18f},
                    Poder = { Taxa = 0.17f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Fogo)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new FayvitMove.MoveFeatures()
                    {
                        walkSpeed = 5.7f,
                        rollSpeed  =10,
                        runSpeed = 6.3f,
                        jumpFeat = new FayvitMove.JumpFeatures()
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