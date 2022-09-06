using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Cracler
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
        #region comPergaminhos]

         new PetAttackDb()
            {
                NivelDoGolpe = -1,
                Nome = AttackNameId.olharEnfraquecedor,
                Colisor = new Colisor("Armação/Bone/Bone_R/Bone_R.001"),
                DistanciaEmissora = 1.5f,
                AcimaDoChao = 0.25f,
                TaxaDeUso=.125f
            },
        new PetAttackDb()
            {
                NivelDoGolpe = -1,
                Nome = AttackNameId.sabreDeBastao,
                Colisor = new Colisor("Armação/Bone/Bone_R/Bone_R.001"),
                DistanciaEmissora = 1.5f,
                AcimaDoChao = 0.25f
            },
        #endregion comPErgaminhos
            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("Armação/Bone/Bone_R/Bone_R.001"),
                Nome = AttackNameId.agulhaVenenosa,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.5f,
                TempoDeInstancia = 0.25f,
                AcimaDoChao = -0.1f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.bastao,
                NivelDoGolpe = 1,
                Colisor = new Colisor("Armação/Bone/Bone.001",
                                                    new Vector3(0,0,0),
                                                    new Vector3(0,0,0)),
                TaxaDeUso = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.ondaVenenosa,
                NivelDoGolpe = 2,
                Colisor = new Colisor("Armação/Bone/Bone_R/Bone_R.001"),
                DistanciaEmissora = 1f,
                AcimaDoChao = 0.15f,
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.tesoura,
                NivelDoGolpe = 3,
                Colisor = new Colisor(""),
                TaxaDeUso = .5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.chuvaVenenosa,
                NivelDoGolpe = 7,
                Colisor = new Colisor("",new Vector3(0,0.62f,0.21f),Vector3.zero),
                TaxaDeUso = 1.25f
            }
        };

        public static PetBase Criature
        {
            get
            {
                PetBase X = new PetBase()
                {
                    NomeID = PetName.Cracler,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Veneno },
                        distanciaFundamentadora = 0.1f,
                        meusAtributos = {
                    PV = { Taxa = 0.22f,},
                    PE = { Taxa = 0.17f},
                    Ataque = { Taxa = 0.22f},
                    Defesa = { Taxa = 0.19f},
                    Poder = { Taxa = 0.2f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Veneno)
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
                X.PetFeat.contraTipos[9].Mod = 0.1f;//Veneno
                X.PetFeat.contraTipos[7].Mod = 2f;//Eletrico

                return X;
            }
        }
    }
}