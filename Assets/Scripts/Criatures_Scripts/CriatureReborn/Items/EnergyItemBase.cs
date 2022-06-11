using FayvitMessageAgregator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Criatures2021
{
    [System.Serializable]
    public class EnergyItemBase : ConsumableItemBase
    {
        protected PetTypeName recuperaDoTipo = PetTypeName.nulo;
        protected int valorDeRecuperacao = 40;

        public EnergyItemBase(ItemFeatures C) : base(C) { }


        public override void IniciaUsoComCriature(GameObject dono, List<ItemBase> lista)
        {
            
            Lista = lista;
            PetFeatures P = dono.GetComponent<CharacterManager>().ActivePet.MeuCriatureBase.PetFeat;
            IniciaUsoDesseItem(dono, QuantitativeItem.CanUseEnergyItem(P.meusAtributos),P.TemOTipo(recuperaDoTipo), recuperaDoTipo);
        }

        public override bool AtualizaUsoComCriature()
        {
            return AtualizaUsoDesseItem(GeneralParticles.animaPE,FayvitSounds.SoundEffectID.XP_Heal07);
        }

        public override void IniciaUsoDeHeroi(GameObject dono, List<ItemBase> lista)
        {
            
            Lista = lista;
            PetFeatures P = dono.GetComponent<CharacterManager>().ActivePet.MeuCriatureBase.PetFeat;
            IniciaUsoDesseItem(dono, QuantitativeItem.CanUseEnergyItem(P.meusAtributos),P.TemOTipo(recuperaDoTipo),recuperaDoTipo);
        }

        public override bool AtualizaUsoDeHeroi()
        {
            return AtualizaUsoDesseItem(GeneralParticles.animaPE, FayvitSounds.SoundEffectID.XP_Heal07);
        }

        protected override void EscolhiEmQuemUsar(int indice)
        {
            PetFeatures P = Dono.GetComponent<CharacterManager>().Dados.CriaturesAtivos[indice].PetFeat;
            PetAtributes A = P.meusAtributos;

            EscolhiEmQuemUsar(indice,
                QuantitativeItem.CanUseEnergyItem(A) , P.TemOTipo(recuperaDoTipo),
                valorDeRecuperacao, A.PE.Corrente,
                A.PE.Maximo,
                recuperaDoTipo);
        }

        public override void AcaoDoItemConsumivel(int indice)
        {
            

            PetBase pb = Dono.GetComponent<CharacterManager>().Dados.CriaturesAtivos[indice];
            PetAtributes P = pb.PetFeat.meusAtributos;

            int antMp = P.PE.Corrente;


            QuantitativeItem.RecuperaPE(P, valorDeRecuperacao);

            
            MessageAgregator<MsgChangeMP>.Publish(new MsgChangeMP()
            {
                antMp = antMp,
                currentMp = P.PE.Corrente,
                maxMp = P.PE.Maximo,
                target = pb
            });
        }
    }
}