using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitMessageAgregator;
using FayvitMove;
using FayvitBasicTools;
using Criatures2021;
using System;

public class HumanAnimationListener : MonoBehaviour
{
    [SerializeField] private string velString = "velocidade";
    [SerializeField] private string jumpAnimationName = "pulando";
    [SerializeField] private string jumpAnimationBool = "pulo";
    [SerializeField] private string groundedBool = "noChao";
    [SerializeField] private string callBool = "chama";
    [SerializeField] private string sendBool = "envia";
    [SerializeField] private string lockBool = "travar";
    [SerializeField] private string captureAnimationName = "capturou";
    [SerializeField] private string damageAnimationName = "DanoPrincipal";
    [SerializeField] private string pushAnimate = "Empurrando";
    [SerializeField] private float newRotationFactor = .001f;

    private Animator A;
    private IkManager myIk=new IkManager();

    // Start is called before the first frame update
    void Start()
    {
        A = GetComponent<Animator>();
        myIk.Start(A);
        MessageAgregator<ChangeMoveSpeedMessage>.AddListener(OnChangeMoveSpeed);
        MessageAgregator<AnimateStartJumpMessage>.AddListener(OnStartJump);
        MessageAgregator<AnimateDownJumpMessage>.AddListener(OnDownJump);
        MessageAgregator<MsgRequestCallAnimation>.AddListener(OnRequestCall);
        MessageAgregator<MsgRequestSendAnimation>.AddListener(OnRequestSend);
        MessageAgregator<MsgRequestEndArmsAnimations>.AddListener(OnRequestEndArmAnimations);
        MessageAgregator<MsgAnimaCaptura>.AddListener(OnStartCapture);
        MessageAgregator<MsgEndOfCaptureAnimate>.AddListener(OnEndCapture);
        MessageAgregator<AnimateFallMessage>.AddListener(OnRequestAnimateFallJump);
        MessageAgregator<MsgDesyncStandardAnimation>.AddListener(OnRequestDesyncStandAnimation);
        MessageAgregator<MsgRequestHumanDamage>.AddListener(OnRequestDamageAnimate);
        MessageAgregator<MsgStartPushElement>.AddListener(OnStartPushElement);
        MessageAgregator<MsgChangeToHero>.AddListener(OnChangeToHero);
    }

    private void OnDestroy()
    {
        MessageAgregator<ChangeMoveSpeedMessage>.RemoveListener(OnChangeMoveSpeed);
        MessageAgregator<AnimateStartJumpMessage>.RemoveListener(OnStartJump);
        MessageAgregator<AnimateDownJumpMessage>.RemoveListener(OnDownJump);
        MessageAgregator<MsgRequestCallAnimation>.RemoveListener(OnRequestCall);
        MessageAgregator<MsgRequestSendAnimation>.RemoveListener(OnRequestSend);
        MessageAgregator<MsgRequestEndArmsAnimations>.RemoveListener(OnRequestEndArmAnimations);
        MessageAgregator<MsgAnimaCaptura>.RemoveListener(OnStartCapture);
        MessageAgregator<MsgEndOfCaptureAnimate>.RemoveListener(OnEndCapture);
        MessageAgregator<AnimateFallMessage>.RemoveListener(OnRequestAnimateFallJump);
        MessageAgregator<MsgDesyncStandardAnimation>.RemoveListener(OnRequestDesyncStandAnimation);
        MessageAgregator<MsgRequestHumanDamage>.RemoveListener(OnRequestDamageAnimate);
        MessageAgregator<MsgStartPushElement>.RemoveListener(OnStartPushElement);
        MessageAgregator<MsgChangeToHero>.RemoveListener(OnChangeToHero);
    }

    private void OnChangeToHero(MsgChangeToHero obj)
    {
        if (obj.myHero == gameObject)
        {
            myIk.ChangeState(IkManager.IkManagerState.semIk, null);
            A.SetBool(lockBool, false);
        }
    }

