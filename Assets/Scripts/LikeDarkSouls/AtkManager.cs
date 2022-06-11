using FayvitMove;
using FayvitCommandReader;
using UnityEngine;
using FayvitMessageAgregator;

namespace FayvitLikeDarkSouls
{
    [System.Serializable]
    public class AtkManager
    {
        [SerializeField] private int maxStep = 100;
        [SerializeField] private float timeToRunAtk = 1;
        [SerializeField] private DamageCollider colisorDeAtaque;

        private int atkStep = 0;
        private bool schelduleNewAtk = false;
        private bool endAtk = false;
        private bool freedonMove = false;
        private GameObject dono;

        public bool OpenNewAtk { get; set; } = false;
        public float RunTime { get; set; } = 0;

        public void StartAttack(GameObject G)
        {
            dono = G;
            endAtk = false;
            OpenNewAtk = false;
            schelduleNewAtk = false;
            freedonMove = false;

            if (RunTime < timeToRunAtk)
            {
                atkStep = 1;
            }
            else
            {
                atkStep = -1;
            }

            MessageAgregator<MsgAtkTrigger>.Publish(new MsgAtkTrigger()
            {
                dono = dono,
                atkSteep = atkStep
            });
            //FayvitMoveEventAgregator.Publish(new FayvitMoveEvent(FayvitMoveEventKey.atkTrigger, dono, atkStep));
        }

        public void OnReceivedAnimationPoint(MsgAnimationPointCheck obj)
        {
            if (obj.sender == dono)
            {
                string s = obj.extraInfo;
                switch (s)
                {
                    case "trailStart":
                        InstancieColisorDeAtaque();
                        break;
                    case "offTrail":
                        DestruaColisorInstanciado();
                        break;
                }
            }
        }

        private void DestruaColisorInstanciado()
        {
            colisorDeAtaque.gameObject.SetActive(false);
            colisorDeAtaque.Reestart();
        }

        private void InstancieColisorDeAtaque()
        {
            colisorDeAtaque.gameObject.SetActive(true);
            colisorDeAtaque.Reestart();
        }

        public bool UpdateAttack(bool requestNewAtk)
        {
            // bool retorno = false;

            if (requestNewAtk && OpenNewAtk)
                schelduleNewAtk = true;

            return endAtk;
        }

        public void VerifyEndAtk()
        {

            if (schelduleNewAtk && (maxStep > 0 ? atkStep < maxStep : true))
            {
                atkStep++;
                MessageAgregator<MsgAtkTrigger>.Publish(new MsgAtkTrigger()
                {
                    atkSteep = atkStep,
                    dono = dono
                });
                //FayvitMoveEventAgregator.Publish(new FayvitMoveEvent(FayvitMoveEventKey.atkTrigger, dono, atkStep));
                schelduleNewAtk = false;
                OpenNewAtk = false;
            }
            else
            {
                freedonMove = true;
            }

        }

        public void ReturnToDefault()
        {
            atkStep = 0;
            OpenNewAtk = false;
            endAtk = false;

        }

        public bool EndAnimation()
        {
            if (freedonMove)
                endAtk = true;

            return endAtk;

        }
    }

    public struct MsgAtkTrigger : IMessageBase
    {
        public GameObject dono;
        public int atkSteep;
    }
}