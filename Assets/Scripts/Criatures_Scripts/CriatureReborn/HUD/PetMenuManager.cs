using FayvitUI;
using System.Collections;
using UnityEngine;

namespace Criatures2021Hud
{
    public class PetMenuManager : MonoBehaviour
    {
        [SerializeField] private PetMenu petMenu;

        public PetMenu Menu => petMenu;
        public static PetMenuManager instance;

        public void SetPercentSizeInTheParent(float xMin, float yMIn, float xMax, float yMax)
        {
            FayvitUiUtility.SetPercentSizeInTheParent(petMenu.GetTransformContainer.GetComponent<RectTransform>(), xMin, yMIn, xMax, yMax);
        }

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