    private void OnStartPushElement(MsgStartPushElement obj)
    {
        if (obj.quemEstaEmpurrando == gameObject)
        {
            A.SetBool(lockBool, true);
            A.Play(pushAnimate);
            myIk.ChangeState(IkManager.IkManagerState.maosDeEmpurrando, new Transform[2] {
                obj.ikLeftHand,
                obj.ikRightHand
            });
        }
    }

    private void OnRequestDamageAnimate(MsgRequestHumanDamage obj)
    {
        if(gameObject==obj.gameObject)
            A.Play(damageAnimationName);
    }

    private void OnRequestDesyncStandAnimation(MsgDesyncStandardAnimation obj)
    {
        if (obj.gameObject == gameObject)
        {
            //Debug.Log("Desync");
            A.StopPlayback();
            A.Play("padrao",0, UnityEngine.Random.value);   
        }
    }

    private void OnRequestAnimateFallJump(AnimateFallMessage obj)
    {
        OnStartJump(new AnimateStartJumpMessage() { gameObject = obj.gameObject });
    }

    private void OnEndCapture(MsgEndOfCaptureAnimate obj)
    {
        if (obj.dono == gameObject)
        {
            A.SetBool(lockBool, false);
        }
    }

    private void OnStartCapture(MsgAnimaCaptura obj)
    {
        if (obj.dono == gameObject)
        {
            A.SetBool(lockBool, true);
            A.SetBool(callBool, false);
            A.Play(captureAnimationName);
        }
    }

    private void OnRequestEndArmAnimations(MsgRequestEndArmsAnimations obj)
    {
        if (obj.oAnimado == gameObject)
        {
            A.SetBool(callBool, false);
            A.SetBool(sendBool, false);
        }
    }

    private void OnRequestSend(MsgRequestSendAnimation obj)
    {
        if (obj.oAnimado == gameObject)
        {
            A.SetBool(sendBool, true);
        }
    }

    private void OnRequestCall(MsgRequestCallAnimation obj)
    {
        if (obj.oAnimado == gameObject)
        {
            A.SetBool(callBool, true);
        }
    }

    private void OnDownJump(AnimateDownJumpMessage obj)
    {
        if (obj.gameObject == gameObject)
        {
            A.SetBool(jumpAnimationBool, false);
            A.SetBool(groundedBool, true);
        }
    }

    private void OnStartJump(AnimateStartJumpMessage obj)
    {
        if (obj.gameObject == gameObject)
        {
            A.Play(jumpAnimationName);
            A.SetBool(jumpAnimationBool, true);
            A.SetBool(groundedBool, false);

            MessageAgregator<MsgRequest3dSound>.Publish(new MsgRequest3dSound()
            {
                sfxId = FayvitSounds.SoundEffectID.XP_Swing03,
                sender = transform
            });
        }
    }

    //public float rotationFactor = .001f;
    

    private void OnChangeMoveSpeed(ChangeMoveSpeedMessage obj)
    {
        if (obj.gameObject == gameObject)
        {
            
            float f2 = obj.velocity.sqrMagnitude;

            /*Esse condicional foi colocado para evitar a rotação sem animação
             foi criado o rotation factor que é um dot entre os forward antes e depois de aplicado o movimento
             */
            if (f2 <= 1)
            {
                A.SetFloat("forView", 1 - obj.rotationFactor);
                if (1 - obj.rotationFactor > newRotationFactor)
                    A.SetFloat(velString, 4);
                else
                    A.SetFloat(velString, f2);
            }
            else
                A.SetFloat(velString, f2);
            
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        //A.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        //A.SetIKPosition(AvatarIKGoal.LeftHand, transform.position + Vector3.up);
        myIk.Update();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public struct MsgRequestHumanDamage : IMessageBase {
    public GameObject gameObject;
    public PetAttackBase esseGolpe;
}

public struct MsgDesyncStandardAnimation : IMessageBase
{
    public GameObject gameObject;
}