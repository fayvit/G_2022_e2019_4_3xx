using UnityEngine;
using System.Collections.Generic;
using FayvitMove;

namespace Criatures2021
{
    public class Nedak : PetBase
    {

        static List<PetAttackDb> listaDosGolpes = new List<PetAttackDb>()
        {
            new PetAttackDb()
            {
                Nome = AttackNameId.cabecada,
                NivelDoGolpe = 1,
                Colisor = new Colisor("Armação/Bone",
                                    new Vector3(0,.35f,.75f),
                                    new Vector3(0,.497f,-.548f)),
                TaxaDeUso = 0.5f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 1,
                ModPersonagem = 0,
                Colisor = new Colisor("Armação/Bone"),
                Nome = AttackNameId.rajadaDeAgua,
                TaxaDeUso = 1,
                DistanciaEmissora = 0.5f
            },
            new PetAttackDb()
            {
                NivelDoGolpe = 2,
                ModPersonagem = 0,
                Colisor = new Colisor("Armação/Bone"),
                Nome = AttackNameId.turboDeAgua,
                TaxaDeUso = 1.25f,
                DistanciaEmissora = 0.5f
            },new PetAttackDb()
            {
                NivelDoGolpe = 7,
                ModPersonagem = 0,
                Colisor = new Colisor("Armação/Bone",
                                                  new Vector3(0,0,0),
                                                  new Vector3(-0.26f,-0,0)),
                Nome = AttackNameId.hidroBomba,
                TaxaDeUso = 1.25f
            }
        };

        public static PetBase Criature
        {
            get
            {
                PetBase X = new PetBase()
                {
                    NomeID = PetName.Nedak,
                    PetFeat = new PetFeatures()
                    {
                        meusTipos = new PetTypeName[1] { PetTypeName.Agua },
                        distanciaFundamentadora = -0.01f,
                        meusAtributos = {
                            PV = { Taxa = 0.21f,},
                            PE = { Taxa = 0.19f},
                            Ataque = { Taxa = 0.22f},
                            Defesa = { Taxa = 0.19f},
                            Poder = { Taxa = 0.19f}
                        },
                        contraTipos = PetWeaknessAndResistence.ApplyPetWeaknessAndResistence(PetTypeName.Agua)
                    },
                    GerenteDeGolpes = new PetAttackManager()
                    {
                        listaDeGolpes = listaDosGolpes
                    },
                    MovFeat = new MoveFeatures()
                    {
                        rollSpeed = 12,
                        walkSpeed = 5,
                        runSpeed = 6,
                        jumpFeat = new JumpFeatures()
                        {
                            fallSpeed = 20,
                            jumpHeight = 2,
                            maxTimeJump = 1,
                            risingSpeed = 5,
                            inJumpSpeed = 4
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