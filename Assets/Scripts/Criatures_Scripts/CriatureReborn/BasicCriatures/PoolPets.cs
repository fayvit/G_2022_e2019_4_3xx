using System.Collections.Generic;
using UnityEngine;

namespace Criatures2021.BasicCriatures
{
    public class PoolPet : MonoBehaviour
	{
		

		public Dictionary<PetName, List<GameObject>> ativos;
		public Dictionary<PetName, List<GameObject>> inativos;

		public static PoolPet instace;

		void Start()
		{
			instace = this;
		}

		private void OnDestroy()
		{
			foreach (var v in ativos)
				foreach (var w in v.Value)
					Destroy(w);


			foreach (var v in inativos)
				foreach (var w in v.Value)
					Destroy(w);
		}

		public GameObject GetPetGO(PetName p)
		{
			if (inativos.ContainsKey(p) && inativos[p].Count > 0)
			{
				GameObject G = inativos[p][0];
				inativos[p].Remove(G);
				if (ativos.ContainsKey(p))
				{
					ativos[p].Add(G);
				}
				else
					ativos.Add(p, new List<GameObject> { G });

				return G;
			}
			else
			{
				GameObject G = Instantiate(Resources.Load<GameObject>("Criatures/" + p.ToString()));
				ativos.Add(p,new List<GameObject> {G});
				return G;
			}
		}

		public void DisablePetGO(GameObject G, PetName p)
		{
			ativos[p].Remove(G);
			if (inativos.ContainsKey(p))
				inativos[p].Add(G);
			else
				inativos.Add(p, new List<GameObject> { G });
		}
	}
}