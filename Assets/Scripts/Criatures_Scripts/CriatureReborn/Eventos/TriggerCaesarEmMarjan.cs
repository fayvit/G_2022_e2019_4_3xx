using Criatures2021;
using FayvitBasicTools;
using FayvitCam;
using FayvitMessageAgregator;
using FayvitMove;
using FayvitSupportSingleton;
using System.Collections;
using System.Collections.Generic;
using TalkSpace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerCaesarEmMarjan : MonoBehaviour
{
    [SerializeField] private KeyShift ID = KeyShift.CaesarFalaEmMarjan;
    [SerializeField] private ScheduledTalkManager npc;
    [SerializeField] private Transform posInicialCamera;
    [SerializeField] private Transform posParaDeslPlayer;
    [SerializeField] private Transform posDeslCaesar;
    [SerializeField] private Transform posCaesarSaindo;
    [SerializeField] private Transform pos2Camera;
    [SerializeField] private ControlledMoveForCharacter controlleDoManager;
    [SerializeField] private ControlledMoveForCharacter controlleDoCaesar;

    private int cont = 0;
    private CharacterManager manager;
    private GameObject goCaesar;
    private MsgSendExternalPanelCommand cmd;
    private FasesDaqui fase = FasesDaqui.emEspera;
    private enum FasesDaqui
    {
        emEspera,
        iniciando,
        iniciaConversa,
        mudouCamera,
        caesarSaindo
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
    }

    private void OnReceiveCommands(MsgSendExternalPanelCommand obj)
    {
        cmd = obj;
    }

    bool b;
    // Update is called once per frame
    void Update()
    {
        switch (fase)
        {
            case FasesDaqui.iniciando:
                Vector3 difCam = CameraApplicator.cam.transform.position- goCaesar.transform.position;
                controlleDoManager.UpdatePosition();
                if (controlleDoCaesar.UpdatePosition())
                {
                    CameraApplicator.cam.transform.position = pos2Camera.position;
                    CameraApplicator.cam.transform.rotation = pos2Camera.rotation;
                    //tempodecorrido = 0;
                    fase = FasesDaqui.mudouCamera;
                }
                else
                    CameraApplicator.cam.transform.position = goCaesar.transform.position + difCam;
            break;
            case FasesDaqui.mudouCamera:
                if (controlleDoManager.UpdatePosition(run:true))
                {
                    MessageAgregator<MsgStartExternalInteraction>.Publish();
                    MessageAgregator<MsgSendExternalPanelCommand>.AddListener(OnReceiveCommands);
                    npc.Start(gameObject);
                    npc.IniciaConversa();
                    npc.GO_Reference = gameObject;
                    fase = FasesDaqui.iniciaConversa;
                    b = false;
                }
            break;
            case FasesDaqui.iniciaConversa:
                b = cmd.returnButton ? true : b;
                //Debug.Log(cmd.returnButton + " : " + b);
                if (npc.Update(cmd.confirmButton,cmd.returnButton))
                {

                    //Debug.Log(cmd.returnButton + " : " + b);

                    if (b)
                    {
                        //Debug.Log("apertou");
                        //SupportSingleton.Instance.InvokeOnCountFrame(() =>
                        //{
                            //Debug.Log("chamou");
                            CameraApplicator.cam.transform.position = pos2Camera.position;
                            CameraApplicator.cam.transform.rotation = pos2Camera.rotation;
                        //},5);
                    }
                    MessageAgregator<MsgReturnRememberedMusic>.Publish();
                    controlleDoCaesar.ModificarOndeChegar(posCaesarSaindo.position);
                    fase = FasesDaqui.caesarSaindo;
                }
            break;
            case FasesDaqui.caesarSaindo:
                if (controlleDoCaesar.UpdatePosition(run: true))
                {
                    SceneManager.UnloadSceneAsync("cutsceneCaesarEmMarjan");
                    fase = FasesDaqui.emEspera;
                    MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero()
                    {
                        myHero = manager.gameObject
                    });

                    Destroy(gameObject);
                }
            break;
                
        }
    }

    private void SetarElementos()
    {

        goCaesar = GameObject.Find("Npc_Caesar Palace");
        controlleDoCaesar = new ControlledMoveForCharacter(goCaesar.transform);
        controlleDoCaesar.StartFields(goCaesar.transform);
        controlleDoCaesar.ModificarOndeChegar(MelhoraInstancia3D.ProcuraPosNoMapa(posDeslCaesar.position));

        controlleDoManager = new ControlledMoveForCharacter(manager.transform);
        controlleDoManager.StartFields(manager.transform);
        controlleDoManager.ModificarOndeChegar(MelhoraInstancia3D.ProcuraPosNoMapa(posParaDeslPlayer.position));
        ////Destroy(goKeyDjey.GetComponent<PetManager>());
        MessageAgregator<MsgStartMusicWithRecovery>.Publish(new MsgStartMusicWithRecovery()
        {
            nmcvc = new FayvitSounds.NameMusicaComVolumeConfig()
            {
                Musica = FayvitSounds.NameMusic.xodoDaBaiana,
                Volume=1
            }
        });

        fase = FasesDaqui.iniciando;

    }

    private void OnLoadScene(Scene arg0, LoadSceneMode arg1)
    {
        cont++;

        if (cont >= 2)
            SupportSingleton.Instance.InvokeInSeconds(() =>
            {
                SceneManager.sceneLoaded -= OnLoadScene;
                AbstractGlobalController.Instance.FadeV.StartFadeInWithAction(SetarElementos);
            }, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!AbstractGameController.Instance.MyKeys.VerificaAutoShift(ID))
        {
            if (other.CompareTag("Criature"))
            {
                KeyDjeyTransportManager k = other.GetComponent<KeyDjeyTransportManager>();
                if (k)
                {
                    k.SairDoKeyDjey(returnState: CharacterState.stopped, melhorarPosicaoDoPet: false);
                }
            }
            else
            if (other.CompareTag("Player"))
            {
                AbstractGameController.Instance.MyKeys.MudaShift(ID, true);
                manager = other.GetComponent<CharacterManager>();

                MessageAgregator<MsgRequestExternalMoviment>.Publish(new MsgRequestExternalMoviment()
                {
                    oMovimentado = manager.gameObject
                });

                if (SceneManager.GetSceneByName("cutsceneCaesarEmMarjan").isLoaded)
                {
                    SupportSingleton.Instance.InvokeInSeconds(() =>
                    {
                        CameraApplicator.cam.OffCamera();
                        CameraApplicator.cam.transform.position = posInicialCamera.position;
                        CameraApplicator.cam.transform.rotation = posInicialCamera.rotation;
                    }, .6f);

                    SetarElementos();
                }
                else
                {
                    AbstractGlobalController.Instance.FadeV.StartFadeOutWithAction(() =>
                    {

                        SupportSingleton.Instance.InvokeInSeconds(() =>
                        {
                            CameraApplicator.cam.OffCamera();
                            CameraApplicator.cam.transform.position = posInicialCamera.position;
                            CameraApplicator.cam.transform.rotation = posInicialCamera.rotation;

                            SceneManager.sceneLoaded += OnLoadScene;
                            SceneManager.LoadSceneAsync("cutsceneCaesarEmMarjan", LoadSceneMode.Additive);
                        }, 1f);
                        OnLoadScene(default, default);


                    });

                }
            }
        }
    }
}
