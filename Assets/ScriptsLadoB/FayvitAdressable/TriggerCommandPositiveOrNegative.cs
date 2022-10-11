using FayvitCommandReader;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.ScriptsLadoB.FayvitAdressable
{
    public class TriggerCommandPositiveOrNegative : MonoBehaviour
    {
        [SerializeField] private CommandConverterString commandName;
        [SerializeField] private GameObject txtContainer;
        [SerializeField] private GameObject imgcontainer;
        [SerializeField] private Text t;
        [SerializeField] private Image img;
        [SerializeField] private Controlador controll;
        [SerializeField]private PosOrNeg sinal;

        private enum PosOrNeg
        { 
            positivo,
            negativo
        }

        // Use this for initialization
        void Start()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            if(Application.isPlaying&& enabled)
                CommandsDictionary.SelectShowInfo(imgcontainer, img, txtContainer, t, commandName, sinal == PosOrNeg.positivo, controll);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}