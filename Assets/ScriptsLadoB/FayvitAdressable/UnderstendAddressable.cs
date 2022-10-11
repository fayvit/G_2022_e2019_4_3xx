using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UnderstendAddressable : MonoBehaviour
{
    [SerializeField] private List<AssetReference> references;

    // Start is called before the first frame update
    void Start()
    {
        AsyncOperationHandle<Sprite> loadOperation = Addressables.LoadAssetAsync<Sprite>("btnB.png");
        loadOperation.Completed += Completou;
    }

    private void Completou(AsyncOperationHandle<Sprite> obj)
    {
        //references.Add(obj.);
        Debug.Log(obj.Result);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
