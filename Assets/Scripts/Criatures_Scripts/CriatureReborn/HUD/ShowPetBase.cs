using UnityEngine;
using UnityEngine.UI;
using Criatures2021;

namespace Criatures2021Hud
{
    public class ShowPetBase:MonoBehaviour
    {
        [SerializeField] private Image imgDoPersonagem;
        [SerializeField] private Text numPV;
        [SerializeField] private Text numPE;
        [SerializeField] private Text numAtk;
        [SerializeField] private Text numDef;
        [SerializeField] private Text numPod;
        [SerializeField] private Text numXp;
        [SerializeField] private Text txtMeusTipos;
        [SerializeField] private Text txtNomeC;
        [SerializeField] private Text numNivel;
        [SerializeField] private Text txtStatus;

        public void InserirDadosNoPainelPrincipal(PetBase C)
        {
            PetAtributes A = C.PetFeat.meusAtributos;
            IGerenciadorDeExperiencia g_XP = C.PetFeat.mNivel;

            imgDoPersonagem.sprite = Resources.Load<Sprite>("miniCriatures/" + C.NomeID);
            txtNomeC.text = C.GetNomeEmLinguas;
            numNivel.text = g_XP.Nivel.ToString();
            numPV.text = A.PV.Corrente + "\t/\t" + A.PV.Maximo;
            numPE.text = A.PE.Corrente + "\t/\t" + A.PE.Maximo;
            numXp.text = g_XP.XP + "\t/\t" + g_XP.ParaProxNivel;
            numAtk.text = A.Ataque.Corrente.ToString();
            numDef.text = A.Defesa.Corrente.ToString();
            numPod.text = A.Poder.Corrente.ToString();
            string paraTipos = "";

            for (int i = 0; i < C.PetFeat.meusTipos.Length; i++)
            {
                paraTipos += PetWeaknessAndResistence.NomeEmLinguas(C.PetFeat.meusTipos[i]) + ", ";
            }

            if (C.StatusTemporarios.Count > 0)
            {
                string sTemp = "";
                foreach (var v in C.StatusTemporarios)
                {
                    sTemp += v.GetNomeEmLinguas + ", ";
                }

                txtStatus.text = sTemp.Substring(0, sTemp.Length - 2);
                txtStatus.transform.parent.gameObject.SetActive(true);
            }
            else
                txtStatus.transform.parent.gameObject.SetActive(false);

            txtMeusTipos.text = paraTipos.Substring(0, paraTipos.Length - 2);

        }

    }
}
