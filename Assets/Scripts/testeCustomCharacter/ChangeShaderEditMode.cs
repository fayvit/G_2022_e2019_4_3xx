using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ChangeShaderEditMode : MonoBehaviour
{
    [SerializeField] private bool vai;
    [SerializeField] private int indexOfMaterial;
    [SerializeField] private string shaderName;
    [SerializeField] private bool digaSeuNome;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (vai)
        {
            Renderer R = GetComponent<SkinnedMeshRenderer>();
            if (R == null)
            {
                R = GetComponent<MeshRenderer>();
            }

            R.materials[indexOfMaterial].shader = Shader.Find(shaderName);
            vai = false;
        }

        if (digaSeuNome)
        {
            Renderer R = GetComponent<SkinnedMeshRenderer>();
            if (R == null)
            {
                R = GetComponent<MeshRenderer>();
            }

            Debug.Log(R.materials[indexOfMaterial].shader.name);

            digaSeuNome = false;

        }
    }
}
