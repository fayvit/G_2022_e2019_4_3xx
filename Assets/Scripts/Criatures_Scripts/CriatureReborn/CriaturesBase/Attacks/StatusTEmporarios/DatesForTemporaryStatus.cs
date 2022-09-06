using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class DatesForTemporaryStatus
    {
        [SerializeField] private float quantificador = 1;
        [SerializeField] private float tempoSignificativo = 50;
        [SerializeField] private StatusType tipo = StatusType.nulo;

        public float Quantificador
        {
            get { return quantificador; }
            set { quantificador = value; }
        }

        public StatusType Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        public float TempoSignificativo
        {
            get { return tempoSignificativo; }
            set { tempoSignificativo = value; }
        }

        public string GetNomeEmLinguas { get => StatusTemporarioBase.NomeEmLinguas(Tipo); }
    }

    public enum StatusType
    {
        todos = -2,
        nulo = -1,
        envenenado,
        fraco,
        amedrontado
    }
}