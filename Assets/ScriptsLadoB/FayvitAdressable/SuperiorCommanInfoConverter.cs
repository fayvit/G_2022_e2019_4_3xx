using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using FayvitCommandReader;

namespace Assets.ScriptsLadoB.FayvitAdressable
{
    public class SuperiorCommanInfoConverter : MonoBehaviour
    {
        [SerializeField] private CommandConverterInt commandName;
        [SerializeField] private GameObject txtContainer;
        [SerializeField] private GameObject imgcontainer;
        [SerializeField] private Text t;
        [SerializeField] private Image img;
        [SerializeField] private Controlador controll;
        

        // Use this for initialization
        void Start()
        {
            CommandsDictionary.SelectShowInfo(imgcontainer, img, txtContainer, t, commandName, controll);
        }

        private void OnValidate()
        {
            if(Application.isPlaying&& enabled)
                CommandsDictionary.SelectShowInfo(imgcontainer, img, txtContainer, t, commandName, controll);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}