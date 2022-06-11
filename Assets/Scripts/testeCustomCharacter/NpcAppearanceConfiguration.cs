﻿using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;
using System.Collections.Generic;

namespace Npc2021
{
    public class NpcAppearanceConfiguration : MonoBehaviour
    {

        [SerializeField] private string sId;
        [SerializeField] private Vector3 startRotation = Vector3.forward;

        //[SerializeField] private TesteMeshCombiner tCombiner;
        //[SerializeField] private SectionCustomizationManager secM;
        //[SerializeField] private SectionCustomizationManager secH;

        // Use this for initialization
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
            //GameObject G = tCombiner.StartCombiner(ccd);



            GameObject G = CombinerSingleton.Instance.GetCombination(ccd);

            G.name = "Npc_" + sId;
            

            FayvitSupportSingleton.SupportSingleton.Instance.InvokeOnCountFrame(() =>
            {
                G.transform.position = transform.position;
                transform.parent = G.transform;

                MessageAgregator<MsgDesyncStandardAnimation>.Publish(new MsgDesyncStandardAnimation()
                {
                    gameObject = G
                });

                G.transform.rotation = Quaternion.LookRotation(startRotation);

            }, 10);

            

            //CombinerSingleton.Instance.SchelduleCombiner(ccd, (GameObject G) => {
            //    G.name = "Npc_" + sId.ToString();

            //    FayvitSupportSingleton.SupportSingleton.Instance.InvokeOnCountFrame(() =>
            //    {
            //        G.transform.position = transform.position;
            //        transform.parent = G.transform;
            //    }, 10);
            //});



            //GameObject G;
            //if (ccd.PersBase == PersonagemBase.feminino)
            //{
            //    secM.SetCustomDates(ccd);
            //    G = secM.gameObject;
            //    Destroy(secH.gameObject);
            //}
            //else
            //{
            //    Destroy(secM.gameObject);
            //    secH.SetCustomDates(ccd);
            //    G = secH.gameObject;
            //}
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    
}

public struct MsgApperanceTransport : IMessageBase
{
    public List<CustomizationContainerDates> lccd;
}
