using UnityEngine;
using System.Collections;
using Criatures2021Hud;
using FayvitBasicTools;
using FayvitMessageAgregator;

namespace Criatures2021
{
    public class ActionManager
    {
        private ButtonActivate visualizado;
        private System.Action acao;

        //public bool useiCancel = false;

        //private static bool esteQuadro = false;
        private static ActionManager instance;

        public static ActionManager Instance {
            get {
                if (instance == null)
                {
                    instance = new ActionManager();
                }

                return instance;
            }
        }

        private ActionManager()
        {
            MessageAgregator<MsgInvokeActionFromHud>.AddListener(OnRequestInvoke);
        }

        public void OnDestroy()
        {
            MessageAgregator<MsgInvokeActionFromHud>.RemoveListener(OnRequestInvoke);
        }

        private void OnRequestInvoke(MsgInvokeActionFromHud obj)
        {
            Debug.Log("Request Invoke");
            //if(visualizado!=null)
            //    if(visualizado)

            if (ActionHudManager.Active)
                acao?.Invoke();
        }

        public bool PodeVisualizarEste(ButtonActivate Tt)
        {
            Transform T = Tt.transform;

            bool pode = false;
            if (visualizado != null)
            {
                Transform player = AbstractGlobalController.Instance.Players[0].Manager.transform;

                if (Vector3.Distance(player.position, T.position)
                    <
                    Vector3.Distance(player.position, visualizado.transform.position))
                {
                    pode = true;
                    visualizado = Tt;
                    acao = visualizado.FuncaoDoBotao;
                }

                if (visualizado == Tt)
                    pode = true;
            }
            else
            {
                pode = true;
                visualizado = Tt;
                acao = visualizado.FuncaoDoBotao;
            }
            return pode;
        }

        public bool TransformDeActionE(Transform T)
        {
            return T == visualizado;
        }

        public void ModificarAcao(ButtonActivate T, System.Action acao)
        {
            visualizado = T;
            this.acao = acao;
        }


        //public static void VerificaAcao()
        //{
        //    if (!esteQuadro)
        //    {
        //        AgendaEsseQuadro();
        //        if (visualizado != null)
        //            if (visualizado.Btn.activeSelf)
        //            {
        //                if (acao != null)
        //                {
        //                    acao();
        //                }
        //                else
        //                {
        //                    visualizado.FuncaoDoBotao();
        //                }
        //            }
        //    }
        //}

        //public static bool ButtonUp(int n, Controlador c)
        //{
        //    bool press = CommandReader.ButtonUp(n, c);
        //    if (!esteQuadro && press)
        //    {

        //        AgendaEsseQuadro();
        //        return true;
        //    }
        //    else return false;
        //}


        //static void AgendaEsseQuadro()
        //{
        //    esteQuadro = true;
        //    SupportSingleton.Instance.InvokeOnCountFrame(() =>
        //    {
        //        esteQuadro = false;
        //    });
        //}
    }
}