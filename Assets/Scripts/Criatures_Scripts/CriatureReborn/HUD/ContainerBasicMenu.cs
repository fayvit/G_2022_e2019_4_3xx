using UnityEngine;
using System.Collections;
using FayvitUI;

public class ContainerBasicMenu : MonoBehaviour
{
    [SerializeField] private BasicMenu bMenu;

    public static ContainerBasicMenu instance;

    public BasicMenu Menu => bMenu;

    public void SetPercentSizeInTheParent(float xMin,float yMIn,float xMax,float yMax)
    {
        FayvitUiUtility.SetPercentSizeInTheParent(bMenu.GetTransformContainer.GetComponent<RectTransform>(), xMin, yMIn, xMax, yMax);
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
