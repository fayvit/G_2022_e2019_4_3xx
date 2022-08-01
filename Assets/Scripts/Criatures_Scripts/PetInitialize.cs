using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Criatures2021
{
    public class PetInitialize
    {
        public static GameObject Initialize(Transform dono, PetBase pet)
        {
            GameObject G = InstantiatePet(dono, pet);
            ConfigureCriatureBase(G, pet, dono);
            return G;
        }

        public static GameObject InstantiatePet(Transform dono, PetBase criature)
        {

            GameObject CA = ResourcesFolders.GetPet(criature.NomeID);//Resources.Load<GameObject>("Criatures/"+criature.NomeID.ToString());
            CA = MonoBehaviour.Instantiate(CA, dono.position - 3 * dono.forward, Quaternion.identity)
                as GameObject;

            
            SceneManager.MoveGameObjectToScene(CA,
            SceneManager.GetSceneByName(SpecialSceneName.ComunsDeFase.ToString())
            );
            return CA;
        }

        public static void ConfigureCriatureBase(GameObject G, PetBase cBase,Transform dono)
        {
            G.name = "CriatureAtivo";
            PetManager Cc = G.GetComponent<PetManager>();
            MonoBehaviour.Destroy(Cc);
            PetManagerCharacter C = G.AddComponent<PetManagerCharacter>();
            C.T_Dono = dono.GetComponent<CharacterManager>();
            C.MeuCriatureBase = cBase;

            StatusReplacer.VerificaInsereParticulaDeStatus(C);
        }
    }
}