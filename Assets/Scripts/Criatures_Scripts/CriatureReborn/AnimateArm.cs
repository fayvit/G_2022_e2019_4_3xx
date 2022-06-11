using UnityEngine;
using System.Collections;
using FayvitCam;
using FayvitMessageAgregator;
using FayvitBasicTools;

namespace Criatures2021
{
    public class AnimateArm
    {
        private EstadoDoAnimaBraco estado = EstadoDoAnimaBraco.manipulandoCamera;
        private EstadoDoAnimaEnvia estadoEnvia = EstadoDoAnimaEnvia.iniciaAnimacao;

        private GameObject luz;
        private GameObject raio;
        private GameObject C;
        private GameObject alvo;
        private Transform oAnimado;
        private Vector3 posCriature = Vector3.zero;
        private bool treinador = false;
        private float tempoDecorrido = 0;

        private const float TEMPO_BASE_DE_MOVIMENTO_DE_CAMERA = 0.05F;
        private const float TEMPO_PARA_INSTANCIAR_PARTICULA_CHAO = 0.5F;
        private const float TEMPO_PARA_FINALISAR_RAIO = 0.25F;
        private const float TEMPO_DE_REDUCAO_DE_ESCALA = 1f;
        private const float TEMPO_MINIMO_PARA_MOVIMENTO_DE_CAMERA = .25f;
        private const float TEMPO_MAXIMO_PARA_MOVIMENTO_DE_CAMERA = 1;

        private const float TEMPO_PARA_INSTANCIAR_CRIATURE = 1F;

        public AnimateArm(Transform oAnimado, Transform alvo, bool treinador = false)
        {
            this.treinador = treinador;
            this.oAnimado = oAnimado;

            CameraApplicator.cam.StartShowPointCamera(oAnimado, new SinglePointCameraProperties()
            {
                withTime = true,
                dodgeCam = true,
                velOrTimeFocus = Mathf.Min( Mathf.Max(TEMPO_BASE_DE_MOVIMENTO_DE_CAMERA
                    * Vector3.Distance(oAnimado.position, alvo.transform.position),
                    TEMPO_MINIMO_PARA_MOVIMENTO_DE_CAMERA),
                    TEMPO_MAXIMO_PARA_MOVIMENTO_DE_CAMERA)
            });
            
            oAnimado.rotation = Quaternion.LookRotation(
                Vector3.ProjectOnPlane(alvo.position - oAnimado.position, Vector3.up)
                );
            this.alvo = alvo.gameObject;
            PosCriature = alvo.position;
        }

        private enum EstadoDoAnimaEnvia
        {
            iniciaAnimacao,
            animaEnvia,
            Instancia,
            AumentaEscala,
            finalizaEnvia
        }

        private enum EstadoDoAnimaBraco
        {
            manipulandoCamera,
            animaTroca,
            AnimandoTroca,
            InsereRaioDeLuva,
            DiminuiEscalaDoCriature,
            TerminaORaio,
            AnimaBracoFinalizado
        }

        public Vector3 PosCriature
        {
            get => posCriature;
            set => posCriature = value; 
        }

