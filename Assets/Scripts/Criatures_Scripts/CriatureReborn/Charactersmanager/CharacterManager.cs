﻿using FayvitBasicTools;
using System.Collections.Generic;
using UnityEngine;
using FayvitCommandReader;
using FayvitCam;
using FayvitMove;
using FayvitMessageAgregator;
using Criatures2021Hud;
using TextBankSpace;

namespace Criatures2021
{
    public class CharacterManager : MonoBehaviour, ICharacterManager
    {
        [SerializeField] private BasicMove mov;
        [SerializeField] private DadosDeJogador dados;
        [SerializeField] private DamageState damageState;

        public bool ContraTreinador { get; set; }
        public bool InTeste { get; set; }
        public CharacterState ThisState { get; private set; } = CharacterState.notStarted;
        public PetManager ActivePet { get; private set; }
        public CustomizationContainerDates Ccd { get; set; }

        [SerializeField] private CharacterState returnState;

        private ICommandReader CurrentCommander
        {
            get => CommandReader.GetCR(AbstractGlobalController.Instance.Control/*Controlador.teclado*/);
        }

        public DadosDeJogador Dados { get => dados; set => dados = value; }

        // Start is called before the first frame update
        void Start()
        {
            damageState = new DamageState(transform);
            mov = new BasicMove(new MoveFeatures() { jumpFeat = new JumpFeatures() });
            mov.StartFields(transform);

            if (ThisState == CharacterState.notStarted)
            {
                if (dados == null)
                    dados = new DadosDeJogador();

                ThisState = CharacterState.onFree;

                if (InTeste)
                {
                    dados.InicializadorDosDados();

                    if (ActivePet == null)
                        SeletaDeCriatures();

                    Debug.Log("Nâo é possivel");
                    RequisitarHudElements();
                }
                else
                {
                    dados = new DadosDeJogador();
                    dados.DadosLimpos();
                    MessageAgregator<MsgClearStatusUpdater>.Publish();
                    Debug.Log("Não tem como isso ser nulo");
                }
            }

            MessageAgregator<MsgChangeToHero>.AddListener(OnChangeToHero);
            MessageAgregator<MsgRequestChangeSelectedPetWithPet>.AddListener(OnRequestChangePet);
            MessageAgregator<MsgStartExternalInteraction>.AddListener(OnStartTalk);
            MessageAgregator<MsgFinishExternalInteraction>.AddListener(OnFinishTalk);
            MessageAgregator<MsgChangeActivePet>.AddListener(OnChangeActivePet);
            MessageAgregator<MsgRequestReplacePet>.AddListener(OnRequestReplacePet);
            MessageAgregator<MsgRequestChangeToPetByReplace>.AddListener(OnRequestChangeToPetByReplace);
            MessageAgregator<MsgRequestChangeSelectedItemWithPet>.AddListener(OnRequestChangeSelectedItem);
            MessageAgregator<MsgRequestUseItem>.AddListener(OnRequestUseItem);
            MessageAgregator<MsgStartUseItem>.AddListener(OnStartUseItem);
            MessageAgregator<MsgPlayerPetDefeated>.AddListener(OnPlayerPetDefeated);
            MessageAgregator<MsgGetChestItem>.AddListener(OnGetChestItem);
            MessageAgregator<MsgEndNewAttackHud>.AddListener(OnFinishNewAttackHud);
            MessageAgregator<MsgCriatureUpdateButtonPress>.AddListener(OnPetUpdatePress);
            MessageAgregator<MsgCriatureDefeated>.AddListener(OnCriatureDefeated);
            MessageAgregator<MsgRequestHumanDamage>.AddListener(OnRequestHumanDamage);
            MessageAgregator<MsgRequestExternalMoviment>.AddListener(OnRequestExternalMoviment);
            MessageAgregator<MsgFinishPauseMenu>.AddListener(OnFinishPauseMenu);
            MessageAgregator<MsgNewSeen>.AddListener(OnReveiceNewSeen);
            MessageAgregator<MsgBlockPetAdvanceInTrigger>.AddListener(OnPetTriggerBlockable);
            MessageAgregator<MsgChangeGameScene>.AddListener(OnChangeGameScene);
            MessageAgregator<MsgSlopeSlip>.AddListener(OnSlopeSlip);

        }

