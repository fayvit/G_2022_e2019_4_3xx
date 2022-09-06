using UnityEngine;
using System.Collections.Generic;
using FayvitLoadScene;
using TextBankSpace;
using Spawns;

[System.Serializable]
public class ArmagedomsLocations
{
    private static Dictionary<IndiceDeArmagedoms, VisistsToArmagedoms> l = new Dictionary<IndiceDeArmagedoms, VisistsToArmagedoms>()
    {
        { IndiceDeArmagedoms.acampamentoDaResistencia, new VisistsToArmagedoms(){
            nomeDasCenas=new NomesCenas[1]{NomesCenas.acampamentoDaResistencia },
            spawnId = SpawnID.numero2
        } },
        { IndiceDeArmagedoms.deKatids,new VisistsToArmagedoms()
        {
        nomeDasCenas=new NomesCenas[1]{NomesCenas.planicieDeKatids },
        spawnId = SpawnID.numero2
        } },
        { IndiceDeArmagedoms.deMarjan,new VisistsToArmagedoms()
        {
        nomeDasCenas=new NomesCenas[1]{NomesCenas.planicieDeKatids },
        spawnId = SpawnID.numero3
    } }
    };

    public static Dictionary<IndiceDeArmagedoms, VisistsToArmagedoms> L
    {
        get { return l; }
    }
}

public class VisistsToArmagedoms
{
    public SpawnID spawnId;
    public NomesCenas[] nomeDasCenas;    

    public static string NomeEmLinguas(IndiceDeArmagedoms i)
    {
        return TextBank.RetornaListaDeTextoDoIdioma(TextKey.armagedomNames)[(int)Mathf.Log((int)i,2)];
    }
}

public enum IndiceDeArmagedoms
{
    // Registrar no nome em linguas
    None=0,
    acampamentoDaResistencia=1,
    deKatids = 2,
    deMarjan = 3
}
