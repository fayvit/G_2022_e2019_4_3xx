using UnityEngine;
using FayvitBasicTools;
using Criatures2021Hud;

namespace Criatures2021
{
    public abstract class EventoComGolpe : ButtonActivate
    {
        [SerializeField] private string chave;
        //[SerializeField] private KeyShift chaveEspecial = KeyShift.nula;
        [SerializeField] private AttackNameId[] ativaveis;
        [Space(5)]
        [SerializeField] private bool todoDoTipo = false;
        [SerializeField] private PetTypeName tipoParaAtivar = PetTypeName.nulo;


        //protected KeyShift ChaveEspecial
        //{
        //    get { return chaveEspecial; }
        //}

        protected string Chave
        {
            get { return chave; }
            set { chave = value; }
        }

        private void OnValidate()
        {
            BuscadorDeID.Validate(ref chave, this, "chave");
//#if UNITY_EDITOR
//            if (string.IsNullOrEmpty(Chave) && gameObject.scene.name != null)
//            {
//                // ID = BuscadorDeID.GetUniqueID(gameObject, ID.ToString());

//                Chave = GetInstanceID() + "_" + gameObject.scene.name + "_Barreira";
//                //ID = System.Guid.NewGuid().ToString();
//                BuscadorDeID.SetUniqueIdProperty(this, Chave, "chave");
//            }
//#endif
        }

        private bool VerificaGolpeNaLista(AttackNameId nomeDoGolpe)
        {
            for (int i = 0; i < ativaveis.Length; i++)
                if (ativaveis[i] == nomeDoGolpe)
                    return true;

            return false;
        }

        protected bool EsseGolpeAtiva(AttackNameId nomeDoGolpe)
        {
            Debug.Log(nomeDoGolpe);
            bool ativa = false;
            if (todoDoTipo)
                if (AttackFactory.GetAttack(nomeDoGolpe).Tipo == tipoParaAtivar)
                    ativa = true;
            if (VerificaGolpeNaLista(nomeDoGolpe))
                ativa = true;

            return ativa;
        }

        public abstract void DisparaEvento(AttackNameId nomeDoGolpe);
    }
}