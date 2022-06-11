using UnityEngine;
using System.Collections.Generic;

namespace Criatures2021
{
    public class Estrep
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
        #region GolpesAprendidosComPergaminhos
        //Golpes aprendidos com pergaminhos        
        new PetAttackDb()
            {
                Nome = AttackNameId.olharMal,
                NivelDoGolpe = -1,
                Colisor = new Colisor("Armação/Bone/Bone_end"),
                TaxaDeUso = .25f,
                AcimaDoChao = -0.5f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = -1,
                ModPersonagem = 0,
                Colisor = new Colisor("Armação/Bone/Bone_end"),
                Nome = AttackNameId.olharEnfraquecedor,
                TaxaDeUso = .25f,
                AcimaDoChao = -0.5f
            },
        #endregion GolpesAprendidosComPergaminhos
        /*
            fim dos golpes aprendidos com eprgaminhos
        */
        new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("Armação/Bone/Bone_end"),
                Nome = AttackNameId.agulhaVenenosa,
                TaxaDeUso = 1,
                AcimaDoChao = -0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.sobreSalto,
                NivelDoGolpe = 1,
                Colisor = new Colisor("Armação/Bone/Bone_end"),
                TaxaDeUso = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.ondaVenenosa,
                NivelDoGolpe = 2,
                Colisor = new Colisor("Armação/Bone/Bone_end"),
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.chuvaVenenosa,
                NivelDoGolpe = 7,
                Colisor = new Colisor("Armação/Bone/Bone_end"),
                TaxaDeUso = 1.25f
            }
        };

        public static PetBase Criature
        {
            get
            {
                PetBase X = new PetBase()
                {
                    NomeID = PetName.Estrep,
                    //alturaCamera = 4,
                    //distanciaCamera = 5.5f,
                    //alturaCameraLuta = 8,
                    //distanciaCameraLuta = 3.5f,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Veneno },
                        distanciaFundamentadora = 0.01f,
                        meusAtributos = {
                    PV = { Taxa = 0.11f,},
                    PE = { Taxa = 0.2f},
                    Ataque = { Taxa = 0.25f},
                    Defesa = { Taxa = 0.23f},
                    Poder = { Taxa = 0.21f}
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
                        runSpeed = 6,
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