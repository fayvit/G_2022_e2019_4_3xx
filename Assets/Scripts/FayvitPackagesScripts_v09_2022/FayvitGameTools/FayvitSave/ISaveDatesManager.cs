using System.Collections.Generic;

namespace FayvitSave
{
    public interface ISaveDatesManager
    {
        LanguageKey ChosenLanguage { get; set; }
        SaveDates CurrentSaveDate { get; }
        List<SaveDates> SavedGames { get; set; }
        List<PropriedadesDeSave> SaveProps { get; set; }

        void RemoveSaveDates(int indice);
    }

    public enum LanguageKey
    {
        pt_br,
        en_google
    }
}