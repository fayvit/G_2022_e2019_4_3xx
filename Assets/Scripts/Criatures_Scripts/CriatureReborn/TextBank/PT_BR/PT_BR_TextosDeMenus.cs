using UnityEngine;
using System.Collections.Generic;

namespace TextBankSpace
{
    public static class PT_BR_TextosDeMenus
    {
        public static Dictionary<TextKey, List<string>> txt = new Dictionary<TextKey, List<string>>()
            {
            {TextKey.menuUsoDeItem, new List<string>()
                    { "Usar","Organizar","Item Rapido","Descartar","Voltar"}
            },
            { TextKey.MenuDescartavel,new List<string>(){ 
                "Qual a quantidade de {0} que gostaria de descartar?",
                "O minimo descartavel é 1",
                "O máximo descartabel é {0}"
            } },
            { TextKey.mudouItemRapido,new List<string>()
            { 
            "O item {0} entrou para os itens rápidos",
            "O item {0} saiu dos itens rapidos"
            } },
            { TextKey.menuPets,new List<string>()
            {
            "Voltar",
            "Organizar",
            "Ver status"
            } }
        };
    }
}