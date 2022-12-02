using UnityEngine;

namespace FayvitBasicTools
{
    public static class HierarchyTools
    {
        public static bool EstaNaHierarquia(Transform candidatoPai,Transform hierarchyVerify)
        {
            
            while (hierarchyVerify != null)
            {
                //Debug.Log(hierarchyVerify.name + " : " + candidatoPai.name + " : " + (hierarchyVerify == candidatoPai));
                if (hierarchyVerify == candidatoPai)
                    return true;

                hierarchyVerify = hierarchyVerify.parent;
            }

            return false;
        }

        public static Transform GetRootParent(Transform T)
        {
            Transform retorno = T;
            while (retorno.parent!=null)
            {
                retorno = retorno.parent;
            }

            return retorno;
        }
    }
}
