using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FayvitMessageAgregator;
using FayvitLoadScene;
using FayvitBasicTools;
using FayvitCam;
using FayvitSupportSingleton;

namespace FayvitLikeDarkSouls
{
    public class StartGameWizard : MonoBehaviour
    {

        private TesteMeshCombiner meshCombiner;
        private SectionCustomizationManager secManager;

        private Transform character;
        private string guid;

        public static void InitProvisionalStartGame(
        TesteMeshCombiner meshCombiner,
        SectionCustomizationManager secManager)
        {
            GameObject G = new GameObject();
            StartGameWizard P = G.AddComponent<StartGameWizard>();
            P.SetDates(meshCombiner, secManager);
            P.Init();
        }

        public void SetDates(
        TesteMeshCombiner meshCombiner,
        SectionCustomizationManager secManager)
        {
            this.meshCombiner = meshCombiner;
            this.secManager = secManager;
        }

        public void Init()
        {

            MessageAgregator<MsgCombinationComplete>.AddListener(OnCombinationComplete);
            guid = System.Guid.NewGuid().ToString();
            meshCombiner.StartCombiner(secManager, guid);

        }

        private void OnCombinationComplete(MsgCombinationComplete obj)
        {
            if (obj.checkKey == guid)
            {
                character = obj.combined;
                Scene comuns = SceneManager.GetSceneByName(NomesCenasEspeciais.ComunsDeFase.ToString());

                SceneManager.MoveGameObjectToScene(character.gameObject, comuns);

                Debug.Log(character);

                CharacterManager c = character.gameObject.GetComponent<CharacterManager>();
                AbstractGlobalController.Instance.Players.Add(
                    new PlayersInGameDb()
                    {
                        Control = AbstractGlobalController.Instance.Control,
                        DbState = PlayerDbState.ocupadoLocal,
                        Manager = c
                    }
                    );

                c.InputIndex = AbstractGlobalController.Instance.Control;
                c.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("likeDsAnimator");

                SetHeroCamera.Set(character);


                Destroy(CameraApplicator.cam.GetComponent<CustomizationCamManager>());
                Destroy(CameraApplicator.cam.transform.GetChild(0).GetChild(0).gameObject);
                CameraApplicator.cam.GetComponentInChildren<Camera>().rect = new Rect(0, 0, 1, 1);
                SceneManager.MoveGameObjectToScene(CameraApplicator.cam.gameObject, comuns);


                SupportSingleton.Instance.InvokeOnEndFrame(() =>
                {
                    MessageAgregator<MsgCombinationComplete>.RemoveListener(OnCombinationComplete);
                });

                MessageAgregator<MsgFinishEdition>.Publish();

                MessageAgregator<MsgStartDsHud>.Publish(new MsgStartDsHud()
                { 
                    dono= c.gameObject
                });
                MessageAgregator<MsgStartMusic>.Publish(new MsgStartMusic()
                {
                    clip = Resources.Load<AudioClip>("choroChorandoParaPaulinhoNogueira")
                });

                //GameObject G = new GameObject();
                //G.name = "provisionalEnemyInstantiate";
                //G.AddComponent<ProvisionalEnemyInstantiate>();
            }
        }
    }
}