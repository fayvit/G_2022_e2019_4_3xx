using System.Collections.Generic;
using UnityEngine;

namespace TextBankSpace
{
    public class PT_BR_TextosIniciais
    {
        public static Dictionary<TextKey, List<string>> txt = new Dictionary<TextKey, List<string>>()
        {
            { TextKey.armagedomInicial,new List<string>(){
            "A resistencia ao imperio está instalando estações de conexão com armagedom em lugares estratégicos e escondidos nas planicies de Orion",
            "Você como um membro novato da resistencia talvez tenha dificuldades em encontra-las, mas em pouco tempo deve acostumar com o padrão" +
                " e vai as localizar facilmente",
            "Toda essa genialidade de engenharia se deve a <color=orange>Lance Lutz</color>. Ele é um grande estudioso da universidade da predominancia" +
                " e está conosco na resistencia."
            }
            },{
            TextKey.armagedomInicialFraseRapidaPadrao, new List<string>()
            {
            "Se precisar curar os seus criatures volte até aqui"
            }
            },
            {
            TextKey.RandomHooliganInicial,new List<string>()
            { 
                "Olá estranho!! Meu nome é <color=orange>Random Hooligan</color>. Minha família vem de uma tradição aristocrática em Orion.",
                "Meu irmão é o regente de uma area divina de Criatures e eu estou ajudando a resistencia com informações privilegiadas que consigo com ele",
                "Logo iniciarei viagem para a cidade de <color=yellow>Afarenga</color> encontrar com meu irmão.",
                "Talvez possamos nos encontrar nos caminhos adiante."
            }
                },
            {
            TextKey.LanceLutzInicial,new List<string>()
            {
                "Olá novato!! Meu nome é <color=orange>Lance Lutz</color>. Eu sou um estudioso formado pela grande universidade da predominancia.",
                "O imperador no auge de sua sanha ganânciosa mandou fechar a universidade. Ele temia que os intelectuais iniciassem uma revolta.",
                "O imperador sabe que o pensamento critico da universidade logo iria causar problemas para ele e resolveu nos atacar primeiro",
                "Atualmente estou viajando pelas planícies de Orion para ajudar a instalação de conexões clandestinas ao <color=cyan>Armagedom</color>.",
                "Logo iniciarei o caminho para a cidade de <color=yellow>Ofawing</color>. Lá tentarei uma audiencia com autoridades do governo em auxilio da universidade"
            }
                }
        };
        
    }
}