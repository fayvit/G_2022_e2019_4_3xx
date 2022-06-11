using FayvitBasicTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirculeEntrePontos : MonoBehaviour
{
    [SerializeField] private Transform[] pontos;
    [SerializeField] private float vel;
    [SerializeField] private int indiceAtual;
    [SerializeField] private int indiceAntigo;

    private float tempoDecorrido = 0;
    private Vector3 posOrigem;


    // Start is called before the first frame update
    void Start()
    {
        posOrigem = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        tempoDecorrido += Time.deltaTime;
        float distancia = Vector3.Distance(pontos[indiceAtual].position, posOrigem);
        transform.position = Vector3.Lerp(posOrigem, pontos[indiceAtual].position,vel*tempoDecorrido/distancia);

        if (Vector3.Distance(transform.position,pontos[indiceAtual].position) <= .1f)
        {
            indiceAntigo = indiceAtual;
            indiceAtual = ContadorCiclico.Contar(1, indiceAtual, pontos.Length);
            posOrigem = pontos[indiceAntigo].position;
            tempoDecorrido = 0;
        }
    }
}
