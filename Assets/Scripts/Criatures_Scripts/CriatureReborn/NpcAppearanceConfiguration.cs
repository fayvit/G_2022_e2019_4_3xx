using UnityEngine;
using FayvitMessageAgregator;
using CustomizationSpace;

namespace Npc2021
{
    public class NpcAppearanceConfiguration : MonoBehaviour
    {

        [SerializeField] private string sId;
        [SerializeField] private Vector3 startRotation = Vector3.forward;

        public string Sid => sId;

        void Start()
        {
            MessageAgregator<MsgApperanceTransport>.AddListener(OnReceiveApperance);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgApperanceTransport>.RemoveListener(OnReceiveApperance);
        }

        private void OnReceiveApperance(MsgApperanceTransport obj)
        {
            bool foi = false;
            foreach (var v in obj.lccd)
                if (!foi && v.Sid == sId)
                {
                    foi = true;
                    Create(v);
                }
        }

        void Create(CustomizationContainerDates ccd)
        {

            GameObject G = CombinerSingleton.Instance.GetCombination(ccd);
            //Debug.Log("CombinerSingleton.Instance.name: "+  CombinerSingleton.Instance.name);

            G.name = "Npc_" + sId;

            BasicLodInsert.Insert(G, 0.04f);

            FayvitSupportSingleton.SupportSingleton.Instance.InvokeOnCountFrame(() =>
            {
                G.AddComponent<MyOtimization>();
                
                G.transform.position = transform.position;
                transform.parent = G.transform;

                MessageAgregator<MsgDesyncStandardAnimation>.Publish(new MsgDesyncStandardAnimation()
                {
                    gameObject = G
                });

                G.transform.rotation = Quaternion.LookRotation(startRotation);

            }, 10);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    
}
