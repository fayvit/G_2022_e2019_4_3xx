using UnityEngine;

public class InternalResourcesFolders
{
    public static GameObject GetPet(PetName NomeID)
    {
        return Resources.Load<GameObject>("Criatures/" + NomeID.ToString());
    }
}