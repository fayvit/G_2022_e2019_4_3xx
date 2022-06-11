using FayvitLoadScene;
using System.Collections.Generic;

namespace FayvitBasicTools
{
    public interface IKeyVar
    {
        NomesCenas CenaAtiva { get; set; }
        List<NomesCenas> CenasAtivas { get; set; }

        void MudaAutoCont(string key, int val = 0);
        void MudaAutoShift(string key, bool val = false);
        void MudaCont(KeyCont key, int val = 0);
        void MudaEnemyShift(string key, bool val = false);
        void MudaShift(KeyShift key, bool val = false);
        void ReviverInimigos();
        void SetarCenasAtivas();
        void SetarCenasAtivas(NomesCenas[] cenasAtivas);
        void SomaAutoCont(string key, int soma = 0);
        void SomaCont(KeyCont key, int soma = 0);
        int VerificaAutoCont(string key);
        bool VerificaAutoShift(KeyShift key);
        bool VerificaAutoShift(string key);
        int VerificaCont(KeyCont key);
        bool VerificaEnemyShift(string key);
    }
}