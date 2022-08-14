using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CriarBoxxle : MonoBehaviour
{
    [SerializeField] private GameObject paiDoPai;
    [SerializeField] private GameObject upperGlass;
    [SerializeField] private GameObject cornerTower;
    [SerializeField] private Texture2D tex;
    [SerializeField] private List<ColorPar> colorPares;
    [SerializeField] private bool getAllColor;
    [SerializeField] private bool getNewColor;
    [SerializeField] private bool criar;

    private GameObject pai;
    [System.Serializable]
    private struct ColorPar
    {
        public Color cor;
        public GameObject refGO;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (getAllColor)
        {
            GetColors(true);
            getAllColor = false;
        }

        if (getNewColor)
        {
            GetColors(false);
            getNewColor = false;
        }

        if (criar)
        {
            pai = Criatures2021.InstanceSupport.InstancieLigando(paiDoPai, paiDoPai.transform.position, default);
            pai.transform.parent = transform;
            Vector3 guardPos = pai.transform.position;
            pai.transform.position = Vector3.zero;
            for (int i = 0; i < tex.width; i++)
                for (int j = 0; j < tex.height; j++)
                {
                    ColoqueBLoco(tex.GetPixel(i, j), i, j);
                }

            pai.transform.position = guardPos;
            GameObject G = Instantiate(upperGlass, pai.transform.position, Quaternion.identity, pai.transform);
            G.transform.localPosition = new Vector3(tex.width / 2, 4f, tex.height / 2);
            G.transform.localScale = new Vector3(tex.width, 1, tex.height);
            criar = false;

            G = Instantiate(cornerTower, pai.transform.position, Quaternion.identity, pai.transform);
            G.transform.localPosition = new Vector3(-1, 0, -1);
            G = Instantiate(cornerTower, pai.transform.position, Quaternion.identity, pai.transform);
            G.transform.localPosition = new Vector3(tex.width, 0, -1);
            G = Instantiate(cornerTower, pai.transform.position, Quaternion.identity, pai.transform);
            G.transform.localPosition = new Vector3(-1, 0, tex.height);
            G = Instantiate(cornerTower, pai.transform.position, Quaternion.identity, pai.transform);
            G.transform.localPosition = new Vector3(tex.width, 0, tex.height);

        }
    
    }

    GameObject RetornaObjectDaCor(Color C)
    {
        GameObject G = null;
        for (int i = 0; i < colorPares.Count; i++)
        {
            if (colorPares[i].cor == C)
                G = colorPares[i].refGO;
        }

        return G;
    }

    void ColoqueBLoco(Color C, int x, int y)
    {
        GameObject G = RetornaObjectDaCor(C);

        if (G != null)
            Instantiate(G, new Vector3(x, 0, y), G.transform.rotation, pai.transform);
    }

    void GetColors(bool news)
    {
        if(news)
            colorPares = new List<ColorPar>();

        for (int i = 0; i < tex.width; i++)
            for (int j = 0; j < tex.height; j++)
            {
                bool coloca = true;
                for (int k = 0; k < colorPares.Count; k++)
                {

                    if (colorPares[k].cor == tex.GetPixel(i, j))
                        coloca = false;
                }

                if (coloca)
                    colorPares.Add(new ColorPar() { cor = tex.GetPixel(i, j) });
            }
    }
}
