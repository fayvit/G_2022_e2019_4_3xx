using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitMessageAgregator;
using CustomizationSpace;

public class InfoTextureManager : MonoBehaviour
{

    [SerializeField] private GameObject listOptions;
    [SerializeField] private GameObject generalOptions;
    [SerializeField] private GameObject returnOption;
 
    // Start is called before the first frame update
    void Start()
    {
        

        MessageAgregator<MsgFinishEdition>.AddListener(OnFinishEdition);
        MessageAgregator<MsgEnterInListOptions>.AddListener(OnEnterInListOptions);
        MessageAgregator<MsgFinishCharDbManager>.AddListener(OnExitInListOptions);


    }

    private void OnDestroy()
    {
        MessageAgregator<MsgFinishEdition>.RemoveListener(OnFinishEdition);
        MessageAgregator<MsgEnterInListOptions>.RemoveListener(OnEnterInListOptions);
        MessageAgregator<MsgFinishCharDbManager>.RemoveListener(OnExitInListOptions);

    }

    private void OnExitInListOptions(MsgFinishCharDbManager obj)
    {
        if (generalOptions != null)
        {
            generalOptions.SetActive(true);
            listOptions.SetActive(false);
            returnOption.SetActive(true);
        }
    }

    private void OnEnterInListOptions(MsgEnterInListOptions obj)
    {
        if (generalOptions != null)
        {
            generalOptions.SetActive(false);
            listOptions.SetActive(true);
            returnOption.SetActive(false);
        }
    }

    private void OnFinishEdition(MsgFinishEdition obj)
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
