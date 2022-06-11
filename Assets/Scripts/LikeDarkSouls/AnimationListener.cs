using UnityEngine;
using FayvitCam;
using FayvitMove;
using FayvitMessageAgregator;

namespace FayvitLikeDarkSouls
{
    public class AnimationListener : MonoBehaviour
    {
        [SerializeField] private Animator A;
        [SerializeField] private CharacterIkSupport ikSupport;


        // Start is called before the first frame update
        void Start()
        {
            if (A == null)
                A = GetComponent<Animator>();

            ikSupport.SetAnimator(A);

            MessageAgregator<ChangeMoveSpeedMessage>.AddListener(OnChangeMoveSpeed);
            MessageAgregator<AnimateStartJumpMessage>.AddListener(OnRequestAnimateJump);
            MessageAgregator<AnimateDownJumpMessage>.AddListener(OnFinishJump);
            MessageAgregator<MsgStartRoll>.AddListener(OnStartRoll);
            MessageAgregator<MsgEndRoll>.AddListener(OnEndRoll);
            MessageAgregator<MsgPosRollAtk>.AddListener(OnRequestPosRollAttack);
            MessageAgregator<MsgAtkTrigger>.AddListener(OnRequestAtkAnimate);
            MessageAgregator<MsgDamageAnimate>.AddListener(OnRequestDamageAnimate);
            MessageAgregator<MsgDeathAnimate>.AddListener(OnRequestDeathAnimate);
            MessageAgregator<AnimateFallMessage>.AddListener(OnRequestAnimateFallJump);
            MessageAgregator<MsgEndAtk>.AddListener(OnRequestEndAttack);
            MessageAgregator<MsgEnterInBlock>.AddListener(OnEnterInBlockState);
            MessageAgregator<MsgExitInBlock>.AddListener(OnExitInBlockState);
            MessageAgregator<MsgBlockHit>.AddListener(OnBlockHit);
        }

        private void OnDestroy()
        {
            MessageAgregator<ChangeMoveSpeedMessage>.RemoveListener(OnChangeMoveSpeed);
            MessageAgregator<AnimateStartJumpMessage>.RemoveListener(OnRequestAnimateJump);
            MessageAgregator<AnimateDownJumpMessage>.RemoveListener(OnFinishJump);
            MessageAgregator<MsgStartRoll>.RemoveListener(OnStartRoll);
            MessageAgregator<MsgEndRoll>.RemoveListener(OnEndRoll);
            MessageAgregator<MsgPosRollAtk>.RemoveListener(OnRequestPosRollAttack);
            MessageAgregator<MsgAtkTrigger>.RemoveListener(OnRequestAtkAnimate);
            MessageAgregator<MsgDamageAnimate>.RemoveListener(OnRequestDamageAnimate);
            MessageAgregator<MsgDeathAnimate>.RemoveListener(OnRequestDeathAnimate);
            MessageAgregator<MsgEndAtk>.RemoveListener(OnRequestEndAttack);
            MessageAgregator<MsgEnterInBlock>.RemoveListener(OnEnterInBlockState);
            MessageAgregator<MsgExitInBlock>.RemoveListener(OnExitInBlockState);
            MessageAgregator<MsgBlockHit>.RemoveListener(OnBlockHit);
            MessageAgregator<AnimateFallMessage>.RemoveListener(OnRequestAnimateFallJump);

        }

        private void OnRequestAnimateFallJump(AnimateFallMessage obj)
        {
            OnRequestAnimateJump(new AnimateStartJumpMessage() { gameObject = obj.gameObject });
        }

        void OnBlockHit(MsgBlockHit obj)
        {
            if (obj.gameObject == gameObject)
            {
                int x = UnityEngine.Random.Range(1, 3);
                string animationLabel = "GetBlockHit_" + x;
                A.Play(animationLabel);
                A.Play(animationLabel, 1);

            }
        }

        void OnExitInBlockState(MsgExitInBlock obj)
        {
            if (obj.gameObject == gameObject)
                A.SetBool("Block", false);
        }

        void OnEnterInBlockState(MsgEnterInBlock obj)
        {
            if (obj.gameObject == gameObject)
                A.SetBool("Block", true);
        }

        private void OnRequestEndAttack(MsgEndAtk obj)
        {
            if (obj.sender == gameObject)
            {
                A.SetTrigger("endAttack");
            }
        }

        private void OnRequestDeathAnimate(MsgDeathAnimate obj)
        {
            if (obj.gameObject == gameObject)
            {
                A.Play("Death");
            }
        }

        private void OnRequestDamageAnimate(MsgDamageAnimate obj)
        {
            if (obj.G == gameObject)
            {
                A.Play(obj.animKey);
            }
        }

        private void OnRequestAtkAnimate(MsgAtkTrigger obj)
        {
            if (obj.dono== gameObject)
            {
                int step = obj.atkSteep;

                if (step > 3)
                    step = step % 2 + 2;
                switch (step)
                {
                    case 1:
                        A.Play("Espadada");
                        break;
                    case 2:
                        //A.Play("RetornoDeEspadada");
                        A.SetTrigger("newAttack");
                        break;
                    case 3:
                        //A.Play("AcaoDeEspadada");
                        A.SetTrigger("newAttack");
                        break;
                    case -1:
                        A.Play("espadadaCorrendo");
                        break;
                }
            }
        }

