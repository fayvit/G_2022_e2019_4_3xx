using FayvitBasicTools;
using FayvitCam;
using FayvitSounds;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Criatures2021
{
    public class EletricBarrier : BarrierBarEventBase
    {
        [SerializeField] private SoundEffectID somDaParticula;
        [SerializeField] private GameObject particulaDoAcionamento;
        [SerializeField] private GameObject raioDoMotor;

        protected override void AtivacaoEspecifica()
        {
            VeririqueSom(TempoDeEfetivaAcao, somDaParticula);

            base.AtivacaoEspecifica();

            if (Estado == BarrierEventsState.barrasDescendo)
                NumJaRepetidos = 0;


        }
        protected override void EfetivadorDaAcao()
        {
            CameraApplicator.cam.NewFocusForBasicCam(transform, 10, 10, true, UsarForwardDoObjeto);
            raioDoMotor.SetActive(true);
        }

        public override void DisparaEvento(AttackNameId nomeDoGolpe)
        {
            if (EsseGolpeAtiva(nomeDoGolpe))
                particulaDoAcionamento.SetActive(true);

            if(!AbstractGameController.Instance.MyKeys.VerificaAutoShift(Chave))
            //if (!GameController.g.MyKeys.VerificaAutoShift(ID))
                base.DisparaEvento(nomeDoGolpe);
        }

    }
}