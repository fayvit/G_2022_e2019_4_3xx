using ChangeScene;
using FayvitBasicTools;
using FayvitLoadScene;
using FayvitMessageAgregator;
using FayvitSupportSingleton;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Criatures2021
{
    public class ReturnToArmagedomAfterDefeated
    {

        private static bool InTeste { get; set; }
        public static void StartReturn(CharacterManager manager)
        {
            GameObject gameObject = manager.gameObject;
            DadosDeJogador Dados = manager.Dados;
            ReturnToArmagedomAfterDefeated.InTeste = manager.InTeste;

            AbstractGlobalController.Instance.FadeV.StartFadeOutWithAction(() =>
            {
                MessageAgregator<MsgRequestHideUpperLargeMessage>.Publish();
                MessageAgregator<MsgStartRerturnToArmagedom>.Publish(new MsgStartRerturnToArmagedom()
                {
                    dono = manager
                });
                
                //MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero() { myHero = gameObject });

                VisistsToArmagedoms V = ArmagedomsLocations.L[Dados.UltimoArmagedom];
                Dados.TodosCriaturesPerfeitos();

                if (CenasCarregadas(V.nomeDasCenas))
                {
                    
                    MudeCena.PosicionarPersonagemFocandoCamera(V.spawnId, gameObject.transform);
                    SupportSingleton.Instance.InvokeInSeconds(() =>
                    {
                        MessageAgregator<MsgClearPetTarget>.Publish(new MsgClearPetTarget()
                        {
                            owner = gameObject
                        });

                        AbstractGlobalController.Instance.FadeV.StartFadeInWithAction(() =>
                        {
                            MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero()
                            {
                                myHero = gameObject
                            });
                            MessageAgregator<MsgBlockPetAdvanceInTrigger>.Publish(new MsgBlockPetAdvanceInTrigger()
                            {
                                pet = manager.ActivePet.gameObject
                            });
                            
                            MessageAgregator<MsgChangeMusicIfNew>.Publish(new MsgChangeMusicIfNew()
                            {
                                nmcvc = MusicDictionary.GetSceneMusic(V.nomeDasCenas[0])
                            });

                            VerifiqueSalvar();

                        });
                    }, 3);
                }
                else
                {
                    MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero() { myHero = gameObject });

                    MessageAgregator<MsgChangeGameScene>.AddListener(OnChangeGameSceneToArmagedom);

                    MudeCena.MudarCena(
                        V.nomeDasCenas, V.spawnId, gameObject.transform
                        );
                }
            });
        }

        private static bool CenasCarregadas(NomesCenas[] Ns)
        {
            bool carregadas = true;
            foreach (var N in Ns)
                carregadas &= SceneManager.GetSceneByName(N.ToString()).isLoaded;

            return carregadas;
        }

        private static void VerifiqueSalvar()
        {
            if (!InTeste)
                FayvitSave.SaveDatesManager.SalvarAtualizandoDados(new Criatures2021.CriaturesSaveDates());
        }

        private static void OnChangeGameSceneToArmagedom(MsgChangeGameScene obj)
        {
            VerifiqueSalvar();

            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                MessageAgregator<MsgChangeGameScene>.RemoveListener(OnChangeGameSceneToArmagedom);
            });
        }
    }
}