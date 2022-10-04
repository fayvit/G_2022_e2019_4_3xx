using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitMove;
using FayvitMessageAgregator;

public class CharRotateTo : MonoBehaviour
{
    private Quaternion dir;
    private Quaternion startRot;
    
    private float time = .35f;
    private float tempoDeCorrido = 0;
    public static void RotateDir(Vector3 dir,GameObject target)
    {
        CharRotateTo c = target.AddComponent<CharRotateTo>();
        c.dir = Quaternion.LookRotation(Vector3.ProjectOnPlane(dir,Vector3.up));
        c.time = Random.Range(.35f, .65f);
    }

    // Start is called before the first frame update
    void Start()
    {
        startRot = transform.rotation;
        MessageAgregator<MsgStartAnimateArmToFight>.AddListener(OnStartAnimateFight);
    }

    private void OnDestroy()
    {
        MessageAgregator<MsgStartAnimateArmToFight>.RemoveListener(OnStartAnimateFight);
    }

    private void OnStartAnimateFight(MsgStartAnimateArmToFight obj)
    {
        if (obj.sender == gameObject)
        {
            MessageAgregator<ChangeMoveSpeedMessage>.Publish(new ChangeMoveSpeedMessage()
            {
                velocity = Vector3.zero,
                rotationFactor=1,
                gameObject = gameObject
            });

            tempoDeCorrido = time + 1;

            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tempoDeCorrido > time)
        {
            MessageAgregator<ChangeMoveSpeedMessage>.Publish(new ChangeMoveSpeedMessage()
            {
                velocity=Vector3.zero,
                rotationFactor=1,
                gameObject = gameObject
            });
            Destroy(this, 1);
        }
        else
        {
            Vector3 startForward = transform.forward;
            tempoDeCorrido += Time.deltaTime;

            transform.rotation = Quaternion.Lerp(startRot, dir, tempoDeCorrido / time);
            MessageAgregator<ChangeMoveSpeedMessage>.Publish(new ChangeMoveSpeedMessage()
            {
                rotationFactor = Vector3.Dot(startForward, transform.forward),
                gameObject = gameObject
            });
        }

        
        
    }
}
