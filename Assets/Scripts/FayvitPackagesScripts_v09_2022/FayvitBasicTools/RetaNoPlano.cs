using UnityEngine;

namespace FayvitBasicTools
{
    public class QuadrilateroNoPlano
    {
        private RetaNoPlano[] lados;
        public QuadrilateroNoPlano(RetaNoPlano sul,RetaNoPlano leste,RetaNoPlano norte,RetaNoPlano oeste)
        {
            lados = new RetaNoPlano[4]
                    {
                sul,
                leste,
                norte,
                oeste
                    };
        }

        public bool DentroDoQuarilatero(Vector2 ponto)
        {
            return DentroDoQuarilatero(ponto.x, ponto.y);
        }

        public bool DentroDoQuarilatero(float x,float y)
        {
            return 
            lados[0].PosRelativaDoPonto(x, y) == RetaNoPlano.PosRelativa.acima &&
                lados[1].PosRelativaDoPonto(x, y) == RetaNoPlano.PosRelativa.abaixo &&
                lados[2].PosRelativaDoPonto(x, y) == RetaNoPlano.PosRelativa.abaixo &&
                lados[3].PosRelativaDoPonto(x, y) == RetaNoPlano.PosRelativa.acima;
            
        }
    }
    public class RetaNoPlano
    {
        private float coeficienteAngular;
        private float coeficienteLinear;
        private bool retaVertical;

        public enum PosRelativa
        {
            acima,
            abaixo,
            naReta
        }

        public RetaNoPlano(Vector2 P1, Vector2 P2)
        {
            RetaNoPlano R = new RetaNoPlano(P1.x, P1.y, P2.x, P2.y);
            coeficienteAngular = R.coeficienteAngular;
            coeficienteLinear = R.coeficienteLinear;
            retaVertical = R.retaVertical;
        }

        public RetaNoPlano(float x1, float y1, float x2, float y2)
        {
            if (x2 - x1 != 0)
            {
                coeficienteAngular = (y2 - y1) / (x2 - x1);
                coeficienteLinear = y1 - x1 * coeficienteAngular;
            }
            else
            {
                coeficienteAngular = float.MaxValue;
                coeficienteLinear = x1;
                retaVertical = true;
            }
        }


        public PosRelativa PosRelativaDoPonto(Vector2 ponto)
        {
            return PosRelativaDoPonto(ponto.x, ponto.y);
        }

        public PosRelativa PosRelativaDoPonto(float x, float y)
        {
            if (retaVertical)
            {
                return x > coeficienteLinear ? PosRelativa.acima : (x < coeficienteLinear ? PosRelativa.abaixo : PosRelativa.naReta);
            }
            else
            {
                float f = coeficienteAngular * x + coeficienteLinear;
                return f > y ? PosRelativa.acima : (f < y ? PosRelativa.abaixo : PosRelativa.naReta);
            }
        }
    }
}