        private void OnDestroy()
        {
            MessageAgregator<MsgChangeToHero>.RemoveListener(OnChangeToHero);
            MessageAgregator<MsgRequestChangeSelectedPetWithPet>.RemoveListener(OnRequestChangePet);
            MessageAgregator<MsgStartExternalInteraction>.RemoveListener(OnStartTalk);
            MessageAgregator<MsgFinishExternalInteraction>.RemoveListener(OnFinishTalk);
            MessageAgregator<MsgChangeActivePet>.RemoveListener(OnChangeActivePet);
            MessageAgregator<MsgRequestReplacePet>.RemoveListener(OnRequestReplacePet);
            MessageAgregator<MsgRequestChangeToPetByReplace>.RemoveListener(OnRequestChangeToPetByReplace);
            MessageAgregator<MsgRequestChangeSelectedItemWithPet>.RemoveListener(OnRequestChangeSelectedItem);
            MessageAgregator<MsgRequestUseItem>.RemoveListener(OnRequestUseItem);
            MessageAgregator<MsgStartUseItem>.RemoveListener(OnStartUseItem);
            MessageAgregator<MsgPlayerPetDefeated>.RemoveListener(OnPlayerPetDefeated);
            MessageAgregator<MsgGetChestItem>.RemoveListener(OnGetChestItem);
            MessageAgregator<MsgEndNewAttackHud>.RemoveListener(OnFinishNewAttackHud);
            MessageAgregator<MsgCriatureUpdateButtonPress>.RemoveListener(OnPetUpdatePress);
            MessageAgregator<MsgCriatureDefeated>.RemoveListener(OnCriatureDefeated);
            MessageAgregator<MsgRequestHumanDamage>.RemoveListener(OnRequestHumanDamage);
            MessageAgregator<MsgRequestExternalMoviment>.RemoveListener(OnRequestExternalMoviment);
            MessageAgregator<MsgFinishPauseMenu>.RemoveListener(OnFinishPauseMenu);
            MessageAgregator<MsgNewSeen>.RemoveListener(OnReveiceNewSeen);
            MessageAgregator<MsgBlockPetAdvanceInTrigger>.RemoveListener(OnPetTriggerBlockable);
            MessageAgregator<MsgChangeGameScene>.RemoveListener(OnChangeGameScene);
            MessageAgregator<MsgSlopeSlip>.RemoveListener(OnSlopeSlip);
        }

        private void OnSlopeSlip(MsgSlopeSlip obj)
        {
            
            if (obj.slipped == gameObject)
            {
                PetAttackBase petAttack = SlopeSlipManager.Slip(gameObject, obj.hit);

                MessageAgregator<MsgRequestHumanDamage>.Publish(new MsgRequestHumanDamage()
                {
                    esseGolpe = petAttack,
                    gameObject = transform.gameObject,
                    autoReturnToMove = true
                });
            }
        }

        private void OnChangeGameScene(MsgChangeGameScene obj)
        {
            if (obj.personagem == transform)
            {
                //Debug.Log(ActivePet + " :MUDOU DE CENA");

                if (ActivePet != null)
                {
                    Destroy(ActivePet.gameObject);

                    //Debug.Log(ActivePet + " :2MUDOU DE CENA");

                    //FayvitSupportSingleton.SupportSingleton.Instance.InvokeOnCountFrame(() =>
                    //{
                        //Debug.Log(ActivePet + " :2.5f MUDOU DE CENA");
                        InicializarPet();
                    //},5);
                }
            }
        }

        private void OnPetTriggerBlockable(MsgBlockPetAdvanceInTrigger obj)
        {
            if (obj.pet == ActivePet.gameObject)
            {
                GameObject G = Resources.Load<GameObject>("particles/" + GeneralParticles.rollParticles.ToString());
                Destroy(Instantiate(G, ActivePet.transform.position, Quaternion.identity, transform), 3);
                Destroy(ActivePet.gameObject);
                SeletaDeCriatures();
                SetHeroCamera.Set(transform);
                FayvitSupportSingleton.SupportSingleton.Instance.InvokeInSeconds(() =>
                {
                    MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero()
                    {
                        myHero = gameObject
                    });
                },.25f);
                Destroy(Instantiate(G, ActivePet.transform.position, Quaternion.identity, transform), 3);
            }
        }

