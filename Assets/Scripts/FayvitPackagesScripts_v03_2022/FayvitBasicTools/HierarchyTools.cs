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
    }
}
