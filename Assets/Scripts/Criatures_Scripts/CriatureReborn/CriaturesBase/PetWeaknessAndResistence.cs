using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    [System.Serializable]
    public class PetWeaknessAndResistence
    {

        [SerializeField] private float _mod;
        [SerializeField] private string _nome;

        public static PetWeaknessAndResistence[] ApplyPetWeaknessAndResistence(PetTypeName nomeDoTipo)
        {
            PetWeaknessAndResistence[] retorno = new PetWeaknessAndResistence[System.Enum.GetValues(typeof(PetTypeName)).Length];

            switch (nomeDoTipo)
            {
                case PetTypeName.Agua:
                    retorno = new PetWeaknessAndResistence[]
                        {
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Agua.ToString(),    Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Fogo.ToString(),    Mod = 0.25f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Planta.ToString(),  Mod = 1.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gelo.ToString(),    Mod = 2},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Terra.ToString(),   Mod = 0.5f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Pedra.ToString(),   Mod = 0.5f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Psiquico.ToString(),Mod = 1.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Eletrico.ToString(),Mod = 2f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Normal.ToString(),  Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Veneno.ToString(),  Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Inseto.ToString(),  Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Voador.ToString(),  Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gas.ToString(),     Mod = 1}
                        };

                    break;
                case PetTypeName.Planta:
                    retorno = new PetWeaknessAndResistence[]
                        {
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Agua.ToString(),    Mod = 0.25f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Fogo.ToString(),    Mod = 2f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Planta.ToString(),  Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gelo.ToString(),    Mod = 1.25f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Terra.ToString(),   Mod = 0.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Pedra.ToString(),   Mod = 0.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Psiquico.ToString(),Mod = 1.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Eletrico.ToString(),Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Normal.ToString(),  Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Veneno.ToString(),  Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Inseto.ToString(),  Mod = 1.25f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Voador.ToString(),  Mod = 1.25f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gas.ToString(),     Mod = 1}
                        };

                    break;
                case PetTypeName.Fogo:
                    retorno = new PetWeaknessAndResistence[]
                        {
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Agua.ToString(),    Mod = 1.5f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Fogo.ToString(),    Mod = 0.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Planta.ToString(),  Mod = 0.5f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gelo.ToString(),    Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Terra.ToString(),   Mod = 1.5f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Pedra.ToString(),   Mod = 1.25f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Psiquico.ToString(),Mod = 1.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Eletrico.ToString(),Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Normal.ToString(),  Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Veneno.ToString(),  Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Inseto.ToString(),  Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Voador.ToString(),  Mod = 1.25f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gas.ToString(),     Mod = 2}
                        };

                    break;
                case PetTypeName.Voador:
                    retorno = new PetWeaknessAndResistence[]
                        {
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Agua.ToString(),    Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Fogo.ToString(),    Mod = 0.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Planta.ToString(),  Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gelo.ToString(),    Mod = 2f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Terra.ToString(),   Mod = 0.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Pedra.ToString(),   Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Psiquico.ToString(),Mod = 0.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Eletrico.ToString(),Mod = 1.5f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Normal.ToString(),  Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Veneno.ToString(),  Mod = 1.5f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Inseto.ToString(),  Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Voador.ToString(),  Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gas.ToString(),     Mod = 0.25f}
                        };
                    break;
                case PetTypeName.Inseto:
                    retorno = new PetWeaknessAndResistence[]
                        {
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Agua.ToString(),    Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Fogo.ToString(),    Mod = 1.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Planta.ToString(),  Mod = 0.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gelo.ToString(),    Mod = 1.25f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Terra.ToString(),   Mod = 0.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Pedra.ToString(),   Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Psiquico.ToString(),Mod = 0.25f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Eletrico.ToString(),Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Normal.ToString(),  Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Veneno.ToString(),  Mod = 2f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Inseto.ToString(),  Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Voador.ToString(),  Mod = 1.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gas.ToString(),     Mod = 1.5f}
                        };
                    break;
                case PetTypeName.Psiquico:
                    retorno = new PetWeaknessAndResistence[]
                        {
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Agua.ToString(),    Mod = 0.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Fogo.ToString(),    Mod = 0.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Planta.ToString(),  Mod = 0.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gelo.ToString(),    Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Terra.ToString(),   Mod = 0.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Pedra.ToString(),   Mod = 0.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Psiquico.ToString(),Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Eletrico.ToString(),Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Normal.ToString(),  Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Veneno.ToString(),  Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Inseto.ToString(),  Mod = 1.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Voador.ToString(),  Mod = 1.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gas.ToString(),     Mod = 1.5f}
                        };
                    break;
                case PetTypeName.Normal:
                    retorno = new PetWeaknessAndResistence[]
                        {
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Agua.ToString(),    Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Fogo.ToString(),    Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Planta.ToString(),  Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gelo.ToString(),    Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Terra.ToString(),   Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Pedra.ToString(),   Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Psiquico.ToString(),Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Eletrico.ToString(),Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Normal.ToString(),  Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Veneno.ToString(),  Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Inseto.ToString(),  Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Voador.ToString(),  Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gas.ToString(),     Mod = 1},
                        };
                    break;
                case PetTypeName.Veneno:
                    retorno = new PetWeaknessAndResistence[]
                        {
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Agua.ToString(),    Mod = 1.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Fogo.ToString(),    Mod = 0.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Planta.ToString(),  Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gelo.ToString(),    Mod = 1.25f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Terra.ToString(),   Mod = 1.25f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Pedra.ToString(),   Mod = 1.25f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Psiquico.ToString(),Mod = 0.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Eletrico.ToString(),Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Normal.ToString(),  Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Veneno.ToString(),  Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Inseto.ToString(),  Mod = 0.5f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Voador.ToString(),  Mod = 1},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gas.ToString(),     Mod = 0.5f},
                        };
                    break;
                case PetTypeName.Pedra:
                    retorno = new PetWeaknessAndResistence[]
                        {
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Agua.ToString(),    Mod = 2f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Fogo.ToString(),    Mod = 0.25f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Planta.ToString(),  Mod = 1.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gelo.ToString(),    Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Terra.ToString(),   Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Pedra.ToString(),   Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Psiquico.ToString(),Mod = 1.5f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Eletrico.ToString(),Mod = 0.1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Normal.ToString(),  Mod = 0.5f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Veneno.ToString(),  Mod = 0.5f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Inseto.ToString(),  Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Voador.ToString(),  Mod = 0.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gas.ToString(),     Mod = 0.5f},
                        };
                    break;
                case PetTypeName.Eletrico:
                    retorno = new PetWeaknessAndResistence[]
                        {
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Agua.ToString(),    Mod = 0.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Fogo.ToString(),    Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Planta.ToString(),  Mod = 1.25f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gelo.ToString(),    Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Terra.ToString(),   Mod = 1.5f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Pedra.ToString(),   Mod = 1.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Psiquico.ToString(),Mod = 1.5f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Eletrico.ToString(),Mod = 0.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Normal.ToString(),  Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Veneno.ToString(),  Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Inseto.ToString(),  Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Voador.ToString(),  Mod = 0.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gas.ToString(),     Mod = 1f},
                        };
                    break;
                case PetTypeName.Terra:
                    retorno = new PetWeaknessAndResistence[]
                        {
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Agua.ToString(),    Mod = 2f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Fogo.ToString(),    Mod = 0.1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Planta.ToString(),  Mod = 1.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gelo.ToString(),    Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Terra.ToString(),   Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Pedra.ToString(),   Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Psiquico.ToString(),Mod = 1.5f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Eletrico.ToString(),Mod = 0.15f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Normal.ToString(),  Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Veneno.ToString(),  Mod = 0.95f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Inseto.ToString(),  Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Voador.ToString(),  Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gas.ToString(),     Mod = 0.75f},
                        };
                    break;
                case PetTypeName.Gas:
                    retorno = new PetWeaknessAndResistence[]
                        {
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Agua.ToString(),    Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Fogo.ToString(),    Mod = 2f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Planta.ToString(),  Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gelo.ToString(),    Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Terra.ToString(),   Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Pedra.ToString(),   Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Psiquico.ToString(),Mod = 0.5f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Eletrico.ToString(),Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Normal.ToString(),  Mod = 1f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Veneno.ToString(),  Mod = 0.75f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Inseto.ToString(),  Mod = 0.5f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Voador.ToString(),  Mod = 2f},
                        new PetWeaknessAndResistence (){ Nome = PetTypeName.Gas.ToString(),     Mod = 1f},
                        };
                    break;
            }
            return retorno;
        }

        public PetWeaknessAndResistence()
        {
            _mod = 1.0f;
            _nome = "";
        }

        public float Mod
        {
            get { return _mod; }
            set { _mod = value; }
        }

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public static string NomeEmLinguas(PetTypeName nome)
        {
            return nome.ToString();
        }
    }
}
