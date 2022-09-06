using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class ConsumableAttribute
    {
        [SerializeField] int basico;
        [SerializeField] int corrente;
        [SerializeField] int maximo;
        [SerializeField] int modMaximo;
        [SerializeField] float taxa;

        public ConsumableAttribute(int corrente, float taxa = 0.2f, int maximo = 0, int modMaximo = 0)
        {
            this.corrente = corrente;
            this.taxa = taxa;

            if (maximo != 0)
                this.maximo = maximo;
            else
                this.maximo = corrente;

            this.modMaximo = modMaximo;

            basico = this.maximo / 4;
        }

        public float Taxa
        {
            get { return taxa; }
            set { taxa = value; }
        }

        public int Basico
        {
            get { return basico; }
            set { basico = value; }
        }

        public int Corrente
        {
            get { return corrente; }
            set { corrente = Mathf.Max(0, Mathf.Min(value, maximo)); }
        }

        public int Maximo
        {
            get { return maximo; }
            set { maximo = Mathf.Max(1, value); }
        }

        public int ModMaximo
        {
            get { return modMaximo; }
            set { modMaximo = Mathf.Max(0, value); }
        }

        public void AumentaConsumivel(int valor)
        {
            corrente = Mathf.Min(corrente + valor, maximo);
        }

        public void AumentaAoMaximo()
        {
            corrente = maximo;
        }
    }
}