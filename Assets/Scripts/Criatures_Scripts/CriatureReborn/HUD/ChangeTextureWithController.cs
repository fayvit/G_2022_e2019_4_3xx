using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitMessageAgregator;
using FayvitBasicTools;
using FayvitCommandReader;

public class ChangeTextureWithController : MonoBehaviour
{
    [SerializeField] private GameObject keyboardTexture;
    [SerializeField] private GameObject xboxTexture;

    private Dictionary<Controlador, GameObject> alltextures;

    private void OnEnable()
    {
        if (StaticInstanceExistence<IGlobalController>.SchelduleExistence(
            OnEnable, this, () => { return AbstractGlobalController.Instance; }))
        {
            alltextures = new Dictionary<Controlador, GameObject>()
            {
                { Controlador.teclado, keyboardTexture},
                { Controlador.generalJoystick,xboxTexture}
            };

            ChangeTextureController(AbstractGlobalController.Instance.Control);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
        MessageAgregator<MsgChangeHardwareControler>.AddListener(OnChangeController);
    }

    private void OnDestroy()
    {
        MessageAgregator<MsgChangeHardwareControler>.RemoveListener(OnChangeController);
    }

    private void OnChangeController(MsgChangeHardwareControler obj) {
        ChangeTextureController(obj.newControler);
    }

    void ChangeTextureController(Controlador c)
    {
        //Debug.Log("controlador atual: " + c);
        Controlador key;
        switch (c)
        {
            case Controlador.joystick1:
            case Controlador.joystick2:
            case Controlador.joystick3:
            case Controlador.joystick4:
                
                key = Controlador.generalJoystick;
            break;
            default:
                
                key = Controlador.teclado;
            break;
        }

        foreach (var v in alltextures)
        {
            bool b;
            if (v.Key == key)
                b = true;
            else
                b = false;

            v.Value.SetActive(b);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
