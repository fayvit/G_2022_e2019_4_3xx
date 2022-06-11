using UnityEngine;
using UnityEngine.UI;
using FayvitUI;
using Criatures2021;

namespace Criatures2021Hud
{
    [System.Serializable]
    public class PetBookHud {
        [SerializeField] private GameObject container;
        [SerializeField] private Image imgPet;
        [SerializeField] private Text nameAndNumber;
        [SerializeField] private Text type;
        [SerializeField] private Text descript;
        [SerializeField] private Text encounter;
        [SerializeField] private Text numberSeen;
        [SerializeField] private Text numberDefeated;
        [SerializeField] private Text numberCapture;
        [SerializeField] private TextAndImageMenu menu;

        private LivroDosCriatures livro;

        public void StartHud(LivroDosCriatures livro)
        {
            this.livro = livro;
            container.SetActive(true);
            int index = menu.SelectedOption;
            PetBase P = PetFactory.GetPet(livro.GetRegistered()[index]);
            menu.StartHud((int x) => { }, livro.GetStringAndNumbersOfRegistred(), livro.GetSpritesOfRegistred(), selectIndex: index);
            PreencherPagina(P);
        }

        void PreencherPagina(PetBase P)
        {
            
            imgPet.sprite = ResourcesFolders.GetMiniPet(P.NomeID);

            nameAndNumber.text = "Nº: " + ((int)P.NomeID + 1).ToString("000") + " - " + P.GetNomeEmLinguas;

            type.text = "Criature do Tipo " + TypeNameInLanguages.Get(P.PetFeat.meusTipos[0]);
            descript.text = TextBankSpace.TextBank.GetPetDescription(P.NomeID);
            encounter.text = "<color=yellow>Encontrado:</color> Lugares que ainda serão editados e feitos, " +
                "por enquanto vamos ficar com o texto generico";
            LivroDosCriatures.Pagina page = livro.GetPage(P.NomeID);
            numberSeen.text = page.visto.ToString();
            numberDefeated.text = page.derrotado.ToString();
            numberCapture.text = page.capturado.ToString();
        }

        public bool Update(int change,bool confirm/*pretendo usar esse bool para mostrar o modelo do pet*/,bool retorno)
        {
            if (change != 0)
            {
                menu.ChangeOption(change);
                PreencherPagina(PetFactory.GetPet(livro.GetRegistered()[menu.SelectedOption]));
            }
            else if (retorno)
                return true;

            return false;
        }

        public void FinishHud()
        {
            menu.FinishHud();
        }


    }
}