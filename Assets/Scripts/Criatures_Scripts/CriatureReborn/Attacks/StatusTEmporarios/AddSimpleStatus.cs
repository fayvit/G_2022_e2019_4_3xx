using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitMessageAgregator;

namespace Criatures2021
{
    public class AddSimpleStatus
    {
        public static void InsereStatusSimples(PetManager levou, StatusTemporarioBase S, int numStatus)
        {
            //int numStatus = StatusTemporarioBase.ContemStatus(TipoStatus.envenenado, C);
            PetBase C = levou.MeuCriatureBase;
            if (numStatus == -1)
            {
                InserindoNovoStatus(levou, C, S);
            }
            else
            {
                DatesForTemporaryStatus d = C.StatusTemporarios[numStatus];
                d.Quantificador = Mathf.Max(S.Dados.Quantificador, d.Quantificador + 1);
                d.TempoSignificativo += 15f / 14f * S.Dados.TempoSignificativo;

                S.StatusHitUpdater();

                MessageAgregator<MsgSendUpdateStatus>.Publish(new MsgSendUpdateStatus()
                {
                    receiver = levou,
                    S = S
                });
            }
        }

        public static void InserindoNovoStatus(
            PetManager levou,
            PetBase C,
            StatusTemporarioBase S, bool deMenu = false)
        {
            C.StatusTemporarios.Add(S.Dados);

            MessageAgregator<MsgSendNewStatus>.Publish(new MsgSendNewStatus()
            {
                receiver = levou,
                S = S,
                deMenu=deMenu
            });
        }
    }

    public struct MsgSendNewStatus : IMessageBase
    {
        public PetManager receiver;
        public StatusTemporarioBase S;
        public bool deMenu;
    }

    public struct MsgSendUpdateStatus : IMessageBase
    {
        public PetManager receiver;
        public StatusTemporarioBase S;
    }
}