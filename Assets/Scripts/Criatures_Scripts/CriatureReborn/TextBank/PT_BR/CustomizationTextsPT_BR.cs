using System.Collections.Generic;

namespace TextBankSpace
{
    public static class CustomizationTextsPT_BR
    {
        public static Dictionary<TextKey, List<string>> txt = new Dictionary<TextKey, List<string>>()
        {{ TextKey.customizationParts, new List<string>(){
            "Base",
    "cabelo",
    "queixo",
    "globoOcular",
    "pupila",
    "iris",
    "umidade",
    "sobrancelha",
    "barba",
    "torso",
    "mao",
    "cintura",
    "pernas",
    "botas",
    "particular",
    "nariz",
    "empty",//ID=16
    "Genero",
    "Cor",
    "*** Concluir ***",
    "labios",
    "Tipo",
    "Detalhe"
        } },
            { 
            TextKey.frasesDoCustomization,new List<string>()
            {
            "Iniciar o jogo com esse personagem?",
            "Deseja salvar esse personagem?",
            "Selecionar a cor e não usar registro",
            "Selecionar a cor e usar o registro",
            "Voltar para o menu anterior"
            }
            },
            { 
            TextKey.frasesDoCharDbMenu,new List<string>()
            {
            "Gostaria de deletar esse personagem do vetor?",
            "Escolha um nome, que será identificador  ID, para esse personagem",
            "Escolha um nome para a lista de Personagens",
            "Deseja criar uma lista de personagens salvos?",
            "Deseja copiar esse personagem para uma lista de personagens?",
            "Essa lista não tem personagens salvos. Deseja escolher outra lista ou voltar para lista padrão?",
            "Voltar para padrão",
            "Escolher outra Lista",
            "Gostaria de deletar a lista <color=yellow>{0}</color> de personagem?"
            }
            }
        };
    }
    
}
