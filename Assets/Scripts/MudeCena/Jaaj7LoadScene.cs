using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitMessageAgregator;
using FayvitBasicTools;
using UnityEngine.SceneManagement;
using Criatures2021Hud;
using FayvitLoadScene;

public class Jaaj7LoadScene : MonoBehaviour
{
    [SerializeField] private Animator A;
    [SerializeField] private GameObject[] ligaveis;
    [SerializeField] private GameObject kamera;

    [SerializeField] private TutoInfoMessage infoMessage;


    public void CadeCamera()
    {
        kamera.SetActive(true);
        OnJaajRequestEnter(new MsgJaajEntrando());
        MessageAgregator<FadeOutStart>.AddListener(OnFadeOuTStart);
    }

    private void OnFadeOuTStart(FadeOutStart obj)
    {
        OnJaajRequestExit(new MsgJaajSaindo());
        FayvitSupportSingleton.SupportSingleton.Instance.InvokeOnEndFrame(() =>
        {
            MessageAgregator<FadeOutStart>.RemoveListener(OnFadeOuTStart);
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetSceneByName(NomesCenasEspeciais.menuInicial.ToString()).isLoaded||Camera.main==null)
        {
            CadeCamera();
        }
        
        int num = System.Enum.GetValues(typeof(InfoMessageType)).Length;

        infoMessage.FillMessage((InfoMessageType)Random.Range(0, num),true);

        MessageAgregator<MsgJaajEntrando>.AddListener(OnJaajRequestEnter);
        MessageAgregator<MsgJaajSaindo>.AddListener(OnJaajRequestExit);
        MessageAgregator<MsgAnimationPointCheck>.AddListener(OnReceiveAnimationPoint);
    }

    private void OnDestroy()
    {
        MessageAgregator<MsgJaajEntrando>.RemoveListener(OnJaajRequestEnter);
        MessageAgregator<MsgJaajSaindo>.RemoveListener(OnJaajRequestExit);
        MessageAgregator<MsgAnimationPointCheck>.RemoveListener(OnReceiveAnimationPoint);
    }

    private void OnReceiveAnimationPoint(MsgAnimationPointCheck obj)
    {
        if (obj.sender == gameObject && obj.extraInfo== "Jaaj_Saindo")
        {
            SceneManager.UnloadSceneAsync(FayvitLoadScene.NomesCenasEspeciais.CenaDeCarregamento.ToString());
        }
    }

    private void OnJaajRequestExit(MsgJaajSaindo obj)
    {
        A.SetTrigger("sair");
        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
        {
            sfxId = FayvitSounds.SoundEffectID.Book1
        });
    }

    private void OnJaajRequestEnter(MsgJaajEntrando obj)
    {
        A.SetTrigger("entrar");

        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
        {
            sfxId = FayvitSounds.SoundEffectID.Evasion1
        });

        for (int i = 0; i < ligaveis.Length; i++)
            ligaveis[i].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public struct MsgJaajEntrando : IMessageBase { }
public struct MsgJaajSaindo : IMessageBase { }

