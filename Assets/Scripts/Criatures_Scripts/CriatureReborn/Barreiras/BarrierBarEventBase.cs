using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitBasicTools;
using FayvitCam;
using FayvitMessageAgregator;

namespace Criatures2021
{
    public class BarrierBarEventBase : BarrierEventBase
    {
        [SerializeField] private Transform barras;
        [SerializeField] private Transform focoDaCam;
        [SerializeField] private GameObject particulaDaAcao;

        private float tempoDeBarra = 2.5f;
        private Vector3 originalPosition;
        private Vector3 hidePosition;

        public GameObject ParticulaDaAcao
        {
            get { return particulaDaAcao; }
        }

        public Transform FocoDaCam
        {
            get { return focoDaCam; }
        }

        protected override void EfetivadorDaAcao() { }

        new void Start()
        {
            originalPosition = barras.position;
            hidePosition = barras.position - 8 * Vector3.up;
            base.Start();
        }

        new void Update()
        {

            if (JaIniciaou)
            {
                if(!AbstractGameController.Instance.MyKeys.VerificaAutoShift(Chave))
                //if (!GameController.g.MyKeys.VerificaAutoShift(ID))
                    base.Update();

                CaseDoNaoFeito();

                CaseDoFeito();
            }
            else
            {
                Start();
            }
        }

        protected virtual void AtivacaoEspecifica()
        {
            TempoDecorrido += Time.deltaTime;
            if (TempoDecorrido > TempoDeEfetivaAcao)
            {
                TempoDecorrido = 0;
                Estado = BarrierEventsState.barrasDescendo;
                CameraApplicator.cam.NewFocusForBasicCam(FocoDaCam, 10, 10, true, UsarForwardDoObjeto);
                ParticulaDaAcao.SetActive(true);
            }
        }

        protected virtual void CaseDoFeito() { }

        protected virtual void ApresentacaoDeFinalizacaoEspecifica()
        {
            TempoDecorrido += Time.deltaTime;
            if (TempoDecorrido > TempoDoFinalizaAcao)
            {
                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                {
                    sfxId = VinhetaDaFinalizacao
                });
                //EventAgregator.Publish(new StandardSendStringEvent(gameObject, SoundEffectID.Item.ToString(), EventKey.disparaSom));
                Estado = BarrierEventsState.emEspera;
                VoltarAoFLuxoDeJogo();
            }
        }

        protected virtual void BarraDescendo()
        {
            VeririqueSom(tempoDeBarra);
            TempoDecorrido += Time.deltaTime;
            barras.position = Vector3.Lerp(originalPosition, hidePosition, TempoDecorrido / tempoDeBarra);

            if (TempoDecorrido > tempoDeBarra)
            {
                TempoDecorrido = 0;
                ParticulaDaAcao.SetActive(false);
                Destroy(GetComponent<BoxCollider>());
                Estado = BarrierEventsState.apresentaFinalizaAcao;
                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                {
                    sfxId = SomDaFinalizacao
                });
            }
        }

        protected virtual void CaseDoNaoFeito()
        {
            switch (Estado)
            {
                case BarrierEventsState.mensAberta:
                    if (externalCommand.confirmButton||externalCommand.returnButton)
                    {
                        AcaoDeMensAberta();
                    }
                    break;
                case BarrierEventsState.ativou:
                    AtivacaoEspecifica();
                    break;
                case BarrierEventsState.barrasDescendo:
                    BarraDescendo();
                    break;
                case BarrierEventsState.apresentaFinalizaAcao:
                    ApresentacaoDeFinalizacaoEspecifica();
                    break;
            }
        }
    }
}