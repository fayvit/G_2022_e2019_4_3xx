using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;
using Criatures2021;
using FayvitBasicTools;

public class IamTarget:MonoBehaviour
{
    private int totalEnemies;
    private int contEnemies = 0;
    private bool thisFinally;
    public PetManager comparable;
    private System.Action onPetNottarget;
    private System.Action onPetTarget;

    public static void StaticStart(PetManager comparable,System.Action OnPetNottarget,System.Action onPettarget=null)
    {
        GameObject G = new GameObject();
        G.name = "I'am target";
        IamTarget T = G.AddComponent<IamTarget>();
        T.StartThis(comparable, OnPetNottarget,onPettarget);

        MessageAgregator<MsgAnEnemyTargetMe>.Publish();
    }

    public void StartThis(PetManager comparable,System.Action onPetNotTarget, System.Action onPettarget)
    {
        contEnemies = 0;
        this.onPetNottarget += onPetNotTarget;
        this.onPetTarget += onPettarget;
        this.totalEnemies = FindObjectsOfType<PetManager>().Length;
        this.comparable = comparable;
        MessageAgregator<MsgSendEnemyTargets>.AddListener(OnReceiveEnemyTargets);
    }

    public void EndObject()
    {
        thisFinally = true;
        

        
        Destroy(gameObject);
        
    }

    private void OnDestroy()
    {
        MessageAgregator<MsgSendEnemyTargets>.RemoveListener(OnReceiveEnemyTargets);
    }

    private void OnReceiveEnemyTargets(MsgSendEnemyTargets obj)
    {
        
        if (!thisFinally)
        {
            contEnemies++;

            if (obj.target == comparable && obj.sender != null && obj.sender.MeuCriatureBase.PetFeat.meusAtributos.PV.Corrente > 0)
            {
                onPetTarget?.Invoke();
                EndObject();
            }
            else if (contEnemies >= totalEnemies)
            {
                onPetNottarget();
                EndObject();
            }
        }
    }
}
