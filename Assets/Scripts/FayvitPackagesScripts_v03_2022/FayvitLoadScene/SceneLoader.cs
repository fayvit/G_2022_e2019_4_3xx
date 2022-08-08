using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using FayvitBasicTools;
using FayvitSave;
using FayvitSupportSingleton;
using FayvitMessageAgregator;

namespace FayvitLoadScene
{
    [System.Serializable]
    public class SceneLoader : MonoBehaviour, ISceneLoader
    {
        //[SerializeField] private LoadBar loadBar;

        private SaveDates S;
        private AsyncOperation[] a2;
        private System.Action acaoFinalizadora;
        private FasesDoLoad fase = FasesDoLoad.emEspera;
        private bool podeIr = false;
        private float tempo = 0;
        private int indiceDoJogo = 0;

        private int aSerCarregado = 0;
        private int numCarregador = 0;

        private const float tempoMin = 0.25f;

        private enum FasesDoLoad
        {
            emEspera,
            carregando,
            //escurecendo,
            //clareando,
            eventInProgress
        }

        public static void CarergarMenuPrincipal()
        {

            AbstractGlobalController.Instance.FadeV.StartFadeIn();
            SceneManager.LoadScene(NomesCenasEspeciais.menuInicial.ToString());
            AbstractGlobalController.Instance.EmTeste = false;
            Destroy(AbstractGameController.Instance.ThisGameObject);
        }

        public static void IniciarCarregamento(int slote, System.Action acaoFinalizadora = null)
        {
            Physics2D.gravity = Vector2.zero;
            GameObject G = new GameObject();
            SceneLoader loadScene = G.AddComponent<SceneLoader>();


            loadScene.CenaDoCarregamento(slote, acaoFinalizadora);

        }

        public void CenaDoCarregamento(int indice, System.Action acaoFinalizadora = null)
        {

            DontDestroyOnLoad(gameObject);
            indiceDoJogo = indice;
            this.acaoFinalizadora = acaoFinalizadora;

            if (SceneManager.GetSceneByName(NomesCenasEspeciais.ComunsDeFase.ToString()).isLoaded)
            {
                AditiveLoadScene(NomesCenasEspeciais.CenaDeCarregamento);
                SceneManager.sceneLoaded += IniciarCarregamentoComComuns;
                //Camera.main.farClipPlane = 0.4f;
                Time.timeScale = 0;
            }
            else
            {

                AditiveLoadScene(NomesCenasEspeciais.CenaDeCarregamento);
                SceneManager.sceneLoaded += IniciarCarregamento;
            }
        }

        private void IniciarCarregamento(Scene arg0, LoadSceneMode arg1)
        {
            if (SceneManager.GetSceneByName(NomesCenasEspeciais.menuInicial.ToString()).isLoaded)
            {
                SceneManager.UnloadSceneAsync(NomesCenasEspeciais.menuInicial.ToString());
            }

            //loadBar = FindObjectOfType<LoadBar>();

            AditiveLoadScene(NomesCenasEspeciais.ComunsDeFase);
            SceneManager.sceneLoaded -= IniciarCarregamento;
            SceneManager.sceneLoaded += CarregouComuns;
        }

        private void CarregouComuns(Scene arg0, LoadSceneMode arg1)
        {
            ComunsCarregado();
        }

        private void IniciarCarregamentoComComuns(Scene arg0, LoadSceneMode arg1)
        {

            SceneManager.sceneLoaded -= IniciarCarregamentoComComuns;
            //loadBar = FindObjectOfType<LoadBar>();
            ComunsCarregado();
        }

