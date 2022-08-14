using System.Collections;
using Criatures2021Hud;
using FayvitBasicTools;
using FayvitMessageAgregator;
using TextBankSpace;
using UnityEngine;

namespace Criatures2021
{
    [System.Serializable]
    public class AttackLearnItem : ModifyPetItemBase
    {
        private string virtualMessage="";
        protected AttackNameId[] golpeDoPergaminho;
        protected bool esqueceu = false;

        protected int indiceDoEscolhido = -1;
        protected EstadoDoAprendeGolpe estadoDoAprendeGolpe = EstadoDoAprendeGolpe.baseUpdate;

        protected enum EstadoDoAprendeGolpe
        {
            baseUpdate,
            esperandoConfirmacaoDeEsquecimento,
            esperandoConfirmacaoDoNaoAprender,
            aprendiSemEsquecer
        }

        public AttackLearnItem(ItemFeatures cont) : base(cont) { confirmarRetorno = true; }

        protected virtual string NomeBasico
        {
            get { return PetAttackBase.NomeEmLinguas(golpeDoPergaminho[0]); }
        }

        protected override bool AtualizaUsoDoPergaminho()
        {
            switch (estadoDoAprendeGolpe)
            {
                case EstadoDoAprendeGolpe.baseUpdate:
                    return base.AtualizaUsoDoPergaminho();
                #region FoiDesnecessario
                //case EstadoDoAprendeGolpe.esperandoConfirmacaoDeEsquecimento:
                //    if (Commands.confirmButton)
                //    { 

                //    }
                //    //if (GameController.g.CommandR.DisparaAcao())
                //    //{
                //    //    GameController.g.HudM.P_Golpe.gameObject.SetActive(false);
                //    //    GameController.g.HudM.Painel.EsconderMensagem();
                //    //    base.OpcaoEscolhida(indiceDoEscolhido);
                //    //    estadoDoAprendeGolpe = EstadoDoAprendeGolpe.baseUpdate;
                //    //}
                //    break; 
                #endregion
                case EstadoDoAprendeGolpe.aprendiSemEsquecer:
                case EstadoDoAprendeGolpe.esperandoConfirmacaoDoNaoAprender:
                    if (Commands.confirmButton)
                    {
                        estadoDoAprendeGolpe = EstadoDoAprendeGolpe.baseUpdate;
                        MessageAgregator<MsgHideSimpleAttackShow>.Publish();
                        MessageAgregator<MsgRequestHideUpperLargeMessage>.Publish();
                        Finaliza();
                    }
                    //if (GameController.g.CommandR.DisparaAcao())
                    //{
                    //    GameController.g.HudM.P_Golpe.gameObject.SetActive(false);
                    //    GameController.g.HudM.Painel.EsconderMensagem();
                    //    estadoDoAprendeGolpe = EstadoDoAprendeGolpe.baseUpdate;
                    //    Estado = EstadoDeUsoDeItem.finalizaUsaItem;
                    //}
                    break;
            }

            return true;

        }

        private struct Usara
        {
            public bool jaTem;
            public bool diferenteDeNulo;
        }

        Usara VerificaUso(PetBase C)
        {
            AttackNameId golpePorAprender = GolpePorAprender(C);


            return new Usara()
            {
                diferenteDeNulo = golpePorAprender != AttackNameId.nulo,
                jaTem = C.GerenteDeGolpes.TemEsseGolpe(golpePorAprender)
            };
        }

