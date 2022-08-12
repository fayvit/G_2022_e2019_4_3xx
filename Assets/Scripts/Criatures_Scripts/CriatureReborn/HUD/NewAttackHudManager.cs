using UnityEngine;
using FayvitMessageAgregator;
using Criatures2021;
using FayvitSupportSingleton;
using System.Collections.Generic;
using TextBankSpace;
using FayvitBasicTools;
using FayvitUI;

namespace Criatures2021Hud
{
    public class NewAttackHudManager : MonoBehaviour
    {
        [SerializeField] private ShowNewAttackHud showHud;
        [SerializeField] private TryLearnNewAttackHud tryLearn;

        private int hChange;
        private bool confirmButton;
        private bool returnButton;
        private bool ePergaminho;
        private PetAttackDb golpePorAprender;
        private LocalState lState = LocalState.inStand;
        private PetBase oAprendiz;
        private FluxoDeRetorno fluxo;

        private enum LocalState
        { 
            inStand,
            showOpened,
            tryLearnOpened,
            menuOpened
        }

        // Use this for initialization
        void Start()
        {
            MessageAgregator<MsgRequestNewAttackHud>.AddListener(OnRequestNewAttackHud);
            MessageAgregator<MsgSimpleShowAttack>.AddListener(OnRequestSimpleShow);
            MessageAgregator<MsgHideSimpleAttackShow>.AddListener(OnRequestHideSimpleShow);
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgRequestNewAttackHud>.RemoveListener(OnRequestNewAttackHud);
            MessageAgregator<MsgSimpleShowAttack>.RemoveListener(OnRequestSimpleShow);
            MessageAgregator<MsgHideSimpleAttackShow>.RemoveListener(OnRequestHideSimpleShow);
        }

        private void OnRequestHideSimpleShow(MsgHideSimpleAttackShow obj)
        {
            showHud.EndHud();
        }

        private void OnRequestSimpleShow(MsgSimpleShowAttack obj)
        {
            showHud.Start(AttackFactory.GetAttack(obj.attackDb.Nome), obj.attackDb.ModPersonagem);
        }

        private void OpenSimpleNewAttackLearn(MsgRequestNewAttackHud obj)
        {
            lState = LocalState.showOpened;

            
            
            
            showHud.Start(AttackFactory.GetAttack(golpePorAprender.Nome),golpePorAprender.ModPersonagem);

            OpenMessageAndSchelduleReadInputs(string.Format(TextBank.RetornaFraseDoIdioma(TextKey.aprendeuGolpe),
                    oAprendiz.GetNomeEmLinguas, PetAttackBase.NomeEmLinguas(golpePorAprender.Nome)));
        }

        private void OpenMessageAndSchelduleReadInputs(string s,bool bestFit  =false)
        {
            MessageAgregator<MsgRequestUpperLargeMessage>.Publish(new MsgRequestUpperLargeMessage()
            {
                message = s,
                useBestFit = bestFit
            });

            SupportSingleton.Instance.InvokeOnCountFrame(() =>
            {
                MessageAgregator<MsgSendExternalPanelCommand>.AddListener(OnReceiveExternalCommands);
            }, 3);
        }