        void ComunsCarregado()
        {
            if (StaticInstanceExistence<IGameController>.SchelduleExistence(ComunsCarregado, this, () => {
                return AbstractGameController.Instance;
            }))
            //if (ExistenciaDoController.AgendaExiste(ComunsCarregado, this))
            {
                SceneManager.sceneLoaded -= CarregouComuns;
                SceneManager.sceneLoaded += SetarCenaPrincipal;
                SceneManager.sceneLoaded += TatudoCarregado;

                if (SaveDatesManager.s.SavedGames.Count > indiceDoJogo)
                    S = SaveDatesManager.s.SavedGames[indiceDoJogo];
                else
                    S = null;

                if (S == null || S.VariaveisChave == null || S.VariaveisChave.CenasAtivas == null)
                {
                    fase = FasesDoLoad.carregando;
                    aSerCarregado = 1;
                    a2 = new AsyncOperation[1];
                    a2[0] = AditiveLoadScene(NomesCenas.cenaTeste);

                }
                else
                {
                    
                    NomesCenas[] N2 = DescarregarCenasDesnecessarias(S.VariaveisChave.CenasAtivas.ToArray());

                    numCarregador = 0;
                    aSerCarregado = N2.Length;


                    for (int i = 0; i < N2.Length; i++)
                    {
                        SceneManager.UnloadSceneAsync(N2[i].ToString());
                    }

                    FuncaoCarregadora();
                }
            }
        }

        public static NomesCenas[] PegueAsCenasPorCarregar(NomesCenas[] N)
        {

            System.Collections.Generic.List<NomesCenas> retorno = new System.Collections.Generic.List<NomesCenas>();
            for (int i = 0; i < N.Length; i++)
            {
                //Debug.Log("nomes cenas por carregar: "+N[i]);
                if (!SceneManager.GetSceneByName(N[i].ToString()).isLoaded)
                {
                    retorno.Add(N[i]);
                }
            }

            return retorno.ToArray();
        }

        void FuncaoCarregadora()
        {
            fase = FasesDoLoad.carregando;
            NomesCenas[] N = PegueAsCenasPorCarregar(S.VariaveisChave.CenasAtivas.ToArray());
            Debug.Log("numero de cenas carregando: " + N.Length);

            aSerCarregado = N.Length;

            a2 = new AsyncOperation[N.Length];
            for (int i = 0; i < N.Length; i++)
            {
                a2[i] = SceneManager.LoadSceneAsync(N[i].ToString(), LoadSceneMode.Additive);

            }



            if (N.Length == 0 || SceneManager.GetSceneByName(AbstractGameController.Instance.MyKeys.CenaAtiva.ToString()).isLoaded)
            {
                SetarCenaPrincipal(SceneManager.GetSceneByName(AbstractGameController.Instance.MyKeys.CenaAtiva.ToString()), LoadSceneMode.Single);
                TatudoCarregado();
            }
            Time.timeScale = 0;
        }

        void SetarCenaPrincipal(Scene scene, LoadSceneMode mode)
        {
            if (S != null&&S.VariaveisChave!=null&&S.VariaveisChave.CenasAtivas!=null)
            {
                //Debug.Log(S.VariaveisChave.CenaAtiva.ToString() + " : " + scene.name);
                if (scene.name == S.VariaveisChave.CenaAtiva.ToString())
                {
                    PedirAtualizacaoDeDados(scene);
                }
            }
            else if (scene.name == NomesCenas.cenaTeste.ToString())
            {
                PedirAtualizacaoDeDados(scene);
            }
        }

        void PedirAtualizacaoDeDados(Scene scene)
        {
            InvocarSetScene(scene, S);
            SceneManager.sceneLoaded -= SetarCenaPrincipal;


            SaveDatesManager.s.IndiceDoJogoAtualSelecionado = indiceDoJogo;
        }


        static IEnumerator setarScene(Scene scene, SaveDates S)
        {
            yield return new WaitForSeconds(0.5f);
            InvocarSetScene(scene, S);
        }

        public static void InvocarSetScene(Scene scene, SaveDates S)
        {

            SceneManager.SetActiveScene(scene);


            if (SceneManager.GetActiveScene() != scene)
                SupportSingleton.Instance.InvokeInSeconds(() => { InvocarSetScene(scene, S); }, 0.5f);
            else
                //EventAgregator.Publish(new StandardSendGameEvent(EventKey.requestToFillDates, S));//ComoPode();
                MessageAgregator<MsgUpdateDates>.Publish(new MsgUpdateDates() { saveDates = S });

        }

