using FayvitMessageAgregator;
using FayvitSupportSingleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinerSingleton : MonoBehaviour
{
    //[SerializeField] private TesteMeshCombiner tCombiner;
    [SerializeField] private GameObject baseM;
    [SerializeField] private GameObject baseH;

    //private Queue<SchelduleCombinerContainer> combinationsQueue=new Queue<SchelduleCombinerContainer>();
    //private bool activeCombination;

    //private struct SchelduleCombinerContainer
    //{
    //    public CustomizationContainerDates ccd;
    //    public string guid;
    //    public System.Action<GameObject> acao;
    //}
   

    private static CombinerSingleton instance;
    public static CombinerSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<CombinerSingleton>("Fayvit_CombinerSingleton");
                instance.name = "Fayvit_CombinerSingleton";

                //SupportSingleton.Instance.InvokeOnCountFrame(() =>
                //{
                //    Debug.Log("Combiner name: "+instance.name);
                //}, 100);
            }

            return instance;

        }
    }

    //public TesteMeshCombiner Combiner => tCombiner;

    protected virtual void Awake()
    {
        CombinerSingleton[] gg = FindObjectsOfType<CombinerSingleton>();

        if (gg.Length > 1)
        {
            Debug.Log("Me fui");
            Destroy(gameObject);
            return;
        }

        transform.parent = null;

        DontDestroyOnLoad(gameObject);

        instance = this;
    }

    [System.Serializable]
    private struct MeshCombinationGUID
    {
        public TesteMeshCombiner tCombiner;
        public string guid;
    }

    [SerializeField]List<MeshCombinationGUID> lGuid = new List<MeshCombinationGUID>();

    public GameObject GetCombination(CustomizationContainerDates ccd)
    {
        //Debug.Log("getCOmbination");
        lGuid.Add(new MeshCombinationGUID()
        {
            guid = Guid.NewGuid().ToString(),
            tCombiner = new TesteMeshCombiner()
        });
        return lGuid[lGuid.Count-1].tCombiner.StartCombiner(ccd,baseM,baseH, lGuid[lGuid.Count-1].guid);
    }

    private void Start()
    {
        MessageAgregator<MsgCombinationComplete>.AddListener(OnCombinationComplete);
    }

    private void OnDestroy()
    {
        MessageAgregator<MsgCombinationComplete>.RemoveListener(OnCombinationComplete);
    }

    private void OnCombinationComplete(MsgCombinationComplete obj)
    {
        int guard = -1;
        for (int i = 0; i < lGuid.Count; i++)
        {
            if (lGuid[i].guid == obj.checkKey)
                guard = i;
        }

        if(guard!=-1)
            lGuid.RemoveAt(guard);
    }

    //private void OnCombinationComplete(MsgCombinationComplete obj)
    //{
    //    if (combinationsQueue.Count > 0)
    //    {
    //        SchelduleCombinerContainer scc = combinationsQueue.Peek();
    //        if (obj.checkKey == scc.guid)
    //        {
    //            scc.acao(obj.combined.gameObject);
    //            combinationsQueue.Dequeue();
    //            activeCombination = false;
    //        }
    //    }
    //}

    //public void SchelduleCombiner(CustomizationContainerDates ccd,System.Action<GameObject> acao)
    //{
    //    combinationsQueue.Enqueue(new SchelduleCombinerContainer()
    //    {
    //        ccd = ccd,
    //        acao = acao,
    //        guid = System.Guid.NewGuid().ToString()
    //    });

    //}

    private void Update()
    {
        //if (combinationsQueue.Count > 0 && !activeCombination)
        //    StartCombination();
        
    }

    //private void StartCombination()
    //{
    //    SchelduleCombinerContainer scc = combinationsQueue.Peek();
    //    tCombiner.StartCombiner(scc.ccd, scc.guid);
    //    activeCombination = true;
    //}



    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
