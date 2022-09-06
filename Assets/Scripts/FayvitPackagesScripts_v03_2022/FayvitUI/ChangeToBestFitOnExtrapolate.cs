using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FayvitUI
{
    public class ChangeToBestFitOnExtrapolate
    {
        public static void Verify(Text t)
        {
            t.resizeTextForBestFit = false;
            
            t.resizeTextForBestFit = t.IsOverflowingHorizontal() || t.IsOverflowingVerticle();
        }
    }
}