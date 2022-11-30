using Criatures2021;
using Criatures2021Hud;
using FayvitBasicTools;
using FayvitCam;
using FayvitMessageAgregator;
using FayvitMove;
using FayvitSupportSingleton;
using TalkSpace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerKeyDjeyHooliganTalk : MonoBehaviour
{
    [SerializeField] private KeyShift ID = KeyShift.hooliganKeyDjey;
    [SerializeField] private Transform posParaDeslPlayer;
    [SerializeField] private Transform posInicialCamera;
    [SerializeField] private Transform posSaidaHooligan;
    [SerializeField] private Transform posDaChegadaDoHooliganNaConversa;
    [SerializeField] private ScheduledTalkManager npc;
    [SerializeField] private ScheduledTalkManager npcSaida;
    [SerializeField] private float lerpTimeForFirstRotationCam;

    private CharacterManager manager;
    private ControlledMoveForCharacter controlleDoManager;
    private ControlledMoveForCharacter controlleDoKeyDjey;
    private MsgSendExternalPanelCommand cmd;
    private GameObject goHooligan;
    private GameObject goKeyDjey;
    private FasesDaqui fase = FasesDaqui.emEspera;
    private float tempodecorrido = 0;

    private enum FasesDaqui
    { 
        emEspera,
        iniciando,
        rotStartCam,
        hooliganVindo,
        girandoParaConversa,
        conversa,
        keyDjeyInfoOpened,
        fraseDeSaidaHooligan,
        saindoHooligan,
        finalizar
    }

    // Start is called before the first frame update
    void Start()
    {
        if (StaticInstanceExistence<IGameController>.SchelduleExistence(Start, this, () =>
        {
            return AbstractGameController.Instance;
        })) 
        {
            if (AbstractGameController.Instance.MyKeys.VerificaAutoShift(ID))
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnValidate()
    {
        npc.OnVallidate();
        npcSaida.OnVallidate();
    }

    // Update is called once per frame
    void Update()
    {
        switch (fase)
        {
            case FasesDaqui.iniciando:
                if (controlleDoManager.UpdatePosition())
                {
                    tempodecorrido = 0;
                    fase = FasesDaqui.rotStartCam;
                }
            break;
            case FasesDaqui.rotStartCam:
                tempodecorrido += Time.deltaTime;
                posInicialCamera.LookAt(goHooligan.transform);
                CameraApplicator.cam.transform.rotation = Quaternion.Lerp(
                    CameraApplicator.cam.transform.rotation,
                    posInicialCamera.rotation,
                    tempodecorrido / lerpTimeForFirstRotationCam
                    );

                if (tempodecorrido > lerpTimeForFirstRotationCam)
                {
                    MontarHooliganNoKeyDjey();

                    controlleDoKeyDjey = new ControlledMoveForCharacter(goKeyDjey.transform);
                    controlleDoKeyDjey.StartFields(goKeyDjey.transform);
                    controlleDoKeyDjey.ModificarOndeChegar(MelhoraInstancia3D.ProcuraPosNoMapa(posDaChegadaDoHooliganNaConversa.position));
                    controlleDoKeyDjey.Mov.ChangeWalkSpeed(20);
                    fase = FasesDaqui.hooliganVindo;
                }
            break;
            case FasesDaqui.hooliganVindo:

                posInicialCamera.LookAt(goHooligan.transform);
                CameraApplicator.cam.transform.rotation = Quaternion.Lerp(
                    CameraApplicator.cam.transform.rotation,
                    posInicialCamera.rotation,
                    tempodecorrido / lerpTimeForFirstRotationCam
                    );

                if (controlleDoKeyDjey.UpdatePosition())
                {
                    ParticulaDoKeyDjey();

                    goHooligan.transform.parent = null;
                    goHooligan.transform.rotation = DirectionOnThePlane.Rotation(goHooligan.transform.forward);
                    
                    MessageAgregator<MsgExitKeyDjey>.Publish(new MsgExitKeyDjey()
                    {
                        usuario = goHooligan,
                        returnState = CharacterState.onFree
                    });
                    goHooligan.GetComponent<CharacterController>().enabled = true;
                    goKeyDjey.SetActive(false);
                    fase = FasesDaqui.girandoParaConversa;

                    Vector3 dir = goHooligan.transform.position - manager.transform.position;
                    CharRotateTo.RotateDir(dir, manager.gameObject);
                    CharRotateTo.RotateDir(-dir, goHooligan);
                    
                    posInicialCamera.position = 0.5f * (goHooligan.transform.position + manager.transform.position);
                    posInicialCamera.rotation = Quaternion.LookRotation(Vector3.Cross(dir, Vector3.up));
                    CameraApplicator.cam.NewFocusForBasicCam(posInicialCamera, 4, 7, true, true);

                    tempodecorrido = 0;
                }
            break;
            case FasesDaqui.girandoParaConversa:
                tempodecorrido += Time.deltaTime;
                if (tempodecorrido > .65f)
                {
                    MessageAgregator<MsgStartExternalInteraction>.Publish();
                    MessageAgregator<MsgSendExternalPanelCommand>.AddListener(OnReceiveCommands);
                    npc.Start(gameObject);
                    npc.IniciaConversa();
                    fase = FasesDaqui.conversa;
                }
            break;
            case FasesDaqui.conversa:
                if (npc.Update(cmd.confirmButton,cmd.returnButton))
                {
                    MessageAgregator<MsgRequestOpenInfoMessage>.Publish(new MsgRequestOpenInfoMessage()
                    {
                        info = InfoMessageType.keyDjeyInfo
                    });

                    MessageAgregator<MsgCloseInfoMessage>.AddListener(OnCloseInfoMessage);
                    fase = FasesDaqui.keyDjeyInfoOpened;
                }
            break;
            case FasesDaqui.keyDjeyInfoOpened:
            break;
            case FasesDaqui.fraseDeSaidaHooligan:
                if (npcSaida.Update(cmd.confirmButton, cmd.returnButton))
                {
                    ParticulaDoKeyDjey();
                    goKeyDjey.SetActive(true);
                    MontarHooliganNoKeyDjey();
                    controlleDoKeyDjey.ModificarOndeChegar(MelhoraInstancia3D.ProcuraPosNoMapa(posSaidaHooligan.position));
                    fase = FasesDaqui.saindoHooligan;
                    tempodecorrido = 0;
                }
            break;
            case FasesDaqui.saindoHooligan:
                tempodecorrido += Time.deltaTime;
                if (tempodecorrido>1&& controlleDoKeyDjey.UpdatePosition())
                {
                    fase = FasesDaqui.finalizar;
                    SceneManager.UnloadSceneAsync("CutsceneRandomHooliganKatids");

                    MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero() { myHero = manager.gameObject });
                }
            break;
        }
    }

    private void MontarHooliganNoKeyDjey()
    {
        goKeyDjey.transform.rotation = goHooligan.transform.rotation;
        MessageAgregator<MsgRequestMountedAnimation>.Publish(new MsgRequestMountedAnimation()
        {
            gameObject = goHooligan
        });
        goHooligan.GetComponent<CharacterController>().enabled = false;
        goHooligan.transform.SetParent(goKeyDjey.transform.Find("Armature/Bone"));

        SupportSingleton.Instance.InvokeOnEndFrame(() =>
        {

            goHooligan.transform.localPosition = new Vector3(0, .54f, .85f);

        });
    }

    private void ParticulaDoKeyDjey()
    {
        GameObject G = ResourcesFolders.GetGeneralElements(GeneralElements.keydjeyParticle);
        G = Instantiate(G, MelhoraInstancia3D.ProcuraPosNoMapa(goHooligan.transform.position), goHooligan.transform.rotation);
        Destroy(G, 5);
        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
        {
            sfxId = FayvitSounds.SoundEffectID.keyDjeySound
        });
    }

    private void MsgCloseInfoMessage()
    {
        npcSaida.Start(gameObject);
        npcSaida.IniciaConversa();
        fase = FasesDaqui.fraseDeSaidaHooligan;
    }

    private void OnCloseInfoMessage(MsgCloseInfoMessage obj)
    {
        MsgCloseInfoMessage();
    }

    private void OnReceiveCommands(MsgSendExternalPanelCommand obj)
    {
        cmd = obj;
    }

    int cont = 0;

    private void OnLoadScene(Scene arg0, LoadSceneMode arg1)
    {
        cont++;

        if (cont >= 2)
            SupportSingleton.Instance.InvokeInSeconds(() =>
            {
                SceneManager.sceneLoaded -= OnLoadScene;
                AbstractGlobalController.Instance.FadeV.StartFadeInWithAction(SetarElementos);
            },1);
    }

    private void SetarElementos()
    {
        
        goHooligan = GameObject.Find("Npc_Random Hooligan");
        goKeyDjey = GameObject.Find("KeyDjey");

        controlleDoManager = new ControlledMoveForCharacter(manager.transform);
        controlleDoManager.StartFields(manager.transform);
        controlleDoManager.ModificarOndeChegar(MelhoraInstancia3D.ProcuraPosNoMapa(posParaDeslPlayer.position));
        //Destroy(goKeyDjey.GetComponent<PetManager>());


        fase = FasesDaqui.iniciando;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")&&!AbstractGameController.Instance.MyKeys.VerificaAutoShift(ID))
        {
            AbstractGameController.Instance.MyKeys.MudaShift(ID, true);
            manager = other.GetComponent<CharacterManager>();

            MessageAgregator<MsgRequestExternalMoviment>.Publish(new MsgRequestExternalMoviment()
            {
                oMovimentado = manager.gameObject
            });
            
            if (SceneManager.GetSceneByName("CutsceneRandomHooliganKatids").isLoaded)
            {
                SetarElementos();
            }
            else {
                AbstractGlobalController.Instance.FadeV.StartFadeOutWithAction(()=> {

                    SupportSingleton.Instance.InvokeInSeconds(() =>
                    {
                        CameraApplicator.cam.OffCamera();
                        CameraApplicator.cam.transform.position = posInicialCamera.position;
                        CameraApplicator.cam.transform.rotation = posInicialCamera.rotation;
                    },.6f);                  
                    OnLoadScene(default, default);
                });
                SceneManager.sceneLoaded += OnLoadScene;
                SceneManager.LoadSceneAsync("CutsceneRandomHooliganKatids", LoadSceneMode.Additive);
            }
        }
    }
}
