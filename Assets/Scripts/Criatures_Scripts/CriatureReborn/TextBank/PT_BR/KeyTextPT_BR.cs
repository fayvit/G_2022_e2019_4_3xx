using UnityEngine;
using System.Collections.Generic;

namespace TextBankSpace
{
    public static class KeyTextPT_BR
    {
        public static Dictionary<TextKey, List<string>> txt = new Dictionary<TextKey, List<string>>()
            {
                {TextKey.bomDia, new List<string>()
                    {
                        "bom dia pra você",
                        "bom dia pra você",
                        "bom diaaaaaaaaaaa....",
                        "bom dia pra você"
                    }},
                {TextKey.nomeTipos, new List<string>()
                    {
                        "Agua",
                        "Fogo",
                        "Planta",
                        "Terra",
                        "Pedra",
                        "Psiquico",
                        "Eletrico",
                        "Normal",
                        "Veneno",
                        "Inseto",
                        "Voador",
                        "Gas",
                        "Gelo"
                } },
                {
                    TextKey.apresentaInimigo,new List<string>()
                    {
                        "Você encontrou um <color=orange>{0}</color> Nivel {1} \n\r PV: {2} \t\t\t PE: {3}",
                        "O treinador {0}",
                        " enviou um <color=orange>{0}</color> Nivel {1} \n\r PV: {2} \t\t\t PE: {3}"
                    }
                },
                {
                    TextKey.usoDeGolpe,new List<string>()
                    {
                    "Golpe em tempo de recarga. \r\n{0}\r\n até o próximo uso. ",
                    "Não há pontos de energia suficientes para esse golpe"
                    }
                },
                {
                    TextKey.apresentaFim,new List<string>()
                    {
                        "{0} derrotou {3}, pela vitória {0} recebeu {1} <color=#FFD700> pontos de experiencia.</color> e você recebeu {2} <color=#FF4500> CRISTAIS</color>"
                    }
                },
                {
                    TextKey.apresentaDerrota,new List<string>()
                    {
                        "<color=orange>{0}</color> foi derrotado",
                        "Qual criature sairá para continuar a batalha?",
                        "Todos os seus criatures foram derrotados.\n\r Agora você deve voltar para o armagedom, curar seus criatures e voltar para a sua aventura"
                    }
                },
                {
                    TextKey.criatureParaMostrador,new List<string>()
                    {
                        "Você tem certeza que quer colocar <color=orange>{0}</color> para continuar a luta?",
                        "O criature <color=orange>{0}</color> não está em condições para entrar na luta",
                        "O criature <color=orange>{0}</color> está desmaiado e não pode entrar",

                    }
                },
                {
                    TextKey.passouDeNivel,new List<string>()
                    {
                        "{0} conseguiu alcançar o <color=yellow>nível {1}</color>"
                    }
                },
     //           {
     //               TextKey.naoPodeEssaAcao,new List<string>()
     //               {
     //                   "<color=orange>Seu personagem não está em condições de realizar essa ação</color>",
     //                   "<color=orange>O Criature {0} já está em campo</color>",
     //                   "<color=orange>Selecione um item antes de clicar no botão usar</color>"
     //               }
     //           },
     //           {
     //               TextKey.jogoPausado,new List<string>()
     //               {
     //                   "Jogo Pausado"
     //               }
     //           },
     //           {
     //               TextKey.selecioneParaOrganizar,new List<string>()
     //               {
     //                   "Selecione o item para ser reposicionado",
     //                   "Selecione o item para trocar de posição com {0}"
     //               }
     //           },
                {
                    TextKey.emQuem,new List<string>()
                    {
                        "<color=yellow>Em qual criature você irá usar o item {0}</color>"
                    }
                },
                {
                    TextKey.aprendeuGolpe,new List<string>()
                    {
                        "{0} aprendeu o ataque <color=yellow>{1}</color>",
                        "{0} está tentando aprender o ataque <color=yellow>{1}</color>, mas para isso precisa esquecer um ataque.",
                        "Qual ataque {0} irá esquecer?",
                        "Você tem certeza que deseja esquecer o ataque <color=yellow>{0}</color> para aprender o ataque <color=yellow>{1}</color>??",
                        "<color=orange>{0}</color> deixará de aprender o golpe <color=yellow>{1}</color>. Você está certo disso?",
                        "<color=orange>{0}</color> esqueceu <color=yellow>{1}</color> e aprendeu <color=yellow>{2}</color>",
                        "<color=orange>{0}</color> não aprendeu o ataque <color=yellow>{1}</color> e manteve seus quatro ataques conhecidos"
                    }
                },
     //           {
     //               TextKey.tentandoAprenderGolpe,new List<string>()
     //               {
     //                  "{0} está tentando aprender o ataque <color=yellow>{1}</color>, mas para isso precisa esquecer um ataque."
     //               }
     //           },
     //           {
     //               TextKey.precisaEsquecer,new List<string>()
     //               {
     //                   "Qual ataque {0} irá esquecer?"
     //               }
     //           },
     //           {
     //               TextKey.certezaEsquecer,new List<string>()
     //               {
     //                   "Você tem certeza que deseja esquecer o ataque <color=yellow>{0}</color> para aprender o ataque <color=yellow>{1}</color>??"
     //               }
     //           },
     //           {
     //               TextKey.naoQueroAprenderEsse,new List<string>()
     //               {
     //                   "<color=orange>{0}</color> deixará de aprender o golpe <color=yellow>{1}</color>. Você está certo disso?"
     //               }
     //           },
               {
                   TextKey.aprendeuGolpeEsquecendo,new List<string>()
                   {
                       "<color=orange>{0}</color> esqueceu <color=yellow>{1}</color> e aprendeu <color=yellow>{2}</color>"
                   }
               },
               {
                   TextKey.naoAprendeuGolpeNovo,new List<string>()
                   {
                       "<color=orange>{0}</color> não aprendeu o ataque <color=yellow>{1}</color> e manteve seus quatro ataques conhecidos"
                   }
               },
                {
                    TextKey.foiParaArmagedom,new List<string>()
                    {
                        "A luva de Guarde só pode carregar {0} Criatures. Então <color=orange>{1}</color> nivel {2} foi enviado para o Armagedom"
                    }
                },
                {
                    TextKey.primeiroArmagedom,new List<string>()
                    {
                        "Olá Estranho!!\n\r Seja bem vindo à nossa estação de conexão com <color=cyan>Armagedom.</color>",
                        "No armagedom você pode curar seus criature além de substituir os criatures atuais pelos que estão na sua reserva",
                        "A resistência acaba fazendo conexões clandestinas ao Armagedom já que a rede é imperial e " +
                        "podemos não ser bem recebidos em estações oficiais"
                    }
                },
                {
                    TextKey.primeiroArmagedomMensCurta,new List<string>()
                    {
                        "Olá!!\n\r É bom saber que nosso mini armagedom conta com a sua confiança",
                        
                    }
                },
                {TextKey.simOuNao,new List<string>()
                    {
                        "Sim",
                        "Não"
                    }},
                {TextKey.comprarOuVender,new List<string>()
                    {
                        "Comprar",
                        "Vender",
                        "Conversar",
                        "Sair"
                    }},
                {
                    TextKey.frasesDeArmagedom,new List<string>()
                    {
                        "Seus criatures estão curados, estranho!!",
                        "Me desculpe estranho, mas parece que não há criatures teus guardados no armagedom",
                        "Seus Criatures Armagedados",
                        "O criature <color=orange>{0}</color> nivel {1} entrou para o seu time",
                        "Seu time já tem o número máximo de Criatures. Para colocar <color=orange>{0}</color> nivel {1} no time você precisa retirar um Criature do seu time",
                        "Qual Criature sairá do seu time?",
                        "O Criature {0} nivel {1} entrou no seu time no lugar do Criature {2} nivel {3}",
                        "Tem certeza que retirará {0} nivel {1} do seu time?",
                        "Eu posso lhe vender cada pergaminho de armagedom por <color=yellow>{0}</color> Cristais. Você quer comprar?",
                        "No que posso te ajudar?"
                    }
                },
     //           {
     //               TextKey.shopBasico,new List<string>()
     //               {
     //                   "Seja muito bem vindo a  minha loja estranho.",
     //                   "No que posso te ajudar??"
     //               }
     //           },
                {
                    TextKey.frasesDeShoping,new List<string>()
                    {
                        "Tenho excelentes produtos pra você estranho. Gostaria de comprar algo?",
                        "O que você gostaria de vender? estranho...",
                        "Muito obrigado por sua compra, estranho...",
                        "Excelente negocio, estranho...",
                        "Espero fazer negocios com você novamente, estranho...",
                        "No que posso te ajudar?",
                        "Me desculpe estranho mas parece que tudo que eu tinha desse item já foi vendido.",
                        "A menor quantidade que você pode comprar é 1",
                        "A maior quantidade que você pode comprar é {0}",//ID=8
                        "Desculpe estranho, mas parece que você só pode pagar por {0} unidades.",
                        "Desculpe estranho, mas parece que você não tem cristais suficientes para pagar por esse item",
                        "Me desculpe estranho mas eu não posso comprar esse item",
                        "Parece que você não tem nenhum item para vender."
                    }
                },
     //   {
     //       TextKey.textosParaQuantidadesEmShop,new List<string>()
     //       {
     //           "CRISTAIS: ",
     //           "valor a pagar",
     //           "valor a receber",
     //           "Você quer comprar qual quantidade de {0} ??",
     //           "Você quer vender qual quantidade de {0} ??",
     //           "Comprar",
     //           "Vender",
     //           "Infelizmente os cristais que você carrega só lhe permitem comprar {0} {1}.",
     //           "Infelizmente você só possui {0} {1} para vender.",
     //           "A quantidade mínima que você pode comprar é 1",
     //           "A quantidade mínima que você pode vender é 1",
     //           "Você não tem o suficiente para comprar esse item"
     //       }
     //   },
     //   {
     //       TextKey.certezaExcluir,new List<string>()
     //       {
     //           "Tem certeza que deseja excluir o Save {0} ?"
     //       }
     //   },
                {TextKey.itens,new List<string>()
                {
                    "Voce não pode usar esse item nesse momento.",
                    "Ele não precisa usar esse item nesse momento.",
                    "O criature {0} está desmaiado e não pode usar esse item nesse momento.",//O {0} será substituito pelo nome do Criature
					"Somente Criatures do tipo {0} podem usar esse item",
                    "O criature {0} não pode usar o item {1} pois ele já sabe o golpe {2}",
                    "O Criature {0} não pode aprender o golpe {1}",
                    "{0} não usou o item {1}",
                    "Tem certeza que deseja usar o item {0} ?",
                    " Você não pode usar esse item nesse local",
                    "{0} não precisa usar esse item nesse momento",
                    "Você não pode usar esse item pelo menu"/*ID=10*/
                }},
                {TextKey.bau,new List<string>()
                {
                    "Você encontrou um báu. Quer abri-lo?",
                    "O báu está vazio",
                    "Dentro do báu Você consegue <color=cyan>{0} {1}</color>",
                    "Com a vitória você consegue <color=cyan>{0} {1}</color>"
                }},
     //           {TextKey.resetPuzzle,new List<string>()
     //           {
     //               "Gostaria de retornar os elementos as suas posições iniciais?"
     //           }},
     //           {TextKey.viajarParaArmagedom,new List<string>()
     //           {
     //               "<color=cyan>Para qual Armagedom gostaria de viajar?</color>"
     //           }},
     //           {TextKey.Voltar,new List<string>()
     //           {
     //               "Voltar"
     //           }},
     //           {TextKey.ObrigadoComPressa,new List<string>()
     //           {
     //               "Obrigado, mas estou com pressa ..."
     //           }},

                {TextKey.tentaCapturar,new List<string>()
                {
                    "{0} resistiu a tentativa de Captura.",
                    "A sua luva de guarde so pode carregar ",
                    "Então: ",
                    " Nível ",
                    "foi enviado para o ",
                    "Você conseguiu capturar um {0}"
                }},

                {TextKey.listaDeItens,new List<string>()
                {
					/*ID==0*/"Maçã", "Burguer", "Carta Luva",
                    "Gasolina","Água Tônica",/*ID==5*/"Regador","Estrela","Quartzo","Adubo",
                    "Seiva",/*ID==10*/"Inseticida","Aura","Repolho com Ovo","Ventilador","Pilha",
					/*ID==15*/"Gelo Seco","Pergaminho de Fuga","Segredo","Estatua Misteriosa",
                    "Cristais","Pergaminho de Perfeição","Antidoto","Amuleto da Coragem","Tônico",/*ID = 24*/
					"Perg. Rajada de Agua","Pergaminho de Saída","Condecoração Alpha","Perg. de Armagedom","Pergaminho de Sabre","Perg. da Gosma de Inceto",
                    "Perg. GosmaAcida","Perg.Multiplicar","Perg. Ventânia",/*ID = 33*/
                    "Perg. Ventos Cortantes","Perg. Olhar Enfraquecedor","Perg.Olhar Mal","Condecoração Beta","Perg. do Furacão de Folhas","Explosivos","Medalhão das Águas",
                    "Tinteiro Sagrado de Log","Perg. de Laurense","Perg. de Boutjoi","Perg. de Ananda",/*ID = 45*/
                    "Caneta Sagrada de Log","Perg. de Sinara","Perg. de Alana","Perg. de Tempestade Eletrica","Brasão de Laurense","Brasão de Ananda",
                    "Brasão de Boutjoi","Perg. do Turbo de Agua","Perg. da Hidro Bomba","Perg. da Lâmina de Folhas","Perg. da Tempestade de Folhas",/*ID=54*/
                    "Perg. da Bola de Fogo","Perg. da Rajada de Fogo","Perg. do Toste Ataque",

                }},


                {TextKey.mensLuta,new List<string>()
                {
                    "Você não pode usar esse item nesse momento.",
                    "Você usa {1}",
                    "{0} não precisa usar esse item nesse momento",
                    "Você não pode tentar capturar criatures de um treinador",
                    "Você não pode usar pergaminho de fuga contra treinadores",
                    "Trave a mira em um criature para usar esse item",
                    "Você não pode retornar para o controle do heroi enquanto luta contra um treinador"
                }},

                {TextKey.shopInfoItem,new List<string>()
                {
                    " Maçã recupera 40 PV de um Criature",
                    " Burguer recupera 100 PV de um Criature",
                    " Carta Luva é usada para tentar capturar novos Criatures",
                    " Gasolina recupera 40 PE de um Criature do tipo Fogo",
                    " Água Tônica recupera 40 PE de um Criature do tipo Água",
                    " Regador recupera 40 PE de um Criature do tipo Planta",
                    " Estrela recupera 40 PE de um Criature do tipo Normal",
                    " Quartzo recupera 40 PE de um Criature do tipo Pedra",
                    " Adubo recupera 40 PE de um Criature do tipo Terra",
                    " Seiva recupera 40 PE de um Criature do tipo Inseto",
                    " Inseticida recupera 40 PE de um Criature do tipo Veneno",
                    " Aura recupera 40 PE de um Criature do tipo Psiquico",
                    " Repolho com Ovo recupera 40 PE de um Criature do tipo Gás",
                    " Ventilador recupera 40 PE de um Criature do tipo Voador",
                    " Pilha recupera 40 PE de um Criature do tipo Eletrico",
                    " Gelo Seco recupera 40 PE de um Criature do tipo Gelo",
                    " Quando lido esse pergaminho invoca uma magia para expulsar o oponente da luta ",
                    " Um item muito suspeito encostado no fundo da loja",
                    " Uma estatua feita de pedra amarelada em pose imponente",
                    " Cristais é a unidade monetaria de Orion",
                    " Quando lido esse pergaminho, o criature alvo recupera totalmente os PVs e os PEs além de remover os status negativos ",
                    " Cura Criatures que estão envenenados ",
                    " Devolve a coragem para Criatures amedrontados ",
                    " Cura Criatures enfraquecidos",
                    " O pergaminho de Rajada de Agua pode ensinar o golpe Rajada de Agua para alguns Criatures",
                    " Pode ser usado em lugares fechado para te teletransportar para fora",
                    " A condecoração que você recebeu do Capitão Atos Aramis.",
                    " O pergaminho de Armagedom te teletransporta para o um Armagedom que você visitou.",
                    "O pergaminho de Sabre ajuda um Criature a aprender o golpe Sabre",
                    " O pergaminho da Gosma pode ensinar o golpe Gosma de Inseto para alguns Criatures",
                    " O pergaminho da Gosma Acida ajuda um Criature Inseto a aprender o golpe Gosma Acida",
                    " O pergaminho do Multiplicar insetos ajuda um Criature Inseto a aprender o golpe Multiplicar",
                    " O pergaminho da Ventânia ajuda um Criature Voador a aprender o golpe Ventânia",
                    " O pergaminho dos Ventos Cortantes ajuda um Criature Voador a aprender o golpe Ventos Cortantes",
                    " O pergaminho do Olhar Enfraquecedor ajuda um Criature a aprender o golpe Olhar Enfraquecedor",
                    " O pergaminho do Olhar Mal ajuda um Criature a aprender o golpe Olhar Mal",
                    " A condecoração que você recebeu do Capitão Espinha de Peixe.",
                    " O pergaminho do Furacão de Folhas pode ensinar o golpe Furacão de Folhas para um Criature com a capacidade de aprende-lo",
                    "Os Explosivos necessários para desobstruir o caminho para Afarenga",
                    "O medalhão das Águas do Deus Drag conseguido com Omar Water",
                    "Um recipiente contendo a tinta utilizada pelos sacerdotes de Log para escrever pergaminhos",
                    "O pergaminho em nome de Laurense o deus da força, quando lido, aumenta 1 ponto de ataque de um criature",
                    "O pergaminho em nome de Boutijoi o deus da natureza, quando lido, aumenta 1 ponto de defesa de um criature",
                    "O pergaminho em nome de Ananda a deusa da magia, quando lido, aumenta 1 ponto de poder de um criature",
                    "A caneta usada por sacerdotes de Log para escrever pergaminhos mágicos",
                    "O pergaminho em nome de Sinara, deusa mãe dos criatures, quando lido, aumenta em 4 pontos a vida máximo de um criature",
                    "O pergaminho em nome de Alana, deusa da fertilidade, quando lido, aumenta em 4 pontos a energia máxima de um criature",
                    "O pergaminho de Tempestade Eletrica pode ensinar o golpe Tempestade Eletrica para um Criature com a capacidade de aprende-la",
                    "Um brasão em homenagem a Laurense o deus da força e da coragem",
                    "Um brasão em homenagem a Ananda a deusa da magia",
                    "Um brasão em homenagem a Boutjoi o deus da natureza, terras e plantações",
                    " O pergaminho pode ensinar o golpe Turbo de Agua para um Criature com a capacidade de aprende-lo",
                    " O pergaminho pode ensinar o golpe Hidro Bomba para um Criature com a capacidade de aprende-lo",
                    " O pergaminho pode ensinar o golpe Lâmina de Folhas para um Criature com a capacidade de aprende-lo",
                    " O pergaminho pode ensinar o golpe Tempestade de Folhas para um Criature com a capacidade de aprende-lo",
                    " O pergaminho pode ensinar o golpe Bola de Fogo para um Criature com a capacidade de aprende-lo",
                    " O pergaminho pode ensinar o golpe Rajada de Fogo para um Criature com a capacidade de aprende-lo",
                    " O pergaminho pode ensinar o golpe Toste Ataque para um Criature com a capacidade de aprende-lo",

                }},
                {TextKey.textoBaseDeAcao,new List<string>()
                {
                    "Conversar",
                    "Examinar",
                    "Empurrar"
                }
                },{TextKey.armagedomNames,new List<string>()
                {
                    "Acampamento da Resistência",
                    "Segundo Armagedom"
                }
                },
                {
            TextKey.usarPergaminhoDeSinara,new List<string>()
            {
                "O pergaminho de Sinara pode aumentar em 4 pontos o PV max de um criature. Em quem gostaria de usar?",
                "O criature {0} nivel {1} aumentou 4 pontos em seu PV max"
            }
        },
                {
            TextKey.usarPergaminhoDeAlana,new List<string>()
            {
                "O pergaminho de Alana pode aumentar em 4 pontos o PE max de um criature. Em quem gostaria de usar?",
                "O criature {0} nivel {1} aumentou 4 pontos em seu PE max"
            }
        },
        {
            TextKey.usarPergaminhoDeLaurense,new List<string>()
            {
                "O pergaminho de Laurense pode aumentar 1 ponto de Ataque de um criature. Em quem gostaria de usar?",
                "O criature {0} nivel {1} aumentou 1 ponto em seu ataque"
            }
        },

        {
            TextKey.usarPergaminhoDeBoutjoi,new List<string>()
            {
                "O pergaminho de Boutjoi pode aumentar 1 ponto de Defesa de um criature. Em quem gostaria de usar?",
                "O criature {0} nivel {1} aumentou 1 ponto em sua defesa"
            }
        },
        {
            TextKey.usarPergaminhoDeAnanda,new List<string>()
            {
                "O pergaminho de Ananda pode aumentar 1 ponto de Poder de um criature. Em quem gostaria de usar?",
                "O criature {0} nivel {1} aumentou 1 ponto em seu poder"
            }
        },
     //   {
     //       TextKey.ComecandoConversaComIan,new List<string>()
     //       {
     //           "Olá viajante!! Gostaria que eu escrevesse pergaminhos para você?",
     //           ""
     //       }
     //   },
     //   {
     //       TextKey.opcoesDeIan,new List<string>()
     //       {
     //           "Escrever pergaminho de Sinara",
     //           "Escrever pergaminho de Alana",
     //           "Conversar",
     //           "Sair"
     //       }
     //   },
     //   {
     //       TextKey.conversaBasicaDeIan,new List<string>()
     //       {
     //           "Eu sou um estudante, estudo para me tornar um sacerdote de <color=cyan>Log</color>, o deus da sabedoria e conhecimento",
     //           "Por isso, ainda sei escrever poucos pergaminhos, na verdade, apenas dois que posso dizer uteis",
     //           "O pergaminho de Sinara, deusa dos criatures, quando lido aumenta em 4 pontos o limite de vida de um criature",
     //           "O pergaminho de Alana, deusa da fertilidade, quando lido aumenta em 4 pontos o limite de energia de um criature",
     //           "Espero que esse pergaminhos sejam úteis pra você viajante"
     //       }
     //   },
     //   {
     //       TextKey.conversaBasicaDeIan2,new List<string>()
     //       {
     //           "Estou numa jornada em direção ao norte, tenho contas a acertar com um sujeitinho na cidade de <color=yellow>Cyzor</color>",
     //           "Ficarei aqui por mais algum tempo, mas logo precisarei retomar minha viajem",
     //           "Portanto, se quiser que escreva pergaminhos para você, volte logo,",
     //           "caso contrario, poderá não me encontrar mais aqui"
     //       }
     //   },
     //   {
     //       TextKey.despedidabasicaDeIan,new List<string>()
     //       {
     //           "Até mais viajante, volte logo para que eu escreva mais pergaminhos para você. Logo voltarei para minha jornada"
     //       }
     //   },
     //   {
     //       TextKey.frasesDeVendaDeIan,new List<string>()
     //       {
     //           "Para que eu escreva um {0} para você, preciso de {1} tinteiro sagrado de Log e {2} cristais. Gostaria que eu escrevesse?",
     //           "Infelizmente, parece que você não tem os requisitos necessários para a compra, viajante",
     //           "Ho How, parece que teremos um pergaminho!!",
     //           "Receba seu pergaminho viajante",
     //           "Gostaria que eu escrevesse mais um pergaminho?",
     //       }
     //   },
        {
            TextKey.frasesDaLutaContraTreinador,new List<string>()
            {
                "Nesta luta eu utilizarei {0} criatures",
                "Para inicar eu escolho...",
                "Parece que você teve uma vitória parcial, mas isso ainda não terminou",
                "Meu proximo criature será...",
                "O treinador <color=orange>{0}</color> enviou um <color=yellow>{4}</color> Nivel {1} \n\r PV: {2} \t\t\t PE: {3}",
            }
        },
     //   {
     //       TextKey.usarPergaminhoDeSinara,new List<string>()
     //       {
     //           "O pergaminho de Sinara pode aumentar em 4 pontos o PV max de um criature. Em quem gostaria de usar?",
     //           "O criature {0} nivel {1} aumentou 4 pontos em seu PV max"
     //       }
     //   },
     //   {
     //       TextKey.usarPergaminhoDeAlana,new List<string>()
     //       {
     //           "O pergaminho de Alana pode aumentar em 4 pontos o PE max de um criature. Em quem gostaria de usar?",
     //           "O criature {0} nivel {1} aumentou 4 pontos em seu PE max"
     //       }
     //   },
        {
            TextKey.nomesDosGolpes,new List<string>()
            {
                "Rajada de Água","Turbo de Água","Bola de Fogo","Rajada de Fogo", "Lâmina de Folhas","Furacão de Folhas","Chifre","Tapa",
                "Garra",
                "Chicote de Mão","Dentada","Bico",/*ID=13*/"Ventânia","Ventos Cortantes","Gosma De Inseto","Gosma Acida","Chicote de Calda",
                "Cabeçada",/*ID=19*/"Eletricidade","Eletricidade Concentrada","Agulha Venenosa","Onda Venenosa","Chute","Espada",
                "Sobre Salto",/*ID=26*/"Cascalho","Pedregulho","Rajada de Terra","Energia de Garras","Vingança da Terra","Psicose",
                "Hidro Bomba","Bola Psiquica","Toste Ataque","Tempestade de Folhas","Chuva Venenosa","Multiplicar","Tempestade Eletrica",
                "Avalanche",/*ID=40*/"Anel do Olhar","Olhar Mal","Cortina de Terra","Teletransporte","Sobre Voo","Olhar Paralisante",
                "Bomba de Gás","Rajada de Gás","Cortina de Fumaça","Bastão","Sabre de Asa","Sabre de Bastão","Sabre de Nadadeira",
                "Sabre de Espada","Tesoura",/*ID=55*/"Ataque em Giro","Impulso Aquático","Impulso Flamejante","Deslizamento em Gosma",
                "Flash Psiquico",/*ID=60*/"Impulso Eletrico","Pedra Partida","Terra Venenosa","Turbilhão Veloz","Nuvem de Terra",
                "Spray Toxico",/*ID=66*/"Espiral de Cipó"
            }
        },
     //   {
     //       TextKey.preparadoParaMeEnfrentar,new List<string>()
     //       { "Você está preparado para me enfrentar?"}
     //   },
     //   {
     //       TextKey.itemEmTempoDeRecarga,new List<string>()
     //       { "O uso de itens está em tempo de recarga"}
     //   },
        {
            TextKey.status,new List<string>()
            {
                "<color=yellow>Atenção</color>\r\n Seu Criature <color=orange>{0}</color> desmaiou por envenenamento",
                "Seu Criature <color=orange>{0}</color> sofreu {1} PV de dano por envenenamento",
                "O criature Inimigo sofreu {0} PV de dano por envenenamento"
            }
        },
        {
            TextKey.nomeStatus,new List<string>()
            {
                "envenenado",
                "fraco",
                "amedrontado"
            }
        },
            { 
            TextKey.blocoBarreiraBloqueado,new List<string>()
            { 
                "Certifique-se que a direção de movimento não está bloqueada"    
            }
        },
            { 
            TextKey.menuInicial,new List<string>()
                { "Iniciar Jogo", "Carregar jogo salvo", "Linguagem", "Creditos" }
            },
     //   {
     //       TextKey.menuDePause,new List<string>()
     //       {
     //           "Status", "Itens", "Coleção", "Voltar ao título", "Voltar ao jogo"
     //       }
     //   },
        {
            TextKey.menuDeArmagedom,new List<string>()
            {
                "Curar criatures",
                "Meus criatures armagedados",
                "Conversar",
                "Voltar ao Jogo"
            }
        },{
            TextKey.godPushPuzzleActivate,new List<string>()
            {
                "Gostaria de usar {0} {1} para ativar o desafio {2}",
                "da força",
                "do poder",
                "da resistência",
                "Você não tem {0} {1}!"
            }
        },
            {
            TextKey.textosDeNaoPodeUsar,new List<string>()
            {
                "Sem <color=#00ff00>estâmina</color> para realizar a ação",
                "Não há <color=cyan>pontos de energia</color> para realizar a ação"
            }
        },

    };
    }
}