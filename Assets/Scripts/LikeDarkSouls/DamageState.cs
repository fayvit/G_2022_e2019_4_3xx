using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FayvitLikeDarkSouls
{
    [System.Serializable]
    public class DamageState
    {
        [SerializeField] private float tempoDeDanoBase = 0.25f;
        [SerializeField] private float tempoDeAfastamentoBase = 0;
        [SerializeField] private float distanciaDeAfastamentoBase = 1;

        [Header("Interno somente para visualização")]
        [SerializeField] private float tempoNoDanoAtual;
        [SerializeField] private float tempoDeAfastamentoAtual = 0;
        [SerializeField] private float distanciaDeAfastamentoAtual = 1;
        [SerializeField] private Vector3 dirDeAfastamento;

        private float timeCount = 0;
        private CharacterController controle;
        private Vector3 startPos;

        public void StartFields(CharacterController controle)
        {
            this.controle = controle;
        }

        public void Start(
            Vector3 dirAfastamento = default,
            float tempoNoDano = -1,
            float tempoDeAfastamento = -1,
            float distanciaDeAfastamento = -1)
        {
            if (tempoNoDano < 0)
                tempoNoDano = tempoDeDanoBase;

            if (tempoDeAfastamento < 0)
                tempoDeAfastamento = tempoDeAfastamentoBase;

            if (distanciaDeAfastamento < 0)
                distanciaDeAfastamento = distanciaDeAfastamentoBase;


            startPos = controle.transform.position;
            dirDeAfastamento = dirAfastamento;
            distanciaDeAfastamentoAtual = distanciaDeAfastamento;
            tempoDeAfastamentoAtual = tempoDeAfastamento;
            tempoNoDanoAtual = tempoNoDano;
            timeCount = 0;
        }

        public bool Update()
        {
            bool retorno = false;
            timeCount += Time.deltaTime;

            if (timeCount > tempoNoDanoAtual)
                retorno = true;

            if (dirDeAfastamento != default)
            {
                Vector3 motion = Vector3.Lerp(startPos, startPos + distanciaDeAfastamentoAtual * dirDeAfastamento.normalized, timeCount / tempoDeAfastamentoAtual);
                controle.Move(motion - controle.transform.position);
            }


            return retorno;
        }
    }
}