        private void OnRequestPosRollAttack(MsgPosRollAtk obj)
        {
            if (obj.sender == gameObject)
            {
                A.Play("posRollAttack");
            }
        }

        private void OnEndRoll(MsgEndRoll obj)
        {
            if (obj.gameObject == gameObject)
            {
                //A.SetFloat("Vel", 0);
                //A.SetFloat("Vert", 0);
                //A.SetFloat("Horiz", 0);

            }
        }

        private void OnStartRoll(MsgStartRoll obj)
        {
            if (obj.gameObject == gameObject)
            {
                Vector3 dir = obj.startDir;
                A.SetFloat("Vert", dir.z);
                A.SetFloat("Horiz", dir.x);
                A.Play("Roll");
            }
        }

        private void OnFinishJump(AnimateDownJumpMessage obj)
        {
            if (obj.gameObject == gameObject)
            {
                A.SetBool("grounded", true);
            }
        }

        private void OnRequestAnimateJump(AnimateStartJumpMessage obj)
        {
            if (obj.gameObject == gameObject)
            {
                A.SetBool("grounded", false);
                A.Play("Jump");
            }
        }

        private void OnChangeMoveSpeed(ChangeMoveSpeedMessage e)
        {
            if (e.gameObject == gameObject)
            {
                var V = e.velocity;

                if (e.lockTarget)
                {

                    #region testes
                    //var x = Vector3.ProjectOnPlane(V, r).magnitude * Mathf.Sign(Vector3.Dot(V, r));
                    // var z = Vector3.ProjectOnPlane(V, f).magnitude * Mathf.Sign(Vector3.Dot(V, f));
                    //x = Mathf.Abs(Vector3.Dot(V, r)) < .1f ? 0 : x;
                    //z = Mathf.Abs(Vector3.Dot(V, f)) < .1f  ? 0 : z;

                    //Debug.Log(V.x + " : " + x + " : " + V.z + " : " + z);

                    //Debug.Log("com forward: " + Mathf.Sign(Vector3.Dot(V, f)) + " : " + Vector3.Angle(V, f) + " : " + Vector3.Dot(V, f));
                    //Debug.Log("com right: " + Mathf.Sign(Vector3.Dot(V, r)) + " : " + Vector3.Angle(V, r) + " : " + Vector3.Dot(V, r));
                    #endregion

                    var f = CameraApplicator.cam.transform.TransformDirection(Vector3.forward);
                    var r = new Vector3(f.z, 0, -f.x);

                    var z = Vector3.ProjectOnPlane(V, r).magnitude * Mathf.Sign(Vector3.Dot(V, f));
                    var x = Vector3.ProjectOnPlane(V, f).magnitude * Mathf.Sign(Vector3.Dot(V, r));


                    A.SetFloat("Vert", z);
                    A.SetFloat("Horiz", x);
                    A.SetFloat("Vel", V.magnitude);
                }
                else
                {
                    V = new Vector3(V.x, 0, V.z);
                    A.SetFloat("Vert", V.magnitude);
                    A.SetFloat("Vel", A.GetFloat("Vert"));
                    A.SetFloat("Horiz", 0);
                }
            }
        }

        #region ikSuprimido
        //[Range(0,10),SerializeField] private float distanceRay = 3;
        //[Range(0, 1), SerializeField] private float maxDeslBone = .8f;
        //[Range(0, 2), SerializeField] private float varOriginRay = 1;
        //[Range(0, 3), SerializeField] private float varDirRay = 1.15f;
        //[Range(0, 1), SerializeField] private float varFootGround = 0.2f;
        //[SerializeField] private float varTransformPosition = 0;
        //[SerializeField] private float minRadiusToIk = 0.22f;
        //[SerializeField] private LayerMask lMask;

        //void SetFootIk(AvatarIKGoal foot)
        //{
        //    if(!Physics.Raycast(A.GetIKPosition(foot), Vector3.down,minRadiusToIk))

        //    {
        //        RaycastHit hit;
        //        Vector3 startPos = A.GetIKPosition(foot) + varOriginRay * Vector3.up;
        //        Vector3 varDir = transform.position + varTransformPosition * Vector3.up - startPos;//transform.right * (foot == AvatarIKGoal.RightFoot ? -1 : 1);
        //        varDir = varDir.normalized;
        //        Ray r = new Ray(startPos, Vector3.down + varDirRay * varDir);

        //        if (Physics.Raycast(r, out hit, distanceRay, lMask))
        //        {
        //            if (hit.transform.tag == "Chao")
        //            {

        //                Vector3 pos = hit.point;
        //                pos.y += varFootGround;

        //                if (Vector3.Distance(pos, A.GetIKPosition(foot)) < maxDeslBone)
        //                {
        //                    A.SetIKPosition(foot, pos);

        //                    A.SetIKRotation(foot, Quaternion.LookRotation(A.GetIKRotation(foot) * Vector3.forward, hit.normal));
        //                }

        //            }
        //        }
        //    }
        //}
        #endregion

        private void OnAnimatorIK(int layerIndex)
        {
            //if (A.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            if (ikSupport.VerifyIkAnimationApplyable())
            {
                A.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
                A.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
                A.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
                A.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);

                ikSupport.SetFootIk(AvatarIKGoal.RightFoot);
                ikSupport.SetFootIk(AvatarIKGoal.LeftFoot);
            }

        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}