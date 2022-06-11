using UnityEngine;
using System.Collections;
using FayvitCam;

public static class SetHeroCamera
{
    public static void Set(Transform transform)
    {
        CameraApplicator.cam.FocusForDirectionalCam(transform, .1f, 3,.7f);
    }
}
