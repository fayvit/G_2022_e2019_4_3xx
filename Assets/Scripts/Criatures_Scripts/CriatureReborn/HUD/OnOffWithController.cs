using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitCommandReader;
using FayvitMessageAgregator;
using FayvitBasicTools;

public class OnOffWithController : MonoBehaviour
{
    [Header("Escolha com quais controladores o objeto será ligado nos restantes ele será desligado")]
    [SerializeField] private List<Controlador> ligarCom;
    [SerializeField] private GameObject alvo;


    private void OnEnable()
    {
        if(StaticInstanceExistence<IGlobalController>.SchelduleExistence(
            OnEnable, this, () => { return AbstractGlobalController.Instance; }))
            LigarComControlador(AbstractGlobalController.Instance.Control);
        
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

    private void OnChangeController(MsgChangeHardwareControler obj)
    {
        LigarComControlador(obj.newControler);
    }

    private void LigarComControlador(Controlador c)
    {
        if (ligarCom.Contains(c))
            alvo.SetActive(true);
        else
            alvo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
