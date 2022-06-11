using FayvitMessageAgregator;
using FayvitUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class A_GlobalColorOption : AnOption
{
    [SerializeField] private OptionType optType;
    [SerializeField] private RegistroDeCores localIndex = RegistroDeCores.slote1;

    private enum OptionType
    { 
        toggle,
        globalColor,
        button
    }

    private void Start()
    {
        ThisAction += LocalAction;
        MessageAgregator<MsgToggleForOthersToggles>.AddListener(OnChangeToggle);
    }

    private void OnDestroy()
    {
        MessageAgregator<MsgToggleForOthersToggles>.RemoveListener(OnChangeToggle);
    }

    private void OnChangeToggle(MsgToggleForOthersToggles obj)
    {
        Toggle T = GetComponentInChildren<Toggle>();
        if (T != null && T.gameObject!=obj.thisToggle)
        {
            T.isOn = false;
        }
    }

    private void LocalAction(int x)
    {
        Debug.Log(optType + " : " + localIndex);

        switch (optType)
        {
            case OptionType.button:
                MessageAgregator<ButtonMakeGlobalMessage>.Publish(new ButtonMakeGlobalMessage() { indexOfGlobal = localIndex });
            break;
            case OptionType.globalColor:
                MessageAgregator<SelectGlobalColorMessage>.Publish(new SelectGlobalColorMessage() { indexOfGlobal = localIndex });
            break;
            case OptionType.toggle:
                Toggle T = GetComponentInChildren<Toggle>();
                T.isOn = !T.isOn;

                //if (T.isOn)
                //{
                //    MessageAgregator<MsgToggleForOthersToggles>.Publish(
                //        new MsgToggleForOthersToggles() { 
                //            thisToggle = T.gameObject,
                //            reg= localIndex
                //            });
                //}
            
                //MessageAgregator<ToggleGlobalColorMessage>.Publish(new ToggleGlobalColorMessage() { indexOfGlobal = localIndex });
            break;
        }
    }

    public void PublishChange()
    {
        Debug.Log("Publish?");

        bool b = optType == OptionType.toggle ? GetComponentInChildren<Toggle>().isOn : false;
        MessageAgregator<ChangeInGlobalColorMessage>.Publish(new ChangeInGlobalColorMessage()
        {
            optionType=(int)optType,
            registro = localIndex,
            check = b
        });
    }


    public void ToggleFunction(bool b)
    {
        Toggle T = GetComponentInChildren<Toggle>();
        Debug.Log("Toggle function: "+b+" : "+T.isOn);
        MessageAgregator<ToggleGlobalColorMessage>.Publish(new ToggleGlobalColorMessage() { indexOfGlobal = localIndex });
        

        if (T.isOn)
        {
            MessageAgregator<MsgToggleForOthersToggles>.Publish(
                new MsgToggleForOthersToggles()
                {
                    thisToggle = T.gameObject,
                    reg = localIndex
                });
        }

        MessageAgregator<ChangeInGlobalColorMessage>.Publish(new ChangeInGlobalColorMessage()
        {
            optionType = (int)optType,
            registro = localIndex,
            check = T.isOn
        });
    }
}

public struct MsgToggleForOthersToggles : IMessageBase
{
    public GameObject thisToggle;
    public RegistroDeCores reg;
}



public struct ChangeInGlobalColorMessage : IMessageBase
{
    public int optionType;
    public bool check;
    public RegistroDeCores registro;
}

public struct ButtonMakeGlobalMessage : IMessageBase
{
    public RegistroDeCores indexOfGlobal;
}

public struct SelectGlobalColorMessage : IMessageBase
{
    public RegistroDeCores indexOfGlobal;
}

public struct ToggleGlobalColorMessage : IMessageBase
{
    public RegistroDeCores indexOfGlobal;
}

