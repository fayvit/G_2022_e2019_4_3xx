using CustomizationSpace;
using FayvitBasicTools;
using FayvitLoadScene;
using FayvitMessageAgregator;
using FayvitSupportSingleton;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Criatures2021
{
    public class SceneLoader : SceneLoaderBase
    {
        new public static void IniciarCarregamento(int slote, System.Action acaoFinalizadora = null)
        {
            Physics2D.gravity = Vector2.zero;
            GameObject G = new GameObject();
            SceneLoader loadScene = G.AddComponent<SceneLoader>();


            loadScene.CenaDoCarregamento(slote, acaoFinalizadora);

        }

        protected override void IniciarCarregamento(Scene arg0, LoadSceneMode arg1)
        {
            if (SceneManager.GetSceneByName(NomesCenasEspeciais.menuInicial.ToString()).isLoaded)
            {
                FindObjectOfType<Jaaj7LoadScene>().CadeCamera();
                SceneManager.UnloadSceneAsync(NomesCenasEspeciais.menuInicial.ToString());
            }

            base.IniciarCarregamento(arg0, arg1);
        }
        protected override void CarregouComuns(Scene arg0, LoadSceneMode arg1)
        {
            PersonagemParaTeste p = FindObjectOfType<PersonagemParaTeste>();
            if (p)
                p.gameObject.SetActive(false);

            base.CarregouComuns(arg0, arg1);
        }

        protected override void OnFadeOutComplete(FadeOutComplete obj)
        {
            base.OnFadeOutComplete(obj);

            if (!GameObject.FindWithTag("MainCamera"))
            {
                MessageAgregator<MsgRequestCam>.Publish();
            }

            Debug.Log("cena ativa para musica: " + SceneManager.GetActiveScene().name);
            NomesCenas n = StringForEnum.GetEnum<NomesCenas>(SceneManager.GetActiveScene().name);

            MessageAgregator<MsgChangeMusicIfNew>.Publish(new MsgChangeMusicIfNew()
            {
                nmcvc = MusicDictionary.GetSceneMusic(n)
            });

            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                if (!FindObjectOfType<CharacterManager>() && S != null && ((CriaturesSaveDates)S).Ccd != null)
                {
                    GameObject Ggg = CombinerSingleton.Instance.GetCombination(((CriaturesSaveDates)S).Ccd);
                    CharacterManager c = Ggg.AddComponent<CharacterManager>();
                    c.InTeste = false;
                    c.transform.position = S.Posicao;
                    c.transform.rotation = S.Rotacao;
                    c.Ccd = ((CriaturesSaveDates)S).Ccd;

                    SceneManager.MoveGameObjectToScene(Ggg, SceneManager.GetSceneByName(NomesCenasEspeciais.ComunsDeFase.ToString()));

                    SupportSingleton.Instance.InvokeOnEndFrame(() =>
                    {
                        c.Dados = (DadosDeJogador)S.Dados;//((Criatures2021.SaveDates)S).EssesDados;
                        c.InicializarPet();


                        //Debug.Log("Esse é o número de criatures ativos: " + c.Dados.CriaturesAtivos.Count);
                        //Recoloca status nos criatures da reserva
                        SupportSingleton.Instance.InvokeOnEndFrame(() =>
                        {
                            for (int i = 0; i < c.Dados.CriaturesAtivos.Count; i++)
                            {
                                StatusReplacer.ColocarStatus(c.Dados.CriaturesAtivos[i], c.ActivePet);
                            }
                        });


                        CustomizationSavedChars.LoadSavedCharacters();
                    });

                    AbstractGlobalController.Instance.Players = new System.Collections.Generic.List<IPlayersInGameDb>() {
                    new PlayersInGameDb()
                    {
                        Control = FayvitCommandReader.Controlador.teclado,
                        DbState = PlayerDbState.ocupadoLocal,
                        Manager = c
                    }
                        };

                    SetHeroCamera.Set(c.transform);
                    //MessageAgregator<Criatures2021.MsgChangeToHero>.Publish(new Criatures2021.MsgChangeToHero()
                    //{
                    //    myHero = Ggg
                    //});
                }
            });
        }
    }
}