        private void OnReveiceNewSeen(MsgNewSeen obj)
        {
            Dados.Livro.AdicionaVisto(obj.name);
        }

        private void OnFinishPauseMenu(MsgFinishPauseMenu obj)
        {
            if (obj.dono == this)
            {
                ThisState = CharacterState.onFree;
            }
        }

        private void OnRequestExternalMoviment(MsgRequestExternalMoviment obj)
        {
            if (obj.oMovimentado == gameObject)
            {
                CurrentCommander.ZerarGatilhos();
                mov.MoveApplicator(Vector3.zero);
                ThisState = CharacterState.externalMovement;
            }
        }

        private void OnRequestHumanDamage(MsgRequestHumanDamage obj)
        {
            if (obj.autoReturnToMove)
                returnState = ThisState;
            else
                returnState = CharacterState.stopped;

            ThisState = CharacterState.inDamage;
            
            damageState.StartDamageState(obj.esseGolpe);
        }

        private void OnCriatureDefeated(MsgCriatureDefeated obj)
        {
            if (obj.atacker == ActivePet.gameObject)
            {
                dados.Cristais += (ContraTreinador? 2:1)*obj.doDerrotado.PetFeat.meusAtributos.PV.Maximo;
                MessageAgregator<MsgChangeCristalCount>.Publish(new MsgChangeCristalCount()
                {
                    newCristalCount =  dados.Cristais
                });

                dados.Livro.AdicionaDerrotado(obj.doDerrotado.NomeID);
            }
        }

        private void OnPetUpdatePress(MsgCriatureUpdateButtonPress obj)
        {
            if (obj.dono == gameObject && dados.TemGolpesPorAprender())
            {
                MessageAgregator<MsgPetEnterInAttackLearn>.Publish(new MsgPetEnterInAttackLearn()
                {
                    dono = gameObject
                });

                OpenUpdateMenu(FluxoDeRetorno.criature);
            }
        }

        private void OnFinishNewAttackHud(MsgEndNewAttackHud obj)
        {
            if (obj.fluxo == FluxoDeRetorno.heroi && obj.oAprendiz == ActivePet.MeuCriatureBase)
            {
                ThisState = CharacterState.onFree;
            }
        }

        private void OnGetChestItem(MsgGetChestItem obj)
        {
            dados.AdicionaItem(obj.nameItem, obj.quantidade);
            ChangeSelectedItem(0);
        }

        private void OnPlayerPetDefeated(MsgPlayerPetDefeated obj)
        {
            //if(obj.dono==donoDessaHud)
            if (obj.pet == ActivePet)
            {
                if (Dados.TemAlgumPetAtivoVivo())
                {

                    List<string> ls = TextBank.RetornaListaDeTextoDoIdioma(TextKey.apresentaDerrota);
                    string message = string.Format(ls[0], obj.pet.MeuCriatureBase.GetNomeEmLinguas) + " \n\r " + ls[1];
                    AbstractGlobalController.Instance.OneMessage.StartMessagePanel(() =>
                    {
                        //StartFields(obj.dono);
                        ThisState = CharacterState.stopedWithStoppedCam;

                        MessageAgregator<MsgOpenPetList>.Publish(new MsgOpenPetList()
                        {
                            armagedom = false,
                            dono = this
                        });
                    }, message);

                    ThisState = CharacterState.activeSingleMessageOpened;
                }
                else
                {
                    Debug.LogError("Ir para o armagedom");
                }
            }
        }

        private void OnStartUseItem(MsgStartUseItem obj)
        {
            Debug.Log(obj.usuario + " : " + gameObject);

            if (obj.usuario == gameObject)
            {
                if (dados.Itens.Count > 0)
                {
                    if (dados.ItemSai > dados.Itens.Count - 1)
                        dados.ItemSai = 0;
                }

                ThisState = CharacterState.stopedWithStoppedCam;
                MessageAgregator<MsgChangeSelectedItem>.Publish(new MsgChangeSelectedItem()
                {
                    nameItem = dados.Itens.Count > 0 ? dados.Itens[dados.ItemSai].ID : NameIdItem.generico,
                    quantidade = dados.Itens.Count > 0 ? dados.Itens[dados.ItemSai].Estoque : 0
                });
            }
        }

