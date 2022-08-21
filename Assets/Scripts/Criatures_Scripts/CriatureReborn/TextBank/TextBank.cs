using UnityEngine;
using System.Collections.Generic;
using FayvitSave;

namespace TextBankSpace
{
    public class TextBank {
        public static LanguageKey linguaChave = LanguageKey.pt_br;
        public static readonly Dictionary<LanguageKey, Dictionary<TextKey, List<string>>> falacoesComChave
        = new Dictionary<LanguageKey, Dictionary<TextKey, List<string>>>() {
        { LanguageKey.pt_br,
            PT_BR_TextList.Txt
        },
        { LanguageKey.en_google,
            EN_G_TextList.Txt
        }
        };

        public static readonly Dictionary<LanguageKey, Dictionary<PetName, string>> petDescription
            = new Dictionary<LanguageKey, Dictionary<PetName, string>>(){
            {
                LanguageKey.pt_br,
                PetDescriptionPT_BR.txt
            } 
            };

        public static readonly Dictionary<LanguageKey, Dictionary<InterfaceTextKey, string>> textosDeInterface
            = new Dictionary<LanguageKey, Dictionary<InterfaceTextKey, string>>() {
            {
                LanguageKey.pt_br,
                InterfaceTextList.txt
            },
            {
                LanguageKey.en_google,
                InterfaceTextListEN_G.txt
            }
            };

        //public static readonly Dictionary<LanguageKey, Dictionary<IndiceDeArmagedoms, string>> nomesArmagedoms
        //    = new Dictionary<LanguageKey, Dictionary<IndiceDeArmagedoms, string>>() {
        //    {
        //        LanguageKey.pt_br,
        //        NomeDeArmagedomPT_BR.n
        //    },
        //    {
        //        LanguageKey.en_google,
        //        NomeDeArmagedomEN_G.n
        //    }
        //    };

        public static string GetPetDescription(PetName name)
        {
            if (petDescription[linguaChave].ContainsKey(name))
            {
                return petDescription[linguaChave][name];
            }
            else
                return petDescription[linguaChave][PetName.nulo];
        }

        public static void VerificaChavesFortes(LanguageKey primeiro, LanguageKey segundo)
        {
            if (falacoesComChave.ContainsKey(primeiro) && falacoesComChave.ContainsKey(segundo))
            {
                Dictionary<TextKey, List<string>>.KeyCollection keys = falacoesComChave[primeiro].Keys;

                foreach (TextKey k in keys)
                {
                    if (falacoesComChave[segundo].ContainsKey(k))
                    {
                        if (falacoesComChave[segundo][k].Count != falacoesComChave[primeiro][k].Count)
                        {
                            Debug.Log("As listas de mensagem no indice " + k + " tem tamanhos diferentes");
                        }
                    }
                    else
                    {
                        Debug.Log("A lista " + segundo + " nao contem a chave: " + k);
                    }
                }
            }
            else
            {
                Debug.Log("Falacoes nao contem alguma das chaves de LanguageKey");
            }

            if (petDescription.ContainsKey(primeiro) && petDescription.ContainsKey(segundo))
            {
                Dictionary<PetName, string>.KeyCollection keys = petDescription[primeiro].Keys;

                foreach (PetName k in keys)
                {
                    if (!petDescription[segundo].ContainsKey(k))
                    {
                        Debug.Log("petDescription da key: " + segundo + " não contem descrição para a chave " + k);
                    }
                }
            }
            else
            {
                Debug.Log("PetDescription nao contem alguma das chaves de LanguageKey");
            }

            //if (nomesArmagedoms.ContainsKey(primeiro) && nomesArmagedoms.ContainsKey(segundo))
            //{
            //    Dictionary<IndiceDeArmagedoms, string>.KeyCollection keys = nomesArmagedoms[primeiro].Keys;

            //    foreach (IndiceDeArmagedoms k in keys)
            //    {
            //        if (!nomesArmagedoms[segundo].ContainsKey(k))
            //        {
            //            Debug.Log("A lista " + segundo + " nao contem a chave de armagedom: " + k);
            //        }
            //    }
            //}
            //else
            //{
            //    Debug.Log("NomesArmagedoms nao contem alguma das chaves de LanguageKey");
            //}
        }

        public static List<string> RetornaListaDeTextoDoIdioma(TextKey chave)
        {
            return falacoesComChave[linguaChave][chave];
        }

        public static string RetornaFraseDoIdioma(TextKey chave)
        {
            return falacoesComChave[linguaChave][chave][0];
        }

        public static string RetornaTextoDeInterface(InterfaceTextKey chave)
        {
            return textosDeInterface[linguaChave][chave];
        }
    }

