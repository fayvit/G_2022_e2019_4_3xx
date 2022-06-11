using UnityEngine;
using FayvitLoadScene;
using FayvitBasicTools;

namespace FayvitSave
{
    public interface DadosDeJogador { }

    public abstract class SaveDates
    {
        public SaveDates(NomesCenas[] cenasAtivas) { }
        public SaveDates() { }

        protected abstract void SetarSaveDates();

        public DadosDeJogador Dados { get; private set; }

        public KeyVar VariaveisChave { get; private set; }

        public abstract Vector3 Posicao { get; }

        public abstract Quaternion Rotacao { get; }

        //private float[] posicao;
        //private float[] rotacao;

        //public SaveDates(NomesCenas[] cenasAtivas)
        //{
        //    if (AbstractGameController.Instance!=null)

        //        {
        //            SetarSaveDates();
        //            VariaveisChave.SetarCenasAtivas(cenasAtivas);
        //        }

        //}

        //public SaveDates()
        //{
        //    if (AbstractGameController.Instance != null)
        //    {
        //        SetarSaveDates();
        //        VariaveisChave.SetarCenasAtivas();
        //    }


        //}

        //private void SetarSaveDates()
        //{
        //    VariaveisChave = AbstractGameController.Instance.MyKeys;
        //    //Dados = manager.Dados;

        //    //Debug.Log(Dados + " : " + Dados.DinheiroCaido);

        //    Vector3 X = manager.transform.position;
        //    Vector3 R = manager.transform.forward;


        //    posicao = new float[3] { X.x, X.y, X.z };
        //    rotacao = new float[3] { R.x, R.y, R.z };

        //    //Debug.Log(X +" : "+ posicao[0]+" : "+posicao[1]+" : "+posicao[2]);
        //}

        ////public DadosDoJogador Dados { get; private set; }

        //public KeyVar VariaveisChave { get; private set; }

        //public Vector3 Posicao
        //{
        //    get { return new Vector3(posicao[0], posicao[1], posicao[2]); }
        //}

        //public Quaternion Rotacao
        //{
        //    get { return Quaternion.LookRotation(new Vector3(rotacao[0], rotacao[1], rotacao[2])); }
        //}
    }
}