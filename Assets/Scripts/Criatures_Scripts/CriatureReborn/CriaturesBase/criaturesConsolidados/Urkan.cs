using UnityEngine;
using System.Collections.Generic;

namespace Criatures2021
{
    public class Urkan
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {

            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("Esqueleto/Bone/Bone.001/Bone.002/Bone.003/Bone.004"),
                Nome = AttackNameId.psicose,
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
                Nome = AttackNameId.bolaPsiquica,
                NivelDoGolpe = 2,
                Colisor = new Colisor("Esqueleto/Bone/Bone.001/Bone.002/Bone.003/Bone.004"),
                DistanciaEmissora = 1f,
                AcimaDoChao = 0.15f,
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.teletransporte,
                NivelDoGolpe = 7,
                Colisor = new Colisor(),
                TaxaDeUso = 1.25f
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Urkan,
                    //alturaCamera = 4,
                    //distanciaCamera = 5.5f,
                    //alturaCameraLuta = 6,
                    //distanciaCameraLuta = 4.5f,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Psiquico },
                        distanciaFundamentadora = 0.03f,
                        meusAtributos = {
                    PV = { Taxa = 0.195f,},
                    PE = { Taxa = 0.205f},
                    Ataque = { Taxa = 0.21f},
                    Defesa = { Taxa = 0.21f},
                    Poder = { Taxa = 0.18f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Psiquico)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new FayvitMove.MoveFeatures()
                    {
                        rollSpeed = 10,
                        walkSpeed = 6,
                        runSpeed = 7,
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