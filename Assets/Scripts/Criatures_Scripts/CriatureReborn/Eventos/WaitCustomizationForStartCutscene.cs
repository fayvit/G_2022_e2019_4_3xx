using UnityEngine;
using UnityEngine.SceneManagement;
using FayvitMessageAgregator;
using FayvitBasicTools;
using FayvitLoadScene;
using FayvitSave;
using System;
using FayvitSupportSingleton;
using FayvitCam;

namespace Criatures2021
{
    public class WaitCustomizationForStartCutscene : MonoBehaviour
    {
        private Transform character;
        private CustomizationContainerDates ccd;
        private string guid;
        private bool fadeOutComplete=false;
        private bool combinationComplete=false;
        private bool jaIniciouCarregamento=false;

        void Start()
        {
            MessageAgregator<MsgFinishCustomizationMenu>.AddListener(OnEndCustomizationMenu);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgFinishCustomizationMenu>.RemoveListener(OnEndCustomizationMenu);
        }

        private void Update()
        {
            if (fadeOutComplete && combinationComplete&&!jaIniciouCarregamento)
            {
                Time.timeScale = 0.25f;
                SceneManager.LoadSceneAsync(NomesCenasEspeciais.cutsceneIntro.ToString(), LoadSceneMode.Additive);
                SceneManager.sceneLoaded += CarregouCutScene;
                jaIniciouCarregamento = true;
            }
        }

        private void OnEndCustomizationMenu(MsgFinishCustomizationMenu obj)
        {
            ccd = obj.secManager.GetCustomDates();
            MessageAgregator<MsgCombinationComplete>.AddListener(OnCombinationComplete);
            guid = System.Guid.NewGuid().ToString();
            obj.tCombiner.StartCombiner(obj.secManager, guid);

            IGlobalController glob = AbstractGlobalController.Instance;

            SaveDatesManager.SalvarAtualizandoDados(new Criatures2021.SaveDates(false));
            
            DontDestroyOnLoad(gameObject);

            glob.FadeV.StartFadeOutWithAction(() =>
            {
                fadeOutComplete = true;
            });
        }

        private void OnCombinationComplete(MsgCombinationComplete obj)
        {
            if (obj.checkKey == guid)
            {
                character = obj.combined;
                Scene comuns = SceneManager.GetSceneByName(NomesCenasEspeciais.ComunsDeFase.ToString());

                SceneManager.MoveGameObjectToScene(character.gameObject, comuns);

                CharacterManager c = character.gameObject.AddComponent<CharacterManager>();
                c.InTeste = false;
                c.enabled = false;  
                c.Ccd = ccd;
                c.transform.position = Vector3.zero;
                SupportSingleton.Instance.InvokeOnEndFrame(() =>
                {
                    MessageAgregator<MsgStartExternalInteraction>.Publish();
                }); 

                AbstractGlobalController.Instance.Players.Add(
                    new PlayersInGameDb()
                    {
                        Control = FayvitCommandReader.Controlador.teclado,
                        DbState = PlayerDbState.ocupadoLocal,
                        Manager = c
                    }
                    );

                //SetHeroCamera.Set(character);


                Destroy(CameraApplicator.cam.GetComponent<CustomizationCamManager>());
                Destroy(CameraApplicator.cam.transform.GetChild(0).GetChild(0).gameObject);
                CameraApplicator.cam.GetComponentInChildren<Camera>().rect = new Rect(0, 0, 1, 1);
                SceneManager.MoveGameObjectToScene(CameraApplicator.cam.gameObject, comuns);


                SupportSingleton.Instance.InvokeOnEndFrame(() =>
                {
                    MessageAgregator<MsgCombinationComplete>.RemoveListener(OnCombinationComplete);
                });

                MessageAgregator<MsgFinishEdition>.Publish();
                MessageAgregator<MsgStartMusic>.Publish(new MsgStartMusic()
                {
                    nameMusic = FayvitSounds.NameMusic.xodoDaBaiana
                });

                combinationComplete = true;

                //GameObject G = new GameObject();
                //G.name = "provisionalEnemyInstantiate";
                //G.AddComponent<ProvisionalEnemyInstantiate>();

                //Destroy(secManager.gameObject);
                //Destroy(gameObject);
            }
        }

        private void CarregouCutScene(Scene arg0, LoadSceneMode arg1)
        {
            MessageAgregator<MsgClearStatusUpdater>.Publish();
            MessageAgregator<MsgStartExternalInteraction>.Publish();


            SceneManager.sceneLoaded -= CarregouCutScene;
            SceneManager.UnloadSceneAsync(NomesCenas.cenaTeste.ToString());
            AbstractGlobalController.Instance.FadeV.StartFadeInWithAction(() =>
            {
                Time.timeScale = 1;
                Destroy(gameObject);
            });
        }
    }
}