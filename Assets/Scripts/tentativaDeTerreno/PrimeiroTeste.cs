using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class PrimeiroTeste : MonoBehaviour
{
    static void SceneGUI(SceneView s)
    {
        Event e = Event.current;
        Debug.Log(e.mousePosition);
    }
}
