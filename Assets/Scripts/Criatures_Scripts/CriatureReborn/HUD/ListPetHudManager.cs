using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Criatures2021;
using FayvitCommandReader;
using FayvitBasicTools;
using TextBankSpace;
using FayvitMessageAgregator;

namespace Criatures2021Hud
{
    public class ListPetHudManager : MonoBehaviour
    {
        [SerializeField] private ListPetHud lPet;

        private PetBase[] lista;
        private CharacterManager mDoDono;
        
        private bool armagedom = false;
        private int oneChoose = -1;
        private LocalState thisState = LocalState.emEspera;
        private ArmagedomState armgdState=ArmagedomState.firstChoose;
        

        private enum ArmagedomState
        { 
            firstChoose,
            secondChoose
        }

        private enum LocalState
        { 
            emEspera,
            ativado,
            confirmationOpened,
            singleMessageOpened
        }

        public ICommandReader CurrentCommander => CommandReader.GetCR(AbstractGlobalController.Instance.Control);

        // Use this for initialization
        void Start()
        {
            MessageAgregator<MsgOpenPetList>.AddListener(OnRequestStartThisHud);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgOpenPetList>.RemoveListener(OnRequestStartThisHud);
        }

        private void OnRequestStartThisHud(MsgOpenPetList obj)
        {
            StartFields(obj.dono, obj.armagedom);
        }


        public void StartFields(CharacterManager dono,bool armagedom=false)
        {
            this.mDoDono = dono;
            this.armagedom = armagedom;
            thisState = LocalState.ativado;

            if (!armagedom)
            {
                this.lista = mDoDono.Dados.CriaturesAtivos.ToArray();
                lPet.StartHud(lista, OnSelectedPet);
            }
            if (armagedom)
            {

                StartFirstChoose();
            }

            
        }

        private void StartFirstChoose()
        {
            MessageAgregator<MsgRequestUpperLargeMessage>.Publish(new MsgRequestUpperLargeMessage()
            {
                message = TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDeArmagedom)[2]
            });

            lista = mDoDono.Dados.CriaturesArmagedados.ToArray();
            lPet.StartHud(lista, OnArmagedomFirstChoose);
            armgdState = ArmagedomState.firstChoose;
        }

