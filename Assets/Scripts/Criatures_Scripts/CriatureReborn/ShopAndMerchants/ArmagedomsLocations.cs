using UnityEngine;
using System.Collections.Generic;
using FayvitLoadScene;
using TextBankSpace;

[System.Serializable]
public class ArmagedomsLocations
{
    private static Dictionary<IndiceDeArmagedoms, VisitasParaArmagedom> l = new Dictionary<IndiceDeArmagedoms, VisitasParaArmagedom>();

    public static Dictionary<IndiceDeArmagedoms, VisitasParaArmagedom> L
    {
        get { return l; }
    }
}

public class VisitasParaArmagedom
{
    private float endX = 0;
    private float endY = 0;
    private float endZ = 0;
    public NomesCenas[] nomeDasCenas;

    public Vector3 Endereco
    {
        get
        {
            return new Vector3(endX, endY, endZ);
        }

        set
        {
            Vector3 V = value;
            endX = V.x;
            endY = V.y;
            endZ = V.z;
        }
    }

    public static string NomeEmLinguas(IndiceDeArmagedoms i)
    {
        return TextBank.RetornaListaDeTextoDoIdioma(TextKey.armagedomNames)[(int)Mathf.Log((int)i,2)];
    }
}

[System.Flags]
public enum IndiceDeArmagedoms
{
    // Registrar no nome em linguas
    None=0,
    acampamentoDaResistencia = 1<<1,
    segundoArmagedom = 1<<2
}
