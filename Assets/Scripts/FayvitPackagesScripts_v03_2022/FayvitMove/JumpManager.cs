using UnityEngine;
using FayvitMessageAgregator;

namespace FayvitMove
{
    [System.Serializable]
    public class JumpManager
    {
        private JumpFeatures features;
        private Transform transform;
        private CharacterController controle;
        private Vector3 verticalMove = Vector3.zero;
        private float lastGroundedY = 0;
        private float timeInJump = 0;
        private float timeOfRising = 0;
        private bool isRising = false;


        public JumpManager(JumpFeatures caracteristicas,Transform T,CharacterController c )
        {
            this.features = caracteristicas;
            transform = T;
            controle = c;
        }

        public bool isJumping
        {
            get { return features.isJumping; }
        }

        public void StartFall()
        {
            features.isJumping = true;
            features.wasJumping = true;
        }

        public void StartApplyJump()
        {
            lastGroundedY = transform.position.y;
            features.isJumping = true;
            controle.Move(Vector3.up * features.initialImpulse);

            MessageAgregator<AnimateStartJumpMessage>.Publish(
                new AnimateStartJumpMessage(){ gameObject = controle.gameObject });

            //EventAgregator.Publish(new GameEvent(EventKey.animateStartJump,controle.gameObject));
        }

        public void UpdateJump(Vector3 moveDirection, bool isGrounded, bool jump)
        {

            VerifyIsWasJump();

            if (
                isRising == true
                &&
                transform.position.y - lastGroundedY < features.jumpHeight
                &&
                timeInJump < features.maxTimeJump
                &&
                jump
                )
            {

                RisingJump(moveDirection);

            }
            else if (
              (transform.position.y - lastGroundedY >= features.jumpHeight
                ||
                timeInJump >= features.maxTimeJump
                || !jump
                )
              &&
              isRising == true
              )
            {
                KeyOfJumpTransition();
            }
            else if (isRising == false)
            {

                FallingJump(moveDirection, isGrounded);

            }
        }

        void VerifyIsWasJump()
        {
            if (features.wasJumping == false && features.isJumping == true)
            {
                timeInJump = 0;
                isRising = true;
            }

            features.wasJumping = features.isJumping;
        }

        void RisingJump(Vector3 direcaoMovimento)
        {
            timeInJump += Time.deltaTime;
            
            verticalMove = (direcaoMovimento * features.inJumpSpeed
                + Vector3.up * features.risingSpeed);
            controle.Move(verticalMove * Time.deltaTime);
        }

        void KeyOfJumpTransition()
        {
            timeOfRising = timeInJump;
            isRising = false;
            controle.Move(verticalMove * Time.deltaTime);
        }

        void FallingJump(Vector3 direcaoMovimento,bool noChao)
        {
            timeOfRising += Time.deltaTime;
            float amortecimento = features.verticalDamping;

            verticalMove = FallingHorizontalMove(direcaoMovimento)
                + FallingVerticalMove(amortecimento);


            controle.Move(verticalMove * Time.deltaTime);

            if (noChao && timeOfRising > features.minTimeJump)
                NotJumping();
            else if (!noChao && timeOfRising > features.minTimeJump)
            {
                RaycastHit hit;

                //if(Physics.SphereCast(transform.position+.5f*controle.height*Vector3.up,1.1f*controle.radius,transform.forward,out hit,.1f)
                //    &&
                //    hit.collider.gameObject.CompareTag("cenario")
                //    )
                if (Physics.Raycast(transform.position, transform.forward, out hit, 1.2f * controle.radius)
                    ||
                    Physics.Raycast(transform.position, transform.right, out hit, 1.2f * controle.radius)
                    ||
                    Physics.Raycast(transform.position, -transform.forward, out hit, 1.2f * controle.radius)
                    ||
                    Physics.Raycast(transform.position, -transform.right, out hit, 1.2f * controle.radius)
                    )
                {
                    Debug.Log("pegou um cenario");
                    verticalMove = FallingHorizontalMove(1.5f*features.inJumpSpeed* hit.normal)
                        + FallingVerticalMove(amortecimento);
                }
            }
        }

        Vector3 FallingVerticalMove(float damping)
        { 
        return new Vector3(0,
                        Mathf.Lerp(verticalMove.y, -features.fallSpeed, damping * Time.deltaTime),
                    0);
        }

        Vector3 FallingHorizontalMove(Vector3 moveDirection)
        {
            Vector3 V = new Vector3(verticalMove.x, 0, verticalMove.z);
            Vector3 V2 = new Vector3(moveDirection.x, 0, moveDirection.z) * features.inJumpSpeed;
            return Vector3.Lerp(V, V2, features.horizontalDamping * Time.deltaTime);
        }

        public void NotJumping()
        {

            if (features.isJumping)
            {
                MessageAgregator<AnimateDownJumpMessage>.Publish(
                    new AnimateDownJumpMessage() { gameObject = controle.gameObject });

                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit))
                {

                    float angle = Vector3.Angle(hit.normal, Vector3.up);
                    if (angle>65 && angle<85)
                    {

                        MessageAgregator<MsgSlopeSlip>.Publish(new MsgSlopeSlip()
                        {
                            slipped = transform.gameObject,
                            hit=hit,
                            angle = angle
                        });

                    }
                }
            }
                //EventAgregator.Publish(new GameEvent(EventKey.animateDownJump,controle.gameObject));

            features.isJumping = false;
            features.wasJumping = false;
            
            verticalMove = Vector3.zero;
        }
    }

    public struct MsgSlopeSlip : IMessageBase
    {
        public GameObject slipped;
        public RaycastHit hit;
        public float angle;
    }
}