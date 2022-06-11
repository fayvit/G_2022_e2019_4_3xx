﻿using UnityEngine;
using UnityEngine.UI;

namespace TextBankSpace
{
    public class InterfaceLanguageConverter : MonoBehaviour
    {
        private Text textoConvertivel;
        [SerializeField] private InterfaceTextKey key;

        public void MudaTexto()
        {
            if (textoConvertivel != null)
            {
                textoConvertivel.text = TextBank.RetornaTextoDeInterface(key);
            }
            else
            {
                Invoke("MudaTexto", 0.15f);
                Debug.Log("Fiz um Invoke de texto");
            }
        }

        void OnEnable()
        {
            textoConvertivel = GetComponent<Text>();
            MudaTexto();
        }
    }
}