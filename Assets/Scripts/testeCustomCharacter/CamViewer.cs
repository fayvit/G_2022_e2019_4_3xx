using UnityEngine;
using System.Collections;

public class CamViewer : MonoBehaviour
{
    Transform tCam;

    // Use this for initialization
    void Start()
    {
        tCam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(tCam);
    }
}
