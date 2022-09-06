using UnityEngine;
using TextBankSpace;
using FayvitMessageAgregator;
using FayvitCam;
using Criatures2021Hud;
using FayvitCommandReader;
using FayvitBasicTools;
using FayvitSave;

namespace Criatures2021
{
    public class AnimateCapturePose
    {
        private GameObject dono;
        private PetBase oCapturado;
        private FaseDoAnimaPose fase = FaseDoAnimaPose.inicia;
        private bool foiParaArmagedom = false;
        private bool ativarAcao = false;
        private float tempoDecorrido = 0;

        private const int TEMPO_DE_MENS_DE_CAPTURA = 10;
        private enum FaseDoAnimaPose
        {
            inicia,
            brilho2,
            insereInfos,
            mensDoArmagedom,
            finaliza,
            umaMensAberta
        }

        public ICommandReader CurrentCommander { get => CommandReader.GetCR(AbstractGlobalController.Instance.Control); }

        public AnimateCapturePose(PetBase oCapturado,GameObject dono)
        {
            this.dono = dono;
            this.oCapturado = oCapturado;

            CharacterManager manager = dono.GetComponent<CharacterManager>();
            DadosDeJogador dados = manager.Dados;

            if (dados.CriaturesAtivos.Count < dados.MaxCarregaveis)
            {
                dados.CriaturesAtivos.Add(oCapturado);
                foiParaArmagedom = false;
                if (dados.CriaturesAtivos.Count == 2)
                {
                    manager.RequisitarHudElements();
                }
                
            }
            else
            {
                dados.CriaturesArmagedados.Add(oCapturado);
                /*
                linhas para encher a vida e retirar status quando o Criature for para o Armagedom
                 */

                PetAtributes A = oCapturado.PetFeat.meusAtributos;
                A.PV.Corrente = A.PV.Maximo;
                A.PE.Corrente = A.PE.Maximo;

                /**************************************************/
                foiParaArmagedom = true;
            }

            dados.Livro.AdicionaCapturado(oCapturado.NomeID);

            //Trofeus.ProcurarTrofeuDeCriature(oCapturado.NomeID);


            MessageAgregator<MsgAnimaCaptura>.Publish(new MsgAnimaCaptura() { dono = dono });
            SaveDatesManager.SalvarAtualizandoDados(new Criatures2021.CriaturesSaveDates());

        }

