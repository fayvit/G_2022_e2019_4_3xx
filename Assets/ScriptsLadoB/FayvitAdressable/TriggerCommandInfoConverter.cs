using FayvitCommandReader;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.ScriptsLadoB.FayvitAdressable
{
    public class TriggerCommandInfoConverter : MonoBehaviour
    {
        [SerializeField] private CommandConverterString commandName;
        [SerializeField] private GameObject txt1Container;
        [SerializeField] private GameObject img1container;
        [SerializeField] private Text t1;
        [SerializeField] private Image img1;
        [SerializeField] private GameObject txt2Container;
        [SerializeField] private GameObject img2container;
        [SerializeField] private Text t2;
        [SerializeField] private Image img2;
        [SerializeField] private Controlador controll;

        // Use this for initialization
        void Start()
        {
            OnValidate();
            //CommandsDictionary.SelectShowInfo(img1container,img2container, img1,img2, txt1Container,txt2Container, t1,t2, commandName, controll);
        }

        private void OnValidate()
        {
            if (Application.isPlaying&&enabled)
            {
                CommandsDictionary.SelectShowInfo(img1container, img2container, img1, img2, txt1Container, txt2Container, t1, t2, commandName, controll);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}