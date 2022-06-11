using FayvitBasicTools;
using FayvitMessageAgregator;
using FayvitUI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GlobalColorMenu
{
    [SerializeField] private A_GlobalColorOption[] opcoes;
    [SerializeField] private Image receivedColor;
    [SerializeField] protected Color selectedColor = Color.gray;
    [SerializeField] protected Color standardColor = Color.white;

    public int SelectedOption { get; private set; } = 0;

    public Color RememberedColor { get => receivedColor.color; }
    public Color ChangedColor { get; private set; }

    public Color GetSloteColor(RegistroDeCores v)
    {
        return opcoes[(int)v - 2].transform.GetChild(1).GetChild(0).GetComponentInChildren<Image>().color;
    }

    public void StartHud(Color receivedColor,Color regColor,Dictionary<RegistroDeCores,Color> dicReg,RegistroDeCores registrado)
    {
        this.receivedColor.color = receivedColor;
        opcoes[0].transform.parent.parent.gameObject.SetActive(true);
        ChangeSelectionTo(4);
        
        foreach (var v in dicReg.Keys)
        {
            Color c;
            if (v != registrado)
                c = dicReg[v];
            else
                c = regColor;

            opcoes[(int)v - 2].transform.GetChild(1).GetChild(0).GetComponentInChildren<Image>().color = c;
        }

        
        for (int i = 4; i < 8; i++)
        {
            bool foi = false;
            if (registrado == (RegistroDeCores)(i - 2))
            {
                foi = true;
                ChangedColor = GetSloteColor(registrado);
            }
            opcoes[i].transform.GetChild(0).GetComponent<Toggle>().isOn = foi;
        }

        MessageAgregator<MsgToggleForOthersToggles>.AddListener(OnToggleOn);

    }

    public void SetGlobalViewInTheIndex(RegistroDeCores v)
    {
        opcoes[(int)v - 2].transform.GetChild(1).GetChild(0).GetComponentInChildren<Image>().color = RememberedColor;

        if (opcoes[(int)v + 2].transform.GetChild(0).GetComponent<Toggle>().isOn)
            ChangedColor = RememberedColor;
    }

    public void FinishHud()
    {
        opcoes[0].transform.parent.parent.gameObject.SetActive(false);
        MessageAgregator<MsgToggleForOthersToggles>.RemoveListener(OnToggleOn);
    }

    private void OnToggleOn(MsgToggleForOthersToggles obj)
    {
        ChangedColor = GetSloteColor(obj.reg);
    }

    public void InvokeSelectedAction()
    {
        opcoes[SelectedOption].InvokeAction();
    }

    public void ChangeOption(int x,int y)
    {
        if (x != 0 || y != 0)
        {
            SelectedOption = ContadorCiclico.ContarComResto(x * 4 + y, SelectedOption, 12);

            ChangeSelectionTo(SelectedOption);
        }
    }

    public virtual void RemoveHighlightFromSelected(AnOption uma)
    {
        uma.SpriteDoItem.color = standardColor;   
    }

    public virtual void HighlightSelected(AnOption uma)
    {
        uma.SpriteDoItem.color = selectedColor;
    }

    public void SelectiAnOption(int qual)
    {
        if (opcoes.Length > qual)
        {
            SelectedOption = qual;
            AnOption uma = opcoes[qual].GetComponent<AnOption>();
            HighlightSelected(uma);
        }
    }

    public void ChangeSelectionTo(int qual)
    {
        AnOption[] umaS = opcoes;
        RemoveAllHighlights(umaS);
        SelectiAnOption(qual);
        opcoes[SelectedOption].PublishChange();
    }

    public void RemoveAllHighlights(AnOption[] umaS)
    {
        for (int i = 0; i < umaS.Length; i++)
        {
            RemoveHighlightFromSelected(umaS[i]);
        }
    }
}
