using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Criatures2021;
using FayvitMessageAgregator;

public class MyOtimization : MonoBehaviour
{
    [SerializeField] private Cloth[] cloths;
    [SerializeField] private Animator[] animators;
    [SerializeField] private List<MonoBehaviour> behaviours=new List<MonoBehaviour>();
    [SerializeField] private float distanceCloth = 20;
    [SerializeField] private float distanceAnimators = 100;
    [SerializeField] private float distanceBehaviours = 250;
    [SerializeField] private bool debug;

    private static Transform target;

    public float DistanceBehaviours
    {
        get => distanceBehaviours;
        set => distanceBehaviours = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        cloths = gameObject.GetComponentsInChildren<Cloth>();
        //animators = new Animator[0];
        animators = gameObject.GetComponentsInChildren<Animator>();
        behaviours.AddRange(gameObject.GetComponentsInChildren<MonoBehaviour>());

        foreach (var v in behaviours.ToList())
            if (v.enabled == false)
                behaviours.Remove(v);

        float x = Random.Range(1f, 3f);
        InvokeRepeating("VerifyOnOffCloth", 1, x);

        MessageAgregator<MsgChangeToHero>.AddListener(OnChangeToHero);
        MessageAgregator<MsgChangeToPet>.AddListener(OnChangeToPet);
        
        
    }



    private void OnDestroy()
    {

        MessageAgregator<MsgChangeToHero>.RemoveListener(OnChangeToHero);
        MessageAgregator<MsgChangeToPet>.RemoveListener(OnChangeToPet);

        CancelInvoke();
    }

    private void OnChangeToPet(MsgChangeToPet obj)
    {
        target = FindByOwner.GetHeroActivePet(MyGlobalController.MainCharTransform.gameObject).transform;
        VerifyOnOffCloth();
    }

    private void OnChangeToHero(MsgChangeToHero obj)
    {
        target = MyGlobalController.MainCharTransform;
        VerifyOnOffCloth();
    }

    void VerifyOnOffCloth()
    {
        

        if (debug)
            Debug.Log("A distancia é: " + Vector3.Distance(target.position, transform.position));

        if (target == null)
            target = MyGlobalController.MainCharTransform;

        if (target!=null)
        {
            float d = Vector3.Distance(target.position, transform.position);
            if ( d< distanceCloth)
            {
                foreach (var v in cloths)
                    v.enabled = true;
            }
            else
            {
                foreach (var v in cloths)
                    v.enabled = false;
            }

            if (d < distanceBehaviours)
            {
                foreach (var v in behaviours)
                    if(v!=null)
                        v.enabled = true;
            }
            else
            {
                foreach (var v in behaviours)
                    if(v!=null)
                    v.enabled = false;
            }

            if (d < distanceAnimators)
            {
                foreach (var v in animators)
                    v.StopPlayback();
            }
            else
            {
                foreach (var v in animators)
                    v.StartPlayback();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
