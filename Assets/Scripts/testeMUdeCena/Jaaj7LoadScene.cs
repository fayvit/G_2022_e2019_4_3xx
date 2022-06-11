using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitMessageAgregator;
using FayvitBasicTools;
using UnityEngine.SceneManagement;

public class Jaaj7LoadScene : MonoBehaviour
{
    [SerializeField] private Animator A;
    [SerializeField] private GameObject[] ligaveis;
    // Start is called before the first frame update
    void Start()
    {
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
            SceneManager.UnloadSceneAsync(FayvitLoadScene.NomesCenasEspeciais.LoadScene.ToString());
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

