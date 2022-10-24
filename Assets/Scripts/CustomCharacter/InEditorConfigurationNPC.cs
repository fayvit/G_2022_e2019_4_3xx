using FayvitSupportSingleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomizationSpace
{
    [ExecuteInEditMode]
    public class InEditorConfigurationNPC : MonoBehaviour
    {
        [SerializeField] private bool vai;
        [SerializeField] private string sId;
        [SerializeField] private bool manterPai;
        // Start is called before the first frame update
        void Start()
        {

        }

        void LoadApperance()
        {
            ToSaveCustomizationContainer.Instance.Load();
            ListToSaveCustomizationContainer.Instance.Load();

            List<CustomizationContainerDates> listList = new List<CustomizationContainerDates>();

            listList.AddRange(ToSaveCustomizationContainer.Instance.ccds.ToArray());

            foreach (var v in ListToSaveCustomizationContainer.Instance.dccd)
            {
                listList.AddRange(v.Value.ToArray());
            }

            Debug.Log(ToSaveCustomizationContainer.Instance.ccds.Count + " : " + ListToSaveCustomizationContainer.Instance.dccd.Count);

            OnLoadAppearanceDB(listList.ToArray());


        }




        void OnLoadAppearanceDB(CustomizationContainerDates[] lccd)
        {
            bool foi = false;
            foreach (var v in lccd)
                if (!foi && v.Sid == sId)
                {
                    Debug.Log("encontrou id na lista");
                    foi = true;
                    Create(v);
                }

            if (!foi)
                Debug.Log("Não encontrou Id na lista");
        }

        void Create(CustomizationContainerDates ccd)
        {
            //GameObject G = tCombiner.StartCombiner(ccd);



            GameObject G = CombinerSingleton.Instance.GetCombination(ccd);

            G.name = "Npc_" + sId;

            Transform pai = null;
            if (manterPai)
                pai = transform.parent;

            

            InEditorSupportSingleton.Instance.InvokeOnCountFrame(() =>
            {
                G.transform.position = transform.position;
                transform.parent = G.transform;
                if(pai!=null)
                    G.transform.SetParent(pai);


            }, 10);
        }

        // Update is called once per frame
        void Update()
        {
            if (vai)
            {
                vai = false;
                LoadApperance();
                Debug.Log("Iniciou processo");
            }
        }
    }
}