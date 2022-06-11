using UnityEngine;
using System.Collections;
using FayvitSounds;
using FayvitMessageAgregator;
using FayvitBasicTools;

namespace Criatures2021
{
    public class OnFloorImpactParticles : MonoBehaviour
	{

		public float tempoDeDestruicao = 1;
		public string aoChao = "impactoAoChao";
		public SoundEffectID onGroundSound;
		private CharacterController controller;

		// Use this for initialization
		void Start()
		{
			Destroy(this, tempoDeDestruicao);
			controller = GetComponent<CharacterController>();
		}

		// Update is called once per frame
		void Update()
		{


			if (controller.collisionFlags == CollisionFlags.Below)
			{
				MessageAgregator<MsgRequest3dSound>.Publish(new MsgRequest3dSound()
				{
					sender = transform,
					sfxId = onGroundSound
				});

				GameObject G = Resources.Load<GameObject>("particles/"+aoChao);
					//GameController.g.El.retorna(aoChao);
				G = Instantiate(G, transform.position + transform.forward, Quaternion.identity) as GameObject;
				Destroy(G, 1);
				Destroy(this);
			}



		}
	}

}