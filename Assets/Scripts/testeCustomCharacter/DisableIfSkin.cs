using FayvitMessageAgregator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIfSkin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MessageAgregator<MsgOpenColorMenu>.AddListener(OnOpenColorMenu);
    }

    private void OnDestroy()
    {
        MessageAgregator<MsgOpenColorMenu>.RemoveListener(OnOpenColorMenu);
    }

    private void OnOpenColorMenu(MsgOpenColorMenu obj)
    {
        if (obj.reg == RegistroDeCores.skin)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public struct MsgOpenColorMenu : IMessageBase
{
    public RegistroDeCores reg;
}