        private void TatudoCarregado(Scene arg0, LoadSceneMode arg1)
        {
            TatudoCarregado();
        }

        private void TatudoCarregado()
        {
            numCarregador++;
            if (aSerCarregado <= numCarregador)
            {
                podeIr = true;
                SceneManager.sceneLoaded -= TatudoCarregado;
            }
        }

        public static NomesCenas[] DescarregarCenasDesnecessarias(NomesCenas[] N)
        {
            System.Collections.Generic.List<NomesCenas> retorno = new System.Collections.Generic.List<NomesCenas>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene S = SceneManager.GetSceneAt(i);

                //Debug.Log("tentativa de string para enum: " + StringParaEnum.ObterEnum<NomesCenasEspeciais>(S.name));

                if (S.isLoaded && S.name != NomesCenasEspeciais.ComunsDeFase.ToString()
                    && S.name != NomesCenasEspeciais.LoadScene.ToString()
                    && S.name != NomesCenasEspeciais.CenaDeCarregamento.ToString())
                {
                    if (S.isLoaded)
                    {
                        bool foi = false;
                        for (int j = 0; j < N.Length; j++)
                        {
                            if (S.name == N[j].ToString())
                                foi = true;
                        }

                        if (!foi)
                            retorno.Add(StringForEnum.GetEnum<NomesCenas>(S.name));
                    }
                }
            }

            return retorno.ToArray();
        }

        AsyncOperation AditiveLoadScene(NomesCenasEspeciais n)
        {
            return SceneManager.LoadSceneAsync(n.ToString(), LoadSceneMode.Additive);
        }

        AsyncOperation AditiveLoadScene(NomesCenas n)
        {
            return SceneManager.LoadSceneAsync(n.ToString(), LoadSceneMode.Additive);
        }

        public void Update()
        {
            switch (fase)
            {
                case FasesDoLoad.carregando:

                    tempo += Time.fixedDeltaTime;

                    float progresso = 0;
                    if (a2.Length > 0)
                    {
                        for (int i = 0; i < a2.Length; i++)
                        {
                            progresso += a2[i].progress;
                        }

                        progresso /= a2.Length;

                    }
                    else
                        progresso = 1;

                    //loadBar.ValorParaBarra(Mathf.Min(progresso, tempo / tempoMin, 1));

                    if (podeIr && tempo >= tempoMin)
                    {
                        GameObject go = GameObject.Find("EventSystem");
                        if (go)
                            SceneManager.MoveGameObjectToScene(go, SceneManager.GetSceneByName(NomesCenasEspeciais.ComunsDeFase.ToString()));

                        AbstractGlobalController.Instance.FadeV.StartFadeOut();
                        MessageAgregator<FadeOutComplete>.AddListener(OnFadeOutComplete);
                        //EventAgregator.AddListener(EventKey.fadeOutComplete, OnFadeOutComplete);
                        fase = FasesDoLoad.eventInProgress;

                    }

                    break;
            }
        }

