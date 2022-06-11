using UnityEngine;
using System.Collections;
using FayvitUI;

public class YesOrNoMenu : MonoBehaviour
{
    [SerializeField] private BasicMenu bMenu;

    public static YesOrNoMenu instance;

    public BasicMenu Menu => bMenu;

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
