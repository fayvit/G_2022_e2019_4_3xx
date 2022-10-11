using System.Collections.Generic;
using UnityEngine;
using GameJolt.API;
using GameJolt.UI;
using GameJolt.UI.Controllers;
using UnityEngine.SceneManagement;
using Criatures2021;
using CustomizationSpace;
//using System.IO;
//using Unity.Mathematics;
//using System.Runtime.Serialization.Formatters.Binary;
//using FayvitSave;

namespace MyJoltSpace
{
    public class JoltLoginManager : MonoBehaviour
    {
        [SerializeField] private GameObject JoltManager;
        [SerializeField] private GameObject loadBar;
        [SerializeField] private GameObject buttonsPanel;

        private bool estouCarregando = false;
        private bool iniciouCarregamento = false;

        //[System.Serializable]
        //private class LocalPreJson
        //{
        //    public float4[] dado;
        //}

        //void ConverterPraTxt()
        //{
        //    List<float4> f4 = RawLoadAndSave.CarregarArquivo<List<float4>>("DateColors/mainColors.crs", Application.dataPath);

        //    LocalPreJson l = new LocalPreJson()
        //    {
        //        dado = f4.ToArray()
        //    };

        //    string s = JsonUtility.ToJson(l);
        //    BinaryFormatter bf = new BinaryFormatter();
        //    try
        //    {
        //        Debug.Log("Deu???: "+s);
        //        StreamWriter file = File.CreateText(Application.dataPath+ "/DateColors/mainColors.txt");


        //        file.Write(s);

        //        file.Close();
        //    }
        //    catch (IOException e)
        //    {
        //        Debug.Log(e.StackTrace);
        //        Debug.LogWarning("Save falhou");
        //    }
        //}

        //void ConverterPersonagensParaTxt()
        //{
        //    Dictionary<string, List<CustomizationContainerDates>> dccd
        //        = FayvitSave.RawLoadAndSave.CarregarArquivo<Dictionary<string, List<CustomizationContainerDates>>>(
        //            "listaDeCustomizados.tsc", Application.dataPath
        //            );
        //    FayvitSave.preJSON pre = new FayvitSave.preJSON()
        //    {
        //        b = FayvitBasicTools.BytesTransform.ToBytes(dccd)
        //    };
        //    System.IO.StreamWriter file = System.IO.File.CreateText(Application.dataPath + "/DateColors/listaDeCustomizados.txt");
        //    file.Write(JsonUtility.ToJson(pre));
        //    file.Close();
        //}
        // Start is called before the first frame update
        void Start()
        {
            //ConverterPersonagensParaTxt();
            //ConverterPraTxt();
            FayvitSupportSingleton.SupportSingleton.Instance.InvokeOnCountFrame(() =>
            {
                TextBankSpace.TextBank.VerificaChavesFortes(FayvitSave.LanguageKey.pt_br, FayvitSave.LanguageKey.en_google);
            });
            iniciouCarregamento = false;

            buttonsPanel.SetActive(true);
            loadBar.SetActive(false);

            if (!GameObject.Find("GameJoltAPI"))
                JoltManager.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {

            //GameJoltAPI.Instance.CurrentUser.SignOut();
            bool isSignedIn = GameJoltAPI.Instance.CurrentUser != null;
            if (isSignedIn)
            //if(false)
            {
                GameObject G = GameObject.Find("SignInPanel");

                if (G)
                {
                    SignInWindow S = GameObject.Find("SignInPanel").GetComponent<SignInWindow>();
                    if (S)
                        S.Dismiss(true);
                }

                buttonsPanel.SetActive(false);
                loadBar.SetActive(true);

                if (!estouCarregando)
                {
                    AgendeODiferenteDeZero();
                    estouCarregando = true;
                    SaveAndLoadInJolt.estaCarregado = false;
                }

                if (SaveAndLoadInJolt.estaCarregado)
                {
#if !UNITY_EDITOR
                Sessions.Open(AbriuSessao);
                Sessions.Ping();
#endif
                    if (!iniciouCarregamento)
                    {
                        CarregarCena();
                    }
                }


            }
        }

        private void AbriuSessao(bool obj)
        {
            Debug.Log("Abriu sessão: " + obj);
        }

        void CarregarCena()
        {

            buttonsPanel.SetActive(false);
            loadBar.SetActive(true);

            iniciouCarregamento = true;
            //SceneManager.LoadScene("cenaTeste");
            SceneManager.sceneLoaded += OnComunsLoaded;
            SceneManager.LoadSceneAsync("ComunsDeFase", LoadSceneMode.Additive);

            Debug.Log("Atenção a condição de carregado");
        }

        private void OnComunsLoaded(Scene arg0, LoadSceneMode arg1)
        {
            SceneManager.sceneLoaded -= OnComunsLoaded;
            SceneManager.sceneLoaded += OnCenaTesteLoaded;
            SceneManager.LoadScene("cenaTeste_indoAoJolt", LoadSceneMode.Additive);
            
        }

        private void OnCenaTesteLoaded(Scene arg0, LoadSceneMode arg1)
        {
            SceneManager.UnloadSceneAsync("JoltLoad");
            SceneManager.sceneLoaded -= OnCenaTesteLoaded;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("cenaTeste_indoAoJolt"));

            CustomizationSavedChars.LoadSavedCharacters();

        }

