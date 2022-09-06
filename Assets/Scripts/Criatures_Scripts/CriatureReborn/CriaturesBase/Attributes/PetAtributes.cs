using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class PetAtributes
    {
        [SerializeField] ConsumableAttribute pv;
        [SerializeField] ConsumableAttribute pe;
        [SerializeField] IntrinsicAttribute ataque;
        [SerializeField] IntrinsicAttribute defesa;
        [SerializeField] IntrinsicAttribute poder;

        public PetAtributes(AttributesContainer container)
        {
            pv = container.pv;
            pe = container.pe;
            ataque = container.ataque;
            defesa = container.defesa;
            poder = container.poder;
        }
        public IntrinsicAttribute Ataque
        {
            get { return ataque; }
            set { ataque = value; }
        }

        public IntrinsicAttribute Defesa
        {
            get { return defesa; }
            set { defesa = value; }
        }

        public ConsumableAttribute PE
        {
            get { return pe; }
            set { pe = value; }
        }

        public IntrinsicAttribute Poder
        {
            get { return poder; }
            set { poder = value; }
        }

        public ConsumableAttribute PV
        {
            get { return pv; }
            set { pv = value; }
        }
    }
}