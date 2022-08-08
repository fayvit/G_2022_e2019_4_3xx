using Criatures2021;
using FayvitBasicTools;
using FayvitLoadScene;
using FayvitMessageAgregator;
using FayvitSupportSingleton;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersonagemParaTeste : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CharacterManager c = FindObjectOfType<CharacterManager>();

        if (c == null)
        {
            if (StaticInstanceExistence<IGameController>.SchelduleExistence(Start, this, () =>
            {
                return AbstractGameController.Instance;
            }))
            {
                var v = MyGameController.listaDePersonagens;
                int x = Random.Range(0, v.Count);
                FazPersonagem(v[x], true,transform);
            }
        }
    }

    public static GameObject FazPersonagem(CustomizationContainerDates ccd,bool inTeste,Transform transform)
    {
        GameObject Ggg = CombinerSingleton.Instance.GetCombination(ccd);
        CharacterManager c = Ggg.AddComponent<CharacterManager>();
        c.InTeste = inTeste;
        c.transform.position = transform.position;
        c.Ccd = ccd;
        c.enabled = false;



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
                MyGameController.LoadSavedCharacters();
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
