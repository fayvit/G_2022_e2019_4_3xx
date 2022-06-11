using UnityEngine;
using System.Collections.Generic;
using FayvitMessageAgregator;
using Criatures2021Hud;
using TextBankSpace;

namespace Criatures2021
{
    [System.Serializable]
    public class ItemAntiStatusBase : ConsumableItemBase
    {
        protected StatusType qualStatusRemover = StatusType.nulo;
        public ItemAntiStatusBase(ItemFeatures C) : base(C) { }

        bool VerificaPodeUsarStatus(PetBase C)
        {
            var A = C.PetFeat.meusAtributos;
            int temStatus = StatusTemporarioBase.ContemStatus(qualStatusRemover, C);
            bool vivo = A.PV.Corrente > 0;

            return vivo && temStatus > -1;
        }

        public override void IniciaUsoComCriature(GameObject dono, List<ItemBase> lista)
        {
            Lista = lista;
            IniciaUsoDesseItem(dono, VerificaPodeUsarStatus(dono.GetComponent<CharacterManager>().ActivePet.MeuCriatureBase));
        }

        public override bool AtualizaUsoComCriature()
        {
            return AtualizaUsoDesseItem(GeneralParticles.animaAntiStatus,FayvitSounds.SoundEffectID.Darkness8);
        }

        public override void IniciaUsoDeHeroi(GameObject dono, List<ItemBase> lista)
        {
            Lista = lista;
            IniciaUsoDesseItem(dono, VerificaPodeUsarStatus(dono.GetComponent<CharacterManager>().ActivePet.MeuCriatureBase));
        }

        public override bool AtualizaUsoDeHeroi()
        {
            return AtualizaUsoDesseItem(GeneralParticles.animaAntiStatus, FayvitSounds.SoundEffectID.Darkness8);
        }

        protected override void EscolhiEmQuemUsar(int indice)
        {
            PetBase C = Dono.GetComponent<CharacterManager>().Dados.CriaturesAtivos[indice];
            PetAtributes A = C.PetFeat.meusAtributos;

            int temStatus = StatusTemporarioBase.ContemStatus(qualStatusRemover, C);
            bool vivo = A.PV.Corrente > 0;

            if (temStatus > -1 || !vivo)
            {
                EscolhiEmQuemUsar(indice,
                    vivo, true);
            }
            else
            {
                MessageAgregator<MsgNotUseItem>.Publish(new MsgNotUseItem()
                {
                    notMessage = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.itens)[9], C.GetNomeEmLinguas)
                });

                //MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
                //{
                //    message = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.itens)[9], C.GetNomeEmLinguas)
                //}) ;
                //MensDeUsoDeItem.MensDeNaoPrecisaDesseItem(C.NomeEmLinguas);
            }
        }


        public override void AcaoDoItemConsumivel(int indice)
        {
            Debug.Log("Acão do Item anti Status: ");
            MessageAgregator<MsgItemRemoveStatus>.Publish(new MsgItemRemoveStatus()
            {
                petTarget = Dono.GetComponent<CharacterManager>().Dados.CriaturesAtivos[indice],
                statusForRemove = qualStatusRemover
            });


            //List<PetBase> meusC = Dono.GetComponent<CharacterManager>().Dados.CriaturesAtivos;
            ////StatusTemporarioBase[] meusStatus = MyGameController.Instance.ContStatus.StatusDoHeroi.ToArray();
            //StatusTemporarioBase sTb = null;

            //for (int i = 0; i < meusStatus.Length; i++)
            //{
            //    //Debug.Log(meusC.IndexOf(meusStatus[i].OAfetado) + " :" + meusC.IndexOf(C) + " : " + meusStatus[i].Dados.Tipo + " : " + qualStatusRemover);
            //    if (meusC.IndexOf(meusStatus[i].OAfetado) == meusC.IndexOf(C) && meusStatus[i].Dados.Tipo == qualStatusRemover)
            //        sTb = meusStatus[i];
            //}

            //if (sTb != null)
            //    sTb.RetiraComponenteStatus();
            //else
            //    Debug.Log("Status foi alcançado como nulo");

        }
    }

    public struct MsgItemRemoveStatus : IMessageBase
    {
        public PetBase petTarget;
        public StatusType statusForRemove;
    }
}