using System.Collections;
using System.Collections.Generic;
using Criatures2021;
using UnityEngine;
using UnityEngine.SceneManagement;
using FayvitMessageAgregator;
using FayvitCam;
using FayvitSupportSingleton;
using FayvitBasicTools;
using FayvitLoadScene;

public class ProvisionalStartGame   : MonoBehaviour
{
    private TesteMeshCombiner meshCombiner;
    private SectionCustomizationManager secManager;

    private Transform character;
    private string guid;

    public static void InitProvisionalStartGame(
        TesteMeshCombiner meshCombiner,
        SectionCustomizationManager secManager)
    {
        GameObject G = new GameObject();
        ProvisionalStartGame P = G.AddComponent<ProvisionalStartGame>();
        P.SetDates(meshCombiner, secManager);
        P.Init();
    }

    public void SetDates(
        TesteMeshCombiner meshCombiner,
        SectionCustomizationManager secManager)
    {
        this.meshCombiner = meshCombiner;
        this.secManager = secManager;
    }

    public void Init()
    {

        MessageAgregator<MsgCombinationComplete>.AddListener(OnCombinationComplete);
        guid = System.Guid.NewGuid().ToString();
        meshCombiner.StartCombiner(secManager,guid);
        
    }

    private void OnCombinationComplete(MsgCombinationComplete obj)
    {
        if (obj.checkKey == guid)
        {
            character = obj.combined;
            Scene comuns = SceneManager.GetSceneByName(NomesCenasEspeciais.ComunsDeFase.ToString());

            SceneManager.MoveGameObjectToScene(character.gameObject, comuns);
                
            Debug.Log(character);

            CharacterManager c = character.gameObject.AddComponent<CharacterManager>();
            c.InTeste = true;
            AbstractGlobalController.Instance.Players.Add(
                new PlayersInGameDb() { 
                Control = FayvitCommandReader.Controlador.teclado,
                DbState = PlayerDbState.ocupadoLocal,
                Manager = c
                }
                );

            SetHeroCamera.Set(character);


            Destroy(CameraApplicator.cam.GetComponent<CustomizationCamManager>());
            Destroy(CameraApplicator.cam.transform.GetChild(0).GetChild(0).gameObject);
            CameraApplicator.cam.GetComponentInChildren<Camera>().rect = new Rect(0, 0, 1, 1);
            SceneManager.MoveGameObjectToScene(CameraApplicator.cam.gameObject, comuns);


            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<MsgCombinationComplete>.RemoveListener(OnCombinationComplete);
            });

            MessageAgregator<MsgFinishEdition>.Publish();
            MessageAgregator<MsgStartMusic>.Publish(new MsgStartMusic()
            {
                clip = Resources.Load<AudioClip>("choroChorandoParaPaulinhoNogueira")
            });

            GameObject G = new GameObject();
            G.name = "provisionalEnemyInstantiate";
            G.AddComponent<ProvisionalEnemyInstantiate>();

            Destroy(secManager.gameObject);
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MessageAgregator<MsgFinishCustomizationMenu>.AddListener(OnEndCustomizationMenu);
    }

    private void OnDestroy()
    {
        MessageAgregator<MsgFinishCustomizationMenu>.RemoveListener(OnEndCustomizationMenu);
    }

    private void OnEndCustomizationMenu(MsgFinishCustomizationMenu obj)
    {
        SetDates(obj.tCombiner, obj.secManager);
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public struct MsgFinishEdition : IMessageBase { }