using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SectionDataBaseContainer : MonoBehaviour
{
    [ArrayElementTitle("id"), SerializeField] private TextureDataBase[] tBases;
    [ArrayElementTitle("id"), SerializeField] private MeshDataBase[] mBases;
    [ArrayElementTitle("id"), SerializeField] private CombinedMeshDataBase[] cBases;

    public MaskedTexture[] GetMaskedTexDbWithId(SectionDataBase id)
    {
        int check = -1;
        for (int i = 0; i < tBases.Length; i++)
        {
            if (tBases[i].id == id)
                check = i;
        }

        if (check == -1)
            return null;
        else
            return tBases[check].dataBase;
    }

    public ChangebleElement[] GetChangebleElementWithId(SectionDataBase id)
    {
        ChangebleElement[] retorno = null;
        foreach (var m in tBases)
            if (m.id == id)
                retorno = m.dataBase;

        foreach (var n in mBases)
            if (n.id == id)
                retorno = n.dataBase;
        

        foreach (var p in cBases)
            if (p.id == id)
                retorno = p.dataBase;

        return retorno;


    }

    public SimpleChangebleMesh[] GetDbMeshWithId(SectionDataBase id)
    {
        int check = -1;
        for (int i = 0; i < mBases.Length; i++)
        {
            if (mBases[i].id == id)
                check = i;
        }

        if (check == -1)
            return null;
        else
            return mBases[check].dataBase;
    }

    public CombinedChangebleMesh[] GetCombinedMeshDbWithID(SectionDataBase id)
    {
        int check = -1;
        for (int i = 0; i < cBases.Length; i++)
        {
            if (cBases[i].id == id)
                check = i;
        }

        if (check == -1)
            return null;
        else
            return cBases[check].dataBase;
    }

    [System.Serializable]
    private class TextureDataBase
    {
        public SectionDataBase id;
        public MaskedTexture[] dataBase;

        
    }

    [System.Serializable]
    private class MeshDataBase
    {
        public SectionDataBase id;
        public  SimpleChangebleMesh[] dataBase;
    }

    [System.Serializable]
    private class CombinedMeshDataBase
    {
        public SectionDataBase id;
        public CombinedChangebleMesh[] dataBase;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (TextureDataBase t in tBases)
        {
            IEnumerable<MaskedTexture> query = from element in t.dataBase.Distinct()
                                               select element;

            t.dataBase = query.ToArray();
        }

        foreach (MeshDataBase t in mBases)
        {
            IEnumerable<SimpleChangebleMesh> query = from element in t.dataBase.Distinct()
                                                     select element;

            t.dataBase = query.ToArray();
        }

        foreach (CombinedMeshDataBase t in cBases)
        {
            IEnumerable<CombinedChangebleMesh> query = from element in t.dataBase.Distinct()
                                                       select element;

            t.dataBase = query.ToArray();
        }
    }

        // Update is called once per frame
    void Update()
    {
        
    }
}