        private void OnRequestChangeSelectedItem(MsgRequestChangeSelectedItemWithPet obj)
        {
            if (obj.pet == ActivePet.gameObject)
            {
                ChangeSelectedItem(obj.change);
            }
        }

        private void OnRequestUseItem(MsgRequestUseItem obj)
        {
            if (obj.dono == gameObject)
            {
                StartUseItem(FluxoDeRetorno.criature);
                //ThisState = CharacterState.stopedWithStoppedCam;
            }
        }

        private void OnRequestChangeToPetByReplace(MsgRequestChangeToPetByReplace obj)
        {
            if (obj.dono == gameObject && obj.fluxo == FluxoDeRetorno.criature)
            {
                PublishChangeToPet(obj.lockTarget);
            }
        }

        private void OnRequestReplacePet(MsgRequestReplacePet obj)
        {
            if (obj.dono == gameObject)
            {
                if (obj.replaceIndex)
                    dados.CriatureSai = obj.newIndex;

                StartReplacePet(FluxoDeRetorno.criature, obj.lockTarget);
            }
        }

        private void OnChangeActivePet(MsgChangeActivePet obj)
        {
            if (obj.dono == gameObject)
            {
                ActivePet = obj.pet.GetComponent<PetManagerCharacter>();
                Debug.Log("active pet changed");

                PetBase P = dados.CriaturesAtivos[0];
                dados.CriaturesAtivos[0] = dados.CriaturesAtivos[dados.CriatureSai + 1];
                dados.CriaturesAtivos[dados.CriatureSai + 1] = P;

                MessageAgregator<MsgChangeSelectedPet>.Publish(
                new MsgChangeSelectedPet()
                {
                    petname = dados.CriaturesAtivos[dados.CriatureSai + 1].NomeID,
                    temGolpePorAprender = dados.TemGolpesPorAprender(0)
                });
            }
        }

        private void OnFinishTalk(MsgFinishExternalInteraction obj)
        {
            if (returnState == CharacterState.withPet)
            {
                PublishChangeToPet(null);
                ThisState = CharacterState.withPet;
            }
            else
            {
                ThisState = CharacterState.onFree;
                SetHeroCamera.Set(transform);
            }
        }

        private void OnStartTalk(MsgStartExternalInteraction obj)
        {
            returnState = ThisState;

            mov.MoveApplicator(Vector3.zero);
            ThisState = CharacterState.externalPanelOpened;

        }

        private void OnRequestChangePet(MsgRequestChangeSelectedPetWithPet obj)
        {
            if (obj.pet == ActivePet.gameObject)
            {
                ChangeSelectedPet();
            }
        }

        private void OnChangeToHero(MsgChangeToHero obj)
        {
            if (obj.myHero == gameObject && ThisState != CharacterState.onFree)
            {
                ThisState = CharacterState.onFree;
                if(!obj.blockReturnCam)
                    SetHeroCamera.Set(transform);
            }
        }

        void ChangeSelectedItem(int change)
        {
            if (dados.Itens.Count > 0)
            {
                dados.ItemSai = ContadorCiclico.Contar(change, dados.ItemSai, dados.Itens.Count);
                MessageAgregator<MsgChangeSelectedItem>.Publish(new MsgChangeSelectedItem()
                {
                    nameItem = dados.Itens[dados.ItemSai].ID,
                    quantidade = dados.Itens[dados.ItemSai].Estoque
                });
            }
        }

        void ChangeSelectedPet()
        {
            if (dados.CriaturesAtivos.Count > 1)
            {
                dados.CriatureSai = ContadorCiclico.Contar(1, dados.CriatureSai, dados.CriaturesAtivos.Count - 1);
                MessageAgregator<MsgChangeSelectedPet>.Publish(
                    new MsgChangeSelectedPet()
                    {
                        petname = dados.CriaturesAtivos[dados.CriatureSai + 1].NomeID
                    });
            }
        }

