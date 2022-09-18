using UnityEngine.UI;
using FayvitUI;

public static class TextExtension
{

    public static void SetText(this Text text,string texto)
    {
        text.text = texto;
        ChangeToBestFitOnExtrapolate.Verify(text);
    }

    /// <summary>
    /// Returns true when the Text object contains more lines of text than will fit in the text container vertically
    /// </summary>
    /// <returns></returns>
    public static bool IsOverflowingVerticle(this Text text)
    {
        return LayoutUtility.GetPreferredHeight(text.rectTransform) > GetCalculatedPermissibleHeight(text);
    }

    private static float GetCalculatedPermissibleHeight(Text text)
    {
        //if (cachedCalculatedPermissibleHeight != -1) return cachedCalculatedPermissibleHeight;

        float cachedCalculatedPermissibleHeight = text.rectTransform.rect.height;
        return cachedCalculatedPermissibleHeight;
    }
    //private static float cachedCalculatedPermissibleHeight = -1;

    /// <summary>
    /// Returns true when the Text object contains more character than will fit in the text container horizontally
    /// </summary>
    /// <returns></returns>
    public static bool IsOverflowingHorizontal(this Text text)
    {
        return LayoutUtility.GetPreferredWidth(text.rectTransform) > GetCalculatedPermissibleWidth(text);
    }

    private static float GetCalculatedPermissibleWidth(Text text)
    {
        //if (cachedCalculatedPermissiblWidth != -1) return cachedCalculatedPermissiblWidth;

        float cachedCalculatedPermissiblWidth = text.rectTransform.rect.width;
        return cachedCalculatedPermissiblWidth;
    }
    //private static float cachedCalculatedPermissiblWidth = -1;

}