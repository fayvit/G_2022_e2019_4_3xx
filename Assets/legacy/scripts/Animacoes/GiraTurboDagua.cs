using UnityEngine;
using System.Collections;

public class GiraTurboDagua : MonoBehaviour {
	[SerializeField] private float rotVel = 400;
	[SerializeField] private float afastamento = .5f;
	float tempo = 0;
	bool sinal;
	bool naoAfastou = true;
	// Use this for initialization
	void Start () {
		sinal = (transform.localPosition.x>0)?true:false;
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(transform.parent.position,transform.parent.forward,rotVel*Time.deltaTime);
		tempo+=Time.deltaTime;

		if(tempo>0.1f&&naoAfastou)
		{
			naoAfastou = false;
			float novoX  = sinal ? afastamento: - afastamento;
			float novoY = 0;

			transform.localPosition = new Vector3(novoX,novoY,transform.localPosition.z);
		}
	}
}
