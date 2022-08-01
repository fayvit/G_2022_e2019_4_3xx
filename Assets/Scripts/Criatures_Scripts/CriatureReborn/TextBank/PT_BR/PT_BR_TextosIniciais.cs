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
                },
            {
            TextKey.caesarAntesDaLutaTuto,new List<string>()
            {
                "Olá novato! Você deve estar ansioso para aventurar-se mundo a fora buscar os medalhões dos deuses não é mesmo?",
                "Para permitir que você se arrisque nessa jornada eu preciso saber que você tem os conhecimentos minimos para começar seu caminho",
                "Para isso eu quero que você me enfrente numa batalha de criatures."
            }
            },
            {
            TextKey.primeiraPerguntaMeEnfrentar,new List<string>()
            {
                "Você se sente preparado para me enfrentar novato?"
            }
            },
            {
            TextKey.vitoriaSobreCaesar,new List<string>()
            {
                "Você me venceu! Parece que você está pronto para iniciar sua jornada",
                "Mas devo te informar: Eu não usei todo meu potencial de batalha porque eu queria apenas saber se você tinhas os conhecimentos minimos",
                "Pretendo voltar a te testar mais adiante na sua jornada",
                "Uma informação importante que você deve saber ao iniciar o seu caminho é que você pode capturar novos criatures.",
                "Para capturar novos criatures você deve entrar em luta contra eles e deixa-los com a vida baixa",
                "Quandno um criature estiver com a vida baixa você deve usar o item <color=cyan>Carta Luva</color> para iniciar uma tentativa de captura",
                "Para te ajudar no seu começo vou lhe dar algumas cartas luva"
            }
            },
            {
            TextKey.caesarDepoisDeVencido,new List<string>()
            {
                "Nós da resistência planejamos encarar o imperador e com o poder dos nossos criatures destitui-lo de seu poder",
                "O imperador vive recluso numa fortaleza fechada pela magia do acordo dos deuses chamada <color=yellow>Torre da vida eterna</color>",
                "para abrir a torre da vida eterna precisamos juntar oito dos medalhões dos deuses",
                "a porta da torre da vida eterna abrirá sem problemas para os portadores de oito medalhões dos deuses",
                "assim vamos encarar o imperador <color=orange>Logan</color> e devolver a paz e estabilidade para <color=yellow>Orion</color>"
            }
            },
            {
            TextKey.caesarDepoisDeVencidoOsMedalhoes,new List<string>()
            {
                "Para juntar os medalhões dos deuses nós precisamos encontrar as <color=cyan>Arenas divinas dos deuses</color>",
                "Nas arenas dos deuses nós desafiaremos os clérigos regentes. Ao vencer os clérigos eles por obrigação nos darão o brasão dos deuses",
                "Nosso primeiro trabalho será localizar e viajar até cada arena divina para juntar os medalhões"
            }
            }
        };
        
    }
}