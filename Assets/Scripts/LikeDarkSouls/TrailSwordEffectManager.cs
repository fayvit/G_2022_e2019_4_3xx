using FayvitCommandReader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitMessageAgregator;

public class TrailSwordEffectManager : MonoBehaviour
{
    [SerializeField] private GameObject[] trails;
    [SerializeField] private GameObject[] transitoryTrails;
    [SerializeField] private GameObject dono;

    // Start is called before the first frame update
    void Start()
    {
        MessageAgregator<MsgAnimationPointCheck>.AddListener(OnReceivedAnimationPoint);
        
    }

    private void OnDestroy()
    {
        MessageAgregator<MsgAnimationPointCheck>.RemoveListener(OnReceivedAnimationPoint);
        
    }

    private void OnReceivedAnimationPoint(MsgAnimationPointCheck obj)
    {
        if (obj.sender == dono)
        {
            string s = obj.extraInfo;
            switch (s)
            {
                case "trailStart":
                    TrailsSetActive(true);
                break;
                case "offTrail":
                    TrailsSetActive(false);
                break;
            }
        }
    }

    void TrailsSetActive(bool b)
    {
        if (b)
        {
            transitoryTrails = new GameObject[trails.Length];
            for (int i = 0; i < trails.Length; i++)
            {
                transitoryTrails[i] = Instantiate(trails[i], trails[i].transform.position, trails[i].transform.rotation, trails[i].transform.parent);
                transitoryTrails[i].SetActive(true);
                Destroy(transitoryTrails[i], 1);
            }
        }
        else
        {
            for (int i = 0; i < transitoryTrails.Length; i++)
            {
                if (transitoryTrails[i] != null)
                {
                    transitoryTrails[i].transform.SetParent(null);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