        private void OnArmagedomFirstChoose(int qual)
        {
            lPet.FinishHud();
            string nomeCriature = lista[qual].GetNomeEmLinguas;
            string txtNivelNum = lista[qual].PetFeat.mNivel.Nivel.ToString();
            string texto;
            System.Action acao=null;

            MessageAgregator<MsgRequestHideUpperLargeMessage>.Publish();

            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.Decision1
            });

            if (mDoDono.Dados.CriaturesAtivos.Count < mDoDono.Dados.MaxCarregaveis)
            {
                texto = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDeArmagedom)[3], nomeCriature, txtNivelNum);
                acao += () => { 
                    VoltarParaMenuDeArmagedom(lPet.SelectedOption, -1);
                    MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                    {
                        sfxId = FayvitSounds.SoundEffectID.Book1
                    });
                };
            }
            else {
                texto = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDeArmagedom)[4], nomeCriature, txtNivelNum);
                acao += () => {
                    MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                    {
                        sfxId = FayvitSounds.SoundEffectID.painelAbrindo
                    });

                    IniciarSegundaEscolha();
                };
            }
            

                AbstractGlobalController.Instance.OneMessage.StartMessagePanel(acao,
                    texto,hideOpenSound:true
                    );
                thisState = LocalState.singleMessageOpened;
                

                
            //}
        }

        private void IniciarSegundaEscolha()
        {
            armgdState = ArmagedomState.secondChoose;
            oneChoose = lPet.SelectedOption;

            this.lista = mDoDono.Dados.CriaturesAtivos.ToArray();
            lPet.StartHud(lista, OnArmagedomSecondChoose);

            MessageAgregator<MsgRequestUpperLargeMessage>.Publish(new MsgRequestUpperLargeMessage()
            {
                message = TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDeArmagedom)[5]
            });

            thisState = LocalState.ativado;
        }

        private void OnArmagedomSecondChoose(int qual)
        {
            lPet.ChangeInteractiveButtons(false);
            string nomeCriature = lista[qual].GetNomeEmLinguas;
            string txtNivelNum = lista[qual].PetFeat.mNivel.Nivel.ToString();
            string texto = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.frasesDeArmagedom)[7],nomeCriature,txtNivelNum);

            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.Decision1
            });

            AbstractGlobalController.Instance.Confirmation.StartConfirmationPanel(() => {
                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                {
                    sfxId = FayvitSounds.SoundEffectID.Book1
                });
                VoltarParaMenuDeArmagedom(oneChoose, lPet.SelectedOption);
                MessageAgregator<MsgRequestHideUpperLargeMessage>.Publish();
            }, ()=> {
                DeVoltaAoMenu();
                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                {
                    sfxId = FayvitSounds.SoundEffectID.Book1
                });
            },
                    texto
                    );

            thisState = LocalState.confirmationOpened;
        }

        private void VoltarParaMenuDeArmagedom(int entra,int sai)
        {
            MessageAgregator<MsgReturnToArmgdMenu>.Publish(new MsgReturnToArmgdMenu()
            {
                entra = entra,
                sai = sai
            });

            thisState = LocalState.emEspera;
            lPet.FinishHud();
        }

        void OnSelectedPet(int qual)
        {
            lPet.PodeMudar = false;
            lPet.ChangeInteractiveButtons(false);
            string nomeCriature = lista[qual].GetNomeEmLinguas;
            string txtNivelNum = lista[qual].PetFeat.mNivel.Nivel.ToString();
            if (lista[qual].PetFeat.meusAtributos.PV.Corrente > 0)
            {
                string texto =
                    string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.criatureParaMostrador)[0], nomeCriature);
                    


                AbstractGlobalController.Instance.Confirmation.StartConfirmationPanel(() => {
                    QueroColocarEsse(qual);
                    MessageAgregator<MsgRequestHideUpperLargeMessage>.Publish();
                }, DeVoltaAoMenu,
                    texto
                    );
                thisState = LocalState.confirmationOpened;
                //if (cliqueDoPersonagem != null)
                //    cliqueDoPersonagem(transform.GetSiblingIndex() - 1);
            }
            else
            {
                AbstractGlobalController.Instance.OneMessage.StartMessagePanel(DeVoltaAoMenu,
                    string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.criatureParaMostrador)[1], nomeCriature)
                    );

                thisState = LocalState.singleMessageOpened;
            }
        }



        void RetornarUmItem() {

            lPet.FinishHud();
            MessageAgregator<MsgRequestHideUpperLargeMessage>.Publish();

            if (armgdState == ArmagedomState.firstChoose)
            {
                MessageAgregator<MsgReturnToArmgdMenu>.Publish(new MsgReturnToArmgdMenu()
                {
                    sai = -1,
                    entra = -1
                });
                thisState = LocalState.emEspera;
            } else if (armgdState==ArmagedomState.secondChoose)
            {
                
                StartFirstChoose();
                //thisState = LocalState.ativado;
            }

            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.Book1
            });
        }

        // Update is called once per frame
        void Update()
        {
            switch (thisState)
            {
                case LocalState.ativado:
                    bool confirmInput = CurrentCommander.GetButtonDown(CommandConverterInt.confirmButton, true);
                    int vert = -CurrentCommander.GetIntTriggerDown(CommandConverterString.moveV) -
                        CurrentCommander.GetIntTriggerDown(CommandConverterString.alternativeV_Change);

                    lPet.Update(vert, confirmInput);

                    if (armagedom && CurrentCommander.GetButtonDown(CommandConverterInt.returnButton))
                        RetornarUmItem();
                break;
                case LocalState.confirmationOpened:
                    bool confirmInput_b = CurrentCommander.GetButtonDown(CommandConverterInt.confirmButton, true);
                    bool returninput_b = CurrentCommander.GetButtonDown(CommandConverterInt.returnButton, true);
                    int horiz = -CurrentCommander.GetIntTriggerDown(CommandConverterString.moveH) +
                        CurrentCommander.GetIntTriggerDown(CommandConverterString.alternativeH_Change);

                    AbstractGlobalController.Instance.Confirmation.ThisUpdate(horiz!=0, confirmInput_b, returninput_b);
                break;
                case LocalState.singleMessageOpened:
                    bool confirmInput_c = CurrentCommander.GetButtonDown(CommandConverterInt.confirmButton, true);
                    bool returninput_c = CurrentCommander.GetButtonDown(CommandConverterInt.returnButton, true);

                    AbstractGlobalController.Instance.OneMessage.ThisUpdate(confirmInput_c||returninput_c);
                break;
            }
        }

        //void QueroColocarEsse()
        //{
        //    if (OnSelectPet != null)
        //        OnSelectPet(transform.GetSiblingIndex() - 1);
        //    else
        //        Debug.LogError("A função hedeira não foi setada corretamente");
        //}

        public void QueroColocarEsse(int qual)
        {
            Debug.Log("Active Pet target: "+mDoDono.ActivePet.Mov.LockTarget);

            MessageAgregator<MsgRequestReplacePet>.Publish(new MsgRequestReplacePet()
            {
                dono = mDoDono.gameObject,
                replaceIndex = true,
                newIndex = qual - 1,
                lockTarget = mDoDono.ActivePet.Mov.LockTarget,
                fluxo = FluxoDeRetorno.criature
            });

            thisState = LocalState.emEspera;
            lPet.FinishHud();

            //manager.Dados.CriatureSai = qual - 1;
            //fase = FaseDaDerrota.entrandoUmNovo;
            //replace = new ReplaceManager(manager, manager.CriatureAtivo.transform, FluxoDeRetorno.criature);

            Debug.Log("[parece feito, em observação]Elementos de Hud");
            //GameController.g.HudM.EntraCriatures.FinalizarHud();
            //GameController.g.HudM.Painel.EsconderMensagem();
        }

        void DeVoltaAoMenu()
        {
            //GameController.g.HudM.EntraCriatures.PodeMudar = true;

            //ActionManager.ModificarAcao(GameController.g.transform, GameController.g.HudM.EntraCriatures.AcaoDeOpcaoEscolhida);

            thisState = LocalState.ativado;
            lPet.PodeMudar = true;
            lPet.ChangeInteractiveButtons(true);
        }

    }

    public struct MsgReturnToArmgdMenu : IMessageBase
    {
        public int entra;
        public int sai;
    }
}
