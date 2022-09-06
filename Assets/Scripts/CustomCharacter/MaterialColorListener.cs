using FayvitBasicTools;
using FayvitMessageAgregator;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomizationSpace
{
    public class MaterialColorListener : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer targetMR;
        [SerializeField] private int baseMaterialIndex;
        [SerializeField] private int targetMaterialIndex;
        [SerializeField] private NomeSlotesDeCores colorNameIDofTarget;
        [SerializeField] private NomeSlotesDeCores colorNameIDofBase;

        private Material targetM;
        private Material baseM;

        private CustomizationManagerMenu cmMenu;
        // Start is called before the first frame update
        void Start()
        {
            targetM = targetMR.materials[targetMaterialIndex];
            baseM = targetMR.materials[baseMaterialIndex];


            //cmMenu = GetComponent<CustomizationManagerMenu>();

            MessageAgregator<MsgCombinationComplete>.AddListener(OnCombinationComplete);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgCombinationComplete>.RemoveListener(OnCombinationComplete);
        }

        private void OnCombinationComplete(MsgCombinationComplete obj)
        {
            if (HierarchyTools.EstaNaHierarquia(obj.combined, transform.parent))
            {
                Destroy(this);
            }
        }

        //bool EstaNaHierarquia(Transform T)
        //{
        //    Transform umPai = transform.parent;
        //    while (umPai != null)
        //    {
        //        Debug.Log(umPai.name+" : "+T.name+" : "+(umPai==T));
        //        if (umPai == T)
        //            return true;

        //        umPai = umPai.parent;
        //    }

        //    return false;
        //}

        // Update is called once per frame
        void Update()
        {
            Debug.Log("qual cor: " + baseM.GetColor(colorNameIDofBase.ToString()) + " : " + targetM.GetColor(colorNameIDofTarget.ToString()));
            targetM.SetColor(colorNameIDofTarget.ToString(), baseM.GetColor(colorNameIDofBase.ToString()));

            //if (cmMenu == null)
            //    Destroy(this);

        }
    }
}