using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;
using FayvitCam;
using Criatures2021Hud;
using TextBankSpace;
using FayvitBasicTools;

namespace Criatures2021
{
    public class CaptureManager
    {
        private GameObject CriatureAlvoDoItem;
        private GameObject dono;
        private FaseDoAnimaCaptura fase = FaseDoAnimaCaptura.inicial;
        private AnimateCapturePose animaPose;
        private float tempoDecorrido = 0;
        private bool iraCapturar = false;
        private int disparado = 0;

        private const int LOOPS = 2;

        private enum FaseDoAnimaCaptura
        {
            inicial,
            cameraDoHeroi,
            animaPersonagemCapturando,
            finalizaCapturando,
            finalizaSemCapturar
        }

        public CaptureManager(GameObject dono,bool iraCapturar)
        {
            this.dono = dono;
            this.iraCapturar = iraCapturar;
            CriatureAlvoDoItem = FindByOwner.GetEnemy(dono).gameObject;
            //animator = CriatureAlvoDoItem.GetComponent<Animator>();

            CameraApplicator.cam.StartShowPointCamera(CriatureAlvoDoItem.transform, new SinglePointCameraProperties()
            {
                velOrTimeFocus = .85f,
                withTime=true
            });

            //AplicadorDeCamera.cam.InicializaCameraExibicionista(CriatureAlvoDoItem.transform);asd

        }

        // Update is called once per frame
        public bool Update()
        {
            tempoDecorrido += Time.deltaTime;
            switch (fase)
            {
                case FaseDoAnimaCaptura.inicial:

                    PetManager enemyManager = FindByOwner.GetManagerEnemy(dono);
                    //AplicadorDeCamera.cam.FocarPonto(10, enemyManager.MeuCriatureBase.distanciaCameraLuta);
                    CameraApplicator.cam.FocusInPoint(enemyManager.MeuCriatureBase.distanciaCameraLuta);
                    int arredondado = Mathf.RoundToInt(tempoDecorrido);
                    Vector3 variacao = arredondado % 2 == 1 ? Vector3.zero : new Vector3(1.5f, 1.5f, 1.5f);

                    if (arredondado != disparado && arredondado < LOOPS)
                    {
                        ParticleOfSubstitution.ParticulaSaiDaLuva(CriatureAlvoDoItem.transform.position);

                        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                        {
                            sfxId = FayvitSounds.SoundEffectID.Darkness8
                        });

                        MessageAgregator<MsgRequestDamageAnimateWithFade>.Publish(
                            new MsgRequestDamageAnimateWithFade()
                            {
                                animatePet = enemyManager.gameObject
                            }
                            );
                        //animator.CrossFade("dano1", 0);
                        //animator.SetBool("dano1", true);
                        //animator.SetBool("dano2", true);

                        disparado = arredondado;
                    }

                    if (arredondado >= LOOPS)
                    {
                        if (iraCapturar)
                        {
                            PreparaFinalComCaptura();
                            fase = FaseDoAnimaCaptura.cameraDoHeroi;

                        }
                        else
                        {
                            PreparaFinalSemCaptura();
                            fase = FaseDoAnimaCaptura.finalizaSemCapturar;
                        }

                        tempoDecorrido = 0;
                    }

                    CriatureAlvoDoItem.transform.localScale = Vector3.Lerp(
                        CriatureAlvoDoItem.transform.localScale, variacao, Time.deltaTime);

                    break;
                case FaseDoAnimaCaptura.finalizaSemCapturar:
                    if (tempoDecorrido > 1)
                    {
                        return false;
                    }
                    break;
                case FaseDoAnimaCaptura.cameraDoHeroi:
                    if (tempoDecorrido > 1.5f)
                    {
                        CameraApplicator.cam.StartShowPointCamera(dono.transform, new SinglePointCameraProperties()
                        {
                            velOrTimeFocus=.85f,
                            characterHeight = 1.75f,
                            withTime=true
                        });
                        //AplicadorDeCamera.cam.InicializaCameraExibicionista(GameController.g.Manager.transform);
                        fase = FaseDoAnimaCaptura.animaPersonagemCapturando;
                        tempoDecorrido = 0;
                    }
                    break;
                case FaseDoAnimaCaptura.animaPersonagemCapturando:
                    if (tempoDecorrido > 1)
                    {
                        PetManager P = FindByOwner.GetManagerEnemy(dono);
                        animaPose = new AnimateCapturePose(P.MeuCriatureBase,dono);
                        MonoBehaviour.Destroy(P.gameObject);
                        fase = FaseDoAnimaCaptura.finalizaCapturando;

                    }
                    break;
                case FaseDoAnimaCaptura.finalizaCapturando:
                    if (!animaPose.Update())
                    {
                        IamTarget.StaticStart(dono.GetComponent<CharacterManager>().ActivePet,
                            () => { MessageAgregator<MsgReturnRememberedMusic>.Publish(); });

                        return false;
                    }
                break;

            }

            return true;
        }

        void PreparaFinalComCaptura()
        {
            Debug.Log("Hud captura [parece feito, em observação]");
            MessageAgregator<MsgPrepareFinalWithCapture>.Publish(new MsgPrepareFinalWithCapture()
            {
                capturado = CriatureAlvoDoItem,
                capturador = dono
            });
            //GameController.g.HudM.ModoCriature(false);

            Vector3 maoDoHeroi = dono.transform
                .Find("metarig/hips/spine/chest/shoulder.L/upper_arm.L/forearm.L/hand.L/palm.02.L")
                    .transform.position;
            CriatureAlvoDoItem.transform.localScale = Vector3.zero;

            MonoBehaviour.Destroy(
            ParticleOfSubstitution.InsereParticulaDoRaio(CriatureAlvoDoItem.transform.position, maoDoHeroi), 2.5f);

            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.Collapse1
            });

            fase = FaseDoAnimaCaptura.cameraDoHeroi;
        }

        void PreparaFinalSemCaptura()
        {
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.XP_Ice03
            });
            ParticleOfSubstitution.ParticulaSaiDaLuva(CriatureAlvoDoItem.transform.position, GeneralParticles.captureEscape);
            CriatureAlvoDoItem.transform.localScale = new Vector3(1, 1, 1);
            //animator.SetBool("dano1", false);
            //animator.SetBool("dano2", false);
            MessageAgregator<MsgEndDamageState>.Publish(new MsgEndDamageState()
            {
                gameObject = CriatureAlvoDoItem
            });

            //Debug.LogError("ota HUd");

            MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
            {
                message = string.Format(TextBank.RetornaFraseDoIdioma(TextKey.tentaCapturar),
                    PetBase.NomeEmLinguas(FindByOwner.GetManagerEnemy(dono).MeuCriatureBase.NomeID))
            });

            //GameController.g.HudM.Painel.AtivarNovaMens(
            //   GameController.g.InimigoAtivo.MeuCriatureBase.NomeEmLinguas + BancoDeTextos.RetornaFraseDoIdioma(ChaveDeTexto.tentaCapturar),
            //    24, 5);

        }
    }

    public struct MsgRequestDamageAnimateWithFade : IMessageBase
    {
        public GameObject animatePet;
    }

    public struct MsgPrepareFinalWithCapture : IMessageBase
    {
        public GameObject capturado;
        public GameObject capturador;
    }

}