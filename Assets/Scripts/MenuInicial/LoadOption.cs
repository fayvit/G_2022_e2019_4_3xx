using UnityEngine;
using UnityEngine.UI;
using FayvitUI;
using FayvitSave;
using TextBankSpace;
using FayvitMessageAgregator;

public class LoadOption : AnOption
{
    [SerializeField] private Text nomeDoSave;
    [SerializeField] private Text labelDoSave;

    private System.Action<int> acaoLoad;
    private System.Action<int> acaoDelete;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetarBotao(System.Action<int> acaoLoad, System.Action<int> acaoDelete, PropriedadesDeSave prop, int indice)
    {
        this.acaoLoad += acaoLoad;
        this.acaoDelete += acaoDelete;
        nomeDoSave.text = prop.nome;
        labelDoSave.text = "Save " + (indice + 1);
    }

    public void BotaoCarregar()
    {
        acaoLoad(transform.GetSiblingIndex() - 1);
    }

    public void BotaoExcluir()
    {
        MessageAgregator<MsgDeleteSaveClicked>.Publish();
        //BtnsManager.DesligarBotoes(transform.parent.parent.parent.gameObject);
        //InitialSceneManager.i.Confirmacao.AtivarPainelDeConfirmacao(ExcluirSim, VoltarAoSave,
        //    string.Format(TextBank.RetornaFraseDoIdioma(TextKey.certezaExcluir), nomeDoSave.text));
    }

    public void ExcluirSim()
    {
        MessageAgregator<MsgDeleteSaveConfirmed>.Publish();
        //BtnsManager.ReligarBotoes(transform.parent.parent.parent.gameObject);
        //InitialSceneManager.i.EstadoDeRetornandoAoSave();
        acaoDelete(transform.GetSiblingIndex() - 1);

    }

    public void VoltarAoSave()
    {
        
        MessageAgregator<MsgReturnToSaves>.Publish();
        //BtnsManager.ReligarBotoes(transform.parent.parent.parent.gameObject);
        //InitialSceneManager.i.EstadoDeRetornandoAoSave();
    }
}

public struct MsgDeleteSaveClicked : IMessageBase { }
public struct MsgDeleteSaveConfirmed : IMessageBase { }
public struct MsgReturnToSaves : IMessageBase { }
