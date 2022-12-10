using Criatures2021;
using FayvitBasicTools;
using FayvitMessageAgregator;
using FayvitSupportSingleton;
using System.Collections.Generic;
using TextBankSpace;
using UnityEngine;

namespace Assets.Scripts.Criatures_Scripts.CriatureReborn.Eventos
{
    public class DragPiecesPuzzleManager : ButtonActivate,IIdentifiable
    {
        [SerializeField] private GameObject puzzleContainer;
        [SerializeField] private GameObject[] pieces;
        [SerializeField] private GameObject[] solution;
        [SerializeField] private GameObject endPiece;
        [SerializeField] private int indiceDoLivre;
        [SerializeField] private string ID;

        
        
        private LocalState state = LocalState.emEspera;
        private MsgSendExternalPanelCommand externalCommand;

        private readonly List<List<moves>> movimentacoes = new List<List<moves>>{
            new List<moves>{moves.right,moves.down },
            new List<moves>{moves.right,moves.down,moves.left },
            new List<moves>{moves.left,moves.down },
            new List<moves>{moves.right,moves.down,moves.up },
            new List<moves>{moves.right,moves.down,moves.up,moves.left },
            new List<moves>{moves.left,moves.down,moves.up },
            new List<moves>{moves.right,moves.up },
            new List<moves>{moves.right,moves.up,moves.left },
            new List<moves>{moves.left,moves.up }
        };

        private enum LocalState
        { 
            emEspera,
            mudando,
            verificando
        }

        private enum moves
        { 
            up,left,right,down,nulo
        }

        public string PublicID => ID;

        // Use this for initialization
        void Start()
        {
            if (AbstractGameController.Instance.MyKeys.VerificaAutoShift(ID))
            {
                MessageAgregator<MsgDragPuzzleStartComplete>.Publish(new MsgDragPuzzleStartComplete()
                {
                    ID = ID
                });

                Destroy(this);
            }
            else
            {

                textoDoBotao = TextBank.RetornaListaDeTextoDoIdioma(TextKey.textoBaseDeAcao)[1];
                SempreEstaNoTrigger();
                MessageAgregator<MsgSendExternalPanelCommand>.AddListener(OnReceiveCommands);
            }
        }

        private void OnDestroy()
        {
            MessageAgregator<MsgSendExternalPanelCommand>.RemoveListener(OnReceiveCommands);
        }

        private void OnReceiveCommands(MsgSendExternalPanelCommand obj)
        {
            externalCommand = obj;
        }

        private void OnValidate()
        {
            BuscadorDeID.Validate(ref ID, this);
        }        

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            switch (state)
            {
                case LocalState.mudando:
                    int cmdH = externalCommand.hChange;
                    int cmdV = -externalCommand.vChange;
                    moves m = moves.nulo;

                    if (externalCommand.confirmButton)
                    {
                        bool foi = true;
                        for (int i = 0; i < pieces.Length; i++)
                        {
                            foi &= pieces[i] == solution[i];
                        }

                        pieces[indiceDoLivre].SetActive(false);
                        Vector3 pos = endPiece.transform.position;
                        endPiece.transform.parent = pieces[indiceDoLivre].transform.parent;
                        endPiece.transform.SetSiblingIndex(indiceDoLivre);
                        state = LocalState.verificando;

                        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx() { sfxId = FayvitSounds.SoundEffectID.Parry });

                        SupportSingleton.Instance.InvokeInSeconds(() =>
                        {
                            if (foi)
                            {
                                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx() { sfxId = FayvitSounds.SoundEffectID.tuin_1ponto3 });
                                AbstractGameController.Instance.MyKeys.MudaAutoShift(ID, true);

                                SupportSingleton.Instance.InvokeInSeconds(() => {
                                    puzzleContainer.SetActive(false);

                                    MessageAgregator<MsgDragPuzzleComplete>.Publish(new MsgDragPuzzleComplete()
                                    {
                                        ID = ID
                                    });
                                }, 1);
                                state = LocalState.emEspera;
                                

                                
                            }
                            else
                            {
                                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx() { sfxId = FayvitSounds.SoundEffectID.Buzzer1 });

                                SupportSingleton.Instance.InvokeInSeconds(() => {
                                    endPiece.transform.parent = pieces[indiceDoLivre].transform.parent.parent;
                                    endPiece.transform.position = pos;
                                    pieces[indiceDoLivre].SetActive(true);

                                    state = LocalState.mudando;
                                }, .75f);
                            }
                        }, .25f);

                    }
                    else if (externalCommand.returnButton)
                    {
                        state = LocalState.emEspera;
                        puzzleContainer.SetActive(false);
                        MessageAgregator<MsgChangeToHero>.Publish(new MsgChangeToHero()
                        {
                            myHero = MyGlobalController.MainCharTransform.gameObject
                        });
                        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx() { sfxId = FayvitSounds.SoundEffectID.Book1 });
                        
                    }else
                    if (cmdH != 0)
                    {
                        m = cmdH == 1 ? moves.right : moves.left; 
                        cmdV = 0;
                    } else if (cmdV != 0)
                        m = cmdV == -1 ? moves.up : moves.down;

                    if (movimentacoes[indiceDoLivre].Contains(m))
                    {
                        int novoIndice = indiceDoLivre + cmdH + 3 * cmdV;
                        GameObject G = pieces[novoIndice];
                        pieces[novoIndice] = pieces[indiceDoLivre];
                        pieces[indiceDoLivre] = G;
                        if (indiceDoLivre < novoIndice)
                        {
                            G.transform.SetSiblingIndex(indiceDoLivre);
                            pieces[novoIndice].transform.SetSiblingIndex(novoIndice);
                        }
                        else
                        {
                            pieces[novoIndice].transform.SetSiblingIndex(novoIndice);
                            G.transform.SetSiblingIndex(indiceDoLivre);                            
                        }
                        indiceDoLivre = novoIndice;

                        MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx() { sfxId = FayvitSounds.SoundEffectID.Cursor1 });
                    }
                break;
            }
        }

        public override void FuncaoDoBotao()
        {
            
            if (!AbstractGameController.Instance.MyKeys.VerificaAutoShift(ID))
            {

                SomDoIniciar();
                FluxoDeBotao();

                puzzleContainer.SetActive(true);
                externalCommand = new MsgSendExternalPanelCommand();
                state = LocalState.mudando;
            }

        }
    }

    public struct MsgDragPuzzleComplete : IMessageBase
    {
        public string ID;
    }

    public struct MsgDragPuzzleStartComplete : IMessageBase
    {
        public string ID;
    }

}