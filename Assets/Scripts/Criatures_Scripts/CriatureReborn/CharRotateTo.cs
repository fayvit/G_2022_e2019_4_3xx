using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitMove;

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
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 startForward = transform.forward;
        tempoDeCorrido += Time.deltaTime;
        
        transform.rotation = Quaternion.Lerp(startRot, dir, tempoDeCorrido / time);
        FayvitMessageAgregator.MessageAgregator<ChangeMoveSpeedMessage>.Publish(new ChangeMoveSpeedMessage() {
            rotationFactor = Vector3.Dot(startForward, transform.forward),
            gameObject=gameObject
        });

        if (tempoDeCorrido > time)
            Destroy(this,1);
        
    }
}
