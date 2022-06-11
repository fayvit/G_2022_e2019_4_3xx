using FayvitSave;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomizationContainerDates
{
    [SerializeField] private string toId = "";
    [SerializeField] private List<CustomizationIdentity> malhas;
    [SerializeField] private List<CustomizationIdentity> malhasComb;
    [SerializeField] private List<CustomizationIdentity> texturasE;
    [SerializeField] private List<SerializableColorAssignements> colorAssign;

    public string Sid { get => toId; set => toId = value; }
    public PersonagemBase PersBase { get; set; } = PersonagemBase.masculino;

    public void SetDates(
        PersonagemBase p,
        List<SimpleMesh> malhas,
        List<CombinedMesh> malhasComb,
        List<CustomizationTextures> texturasE,
        List<ColorAssignements> colorAssign,
        string toId=""
        )
    {
        this.PersBase = p;
        if (string.IsNullOrEmpty(toId))
        {
            toId = System.Guid.NewGuid().ToString();
        }

        this.toId = toId;
        this.malhas = new List<CustomizationIdentity>();
        this.malhasComb = new List<CustomizationIdentity>();
        this.texturasE = new List<CustomizationIdentity>();
        this.colorAssign = new List<SerializableColorAssignements>();

        foreach (CustomizationIdentity c in malhas)
        {
            CustomizationIdentity c2 = new CustomizationIdentity() { contador = c.contador, id = c.id };
            this.malhas.Add(c2);
        }
        foreach (CustomizationIdentity c in malhasComb)
        {
            CustomizationIdentity c2 = new CustomizationIdentity() { contador = c.contador, id = c.id };
            this.malhasComb.Add(c2);
        }
        foreach (CustomizationIdentity c in texturasE)
        {
            CustomizationIdentity c2 = new CustomizationIdentity() { contador = c.contador, id = c.id };
            this.texturasE.Add(c2);
        }

        foreach (ColorAssignements ccs in colorAssign)
        {
            this.colorAssign.Add(new SerializableColorAssignements(ccs));
        }
    }

    public void SetDates(
        PersonagemBase p,
        List<CustomizationIdentity> malhas,
        List<CustomizationIdentity> malhasComb,
        List<CustomizationIdentity> texturasE,
        List<SerializableColorAssignements> colorAssign)
    {
        this.malhas = malhas;
        this.malhasComb = malhasComb;
        this.texturasE = texturasE;
        this.colorAssign = colorAssign;
        this.PersBase = p;
        
    }

    public void GetDates
        (
        out List<CustomizationIdentity> malhas,
        out List<CustomizationIdentity> malhasComb,
        out List<CustomizationIdentity> texturasE,
        out List<ColorAssignements> colorAssign)
    {
        malhas = this.malhas;
        malhasComb = this.malhasComb;
        texturasE = this.texturasE;
        colorAssign = new List<ColorAssignements>();

        foreach (SerializableColorAssignements sca in this.colorAssign)
        {
            colorAssign.Add(sca.GetCAss);
        }
    }

    public void Save()
    {
        //RawLoadAndSave.SalvarArquivo("CutomizationDates.ccd", this);
        RawLoadAndSave.SalvarNoPlayerPrefs("CutomizationDates.ccd", this);

    }

    public void Load()
    {
        //CustomizationContainerDates ccd = RawLoadAndSave.CarregarArquivo<CustomizationContainerDates>("CutomizationDates.ccd");
        CustomizationContainerDates ccd = RawLoadAndSave.CarregarDoPlayerPrefs<CustomizationContainerDates>("CutomizationDates.ccd");
        SetDates(ccd.PersBase,ccd.malhas, ccd.malhasComb, ccd.texturasE, ccd.colorAssign);
    }
}

[System.Serializable]
public struct ToSaveCustomizationContainer
{
    public List<CustomizationContainerDates> ccds;

    private static ToSaveCustomizationContainer instance;

