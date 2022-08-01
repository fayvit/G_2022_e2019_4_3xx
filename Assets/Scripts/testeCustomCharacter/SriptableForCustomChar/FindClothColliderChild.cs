using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindClothColliderChild : MonoBehaviour
{
    [SerializeField] private Transform pai;
    private Cloth myCloth;

    private void OnEnable()
    {
        if (myCloth == null)
        {
            Start();
        }

        myCloth.enabled = false;

        List<Transform> filhos = new List<Transform>();

        EncontreO_Pai();

        if (myCloth != null)
        {            
            filhos.Add(pai);
            InsiraOsFilhos(filhos,pai);
        }

        List<CapsuleCollider> cColliders = new List<CapsuleCollider>();
        foreach (var filho in filhos)
        {
            if (filho.gameObject.layer == 8/*clothCollider*/)
            {
                CapsuleCollider cc = filho.GetComponent<CapsuleCollider>();
                if (cc != null)
                {
                    cColliders.Add(cc);
                }
            }
        }

        myCloth.capsuleColliders = cColliders.ToArray();

        FayvitSupportSingleton.SupportSingleton.Instance.InvokeOnEndFrame(gameObject,() =>
        {
            myCloth.enabled = true;
        });
    }

    void EncontreO_Pai()
    {
        Transform paiProcurado = transform;
        Transform paiDoPai = transform.parent;

        while (paiDoPai != null && !paiDoPai.CompareTag("Finish"))
        {
            paiProcurado = paiDoPai;
            paiDoPai = paiDoPai.parent;
        }

        pai = paiProcurado;
    }

    void InsiraOsFilhos(List<Transform> filhos,Transform pai)
    {
        for (int i = 0; i < pai.childCount; i++)
        {
            filhos.Add(pai.GetChild(i));
            InsiraOsFilhos(filhos, pai.GetChild(i));
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        myCloth = GetComponent<Cloth>();

        if (myCloth == null)
        {
            enabled = false;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
