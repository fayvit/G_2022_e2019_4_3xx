using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Criatures2021
{
    public class LivroDosCriatures
    {

        private  Dictionary<PetName, Pagina> registros=new Dictionary<PetName, Pagina>();

        [System.Serializable]
        public class Pagina
        {
            public int visto;
            public int derrotado;
            public int capturado;
        }

        public PetName[] GetRegistered()
        {
            return registros.Keys.ToArray();
        }

        public string[] GetStringAndNumbersOfRegistred()
        {
            PetName[] pets = GetRegistered();
            List<string> s = new List<string>();
            foreach (var p in pets)
                s.Add("Nº:"+((int)p+1).ToString("000")+" - "+ PetBase.NomeEmLinguas(p));

            return s.ToArray();
        }

        public Sprite[] GetSpritesOfRegistred()
        {
            PetName[] pets = GetRegistered();
            List<Sprite> s = new List<Sprite>();
            foreach (var p in pets)
                s.Add(ResourcesFolders.GetMiniPet(p));

            return s.ToArray();
        }

        public Pagina GetPage(PetName name)
        {
            Pagina retorno = new Pagina();
            if (registros.ContainsKey(name))
                retorno = registros[name];
            else
                registros.Add(name, retorno);

            var ordered = registros.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            registros = ordered;

            return retorno;

        }

        public void AdicionaVisto(PetName name)
        {
            Pagina P = GetPage(name);
            P.visto++;
        }

        public void AdicionaDerrotado(PetName name)
        {
            Pagina P = GetPage(name);
            P.derrotado++;
        }

        public void AdicionaCapturado(PetName name)
        {
            Pagina P = GetPage(name);
            P.capturado++;
        }
    }
}