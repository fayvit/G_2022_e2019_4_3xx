using System.Collections.Generic;

namespace TextBankSpace
{
    public static class CustomizationTextsEN_G
    {
        public static Dictionary<TextKey, List<string>> txt = new Dictionary<TextKey, List<string>>()
        {{ TextKey.customizationParts, new List<string>(){
            "Base",
            "Hair",
            "chin",
            "eyeball",
            "pupil",
            "iris",
            "moisture",
            "eyebrow",
            "beard",
            "trunk",
            "hand",
            "waist",
            "legs",
            "boots",
            "private",
            "noise",
            "empty",//ID=16
            "Gender",
            "Color",
            "*** Conclude ***",
            "lips",
            "Type",
            "Detail"
        } },
            {
            TextKey.frasesDoCustomization,new List<string>()
            {
            "Start the game with this character?",
            "Do you want to save this character?",
            "Select color and do not use registration",
            "Select color and use registry",
            "Back to previous menu"
            }
            },
            {
            TextKey.frasesDoCharDbMenu,new List<string>()
            {
            "Would you like to delete this vector character?",
            "Choose a name, which will be an ID identifier, for this character",
            "Choose a name for the Character list",
            "Do you want to create a list of saved characters?",
            "Do you want to copy this character to a character list?",
            "This list has no saved characters. Do you want to choose another list or go back to the default list?",
            "Back to default",
            "Choose another List",
            "Would you like to delete the <color=yellow>{0}</color> character list?"
            }
            }
        };
    }

}
