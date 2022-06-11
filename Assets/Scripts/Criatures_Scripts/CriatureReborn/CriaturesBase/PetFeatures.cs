using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class PetFeatures
    {
        public PetTypeName[] meusTipos;
        public PetWeaknessAndResistence[] contraTipos;
        public PetAtributes meusAtributos = new PetAtributes(new AttributesContainer());
        public GerenciadorDeExperiencia mNivel = new GerenciadorDeExperiencia();
        public float distanciaFundamentadora = 0.2f;

        public void IncrementaNivel(int nivel)
        {
            PetUpLevel.CalculeUpLevel(nivel, meusAtributos, true);

            mNivel.Nivel = nivel;
            mNivel.ParaProxNivel = mNivel.CalculaPassaNivelInicial(nivel);

        }

        public bool TemOTipo(PetTypeName tipo)
        {
            bool retorno = false;
            for (int i = 0; i < meusTipos.Length; i++)
            {
                if (meusTipos[i].ToString() == tipo.ToString())
                    retorno = true;
            }

            return retorno;
        }
    }

}