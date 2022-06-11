using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Simple Changeble Mesh")]
public class SimpleChangebleMesh : ChangebleElement,ITextureContainerElement
{
    public GameObject mesh;
    [ArrayElementTitle("id"), SerializeField] public TextureContainer[] textureSign;
    public TextureContainer GetTexContainerByID(SectionDataBase sdb)
    {
        return TextureContainerElementTools.SelectTexByID(textureSign, sdb);
    }

}