        private void RequisitarHudElements()
        {
            MessageAgregator<MsgStartGameElementsHud>.Publish(new MsgStartGameElementsHud()
            {
                petname = dados.CriaturesAtivos.Count > 1 ? dados.CriaturesAtivos[dados.CriatureSai + 1].NomeID : PetName.nulo,
                nameItem = dados.Itens.Count > 0 ? dados.Itens[dados.ItemSai].ID : NameIdItem.generico,
                countItem = dados.Itens.Count > 0 ? dados.Itens[dados.ItemSai].Estoque : 0,
                temGolpePorAprender = dados.TemGolpesPorAprender(),
                countCristals = dados.Cristais
            });
        }

        public void InicializarPet()
        {
            //Debug.Log(ActivePet + " 3:MUDOU DE CENA");
            ActivePet = PetInitialize.Initialize(transform, dados.CriaturesAtivos[0]).GetComponent<PetManagerCharacter>();
            RequisitarHudElements();
            //Debug.Log(ActivePet + " :4MUDOU DE CENA");
        }

        void SeletaDeCriatures()
        {
            if (InTeste)
            {
                if (dados.CriaturesAtivos.Count > 0 /*&& !eLoad*/)
                {
                    InicializarPet();
                }

                //Configurar Hud ...?
            }// aqui seria um senão para destruir o criature ativo caso exista
        }

        // Update is called once per frame
        void Update()
        {
            switch (ThisState)
            {
                case CharacterState.onFree:
                    MoveControl();
                    ControlCamera();
                    ActionCommand();
                break;
                case CharacterState.stopedWithStoppedCam:
                case CharacterState.withPet:
                    CurrentCommander.DirectionalVector();
                    mov.MoveApplicator(Vector3.zero);
                    break;
                case CharacterState.externalPanelOpened:
                    CurrentCommander.DirectionalVector();
                    mov.MoveApplicator(Vector3.zero);
                    
                    MessageAgregator<MsgSendExternalPanelCommand>.Publish(new MsgSendExternalPanelCommand()
                    {
                        confirmButton = CurrentCommander.GetButtonDown(CommandConverterInt.confirmButton, true),
                        returnButton = CurrentCommander.GetButtonDown(CommandConverterInt.returnButton, true),
                        hChange = CurrentCommander.GetIntTriggerDown(CommandConverterString.moveH) +
                            CurrentCommander.GetIntTriggerDown(CommandConverterString.alternativeH_Change),
                        vChange = CurrentCommander.GetIntTriggerDown(CommandConverterString.moveV) +
                            CurrentCommander.GetIntTriggerDown(CommandConverterString.alternativeV_Change)
                    });
                break;
                case CharacterState.activeSingleMessageOpened:

                    bool press = CurrentCommander.GetButtonDown(CommandConverterInt.confirmButton, true) ||
                        CurrentCommander.GetButtonDown(CommandConverterInt.returnButton, true);

                    AbstractGlobalController.Instance.OneMessage.ThisUpdate(press);

                    break;
                case CharacterState.NonBlockPanelOpened:
                    SingleCommands();

                    MessageAgregator<MsgSendExternalPanelCommand>.Publish(new MsgSendExternalPanelCommand()
                    {
                        confirmButton = CurrentCommander.GetButtonDown(CommandConverterInt.confirmButton, true),
                        returnButton = CurrentCommander.GetButtonDown(CommandConverterInt.returnButton, true),
                        hChange = CurrentCommander.GetIntTriggerDown(CommandConverterString.alternativeH_Change),
                        vChange = CurrentCommander.GetIntTriggerDown(CommandConverterString.alternativeV_Change),
                        extraButton = CurrentCommander.GetButtonDown(CommandConverterInt.updateMenu),
                        pauseMenu = CurrentCommander.GetButtonDown(CommandConverterInt.pauseMenu),
                        leftChangeButton = CurrentCommander.GetButtonDown(CommandConverterInt.itemUse),
                        rightChangeButton = CurrentCommander.GetButtonDown(CommandConverterInt.criatureChange),
                        leftTrigger = CurrentCommander.GetIntTriggerDown(CommandConverterString.focusInTheEnemy),
                        rightTrigger = CurrentCommander.GetIntTriggerDown(CommandConverterString.attack)
                    });

                break;
                case CharacterState.inDamage:
                    ControlCamera();
                    if (damageState.Update())
                    {
                        ThisState = returnState;
                    }
                    break;
                case CharacterState.stopped:
                    CurrentCommander.DirectionalVector();
                    mov.MoveApplicator(Vector3.zero);
                    ControlCamera();
                break;
            }

        }

