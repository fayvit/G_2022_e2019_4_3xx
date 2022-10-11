﻿using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class HolyCharm : PetBase
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
        #region comPergainhos
        new PetAttackDb()
            {
                NivelDoGolpe = -1,
                ModPersonagem = 0,
                Colisor = new Colisor("Armature/coluna1/coluna2/coluna3/pescoco/cabeca"),
                Nome = AttackNameId.olharEnfraquecedor,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.125f
            },
        new PetAttackDb()
            {
                NivelDoGolpe = -1,
                ModPersonagem = 0,
                Colisor = new Colisor("Armature/coluna1/coluna2/coluna3/pescoco/cabeca"),
                Nome = AttackNameId.olharMal,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.125f
            },
        #endregion comPergaminhos

        new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("Armature/coluna1/coluna2/coluna3/pescoco/cabeca"),
                Nome = AttackNameId.bolaDeFogo,
                TaxaDeUso = 1,
                DistanciaEmissora = 1f,
                AcimaDoChao=-.75f                
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.garra,
                NivelDoGolpe = 1,
                Colisor = new Colisor("Armature",
                                   new Vector3(0.18f,0,0),
                                   new Vector3(-0.161F,.53F,0.767f)),
                TaxaDeUso = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.rajadaDeFogo,
                NivelDoGolpe = 2,
                Colisor = new Colisor("Armature/coluna1/coluna2/coluna3/pescoco/cabeca"),
                DistanciaEmissora = 1f,
                AcimaDoChao = 0.15f,
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.tosteAtaque,
                NivelDoGolpe = 7,
                Colisor = new Colisor("Armature/coluna1",
                                                   new Vector3(0f,0,0),
                                                   new Vector3(0,0,0)),
                TaxaDeUso = 1.25f
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.HolyCharm,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Fogo },
                        distanciaFundamentadora = 0.01f,
                        meusAtributos = {
                    PV = { Taxa = 0.21f,},
                    PE = { Taxa = 0.2f},
                    Ataque = { Taxa = 0.21f},
                    Defesa = { Taxa = 0.18f},
                    Poder = { Taxa = 0.2f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Fogo)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new MoveFeatures()
                    {
                        rollSpeed = 10,
                        walkSpeed = 5,
                        runSpeed = 6,
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