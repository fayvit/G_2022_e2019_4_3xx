using UnityEngine;
using FayvitBasicTools;
using FayvitCam;

using FayvitMessageAgregator;

namespace Criatures2021
{

    //[ExecuteInEditMode]
    public class BarrierEventManager : BarrierEventBase
    {
        [Space(5)]
        [SerializeField] private GameObject barreira;
        [SerializeField] private GameObject acaoEfetivada;
        [SerializeField] private GameObject finalizaAcao;
        

        new void Update()
        {
            if (JaIniciaou)
            {
                switch (Estado)
                {
                    case BarrierEventsState.mensAberta:
                        if (externalCommand.confirmButton || externalCommand.returnButton)
                            AcaoDeMensAberta();
                    break;
                    case BarrierEventsState.ativou:
                        TempoDecorrido += Time.deltaTime;
                        VeririqueSom(TempoDeEfetivaAcao);
                        if (TempoDecorrido > TempoDeEfetivaAcao)
                        {
                            TempoDecorrido = 0;
                            finalizaAcao.SetActive(true);
                            barreira.SetActive(false);
                            Estado = BarrierEventsState.apresentaFinalizaAcao;
                            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                            {
                                sfxId = SomDaFinalizacao
                            });
                        }
                    break;
                    case BarrierEventsState.apresentaFinalizaAcao:
                        TempoDecorrido += Time.deltaTime;
                        if (TempoDecorrido > TempoDoFinalizaAcao)
                        {
                            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                            {
                                sfxId = VinhetaDaFinalizacao
                            });

                            gameObject.SetActive(false);
                            VoltarAoFLuxoDeJogo();
                        }
                    break;
                }
                base.Update();
            }
            else
            {
                //  if (Application.isEditor)
                //    Chave = BuscadorDeID.GetUniqueID(gameObject) + "_" + gameObject.scene.name;
                Start();
            }
        }

        protected override void EfetivadorDaAcao()
        {
            acaoEfetivada.SetActive(true);
            CameraApplicator.cam.NewFocusForBasicCam(transform, 10, 10, true, UsarForwardDoObjeto);
        }
    }
}