        public bool Update()
        {
            tempoDecorrido += Time.deltaTime;
            switch (fase)
            {
                case FaseDoAnimaPose.inicia:
                    CameraApplicator.cam.FocusInPoint(3);
                    //AplicadorDeCamera.cam.FocarPonto(10);
                    if (tempoDecorrido > 1)
                    {
                        InsereBrilho();
                        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                        {
                            sfxId = FayvitSounds.SoundEffectID.tuin_1ponto3
                        });
                        tempoDecorrido = 0;
                        fase = FaseDoAnimaPose.brilho2;
                    }
                    break;
                case FaseDoAnimaPose.brilho2:
                    if (tempoDecorrido > 1.1f)
                    {
                        InsereBrilho();
                        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                        {
                            sfxId = FayvitSounds.SoundEffectID.tuimParaNivel
                        });
                        tempoDecorrido = 0;
                        fase = FaseDoAnimaPose.insereInfos;
                    }
                    break;
                case FaseDoAnimaPose.insereInfos:
                    if (tempoDecorrido > 0.4f)
                    {
                        MessageAgregator<MsgRequestUpperLargeMessage>.Publish(new MsgRequestUpperLargeMessage()
                        {
                            message = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.tentaCapturar)[5],
                                "<color=yellow>"+oCapturado.GetNomeEmLinguas+"</color>")
                        });

                        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                        {
                            sfxId = FayvitSounds.SoundEffectID.ItemImportante
                        });

                        MessageAgregator<MsgInsertCaptureInfos>.Publish(new MsgInsertCaptureInfos()
                        {
                            pet = oCapturado
                        });

                        //PainelDeCriature PC = GameController.g.HudM.P_Criature;
                        //GameController.g.HudM.Painel.AtivarNovaMens(
                        //    TextBank.RetornaListaDeTextoDoIdioma(TextKey.tentaCapturar)[5] +
                        //    oCapturado.NomeEmLinguas
                        //    , 25);
                        //PC.gameObject.SetActive(true);
                        //PC.InserirDadosNoPainelPrincipal(oCapturado);

                        Debug.Log("Construir a Hud[parece feito, em observação]");

                        if (foiParaArmagedom)
                        {
                            Debug.Log("Foi para Armagedom, coisas estranhas sobre modificar ação");
                            //ActionManager.ModificarAcao(GameController.g.transform, () => { ativarAcao = true; });
                            fase = FaseDoAnimaPose.mensDoArmagedom;
                        }
                        else
                        {
                            Debug.Log("[em observação]Não foi para Armagedom, coisas estranhas sobre modificar ação");
                            //ActionManager.ModificarAcao(GameController.g.transform, () => { ativarAcao = true; });
                            fase = FaseDoAnimaPose.finaliza;
                            tempoDecorrido = 0;
                        }
                    }
                    break;
                case FaseDoAnimaPose.mensDoArmagedom:
                    ativarAcao = CurrentCommander.GetButtonDown(CommandConverterInt.humanAction, true);
                    if (ativarAcao || tempoDecorrido > TEMPO_DE_MENS_DE_CAPTURA)
                    {
                        ativarAcao = false;

                        Debug.Log("[parece feito, em observação]Mais um Construir a Hud");

                        MessageAgregator<MsgHideShowPetHud>.Publish();
                        MessageAgregator<MsgRequestHideUpperLargeMessage>.Publish();

                        AbstractGlobalController.Instance.OneMessage.StartMessagePanel(() =>
                        {
                            tempoDecorrido = 11;// para finalizar imediatamente
                            fase = FaseDoAnimaPose.finaliza;
                        }, string.Format(TextBank.RetornaFraseDoIdioma(TextKey.foiParaArmagedom),
                         dono.GetComponent<CharacterManager>().Dados.MaxCarregaveis.ToString(),
                         oCapturado.GetNomeEmLinguas,
                         oCapturado.PetFeat.mNivel.Nivel
                         ),infoButtonText: "Press L");

                        fase = FaseDoAnimaPose.umaMensAberta;
                        //GameController.g.HudM.UmaMensagem.ConstroiPainelUmaMensagem(() =>
                        //{
                        //    tempoDecorrido = 11;// para finalizar imediatamente
                        //    fase = FaseDoAnimaPose.finaliza;
                        //    ActionManager.ModificarAcao(GameController.g.transform, () => { ativarAcao = true; });
                        //}, string.Format(BancoDeTextos.RetornaFraseDoIdioma(ChaveDeTexto.foiParaArmagedom),
                        // GameController.g.Manager.Dados.maxCarregaveis,
                        // oCapturado.NomeEmLinguas,
                        // oCapturado.CaracCriature.mNivel.Nivel
                        // ));
                    }
                    break;
                case FaseDoAnimaPose.umaMensAberta:

                    AbstractGlobalController.Instance.OneMessage.
                        ThisUpdate(CurrentCommander.GetButtonDown(CommandConverterInt.humanAction, true));
                    
                break;
                case FaseDoAnimaPose.finaliza:
                    ativarAcao = CurrentCommander.GetButtonDown(CommandConverterInt.humanAction,true);
                    if (ativarAcao || tempoDecorrido > TEMPO_DE_MENS_DE_CAPTURA)
                    {
                        ativarAcao = false;
                        //animator.SetBool("travar", false);
                        MessageAgregator<MsgEndOfCaptureAnimate>.Publish(new MsgEndOfCaptureAnimate()
                        {
                            dono = dono
                        });
                        Debug.Log("[parece feito, em observação]Construir a Hud de novo");
                        //GameController.g.HudM.Painel.EsconderMensagem();
                        //GameController.g.HudM.P_Criature.gameObject.SetActive(false);
                        return false;
                    }
                    break;
            }
            return true;
        }

        void InsereBrilho()
        {
            Vector3 maoDoHeroi = dono.transform
                .Find("metarig/hips/spine/chest/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.04.R")
                    .transform.position;//+0.2f*transform.forward;
            //Object.Instantiate(GameController.g.El.retorna("luz1captura"), maoDoHeroi, Quaternion.identity);
            MonoBehaviour.Destroy(
            Object.Instantiate(Resources.Load<GameObject>("particles/" + GeneralParticles.luz1captura), 
                maoDoHeroi, Quaternion.identity),5);
            
        }
    }

    public struct MsgAnimaCaptura : IMessageBase
    {
        public GameObject dono;
    }
    public struct MsgEndOfCaptureAnimate : IMessageBase {
        public GameObject dono;
    }

    public struct MsgInsertCaptureInfos : IMessageBase {
        public PetBase pet;
    }
}