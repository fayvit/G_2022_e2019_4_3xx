using UnityEngine;
using System.Collections.Generic;

namespace Criatures2021
{
    [System.Serializable]
    public class StatusUpdater
    {
        [SerializeField] private List<StatusTemporarioBase> statusDoHeroi = new List<StatusTemporarioBase>();
        [SerializeField] private List<StatusTemporarioBase> statusDoInimigo = new List<StatusTemporarioBase>();

        public void ClearNegativeStatus(PetBase target)
        {
            List<int> removiveis = new List<int>();
            for (int i = 0; i < statusDoHeroi.Count; i++)
            {
                if (statusDoHeroi[i].OAfetado == target)
                    removiveis.Add(i);
            }

            for (int i = removiveis.Count-1; i > -1; i--)
                statusDoHeroi.RemoveAt(removiveis[i]);
        }

        public void VerificaRemoveStatus(StatusTemporarioBase S)
        {
            if (StatusDoHeroi.Contains(S))
                StatusDoHeroi.Remove(S);
            else if (StatusDoInimigo.Contains(S))
                StatusDoInimigo.Remove(S);
        }

        public void AdicionaStatusAoHeroi(StatusTemporarioBase S)
        {
            //GameController.g.HudM.StatusHud.DoHeroi.IniciarHudStatus(S.OAfetado);
            statusDoHeroi.Add(S);
            S.Start();

        }

        public void AdicionaStatusAoInimigo(StatusTemporarioBase S)
        {
            //GameController.g.HudM.StatusHud.DoInimigo.IniciarHudStatus(S.OAfetado);
            statusDoInimigo.Add(S);
            S.Start();
        }

        public List<StatusTemporarioBase> StatusDoHeroi
        {
            get { return statusDoHeroi; }
            set { statusDoHeroi = value; }
        }

        public List<StatusTemporarioBase> StatusDoInimigo
        {
            get { return statusDoInimigo; }
            set { statusDoInimigo = value; }
        }

        public void Update()
        {
            AtualizaListaDeStatus(StatusDoHeroi);
            AtualizaListaDeStatus(StatusDoInimigo);

        }

        void AtualizaListaDeStatus(List<StatusTemporarioBase> lista)
        {
            for (int i = 0; i < lista.Count; i++)
                lista[i].Update();
        }
    }
}