        private void OnFadeOutComplete(FadeOutComplete obj)
        {
            AbstractGlobalController.Instance.FadeV.StartFadeIn();
            MessageAgregator<FadeInComplete>.AddListener(OnFadeInComplete);
            //EventAgregator.AddListener(EventKey.fadeInComplete, );

            Debug.Log("cena inalcançavel: " + AbstractGameController.Instance.MyKeys.CenaAtiva.ToString());
            //SceneManager.SetActiveScene(
            //   SceneManager.GetSceneByName(AbstractGameController.Instance.MyKeys.CenaAtiva.ToString()));

            //SaveDatesManager.SalvarAtualizandoDados(new Criatures2021.SaveDates());

            fase = FasesDoLoad.eventInProgress;

            SceneManager.UnloadSceneAsync(NomesCenasEspeciais.CenaDeCarregamento.ToString());

            //GameController.g.ModificacoesDaCena();
            Time.timeScale = 1;
            Physics2D.gravity = new Vector2(0, -0.8f);

            if (!GameObject.FindWithTag("MainCamera"))
            {
                MessageAgregator<MsgRequestCam>.Publish();
            }

            Debug.Log("cena ativa para musica: " + SceneManager.GetActiveScene().name);
            NomesCenas n = StringForEnum.GetEnum<NomesCenas>(SceneManager.GetActiveScene().name);
            MessageAgregator<MsgChangeMusicIfNew>.Publish(new MsgChangeMusicIfNew()
            {
                nmcvc = Criatures2021.MusicDictionary.GetSceneMusic(n)
            });

            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                if (!FindObjectOfType<Criatures2021.CharacterManager>() && S != null && ((Criatures2021.SaveDates)S).Ccd != null)
                {
                    GameObject Ggg = CombinerSingleton.Instance.GetCombination(((Criatures2021.SaveDates)S).Ccd);
                    Criatures2021.CharacterManager c = Ggg.AddComponent<Criatures2021.CharacterManager>();
                    c.InTeste = false;
                    c.transform.position = S.Posicao;
                    c.transform.rotation = S.Rotacao;
                    c.Ccd = ((Criatures2021.SaveDates)S).Ccd;

                    SceneManager.MoveGameObjectToScene(Ggg, SceneManager.GetSceneByName(NomesCenasEspeciais.ComunsDeFase.ToString()));

                    SupportSingleton.Instance.InvokeOnEndFrame(() =>
                    {
                        c.Dados = (Criatures2021.DadosDeJogador)S.Dados;//((Criatures2021.SaveDates)S).EssesDados;
                        c.InicializarPet();
                        Criatures2021.MyGameController.LoadSavedCharacters();
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

        //private void OnFadeOutComplete(IGameEvent obj)
        //{
        //    GlobalController.g.FadeV.IniciarFadeIn();
        //    EventAgregator.AddListener(EventKey.fadeInComplete, OnFadeInComplete);

        //    SceneManager.SetActiveScene(
        //       SceneManager.GetSceneByName(GameController.g.MyKeys.CenaAtiva.ToString()));

        //    SaveDatesManager.SalvarAtualizandoDados();

        //    fase = FasesDoLoad.eventInProgress;

        //    SceneManager.UnloadSceneAsync(NomesCenasEspeciais.CenaDeCarregamento.ToString());

        //    //GameController.g.ModificacoesDaCena();
        //    Time.timeScale = 1;
        //    Physics2D.gravity = new Vector2(0, -0.8f);

        //}

        private void OnFadeInComplete(FadeInComplete obj)
        {
            Physics2D.gravity = new Vector2(0, -9.8f);

            Destroy(gameObject);

            InvokeAcaoFinalizadora();
        }

        void InvokeAcaoFinalizadora()
        {
            if (acaoFinalizadora != null)
            {
                acaoFinalizadora();
                acaoFinalizadora = null;
            }
        }


        private void OnDestroy()
        {
            MessageAgregator<FadeInComplete>.RemoveListener(OnFadeInComplete);
            MessageAgregator<FadeOutComplete>.RemoveListener(OnFadeOutComplete);
            //EventAgregator.RemoveListener(EventKey.fadeInComplete, OnFadeInComplete);
            //EventAgregator.RemoveListener(EventKey.fadeOutComplete, OnFadeOutComplete);
        }
    }

    public enum NomesCenas
    {
        teesteinicial = -1,
        nula = 0,
        cenaTeste,
        CenaTesteNumberTwo,
        TutoScene,
        CenaTesteNumber3,
        MAST_Teste,
        acampamentoDaResistencia,
        cavernaAcampamentoKatids,
        planicieDeKatids
    }

    public enum NomesCenasEspeciais
    {
        tentaivaDeMenos1 = -1,
        valorDefaultNoZero,
        ComunsDeFase,
        CenaDeCarregamento,
        menuInicial,
        LoadScene,
        cutsceneIntro,
        SelectCriature
    }

    public struct MsgUpdateDates : IMessageBase
    {
        public SaveDates saveDates;
    }
}