using FayvitBasicTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarMenu : MonoBehaviour
{
    [SerializeField] private Text localTxt;
    [SerializeField] private string[] options = new string[6]
        {
            "Cabelo",
            "Rosto",
            "Nariz",
            "Queixo",
            "Torso",
            "Pernas"
        };

    private int selectedOption = 0;
    private bool barActive = true;

    public void ChangeOption(int val)
    {
        if (barActive)
        {
            selectedOption = ContadorCiclico.Contar(val, selectedOption, options.Length);
            localTxt.text = options[selectedOption];
        }
    }
}
