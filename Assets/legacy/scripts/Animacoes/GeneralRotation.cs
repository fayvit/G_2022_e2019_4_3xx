using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralRotation : MonoBehaviour
{
    [SerializeField] private Vector3 startRotation;
    [SerializeField] private Vector3 speedRotation;
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(startRotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(speedRotation);
    }
}
