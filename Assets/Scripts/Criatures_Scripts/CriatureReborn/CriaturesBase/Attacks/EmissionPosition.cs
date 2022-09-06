using UnityEngine;
using System.Collections;
using Criatures2021;

public class EmissionPosition
{
    public static Vector3 Get(GameObject G, AttackNameId nomeGolpe)
    {
        PetAttackDb gP = PetAttackDb.RetornaGolpePersonagem(G, nomeGolpe);
    return G.transform.Find(gP.Colisor.osso).position
            + G.transform.forward * (gP.DistanciaEmissora)
                + Vector3.up * gP.AcimaDoChao;
    }
}