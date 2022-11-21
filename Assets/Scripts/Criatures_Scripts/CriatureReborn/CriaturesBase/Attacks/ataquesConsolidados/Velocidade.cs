using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;
using FayvitBasicTools;

namespace Criatures2021
{
    [System.Serializable]
    public class Velocidade : PetAttackBase
    {
        [System.NonSerialized] FayvitMove.ControlledMoveForCharacter controll;
        bool iniciou;
        Vector3[] V = null;
        float[] distances;

        public Velocidade() : base(new PetAttackFeatures()
        {
            nome = AttackNameId.velocidade,
            tipo = PetTypeName.Normal,
            carac = AttackDiferentialId.suporte,
            damageAtribute = DamageBaseAtribute.ataque,
            custoPE = 0,
            potenciaCorrente = 1,
            potenciaMaxima = 1,
            potenciaMinima = 1,
            //tempoDeReuso = 5,
            tempoDeMoveMax = .5f,
            tempoDeMoveMin = 0.4f,
            tempoDeDestroy = .6f,
            TempoNoDano = 0.75f,
            velocidadeDeGolpe = 10,
            custoDeStamina = 35,
            podeNoAr = false,
            somDoImpacto = FayvitSounds.SoundEffectID.XP_Knock04,
            somDoGolpe = FayvitSounds.SoundEffectID.velocidadeCriatures
        }
            )
        {
            
        }

        public override void FinalizaEspecificoDoGolpe()
        {
            iniciou = false;
            V = default;
            distances = default;

            base.FinalizaEspecificoDoGolpe();
        }

        public override void IniciaGolpe(GameObject G)
        {
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = SomDoGolpe
            });
            controll = new FayvitMove.ControlledMoveForCharacter(G.transform);
            controll.StartFields(G.transform);
            

            iniciou = false;
            V = default;
            distances = default;
        }

        public override void UpdateGolpe(GameObject G, GameObject focado = null)
        {
            if (!iniciou)
            {
                iniciou = true;

                if (focado)
                {
                    Vector3 onTarget = (focado.transform.position - G.transform.position).normalized;
                    Vector3 cross = Vector3.Cross(onTarget, Vector3.up) * (Random.Range(0, 2) == 0 ? 1 : -1);
                    //Vector3 sub = DirectionOnThePlane.NormalizedInTheUp(Random.insideUnitSphere);
                    V = new Vector3[3] {
                    MelhoraInstancia3D.ProcuraPosNoMapa(G.transform.position + cross+onTarget),
                    MelhoraInstancia3D.ProcuraPosNoMapa(focado.transform.position + 2*cross),
                    G.transform.position
                };

                    distances = new float[2]
                        {
                        Vector3.Distance(G.transform.position,G.transform.position + cross+onTarget),
                        Vector3.Distance(focado.transform.position + cross+onTarget,G.transform.position + cross+onTarget),
                        };

                    controll.Mov.ChangeWalkSpeed((distances[0] + distances[1]) / (TempoDeDestroy - TempoDeMoveMax));
                    controll.ModificarOndeChegar(V[0]);
                    

                    for (int i = 0; i < distances[0] + distances[1]; i++)
                    {
                        GameObject go;
                        if (i < distances[0])
                        {
                            go = 
                            InstanceSupport.InstancieEDestrua(/*"particles/poeiraAoVento",*/"GeneralElements/CapsuleSpeeder",
                                Vector3.Lerp(V[2], V[0], i / distances[0]) + Vector3.up,
                                /*Quaternion.identity*/Quaternion.LookRotation(Vector3.down, V[0] - V[2]), .5f);

                            
                        }
                        else
                            go = InstanceSupport.InstancieEDestrua(/*"particles/poeiraAoVento",*/"GeneralElements/CapsuleSpeeder",
                                Vector3.Lerp(V[0], V[1], (i - distances[0]) / distances[1]) + Vector3.up,
                               /*Quaternion.identity*/ Quaternion.LookRotation(Vector3.down, V[1] - V[0]), .5f);

                        FayvitSupportSingleton.SupportSingleton.Instance.InvokeInSeconds(() =>
                        {
                            go.AddComponent<ExpandirEClarearSpeederEffect>();
                        }, i * (TempoDeDestroy - TempoDeMoveMax) / (distances[0] + distances[1]));

                        InstanceSupport.InstancieEDestrua("particles/poeiraAoVento",
                                go.transform.position - .74f*Vector3.up,
                                Quaternion.identity, .5f);
                    }
                }
                else
                {
                    V = new Vector3[1] {
                    MelhoraInstancia3D.ProcuraPosNoMapa(
                    G.transform.position + G.transform.forward * 15) };
                    distances = new float[1] { 15 };
                    controll.Mov.ChangeWalkSpeed(distances[0] / (TempoDeDestroy - TempoDeMoveMax));
                    controll.ModificarOndeChegar(V[0]);

                    for (int i = 0; i < 15; i++)
                    {
                        if (i < distances[0])
                        {
                            GameObject go = 
                            InstanceSupport.InstancieEDestrua(/*"particles/poeiraAoVento",*/"GeneralElements/CapsuleSpeeder",
                                Vector3.Lerp(G.transform.position, V[0], i / distances[0]) + Vector3.up,
                                /*Quaternion.identity*/Quaternion.LookRotation(Vector3.down, V[0] - G.transform.position), .5f);

                            InstanceSupport.InstancieEDestrua("particles/poeiraAoVento",
                                Vector3.Lerp(G.transform.position, V[0], i / distances[0]) + 0.25f*Vector3.up,
                                Quaternion.identity, .5f);

                            FayvitSupportSingleton.SupportSingleton.Instance.InvokeInSeconds(() =>
                            {
                                go.AddComponent<ExpandirEClarearSpeederEffect>();
                            }, i* (TempoDeDestroy - TempoDeMoveMax) / distances[0]);
                        }
                    }
                }
            }

            controll.Mov.Controller.Move(Vector3.down);
            if (controll.UpdatePosition(1.1f))
            {
                if (V.Length > 1 && Vector3.Distance(G.transform.position, V[1]) >= 1.2f)
                {
                    controll.ModificarOndeChegar(V[1]);
                }
            }
        }
    }
}