using UnityEngine;

namespace FayvitBasicTools
{
    public interface IFadeView
    {
        bool Darken { get; set; }
        bool Lighten { get; set; }
        void ClearFade();
        void StartFadeOut(Color fadeColor = default,float darkenTime=0);
        void StartFadeOutWithAction(System.Action endFadeOut, Color fadeColor = default);
        void StartFadeOutWithAction(System.Action endFadeOut, float darkenTime, Color fadeColor = default);
        void StartFadeInWithAction(System.Action endFadeIn, Color fadeColor = default);
        void StartFadeInWithAction(System.Action endFadeIn, float lightenTime, Color fadeColor = default);
        void StartFadeIn(Color fadeColor = default,float lightenTime=0);
    }
}
