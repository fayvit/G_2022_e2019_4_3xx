using FayvitCam;
using FayvitMessageAgregator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitMove
{
    [System.Serializable]
    public class RollManager
    {
        [SerializeField] private float totalTimeInRoll = .5f;
        [SerializeField] private float intervalInTheRoll = .3f;
        [SerializeField] private float returnTime = .15f;
        [SerializeField] private float imunoTax = .75f;

        private GameObject owner;
        private float timeCount = 0;
        private float ultimoRoll = -1;


        public bool RequestAttack { get; set; }

        // Start is called before the first frame update

        public Vector3 DirOfRoll { get; private set; }

        public bool ImunneRoll { get { return timeCount > 0 && timeCount < imunoTax * totalTimeInRoll; } }

        public bool Start(Vector3 startDir, GameObject G)
        {
            if (Time.time - ultimoRoll > intervalInTheRoll + totalTimeInRoll + returnTime || ultimoRoll < 0)
            {
                RequestAttack = false;
                owner = G;
                startDir = startDir == Vector3.zero ? CameraApplicator.cam.transform.TransformDirection(Vector3.forward) : startDir;
                DirOfRoll = startDir;
                timeCount = 0;
                ultimoRoll = Time.time;
                MessageAgregator<MsgStartRoll>.Publish(new MsgStartRoll() { gameObject = owner, startDir = startDir });
                //FayvitMoveEventAgregator.Publish(new FayvitMoveEvent(FayvitMoveEventKey.startRoll, G, startDir));

                return true;
            }
            else
                return false;
        }

        // Update is called once per frame
        public bool Update()
        {
            timeCount += Time.deltaTime;

            if (timeCount > totalTimeInRoll)
            {
                MessageAgregator<MsgEndRoll>.Publish(new MsgEndRoll() { gameObject = owner });
                //FayvitMoveEventAgregator.Publish(new FayvitMoveEvent(FayvitMoveEventKey.endRoll, owner));
                return true;
            }

            return false;
        }

        public bool ReturnTime()
        {
            timeCount += Time.deltaTime;
            MessageAgregator<MsgEndRoll>.Publish(new MsgEndRoll() { gameObject = owner });
            //FayvitMoveEventAgregator.Publish(new FayvitMoveEvent(FayvitMoveEventKey.endRoll, owner));

            if (timeCount > totalTimeInRoll + returnTime)
                return true;

            return false;
        }
    }

    public struct MsgStartRoll : IMessageBase
    {
        public GameObject gameObject;
        public Vector3 startDir;
    }

    public struct MsgEndRoll : IMessageBase
    {
        public GameObject gameObject;
    }
}