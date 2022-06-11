using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProPaiDoPai : MonoBehaviour
{
    [SerializeField] private float tempoDestroy = 0.5f;
    bool foi;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!foi)
        {
            Transform T = transform.parent.parent;
            if (T)
            {
                transform.parent = T;
                foi = true;
                Destroy(gameObject, tempoDestroy);
            }
        }
    }
}
