using UnityEngine;
using System.Collections;
using FayvitMessageAgregator;

namespace Criatures2021
{
    public class StatusReplacer
    {
        public static void ColocarStatus(PetBase C)
        {

            DatesForTemporaryStatus dados;

            if (C.StatusTemporarios != null)
            {
                //PetManager cm = null;

                for (int i = 0; i < C.StatusTemporarios.Count; i++)
                {
                    dados = C.StatusTemporarios[i];

                    //if (GameController.g.Manager.Dados.CriaturesAtivos.IndexOf(C) == 0)
                    //{
                    //    cm = GameController.g.Manager.CriatureAtivo; ;
                    //    Debug.Log("ser ou naõ ser ");
                    //}

                    switch (dados.Tipo)
                    {
                        case StatusType.envenenado:


                            //// Debug.Log(GameController.g.Manager.Dados.CriaturesAtivos.IndexOf(C) + " : " + cm.MeuCriatureBase.NomeID);

                            //GameController.g.ContStatus.StatusDoHeroi.Add(new Envenenado()
                            //{
                            //    Dados = dados,
                            //    CDoAfetado = cm,
                            //    OAfetado = C
                            //});


                            break;
                        case StatusType.amedrontado:
                            //MyGameController.Instance.ContStatus.StatusDoHeroi.Add(new Amedrontado()
                            //{
                            //    Dados = dados,
                            //    CDoAfetado = cm,
                            //    OAfetado = C
                            //});
                        break;
                        case StatusType.fraco:
                            //GameController.g.ContStatus.StatusDoHeroi.Add(new Fraco()
                            //{
                            //    Dados = dados,
                            //    CDoAfetado = cm,
                            //    OAfetado = C
                            //});
                        break;
                        default:
                            Debug.Log("foi encontrado um status sem recolocação configurada");
                        break;
                    }
                }
                //if (cm != null)
                //    VerificaInsereParticulaDeStatus(cm);
            }
            else C.StatusTemporarios = new System.Collections.Generic.List<DatesForTemporaryStatus>();
        }

        public static void VerificaStatusDosAtivos()
        {
            //for (int i = 0; i < GameController.g.Manager.Dados.CriaturesAtivos.Count; i++)
            //    ColocarStatus(GameController.g.Manager.Dados.CriaturesAtivos[i]);
        }

        public static void VerificaInsereParticulaDeStatus(PetManager C)
        {
            MessageAgregator<MsgRequestStatusParticles>.Publish(new MsgRequestStatusParticles()
            {
                pet = C
            });
            //for (int i = 0; i < GameController.g.ContStatus.StatusDoHeroi.Count; i++)
            //{
            //    StatusTemporarioBase sTb = GameController.g.ContStatus.StatusDoHeroi[i];

            //    if (GameController.g.Manager.Dados.CriaturesAtivos.IndexOf(sTb.OAfetado)
            //        == GameController.g.Manager.Dados.CriaturesAtivos.IndexOf(C.MeuCriatureBase))
            //    {
            //        sTb.CDoAfetado = C;
            //        sTb.Start();
            //    }
            //}
        }
    }

    public struct MsgRequestStatusParticles : IMessageBase
    {
        public PetManager pet;
    }
}