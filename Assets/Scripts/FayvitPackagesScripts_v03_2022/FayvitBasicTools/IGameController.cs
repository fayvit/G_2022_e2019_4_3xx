using UnityEngine;
using System.Collections;
using FayvitUI;

namespace FayvitBasicTools
{
    public interface IGameController
    {
        KeyVar MyKeys { get; }
        GameObject ThisGameObject { get; }
    }
}