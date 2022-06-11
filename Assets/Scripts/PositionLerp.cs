using UnityEngine;
using System.Collections;
using FayvitUI;
using FayvitMessageAgregator;
using System;

public class PositionLerp : MonoBehaviour
{
    private Transform findPosition;
    private Vector3 postionVar;
    //private float lerpTime = 1;
    //private bool active = false;
    //private float tempoDecorrido = 0;
    //private Vector3 startPosition = default;
    // Use this for initialization
    void Start()
    {
        //startPosition = transform.position;
        //MessageAgregator<MsgChangeOptionUI>.AddListener(OnChangeUIOption);
    }

    private void OnDestroy()
    {
        //MessageAgregator<MsgChangeOptionUI>.RemoveListener(OnChangeUIOption);
    }

    //private void OnChangeUIOption(MsgChangeOptionUI obj)
    //{
    //    StartRePos();
    //}

    public void StartRePos(Transform targetPos=null,Vector3 varPos=default,float lerpTime=1)
    {
        //startPosition = transform.position;
        findPosition= targetPos==null?findPosition:targetPos;
        postionVar = varPos == default ? postionVar : varPos;
        //this.lerpTime = lerpTime==1?this.lerpTime:lerpTime;
        //tempoDecorrido = 0;
        //active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(findPosition)
            transform.position = findPosition.position + postionVar;
        //if (active)
        //{
        //    tempoDecorrido += Time.deltaTime;
        //    transform.position = Vector3.Lerp(startPosition, findPosition.position + postionVar, tempoDecorrido / lerpTime);
        //    if (tempoDecorrido > lerpTime)
        //        active = false;
        //}
    }
}
