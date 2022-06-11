using UnityEngine;
using System.Collections.Generic;

namespace TextBankSpace
{
    public static class KeyTextEN_G
    {
        public static Dictionary<TextKey, List<string>> txt = new Dictionary<TextKey, List<string>>()
            {
                {TextKey.bomDia, new List<string>()
                    {
                        "goog morning to you",
                        "goog morning to you",
                        "good morniiiiiiiiiing...",
                        "goog morning to you"
                    }},
                {TextKey.nomeTipos, new List<string>()
                    {
                        "Water",
                        "Fire",
                        "Plant",
                        "Ground",
                        "Rock",
                        "Psiquic",
                        "Eletric",
                        "Normal",
                        "Poison",
                        "Bug",
                        "Flying",
                        "Gas",
                        "Ice"
                } },
                {
                    TextKey.apresentaInimigo,new List<string>()
                    {
                        "You have found a <color=orange>{0}</color> Level {1} \n\r LP: {2} \t\t\t EP: {3}",
                        "The trainer {0}",
                        " Sent a <color=orange>{0}</color> Level {1} \n\r LP: {2} \t\t\t EP: {3}"
                    }
                },
                {
                    TextKey.usoDeGolpe,new List<string>()
                    {
                    "Attack at recharge time. \r \n {0} \r \n until the next use.",
                    "There are not enough energy points for this blow"
                    }
                },
                {
                    TextKey.apresentaFim,new List<string>()
                    {
                        "{0} defeated {3}, for victory {0} received {1} <color=#FFD700> XP points.</color> and you receive {2} <color=#FF4500> CRYSTALS</color>"
                    }
                },
                {
                    TextKey.apresentaDerrota,new List<string>()
                    {
                        "{0} was defeated",
                        "Which creature will you leave to continue the battle?",
                        "All your creatures have been defeated. \n\r Now you must go back to armageddon, heal your criatures and return to your adventure"
                    }
                },
                {
                    TextKey.criatureParaMostrador,new List<string>()
                    {
                        "Are you sure you want to put <color=orange> {0} </color> to continue the fight?",
                        "The creature <color=orange> {0} </color> is not fit to enter the fight",
                        "The creature <color=orange> {0} </color> is fainted and can not enter"

                    }
                },
                {
                    TextKey.passouDeNivel,new List<string>()
                    {
                        "{0} was able to reach the <color=yellow> level {1} ​​</color>"
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
                        "<color=yellow> In which criature you will use the item {0} </color>"
                    }
                },
                {
                    TextKey.aprendeuGolpe,new List<string>()
                    {
                        "{0} learned the attack <color=yellow> {1} </color>",
                        "{0} is trying to learn the attack <color=yellow> {1} </color>, but for this he needs to forget an attack.",
                        "Which attack {0} will forget?",
                        "Are you sure you want to forget the <color=yellow> {0} </color> attack to learn the attack <color=yellow> {1} </color> ??",
                        "<color=orange> {0} </color> will stop learning the stroke <color=yellow> {1} </color>. Are you sure about this?",
                        "<color=orange> {0} </color> forgot <color=yellow> {1} </color> and learned <color=yellow> {2} </color>",
                        "<color=orange>{0}</color> not learning the attack <color=yellow>{1}</color> to maintain your four attacks learning"
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
     //           {
     //               TextKey.aprendeuGolpeEsquecendo,new List<string>()
     //               {
     //                   "<color=orange>{0}</color> esqueceu <color=yellow>{1}</color> e aprendeu <color=yellow>{2}</color>"
     //               }
     //           },
     //           {
     //               TextKey.naoAprendeuGolpeNovo,new List<string>()
     //               {
     //                   "<color=orange>{0}</color> não aprendeu o ataque <color=yellow>{1}</color> e manteve seus quatro ataques conhecidos"
     //               }
     //           },
                {
                    TextKey.foiParaArmagedom,new List<string>()
                    {
                        "The Your Guarde glove can only carry {0} Criatures. Then <color=orange> {1} </color> level {2} was sent to Armageddon"
                    }
                },
     //           {
     //               TextKey.primeiroArmagedom,new List<string>()
     //               {
     //                   "Olá Estranho!!\n\r Seja bem vindo ao Armagedom Local.",
     //                   "No que posso te ajudar?"
     //               }
     //           },
                {TextKey.simOuNao,new List<string>()
                    {
                        "Yes",
                        "No"
                    }},
     //           {TextKey.comprarOuVender,new List<string>()
     //               {
     //                   "Comprar",
     //                   "Vender"
     //               }},
                {
                    TextKey.frasesDeArmagedom,new List<string>()
                    {
                        "Your criatures are healed, strange !!",
                        "I'm sorry stranger, but it seems like there are no criatures your saved at armageddon",
                        "Your Criatures in armagedom",
                        "O criature <color=orange> {0} </color> nivel {1} ​​joined your team.",
                        "Your team already has the maximum number of Criatures. To put <color=orange> {0} </color> level {1} ​​on the team you need to remove a Criature from your team",
                        "Which Criature will leave your team?",
                        "Criature {0} Level {1} joined the team in your place on Criature {2} level {3}",
                        "Are you sure you will remove {0} level {1} ​​from your team?",
                        "I can sell you every Armageddon Scroll by <color=yellow> {0} </color> Crystals. Do you want to buy?"
                    }
                },
     //           {
     //               TextKey.shopBasico,new List<string>()
     //               {
     //                   "Seja muito bem vindo a  minha loja estranho.",
     //                   "No que posso te ajudar??"
     //               }
     //           },
     //           {
     //               TextKey.frasesDeShoping,new List<string>()
     //               {
     //                   "Tenho excelentes produtos pra você estranho. Gostaria de comprar algo?",
     //                   "O que você gostaria de vender? estranho...",
     //                   "Muito obrigado por sua compra, estranho...",
     //                   "Excelente negocio, estranho...",
     //                   "Espero fazer negocios com você novamente, estranho..."
     //               }
     //           },
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
                    "You can not use this item at this time.",
                    "He does not need to use this item at this time.",
                    "Criature {0} is fainted and can not use this item at this time.",//O {0} será substituito pelo nome do Criature
					"Only Criatures of type {0} can use this item",
                    "O criature {0} não pode usar o item {1} pois ele já sabe o golpe {2}",
                    "Criature {0} can not learn the hit {1}",
                    "{0} did not use item {1}",
                    "Tem certeza que deseja usar o item {0} ?",
                    " You can not use this item in this location",
                    "{0} does not need to use this item at this time",
                    "You can not use items through the menu while fighting."
                }},
                {TextKey.bau,new List<string>()
                {
                    "You have found chest. Do you want to open it?",
                    "The chest is empty",
                    "Inside the chest you get <color=cyan> {0} {1} </color>",
                    "With the victory you can achieve <color=cyan> {0} {1} </color>"
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
                    "{0} resisted Capture's attempt.",
                    "you only keep glove can carry",
                    "So: ",
                    "Level",
                    "was sent to the",
                    "You was able to capture a {0}"
                }},

                {TextKey.listaDeItens,new List<string>()
                {
					/*ID==0*/"Apple", "Burguer", "Grove Card",
                    "Gasoline", "tonic water", /* ID == 5 */ "Watering", "Star", "Quartz", "Fertilizant",
                    "Sap", /* ID == 10 */ "insecticide", "Aura", "cabbage with egg", "Fan", "battery",
					/* ID == 15 */ "Dry Ice", "Escape Scroll", "Secret", "Mysterious Statue",
                    "Crystals", "Perfection Scroll", "Antidote", "Courage Charm", "tonic", /* ID = 24 */
					"Bust of Water Scroll", "Exit Scroll", "Citations Alpha", "Armageddon Scroll", "Sabre Scroll", "Insect Ooze Scroll",
                    "Acid Ooze Scroll", "Multiply Scroll", "Gale Scroll",
                    "Winds Cutting Scroll", "Look weakening Scroll", "Look Evil Scroll", "Citations Beta", "Leaves Hurricane Scroll",
                    "Explosives", "Medallion of the Waters",
                    "Holy Ink of Log", "Laurense Scroll", "Boutjoi Scroll", "Ananda Scroll", /* ID = 45 */
                    "Holy Pen of Log", "Sinara Scroll", "Alana Scroll", "Saber Scroll"
                }},


                {TextKey.mensLuta,new List<string>()
                {
                    "You can not use this item at this time.",
                    "you uses ",
                    "{0} does not need to use this item at this time",
                    "You can not try to capture a trainer's creatures",
                    "You can not use Escape Scroll against coaches",
                    "Lock on a creature to use this item"
                }},

                {TextKey.shopInfoItem,new List<string>()
                {
                    "Apple recovers 10 PV of a Creature",
                        "Burger recovers 40 PV a Criature",
                        "Glove Card is used to try to capture new Criatures",
                        "Gasoline recovers 40 PE a Criature Fire-type",
                        "Tonic Water recovers 40 PE a Criature Water-type ",
                        " Watering recovers 40 PE of a plant type Criature ",
                        " Star recovers 40 PE a Criature Normal-type ",
                        " Quartz recovers 40 PE a Criature Stone-type ",
                        " Fertilizer recovers 40 PE a Criature Earth-type ",
                        " Sap recovers 40 PE a Criature the Insect ",
                        " Insecticide recovers 40 PE a Criature the Poison ",
                        " Aura recovers 40 PE a Criature the Psychic type ",
                        "cabbage with egg recovers 40 PE of a gas type Criature",
                        "Fan recovers 40 PE a Criature the Flying type",
                        "battery recovers 40 PE a Criature the Eletrico type",
                        "Dry Ice recovers 40 PE a Criature Ice type ",
                        " When you read this scroll invokes a spell to expel the fighting opponent ",
                        " A very suspicious item leaning on Store fund ",
                        " A statue made of yellow stone in imposing pose ",
                        " Crystals is the monetary unit of orion",
                        " When you read this scroll the target criature fully recovers PVs, PEs and removes negative status ",
                        " The Criatures healing Antidoto who are poisoned ",
                        " The Amulet returns the courage to Criatures frightened ",
                        " The tonic cure Criatures Weke ",
                        " Agua Gust of scroll help a Criature type Agua to learn Gust coup de Agua ",
                        " can be used in closed places to teleport you out ",
                        " The award that you received from Captain Atos Aramis. ",
                        " Armageddon scroll teleports you to the last Armageddon you entered. You need to be in the open ",
                        " Sabre scroll helps a Criature to learn Sabre blow ",
                        " The scroll of Goop Insect helps a Criature Insect learn the Goop blow Bug ",
                        " The scroll of Goop Acida help one Criature Insect learn the Goop blow Acid ",
                        " The scroll of Multiply insects helps a Criature Insect learn the Multiply blow ",
                        " The scroll of the wind helps a Criature Flying learn the wind blow ",
                        " The scroll of the Winds Cutters helps a Criature Flying to learn the coup Winds Cutting ",
                        " The scroll of Look weakening helps a Criature to learn Look weakening blow ",
                        " The scroll of Evil Look helps a Criature to learn Evil Look blow ",
                        " The award that you received from Captain Fishbone. ",
                        " The leaves of Hurricane scroll can teach Sheets Hurricane blow to a plant type Criature ",
                        " The explosives needed to clear the way for Afarenga ",
                        " The locket Waters God Drag achieved with Omar Water ",
                        "A container containing the ink used by the priests of Log to write scrolls",
                        "The parchment in the name of Laurense the god of force, when read, increases 1 point of attack of a criature",
                        "The parchment in the name of Boutijoi the god of nature, when read, increases 1 point of defense of a criature",
                        "The parchment in the name of Ananda the goddess of magic, when read, increases 1 point of power to a creature",
                        "The pen used by Log's priests to write magical scrolls",
                        "The parchment in the name of Sinara, goddess mother of the criatures, when read, increases by 4 points the maximum life of a creature",
                        "The parchment in the name of Alana, goddess of fertility, when read, increases by 4 points the maximum energy of a creature",
                        "Saber's parchment can teach the Saber ability to a creature with the ability to learn it"

                }},
                {TextKey.textoBaseDeAcao,new List<string>()
                {
                    "Talk",
                    "Examine"
                }
                },
     //   {
     //       TextKey.usarPergaminhoDeLaurense,new List<string>()
     //       {
     //           "O pergaminho de Laurense pode aumentar 1 ponto de Ataque de um criature. Em quem gostaria de usar?",
     //           "O criature {0} nivel {1} aumentou 1 ponto em seu ataque"
     //       }
     //   },
     //   {
     //       TextKey.usarPergaminhoDeBoutjoi,new List<string>()
     //       {
     //           "O pergaminho de Boutjoi pode aumentar 1 ponto de Defesa de um criature. Em quem gostaria de usar?",
     //           "O criature {0} nivel {1} aumentou 1 ponto em sua defesa"
     //       }
     //   },
     //   {
     //       TextKey.usarPergaminhoDeAnanda,new List<string>()
     //       {
     //           "O pergaminho de Ananda pode aumentar 1 ponto de Poder de um criature. Em quem gostaria de usar?",
     //           "O criature {0} nivel {1} aumentou 1 ponto em seu poder"
     //       }
     //   },
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
     //           "O pergaminho de Sinara, deusa dos criatures, quando lido aumenta em 4 pontos o limite de vida d e um criature",
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
     //   {
     //       TextKey.frasesDaLutaContraTreinador,new List<string>()
     //       {
     //           "Nesta luta eu utilizarei {0} criatures",
     //           "Para inicar eu escolho...",
     //           "Parece que você teve uma vitória parcial, mas isso ainda não terminou",
     //           "Meu proximo criature será..."
     //       }
     //   },
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
                "Burst Water","Water Turbo","Fire Ball","Burst of Fire","Leaf Blade",
                "Leaves Hurricane","Horn","Slap","Claw","Whip Hand","Bite","Beak","Gale","Winds Cutting","Insect Ooze","Acid Ooze","Tail Whip","Halter","Electricity",
                "Concentrated Electricity","Poison needle","Poison Wave","Kick","Sword","Jumping Up","Gravel","Boulder","Burst of Earth","Energy of Claws","Earth Revenge","Psychosis","Hydro Bomb","Psychic Ball","Toast Attack","Leaf Storm",
                "Poison Rain","Multiply","Electrical Storm","Avalanche","Ring of Look",
                "Look Evil","Earth Curtain","Teleportation","Overflight","Look Weakening",
                "Gas Bomb","Burst of Gas","Smokescreen","Rod","Wing Sabre","Rod Sabre","Fin Sabre","Sword Sabre","Scissors","Spin Attack"
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
                "<color=yellow> Attention </color> \r \n Your Criature <color=orange> {0} </color> fainted by poisoning",
                "Your Criature <color=orange> {0} </color> has suffered {1} PV of poison damage",
                "The creature Enemy suffered {0} HP from poison damage"
            }
        },
        {
            TextKey.nomeStatus,new List<string>()
            {
                "poisoned",
                "weak",
                "frightened"
            }
        },
     //   {
     //       TextKey.menuDePause,new List<string>()
     //       {
     //           "Status", "Itens", "Coleção", "Voltar ao título", "Voltar ao jogo"
     //       }
     //   },
     //   {
     //       TextKey.menuDeArmagedom,new List<string>()
     //       {
     //           "Curar criatures",
     //           "Meus criatures armagedados",
     //           "Comprar pergaminho de armagedom",
     //           "Voltar ao Jogo"
     //       }
     //   }

    };
    }
}