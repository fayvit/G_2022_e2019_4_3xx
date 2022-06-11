using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputTextManager : MonoBehaviour
{
    [SerializeField] private Text baseText;
    [SerializeField] private InputField input;
    [SerializeField] private Text confirmationBtnText;
    [SerializeField] private Text returnBtnText;
    [SerializeField] private Text confirmationLabel;
    [SerializeField] private Text returnLabel;

    private System.Action confirmationAction;
    private System.Action backAction;

    public void StartHud(
        System.Action confirm, 
        System.Action back, 
        string baseText,
        string confirmationBtnText="Confirmar",
        string returnBtnText="Descartar",
        string confirmationLabel="Press Enter",
        string returnLabel="Press Esc"
        )
    {
        gameObject.SetActive(true);
        this.baseText.text = baseText;
        this.confirmationBtnText.text = confirmationBtnText;
        this.returnBtnText.text = returnBtnText;
        this.confirmationLabel.text = confirmationLabel;
        this.returnLabel.text = returnLabel;

        confirmationAction += confirm;
        backAction += back;
    }

    public void FinishHud()
    {
        gameObject.SetActive(false);
        confirmationAction = null;
        backAction = null;
    }

    public void ConfirmationAction()
    {
        confirmationAction?.Invoke();
    }

    public void BackAction()
    {
        backAction?.Invoke();
    }

    public string TextContent
    {
        get => string.IsNullOrEmpty(input.text)?System.Guid.NewGuid().ToString():input.text;
    }
}
