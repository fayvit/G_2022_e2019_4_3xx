using UnityEngine;
using System.Collections.Generic;

namespace Criatures2021
{
    [System.Serializable]
    public class PetAttackManager
    {
        public int golpeEscolhido = 0;
        [SerializeField,ArrayElementTitle("_nome")] public List<PetAttackDb> listaDeGolpes;
        [SerializeField,ArrayElementTitle("container.nome")] public List<PetAttackBase> meusGolpes;


        List<PetAttackDb> ListaDeGolpesAtualizada(PetName nome)
        {
            //Debug.LogError("Elemetno por fazer");
            return PetFactory.GetPet(nome).GerenteDeGolpes.listaDeGolpes;

                //personagemG2.RetornaUmCriature(nome).GerenteDeGolpes.listaDeGolpes;
        }

        public PetAttackDb ProcuraGolpeNaLista(PetName nome, AttackNameId esseGolpe)
        {
            PetAttackDb retorno = new PetAttackDb();
            listaDeGolpes = ListaDeGolpesAtualizada(nome);

            for (int i = 0; i < listaDeGolpes.Count; i++)
                if (listaDeGolpes[i].Nome == esseGolpe)
                    retorno = listaDeGolpes[i];

            return retorno;
        }

        public PetAttackDb VerificaGolpeDoNivel(PetName nome, int nivel)
        {
            PetAttackDb retorno = new PetAttackDb();
            listaDeGolpes = ListaDeGolpesAtualizada(nome);

            for (int i = 0; i < listaDeGolpes.Count; i++)
                if (listaDeGolpes[i].NivelDoGolpe == nivel)
                    retorno = listaDeGolpes[i];

            return retorno;
        }

        public bool TemEsseGolpe(AttackNameId nome)
        {
            for (int i = 0; i < meusGolpes.Count; i++)
            {
                if (meusGolpes[i].Nome == nome)
                    return true;
            }

            return false;
        }
    }

}