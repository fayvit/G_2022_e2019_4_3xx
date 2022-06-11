using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuboVertexBlenderUnity : MonoBehaviour
{
    private MeshRenderer mR;
    private MeshFilter mF;
    private Mesh m;
    // Start is called before the first frame update
    void Start()
    {
        mR = GetComponent<MeshRenderer>();
        mF = GetComponent<MeshFilter>();
        m = mF.mesh;

        string s = string.Empty;

        foreach (var V in m.vertices)
        {
            s += V + " : ";
        }
        s += "\n\r";
        foreach (var uv in m.uv)
        {
            s += uv + " : ";
        }

        s += "\n\r";

        foreach (var T in m.triangles)
        {
            s += T + ", ";
        }

        s += "\n\r";
        s += "nVertices: "+m.vertices.Length+" nUv: "+m.uv.Length+" nTriangles: "+m.triangles.Length;

        Debug.Log(s);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