        void AgendeODiferenteDeZero()
        {

            Debug.Log(GameJoltAPI.Instance.CurrentUser.ID);
            if (GameJoltAPI.Instance.CurrentUser.ID != 0)
            {
                Debug.Log("Cena de carregamento"+ GameJoltAPI.Instance.CurrentUser.ID);
                SaveAndLoadInJolt.estaCarregado = true;
                SaveAndLoadInJolt.Load();
            }
            else
                Invoke("AgendeODiferenteDeZero", 0.25f);
        }

        
        void OnLoad(bool X)
        {
            estouCarregando = true;
            if (X)
                AgendeODiferenteDeZero();
            else
                Debug.Log("Puta que o pariu");
        }
        public void FazerLogin()
        {
            bool isSignedIn = GameJoltAPI.Instance.CurrentUser != null;

            if (!isSignedIn)
                GameJoltUI.Instance.ShowSignIn(OnLoad);
        }

        public void ContinuarSemLogin()
        {
            
            SaveDatesForJolt.instance = new SaveDatesForJolt()
            {
                ccds = new ToSaveCustomizationContainer()
                {
                    ccds = new List<CustomizationContainerDates>()
                },
                lccds = new ListToSaveCustomizationContainer()
                {
                    dccd = new Dictionary<string, List<CustomizationContainerDates>>()
                }
            };

            if(!iniciouCarregamento)
                CarregarCena();
            //SceneManager.LoadScene("Inicial");
        }

        public void AutoLogin(AutoLoginResult a)
        {
            Debug.Log("auto login sim: "+a.ToString());
            if (GameJoltAPI.Instance)
            {
                Debug.Log("instance sim: ");
                if(GameJoltAPI.Instance.CurrentUser!=null)
                    Debug.Log("current User sim: ");
            }
        }



        //public void DoubleClickAction(BaseEventData e)
        //{
        //    //if (estado == LocalState.baseMenuOpened)
        //    {
        //        int clicks = ((PointerEventData)e).clickCount;

        //        if (clicks == 2)
        //        {
        //            Debug.Log("do point event: " + ((PointerEventData)e).pointerPress);
        //            int indice = ((PointerEventData)e).pointerCurrentRaycast.gameObject.transform.GetSiblingIndex() - 1;
        //            //MessageAgregator<MsgFinishCharDbManager>.Publish();
        //            //charDbMenu.FinishHud();
        //        }
        //    }
        //}
    }
}