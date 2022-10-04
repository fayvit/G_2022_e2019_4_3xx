using UnityEngine;
using System.Collections;

namespace Criatures2021
{
    public class PetFactory {
        public static PetBase GetPet(PetName nome)
        {
            PetBase retorno;
            switch (nome)
            {
                case PetName.Xuash:
                    retorno = Xuash.Criature;
                break;
                case PetName.Florest:
                    retorno = Florest.Criature;
                break;
                case PetName.PolyCharm:
                    retorno = PolyCharm.Criature;
                break;
                case PetName.Arpia:
                    retorno = Arpia.Criature;
                break;
                case PetName.Marak:
                    retorno = Marak.Criature;
                break;
                case PetName.Grewstick:
                    retorno = Grewstick.Criature;
                break;
                case PetName.Iruin:
                    retorno = Iruin.Criature;
                break;
                case PetName.Urkan:
                    retorno = Urkan.Criature;
                break;
                case PetName.Babaucu:
                    retorno = Babaucu.Criature;
                break;
                case PetName.Uiritibucu:
                    retorno = Uiritibucu.Criature;
                break;
                case PetName.Serpente:
                    retorno = Serpente.Criature;
                break;
                case PetName.Nessei:
                    retorno = Nessei.Criature;
                break;
                case PetName.Cracler:
                    retorno = Cracler.Criature;
                break;
                case PetName.Flam:
                    retorno = Flam.Criature;
                break;
                case PetName.Rocketler:
                    retorno = Rocketler.Criature;
                break;
                case PetName.Aladegg:
                    retorno = Aladegg.Criature;
                break;
                case PetName.Onarac:
                    retorno = Onarac.Criature;
                break;
                case PetName.On_Racani:
                    retorno = On_Racani.Criature;
                break;
                case PetName.Steal:
                    retorno = Steal.Criature;
                break;
                case PetName.Escorpion:
                    retorno = Escorpion.Criature;
                break;
                case PetName.Cabecu:
                    retorno = Cabecu.Criature;
                break;
                case PetName.DogMour:
                    retorno = DogMour.Criature;
                break;
                case PetName.Batler:
                    retorno = Batler.Criature;
                break;
                case PetName.Vampire:
                    retorno = Vampire.Criature;
                break;
                case PetName.Wisks:
                    retorno = Wisks.Criature;
                break;
                case PetName.Quaster:
                    retorno = Quaster.Criature;
                break;
                case PetName.Izicuolo:
                    retorno = Izicuolo.Criature;
                break;
                case PetName.Estrep:
                    retorno = Estrep.Criature;
                break;
                case PetName.Abutre:
                    retorno = Abutre.Criature;
                break;
                case PetName.Baratarab:
                    retorno = Baratarab.Criature;
                break;
                case PetName.Croc:
                    retorno = Croc.Criature;
                break;
                case PetName.Fajin:
                    retorno = Fajin.Criature;
                break;
                case PetName.FelixCat:
                    retorno = FelixCat.Criature;
                break;
                case PetName.Nedak:
                    retorno = Nedak.Criature;
                break;
                case PetName.Oderc:
                    retorno = Oderc.Criature;
                break;
                case PetName.Rabitler:
                    retorno = Rabitler.Criature;
                break;
                case PetName.Galfo:
                    retorno = Galfo.Criature;
                break;
                case PetName.Trude:
                    retorno = Trude.Criature;
                break;
                default:
                    retorno = new PetBase();
                break;
            }

            return retorno;
        }
    }
}