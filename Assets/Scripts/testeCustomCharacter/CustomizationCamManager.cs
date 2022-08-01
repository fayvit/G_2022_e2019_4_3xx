using FayvitCam;
using FayvitCommandReader;
using FayvitMessageAgregator;
using UnityEngine;

public class CustomizationCamManager : MonoBehaviour
{
    [SerializeField] private Vector3 velMinMaxZ;
    [SerializeField] private Vector3 velMinMaxY;

    private SetOfSectionDB setDb = SetOfSectionDB.tronco;
    private DatesForCam dtForCam;

    private bool bloqueiaMouseCam;
    private bool mudandoCam;
    private float tempoDecorrido = 0;
    private float startDistance = 0;
    private float startHeight = 0;
    private const float TEMPO_MUDANDO_CAM = .375F;
    

    // Start is called before the first frame update
    void Start()
    {
        MessageAgregator<MsgChangeMenuDb>.AddListener(OnChangeMenuDb);
    }

    private void OnDestroy()
    {
        MessageAgregator<MsgChangeMenuDb>.RemoveListener(OnChangeMenuDb);
    }

    private void OnChangeMenuDb(MsgChangeMenuDb obj)
    {
        SetOfSectionDB setDb = CustomizatioDatesForCam.GetDataBaseCamSet(obj.sdb);
        if (setDb != this.setDb)
        {
            this.setDb = setDb;
            mudandoCam = true;
            tempoDecorrido = 0;
            startDistance = CameraApplicator.cam.Cdir.SphericalDistance;
            startHeight = CameraApplicator.cam.Cdir.VarVerticalHeightPoint;
            dtForCam = CustomizatioDatesForCam.GetDates(obj.sdb);
        }
    }

    private ICommandReader CurrentCommander
    {
        get => CommandReader.GetCR(FayvitBasicTools.AbstractGlobalController.Instance.Control);
    }

    // Update is called once per frame
    void Update()
    {
        DirectionalCamera cDir = CameraApplicator.cam.Cdir;
        if (mudandoCam)
        {
            
            tempoDecorrido += Time.deltaTime;
            cDir.SphericalDistance = Mathf.Lerp(startDistance, dtForCam.distance, tempoDecorrido / TEMPO_MUDANDO_CAM);
            cDir.VarVerticalHeightPoint = Mathf.Lerp(startHeight, dtForCam.height, tempoDecorrido / TEMPO_MUDANDO_CAM);

            if (tempoDecorrido >= TEMPO_MUDANDO_CAM)
                mudandoCam = false;

        }
        else
        {
            
            Vector3 V = new Vector3(
                -CurrentCommander.GetAxis(CommandConverterString.camX),
                -CurrentCommander.GetAxis(CommandConverterString.camY),
                CurrentCommander.GetAxis(CommandConverterString.camZ)
                );

            bool foi = (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0) ||(Input.GetAxis("Mouse ScrollWheel")!=0);
            foi &=   bloqueiaMouseCam; ;
            foi = !foi;

            if (Input.GetKeyDown(KeyCode.Z))
                bloqueiaMouseCam = !bloqueiaMouseCam;


            if (CameraApplicator.cam)
            {
                
                if (foi)
                {
                    CameraApplicator.cam.ValoresDeCamera(V.x, 0, false, false);
                    float f = cDir.SphericalDistance + V.z * velMinMaxZ.x * Time.deltaTime;
                    cDir.SphericalDistance = Mathf.Clamp(f, velMinMaxZ.y, velMinMaxZ.z);
                    f = cDir.VarVerticalHeightPoint + V.y * velMinMaxY.x * Time.deltaTime;
                    cDir.VarVerticalHeightPoint = Mathf.Clamp(f, velMinMaxY.y, velMinMaxY.z);
                }
                else
                {
                    KeyboardCommandReader.Instance.ZeraLerpVal(CommandConverterString.camX);
                    KeyboardCommandReader.Instance.ZeraLerpVal(CommandConverterString.camY);
                    KeyboardCommandReader.Instance.ZeraLerpVal(CommandConverterString.camZ);
                }
            }
            else
                Debug.Log("Ué");
        }
    }
}

public struct MsgChangeMenuDb : IMessageBase
{
    public SectionDataBase sdb;
}

public struct DatesForCam
{
    public float distance;
    public float height;
}

public enum SetOfSectionDB
{ 
    cabeca,
    tronco,
    membros,
    allView
}


public class CustomizatioDatesForCam
{
    public static DatesForCam GetDates(SectionDataBase sdb)
    {
        SetOfSectionDB sosDb = GetDataBaseCamSet(sdb);
        switch( sosDb)
        {
            case SetOfSectionDB.cabeca: return new DatesForCam() { distance = .8f, height = .7f };
            case SetOfSectionDB.tronco: return new DatesForCam() { distance = 1.5f, height = .32f };
            case SetOfSectionDB.membros: return new DatesForCam() { distance = 1.5f, height = -0.27f };
            case SetOfSectionDB.allView: return new DatesForCam() { distance = 1.8f, height = 0f };
            default:  return new DatesForCam() { distance = .8f, height = .7f };
        };
        //return sosDb switch
        //{
        //    SetOfSectionDB.cabeca => new DatesForCam() { distance = .8f, height = .7f },
        //    SetOfSectionDB.tronco => new DatesForCam() { distance = 1.5f, height = .32f },
        //    SetOfSectionDB.membros => new DatesForCam() { distance = 1.5f, height = -0.27f },
        //    SetOfSectionDB.allView => new DatesForCam() { distance = 1.8f, height = 0f },
        //    _ => new DatesForCam() { distance = .8f, height = .7f }
        //};
    }

    public static SetOfSectionDB GetDataBaseCamSet(SectionDataBase sdb)
    {

        switch(sdb)
        {
            case SectionDataBase.barba       :return SetOfSectionDB.cabeca;
            case SectionDataBase.@base       :return SetOfSectionDB.cabeca;
            case SectionDataBase.cabelo      :return SetOfSectionDB.cabeca;
            case SectionDataBase.globoOcular :return SetOfSectionDB.cabeca;
            case SectionDataBase.iris        :return SetOfSectionDB.cabeca;
            case SectionDataBase.nariz       :return SetOfSectionDB.cabeca;
            case SectionDataBase.pupila      :return SetOfSectionDB.cabeca;
            case SectionDataBase.queixo      :return SetOfSectionDB.cabeca;
            case SectionDataBase.umidade     :return SetOfSectionDB.cabeca;
            case SectionDataBase.sobrancelha :return SetOfSectionDB.cabeca;
            case SectionDataBase.torso       :return SetOfSectionDB.tronco;
            case SectionDataBase.mao         :return SetOfSectionDB.tronco;
            case SectionDataBase.pernas      :return SetOfSectionDB.membros;
            case SectionDataBase.botas       :return SetOfSectionDB.membros;
            case SectionDataBase.cintura     :return SetOfSectionDB.membros;
            case SectionDataBase.empty       :return SetOfSectionDB.allView;
            default                           :return SetOfSectionDB.cabeca;
        };
    }
}