        private void OpenTryLearnHud(MsgRequestNewAttackHud obj)
        {
            lState = LocalState.tryLearnOpened;
            
            tryLearn.StartHud(obj.oAprendiz.GerenteDeGolpes.meusGolpes.ToArray(),
                AttackFactory.GetAttack(golpePorAprender.Nome),obj.oAprendiz);

            OpenMessageAndSchelduleReadInputs(string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.aprendeuGolpe)[1],
                    oAprendiz.GetNomeEmLinguas, PetAttackBase.NomeEmLinguas(golpePorAprender.Nome))+"\n\r"+
                    string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.aprendeuGolpe)[2], oAprendiz.GetNomeEmLinguas),
                    true
                    );
        }

        private void OnRequestNewAttackHud(MsgRequestNewAttackHud obj)
        {
            fluxo = obj.fluxo;
            returnButton = false;
            confirmButton = false;
            oAprendiz = obj.oAprendiz;
            golpePorAprender = obj.golpePorAprender;
            ePergaminho = obj.dePergaminho;

            if (obj.oAprendiz.GerenteDeGolpes.meusGolpes.Count < 4)
            {
                OpenSimpleNewAttackLearn(obj);
            }
            else
            {
                OpenTryLearnHud(obj);
            }
            
        }

        private void OnReceiveExternalCommands(MsgSendExternalPanelCommand obj)
        {
            confirmButton= obj.confirmButton;
            returnButton = obj.returnButton;
            hChange = obj.hChange;
        }

        private void EndNewAttackHud()
        {
            
            showHud.EndHud();
            oAprendiz.GerenteDeGolpes.meusGolpes.Add(AttackFactory.GetAttack(golpePorAprender.Nome));
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.VinhetinhaCompletaComFim
            });


            ComunsDeFinalizacao();


        }

        void ComunsDeFinalizacao(string message="")
        {
            if (ePergaminho)
            {
                EndHud(message);
            }
            else
            {
                oAprendiz.GolpesPorAprender.RemoveAt(0);

                if (oAprendiz.GolpesPorAprender.Count <= 0)
                {
                    EndHud();

                }
            }
        }

        void EndHud(string message = "")
        {
            lState = LocalState.inStand;

            MessageAgregator<MsgEndNewAttackHud>.Publish(new MsgEndNewAttackHud()
            {
                fluxo = fluxo,
                oAprendiz = oAprendiz,
                message=message
            });

            MessageAgregator<MsgRequestHideUpperLargeMessage>.Publish();

            MessageAgregator<MsgSendExternalPanelCommand>.RemoveListener(OnReceiveExternalCommands);
        }

        // Update is called once per frame
        void Update()
        {
            switch (lState)
            {
                case LocalState.showOpened:
                    if (confirmButton || returnButton)
                        EndNewAttackHud();
                break;
                case LocalState.tryLearnOpened:
                    if (confirmButton)
                    {
                        string message;
                        if (tryLearn.SelectedOption == 4)
                            message = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.aprendeuGolpe)[4],
                                oAprendiz.GetNomeEmLinguas,tryLearn.L_Attacks[4].GetNomeEmLinguas);
                        else
                            message = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.aprendeuGolpe)[3],
                                tryLearn.L_Attacks[tryLearn.SelectedOption].GetNomeEmLinguas,
                                tryLearn.L_Attacks[4].GetNomeEmLinguas);

                        AbstractGlobalController.Instance.Confirmation.StartConfirmationPanel(
                            () => {
                                ConfirmaEsquece(tryLearn.SelectedOption);
                                },
                            RetornaEsquece, message
                            );

                        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx() { 
                            sfxId=FayvitSounds.SoundEffectID.Decision1
                        });

                        tryLearn.EndHud();

                        lState = LocalState.menuOpened;
                    }
                    else if (returnButton)
                    {
                        tryLearn.EndHud();

                        EndHud();

                        MessageAgregator<MsgCloseNewAttackHudNonFinally>.Publish();
                    }
                    else if (hChange != 0)
                    {
                        tryLearn.ChangeOption(hChange);
                    }

                break;
                case LocalState.menuOpened:
                    ConfirmationPanel c =  AbstractGlobalController.Instance.Confirmation;

                    c.ThisUpdate(hChange!=0, confirmButton, returnButton);

                    if (hChange != 0)
                        MessageAgregator<MsgChangeOptionUI>.Publish();
                break;
            }
        }

        void ConfirmaEsquece(int esquecido)
        {
            string nome2 = PetAttackBase.NomeEmLinguas(golpePorAprender.Nome);
            string message = "";
            if (esquecido == 4)
            {
                message = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.aprendeuGolpe)[6],
                    oAprendiz.GetNomeEmLinguas, nome2);

                ComunsDeFinalizacao(message);

                if (!ePergaminho)
                {
                    MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
                    {
                        message = message
                    });

                    MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                    {
                        sfxId = FayvitSounds.SoundEffectID.VinhetinhaCurta
                    });
                }
            }
            else
            {
                string nome1 = oAprendiz.GerenteDeGolpes.meusGolpes[esquecido].GetNomeEmLinguas;
                message = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.aprendeuGolpe)[5],
                        oAprendiz.GetNomeEmLinguas, nome1, nome2);
                oAprendiz.GerenteDeGolpes.meusGolpes[esquecido] = AttackFactory.GetAttack(golpePorAprender.Nome);
                ComunsDeFinalizacao(message);

                if(!ePergaminho)
                    MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
                    {
                        message = message
                    });

                //MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                //{
                //    sfxId = FayvitSounds.SoundEffectID.vinhetinhaMedia
                //});

                TuinManager.RequestDecreaseTuin();
            }
        }

        void RetornaEsquece() {
            OpenTryLearnHud(new MsgRequestNewAttackHud()
            {
                fluxo = fluxo,
                oAprendiz = oAprendiz
            });

            MessageAgregator<MsgNegativeUiInput>.Publish();
        }
    }

    public struct MsgCloseNewAttackHudNonFinally : IMessageBase { }

    public struct MsgRequestNewAttackHud : IMessageBase
    {
        public PetBase oAprendiz;
        public FluxoDeRetorno fluxo;
        public PetAttackDb golpePorAprender;
        public bool dePergaminho;
    }

    public struct MsgEndNewAttackHud : IMessageBase {
        public PetBase oAprendiz;
        public FluxoDeRetorno fluxo;
        public string message;
    }

    public struct MsgSimpleShowAttack : IMessageBase
    {
        public PetAttackDb attackDb;
    }

    public struct MsgHideSimpleAttackShow : IMessageBase { }
}