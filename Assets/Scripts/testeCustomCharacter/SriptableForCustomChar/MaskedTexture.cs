using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Masked Texture")]
public class MaskedTexture : ChangebleElement
{
    public Texture2D baseTex;
    public Texture2D maskTex;
    public SectionDataBase meshParent;
}

public class ChangebleElement : ScriptableObject
{
    public SectionDataBase[] subsection;
    [ArrayElementTitle("materialColorTarget")] public ColorContainer[] coresEditaveis;
}

public enum NomeSlotesDeCores
{ 
    _BrancoDosOlhos,
    _MyBaseColor,
    _CorPupila,
    _CorDaIris,
    _CorDaUmidade,
    _CorDaSobrancelha,
    _Cor_1,//Shader COlored Masked
    _Cor_2,//Shader COlored Masked
    _Cor_3,//Shader COlored Masked
    _Cor_4,//Shader COlored Masked
    _Cor_5,//Shader COlored Masked
    _Cor_6,//Shader COlored Masked
    _BaseColor,//URP Lit Shader
    _TintColor,//Particles for Hooligan
    _CorDaGola,//Shader Torso Base
    _CorDosBotoes,//Shader Torso Base
    _CorDoCinto,//Shader Torso Base
    _CorDaManga,//Shader Torso Base
    _CorDaPele,//Shader Torso Base
    _CorDaBarba//Shader rosto
}

