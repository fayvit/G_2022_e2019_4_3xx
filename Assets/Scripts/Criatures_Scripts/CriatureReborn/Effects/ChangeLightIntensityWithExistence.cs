using UnityEngine;


public class ChangeLightIntensityWithExistence : MonoBehaviour
{
    [SerializeField] private float changedIntensity = .125f;
    private void Start()
    {
        Light[] L = FindObjectsOfType<Light>();
        Light minhaLuz = GetComponent<Light>();

        foreach (Light l in L)
        {
            if (l.type == LightType.Directional &&l!=minhaLuz)
                minhaLuz.intensity = changedIntensity;
        }
    }
}

