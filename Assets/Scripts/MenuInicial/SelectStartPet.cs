using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FayvitCommandReader;
using FayvitBasicTools;
using FayvitLoadScene;
using Criatures2021;
using FayvitSave;
using CustomizationSpace;

public class SelectStartPet : MonoBehaviour
{
    [SerializeField] private Text nameTxt;
    [SerializeField] private Text type;
    [SerializeField] private Text description;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private float totalLerpTime;
    private Vector3[] pos;
    private Vector3Int[] indexVect = new Vector3Int[3] {
        new Vector3Int(0, 1, 2),
        new Vector3Int(1, 2, 0),
        new Vector3Int(2, 0, 1)
    };
    private float tempoDecorrido = 0;
    private int chooseIndex = 0;
    private bool confirmationOpened;

    private PetName[] names = new PetName[3] { PetName.Florest, PetName.PolyCharm, PetName.Xuash };



    public ICommandReader Commander => CommandReader.GetCR(AbstractGlobalController.Instance.Control);

    // Start is called before the first frame update
    void Start()
    {
        AbstractGlobalController.Instance.Music.StartMusic(FayvitSounds.NameMusic.Brasileirinho);
        pos = new Vector3[prefabs.Length];

        for (int i = 0; i < pos.Length; i++)
            pos[i] = prefabs[i].transform.position;

        InsertPetInfos();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (confirmationOpened)
        {
            int change = Commander.GetIntTriggerDown(CommandConverterString.moveH);
            bool selectButton = Commander.GetButtonDown(CommandConverterInt.starterMenuConfirm) || Commander.GetButtonDown(CommandConverterInt.confirmButton);
            AbstractGlobalController.Instance.Confirmation.ThisUpdate(change!=0, selectButton, false);
        }
        else
        {
            int change = Commander.GetIntTriggerDown(CommandConverterString.moveH);
            bool selectButton = Commander.GetButtonDown(CommandConverterInt.starterMenuConfirm) || Commander.GetButtonDown(CommandConverterInt.confirmButton);


            if (change != 0)
            {
                chooseIndex = ContadorCiclico.Contar(change, chooseIndex, 3);
                tempoDecorrido = 0;
                InsertPetInfos();
            }
            else if (selectButton)
            {
                AbstractGlobalController.Instance.Confirmation.StartConfirmationPanel(SimQueroEsse,AindaNao,"Tem certeza que quer escolher esseCriature?");
                confirmationOpened = true;
            }

            tempoDecorrido += Time.deltaTime;

            if (tempoDecorrido < totalLerpTime)
            {
                for (int i = 0; i < prefabs.Length; i++)
                    prefabs[indexVect[chooseIndex][i]].transform.position
                        = Vector3.Lerp(prefabs[indexVect[chooseIndex][i]].transform.position, pos[i], tempoDecorrido / totalLerpTime);
            }
        }
    }

    private void AindaNao()
    {
        confirmationOpened = false;
    }

    private void SimQueroEsse()
    {
        enabled = false;
        AbstractGlobalController.Instance.FadeV.StartFadeOutWithAction(() =>
        {
            SceneManager.LoadSceneAsync(NomesCenas.acampamentoDaResistencia.ToString(), LoadSceneMode.Additive);
            SceneManager.sceneLoaded += OnStartSceneLoaded;
        });
    }

    private void OnStartSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        CharacterManager C = GameObject.FindWithTag("Player").GetComponent<CharacterManager>();
        C.GetComponent<CharacterController>().Move(new Vector3(450, 1, 515) - C.transform.position);
        //C.transform.position = new Vector3(450, 1, 515);
        C.GetComponent<CharacterController>().enabled = true;

        PetBase P = new PetBase(names[chooseIndex],1);

        CustomizationSavedChars.LoadSavedCharacters();
        SceneManager.UnloadSceneAsync(NomesCenasEspeciais.SelectCriature.ToString());
        SceneManager.sceneLoaded -= OnStartSceneLoaded;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(NomesCenas.acampamentoDaResistencia.ToString()));
        
        SetHeroCamera.Set(C.transform);
        AbstractGlobalController.Instance.FadeV.StartFadeInWithAction(() => {
            AbstractGlobalController.Instance.Music.StartMusic(FayvitSounds.NameMusic.choroChorandoParaPaulinhoNogueira);
            C.enabled = true;
            FayvitSupportSingleton.SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                C.Dados.CriaturesAtivos.Add(P);
                C.InicializarPet();

                SaveDatesManager.SalvarAtualizandoDados(new Criatures2021.CriaturesSaveDates());

                C.Dados.Livro.AdicionaVisto(P.NomeID);
                C.Dados.Livro.AdicionaCapturado(P.NomeID);
            });
        });
    }

    void InsertPetInfos()
    {
        PetBase P = PetFactory.GetPet(names[chooseIndex]);
        nameTxt.text = P.GetNomeEmLinguas;
        type.text = TypeNameInLanguages.Get(P.PetFeat.meusTipos[0]);
        description.text = TextBankSpace.TextBank.GetPetDescription(P.NomeID);

        FayvitUI.ChangeToBestFitOnExtrapolate.Verify(nameTxt);
        FayvitUI.ChangeToBestFitOnExtrapolate.Verify(type);
        FayvitUI.ChangeToBestFitOnExtrapolate.Verify(description);

    }
}
