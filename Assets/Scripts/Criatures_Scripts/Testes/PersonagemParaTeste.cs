using Criatures2021;
using CustomizationSpace;
using FayvitBasicTools;
using FayvitLoadScene;
using FayvitMessageAgregator;
using FayvitSupportSingleton;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersonagemParaTeste : MonoBehaviour
{
    [SerializeField] private bool comecarSemKeyDjey;
    [SerializeField] private bool ignorarRestricoesDeKeyDjey;
    [SerializeField] private bool ativarCenaDeCesarEmMarjan;
    // Start is called before the first frame update
    void Start()
    {
        if (StaticInstanceExistence<IGameController>.SchelduleExistence(Start, this, () =>
        {
            return AbstractGameController.Instance;
        }))
        {
           

            SupportSingleton.Instance.InvokeOnCountFrame(() =>
            {
                CharacterManager c = FindObjectOfType<CharacterManager>();
                if (c == null)
                {
                    
                    var v = CustomizationSavedChars.listaDePersonagens;
                    int x = Random.Range(0, v.Count);
                    FazPersonagem(v[x], true, transform).GetComponent<CharacterManager>().IgnorarRestricoesDeKeyDjey=ignorarRestricoesDeKeyDjey;

                    if(!comecarSemKeyDjey)
                        AbstractGameController.Instance.MyKeys.MudaShift(KeyShift.hooliganKeyDjey, true);

                    AbstractGameController.Instance.MyKeys.MudaShift(KeyShift.CaesarFalaEmMarjan, !ativarCenaDeCesarEmMarjan);
                    AbstractGameController.Instance.MyKeys.MudaShift(KeyShift.permitidoKeyDjey,true);
                }
        }, 3);

    }
    }

    public static GameObject FazPersonagem(CustomizationContainerDates ccd,bool inTeste,Transform transform)
    {
        GameObject Ggg = CombinerSingleton.Instance.GetCombination(ccd);
        CharacterManager c = Ggg.AddComponent<CharacterManager>();
        c.InTeste = inTeste;
        c.transform.position = MelhoraInstancia3D.ProcuraPosNoMapa(transform.position);
        c.Ccd = ccd;
        c.enabled = false;
        


        if(SceneManager.GetSceneByName(NomesCenasEspeciais.ComunsDeFase.ToString()).isLoaded)
            SceneManager.MoveGameObjectToScene(Ggg, SceneManager.GetSceneByName(NomesCenasEspeciais.ComunsDeFase.ToString()));

        SupportSingleton.Instance.InvokeOnEndFrame(() =>
        {
            if (inTeste)
            {
                
            }
            else
            {
                c.Dados = new DadosDeJogador();//((Criatures2021.SaveDates)S).EssesDados;
                c.InicializarPet();
                CustomizationSavedChars.LoadSavedCharacters();
            }

            if (!GameObject.FindWithTag("MainCamera"))
            {
                MessageAgregator<MsgRequestCam>.Publish();
            }

            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                SetHeroCamera.Set(c.transform);

                c.enabled = true;
            });
        });

        AbstractGlobalController.Instance.Players = new List<IPlayersInGameDb>() {
                    new PlayersInGameDb()
                    {
                        Control = FayvitCommandReader.Controlador.teclado,
                        DbState = PlayerDbState.ocupadoLocal,
                        Manager = c
                    }
                    };

        AbstractGlobalController.Instance.Music.StartMusic(MusicDictionary.GetSceneMusic(
            StringForEnum.GetEnum(SceneManager.GetActiveScene().name,NomesCenas.acampamentoDaResistencia)
            ));

        return Ggg;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
