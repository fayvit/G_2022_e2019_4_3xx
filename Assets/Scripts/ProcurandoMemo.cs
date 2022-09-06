using FayvitBasicTools;
using FayvitSave;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProcurandoMemo : MonoBehaviour
{
    //private void Start()
    //{
    //    Dictionary<string, List<CustomizationContainerDates>> dccd
    //            = RawLoadAndSave.CarregarArquivo<Dictionary<string, List<CustomizationContainerDates>>>(
    //                "listaDeCustomizados_b.tsc", Application.dataPath
    //                );

    //    Dictionary<string, List<CustomizationSpace.CustomizationContainerDates>> dccd_b = new Dictionary<string, List<CustomizationSpace.CustomizationContainerDates>>();
    //    foreach (var v in dccd.Keys.ToList())
    //    {
    //        dccd_b.Add(v, new List<CustomizationSpace.CustomizationContainerDates>());

    //        foreach (var w in dccd[v])
    //            dccd_b[v].Add(Converter(w));
    //    }


    //    RawLoadAndSave.SalvarArquivo(
    //                "listaDeCustomizados.tsc", dccd_b, Application.dataPath
    //                );

    //}

    //CustomizationSpace.CustomizationContainerDates Converter(CustomizationContainerDates ccd)
    //{
    //    List<CustomizationIdentity> malhas = new List<CustomizationIdentity>();
    //    List<CustomizationIdentity> malhasComb = new List<CustomizationIdentity>();
    //    List<CustomizationIdentity> texturasE = new List<CustomizationIdentity>();
    //    List<ColorAssignements> cores = new List<ColorAssignements>();

    //    ccd.GetDates(out malhas, out malhasComb, out texturasE, out cores);

    //    List<CustomizationSpace.SimpleMesh> malhas_b = new List<CustomizationSpace.SimpleMesh>();
    //    List<CustomizationSpace.CombinedMesh> malhasComb_b = new List<CustomizationSpace.CombinedMesh>();
    //    List<CustomizationSpace.CustomizationTextures> texturasE_b = new List<CustomizationSpace.CustomizationTextures>();
    //    List<CustomizationSpace.ColorAssignements> cores_b = new List<CustomizationSpace.ColorAssignements>();

    //    foreach (var v in malhas)
    //    {
    //        malhas_b.Add(new CustomizationSpace.SimpleMesh()
    //        {
    //            contador = v.contador,
    //            id = StringForEnum.GetEnum<CustomizationSpace.SectionDataBase>(v.id.ToString())
    //        });
    //    }

    //    foreach (var v in malhasComb)
    //    {
    //        malhasComb_b.Add(new CustomizationSpace.CombinedMesh()
    //        {
    //            contador = v.contador,
    //            id = StringForEnum.GetEnum<CustomizationSpace.SectionDataBase>(v.id.ToString())
    //        });
    //    }

    //    foreach (var v in texturasE)
    //    {
    //        texturasE_b.Add(new CustomizationSpace.CustomizationTextures()
    //        {
    //            contador = v.contador,
    //            id = StringForEnum.GetEnum<CustomizationSpace.SectionDataBase>(v.id.ToString())
    //        });
    //    }

    //    foreach (var v in cores)
    //    {
    //        cores_b.Add(new CustomizationSpace.ColorAssignements()
    //        {
    //            coresEditaveis = ConverterCoresEditaveis(v.coresEditaveis),
    //            //VetorDeCoresEditaveis = ConverterVetorDeCoresEditaveis(v.VetorDeCoresEditaveis),
    //            id = StringForEnum.GetEnum<CustomizationSpace.SectionDataBase>(v.id.ToString())
    //        });
    //    }

    //    CustomizationSpace.CustomizationContainerDates ccd_b=new CustomizationSpace.CustomizationContainerDates()
    //    {
    //        PersBase = StringForEnum.GetEnum<CustomizationSpace.PersonagemBase>(ccd.PersBase.ToString()),
    //        Sid = ccd.Sid
    //    };

    //    ccd_b.SetDates(
    //        StringForEnum.GetEnum<CustomizationSpace.PersonagemBase>(ccd.PersBase.ToString()),
    //        malhas_b, malhasComb_b, texturasE_b, cores_b,ccd.Sid
    //        );
    //    return ccd_b;
    //}

    ////CustomizationSpace.ColorContainer[] ConverterVetorDeCoresEditaveis(ColorContainer[] c)
    ////{
    ////    List<CustomizationSpace.ColorContainer> retorno = new List<CustomizationSpace.ColorContainer>();
    ////    foreach (var v in c)
    ////    {
    ////        retorno.Add(ConverterUmColorContainer(v)) ;
    ////    }

    ////    return retorno.ToArray();
    ////}

    //CustomizationSpace.ColorContainer ConverterUmColorContainer(ColorContainer v)
    //{
    //    return new CustomizationSpace.ColorContainer()
    //    {
    //        indiceDoMaterialAlvo = v.indiceDoMaterialAlvo,
    //        meshOrChildren = StringForEnum.GetEnum<CustomizationSpace.MainOrChildren>(v.meshOrChildren.ToString()),
    //        materialColorTarget = StringForEnum.GetEnum<CustomizationSpace.NomeSlotesDeCores>(v.materialColorTarget.ToString()),
    //        registro = StringForEnum.GetEnum<CustomizationSpace.RegistroDeCores>(v.registro.ToString())
    //    };
    //}

    //CustomizationSpace.ColorContainerStruct[] ConverterCoresEditaveis(ColorContainerStruct[] c)
    //{
    //    List<CustomizationSpace.ColorContainerStruct> retorno = new List<CustomizationSpace.ColorContainerStruct>();

    //    foreach (var v in c)
    //    {
    //        retorno.Add(new CustomizationSpace.ColorContainerStruct()
    //        {
    //            cor = v.cor,
    //            coresEditaveis = ConverterUmColorContainer(v.coresEditaveis)
    //        });
    //    }

    //    return retorno.ToArray();
    //}
}

