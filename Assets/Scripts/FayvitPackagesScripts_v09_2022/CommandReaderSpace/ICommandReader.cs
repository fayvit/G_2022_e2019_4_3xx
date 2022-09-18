using UnityEngine;
using System.Collections;

namespace FayvitCommandReader
{
    public interface ICommandReader
    {
        int IndexOfControl { get; }
        Controlador ControlId { get; }
        bool VerifyThisControlUse();
        bool SubmitButtonDown();
        bool GetButton(int numButton);
        bool GetButtonDown(int numButton);
        bool GetButtonUp(int numButton);
        bool GetButton(string nameButton);
        bool GetButtonDown(string nameButton);
        bool GetButtonUp(string nameButton);
        float GetAxis(string esseGatilho);
        int GetIntTriggerDown(string esseGatilho);
        bool GetButton(CommandConverterInt cci);
        bool GetButtonDown(CommandConverterInt cci,bool travaQuadro = false);
        bool GetButtonUp(CommandConverterInt cci, bool travaQuadro = false);
        float GetAxis(CommandConverterString ccs);
        int GetIntTriggerDown(CommandConverterString ccs);
        Vector3 DirectionalVector();
        void ZerarGatilhos();
        
    }
}