        public bool AnimaEnvia(
            PetBase oInstanciado,
            string nomeDoGameObject = "")
        {
            tempoDecorrido += Time.deltaTime;
            switch (estadoEnvia)
            {
                case EstadoDoAnimaEnvia.iniciaAnimacao:
                    tempoDecorrido = 0;
                    MessageAgregator<MsgRequestSendAnimation>.Publish(
                        new MsgRequestSendAnimation()
                        { 
                            oAnimado=oAnimado.gameObject
                        }
                        );
                    
                    estadoEnvia = EstadoDoAnimaEnvia.animaEnvia;
                break;
                case EstadoDoAnimaEnvia.animaEnvia:
                    
                    AnimatorStateInfo A = oAnimado.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                    if (A.IsName("enviaCriature") && A.normalizedTime >= 1)
                    {
                        luz = ParticleOfSubstitution.InsereParticulaDaLuva(oAnimado, false);
                        raio = ParticleOfSubstitution.InsereParticulaDoRaio(posCriature, oAnimado, false);
                        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                        {
                            sfxId = FayvitSounds.SoundEffectID.Collapse1
                        });
                        estadoEnvia = EstadoDoAnimaEnvia.Instancia;
                        tempoDecorrido = 0;
                    }
                    break;
                case EstadoDoAnimaEnvia.Instancia:
                    if (tempoDecorrido > TEMPO_PARA_INSTANCIAR_CRIATURE)
                    {
                        if (!treinador)
                        {

                            C = PetInitialize.Initialize(oAnimado, oInstanciado);

                            //FayvitSupportSingleton.SupportSingleton.Instance.InvokeOnCountFrame(() =>
                            //{
                                C.transform.position = MelhoraInstancia3D.ProcuraPosNoMapa(posCriature);
                            //});
                            
                            C.transform.parent = AbstractGameController.Instance.ThisGameObject.transform;
                            MessageAgregator<MsgChangeActivePet>.Publish(new MsgChangeActivePet()
                            {
                                dono = oAnimado.gameObject,
                                pet = C
                            });
                        }

                        Debug.Log(nomeDoGameObject + " : " + treinador);

                        
                        if (treinador)
                        {
                            Debug.Log("inimigo");
                            posCriature = MelhoraInstancia3D.ProcuraPosNoMapa(posCriature);
                            C = WildPetInitialize.Initialize(oInstanciado, posCriature).gameObject; 
                            
                            
                            C.name = !string.IsNullOrEmpty(nomeDoGameObject) ? nomeDoGameObject : C.name;
                        }

                        C.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                        ParticleOfSubstitution.ParticulaSaiDaLuva(posCriature);

                        tempoDecorrido = 0;
                        estadoEnvia = EstadoDoAnimaEnvia.AumentaEscala;
                    }
                    break;
                case EstadoDoAnimaEnvia.AumentaEscala:
                    if (C.transform.localScale.sqrMagnitude < 2.5f)
                    {
                        C.transform.localScale = Vector3.Lerp(C.transform.localScale, new Vector3(1, 1, 1), 4 * Time.deltaTime);

                        /*
                         Por algum motivo a posição não era alterada nos personagem de animação humanoid
                         eles acabava caindo abaixo do chão
                         o que fez necessário esse invoke ao fim do quadro
                         esse bug se iniciou na versão unity 2019.4.35f1
                         */
                        FayvitSupportSingleton.SupportSingleton.Instance.InvokeOnCountFrame(() =>
                        {
                            C.transform.position = posCriature;
                        });
                    }
                    else
                    {
                        C.transform.localScale = new Vector3(1, 1, 1);
                        estadoEnvia = EstadoDoAnimaEnvia.finalizaEnvia;
                        tempoDecorrido = 0;
                    }
                    break;
                case EstadoDoAnimaEnvia.finalizaEnvia:

                    if (tempoDecorrido < TEMPO_PARA_FINALISAR_RAIO)
                    {
                        ParticleOfSubstitution.ReduzVelocidadeDoRaio(raio);

                        MessageAgregator<MsgRequestEndArmsAnimations>.Publish(new MsgRequestEndArmsAnimations()
                        {
                            oAnimado = oAnimado.gameObject
                        });
                    }
                    else
                    {
                        if (treinador)
                        {
                            MessageAgregator<MsgRequestEndArmsAnimations>.Publish(new MsgRequestEndArmsAnimations()
                            {
                                oAnimado = oAnimado.gameObject
                            });
                        }

                        Object.Destroy(raio);
                        Object.Destroy(luz);
                        return false;
                    }
                    break;
            }
            return true;
        }

