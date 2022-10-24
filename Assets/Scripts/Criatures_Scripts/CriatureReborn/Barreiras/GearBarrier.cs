using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitCam;

namespace Criatures2021
{
    public class GearBarrier : AnyRotateBarrier
    {
        [SerializeField] private Transform[] parteQueGiraHorario;
        [SerializeField] private Transform[] parteQueGiraAntiHorario;


        protected override void EfetivadorDaAcao()
        {
            CameraApplicator.cam.NewFocusForBasicCam(FocoDaCam, 10, 10, true, UsarForwardDoObjeto);
        }

        protected override void EffectiveRotate(float vel)
        {
            for (int i = 0; i < parteQueGiraAntiHorario.Length; i++)
                parteQueGiraAntiHorario[i].Rotate(parteQueGiraAntiHorario[i].forward, -vel * Time.deltaTime, Space.World);
            for (int i = 0; i < parteQueGiraHorario.Length; i++)
                parteQueGiraHorario[i].Rotate(parteQueGiraHorario[i].forward, vel * Time.deltaTime, Space.World);
        }
    }
}