using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MeDeAHierarquia : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Transform T = transform;
        string s = T.name;
        int cont = 0;

        while (T.parent.parent != null && cont < 100)
        {
            s = T.parent.name + "/" + s;
            T = T.parent;

            cont++;
        }

        Debug.Log(s);

        DestroyImmediate(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
