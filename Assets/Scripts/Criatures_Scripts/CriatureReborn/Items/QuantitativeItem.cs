using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class QuantitativeItem
    {
        public static bool NeedUsePerfection(PetBase meuCriature)
        {
            PetAtributes A = meuCriature.PetFeat.meusAtributos;
            return CanUseRecoveryItem(A) || CanUseEnergyItem(A) || meuCriature.StatusTemporarios.Count > 0;
        }

        public static bool CanUseRecoveryItem(PetAtributes A)
        {
            if (A.PV.Corrente < A.PV.Maximo && A.PV.Corrente > 0)
                return true;
            else
                return false;
        }

        public static bool CanUseEnergyItem(PetAtributes A)
        {

            if (A.PE.Corrente < A.PE.Maximo && A.PE.Corrente >= 0 && A.PV.Corrente > 0)
                return true;
            else
                return false;
        }

        public static void RecuperaPV(PetAtributes meusAtributos, int tanto)
        {
            int contador = meusAtributos.PV.Corrente;
            int maximo = meusAtributos.PV.Maximo;

            if (contador + tanto < maximo)
                meusAtributos.PV.Corrente += tanto;
            else
                meusAtributos.PV.Corrente = meusAtributos.PV.Maximo;
        }

        public static void RecuperaPE(PetAtributes meusAtributos, int tanto)
        {
            int contador = meusAtributos.PE.Corrente;
            int maximo = meusAtributos.PE.Maximo;

            if (contador + tanto < maximo)
                meusAtributos.PE.Corrente += tanto;
            else
                meusAtributos.PE.Corrente = meusAtributos.PE.Maximo;
        }
    }
}