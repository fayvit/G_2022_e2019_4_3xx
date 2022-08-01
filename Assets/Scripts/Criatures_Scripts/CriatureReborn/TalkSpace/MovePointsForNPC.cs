using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePointsForNPC : MonoBehaviour
{
    [SerializeField] private Transform[] pontosAlcancaveis;

    public Transform UmPontoSorteado => pontosAlcancaveis[Random.Range(0, pontosAlcancaveis.Length)];

}
