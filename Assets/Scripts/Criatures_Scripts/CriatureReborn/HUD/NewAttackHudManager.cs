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
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgRequestNewAttackHud>.RemoveListener(OnRequestNewAttackHud);
        }

        private void OpenSimpleNewAttackLearn(MsgRequestNewAttackHud obj)
        {
            lState = LocalState.showOpened;

            List<PetAttackDb> learnings = obj.oAprendiz.GolpesPorAprender;
            
            
            showHud.Start(AttackFactory.GetAttack(learnings[0].Nome),learnings[0].ModPersonagem);

            OpenMessageAndSchelduleReadInputs(string.Format(TextBank.RetornaFraseDoIdioma(TextKey.aprendeuGolpe),
                    oAprendiz.GetNomeEmLinguas, PetAttackBase.NomeEmLinguas(learnings[0].Nome)));
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
                AttackFactory.GetAttack(obj.oAprendiz.GolpesPorAprender[0].Nome),obj.oAprendiz);

            OpenMessageAndSchelduleReadInputs(string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.aprendeuGolpe)[1],
                    oAprendiz.GetNomeEmLinguas, PetAttackBase.NomeEmLinguas(obj.oAprendiz.GolpesPorAprender[0].Nome))+"\n\r"+
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
            oAprendiz.GerenteDeGolpes.meusGolpes.Add(AttackFactory.GetAttack(oAprendiz.GolpesPorAprender[0].Nome));
            MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
            {
                sfxId = FayvitSounds.SoundEffectID.VinhetinhaCompletaComFim
            });


            ComunsDeFinalizacao();


        }

        void ComunsDeFinalizacao()
        {

            oAprendiz.GolpesPorAprender.RemoveAt(0);

            if (oAprendiz.GolpesPorAprender.Count <= 0)
            {
                RequisitarFluxoNormalDeJogo();

            }
        }

        void RequisitarFluxoNormalDeJogo()
        {
            lState = LocalState.inStand;

            MessageAgregator<MsgEndNewAttackHud>.Publish(new MsgEndNewAttackHud()
            {
                fluxo = fluxo,
                oAprendiz = oAprendiz
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

                        RequisitarFluxoNormalDeJogo();

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
            string nome2 = PetAttackBase.NomeEmLinguas(oAprendiz.GolpesPorAprender[0].Nome);
            if (esquecido == 4)
            {
                ComunsDeFinalizacao();

                MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
                {
                    message = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.aprendeuGolpe)[6],
                    oAprendiz.GetNomeEmLinguas,nome2)
                });
                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                {
                    sfxId = FayvitSounds.SoundEffectID.VinhetinhaCurta
                });
            }
            else
            {
                string nome1 = oAprendiz.GerenteDeGolpes.meusGolpes[esquecido].GetNomeEmLinguas;
                
                oAprendiz.GerenteDeGolpes.meusGolpes[esquecido] = AttackFactory.GetAttack(oAprendiz.GolpesPorAprender[0].Nome);
                ComunsDeFinalizacao();

                MessageAgregator<MsgRequestRapidInfo>.Publish(new MsgRequestRapidInfo()
                {
                    message = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.aprendeuGolpe)[5],
                    oAprendiz.GetNomeEmLinguas, nome1, nome2)
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
    }

    public struct MsgEndNewAttackHud : IMessageBase {
        public PetBase oAprendiz;
        public FluxoDeRetorno fluxo;
    }
}