        protected override void OpcaoEscolhida(int escolha)
        {
            MessageAgregator<MsgRequestHideUpperLargeMessage>.Publish();
            PetBase P = Dono.GetComponent<CharacterManager>().Dados.CriaturesAtivos[escolha];
            AttackNameId golpePorAprender = GolpePorAprender(P);
            indiceDoEscolhido = escolha;

            Usara usara = VerificaUso(P);

            if (usara.diferenteDeNulo && !usara.jaTem)
            {
                if (P.GerenteDeGolpes.meusGolpes.Count >= 4)
                    VerificaQualEsquecer(P, UsarOuNaoUsar);
                else
                    base.OpcaoEscolhida(escolha);
            }
            else
            {
                string message = "";
                if (usara.jaTem)
                    message = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.itens)[4],
                    P.GetNomeEmLinguas, NomeEmLinguas(ID), PetAttackBase.NomeEmLinguas(golpePorAprender)
                    );
                else if (!usara.diferenteDeNulo)
                    message = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.itens)[5],
                    P.GetNomeEmLinguas, NomeBasico
                    );

                MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                {
                    sfxId = FayvitSounds.SoundEffectID.painelAbrindo
                });
                Estado = ItemUseState.oneMessageOpened;
                AbstractGlobalController.Instance.OneMessage.StartMessagePanel(() => {
                    MessageAgregator<MsgRequestSfx>.Publish(new MsgRequestSfx()
                    {
                        sfxId = FayvitSounds.SoundEffectID.Book1
                    });

                    Finaliza();
                },message
                    );
            }

            ContainerBasicMenu.instance.Menu.FinishHud();

            #region Original
            //GameController g = GameController.g;
            //CriatureBase C = g.Manager.Dados.CriaturesAtivos[escolha];
            //nomesGolpes golpePorAprender = GolpePorAprender(C);
            //indiceDoEscolhido = escolha;
            //g.HudM.Painel.EsconderMensagem();
            ///*
            //bool foi = false;
            //g.HudM.Painel.EsconderMensagem();

            //if (golpePorAprender != nomesGolpes.nulo)
            //    foi = true;

            //bool jaTem = C.GerenteDeGolpes.TemEsseGolpe(golpePorAprender);
            //*/

            //Usara usara = VerificaUso(C);
            //if (usara.diferenteDeNulo && !usara.jaTem)
            //{
            //    if (C.GerenteDeGolpes.meusGolpes.Count >= 4)
            //        VerificaQualEsquecer(C, UsarOuNaoUsar);
            //    else
            //        base.OpcaoEscolhida(escolha);

            //}
            //else if (usara.jaTem)
            //{
            //    g.HudM.UmaMensagem.ConstroiPainelUmaMensagem(() => { Estado = EstadoDeUsoDeItem.finalizaUsaItem; },
            //        string.Format(BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.itens)[4],
            //        C.NomeEmLinguas, MbItens.NomeEmLinguas(ID), GolpeBase.NomeEmLinguas(golpePorAprender)
            //        )
            //        );
            //}
            //else if (!usara.diferenteDeNulo)
            //{
            //    g.HudM.UmaMensagem.ConstroiPainelUmaMensagem(() => { Estado = EstadoDeUsoDeItem.finalizaUsaItem; },
            //        string.Format(BancoDeTextos.RetornaListaDeTextoDoIdioma(ChaveDeTexto.itens)[5],
            //        C.NomeEmLinguas, NomeBasico
            //        )
            //        );
            //}

            //g.HudM.Menu_Basico.FinalizarHud(); 
            #endregion

        }

        protected override void EscolhiEmQuemUsar(int indice)
        {
            indiceDoEscolhido = indice;
            PetBase C = Dono.GetComponent<CharacterManager>().Dados.CriaturesAtivos[indice];
            PetAtributes A = C.PetFeat.meusAtributos;
            Usara usara = VerificaUso(C);

            if ((usara.diferenteDeNulo && !usara.jaTem) || A.PV.Corrente <= 0)
            {
                if (C.GerenteDeGolpes.meusGolpes.Count >= 4 && A.PV.Corrente > 0)
                {

                    VerificaQualEsquecer(C, UsarOuNaoUsarMenu);
                    //GameController.g.HudM.P_EscolheUsoDeItens.gameObject.SetActive(false);
                    //GameController.g.HudM.MenuDePause.EsconderPainelDeItens();
                }
                else
                    EscolhiEmQuemUsar(indice, A.PV.Corrente > 0, true);
            }
            else if (!usara.diferenteDeNulo)
            {
                string m = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.itens)[5],
                    C.GetNomeEmLinguas, NomeBasico
                    );
                if (eMenu)
                {
                    MessageAgregator<MsgNotUseItem>.Publish(new MsgNotUseItem()
                    {
                        notMessage = m
                    });
                }else
                MyGlobalController.Instance.OneMessage.StartMessagePanel(() => { Finaliza(); },m);
                //MensDeUsoDeItem.MensNaoPodeAprenderGolpe(NomeBasico, C.NomeEmLinguas);
            }
            else if (usara.jaTem)
            {
                string m = string.Format(TextBank.RetornaListaDeTextoDoIdioma(TextKey.itens)[4],
                    C.GetNomeEmLinguas, NomeEmLinguas(ID), PetAttackBase.NomeEmLinguas(GolpePorAprender(C))
                    );
                if (eMenu)
                {
                    MessageAgregator<MsgNotUseItem>.Publish(new MsgNotUseItem()
                    {
                        notMessage = m
                    });
                }
                else
                    MyGlobalController.Instance.OneMessage.StartMessagePanel(() => { Finaliza(); },m);
                //MensDeUsoDeItem.MensjaConheceGolpe(C.NomeEmLinguas, MbItens.NomeEmLinguas(ID), GolpeBase.NomeEmLinguas(GolpePorAprender(C)));
            }

        }

        void VerificaQualEsquecer(PetBase C, System.Action<bool> acao)
        {
            Estado = ItemUseState.emEspera;
            MessageAgregator<MsgRequestNewAttackHud>.Publish(new MsgRequestNewAttackHud()
            {
                oAprendiz = C,
                dePergaminho=true,
                golpePorAprender = C.GerenteDeGolpes.ProcuraGolpeNaLista(C.NomeID, GolpePorAprender(C))
            });
            MessageAgregator<MsgEndNewAttackHud>.AddListener((MsgEndNewAttackHud obj)=> {
                Usara usara = VerificaUso(C);
                Debug.Log(usara.diferenteDeNulo + "  diferente de nulo");
                virtualMessage = obj.message;
                acao(usara.diferenteDeNulo);
            });

            #region Original
            //HudManager hudM = GameController.g.HudM;
            //nomesGolpes nomeDoGolpe = GolpePorAprender(C);
            //hudM.H_Tenta.Aciona(C, nomeDoGolpe,
            //    string.Format(BancoDeTextos.RetornaFraseDoIdioma(ChaveDeTexto.precisaEsquecer), C.NomeEmLinguas)
            //    , acao);

            //hudM.Painel.AtivarNovaMens(
            //    string.Format(BancoDeTextos.RetornaFraseDoIdioma(ChaveDeTexto.tentandoAprenderGolpe),
            //    C.NomeEmLinguas,
            //    GolpeBase.NomeEmLinguas(nomeDoGolpe))
            //    , 24
            //    );

            //hudM.P_Golpe.Aciona(PegaUmGolpeG2.RetornaGolpe(nomeDoGolpe)); 
            #endregion
        }


        void UsarOuNaoUsarMenu(bool usou)
        {
            PetBase C = Dono.GetComponent<CharacterManager>().Dados.CriaturesAtivos[indiceDoEscolhido];
            esqueceu = usou;
            if(usou)
            {
                MyGlobalController.Instance.OneMessage.StartMessagePanel(TrocouComMenu,string.Format(
                    TextBank.RetornaFraseDoIdioma(TextKey.aprendeuGolpeEsquecendo),
                           C.GetNomeEmLinguas,
                           "",
                           PetAttackBase.NomeEmLinguas(GolpePorAprender(C))));

            }else
            {
MyGlobalController.Instance.OneMessage.StartMessagePanel(TrocouComMenu,string.Format(
                    TextBank.RetornaFraseDoIdioma(TextKey.naoAprendeuGolpeNovo),
                           C.GetNomeEmLinguas,
                           PetAttackBase.NomeEmLinguas(GolpePorAprender(C))));

            }

            #region Original
            //if (usou)
            //{
            //    GameController.g.HudM.UmaMensagem.ConstroiPainelUmaMensagem(TrocouComMenu,
            //        string.Format(BancoDeTextos.RetornaFraseDoIdioma(ChaveDeTexto.aprendeuGolpeEsquecendo),
            //                C.NomeEmLinguas,
            //                "",
            //                GolpeBase.NomeEmLinguas(GolpePorAprender(C))));
            //}
            //else
            //{
            //    GameController.g.HudM.UmaMensagem.ConstroiPainelUmaMensagem(DesistiuDeAprenderComMenu,
            //    string.Format(BancoDeTextos.RetornaFraseDoIdioma(ChaveDeTexto.naoAprendeuGolpeNovo),
            //                C.NomeEmLinguas,
            //                GolpeBase.NomeEmLinguas(GolpePorAprender(C))));
            //}
            #endregion

        }

        void RetornoComumDeMenu()
        {
            //GameController.g.HudM.P_EscolheUsoDeItens.gameObject.SetActive(true);
            //GameController.g.HudM.P_Golpe.gameObject.SetActive(false);
            ////GameController.g.HudM.MenuDePause.BotaoItens();
        }

        void DesistiuDeAprenderComMenu()
        {

            RetornoComumDeMenu();
        }

        void TrocouComMenu()
        {
            RetornoComumDeMenu();
            esqueceu = true;
            //EscolhiEmQuemUsar(indiceDoEscolhido);
        }

        /*

             case EstadoDoAprendeGolpe.esperandoConfirmacaoDeEsquecimentoMenu:
                    if (GameController.g.CommandR.DisparaAcao())
                    {
                        GameController.g.HudM.P_Golpe.gameObject.SetActive(false);
                        GameController.g.HudM.Painel.EsconderMensagem();
                        GameController.g.HudM.P_EscolheUsoDeItens.gameObject.SetActive(true);
                        EscolhiEmQuemUsar(indiceDoEscolhido);                    
                        return false;
                    }
                    break;
                case EstadoDoAprendeGolpe.esperandoConfirmacaoDoNaoAprenderMenu:
                    if (GameController.g.CommandR.DisparaAcao())
                    {
                        GameController.g.HudM.P_EscolheUsoDeItens.gameObject.SetActive(true);
                        GameController.g.HudM.P_Golpe.gameObject.SetActive(false);
                        GameController.g.HudM.Painel.EsconderMensagem();
                        return false;
                    }
                    break;*/

        void UsarOuNaoUsar(bool usou)
        {
            esqueceu = usou;
            if (usou)
            {
                base.OpcaoEscolhida(indiceDoEscolhido);
                
                //Estado = ItemUseState.finalizaUsaItem;
                //estadoDoAprendeGolpe = EstadoDoAprendeGolpe.baseUpdate;
                //estadoDoAprendeGolpe = EstadoDoAprendeGolpe.esperandoConfirmacaoDeEsquecimento;
                //Debug.Log(estadoDoAprendeGolpe);
            }
            else
            {
                //estadoDoAprendeGolpe = EstadoDoAprendeGolpe.esperandoConfirmacaoDoNaoAprender;
            }

        }

        protected virtual AttackNameId GolpePorAprender(PetBase C)
        {
            return C.GerenteDeGolpes.ProcuraGolpeNaLista(C.NomeID, golpeDoPergaminho[0]).Nome;
        }

        protected override void AplicaEfeito(int indice)
        {
            PetBase C = Dono.GetComponent<CharacterManager>().Dados.CriaturesAtivos[indice];

            if (!esqueceu)
            {
                C.GerenteDeGolpes.meusGolpes.Add(AttackFactory.GetAttack(GolpePorAprender(C)));
            }
            EntraNoModoFinalizacao(C);

        }

        protected override void EntraNoModoFinalizacao(PetBase C)
        {
            Estado = ItemUseState.emEspera;
            AttackNameId nomeDoGolpe = GolpePorAprender(C);
            //if (GameController.g.HudM.MenuDePause.EmPause)
            //{
            //    Finaliza();
            //}
            // else 
            //if (!esqueceu)
            //{
            if (eMenu)
            {

                ItemBase refi = ProcuraItemNaLista(ID, Lista);

                MessageAgregator<MsgUsingQuantitativeItem>.Publish(new MsgUsingQuantitativeItem()
                {
                    temMaisParausar = refi.ID == ID && refi.Estoque > 0,
                    confirmarRetorno = confirmarRetorno,
                    mensagemDeRetorno = esqueceu ? virtualMessage : string.Format(TextBank.RetornaFraseDoIdioma(TextKey.aprendeuGolpe),
                            C.GetNomeEmLinguas,
                            PetAttackBase.NomeEmLinguas(nomeDoGolpe))
                });
            }
            else
            {
                MessageAgregator<MsgStartExternalInteraction>.Publish();

                
                MessageAgregator<MsgRequestUpperLargeMessage>.Publish(new MsgRequestUpperLargeMessage()
                {
                    message = esqueceu ? virtualMessage : string.Format(TextBank.RetornaFraseDoIdioma(TextKey.aprendeuGolpe),
                            C.GetNomeEmLinguas,
                            PetAttackBase.NomeEmLinguas(nomeDoGolpe)),
                    hideTime = 30
                });
                MessageAgregator<MsgSimpleShowAttack>.Publish(new MsgSimpleShowAttack()
                {
                    attackDb = C.GerenteDeGolpes.ProcuraGolpeNaLista(C.NomeID, nomeDoGolpe)
                });


                //    MyGlobalController.Instance.OneMessage.StartMessagePanel(

                //                , 30
                //                );

                //    GameController.g.HudM.P_Golpe.Aciona(PegaUmGolpeG2.RetornaGolpe(nomeDoGolpe));
                estadoDoAprendeGolpe = EstadoDoAprendeGolpe.aprendiSemEsquecer;
                //}
                //else if (esqueceu)
                //{
                ////    GameController.g.StartCoroutine(TerminaDaquiAPouco());
                //}
            }
        }

        IEnumerator TerminaDaquiAPouco()
        {
            yield return new WaitForSeconds(1.5f);
            Finaliza();
        }

    }
}