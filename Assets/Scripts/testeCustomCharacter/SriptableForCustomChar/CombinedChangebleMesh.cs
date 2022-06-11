using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combined Changeble Mesh")]
public class CombinedChangebleMesh : ChangebleElement,ITextureContainerElement
{
    public GameObject mesh;
    public SectionDataBase combinedWithDb;
    public int setID;
    public int toCombineId;
    public int[] combinedWithId;
    [ArrayElementTitle("id"), SerializeField] public TextureContainer[] textureSign;

    public TextureContainer GetTexContainerByID(SectionDataBase sdb)
    {
        return TextureContainerElementTools.SelectTexByID(textureSign, sdb);
    }
}

public static class TextureContainerElementTools
{
    public static TextureContainer SelectTexByID(TextureContainer[] tc, SectionDataBase sdb)
    {
        for (int i = 0; i < tc.Length; i++)
        {
            if (tc[i].id == sdb)
                return tc[i];
        }

        Debug.Log("TextureContainerElementTools não encontrou o indece: " + sdb);
        return null;
    }
}

public interface ITextureContainerElement
{
    TextureContainer GetTexContainerByID(SectionDataBase sdb);
}
