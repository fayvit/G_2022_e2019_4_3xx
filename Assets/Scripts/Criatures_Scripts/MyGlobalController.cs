using UnityEngine;
using FayvitBasicTools;
using FayvitMessageAgregator;
using FayvitUI;
using FayvitSounds;
using Criatures2021Hud;
using TextBankSpace;
using System;
using FayvitMove;

namespace Criatures2021
{
    public class MyGlobalController : AbstractGlobalController
    {
        private static CharacterManager mainManager;

        protected override void Start()
        {
            MessageAgregator<MsgGetChestItem>.AddListener(OnGetChestItem);
            MessageAgregator<MsgChangeOptionUI>.AddListener(OnChangeOptionUI);
            MessageAgregator<MsgPositiveUiInput>.AddListener(OnPositiveUiInput);
            MessageAgregator<MsgNegativeUiInput>.AddListener(OnNegativeUiInput);
            MessageAgregator<FillTextDisplayMessage>.AddListener(OnBoxGoingOut);
            MessageAgregator<MsgHideShowItem>.AddListener(OnHideUpperMessage);
            MessageAgregator<MsgRequestNewAttackHud>.AddListener(OnRequestNewAttackHud);
            MessageAgregator<MsgCloseNewAttackHudNonFinally>.AddListener(OnNewAttackNonFinnaly);
            MessageAgregator<MsgEnterInAggressiveResponse>.AddListener(OnTriggerAggressiveResponse);
            MessageAgregator<MsgOpenOneMessage>.AddListener(OnOpenOneMessage);
            MessageAgregator<MsgCloseMessagePanel>.AddListener(OnCloseOneMessage);
            MessageAgregator<MsgConfirmationPanelChangeOption>.AddListener(OnConfirmationOptionChange);
            MessageAgregator<MsgStartRoll>.AddListener(OnStartRoll);

            base.Start();
        }

        protected override void OnDestroy()
        {
            MessageAgregator<MsgGetChestItem>.RemoveListener(OnGetChestItem);
            MessageAgregator<MsgChangeOptionUI>.RemoveListener(OnChangeOptionUI);
            MessageAgregator<MsgPositiveUiInput>.RemoveListener(OnPositiveUiInput);
            MessageAgregator<MsgNegativeUiInput>.RemoveListener(OnNegativeUiInput);
            MessageAgregator<FillTextDisplayMessage>.RemoveListener(OnBoxGoingOut);
            MessageAgregator<MsgHideShowItem>.RemoveListener(OnHideUpperMessage);
            MessageAgregator<MsgRequestNewAttackHud>.RemoveListener(OnRequestNewAttackHud);
            MessageAgregator<MsgCloseNewAttackHudNonFinally>.RemoveListener(OnNewAttackNonFinnaly);
            MessageAgregator<MsgEnterInAggressiveResponse>.RemoveListener(OnTriggerAggressiveResponse);
            MessageAgregator<MsgOpenOneMessage>.RemoveListener(OnOpenOneMessage);
            MessageAgregator<MsgCloseMessagePanel>.RemoveListener(OnCloseOneMessage);
            MessageAgregator<MsgConfirmationPanelChangeOption>.RemoveListener(OnConfirmationOptionChange);
            MessageAgregator<MsgStartRoll>.RemoveListener(OnStartRoll);


            base.OnDestroy();

            Confirmation.ChangeBtnYesText(TextBank.RetornaFraseDoIdioma(TextKey.simOuNao));
            Confirmation.ChangeBtnNoText(TextBank.RetornaListaDeTextoDoIdioma(TextKey.simOuNao)[1]);
        }

        private void OnStartRoll(MsgStartRoll obj)
        {
            Sfx.Instantiate3dSound(obj.gameObject.transform, SoundEffectID.Evasion2);
        }

        private void OnConfirmationOptionChange(MsgConfirmationPanelChangeOption obj)
        {
            if (!obj.hideSound)
                Sfx.PlaySfx(SoundEffectID.Cursor1);
        }

        private void OnCloseOneMessage(MsgCloseMessagePanel obj)
        {
            if (!obj.hideSound)
                Sfx.PlaySfx(SoundEffectID.Book1);
        }

        private void OnOpenOneMessage(MsgOpenOneMessage obj)
        {
            if (!obj.hideSound)
                Sfx.PlaySfx(SoundEffectID.painelAbrindo);
        }

        public static CharacterManager MainPlayer {
            get {
                if (mainManager == null)
                    if(MainCharTransform!=null)
                        mainManager = MainCharTransform.GetComponent<CharacterManager>();

                return mainManager;
            }
        }

        public static Transform MainCharTransform { 
            get {
                if (mainManager != null)
                    return mainManager.transform;
                else
                if (Instance != null && Instance.Players != null && Instance.Players.Count > 0)
                    return Instance.Players[0].Manager.transform;
                else
                    return null;
            } 
        }

        private void OnTriggerAggressiveResponse(MsgEnterInAggressiveResponse obj)
        {
            
            if (obj.enemyPet.PetFeat.meusAtributos.PV.Corrente > 0
                &&
                Music.CurrentActiveMusic.Musica != ResourcesFolders.GetClip(NameMusic.TicoTicoNoFuba_v1)
                &&
                !MainPlayer.ContraTreinador
                )
            {
                Music.StartMusicRememberingCurrent(NameMusic.TicoTicoNoFuba_v1);
            }
        }

        private void OnNewAttackNonFinnaly(MsgCloseNewAttackHudNonFinally obj)
        {
            Sfx.PlaySfx(FayvitSounds.SoundEffectID.Book1);
        }

        private void OnRequestNewAttackHud(MsgRequestNewAttackHud obj)
        {
            Sfx.PlaySfx(FayvitSounds.SoundEffectID.painelAbrindo);
        }

        private void OnHideUpperMessage(MsgHideShowItem obj)
        {
            Sfx.PlaySfx(FayvitSounds.SoundEffectID.Book1);
        }

        private void OnBoxGoingOut(FillTextDisplayMessage obj)
        {
            Sfx.PlaySfx(FayvitSounds.SoundEffectID.Book1);
        }

        private void OnNegativeUiInput(MsgNegativeUiInput obj)
        {
            Sfx.PlaySfx(FayvitSounds.SoundEffectID.Book1);
        }

        private void OnPositiveUiInput(MsgPositiveUiInput obj)
        {
            Sfx.PlaySfx(FayvitSounds.SoundEffectID.Decision2);
        }

        private void OnChangeOptionUI(MsgChangeOptionUI obj)
        {
            Sfx.PlaySfx(FayvitSounds.SoundEffectID.Cursor1);
        }

        private void OnGetChestItem(MsgGetChestItem obj)
        {
            //Sfx.PlaySfx(obj.getSfx);
            Debug.Log("Ignorei o Sfx enviado pelo Chestmanager");
            TuinManager.RequestDoubleDown();
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}