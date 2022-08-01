using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Criatures2021
{
    public class WildPetInitialize 
    {
        public static PetManager Initialize(PetBase pet, Vector3 pos)
        {
            return Initialize(pet.NomeID, pet.PetFeat.mNivel.Nivel, pos);
        }

        public static PetManager Initialize(PetName petName,int nivel,Vector3 pos)
        {
            PetBase P = new PetBase(petName, nivel);
            GameObject G = InstantiatePet(P,pos);
            G.transform.position = MelhoraInstancia3D.ProcuraPosNoMapa(G.transform.position);
            return ConfigureCriatureBase(G, P);
            
        }

        public static GameObject InstantiatePet(PetBase criature,Vector3 position)
        {
            GameObject CA = Resources.Load<GameObject>("Criatures/" + criature.NomeID.ToString());
            CA = MonoBehaviour.Instantiate(CA, position, Quaternion.identity)
                as GameObject;

            Scene S = SceneManager.GetSceneByName(SpecialSceneName.ComunsDeFase.ToString());

            if(S.isLoaded)
            SceneManager.MoveGameObjectToScene(CA,S);
            return CA;
        }

        public static PetManager ConfigureCriatureBase(GameObject G, PetBase cBase)
        {
            string s = System.Guid.NewGuid().ToString();
            G.name = "Enemy: " + s.Substring(0,7) ;
            PetManager Cc = G.GetComponent<PetManager>();
            MonoBehaviour.Destroy(Cc);
            PetManagerEnemy C = G.AddComponent<PetManagerEnemy>();
            C.MeuCriatureBase = cBase;

            return C;

            //RecolocadorDeStatus.VerificaInsereParticulaDeStatus(C);
        }
    }
}