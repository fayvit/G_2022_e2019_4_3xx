using System;

namespace Criatures2021
{
    [Serializable]
    public class GerenciadorDeExperiencia : IGerenciadorDeExperiencia
    {
        [UnityEngine.SerializeField] private int _nivel = 1;
        [UnityEngine.SerializeField] private int _XP = 0;
        [UnityEngine.SerializeField] private int _paraProxNivel = 40;
        private float _modNivel = 1.25f;
        private int _ultimoPassaNivel = 0;

        public int CalculaPassaNivelInicial(int N, bool tudo = false)
        {
            int retorno = 0;
            int XparaProxNivel = 40;
            int XultimoPassaNivel = 0;

            for (int i = 0; i < N; i++)
            {
                XultimoPassaNivel = retorno;
                retorno = XparaProxNivel;
                XparaProxNivel += (int)Math.Floor(_modNivel * (XparaProxNivel - XultimoPassaNivel));
            }
            return tudo ? retorno : retorno - XultimoPassaNivel;
        }

        public bool VerificaPassaNivel()
        {
            return _XP >= _paraProxNivel;
        }

        public int CalculaPassaNivelAtual()
        {
            return (int)Math.Floor(_modNivel * (_paraProxNivel - _ultimoPassaNivel));
        }

        public void AplicaPassaNivel()
        {
            _nivel++;
            int U = CalculaPassaNivelAtual();
            _ultimoPassaNivel = _paraProxNivel;
            _paraProxNivel += U;
        }

        public int Nivel
        {
            get { return _nivel; }
            set { _nivel = value; }
        }

        public int XP
        {
            get { return _XP; }
            set { _XP = value; }
        }

        public int ParaProxNivel
        {
            get { return _paraProxNivel; }
            set { _paraProxNivel = value; }
        }


        public float ModNIvel
        {
            get
            {
                return _modNivel;
            }

            set
            {
                _modNivel = value;
            }
        }

        public int UltimoPassaNivel
        {
            get
            {
                return _ultimoPassaNivel;
            }
        }
    }
}