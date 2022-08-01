using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitUI;
using FayvitSave;

[System.Serializable]
public class LoadMenuManager : InteractiveUiBase
{
    private System.Action<int> acao;
    private System.Action<int> acaoDelete;
    private PropriedadesDeSave[] lista;
    

    public void StartHud(
        System.Action<int> acao,
        System.Action<int> acaoDelete,
        PropriedadesDeSave[] lista
        )
    {
        
        //this.opcoes = txDeOpcoes;
        this.acao += acao;
        this.acaoDelete += acaoDelete;
        this.lista = lista;
        StartHud(lista.Length, ResizeUiType.horizontal);
    }

    public override void SetContainerItem(GameObject G, int indice)
    {
        G.GetComponent<LoadOption>().SetarBotao(acao, acaoDelete, lista[indice], indice);
    }

    protected override void AfterFinisher()
    {
        acao = null;
        acaoDelete = null;
    }
}