    public static ToSaveCustomizationContainer Instance {
        get {
            VerifyInstance();
            return instance; }
    }

    public void Save(CustomizationContainerDates ccd)
    {
        Load();

        instance.ccds.Add(ccd);

        //RawLoadAndSave.SalvarArquivo("osCustomizados.tsc", instance.ccds, Application.dataPath);
        RawLoadAndSave.SalvarNoPlayerPrefs("osCustomizados.tsc", instance.ccds);

    }

    public void SaveLoaded()
    {
        //RawLoadAndSave.SalvarArquivo("osCustomizados.tsc", instance.ccds, Application.dataPath);
        RawLoadAndSave.SalvarNoPlayerPrefs("osCustomizados.tsc", instance.ccds);
    }

    public void Load()
    {
        //List<CustomizationContainerDates> ccds = RawLoadAndSave.CarregarArquivo<List<CustomizationContainerDates>>("osCustomizados.tsc", Application.dataPath);
        List<CustomizationContainerDates> ccds = RawLoadAndSave.CarregarDoPlayerPrefs<List<CustomizationContainerDates>>("osCustomizados.tsc");
        if (ccds != null)
            instance.ccds = ccds;
        //Debug.Log(instance.ccds+" : "+ccds);
    }

    static void VerifyInstance()
    {
        if (instance.ccds == null || instance.ccds.Count == 0)
        {
            //Debug.Log("Create new ToSaveCustomizationContainer");

            instance.ccds = new List<CustomizationContainerDates>();

            //Debug.Log(instance.ccds);
            
        }
    }
}

[System.Serializable]
public struct ListToSaveCustomizationContainer
{
    public Dictionary<string,List<CustomizationContainerDates>> dccd;

    private static ListToSaveCustomizationContainer instance;

    public static ListToSaveCustomizationContainer Instance
    {
        get
        {
            VerifyInstance();
            return instance;
        }
    }

    public void Save(string dicKey,CustomizationContainerDates ccd)
    {
        Load();

        instance.dccd[dicKey].Add(ccd);

        //RawLoadAndSave.SalvarArquivo("listaDeCustomizados.tsc", instance.dccd, Application.dataPath);
        RawLoadAndSave.SalvarNoPlayerPrefs("listaDeCustomizados.tsc", instance.dccd);

    }

    public void SaveLoaded()
    {
        //RawLoadAndSave.SalvarArquivo("listaDeCustomizados.tsc", instance.dccd, Application.dataPath);
        RawLoadAndSave.SalvarNoPlayerPrefs("listaDeCustomizados.tsc", instance.dccd);
    }

    public void Load()
    {
        //Dictionary<string,List<CustomizationContainerDates>> dccd 
        //    = RawLoadAndSave.CarregarArquivo<Dictionary<string, List<CustomizationContainerDates>>>(
        //        "listaDeCustomizados.tsc", Application.dataPath
        //        );
        Dictionary<string, List<CustomizationContainerDates>> dccd
            = RawLoadAndSave.CarregarDoPlayerPrefs<Dictionary<string, List<CustomizationContainerDates>>>(
                "listaDeCustomizados.tsc"
                );
        if (dccd != null)
            instance.dccd = dccd;
        //Debug.Log(instance.dccd + " : " + dccd);
    }

    public void CreateList(string dicKey)
    {
        if (!dccd.ContainsKey(dicKey))
        {
            dccd.Add(dicKey, new List<CustomizationContainerDates>());
        }

        SaveLoaded();
    }

    static void VerifyInstance()
    {
        if (instance.dccd == null || instance.dccd.Count == 0)
        {
            //Debug.Log("Create new List Characters");

            instance.dccd = new Dictionary<string,List<CustomizationContainerDates>>();

            //Debug.Log(instance.dccd);

        }
    }
}

public enum PersonagemBase
{
    masculino,
    feminino
}