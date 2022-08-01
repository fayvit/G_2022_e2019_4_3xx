using UnityEngine;
using FayvitMessageAgregator;
using System.Collections.Generic;

namespace FayvitCam
{
    public class CameraApplicator : MonoBehaviour
    {
        public static CameraApplicator cam;

        [SerializeField] private bool invetY;
        [SerializeField] private BasicCam basic;
        [SerializeField] private FightCam fightCam;
        [SerializeField] private ExhibitionistCam  cExibe;
        [SerializeField] private DirectionalCamera cDir;
        [SerializeField] private ShowSinglePointCam cShow;
        [SerializeField] private FocarAdversario focarAdv;
        [SerializeField] private float timeStandToAutoAdjustment = 1;
        [SerializeField] private bool autoInicializarCamDir;
        [SerializeField,Tooltip("A camera deve ser setada no inspector ou no script")] 
        private ShakeCam shake;
        [ Header("Target Transform"),SerializeField] private Transform target;

        private float mouseX = 0, 
                      mouseY = 0, 
                      contTimeStand = 0;
        private bool cameraFocus = false,
                     autoAdjust = false;


        public enum EstiloDeCamera
        {
            onFree,
            fight,
            showAnother,
            focusingPoint,
            basic,
            cameraOff
        }

        public BasicCam Basic
        {
            get { return basic; }
            private set { basic = value; }
        }

        public DirectionalCamera Cdir
        {
            get { return cDir; }
        }

        public ShowSinglePointCam Cshow { get => cShow; }

        public EstiloDeCamera Style { get; private set; } = EstiloDeCamera.onFree;

        // Use this for initialization
        void Start()
        {
            cam = this;
            //basic.Start(transform);
            if(autoInicializarCamDir)
                cDir.SetStartFeaturesElements(transform,target);
            MessageAgregator<RequestShakeCamMessage>.AddListener(OnRequestShakeCam);
            MessageAgregator<ControlableReachedMessage>.AddListener(OnControlableReached);
            //EventAgregator.AddListener(EventKey.requestShakeCam, OnRequestShakeCam);
            //EventAgregator.AddListener(EventKey.controlableReached,OnControlableReached );
        }

        private void OnDestroy()
        {
            MessageAgregator<RequestShakeCamMessage>.RemoveListener(OnRequestShakeCam);
            MessageAgregator<ControlableReachedMessage>.RemoveListener(OnControlableReached);
            //EventAgregator.RemoveListener(EventKey.requestShakeCam, OnRequestShakeCam);
            //EventAgregator.RemoveListener(EventKey.controlableReached, OnControlableReached);
        }

        private void OnControlableReached(ControlableReachedMessage obj)
        {
            contTimeStand = 0;
        }

        private void OnRequestShakeCam(RequestShakeCamMessage obj)
        {
            ShakeAxis ax = obj.shakeAxis;
            int totalShake = 5;
            float shakeAngle = 1;
            
            if (obj.numShake > 0)
            {
                totalShake = obj.numShake;
            }

            if (obj.shakeAngle > 0)
                shakeAngle = obj.shakeAngle;


            shake.IniciarShake(ax, totalShake, shakeAngle);
        }

        //private void OnControlableReached(IGameEvent obj)
        //{
        //    contTimeStand = 0;
        //}

        //private void OnRequestShakeCam(IGameEvent obj)
        //{
        //    ShakeAxis ax = ShakeAxis.y;
        //    int totalShake = 5;
        //    float shakeAngle = 1;

        //    if (obj.MySendObjects.Length > 0)
        //    {
        //        ax = (ShakeAxis)obj.MySendObjects[0];
        //        if (obj.MySendObjects.Length > 1)
        //        {
        //            totalShake = (int)obj.MySendObjects[1];
        //            if (obj.MySendObjects.Length > 2)
        //                shakeAngle = (float)obj.MySendObjects[2];
        //        }
        //    }

        //    shake.IniciarShake(ax, totalShake, shakeAngle);
        //}

        public void ValoresDeCamera(float mouseX, float mouseY, bool cameraFocus,bool inMove)
        {
            this.mouseX = mouseX;
            this.mouseY = mouseY * (invetY ? -1 : 1 );
            this.cameraFocus = cameraFocus;

            if (mouseX == 0 && mouseY == 0 && inMove)
                contTimeStand += Time.deltaTime;
            else 
                contTimeStand = 0;
        
            autoAdjust = contTimeStand > timeStandToAutoAdjustment && inMove;
            
        }

        // Update is called once per frame
        void LateUpdate()
        {
            //  if (!GameController.g.HudM.MenuDePause.EmPause)
            switch (Style)
            {
                case EstiloDeCamera.onFree:
                    if (cDir != null)
                        cDir.ApplyCam(mouseX,mouseY,cameraFocus, autoAdjust);//basica.Update();
                break;
                case EstiloDeCamera.fight:
                    focarAdv.Focar(transform,fightCam.T_Enemy, 0);
                    fightCam.Update();
                break;
                case EstiloDeCamera.showAnother:
                    cExibe.ShowAnother();
                break;
                case EstiloDeCamera.basic:
                    basic.Update();
                break;
            }

            if(shake!=null)
                shake.Update();
        }

