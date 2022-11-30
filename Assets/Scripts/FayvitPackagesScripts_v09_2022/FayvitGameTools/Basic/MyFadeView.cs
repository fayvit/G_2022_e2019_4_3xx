using FayvitMessageAgregator;
using FayvitSupportSingleton;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FayvitBasicTools
{
    public class MyFadeView : MonoBehaviour, IFadeView
    {
#pragma warning disable 0649
        [SerializeField] private Image escurecedorUpper;
        [SerializeField] private Image escurecedorLower;
#pragma warning restore 0649
        private Color corDoFade = new Color(0,0,0,0);
        private FaseDaqui fase = FaseDaqui.emEspera;
        private float tempoDeAtividade = 1;
        private float tempoDecorrido = 0;
        private float tempoBaseDoEscurecimento = 1;
        private float tempoBaseDoClareamento = 0.75f;
        private System.Action acaoDoFade;


        private bool escurecer = false;
        private bool clarear = false;

        public bool Darken { get => escurecer; set => escurecer = value; }
        public bool Lighten { get => clarear; set => clarear = value; }

        private enum FaseDaqui
        {
            emEspera,
            escurecendo,
            clareando
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (escurecer)
            {
                StartFadeOut();
                escurecer = false;
            }

            if (clarear)
            {
                StartFadeIn();
                clarear = false;
            }
            switch (fase)
            {
                case FaseDaqui.escurecendo:
                    tempoDecorrido += Time.fixedDeltaTime;
                    corDoFade.a = tempoDecorrido / tempoDeAtividade;
                    escurecedorUpper.color = corDoFade;
                    escurecedorLower.color = corDoFade;

                    if (tempoDecorrido > tempoDeAtividade)
                    {
                        fase = FaseDaqui.emEspera;
                        ChamarAcao();
                        MessageAgregator<FadeOutComplete>.Publish();
                        //EventAgregator.Publish(EventKey.fadeOutComplete, null);
                    }
                    break;
                case FaseDaqui.clareando:
                    tempoDecorrido += Time.fixedDeltaTime;
                    corDoFade.a = (tempoDeAtividade - tempoDecorrido) / tempoDeAtividade;
                    escurecedorUpper.color = corDoFade;
                    escurecedorLower.color = escurecedorUpper.color;
                    if (tempoDecorrido > tempoDeAtividade)
                    {
                        fase = FaseDaqui.emEspera;
                        //EventAgregator.Publish(EventKey.fadeInComplete, null);
                        MessageAgregator<FadeInComplete>.Publish();

                        ChamarAcao();

                        escurecedorLower.gameObject.SetActive(false);
                        escurecedorUpper.gameObject.SetActive(false);
                    }
                    break;
            }
        }

        public void ClearFade()
        {
            escurecedorUpper.color = new Color(0, 0, 0, 0);
            escurecedorLower.color = new Color(0, 0, 0, 0);
        }

        void ComunsDeFadeOut(Color corDoFade)
        {
            Debug.Log("Fade out Start");

            MessageAgregator<FadeOutStart>.Publish();
            //EventAgregator.Publish(EventKey.fadeOutStart, null);
            escurecedorLower.gameObject.SetActive(true);
            escurecedorUpper.gameObject.SetActive(true);
            float alpha = this.corDoFade.a;
            this.corDoFade = corDoFade;
            //this.corDoFade.a = 0;
            fase = FaseDaqui.escurecendo;
            tempoDecorrido = alpha*tempoDeAtividade;
        }




        IEnumerator AcaoDequadro(System.Action acao)
        {
            yield return new WaitForEndOfFrame();
            acaoDoFade += acao;
        }

        void ChamarAcao()
        {
            if (acaoDoFade != null)
            {
                acaoDoFade();
                acaoDoFade = null;
            }
        }




        void ComunsDoFadeIn(Color corDoFade)
        {
            Debug.Log("Fade in Start");
            float alpha = this.corDoFade.a;
            this.corDoFade = corDoFade;
            //this.corDoFade.a = 1;
            fase = FaseDaqui.clareando;
            tempoDecorrido = (1- alpha) *tempoDeAtividade;
        }


        public void StartFadeOut(Color fadeColor = default,float darkenTime=0)
        {
            if (darkenTime <= 0)
                tempoDeAtividade = tempoBaseDoEscurecimento;
            else
                tempoDeAtividade = darkenTime;

            ComunsDeFadeOut(fadeColor);
        }

        public void StartFadeOutWithAction(System.Action acao, Color fadeColor = default)
        {
            StartFadeOutWithAction(acao, 0, corDoFade);
        }

        public void StartFadeOutWithAction(System.Action acao, float darkenTime, Color fadeColor = default)
        {
            if (darkenTime <= 0)
                tempoDeAtividade = tempoBaseDoEscurecimento;
            else
                tempoDeAtividade = darkenTime;
            ComunsDeFadeOut(fadeColor);

            SupportSingleton.Instance.InvokeInSeconds(acao,tempoDeAtividade);

        }

        public void StartFadeInWithAction(System.Action acao, Color fadeColor = default)
        {
            StartFadeInWithAction(acao, 0, fadeColor);
        }

        public void StartFadeInWithAction(System.Action acao, float lightenTime, Color fadeColor = default)
        {
            if (lightenTime <= 0)
                tempoDeAtividade = tempoBaseDoClareamento;
            else
                tempoDeAtividade = lightenTime;
            ComunsDoFadeIn(fadeColor);
            SupportSingleton.Instance.InvokeInSeconds(acao,tempoDeAtividade);
        }

        public void StartFadeIn(Color fadeColor = default,float lightenTime = 0)
        {
            if (lightenTime <= 0)
                tempoDeAtividade = tempoBaseDoClareamento;
            else
                tempoDeAtividade = lightenTime;
            ComunsDoFadeIn(fadeColor);
            MessageAgregator<FadeInStart>.Publish();
            //EventAgregator.Publish(EventKey.fadeInStart, null);
        }
    }

    public struct FadeInStart : IMessageBase { }
    public struct FadeOutStart : IMessageBase { }
    public struct FadeOutComplete : IMessageBase { }
    public struct FadeInComplete : IMessageBase { }
}