    public enum TextKey
    { 
        bomDia,
        apresentaInimigo,
        usoDeGolpe,
        nomesDosGolpes,
        listaDeItens,
        emQuem,
        itens,
        mensLuta,
        tentaCapturar,
        foiParaArmagedom,
        criatureParaMostrador,
        frasesDeArmagedom,
        apresentaDerrota,
        shopInfoItem,
        bau,
        textoBaseDeAcao,
        simOuNao,
        nomeTipos,
        aprendeuGolpe,
        passouDeNivel,
        comoChegamos,
        sagradoProfanado,
        potencialDesperdicado,
        oRetornoDoCataclisma,
        nomeStatus,
        status,
        apresentaFim,
        barreirasDeGolpes,
        customizationParts,
        frasesDoCustomization,
        frasesDoCharDbMenu,
        blocoBarreiraBloqueado,
        menuUsoDeItem,
        MenuDescartavel,
        mudouItemRapido,
        menuPets,
        dodgeInfo,
        targetLockInfo,
        alternanciaInfo,
        mudaGolpeInfo,
        atacarInfo,
        armagedomInicial,
        armagedomInicialFraseRapidaPadrao,
        armagedomNames,
        menuDeArmagedom,
        primeiroArmagedom,
        primeiroArmagedomMensCurta,
        RandomHooliganInicial,
        LanceLutzInicial,
        frasesDaLutaContraTreinador,
        infosDebarreiraasParaGolpes,
        caesarAntesDaLutaTuto,
        primeiraPerguntaMeEnfrentar,
        caesarDepoisDeVencido,
        caesarDepoisDeVencidoOsMedalhoes,
        vitoriaSobreCaesar,
        menuInicial,
        katids1,
        katids2,
        katids3,
        katids4,
        katids5,
        katids6,
        katids7,
        katids8,
        armagedomDeKatids,
        armagedomDeKatidsCurto,
        npcDoArmagedomDeKatids,
        frasesDeShoping,
        comprarOuVender,
        npcShopKatids,
        primeiraConversaShopKatids,
        conversaShopKatids,
        marjan1,
        marjan2,
        marjan3,
        marjan4,
        aprendeuGolpeEsquecendo,
        naoAprendeuGolpeNovo,
        usarPergaminhoDeLaurense,
        usarPergaminhoDeBoutjoi,
        usarPergaminhoDeAnanda,
        godPushPuzzleActivate,
        marjan5,
        marjan6,
        marjan7,
        marjan8,
        npcAarmgdMarjan,
        npcAarmgdMarjanCurto,
        npcArmagedomConversar,
        npcShopMarjanGenerico,
        npcShopVermelhoMarjan,
        npcShopVerdeMarjan,
        npcShopLaranjaMarjan,
        DevotaLauraAntesDaLuta,
        PerguntaDevotosCOmecarLuta,
        DevotaLaurAoGanharLuta,
        DevotaLauraDepoisDaLuta,
        DevotaElianaAntesDaLuta,
        DevotaElianaAoGanharLuta,
        DevotaElianaDepoisDaLuta,
        DevotoSenseAntesDaLuta,
        DevotoSenseAoGanharLuta,
        DevotoSenseDepoisDaLuta,
        DevotoRalfAntesDaLuta,
        DevotoRalfAoGanharLuta,
        DevotoRalfDepoisDaLuta,
        DevotaAnaniasAntesDaLuta,
        DevotoAnaniasAntesDaLuta,
        DevotoAnaniasAoGanharLuta,
        DevotoAnaniasDepoisDaLuta,
        DevotoJohnsonAntesDaLuta,
        falandoPrimeiroComIan,
        falandoPrimeiroComDerek,
        IanDepoisDeDerek,
        DerekDepoisDeIan,
        DerekDerrotado,
        IanComCaneta,
        ComecandoConversaComIanMercante,
        opcoesDeIan,
        conversaBasicaDeIan,
        conversaBasicaDeIan2,
        despedidabasicaDeIan,
        frasesDeVendaDeIan,
        DevotaAnaLeticiaAntesDaLuta,
        DevotaAnaLeticiaAoGanharLuta,
        PerguntaDevotosAnandaComecarLuta,
        DevotaAnaLeticiaDepoisDaLuta,
        DevotaMarianaAntesDaLuta,
        DevotaMarianaAoGanharLuta,
        DevotaMarianaDepoisDaLuta,
        DevotoLuanaAntesDaLuta,
        DevotoLuanaDepoisDaLuta,
        DevotoLuanaAoGanharLuta,
        DevotoMargaridaAntesDaLuta,
        DevotoMargaridaAoGanharLuta,
        DevotoMargaridaDepoisDaLuta,
        DevotoMatiasAntesDaLuta,
        DevotoMatiasAoGanharLuta,
        DevotoMatiasDepoisDaLuta,
        DevotoNunoAntesDaLuta,
        DevotoNunoDepoisDaLuta,
        DevotoNunoAoGanharLuta,
        PerguntaDevotosBoutjoiComecarLuta,
        DevotoNelsonAoGanharLuta,
        DevotoNelsonAntesDaLuta,
        DevotaNelsonDepoisDaLuta,
        DevotaUlicesDepoisDaLuta,
        DevotaUlicesAoGanharLuta,
        DevotoUlicesAntesDaLuta,
        DevotoEdilmaAntesDaLuta,
        DevotoEdilmaAoGanharLuta,
        DevotoEdilmaDepoisDaLuta,
        DevotaMarildaAntesDaLuta,
        DevotaMarildaAoGanharLuta,
        DevotaMarildaDepoisDaLuta,
        DevotaRosanaAoGanharLuta,
        DevotaRosanaAntesDaLuta,
        DevotaNeuzaAntesDaLuta,
        DevotaNeuzaAoGanharLuta,
        DevotaNeuzaDepoisDaLuta,
        DevotaRosanaDepoisDaLuta
    }
}