        void OpenUpdateMenu(FluxoDeRetorno fluxo)
        {
            if (dados.TemGolpesPorAprender(0))
            {
                if (fluxo == FluxoDeRetorno.heroi)
                    ThisState = CharacterState.NonBlockPanelOpened;
                else
                    ThisState = CharacterState.externalPanelOpened;

                MessageAgregator<MsgRequestNewAttackHud>.Publish(new MsgRequestNewAttackHud()
                {
                    fluxo = fluxo,
                    oAprendiz = dados.CriaturesAtivos[0]
                });
            }
        }

        void ActionCommand()
        {
            if (CurrentCommander.GetButtonDown(CommandConverterInt.updateMenu))
            {
                Debug.Log("update commander");
                OpenUpdateMenu(FluxoDeRetorno.heroi);
            }
            else
            if (mov.IsGrounded && CurrentCommander.GetButtonDown(CommandConverterInt.humanAction, true))
            {
                MessageAgregator<MsgInvokeActionFromHud>.Publish(new MsgInvokeActionFromHud()
                { 
                    actionHudManagerActive = ActionHudManager.Active
                });
            }
            else if (CurrentCommander.GetButtonDown(CommandConverterInt.pauseMenu, true))
            {
                ThisState = CharacterState.NonBlockPanelOpened;
                MessageAgregator<MsgRequestPauseMenu>.Publish(new MsgRequestPauseMenu()
                {
                    dono = this
                });
            }
        }

        void PublishChangeToPet(Transform lockTarget)
        {
            PetBase P = dados.CriaturesAtivos[0];
            PetAtributes petA = P.PetFeat.meusAtributos;

            ThisState = CharacterState.withPet;
            MessageAgregator<MsgChangeToPet>.Publish(new MsgChangeToPet()
            {
                dono = transform,
                petName = P.NomeID,
                petToGoOut = dados.CriaturesAtivos.Count > 1 ? dados.CriaturesAtivos[dados.CriatureSai + 1].NomeID : PetName.nulo,
                atkSelected = P.GerenteDeGolpes.meusGolpes[P.GerenteDeGolpes.golpeEscolhido].Nome,
                numCriatures = dados.CriaturesAtivos.Count,
                currentHp = petA.PV.Corrente,
                currentMp = petA.PE.Corrente,
                currentSt = 1,
                maxSt = 1,
                level = P.PetFeat.mNivel.Nivel,
                maxHp = petA.PV.Maximo,
                maxMp = petA.PE.Maximo,
                name = P.GetNomeEmLinguas,
                oCriature = ActivePet.gameObject,
                lockTarget = lockTarget
            });
        }

        void SingleCommands()
        {

            Vector3 V = CameraApplicator.cam.SmoothCamDirectionalVector(
                    CurrentCommander.GetAxis(CommandConverterString.moveH),
                    CurrentCommander.GetAxis(CommandConverterString.moveV)
                    );

            bool run = CurrentCommander.GetButton(CommandConverterInt.run);
            bool startJump = CurrentCommander.GetButtonDown(CommandConverterInt.jump);
            bool pressJump = CurrentCommander.GetButton(CommandConverterInt.jump);

            if (mov != null)
                mov.MoveApplicator(V, run, startJump, pressJump);

        }

