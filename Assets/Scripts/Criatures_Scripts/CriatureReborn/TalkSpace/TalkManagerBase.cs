using UnityEngine;
using FayvitMessageAgregator;
using TextBankSpace;

namespace TalkSpace
{
    [System.Serializable]
    public class TalkManagerBase
    {
        [SerializeField] protected Sprite fotoDoNPC;

        private TextKey chaveDaConversa = TextKey.bomDia;

        protected EstadoDoNPC estado = EstadoDoNPC.parado;
        protected string[] conversa;

        protected enum EstadoDoNPC
        {
            caminhando,
            parado,
            conversando,
            finalizadoComBotao
        }

        public GameObject GO_Reference { get; set; }

        public void ChangeTalkKey(TextKey chave)
        {
            conversa = TextBank.RetornaListaDeTextoDoIdioma(chave).ToArray();
        }

        public void ChangeSpriteView(Sprite S)
        {
            fotoDoNPC = S;
        }

        public void Start()
        {
            conversa = TextBank.RetornaListaDeTextoDoIdioma(chaveDaConversa).ToArray();
        }

        // Update is called once per frame
        public virtual bool Update(bool inputNext, bool inputFinish)
        {
            switch (estado)
            {

                case EstadoDoNPC.conversando:

                    if (DisplayTextManager.instance.DisplayText.UpdateTexts(inputNext,inputFinish,conversa, fotoDoNPC))
                    {
                        FinalizaConversa();
                    }
                break;
                case EstadoDoNPC.finalizadoComBotao:
                    estado = EstadoDoNPC.parado;
                    return true;
            }

            return false;
        }

        protected virtual void FinalizaConversa()
        {
            estado = EstadoDoNPC.finalizadoComBotao;

            MessageAgregator<MsgFinishTextDisplay>.Publish(new MsgFinishTextDisplay()
            {
                talkKey = chaveDaConversa,
                sender = GO_Reference
            });

        }

        public virtual void IniciaConversa()
        {
            MessageAgregator<MsgStartTextDisplay>.Publish(new MsgStartTextDisplay()
            {
                sender = GO_Reference,
                talkKey = chaveDaConversa
            }) ;

            DisplayTextManager.instance.DisplayText.StartTextDisplay();

            estado = EstadoDoNPC.conversando;
        }
    }

    public struct MsgFinishTextDisplay : IMessageBase {
        public TextKey talkKey;
        public GameObject sender;
    }

    public struct MsgStartTextDisplay : IMessageBase {
        public TextKey talkKey;
        public GameObject sender;
    }
}