using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class IntrinsicAttribute
    {
        [SerializeField] int corrente;
        [SerializeField] int modCorrente;
        [SerializeField] int maximo;
        [SerializeField] int modMaximo;
        [SerializeField] int minimo;
        [SerializeField] float taxa;

        public IntrinsicAttribute(int corrente, float taxa = 0.2f, int maximo = 0,
                                int minimo = 1, int modCorrente = 0, int modMaximo = 0)
        {
            this.corrente = corrente;
            this.taxa = taxa;

            if (maximo != 0)
                this.maximo = maximo;
            else
                this.maximo = corrente;

            this.minimo = minimo;
            this.modMaximo = modMaximo;
            this.modCorrente = modCorrente;
        }

        public float Taxa
        {
            get { return taxa; }
            set { taxa = value; }
        }

        public int Corrente
        {
            get { return Mathf.Min(Mathf.Max(Minimo, corrente+ModCorrente)); }
            set { corrente = Mathf.Max(0, value); }
        }

        public int ModCorrente
        {
            get { return modCorrente; }
            set { modCorrente = value; }
        }

        public int Maximo
        {
            get { return maximo; }
            set { maximo = Mathf.Max(1, value); }
        }

        public int Minimo
        {
            get { return minimo; }
            set { minimo = Mathf.Max(1, value); }
        }

        public int ModMaximo
        {
            get { return modMaximo; }
            set { modMaximo = Mathf.Max(0, value); }
        }
    }

}