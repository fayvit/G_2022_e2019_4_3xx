using FayvitBasicTools;
using FayvitMessageAgregator;
using System;
using UnityEngine;

namespace Criatures2021
{
    public class SimpleStoneBarrier : SimpleBarrierBase
    {
        [SerializeField] private GameObject barreira;
        [SerializeField] private GameObject acaoEfetivada;

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
                        //VeririqueSom(TempoDeEfetivaAcao);
                        if (TempoDecorrido > TempoDeEfetivaAcao)
                        {
                            TempoDecorrido = 0;
                            //finalizaAcao.SetActive(true);
                            //barreira.SetActive(false);
                            Estado = BarrierEventsState.apresentaFinalizaAcao;
                            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                            {
                                sfxId = VinhetaDaFinalizacao
                            });

                            gameObject.SetActive(false);

                            //FayvitSupportSingleton.SupportSingleton.Instance.InvokeOnEndFrame(() =>
                            //{
                            //    VoltarAoFLuxoDeJogo();
                            //});
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
            barreira.SetActive(false);
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = SomDaFinalizacao
            });

        }
    }
}
