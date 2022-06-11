using UnityEngine;
using System.Collections;
using FayvitUI;

namespace TalkSpace
{
    public class DisplayTextManager : MonoBehaviour
    {
        [SerializeField] private TextDisplay tDisplay;

        public static DisplayTextManager instance;

        public TextDisplay DisplayText => tDisplay;

        // Use this for initialization
        void Start()
        {
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}