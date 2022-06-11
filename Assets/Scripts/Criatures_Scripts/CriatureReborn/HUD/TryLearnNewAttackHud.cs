using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Criatures2021;
using FayvitSupportSingleton;
using System.Collections.Generic;
using FayvitMessageAgregator;
using FayvitUI;
using FayvitBasicTools;

namespace Criatures2021Hud
{
    [System.Serializable]
    public class TryLearnNewAttackHud
    {
        [SerializeField] private ShowNewAttackHud oAprendido;
        [SerializeField] private ShowNewAttackHud oEsquecivel;
        [SerializeField] private AttackOption[] options;
        [SerializeField] private GameObject container;
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color standardColor;

        private PetBase pet;

        public List<PetAttackBase> L_Attacks { get; private set; } = new List<PetAttackBase>();

        public int SelectedOption { get; set; }

        [System.Serializable]
        private class AttackOption
        {
            public Image selectImage;
            public Image img;
            public Text name;
        }

        public void StartHud(PetAttackBase[] conhecidos, PetAttackBase porConhecer,PetBase pet)
        {
            this.pet = pet;
            container.SetActive(true);

            L_Attacks.Clear();
            L_Attacks.AddRange(conhecidos);
            L_Attacks.Add(porConhecer);

            SelectedOption = 0;
            UnselectAll();
            options[0].selectImage.color = selectedColor;


            float mod = pet.GerenteDeGolpes.ProcuraGolpeNaLista(pet.NomeID, porConhecer.Nome).ModPersonagem;
            oAprendido.Start(porConhecer,mod);
            mod = pet.GerenteDeGolpes.ProcuraGolpeNaLista(pet.NomeID, conhecidos[0].Nome).ModPersonagem;
            oEsquecivel.Start(conhecidos[0],mod);

            for (int i = 0; i < 5; i++)
            {
                options[i].img.sprite = ResourcesFolders.GetMiniAttack(L_Attacks[i].Nome);
                options[i].name.text = PetAttackBase.NomeEmLinguas(L_Attacks[i].Nome);
            }
        }

        public void UnselectAll()
        {
            foreach (var o in options)
            {
                o.selectImage.color = standardColor;
            }
        }

        public void SelectInIndex(int index)
        {
            UnselectAll();
            options[index].selectImage.color = selectedColor;

        }

        public void ChangeOption(int i)
        {
            if (i != 0)
                MessageAgregator<MsgChangeOptionUI>.Publish();

            SelectedOption = ContadorCiclico.Contar(i, SelectedOption, 5);
            oEsquecivel.EndHud();
            SupportSingleton.Instance.InvokeInSeconds(() =>
            {
                float mod = pet.GerenteDeGolpes.ProcuraGolpeNaLista(pet.NomeID, L_Attacks[SelectedOption].Nome).ModPersonagem;
                oEsquecivel.Start(L_Attacks[SelectedOption],mod);
            }, .15f);

            SelectInIndex(SelectedOption);
        }

        public void EndHud()
        {
            container.SetActive(false);
        }

        
    }
}