using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class OnGroundParticles : MonoBehaviour
    {
		public ImpactParticles aoChao=ImpactParticles.fogoAoChao;
		public bool destruir = true;
		public float repetir = 0.25f;
		// Use this for initialization
		void Start()
		{

			Invoke("ImpactoAoChao", 0.15f);
		}

		void ImpactoAoChao()
		{
			GameObject G = Resources.Load<GameObject>("particles/"+aoChao.ToString());//GameController.g.El.retorna(aoChao);
			Vector3 pos = Vector3.zero;
			RaycastHit ray = new RaycastHit();
			if (Physics.Raycast(transform.position, Vector3.down, out ray))
			{
				pos = ray.point;
			}
			else if (Physics.Raycast(transform.position, Vector3.up, out ray))
			{
				pos = ray.point;
			}
			G = Instantiate(G, pos, Quaternion.identity) as GameObject;
			if (destruir)
				Destroy(G, 1.75f);
			Invoke("ImpactoAoChao", repetir);
		}

		void OnCollisionEnter(Collision emQ)
		{
			foreach (ContactPoint P in emQ.contacts)
			{

				if (Vector3.Angle(P.normal, Vector3.up) < 15 && emQ.gameObject.tag != "Criature")
				{

					GameObject G = (GameObject)Instantiate(
						Resources.Load<GameObject>("particles/" + aoChao.ToString()),
						P.point,
						Quaternion.identity
						);

					Destroy(G, 0.5f);
				}
			}
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}