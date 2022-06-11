using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;
using FayvitMove;
using System;

namespace Criatures2021
{
    public class PetAnimationListener : MonoBehaviour
    {
        [SerializeField] private string velAnimatorString = "velocidade";
        [SerializeField] private string jumpNameBool = "pulo";
        [SerializeField] private string groundedNameBool = "noChao";
        [SerializeField] private string jumpAnimationStateName = "pulando";
        [SerializeField] private string atkBool = "atacando";
        [SerializeField] private string damageStateName = "dano1";
        [SerializeField] private string emDanoBool = "dano1";
        [SerializeField] private string emDano_2Bool = "dano2";
        [SerializeField] private string defeatedBool = "cair";
        [SerializeField] private string rollStateName = "Roll";


        [SerializeField] private Animator A;


        // Use this for initialization
        void Start()
        {
            A = GetComponent<Animator>();
            MessageAgregator<ChangeMoveSpeedMessage>.AddListener(OnChangeSpeed);
            MessageAgregator<AnimateStartJumpMessage>.AddListener(OnStartJump);
            MessageAgregator<AnimateDownJumpMessage>.AddListener(OnDownJump);
            MessageAgregator<MsgRequestAtkAnimation>.AddListener(OnStartAtk);
            MessageAgregator<MsgFreedonAfterAttack>.AddListener(OnFinishAtk);
            MessageAgregator<MsgEnterInDamageState>.AddListener(OnEnterInDamageState);
            MessageAgregator<MsgEndDamageState>.AddListener(OnEndDamageState);
            MessageAgregator<AnimateFallMessage>.AddListener(OnStartFall);
            MessageAgregator<MsgCriatureDefeated>.AddListener(OnCriatureDefeated);
            MessageAgregator<MsgStartRoll>.AddListener(OnStartRoll);
            MessageAgregator<MsgRequestDamageAnimateWithFade>.AddListener(OnRequestDamageAnimateWithFade);
            MessageAgregator<AnimateFallMessage>.AddListener(OnRequestAnimateFallJump);
            MessageAgregator<MsgRequestNonReturnableDamage>.AddListener(OnRequestNonReturnableDamage);
        }

        private void OnDestroy()
        {
            MessageAgregator<ChangeMoveSpeedMessage>.RemoveListener(OnChangeSpeed);
            MessageAgregator<AnimateStartJumpMessage>.RemoveListener(OnStartJump);
            MessageAgregator<AnimateDownJumpMessage>.RemoveListener(OnDownJump);
            MessageAgregator<MsgRequestAtkAnimation>.RemoveListener(OnStartAtk);
            MessageAgregator<MsgFreedonAfterAttack>.RemoveListener(OnFinishAtk);
            MessageAgregator<MsgEnterInDamageState>.RemoveListener(OnEnterInDamageState);
            MessageAgregator<MsgEndDamageState>.RemoveListener(OnEndDamageState);
            MessageAgregator<AnimateFallMessage>.RemoveListener(OnStartFall);
            MessageAgregator<MsgCriatureDefeated>.RemoveListener(OnCriatureDefeated);
            MessageAgregator<MsgStartRoll>.RemoveListener(OnStartRoll);
            MessageAgregator<MsgRequestDamageAnimateWithFade>.RemoveListener(OnRequestDamageAnimateWithFade);
            MessageAgregator<AnimateFallMessage>.RemoveListener(OnRequestAnimateFallJump);
            MessageAgregator<MsgRequestNonReturnableDamage>.RemoveListener(OnRequestNonReturnableDamage);
        }

        private void OnRequestNonReturnableDamage(MsgRequestNonReturnableDamage obj)
        {
            OnEnterInDamageState(new MsgEnterInDamageState()
            {
                oAtacado = obj.gameObject
            });
        }

        private void OnRequestAnimateFallJump(AnimateFallMessage obj)
        {
            OnStartJump(new AnimateStartJumpMessage() { gameObject = obj.gameObject });
        }

        private void OnRequestDamageAnimateWithFade(MsgRequestDamageAnimateWithFade obj)
        {
            if (obj.animatePet == gameObject)
            {
                A.CrossFade(damageStateName, 0);
                A.SetBool(emDanoBool, true);
                A.SetBool(emDano_2Bool, true);
            }
        }

        private void OnStartRoll(MsgStartRoll obj)
        {
            if (obj.gameObject == gameObject)
            {
                //Vector3 dir = obj.startDir;//(Vector3)obj.MySendObjects[1];
                //A.SetFloat("Vert", dir.z);
                //A.SetFloat("Horiz", dir.x);
                A.Play(rollStateName);
            }
        }

        private void OnCriatureDefeated(MsgCriatureDefeated obj)
        {
            if (obj.defeated == gameObject)
            {
                A.SetBool(emDanoBool, false);
                A.SetBool(emDano_2Bool, false);
                A.SetBool(defeatedBool,true);
            }
        }

        private void OnStartFall(AnimateFallMessage obj)
        {
            if (obj.gameObject == gameObject)
            {
                A.SetBool(jumpNameBool, true);
                A.SetBool(groundedNameBool, false);
                A.Play(jumpAnimationStateName);
            }
        }

        private void OnEndDamageState(MsgEndDamageState obj)
        {
            if (obj.gameObject == gameObject)
            {
                A.SetBool(emDanoBool, false);
                A.SetBool(emDano_2Bool, false);
            }
        }

        private void OnEnterInDamageState(MsgEnterInDamageState obj)
        {
            if (obj.oAtacado == gameObject)
            {
                bool b = UnityEngine.Random.Range(0, 2)==0 ? true : false;
                string animationName = b?emDanoBool: emDano_2Bool;
                A.Play(animationName);
                A.SetBool(emDanoBool, true);
                A.SetBool(emDano_2Bool, true);
            }
        }

        private void OnFinishAtk(MsgFreedonAfterAttack obj)
        {
            if (obj.gameObject == gameObject)
            {
                A.SetBool(atkBool, false);
            }
        }

        private void OnStartAtk(MsgRequestAtkAnimation obj)
        {
            if (obj.gameObject == gameObject)
            {
                A.SetBool(atkBool, true);
                A.Play(obj.nomeAnima);
            }
        }

        private void OnDownJump(AnimateDownJumpMessage obj)
        {
            if (obj.gameObject == gameObject)
            {
                A.SetBool(jumpNameBool, false);
                A.SetBool(groundedNameBool, true);
            }
        }

        private void OnStartJump(AnimateStartJumpMessage obj)
        {
            if (obj.gameObject == gameObject)
            {
                A.Play(jumpAnimationStateName);
                A.SetBool(jumpNameBool, true);
                A.SetBool(groundedNameBool, false);
            }
        }

        private void OnChangeSpeed(ChangeMoveSpeedMessage obj)
        {
            if (obj.gameObject == gameObject)
            {
                A.SetFloat(velAnimatorString, obj.velocity.sqrMagnitude);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}