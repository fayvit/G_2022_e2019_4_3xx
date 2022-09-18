using UnityEngine.UI;

namespace FayvitUI
{
    public class ChangeToBestFitOnExtrapolate
    {
        public static void Verify(Text t)
        {
            t.resizeTextForBestFit = false;
            
            t.resizeTextForBestFit = t.IsOverflowingVerticle();
        }
    }
}