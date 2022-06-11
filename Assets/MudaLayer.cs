using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MudaLayer : MonoBehaviour
{
    public bool vai;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (vai)
        {
            vai = false;
            GameObject[] Gs = FindObjectsOfType<GameObject>();

            foreach (var G in Gs)
            {
                if (G.layer == 3)
                    G.layer = 8;
                if (G.layer == 6)
                    G.layer = 9;
            }
        }
    }
}
