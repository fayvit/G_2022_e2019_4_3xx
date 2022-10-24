using UnityEngine;
using System.Collections;
using FayvitBasicTools;
using FayvitMessageAgregator;
using FayvitSupportSingleton;
using System.Collections.Generic;
using FayvitLoadScene;
using System;
using TextBankSpace;
using CustomizationSpace;

namespace Criatures2021
{
    public class MyGameController : AbstractGameController
    {
        [SerializeField] private StatusUpdater statusUpdater;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            CustomizationSavedChars.LoadSavedCharacters();

            MessageAgregator<MsgClearStatusUpdater>.AddListener(OnRequestClearStatusUpdater);
            MessageAgregator<MsgSendNewStatus>.AddListener(OnReceiveSendStatus);
            MessageAgregator<MsgRemoveStatus>.AddListener(OnRequestRemoveStatus);
            MessageAgregator<MsgRequestStatusParticles>.AddListener(OnRequestStatusParticles);
            MessageAgregator<MsgItemRemoveStatus>.AddListener(OnUseItemRemoveStatus);
            MessageAgregator<MsgClearNegativeStatus>.AddListener(OnRequestClearNegativeStatus);
            MessageAgregator<MsgUpdateDates>.AddListener(OnRequestUpdateDates);
            MessageAgregator<MsgStartRerturnToArmagedom>.AddListener(OnReturnToArmagedom);
            MessageAgregator<MsgVerifyEventMessage>.AddListener(OnRequestVerifyEvent);
            MessageAgregator<MsgVerifyPoisonDamageMessage>.AddListener(OnRequestPoisonDamageMessage);
            MessageAgregator<MsgVerifyPoisonDefeatedPet>.AddListener(OnPoisonDefeatedPet);
            

            ColocarKeysTrue();

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            MessageAgregator<MsgClearStatusUpdater>.RemoveListener(OnRequestClearStatusUpdater);
            MessageAgregator<MsgSendNewStatus>.RemoveListener(OnReceiveSendStatus);
            MessageAgregator<MsgRemoveStatus>.RemoveListener(OnRequestRemoveStatus);
            MessageAgregator<MsgRequestStatusParticles>.RemoveListener(OnRequestStatusParticles);
            MessageAgregator<MsgItemRemoveStatus>.RemoveListener(OnUseItemRemoveStatus);
            MessageAgregator<MsgClearNegativeStatus>.RemoveListener(OnRequestClearNegativeStatus);
            MessageAgregator<MsgUpdateDates>.RemoveListener(OnRequestUpdateDates);
            MessageAgregator<MsgStartRerturnToArmagedom>.RemoveListener(OnReturnToArmagedom);
            MessageAgregator<MsgVerifyEventMessage>.RemoveListener(OnRequestVerifyEvent);
            MessageAgregator<MsgVerifyPoisonDamageMessage>.RemoveListener(OnRequestPoisonDamageMessage);
            MessageAgregator<MsgVerifyPoisonDefeatedPet>.RemoveListener(OnPoisonDefeatedPet);
        }

        private void OnPoisonDefeatedPet(MsgVerifyPoisonDefeatedPet obj)
        {
            if (obj.afetado is PetManagerCharacter)
            {
                MessageAgregator<MsgWhoIsTheLoserPet>.Publish(new MsgWhoIsTheLoserPet()
                {
                    player = true
                });
                

                MessageAgregator<MsgRequestUpperLargeMessage>.Publish(new MsgRequestUpperLargeMessage()
                {
                    message = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.status)[0],
                                obj.pDoAfetado.GetNomeEmLinguas)
                });
            }
            else if (obj.afetado is PetManagerEnemy)
            {
                MessageAgregator<MsgWhoIsTheLoserPet>.Publish(new MsgWhoIsTheLoserPet()
                {
                    player = false
                });
            }
        }

        private void OnRequestPoisonDamageMessage(MsgVerifyPoisonDamageMessage obj)
        {
            if (obj.afetado != null)
            {
                if (obj.afetado.GetComponent<PetManager>() is PetManagerCharacter)
                    MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
                    {
                        message = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.status)[1],
                            obj.pDoAfetado.GetNomeEmLinguas, (int)obj.quantificador)
                    });
                else
                    MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
                    {
                        message = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.status)[2],
                             (int)obj.quantificador)
                    });
            }
            else
            {
                
                if(MyGlobalController.MainPlayer.Dados.CriaturesAtivos.Contains(obj.pDoAfetado))
                    MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
                    {
                        message = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.status)[1],
                                obj.pDoAfetado.GetNomeEmLinguas, (int)obj.quantificador)
                    });
            }
        }

        void OnRequestVerifyEvent(MsgVerifyEventMessage e)
        {
            GameObject atacado = e.atacado;
            AttackNameId Nome=e.atk.Nome;

            if (atacado.tag == "eventoComGolpe" /*&& !GameController.g.estaEmLuta*/)
            {
                atacado.GetComponent<EventoComGolpe>().DisparaEvento(Nome);
            }
        }

        private void OnReturnToArmagedom(MsgStartRerturnToArmagedom obj)
        {
            OnRequestClearStatusUpdater(new MsgClearStatusUpdater());
        }

        private void OnRequestUpdateDates(MsgUpdateDates obj)
        {
            if(obj.saveDates!=null && obj.saveDates.VariaveisChave!=null)
                MyKeys = obj.saveDates.VariaveisChave;
        }

        private void OnRequestClearStatusUpdater(MsgClearStatusUpdater obj)
        {
            statusUpdater.StatusDoHeroi = new List<StatusTemporarioBase>();
            statusUpdater.StatusDoInimigo = new List<StatusTemporarioBase>();
        }

        private void OnRequestClearNegativeStatus(MsgClearNegativeStatus obj)
        {
            statusUpdater.ClearNegativeStatus(obj.target);
        }

        private void ColocarKeysTrue()
        {
            MyKeys.MudaAutoShift(Criatures2021Hud.InfoMessageType.dodge.ToString() + "open", true);
            MyKeys.MudaShift(KeyShift.sempretrue, true);
        }

        private void OnUseItemRemoveStatus(MsgItemRemoveStatus obj)
        {
            foreach (var v in statusUpdater.StatusDoHeroi)
            {
                if (v.OAfetado == obj.petTarget && v.Dados.Tipo == obj.statusForRemove)
                {
                    SupportSingleton.Instance.InvokeOnEndFrame(() =>
                    {
                        v.RetiraComponenteStatus();
                    });
                }
            }
        }

        private void OnRequestStatusParticles(MsgRequestStatusParticles obj)
        {
            foreach (var v in statusUpdater.StatusDoHeroi)
            {
                if (v.OAfetado == obj.pet.MeuCriatureBase)
                {
                    v.CDoAfetado = obj.pet;
                    v.RecolocaParticula();
                }
            }
        }

        private void OnRequestRemoveStatus(MsgRemoveStatus obj)
        {
            statusUpdater.VerificaRemoveStatus(obj.status);
        }

        private void OnReceiveSendStatus(MsgSendNewStatus obj)
        {

            if (obj.deMenu || obj.receiver.GetComponent<PetManager>() is PetManagerCharacter)
            {
                Debug.Log("é do heroi");
                statusUpdater.AdicionaStatusAoHeroi(obj.S);
            }
            else
            {
                Debug.Log("é inimigo");
                statusUpdater.AdicionaStatusAoInimigo(obj.S);
            }
            
        }

        

       

        protected override void Update()
        {
            statusUpdater.Update();
            base.Update();
        }
    }

    public struct MsgClearStatusUpdater : IMessageBase
    { }

}
