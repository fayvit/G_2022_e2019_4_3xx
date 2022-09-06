using System.Collections;
using UnityEngine;
using FayvitMessageAgregator;
using FayvitBasicTools;
using FayvitSupportSingleton;
using FayvitLoadScene;
using Spawns;
using UnityEngine.SceneManagement;
using Criatures2021;
using FayvitSave;
using CustomizationSpace;

namespace ChangeScene
{
    public class MudeCena : MonoBehaviour
    {

        [SerializeField] private NomesCenas[] nomeCenasParaCarregar;
        [SerializeField] private SpawnID idPos;

        private bool carregouCena;
        private bool terminouFadeOut;
        private bool iniciouLoad;

        private int numCenasParaCarregar = 0;
        private int contCenasCaregadas = 0;
        private NomesCenas cenaAtiva;
        private Transform personagem;

        public static void MudarCena(NomesCenas[] cenasParaCarregar,SpawnID idPos,Transform personagem)
        {
            GameObject G = new GameObject();
            MudeCena mc = G.AddComponent<MudeCena>();
            mc.nomeCenasParaCarregar = cenasParaCarregar;
            mc.idPos = idPos;
            mc.IniciarMudarCena(personagem);
        }

        private void IniciarMudarCena(Transform T)
        {
            personagem = T;
            MessageAgregator<FadeOutComplete>.AddListener(OnFadeOutComplete);
            MessageAgregator<FadeInComplete>.AddListener(OnFadeInComplete);
            MessageAgregator<MsgRequestFadeOut>.Publish();

            SceneManager.LoadSceneAsync(NomesCenasEspeciais.CenaDeCarregamento.ToString(), LoadSceneMode.Additive);

            SceneManager.sceneLoaded += CarregouCenaDeLoader;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name);

            if (other.tag == "Player" && Time.timeScale > 0 && !iniciouLoad)
            {
                iniciouLoad = true;
                IniciarMudarCena(other.transform);
            }
            else if (other.CompareTag("Criature"))
            {
                if (!iniciouLoad)
                {
                    KeyDjeyTransportManager kTransport = other.GetComponent<KeyDjeyTransportManager>();
                    if (kTransport)
                    {
                        kTransport.SairDoKeyDjey();
                    }
                    //else
                    //    MessageAgregator<MsgBlockPetAdvanceInTrigger>.Publish(new MsgBlockPetAdvanceInTrigger()
                    //    {
                    //        pet = other.gameObject
                    //    });
                }
            }
        }

        private void OnFadeInComplete(FadeInComplete obj)
        {
            MessageAgregator<FadeOutComplete>.RemoveListener(OnFadeOutComplete);

            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<FadeInComplete>.RemoveListener(OnFadeInComplete);
            });
        }

        private void CarregouCenaDeLoader(Scene arg0, LoadSceneMode arg1)
        {
            if (arg0.name == NomesCenasEspeciais.CenaDeCarregamento.ToString())
            {
                SceneManager.sceneLoaded -= CarregouCenaDeLoader;
                carregouCena = true;
                VerifiqueProcedimento();

                SupportSingleton.Instance.InvokeOnCountFrame(() =>
                {
                    MessageAgregator<MsgJaajEntrando>.Publish();
                }, 10);
            }
        }

        private void OnFadeOutComplete(FadeOutComplete obj)
        {
            terminouFadeOut = true;
            Time.timeScale = 0;
            contCenasCaregadas = 0;
            cenaAtiva = nomeCenasParaCarregar[0];

            Debug.Log("cena ativa para musica: " + cenaAtiva);
            
            MessageAgregator<MsgChangeMusicIfNew>.Publish(new MsgChangeMusicIfNew()
            {
                nmcvc = MusicDictionary.GetSceneMusic(cenaAtiva)
            });

            VerifiqueProcedimento();
        }

        private void VerifiqueProcedimento()
        {
            if (terminouFadeOut && carregouCena)
            {
                NomesCenas[] N = SceneLoaderBase.DescarregarCenasDesnecessarias(nomeCenasParaCarregar);

                for (int i = 0; i < N.Length; i++)
                {
                    DestruirGameObjectsDaCena(N[i]);
                    SceneManager.UnloadSceneAsync(N[i].ToString());
                }

                SupportSingleton.Instance.StartCoroutine(CarergamentoComPausa(nomeCenasParaCarregar));
            }
        }

        public IEnumerator CarergamentoComPausa(NomesCenas[] cenasAlvo)
        {
            var _asyncOperation = Resources.UnloadUnusedAssets();
            while (!_asyncOperation.isDone)
            { yield return null; }

            Debug.Log("Unload Unused Assets Complete");

            System.GC.Collect();

            yield return new WaitForSecondsRealtime(1);
            NomesCenas[] N = SceneLoaderBase.PegueAsCenasPorCarregar(cenasAlvo);

            numCenasParaCarregar = N.Length;

            for (int i = 0; i < N.Length; i++)
            {
                SceneManager.LoadScene(N[i].ToString(), LoadSceneMode.Additive);
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            contCenasCaregadas++;

            if (contCenasCaregadas >= numCenasParaCarregar)
            {
                SceneManager.sceneLoaded -= OnSceneLoaded;

                SceneManager.activeSceneChanged += OnActiveSceneChanged;


                SceneManager.SetActiveScene(SceneManager.GetSceneByName(cenaAtiva.ToString()));

                CustomizationSavedChars.LoadSavedCharacters();

                SaveDatesManager.SalvarAtualizandoDados(new Criatures2021.CriaturesSaveDates());
            }
        }

        public static void PosicionarPersonagemFocandoCamera(SpawnID idPos,Transform personagem)
        {
            Transform spawnPoint = SpawnPointMark.GetSpawnPointById(idPos).transform;
            //Debug.Log(spawnPoint + " : " + spawnPoint.position);
            personagem.GetComponent<CharacterController>().enabled = false;



            personagem.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            FayvitCam.CameraApplicator.cam.Cdir.RequireImmediateFocusPosition();


            personagem.GetComponent<CharacterController>().enabled = true;
        }

        private void OnActiveSceneChanged(Scene arg0, Scene arg1)
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;

            PosicionarPersonagemFocandoCamera(idPos,personagem);
            MessageAgregator<MsgRequestFadeIn>.Publish();
            Time.timeScale = 1;

            

            
            MessageAgregator<MsgJaajSaindo>.Publish();

            //MonoBehaviour.FindObjectOfType<Camera2D>().AposMudarDeCena(posAlvo + new Vector3(0, 0, -10));

            //EventAgregator.Publish(EventKey.changeActiveScene, null);

            MessageAgregator<MsgChangeGameScene>.Publish(new MsgChangeGameScene()
            {
                personagem = personagem
            });

           
        }

        static void DestruirGameObjectsDaCena(NomesCenas N)
        {
            Scene S = SceneManager.GetSceneByName(N.ToString());

            foreach (GameObject G in S.GetRootGameObjects())
            {
                MonoBehaviour.Destroy(G);
            }

            System.GC.Collect();
            Resources.UnloadUnusedAssets();
        }


    }
    
}

public struct MsgChangeGameScene : IMessageBase
{
    public Transform personagem;
}
