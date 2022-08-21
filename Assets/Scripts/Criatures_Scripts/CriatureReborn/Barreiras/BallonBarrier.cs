using FayvitBasicTools;
using FayvitCam;
using FayvitSounds;
using System.Collections;
using UnityEngine;

namespace Criatures2021
{
    public class BallonBarrier : BarrierBarEventBase
    {
        [SerializeField] private Transform balaoQueSobe;
        [SerializeField] private float alturaDaSubida = 10;
        [SerializeField] private SoundEffectID somDaParticula = SoundEffectID.Fire6;
        [SerializeField] private GameObject particulaDoAcionamento;


        private Vector3 startPosition;

        private enum LocalState
        {
            neutro,
            balaoSubindo,
            balaaoSubiu
        }

        float Interpolation(float inZeroOne)
        {
            return 0.5f * (Mathf.Pow(2f * inZeroOne - 1, 1 / 3) + 1);
        }

        new protected void Start()
        {
            startPosition = balaoQueSobe.position;
            base.Start();
        }

        protected override void AtivacaoEspecifica()
        {
            balaoQueSobe.position = Vector3.Lerp(
                startPosition, 
                startPosition + alturaDaSubida * Vector3.up, 
                TempoDecorrido / TempoDeEfetivaAcao
                );

            base.AtivacaoEspecifica();



        }

        protected override void EfetivadorDaAcao()
        {
            CameraApplicator.cam.NewFocusForBasicCam(balaoQueSobe, 2, 10, true, UsarForwardDoObjeto);
            particulaDoAcionamento.SetActive(true);
            FayvitSupportSingleton.SupportSingleton.Instance.InvokeInSeconds(() =>
            {
                FayvitMessageAgregator.MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                {
                    sfxId = somDaParticula
                });
            }, .15f);
        }

        public override void DisparaEvento(AttackNameId nomeDoGolpe)
        {
            if(!AbstractGameController.Instance.MyKeys.VerificaAutoShift(Chave))
                base.DisparaEvento(nomeDoGolpe);
        }

    }
}