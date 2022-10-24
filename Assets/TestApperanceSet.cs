using FayvitMessageAgregator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Criatures2021;
using CustomizationSpace;

public class TestApperanceSet : MonoBehaviour
{
    [SerializeField] private SectionCustomizationManager secManagerH;
    [SerializeField] private SectionCustomizationManager secManagerM;

    // Start is called before the first frame update
    void Start()
    {
        CharacterManager C = FindObjectOfType<CharacterManager>();

        if (C)
        {
            if (C.Ccd.PersBase == PersonagemBase.masculino)
            {
                secManagerH.SetCustomDates(C.Ccd);
                secManagerM.gameObject.SetActive(false);
            }
            else
            {
                secManagerM.SetCustomDates(C.Ccd);
                secManagerH.gameObject.SetActive(false);
            }
        }
        else
            MessageAgregator<MsgApperanceTransport>.AddListener(OnApperanceReceived);
    }

    private void OnDestroy()
    {
        MessageAgregator<MsgApperanceTransport>.RemoveListener(OnApperanceReceived);
    }

    private void OnApperanceReceived(MsgApperanceTransport obj)
    {
        CustomizationContainerDates ccd = obj.lccd[Random.Range(0, obj.lccd.Count)];
        if (ccd.PersBase == PersonagemBase.masculino)
        {
            secManagerH.SetCustomDates(ccd);
            secManagerM.gameObject.SetActive(false);
        }
        else
        {
            secManagerM.SetCustomDates(ccd);
            secManagerH.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
