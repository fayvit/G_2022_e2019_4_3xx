using UnityEngine;
using System.Collections.Generic;

namespace Criatures2021
{
    public class Serpente
    {
        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
        #region comPergaminhos
        new PetAttackDb()
            {
                NivelDoGolpe = -1,
                ModPersonagem = 0,
                Colisor = new Colisor("esqueleto/centroReverso/r1/r2/r3/rabo"),
                Nome = AttackNameId.olharMal,
                AcimaDoChao = 0.45f,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.75f
            },
        #endregion comPErgaminhos
        new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("esqueleto/centroReverso/r1/r2/r3/rabo"),
                Nome = AttackNameId.laminaDeFolha,
                AcimaDoChao = 0.45f,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.75f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.chicoteDeCalda,
                NivelDoGolpe = 1,
                Colisor = new Colisor("esqueleto/centroReverso/r1/r2/r3/rabo",
                                    new Vector3(0,0f,0),
                                    new Vector3(-0.093f,0.135f,-0.37f)),
                TaxaDeUso = 0.5f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.furacaoDeFolhas,
                NivelDoGolpe = 2,
                Colisor = new Colisor("esqueleto/centroReverso/r1/r2/r3/rabo"),
                DistanciaEmissora = 2.75f,
                AcimaDoChao = 0.15f,
                TaxaDeUso = 1.25f,
                TempoDeInstancia = 0.1f
            },
            new PetAttackDb()
            {
                Nome = AttackNameId.tempestadeDeFolhas,
                NivelDoGolpe = 7,
                Colisor = new Colisor("esqueleto/centroReverso",
                                                            new Vector3(0,0f,0),
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
                    NomeID = PetName.Serpente,
                    //alturaCamera = .5f,
                    //distanciaCamera = 3.8f,
                    //alturaCameraLuta = 3,
                    //distanciaCameraLuta = 5f,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[2] { PetTypeName.Planta, PetTypeName.Normal },
                        distanciaFundamentadora = 0.01f,
                        meusAtributos = {
                    PV = { Taxa = 0.19f,},
                    PE = { Taxa = 0.21f},
                    Ataque = { Taxa = 0.26f},
                    Defesa = { Taxa = 0.17f},
                    Poder = { Taxa = 0.17f}
                },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Normal)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new FayvitMove.MoveFeatures()
                    {
                        walkSpeed = 5.5f,
                        runSpeed=6.25f,
                        rollSpeed=9.5f,
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