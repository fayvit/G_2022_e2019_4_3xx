using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Criatures2021
{
    public class SorteadorDeGolpes
    {
        //public static int Sorteia(PetName nome, petatta GG)
        //{
        //    //bool foi = false;
        //    float roletaDeGolpes = 0;
        //    for (int i = 0; i < GG.meusGolpes.Count; i++)
        //    {
        //        roletaDeGolpes += GG.ProcuraGolpeNaLista(nome, GG.meusGolpes[i].Nome).TaxaDeUso;
        //    }

        //    float roleta = Random.Range(0, roletaDeGolpes);
        //    float sum = 0;
        //    int retorno = -1;
        //    //while(!foi){
        //    for (int i = 0; i < GG.meusGolpes.Count; i++)
        //    {

        //        sum += GG.ProcuraGolpeNaLista(nome, GG.meusGolpes[i].Nome).TaxaDeUso;


        //        if (roleta <= sum && retorno == -1)
        //            retorno = i;
        //    }


        //    retorno = retorno == -1 ? 0 : retorno;
        //    if (GG.meusGolpes[retorno].UltimoUso >= Time.time - GG.meusGolpes[retorno].TempoDeReuso)
        //        for (int i = 0; i < GG.meusGolpes.Count; i++)
        //        {
        //            if (GG.meusGolpes[i].UltimoUso < Time.time - GG.meusGolpes[i].TempoDeReuso)
        //                retorno = i;
        //        }

        //    //}

        //    return retorno == -1 ? 0 : retorno;
        //}

        public static int Sorteia(PetName nome, PetAttackManager GG)
        {
            //bool foi = false;
            float roletaDeGolpes = 0;
            for (int i = 0; i < GG.meusGolpes.Count; i++)
            {
                roletaDeGolpes += GG.ProcuraGolpeNaLista(nome, GG.meusGolpes[i].Nome).TaxaDeUso;
            }

            float roleta = Random.Range(0, roletaDeGolpes);
            float sum = 0;
            int retorno = -1;
            //while(!foi){
            for (int i = 0; i < GG.meusGolpes.Count; i++)
            {

                sum += GG.ProcuraGolpeNaLista(nome, GG.meusGolpes[i].Nome).TaxaDeUso;


                if (roleta <= sum && retorno == -1)
                    retorno = i;
            }


            //retorno = retorno == -1 ? 0 : retorno;
            //if (GG.meusGolpes[retorno].UltimoUso >= Time.time - GG.meusGolpes[retorno].TempoDeReuso)
            //    for (int i = 0; i < GG.meusGolpes.Count; i++)
            //    {
            //        if (GG.meusGolpes[i].UltimoUso < Time.time - GG.meusGolpes[i].TempoDeReuso)
            //            retorno = i;
            //    }

            //}

            return retorno == -1 ? 0 : retorno;
        }
    }
}