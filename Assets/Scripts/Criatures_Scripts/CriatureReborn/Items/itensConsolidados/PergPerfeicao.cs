using System.Collections.Generic;
using UnityEngine;
using FayvitMessageAgregator;
using FayvitSupportSingleton;

namespace Criatures2021
{
    [System.Serializable]
    public class PergPerfeicao : ConsumableItemBase
    {
        public PergPerfeicao(int estoque = 1) : base(new ItemFeatures(NameIdItem.pergaminhoDePerfeicao)
        {
            valor = 400
        }
        )
        {
            Estoque = estoque;
        }

        public override void IniciaUsoDeMenu(GameObject dono, List<ItemBase> lista)
        {
            Dono = dono;
            Lista = lista;
            eMenu = true;

            MessageAgregator<MsgMenuStartUseItem>.Publish(new MsgMenuStartUseItem()
            {
                response = MsgMenuStartUseItem.StartUseResponse.escolhaEmQuemUsar
            });

            MessageAgregator<MsgChoseItemTarget>.AddListener(OnChosenTarget);
        }

        private void OnChosenTarget(MsgChoseItemTarget obj)
        {
            EscolhiEmQuemUsar(obj.indexOfTarget);
            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<MsgChoseItemTarget>.RemoveListener(OnChosenTarget);
            });
        }

        protected override void EscolhiEmQuemUsar(int indice)
        {
            PetBase C = Dono.GetComponent<CharacterManager>().ActivePet.MeuCriatureBase;
            PetAtributes A = C.PetFeat.meusAtributos;

            EscolhiEmQuemUsar(indice,
            QuantitativeItem.NeedUsePerfection(C), true,
            -1, A.PV.Corrente,
            A.PV.Maximo,
            PetTypeName.nulo);
        }

        public override void IniciaUsoDeHeroi(GameObject dono, List<ItemBase> lista)
        {

            Lista = lista;
            eMenu = false;
            PetBase P = dono.GetComponent<CharacterManager>().ActivePet.MeuCriatureBase;
            IniciaUsoDesseItem(dono, QuantitativeItem.NeedUsePerfection(P));
        }

        public override void IniciaUsoComCriature(GameObject dono, List<ItemBase> lista)
        {
            IniciaUsoDeHeroi(dono, lista);
        }

        public override bool AtualizaUsoComCriature()
        {
            return AtualizaUsoDesseItem(GeneralParticles.particulaPerfeicao,FayvitSounds.SoundEffectID.XP_Heal02);
        }

        public override bool AtualizaUsoDeHeroi()
        {
            return AtualizaUsoDesseItem(GeneralParticles.particulaPerfeicao, FayvitSounds.SoundEffectID.XP_Heal02);
        }

        public override void AcaoDoItemConsumivel(int indice)
        {
            PetBase pb = Dono.GetComponent<CharacterManager>().Dados.CriaturesAtivos[indice];
            PetAtributes P = pb.PetFeat.meusAtributos;

            int antHp = P.PV.Corrente;
            int antMp = P.PE.Corrente;

            pb.EstadoPerfeito();


            MessageAgregator<MsgChangeHP>.Publish(new MsgChangeHP()
            {
                antHp = antHp,
                currentHp = P.PV.Corrente,
                maxHp = P.PV.Maximo,
                target = pb
            });

            MessageAgregator<MsgChangeMP>.Publish(new MsgChangeMP()
            {
                antMp=antMp,
                currentMp = P.PE.Corrente,
                maxMp = P.PE.Maximo,
                target = pb
            });


            //ItemQuantitativo.RecuperaPV(C.CaracCriature.meusAtributos, valorDeRecuperacao);

            //if (!GameController.g.estaEmLuta)
            //    GameController.g.Salvador.SalvarAgora();
        }
    }
}