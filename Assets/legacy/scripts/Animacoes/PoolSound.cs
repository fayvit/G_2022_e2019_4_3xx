using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.legacy.scripts.Animacoes
{
	public class PoolSound : MonoBehaviour
	{
		public float tempoDeDuracao = 1.75f;
		public float tempoDeRepeticao = 0.25f;
		public GameObject particula;

		public List<GameObject> ativos;
		public List<GameObject> inativos;


		void Start()
		{
			Invoke("ImpactoAoChao", 0.15f);
		}

		private void OnDestroy()
		{
			foreach (var v in ativos)
			{
				Destroy(v);
			}

			foreach (var v in inativos)
				Destroy(v);
		}


		void ImpactoAoChao()
		{
			//GameObject G = Resources.Load<GameObject>("particles/" + aoChao.ToString());//GameController.g.El.retorna(aoChao);
			Vector3 pos = Vector3.zero;
			RaycastHit ray;
			if (Physics.Raycast(transform.position, Vector3.down, out ray))
			{
				pos = ray.point;
			}
			else if (Physics.Raycast(transform.position, Vector3.up, out ray))
			{
				pos = ray.point;
			}


			if (gameObject.transform.root.gameObject.activeSelf)
			{
				if (inativos.Count > 0)
				{
					GameObject G = inativos[0];
					inativos.Remove(G);
					ativos.Add(G);
					G.transform.position = pos;
					G.SetActive(true);
					AudioSource P = G.GetComponentInChildren<AudioSource>();
					if (P)
						P.Play();
					else
					{
						P = G.GetComponent<AudioSource>();
						if (P)
							P.Play();
					}

					StartCoroutine(
					DesativarNoTempo(G, tempoDeDuracao));

				}
				else
				{

					GameObject G = Instantiate(particula, pos, Quaternion.identity) as GameObject;
					ativos.Add(G);
					StartCoroutine(
					DesativarNoTempo(G, tempoDeDuracao));
					//if (destruir)
					//	Destroy(G, 1.75f);
				}
			}

			Invoke("ImpactoAoChao", tempoDeRepeticao);
		}

		IEnumerator DesativarNoTempo(GameObject G, float t)
		{
			yield return new WaitForSeconds(t);
			if (G)
			{
				G.SetActive(false);
				ativos.Remove(G);
				inativos.Add(G);

			}
		}

		//void OnCollisionEnter(Collision emQ)
		//{
		//	foreach (ContactPoint P in emQ.contacts)
		//	{

		//		if (Vector3.Angle(P.normal, Vector3.up) < 15 && emQ.gameObject.tag != "Criature")
		//		{

		//			GameObject G = (GameObject)Instantiate(
		//				Resources.Load<GameObject>("particles/" + aoChao.ToString()),
		//				P.point,
		//				Quaternion.identity
		//				);

		//			Destroy(G, 0.5f);
		//		}
		//	}
		//}

		// Update is called once per frame
		void Update()
		{

		}
	}
}