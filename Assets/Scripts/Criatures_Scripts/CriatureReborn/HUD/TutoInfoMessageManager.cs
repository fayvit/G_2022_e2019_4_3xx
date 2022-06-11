using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitMessageAgregator;
using FayvitSupportSingleton;
using FayvitBasicTools;
using Criatures2021;
using TextBankSpace;
using FayvitSounds;

namespace Criatures2021Hud
{
    public class TutoInfoMessageManager : ButtonActivate
    {
        [SerializeField] private InfoMessageType infoMessage = InfoMessageType.targetLock;

        public override void FuncaoDoBotao()
        {
            if (gameObject.activeSelf)
            {
                SomDoIniciar();
                MessageAgregator<MsgRequestOpenInfoMessage>.Publish(new MsgRequestOpenInfoMessage()
                {
                    info = infoMessage
                });
                MessageAgregator<MsgCloseInfoMessage>.AddListener(OnCloseInfoMessage);

                MessageAgregator<MsgStartExternalInteraction>.Publish();
            }
        }

        private void OnCloseInfoMessage(MsgCloseInfoMessage obj)
        {
            SupportSingleton.Instance.InvokeOnEndFrame(()=> {
                MessageAgregator<MsgCloseInfoMessage>.RemoveListener(OnCloseInfoMessage);
            });

            MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero()
            {
                myHero = AbstractGlobalController.Instance.Players[0].Manager.transform.gameObject
            });
        }

        // Start is called before the first frame update
        void Start()
        {
            SempreEstaNoTrigger();
            textoDoBotao = TextBank.RetornaListaDeTextoDoIdioma(TextKey.textoBaseDeAcao)[1];
        }
    }
}