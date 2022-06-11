using UnityEngine;
using System.Collections.Generic;

namespace TextBankSpace
{
    public static class TutoInfoPT_BR
    {
        public static Dictionary<TextKey, List<string>> txt = new Dictionary<TextKey, List<string>>()
        {
            { TextKey.dodgeInfo,new List<string>()
            {
                "Esquivar",
                "Os criatures podem esquivar de alguns ataques e perigos",
                "Você pode esquivar com o Criature pressionando <color=yellow>L</color> no teclado ou <color=red>B</color> no controle de Xbox"
            }},
            { TextKey.targetLockInfo,new List<string>()
            {
                "Travar camera em um alvo",
                "Ao travar a camera em um alvo o criature estará sempre atento aos movimentos do alvo facilitando os acertos na batalha",
                "Controlando o criature você pode travar a mira pressionando a tecla <color=yellow>Tab</color> no teclado ou o botão <color=magenta>LT</color> no controle de Xbox"
            }},
            { TextKey.alternanciaInfo,new List<string>()
            {
                "Alternancia",
                "Você pode alternar entre o controle do treinador e do criature",
                "Para alternar o controle entre o treinador e o criature pressione <color=yellow>E</color> no teclado ou <color=magenta>LS</color> no controle do Xbox"
            }},
            { TextKey.mudaGolpeInfo,new List<string>()
            {
                "Mudando o Golpe Selecionado",
                "Altere o golpe selecionado pressionando <color=yellow>numero 3</color> no teclado ou <color=magenta>Dpad Down</color> no controle de Xbox",
                "Os criatures podem manter até quatro golpes na memória. Alguns golpes tem caracteristicas ligadas ao tipo do criature, outros sãao golpes de suporte e alguns são golpes com caracteristicas de impacto. " +
                "Cada caracteristica de golpe pode ter uma utilidade além do uso em batalha"
            }},
            { TextKey.atacarInfo,new List<string>()
            {
                "Atacando com Criature",
                "Use os poderes dos criatures em batalhas ou contra obstaculos que podem estar no caminho",
                "Controlando o criature pressione <color=yellow>P</color> no teclado ou <color=magenta>RT</color> no controle do Xbox"
            }}
        };
    }
}