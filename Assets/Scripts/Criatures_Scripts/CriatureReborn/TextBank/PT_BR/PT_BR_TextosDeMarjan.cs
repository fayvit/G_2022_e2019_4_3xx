using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TextBankSpace
{

    public static class PT_BR_TextosDeMarjan
    {
        public static Dictionary<TextKey, List<string>> txt = new Dictionary<TextKey, List<string>>()
        {
            { TextKey.marjan1,new List<string>()
            { 
            "Olá estranho! Bem vindo a cidade de <color=yellow>Marjan.</color>",
            "O posto de ancoragem no rio faz com que nossa cidade receba visitas de viajantes rotineiramente",
            "Isso alavancou o comercio proporcionando a cidade a possibilidade de abrir várias lojas",
            "Provavelmente, você não passará por Marjan sem conferir o estoque das lojas, não é mesmo?"
            } },
            { TextKey.marjan2,new List<string>()
            {
            "O culto aos deuses de Orion é uma tradição muito antiga na nossa civilização",
            "Espalhados pelas planícies estão inumeros monumentos aos deuses",
            "Os monumentos aos deuses atraem peregrinos que vem visita-los como sinal de devoção",
            "Em geral, os devotos dos deuses são treinadores de criatures",
            "Como sinal de sua devoção eles travam batalhas amistosas nas proximidades dos monumentos",
            "Você ja enfrentou o devoto de algum deus próximo aos monumentos?"
            } },
            { TextKey.marjan3,new List<string>()
            {
            "Você deve estar adimirado da imensidão do Cruzador de Guerra atracado aqui atrás da cidade não é?",
            "O capitão dele passou aqui por <color=yellow>Marjan</color> e eu consegui conversar com ele",
            "Gente boa o capitão <color=orange>João Cândido</color>. Ele me disse que veio a nossa planície em uma missão",
            "Eu sei que uma curiosidade lhe corroi. Eu sei porque também me corroeria. Somos humanos e sucestiveis a curiosidades não é?",
            "A duvida que pode lhe deixar aflito é: Qual era a missão ultrasecreta que trouxe o capitão até Marjan?",
            "Hora... É ultrasecreta... ninguém deve saber!!",
            "Pois bem...\r\n Eu sei!!",
            "Tudo bem vou te contar. \r\n Ele me contou antes de partir",
            "Mas te peço uma coisa ein...\r\n Não conte para mais ninguém! heheheh",
            "É o seguinte... \r\n Os militares já desconfiam da sanidade do imperador",
            "Sim... da sanidade!! Tanto o exercito quanto a <color=orange>Garra Governamental</color> tem recebido ordens estranhas",
            "A obcessão do imperador por <color=cyan>Gemas Laranjes</color> preocupa muito as entidades imperialistas",
            "Então... O capitão foi tentar encontrar a Sacerdotiza de Ananda para que todos tenham uma luz nos caminhos de Orion",
            "Aparentemente, os militares vão tentar tomar o poder caso sejam convencidos da insanidade do imperador",
            "Isso me assusta!! Não asusta você?"
            } },
            { TextKey.marjan4,new List<string>()
            {
                "Atracado nas margens do <color=yellow>rio Marjan</color> está o <color=yellow>Cruzador de Guerra </color>.",
                "O cruzador de guerra é um navio construido para defender nosso continente das invasões estrangeiras.",
                "Com o prolongar dos tempos de paz, o Cruzador se tornou pouco útil para questões de defesa territorial.",
                "Então os militares resolveram disponibilizar o cruzador para transporte",
                "Ultimamente eles estão fazendo a rota entre nossa cidade e a cidade de <color=yellow>Tamaraset</color>",
                "Proximo a Tamaraset fica uma arena divina para o deus <color=cyan>Cianus</color>"
            } },
            { TextKey.marjan5,new List<string>()
            {
                "Alguns devotos dos deuses travam batalhas em homenagem ao seu deus de adoração.",
                "Para os que os vencem em batalha eles entregam um pequeno brasão de seu deus",
                "Rezam as lendas de Orion que entregar os brasões em estatuas em homenagem aos deuses lhe dará a benção do deus",
                "Eu queria a benção de um deus, mas não sou bom em vencer batalhas contra os devotos."
            } },
             { TextKey.marjan6,new List<string>()
            {
                "Tenho visto muitas arbitrariedades por parte do império",
                "Pouco tempo atrás um vizinho fez reclamações sobre o império",
                "Como resposta, sua casa foi invadida na madrigada,",
                "eram agentes do império fazendo vigilancia ideológica.",
                "Acabei viajando para <color=yellow>Tamaraset</color>, e indo visitar o ferreiro do <color=yellow>morro Izan</color>",
                "Lá eu comprei uma corrente bem forte para ajudar na segurança da minha casa",
                "Não quero ser surpreendido por agentes do império na madrugada em minha casa"
            } },
             { TextKey.marjan7,new List<string>()
            {
                "Proximo a cidade de <color=yellow>Tamaraset</color> existe uma usina de produção de eletricidade",
                "Da usina vem o fornecimento de energia da nossa cidade",
                "Já ouvi ameaças do império de cortar o nossofornecimento de energia ",
                "Na verdade, não acredito nessa ameaça, por causa do <color=cyan>Armagedom.</color>",
                "Sem energia a conexão com Armagedom seria perdida e o imperio teria menos controle sobre a cidade",
                "Eles precisam da conexão com Armagedom para manterem-se atualizados com informações da cidade",
                "Afinal, conhecimento sobre o que acontece, também é um poder."
            } },
             { TextKey.marjan8,new List<string>()
            {
                "Próximo a cidade de <color=yellow>Tamaraset</color> existe uma mina de <color=cyan>Gemas laranjes</color>.",
                "A mina parece estar se esgotando e tem recebido pouca atenção do império",
                "mas ainda fornece uma pequena quantidade de gemas",
                "Dizem que a partir da mina existe um caminho que leva a planicie da cidade <color=yellow>Frigida</color>"
            } },
             { TextKey.npcAarmgdMarjan,new List<string>()
            {
                "Muito bem vindo ao Armagedom da cidade de <color=yellow>Marjan</color>",
                "O posto de ancoragem no rio coloca a nossa cidade no mapa do império",
                "Pessoas importantes acabam passando por aqui a caminho de pontos importante do império",
                "Para qual parte do império você está indo?"
            } },
             { TextKey.npcAarmgdMarjanCurto,new List<string>()
            {
                "Muito bem vindo ao Armagedom da cidade de <color=yellow>Marjan</color>"
            } },
             { TextKey.npcArmagedomConversar,new List<string>()
            {
                "Tenho conversado com outros coordenadores de <color=cyan>Armagedom</color> e há um clima de medo nos canais oficiais de comunicação",
                "Há noticias de um grupo de resistência ao império que pode abalar as estruturas de poder como nós as conhecemos",
                "Se eu não me engano o império está os chamando de Pesçonhentos",
                "Quando a gente pensar que a nossa vida está entrando num eixo de tranquilidade, surge um temor que coloca em cheque o amanhã"
            } },
              { TextKey.npcShopMarjanGenerico,new List<string>()
            {
                "Seja bem vindo a loja de <color=yellow>Marjan</color>"
            } },
              { TextKey.npcShopVermelhoMarjan,new List<string>()
            {
                "Eu adoro ter a minha loja proximo da arena divina de <color=cyan>Ananda</color>",
                "Por algumas vezes eu tive contado direto com a sacerdotiza regente da arena",
                "Ela se chama <color=orange>Amanda Leticia</color> e adora treinar criatures do tipo normal",
                "O que acaba deixando a batalha na arena um pouco fácil para os que treinan criatures do tipo pedra"
            } },
              { TextKey.npcShopVerdeMarjan,new List<string>()
            {
                "Cada tipo de criature recupera seus pontos de energia com um tipo de item",
                "Atualmente eu tenho itens para recuperar os pontos de energia de quase todos os tipos de criatures",
                "Também existem itens que recuperam os pontos e energia de qualquer criature, mas esses são um pouco dificil de conseguir",
                "Um dos melhores itens nesse sentido é o <color=cyan>Pergaminho de Perfeição</color>",
                "O pergaminho de perfeição recupera todos os PVs e todos PEs de um criature",
                "E ainda retira status negativos como envenenamento e amedrontamento",
                "Eu gostaria de poder vender pergaminhos de perfeição, mas realmente, não são tão fáceis de conseguir."
            } },
              { TextKey.npcShopLaranjaMarjan,new List<string>()
            {
                "Nosso suprimento de pergaminhos vem direto da cidade de <color=yellow>Cyzor</color>",
                "Em Cyzor reside um sacerdote do deus da sabedoria <color=cyan>Log</color>",
                "O nome dele é <color=orange>Baltazar Gladist</color>",
                "Ele é o responsável por escrever a maioria dos pergaminhos que se encontram em circulação em Orion",
                "Ultimamente temos cada vez menos pergaminhos em circulação.",
                " Os preços praticados por Baltazar Gladist acabam restringindo muito o comercio de pergaminhos",
                "Cada vez que preciso repor meu estoque ele cobra mais caro pelos pergaminhos"
            } },
        };
    }
}
