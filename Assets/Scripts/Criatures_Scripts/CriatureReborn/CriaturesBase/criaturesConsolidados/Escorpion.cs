using UnityEngine;
using System.Collections.Generic;

namespace Criatures2021
{
    public class Escorpion
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
        #region GolpesAprendidosComPergaminhos
        //Golpes aprendidos com pergaminhos        
        new PetAttackDb()
            {
                Nome = AttackNameId.multiplicar,
                NivelDoGolpe = -1,
                Colisor = new Colisor(),
                TaxaDeUso = 1.5f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = -1,
                ModPersonagem = 0,
                Colisor = new Colisor("Armação/Bone/Bone.003/Bone.004/Bone.005/Bone.006/rabo"),
                Nome = AttackNameId.olharEnfraquecedor,
                TaxaDeUso = .125f,
                AcimaDoChao = -0.75f
            },
        #endregion GolpesAprendidosComPergaminhos
        /*
            fim dos golpes aprendidos com eprgaminhos
        */
        new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("Armação/Bone/Bone.003/Bone.004/Bone.005/Bone.006/rabo"),
                Nome = AttackNameId.agulhaVenenosa,
                TaxaDeUso = 1,
                AcimaDoChao = -0.75f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.chicoteDeCalda,
                NivelDoGolpe = 1,
                Colisor = new Colisor("Armação/Bone/Bone.003/Bone.004/Bone.005/Bone.006/rabo",1.25f),
                TaxaDeUso = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.ondaVenenosa,
                NivelDoGolpe = 2,
                Colisor = new Colisor("Armação/Bone/Bone.003/Bone.004/Bone.005/Bone.006/rabo"),
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.chuvaVenenosa,
                NivelDoGolpe = 7,
                Colisor = new Colisor("",new Vector3(0,0.316f,0.437f),Vector3.zero),
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 8,
                ModPersonagem = 0,
                Colisor = new Colisor("Armação/Bone/Bone.003/Bone.004/Bone.005/Bone.006/rabo"),
                Nome = AttackNameId.olharMal,
                TaxaDeUso = .125f,
                AcimaDoChao = -0.75f
            }
        };

        public static PetBase Criature
        {
            get
            {
                PetBase X = new PetBase()
                {
                    NomeID = PetName.Escorpion,
                    //alturaCamera = 4,
                    //distanciaCamera = 5.5f,
                    //alturaCameraLuta = 8,
                    //distanciaCameraLuta = 3.5f,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Veneno },
                        distanciaFundamentadora = 0.01f,
                        meusAtributos = {
                    PV = { Taxa = 0.21f,},
                    PE = { Taxa = 0.17f},
                    Ataque = { Taxa = 0.2f},
                    Defesa = { Taxa = 0.23f},
                    Poder = { Taxa = 0.19f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Veneno)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new FayvitMove.MoveFeatures()
                    {
                        walkSpeed = 5,
                        rollSpeed = 10,
                        runSpeed=6,
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

                return X;
            }
        }
    }
}