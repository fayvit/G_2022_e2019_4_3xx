using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LayerTent : MonoBehaviour
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
            GameObject[] gg = FindObjectsOfType<GameObject>();

            foreach (var g in gg)
            {
                if (g.layer == 3)
                    g.layer = 8;

                if (g.layer == 6)
                    g.layer = 9;
            }
        }
    }
}
