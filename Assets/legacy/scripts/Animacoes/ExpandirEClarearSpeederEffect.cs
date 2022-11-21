using UnityEngine;
public class ExpandirEClarearSpeederEffect : MonoBehaviour
{
    [SerializeField] private Vector3 targetScale=new Vector3(10 ,2 ,10);
    [SerializeField] private float lifeTime = .5f;
    [SerializeField,Range(0,1)] private float targetAlpha = .1f;

    private Renderer R;
    private Vector3 startScale;
    private float startAlpha;
    private float tempoDecorrido = 0;

    private void Start()
    {
        R = GetComponent<Renderer>();
        startAlpha = R.material.GetVector("toAlpha").x;
        startScale = transform.localScale;
    }

    private void Update()
    {
        tempoDecorrido += Time.deltaTime;
        Vector4 V4 = R.material.GetVector("toAlpha");
        float f = Mathf.Lerp(startAlpha, targetAlpha, tempoDecorrido / lifeTime);
        MaterialPropertyBlock P = new MaterialPropertyBlock();
        P.SetVector("toAlpha", new Vector4(f, V4.y, V4.z, V4.w));
        R.SetPropertyBlock(P);
        transform.localScale = Vector3.Lerp(startScale, targetScale, tempoDecorrido / lifeTime);

        //Vector4 f = Mathf.Lerp
        //P.SetVector("toAlpha", f);
    }
}