        public bool AnimaTroca(
            bool eItem = false
            )
        {
            

            tempoDecorrido += Time.deltaTime;
            switch (estado)
            {
                case EstadoDoAnimaBraco.manipulandoCamera:

                    if (eItem)
                        alvo.gameObject.tag = "Untagged";

                    if (CameraApplicator.cam.FocusInPoint(height:3))
                        estado = EstadoDoAnimaBraco.animaTroca;
                    break;
                case EstadoDoAnimaBraco.animaTroca:
                    MessageAgregator<MsgRequestCallAnimation>.Publish(new MsgRequestCallAnimation()
                    { 
                    oAnimado=oAnimado.gameObject
                    });
                    
                    estado = EstadoDoAnimaBraco.AnimandoTroca;
                    tempoDecorrido = 0;
                    break;
                case EstadoDoAnimaBraco.AnimandoTroca:
                    
                    AnimatorStateInfo A = oAnimado.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
                    
                    if (A.IsName("chamaCriature") && A.normalizedTime >= 1)
                    {
                        
                        luz = ParticleOfSubstitution.InsereParticulaDaLuva(oAnimado, true);
                        raio = ParticleOfSubstitution.InsereParticulaDoRaio(posCriature, oAnimado);
                        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                        {
                            sfxId = FayvitSounds.SoundEffectID.Collapse1
                        });

                        estado = EstadoDoAnimaBraco.InsereRaioDeLuva;
                        tempoDecorrido = 0;
                    }
                    break;
                case EstadoDoAnimaBraco.InsereRaioDeLuva:
                    if (tempoDecorrido > TEMPO_PARA_INSTANCIAR_PARTICULA_CHAO && alvo.transform.localScale.sqrMagnitude > 0.01f)
                    {
                        posCriature = alvo.transform.position;
                        ParticleOfSubstitution.ParticulaSaiDaLuva(alvo.transform.position);
                        if (!eItem)
                            estado = EstadoDoAnimaBraco.DiminuiEscalaDoCriature;
                        else
                        {

                            CameraApplicator.cam.StartShowPointCamera(alvo.transform, new SinglePointCameraProperties()
                            {
                                velOrTimeFocus = TEMPO_BASE_DE_MOVIMENTO_DE_CAMERA *
                                    Mathf.Max(Vector3.Distance(oAnimado.position, alvo.transform.position),
                                    TEMPO_MINIMO_PARA_MOVIMENTO_DE_CAMERA),
                                withTime=true
                            });

                            estado = EstadoDoAnimaBraco.TerminaORaio;
                        }
                        tempoDecorrido = 0;
                    }
                    break;
                case EstadoDoAnimaBraco.DiminuiEscalaDoCriature:
                    if (tempoDecorrido < TEMPO_DE_REDUCAO_DE_ESCALA)
                    {
                        alvo.transform.localScale = Vector3.Lerp(alvo.transform.localScale, Vector3.zero, 2 * Time.deltaTime);
                    }
                    else
                    {
                        estado = EstadoDoAnimaBraco.TerminaORaio;
                        tempoDecorrido = 0;
                    }
                    break;
                case EstadoDoAnimaBraco.TerminaORaio:
                    if (tempoDecorrido < TEMPO_PARA_FINALISAR_RAIO)
                    {
                        if (!eItem)
                            Object.Destroy(alvo);

                        ParticleOfSubstitution.ReduzVelocidadeDoRaio(raio);
                    }
                    else if (!eItem)
                    {
                        MudarParaAnimaBracoFinalizado();
                        return false;
                    }

                    if (eItem)
                    {
                        
                        if (CameraApplicator.cam.FocusInPoint(height: 3))
                        {
                            MudarParaAnimaBracoFinalizado();
                            return false;
                        }
                        if (eItem)
                            alvo.gameObject.tag = "Criature";

                    }
                    break;
                case EstadoDoAnimaBraco.AnimaBracoFinalizado:
                    return false;
                    //break;
            }
            return true;
        }

        void MudarParaAnimaBracoFinalizado()
        {
            Object.Destroy(raio);
            Object.Destroy(luz);
            estado = EstadoDoAnimaBraco.AnimaBracoFinalizado;
        }
    }

    public struct MsgRequestCallAnimation : IMessageBase {
        public GameObject oAnimado;
    }
    public struct MsgRequestSendAnimation : IMessageBase {
        public GameObject oAnimado;
    }
    public struct MsgRequestEndArmsAnimations : IMessageBase {
        public GameObject oAnimado;
    }
    public struct MsgInsertPetWithAnimateArm : IMessageBase {
        public GameObject oAnimado;
        public PetBase petBaseInstanciado;
    }
    public struct MsgChangeActivePet : IMessageBase
    {
        public GameObject pet;
        public GameObject dono;
    }
}