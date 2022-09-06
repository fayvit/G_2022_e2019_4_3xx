using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Florest : PetBase
    {

        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
        #region comPergaminhos
        new PetAttackDb()
            {
                NivelDoGolpe = -1,
                ModPersonagem = 0,
                Colisor = new Colisor("Arma__o/corpo"),
                Nome = AttackNameId.olharEnfraquecedor,
                AcimaDoChao = 0.5f,
                TaxaDeUso = .125f,
                DistanciaEmissora = 0.5f
            },
        new PetAttackDb()
            {
                NivelDoGolpe = -1,
                ModPersonagem = 0,
                Colisor = new Colisor("Arma__o/corpo"),
                Nome = AttackNameId.anelDoOlhar,
                AcimaDoChao = 0.5f,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.5f
            },
        #endregion comPergaminhos

        new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("Arma__o/corpo"),
                Nome = AttackNameId.laminaDeFolha,
                AcimaDoChao = 0.5f,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.garra,
                NivelDoGolpe = 1,
                Colisor = new Colisor("Arma__o/corpo",
                                    new Vector3(0,0,0.3f),
                                    new Vector3(-0.087f,0.48f,0.139f)),
                TaxaDeUso = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.furacaoDeFolhas,
                NivelDoGolpe = 2,
                Colisor = new Colisor("Arma__o/corpo"),
                DistanciaEmissora = 1f,
                AcimaDoChao = 0.15f,
                TaxaDeUso = 1.25f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.tempestadeDeFolhas,
                NivelDoGolpe = 7,
                Colisor = new Colisor("Arma__o/corpo",
                                                   new Vector3(0,0,0.3f),
                                                   new Vector3(0,0,0f)),
                TaxaDeUso = 1.25f
            }
        };

        public static PetBase Criature
        {
            get
            {
                return new PetBase()
                {
                    NomeID = PetName.Florest,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Planta },
                        distanciaFundamentadora = 0.01f,
                        meusAtributos = {
                    PV = { Taxa = 0.195f,},
                    PE = { Taxa = 0.205f},
                    Ataque = { Taxa = 0.21f},
                    Defesa = { Taxa = 0.21f},
                    Poder = { Taxa = 0.18f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Planta)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new MoveFeatures()
                    {
                        rollSpeed = 11,
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