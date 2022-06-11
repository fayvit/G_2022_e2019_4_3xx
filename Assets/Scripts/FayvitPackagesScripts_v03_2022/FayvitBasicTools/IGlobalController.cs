using FayvitCommandReader;
using FayvitUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitSounds;

namespace FayvitBasicTools
{
    public interface IGlobalController
    { 
        //Essa propriedade deveria ser Statica mas não é permitido em interface
        //public IGlobalController InterfaceInstance { get; }
        List<IPlayersInGameDb> Players { get; set; }
        Controlador Control { get; }
        ConfirmationPanel Confirmation { get; }
        SingleMessagePanel OneMessage { get; }
        IFadeView FadeV { get; }
        float MusicVolume { get; set; }
        float SfxVolume { get; set; }
        bool EmTeste { get; set; }
        //public ContainerDeDadosDeCena SceneDates { get { return sceneDates; } private set { sceneDates = value; } }
        Vector3 InitialGamePosition { get;}
        IMusicManager Music { get; }
    }
}
