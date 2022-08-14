using Criatures2021Hud;
using FayvitMessageAgregator;
using System.Collections.Generic;
using TextBankSpace;
using UnityEngine;

namespace Criatures2021
{
    [System.Serializable]
    public class UnusableItem : ItemBase
    {
        public UnusableItem(ItemFeatures cont) : base(cont){}

        public override void IniciaUsoDeMenu(GameObject dono, List<ItemBase> lista)
        {
            MessageAgregator<MsgMenuStartUseItem>.Publish(new MsgMenuStartUseItem()
            {
                response = MsgMenuStartUseItem.StartUseResponse.naoPodeUsar
            });
        }

        public override void IniciaUsoComCriature(GameObject dono, List<ItemBase> lista)
        {
            Estado = ItemUseState.finalizaUsaItem;

            MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
            {
                message = TextBank.RetornaFraseDoIdioma(TextKey.mensLuta)
            });
        }

        public override void IniciaUsoDeHeroi(GameObject dono, List<ItemBase> lista)
        {
            IniciaUsoComCriature(dono, lista);
        }


    }
}