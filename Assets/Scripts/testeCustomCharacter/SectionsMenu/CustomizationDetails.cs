[System.Serializable]
public class ColorContainer:SignatureBase
{
    public NomeSlotesDeCores materialColorTarget;
    public RegistroDeCores registro;
    public string ColorTargetName { get => materialColorTarget.ToString(); }
}

[System.Serializable]
public class TextureContainer:SignatureBase
{
    public SectionDataBase id;
    public MaskedTextureId baseTextureID;
    public MaskedTextureId maskedTextureID;
    public string MaskedIdName { get => maskedTextureID.ToString(); }
    public string BaseIdName { get => baseTextureID.ToString(); }
}

[System.Serializable]
public class SignatureBase
{
    public int indiceDoMaterialAlvo;
    public MainOrChildren meshOrChildren;
}

public enum RegistroDeCores
{ 
    skin,
    registravel,
    slote1,
    slote2,
    slote3,
    slote4
}

public enum MainOrChildren
{ 
    mainObject,
    childrenObject,
    parentObject
}

public enum SectionDataBase
{
    @base,
    cabelo,
    queixo,
    globoOcular,
    pupila,
    iris,
    umidade,
    sobrancelha,
    barba,
    torso,
    mao,
    cintura,
    pernas,
    botas,
    particular,
    nariz,
    empty
}