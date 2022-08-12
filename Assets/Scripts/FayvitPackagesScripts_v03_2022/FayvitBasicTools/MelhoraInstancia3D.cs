using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelhoraInstancia3D
{
    public static Vector3 PosParaDeslocamento(Vector3 pontoAlvo, Vector3 posAtual)
    {
        pontoAlvo = ProcuraPosNoMapa(pontoAlvo);
        pontoAlvo = PosEmparedado(pontoAlvo, posAtual);
        return pontoAlvo;
    }

    public static Vector3 PosEmparedado(Vector3 pontoAlvo, Vector3 posAtual)
    {
        Vector3 retorno = pontoAlvo;
        pontoAlvo += Vector3.up;
        posAtual += Vector3.up;
        RaycastHit hit;
        if (Physics.Linecast(posAtual, pontoAlvo, out hit))
        {
            if (Vector3.Angle(hit.normal, Vector3.ProjectOnPlane(hit.normal, Vector3.up)) < 5)
            {
                retorno = ProcuraPosNoMapa(hit.point + hit.normal);
                Debug.LogWarning("[melhoraPos] angulo Menor que 10 " + hit.collider.name);
            }
            else
                retorno = hit.point + hit.normal;   

            //Debug.Log(hit.collider.gameObject+" o angulo e "+Vector3.Angle(hit.normal,oQProcura-oParado));
        }

        return retorno;
    }

    public static Vector3 ProcuraPosNoMapa(Vector3 pontoAlvo,float customVarDir=.1f)
    {
        Vector3 retorno = pontoAlvo;
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(pontoAlvo, Vector3.up, out hit))
        //if (hit.transform.name == terra)
        {
            Debug.Log("down catch");
            retorno = hit.point;


        }

        if (Physics.Raycast(pontoAlvo + customVarDir * Vector3.up, Vector3.down, out hit))
        // if (hit.transform.name == terra)
        {
            Debug.Log("up catch");
            retorno = hit.point;
        }

        return retorno;
    }
}

