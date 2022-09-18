using UnityEngine;
using FayvitMessageAgregator;

namespace Criatures2021
{
    public class StatusReplacer
    {
        public static StatusTemporarioBase StatusFactory(DatesForTemporaryStatus dados,PetBase C,PetManager cm)
        {
            StatusTemporarioBase S = null;
            switch (dados.Tipo)
            {
                case StatusType.envenenado:

                    S = new Envenenado()
                    {
                        Dados = dados,
                        CDoAfetado = cm,
                        OAfetado = C
                    };

                break;
                case StatusType.amedrontado:
                    S =
                    new Amedrontado()
                    {
                        CDoAfetado = cm,
                        Dados = dados,
                        OAfetado = C
                    };

                break;
                case StatusType.fraco:

                    S =
                    new Fraco()
                    {
                        CDoAfetado = cm,
                        Dados = dados,
                        OAfetado = C
                    };

                break;
                default:
                    Debug.Log("foi encontrado um status sem recolocação configurada");
                break;
            }

            return S;
        }

        public static void ColocarStatus(PetBase C,PetManager cm)
        {

            DatesForTemporaryStatus dados;

            if (C.StatusTemporarios != null)
            {
                cm = cm.MeuCriatureBase == C ? cm : null;
                

                for (int i = 0; i < C.StatusTemporarios.Count; i++)
                {
                    
                    dados = C.StatusTemporarios[i];
                    StatusTemporarioBase S = StatusFactory(dados, C, cm);                    

                    MessageAgregator<MsgSendNewStatus>.Publish(new MsgSendNewStatus()
                    {
                        receiver = cm,
                        S = S,
                        deMenu = cm == null
                    });
                }
            }
            else C.StatusTemporarios = new System.Collections.Generic.List<DatesForTemporaryStatus>();
        }

        public static void VerificaInsereParticulaDeStatus(PetManager C)
        {
            MessageAgregator<MsgRequestStatusParticles>.Publish(new MsgRequestStatusParticles()
            {
                pet = C
            });
        }
    }

    public struct MsgRequestStatusParticles : IMessageBase
    {
        public PetManager pet;
    }
}