        public void RemoveMira()
        {
            if(Style== EstiloDeCamera.fight)
                Style = EstiloDeCamera.cameraOff;

            focarAdv.RemoveMira();
        }

        public void RetornarParaCameraDirecional(Vector3 dirViewCam = default)
        {
                
            focarAdv.RemoveMira();
            cDir.SetPositionAndRotationToLerp(dirViewCam);
            Style = EstiloDeCamera.onFree;
        }

        public void FocusForDirectionalCam(Transform T, float height, float distance,float varHeightCamera)
        {
            //Debug.Log("varheighrcamera: "+varHeightCamera);

            target = T;
            cDir.SetFeatures(new CamFeatures()
            {
                Target = T,
                MyCamera = transform,
                targetHeightForCam = height,
                sphericalDistance = distance,
                varVerticalHeightPoint = varHeightCamera
            });
            Style = EstiloDeCamera.onFree;
        }

        public void StartExibitionCam(CharacterController target)
        {
            StartExibitionCam(target.transform,
                target.GetComponent<CharacterController>().height);
        }

        public void StartExibitionCam(Transform target, float height, bool dodgeWall = false)
        {
            cExibe.SetExhibitionElements(transform, target, height, dodgeWall);//cExibe = new ExhibitionistCam (transform, target, height, dodgeWall);
            Style = EstiloDeCamera.showAnother;
        }

        public void StartFightCam(Transform target,Transform enemy)
        {
            fightCam.Start(transform, target);
            fightCam.T_Enemy = enemy;
            Style = EstiloDeCamera.fight;
        }

        public void StartFightCam(Transform target,float height, float distance,Transform enemy)
        {
            fightCam.Start(transform, target, height, distance);
            fightCam.T_Enemy = enemy;
            Style = EstiloDeCamera.fight;
        }

        public void StartShowPointCamera(Transform target)
        {
            StartShowPointCamera(target, Cshow.Prop);
        }

        public void StartShowPointCamera(Transform target,SinglePointCameraProperties S)
        {
            Style = EstiloDeCamera.focusingPoint;
            cShow.SetExhibitionElements(transform,target,S);
        }
        public bool FocusInPoint(float horizontalDistance=6,float height=1)
        {
            return cShow.ShowFixed(horizontalDistance, height);
        }

        public void NewFocusForBasicCam(Transform target, float height, float distance, bool dodgeWall = false, bool dirOfObj = false)
        {
            Style = EstiloDeCamera.basic;
            Cdir.State = StateCam.@static;
            basic.Start(transform);
            Basic.NewFocus(target, height, distance, dodgeWall, dirOfObj);
        }

        public void OffCamera()
        {
            Style = EstiloDeCamera.cameraOff;
        }

        public Vector3 SmoothCamDirectionalVector(Vector3 inputDirection)
        {
            return SmoothCamDirectionalVector(inputDirection.x, inputDirection.z);
        }

        public Vector3 SmoothCamDirectionalVector(float h, float v)
        {
            Vector3 forward = cDir.SmoothInducedDirection(h, v);

            forward.y = 0;
            forward = forward.normalized;

            Vector3 right = new Vector3(forward.z, 0, -forward.x);

            return (h * right + v * forward);
        }

        #region Suprimido
        //public bool FocusInPoint(Transform target, 
        //    Vector3 deslCamFocus,
        //    float velOrTime,//velocidadeTempoDefoco --> bom verificar qual é o verdadeiro
        //    float distance = 6,
        //    float height = -1,
        //    bool withTime = false)
        //{
        //    return FocusInPoint(target,velOrTime, distance, height, withTime, default(Vector3), deslCamFocus, false,true);
        //}

        //public bool FocusInPoint(Transform target,
        //    float velOrTime,
        //    float distance = 6,
        //    float height = -1,
        //    bool withTime = false,
        //    Vector3 startDir = default(Vector3),
        //    Vector3 deslCamFocus = default(Vector3),
        //    bool focusOfTransform = false,
        //    bool dodgeWall = true
        //    )
        //{
        //    Style = EstiloDeCamera.focusingPoint;
        //    cShow.SetExhibitionElements(transform, target, dodgeWall);
        //    return cShow.ShowFixed(velOrTime, distance, height, withTime, startDir, focusOfTransform, deslCamFocus);
        //}

        //public bool FocusInPoint(Transform target, SinglePointCameraProperties S)
        //{
        //    return FocusInPoint(target, S.velOrTimeFocus, S.distance, S.height, S.withTime, S.startPosition, S.deslCamFocus, S.transformFocus, S.dodgeCam);
        //}
        #endregion
    }
}