        void MoveControl()
        {
            SingleCommands();

            int itemchange = CurrentCommander.GetIntTriggerDown(CommandConverterString.itemChange);

            if (CurrentCommander.GetIntTriggerDown(CommandConverterString.selectAttack_selectCriature) > 0)
            {
                ChangeSelectedPet();
            }
            else if (itemchange != 0)
            {
                ChangeSelectedItem(itemchange);
            }

            if (mov.IsGrounded)
            {
                if (CurrentCommander.GetButtonDown(CommandConverterInt.heroToCriature, true))
                {
                    mov.MoveApplicator(Vector3.zero);

                    PublishChangeToPet(null);

                }
                else if (CurrentCommander.GetButtonDown(CommandConverterInt.criatureChange))
                {
                    StartReplacePet(FluxoDeRetorno.heroi, mov.LockTarget);
                }
                else if (CurrentCommander.GetButtonDown(CommandConverterInt.itemUse))
                {
                    //if (g.UsarTempoDeItem == UsarTempoDeItem.sempre || (g.UsarTempoDeItem == UsarTempoDeItem.emLuta && g.estaEmLuta))
                    //    gerente.Dados.TempoDoUltimoUsoDeItem = Time.time;
                    StartUseItem(FluxoDeRetorno.heroi);
                    //ThisState = CharacterState.stopedWithStoppedCam;
                }
            }
        }

        void StartUseItem(FluxoDeRetorno fluxo)
        {
            if (dados.Itens.Count > 0)
            {
                UseItemManager useItem = gameObject.AddComponent<UseItemManager>();
                useItem.StartFields(gameObject, dados.Itens, dados.ItemSai, fluxo);
                CameraApplicator.cam.RemoveMira();
            }
        }

        void StartReplacePet(FluxoDeRetorno fluxo, Transform lockTarget)
        {
            if (dados.CriaturesAtivos.Count > 1
                &&
                dados.CriaturesAtivos[dados.CriatureSai + 1].PetFeat.meusAtributos.PV.Corrente > 0)
            {
                PetReplaceManager prm = gameObject.AddComponent<PetReplaceManager>();
                prm.StartReplace(transform, ActivePet.transform, fluxo,
                    dados.CriaturesAtivos[dados.CriatureSai + 1], lockTarget
                    );
                ThisState = CharacterState.stopedWithStoppedCam;
            }
            else
            {
                if (dados.CriaturesAtivos.Count > 1 && dados.CriaturesAtivos[dados.CriatureSai + 1].PetFeat.meusAtributos.PV.Corrente <= 0)
                    MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
                    {
                        message = string.Format(
                        TextBank.RetornaListaDeTextoDoIdioma(TextKey.criatureParaMostrador)[1],
                        dados.CriaturesAtivos[dados.CriatureSai + 1].GetNomeEmLinguas
                    )
                    });
            }
        }

        void ControlCamera()
        {
            if (ThisState != CharacterState.stopedWithStoppedCam)
            {
                Vector2 V = new Vector3(
                    CurrentCommander.GetAxis(CommandConverterString.camX),
                    CurrentCommander.GetAxis(CommandConverterString.camY)
                    );

                bool focar = CurrentCommander.GetButtonDown(CommandConverterInt.camFocus);
                CameraApplicator.cam.ValoresDeCamera(V.x, V.y, focar, mov.Controller.velocity.sqrMagnitude > .1f);
            }
        }
    }

    public struct MsgChangeToPet : IMessageBase
    {
        public int numCriatures;
        //public int numItens;
        public int level;
        public int currentHp;
        public int maxHp;
        public int currentMp;
        public int maxMp;
        public int currentSt;
        public int maxSt;
        public string name;
        public GameObject oCriature;
        public AttackNameId atkSelected;
        public PetName petName;
        public PetName petToGoOut;
        public Transform dono;
        public Transform lockTarget;
    }
    public struct MsgOpenPetList : IMessageBase
    {
        public CharacterManager dono;
        public bool armagedom;
    }
    public struct MsgPetEnterInAttackLearn : IMessageBase
    {
        public GameObject dono;
    }

    public struct MsgRequestExternalMoviment : IMessageBase
    {
        public GameObject oMovimentado;
    }

    public struct MsgRequestPauseMenu : IMessageBase
    {
        public CharacterManager dono;
    }

    public struct MsgFinishPauseMenu : IMessageBase
    {
        public CharacterManager dono;
    }
}