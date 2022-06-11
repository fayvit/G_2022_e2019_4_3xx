using UnityEngine;
using System.Collections;

public class AnimaLuzPsiquica : MonoBehaviour
{

    private Light L;
    private float luminosidade;
    private float tempoDecorrido = 0;
    private bool clareando = false;

    private const float fStart = .25f;
    private const float fEnd = 5f;
    private const float fDifTime = .25f;
    // Use this for initialization
    void Start()
    {
        L = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        tempoDecorrido += Time.deltaTime;

        if (!clareando)
        {
            luminosidade = Mathf.Lerp(luminosidade, fStart,tempoDecorrido/fDifTime);
            if (tempoDecorrido > fDifTime)
            {
                tempoDecorrido = 0;
                clareando = true;
            }
        }
        else
        {
            luminosidade = Mathf.Lerp(luminosidade, fEnd, 5 * Time.deltaTime);
            if (tempoDecorrido > fDifTime)
            {
                tempoDecorrido = 0;
                clareando = false;
            }
        }

        L.intensity = luminosidade;

    }
}
