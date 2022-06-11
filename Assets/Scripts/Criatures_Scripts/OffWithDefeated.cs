using Criatures2021;
using FayvitMessageAgregator;
using UnityEngine;

public class OffWithDefeated : MonoBehaviour
{
    [SerializeField] private GameObject[] off;
    // Start is called before the first frame update
    void Start()
    {
        MessageAgregator<MsgCriatureDefeated>.AddListener(OnCriatureDefeated);
    }

    private void OnDestroy()
    {
        MessageAgregator<MsgCriatureDefeated>.RemoveListener(OnCriatureDefeated);
    }

    private void OnCriatureDefeated(MsgCriatureDefeated obj)
    {
        if (obj.defeated == gameObject)
        {
            foreach(var v in off)
                v.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
