using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FayvitMessageAgregator;

namespace Criatures2021
{
    [System.Serializable]
    public class RecoveryItem : ConsumableItemBase
    {
        protected int valorDeRecuperacao = 40;
        public RecoveryItem(ItemFeatures C) : base(C) { }

        public override void IniciaUsoComCriature(GameObject dono, List<ItemBase> lista)
        {
            
            Lista = lista;
            PetAtributes P = dono.GetComponent<CharacterManager>().ActivePet.MeuCriatureBase.PetFeat.meusAtributos;
            IniciaUsoDesseItem(dono, QuantitativeItem.CanUseRecoveryItem(P));
        }

        public override bool AtualizaUsoComCriature()
        {
            return AtualizaUsoDesseItem(GeneralParticles.acaoDeCura1, FayvitSounds.SoundEffectID.XP_Heal01);
        }

        public override void IniciaUsoDeHeroi(GameObject dono, List<ItemBase> lista)
        {
            
            Lista = lista;
            PetAtributes P = dono.GetComponent<CharacterManager>().ActivePet.MeuCriatureBase.PetFeat.meusAtributos;
            IniciaUsoDesseItem(dono, QuantitativeItem.CanUseRecoveryItem(P));
        }

        public override bool AtualizaUsoDeHeroi()
        {
            return AtualizaUsoDesseItem(GeneralParticles.acaoDeCura1,FayvitSounds.SoundEffectID.XP_Heal01);
        }

        protected override void EscolhiEmQuemUsar(int indice)
        {
            PetAtributes A = Dono.GetComponent<CharacterManager>().Dados.CriaturesAtivos[indice].PetFeat.meusAtributos;            

            EscolhiEmQuemUsar(indice,
                QuantitativeItem.CanUseRecoveryItem(A), true,
                valorDeRecuperacao, A.PV.Corrente,
                A.PV.Maximo,
                PetTypeName.nulo);
        }

        public override void AcaoDoItemConsumivel(int indice)
        {
            PetBase pb = Dono.GetComponent<CharacterManager>().Dados.CriaturesAtivos[indice];
            PetAtributes P = pb.PetFeat.meusAtributos;

            int antHp = P.PV.Corrente;

            QuantitativeItem.RecuperaPV(P, valorDeRecuperacao);

            
            MessageAgregator<MsgChangeHP>.Publish(new MsgChangeHP()
            {
                antHp = antHp,
                currentHp = P.PV.Corrente,
                maxHp = P.PV.Maximo,
                target = pb
            });


            //ItemQuantitativo.RecuperaPV(C.CaracCriature.meusAtributos, valorDeRecuperacao);

            //if (!GameController.g.estaEmLuta)
            //    GameController.g.Salvador.SalvarAgora();
        }
    }

}