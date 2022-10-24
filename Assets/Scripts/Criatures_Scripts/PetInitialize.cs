using FayvitLoadScene;
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

        public static GameObject Instantiate(Vector3 position,PetName NomeID,float customVarDir=0.1f) {
            GameObject CA = ResourcesFolders.GetPet(NomeID);//Resources.Load<GameObject>("Criatures/"+criature.NomeID.ToString());
            CA = MonoBehaviour.Instantiate(CA,
                MelhoraInstancia3D.ProcuraPosNoMapa(position,customVarDir)
                , Quaternion.identity)
                as GameObject;
            ResourcesFolders.OtimizePet(NomeID,CA);
            return CA;
        }

        public static GameObject InstantiatePet(Transform dono, PetBase criature)
        {

            GameObject CA = Instantiate(dono.position - 3 * dono.forward, criature.NomeID, 5);
                
            //    ResourcesFolders.GetPet(criature.NomeID);//Resources.Load<GameObject>("Criatures/"+criature.NomeID.ToString());
            //CA = MonoBehaviour.Instantiate(CA, 
            //    MelhoraInstancia3D.ProcuraPosNoMapa(dono.position - 3 * dono.forward,5)
            //    , Quaternion.identity)
            //    as GameObject;

            if(SceneManager.GetSceneByName(NomesCenasEspeciais.ComunsDeFase.ToString()).isLoaded)
            SceneManager.MoveGameObjectToScene(CA,
            SceneManager.GetSceneByName(NomesCenasEspeciais.ComunsDeFase.ToString())
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