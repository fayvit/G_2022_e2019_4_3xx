using UnityEngine;
using System.Collections.Generic;

namespace TextBankSpace
{
    public static class PetDescriptionPT_BR
    {
        public static Dictionary<PetName,string> txt = new Dictionary<PetName, string>()
        {
            { PetName.nulo, 
                "Um criature é um ser que vive no planeta de Orion com dons especiais que variam de controlar elementos da natureza" +
                " até  habilidade de luta e salto sem iguais. Criatures tem temperamentos e comportamento variados  e estão espalhados " +
                "por toda a Orion"
            
            },
            { 
                PetName.Florest,
                "Florest é um criature que demonstra um comportamento amedrontado, mas guarda em si um grande poder. " +
                "As lâminas de Florest podem facilmente cortar grades. Quando confortável com a companhia Florest é um criature amigável" +
                "e companheiro."
            },
            { 
                PetName.PolyCharm,
                "PolyCharm é um criature com o comportamento ranzinza, sempre parecendo estar mal humorado e insatisfeito com alguma coisa." +
                "É um criature confiável está sempre disposto a mostrar seu poder em batalha e tem grande potencial" +
                "com seus ataques de fogo"
            },
            { 
                PetName.Xuash,
                "Xuash é um criature confiante, valente e de comportamento positivo, demostra ter muita vontade de entrar numa briga e lutar pra vencer." +
                "Em terra tem pouca agilidade, parece um pouco desengonçado ao caminhar, mas compensa isso com um alto poder de" +
                " ataque com agua."
            }
        };
    }
}
