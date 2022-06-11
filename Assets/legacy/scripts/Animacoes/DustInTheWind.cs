using UnityEngine;
using System.Collections;
using Criatures2021;

public class DustInTheWind : MonoBehaviour {
	public float tempoDeRepeticao = 0.25f;
	public string particula = "poeiraAoVento";

	// Use this for initialization
	void Start () {

		Poeira();
	}

	void Poeira()
	{
		GameObject G = Resources.Load<GameObject>("particles/" + particula);
		Vector3 pos = Vector3.zero;
		RaycastHit ray = new RaycastHit();
		if(Physics.Raycast(transform.position,Vector3.down,out ray))
		{
			pos = ray.point;
		}else if(Physics.Raycast(transform.position,Vector3.up,out ray))
		{
			pos = ray.point;
		}
		G = Instantiate(G,pos,Quaternion.identity) as GameObject;
		Destroy(G,1.75f);
		Invoke("Poeira",tempoDeRepeticao);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
