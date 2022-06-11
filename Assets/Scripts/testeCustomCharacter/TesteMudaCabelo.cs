using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FayvitBasicTools;
using FayvitSupportSingleton;
using FayvitMessageAgregator;


#if UNITY_EDITOR
using UnityEditor;
#endif

//#region PropertyAtribute
//public class ArrayElementTitleAttribute : PropertyAttribute
//{
//    public string Varname;
//    public ArrayElementTitleAttribute(string ElementTitleVar)
//    {
//        Varname = ElementTitleVar;
//    }
//}
//#endregion

public class TesteMudaCabelo : MonoBehaviour
{
    [Header("----------Novo----------")]
    [ArrayElementTitle("id")]
    [SerializeField] private List<ConjuntoDasPartes_b> partes_b;
    [ArrayElementTitle("id")]
    [SerializeField] private List<ConjuntoDasPartesComTamanhoCombinado_b> partesComTamanho_b;
    [Header("-----------------------")]

    [ArrayElementTitle("id")]
    [SerializeField] private List<ConjuntoDasPartes> partes;

    [ArrayElementTitle("id")]
    [SerializeField] private List<ConjuntoDasPartesComTamanhoCombinado> partesComTamanho;
    [SerializeField] private SkinnedMeshRenderer mesh;
    [SerializeField] private UnityEngine.UI.Text umLabel;
    [SerializeField] private FayvitUI.GetColorHudManager tGetColor;
    [SerializeField] private Color umaCor = Color.red;

    [SerializeField] private string textureTarget;
    [SerializeField] private string textureMaskTarget;

    private ContadorAtual cAtual = ContadorAtual.cabelo;

    /// <summary>
    /// Para elementos de textura essa variável se torna um contador. 
    /// Para elementos de Mesh essa variavel se torna um indice do banco de dados
    /// </summary>
    private Dictionary<ContadorAtual, int> guardCont = new Dictionary<ContadorAtual, int>() { 
        { ContadorAtual.cabelo,0},
        { ContadorAtual.nariz,1},
        { ContadorAtual.queixo,2},
        { ContadorAtual.globoOcular,0},
        { ContadorAtual.@base,3},
        { ContadorAtual.torso,0},
        { ContadorAtual.mao,1},
    };

    private SectionDataBaseContainer S;
    private Color skinColor;

    private int tamanho = 0;

    #region varSuprimidas
    /*
   [SerializeField] private GameObject[] possiveisCabelos;
   [SerializeField] private GameObject cabeloAtual;
   [SerializeField] private GameObject[] possiveisNarizes;
   [SerializeField] private GameObject narizAtual;
   [SerializeField] private GameObject[] possiveisQueixos;
   [SerializeField] private GameObject queixoAtual;
   */
    //private int contadorDeCabelo = 0;
    //private int contadorDeNariz = 0;
    //private int contadorDeQueixo = 0;
    #endregion

    #region subclasses

    [System.Serializable]
    private class ConjuntoDasPartes
    {
        public ContadorAtual id;
        public GameObject[] possiveis;
        public GameObject atual;
        [HideInInspector] public int contador = 0;
    }

    [System.Serializable]
    private class ConjuntoDasPartes_b
    {
        public ContadorAtual id;
        public GameObject atual;
        [HideInInspector] public int contador = 0;
    }

    [System.Serializable]
    private class ConjuntoDasPartesComTamanhoCombinado_b
    {

        public ContadorAtual id;
        public GameObject atual;
        public ContadorAtual combinadoCom;
        [HideInInspector] public int contador = 0;       

    }

    [System.Serializable]
    private class ConjuntoDasPartesComTamanhoCombinado
    {

        public ContadorAtual id;
        public ObjetosPorCombinacao[] possiveis;
        public GameObject atual;
        [HideInInspector] public int contador = 0;

        [System.Serializable]
        public class ObjetosPorCombinacao
        {
            public ObjetosPorTamanho[] tamanhosDeObjetos;

            [System.Serializable]
            public class ObjetosPorTamanho
            {
                public int CombinadoCom = 0;
                public GameObject objeto;
            }
        }
        
    }

    #endregion

    #region Editor



#if UNITY_EDITOR



    [CustomPropertyDrawer(typeof(ArrayElementTitleAttribute))]
    public class ArrayElementTitleDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property,
                                        GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        protected virtual ArrayElementTitleAttribute Atribute
        {
            get { return (ArrayElementTitleAttribute)attribute; }
        }
        SerializedProperty TitleNameProp;
        public override void OnGUI(Rect position,
                                  SerializedProperty property,
                                  GUIContent label)
        {
            string FullPathName = property.propertyPath + "." + Atribute.Varname;
            TitleNameProp = property.serializedObject.FindProperty(FullPathName);
            string newlabel = GetTitle();
            if (string.IsNullOrEmpty(newlabel))
                newlabel = label.text;
            EditorGUI.PropertyField(position, property, new GUIContent(newlabel, label.tooltip), true);
        }
        private string GetTitle()
        {
            switch (TitleNameProp.propertyType)
            {
                case SerializedPropertyType.Generic:
                    break;
                case SerializedPropertyType.Integer:
                    return TitleNameProp.intValue.ToString();
                case SerializedPropertyType.Boolean:
                    return TitleNameProp.boolValue.ToString();
                case SerializedPropertyType.Float:
                    return TitleNameProp.floatValue.ToString();
                case SerializedPropertyType.String:
                    return TitleNameProp.stringValue;
                case SerializedPropertyType.Color:
                    return TitleNameProp.colorValue.ToString();
                case SerializedPropertyType.ObjectReference:
                    return TitleNameProp.objectReferenceValue.ToString();
                case SerializedPropertyType.LayerMask:
                    break;
                case SerializedPropertyType.Enum:
                    return TitleNameProp.enumNames[TitleNameProp.enumValueIndex];
                case SerializedPropertyType.Vector2:
                    return TitleNameProp.vector2Value.ToString();
                case SerializedPropertyType.Vector3:
                    return TitleNameProp.vector3Value.ToString();
                case SerializedPropertyType.Vector4:
                    return TitleNameProp.vector4Value.ToString();
                case SerializedPropertyType.Rect:
                    break;
                case SerializedPropertyType.ArraySize:
                    break;
                case SerializedPropertyType.Character:
                    break;
                case SerializedPropertyType.AnimationCurve:
                    break;
                case SerializedPropertyType.Bounds:
                    break;
                case SerializedPropertyType.Gradient:
                    break;
                case SerializedPropertyType.Quaternion:
                    break;
                default:
                    break;
            }
            return "";
        }
    }

#endif
    #endregion

    private enum ContadorAtual
    { 
        cabelo,
        nariz,
        queixo,
        torso,
        mao,
        globoOcular,
        @base
    }

    private ConjuntoDasPartesComTamanhoCombinado_b ConjuntoCombinadoPorID(ContadorAtual c)
    {   
        ConjuntoDasPartesComTamanhoCombinado_b retorno = null;
        for (int i = 0; i < partesComTamanho_b.Count; i++)
        {
            if (partesComTamanho_b[i].id == c)
                retorno = partesComTamanho_b[i];
        }

        return retorno;
    }

    // Start is called before the first frame update
    void Start()
    {
        #region materialColor
        //Material material = mesh.material;
        //material.SetColor("_MyBaseColor", 0.65f*Color.blue);
        //material.SetColor("_CorDoCinto", 0.25f*Color.blue);
        //material.SetColor("_CorDaPele", Color.blue);
        #endregion
        S = GetComponent<SectionDataBaseContainer>();
        umLabel.text = SectionDataBase.globoOcular.ToString();
        guardCont[cAtual] = 0;
        //skinColor = mesh.material.GetColor("_MyBaseColor");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log(tGetColor.CurrentColor);
        }

        int change = 0;
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            change = 1;

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            change = -1;
        }

        VerifyChangePart();
        VerifyChangeColor();

        if (change != 0)
        {

            switch(cAtual)
            {
                case ContadorAtual.globoOcular: 
                    GloboOcular(change);
                break;
                case ContadorAtual.cabelo:
                case ContadorAtual.nariz:
                case ContadorAtual.queixo:
                    TrocaMesh(change);
                break;
                case ContadorAtual.torso:
                    ChangeCombinedMesh(change);
                break;
                
            };

        }
    }

    void VerifyChangeColor()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (C_AtualContainsMainColor())
            {
                GameObject G =
                tGetColor.transform.parent.gameObject;
                G.SetActive(!G.activeSelf);

                if (!G.activeSelf)
                    MessageAgregator<MsgChangeColorPicker>.RemoveListener(OnChangeColorPicker);
                    //UiEventAgregator.RemoveListener(UIEventKey.changeColorPicker, OnChangeColorPicker);

                SupportSingleton.Instance.InvokeOnEndFrame(() =>
                {
                    tGetColor.SetColor(umaCor);
                });
            }
        }
    }

    bool C_AtualContainsMainColor()
    {
        bool retorno = false;
        int cont = -1;
        Material material = null;
        SimpleChangebleMesh scm;
        GameObject gg;

        switch (cAtual)
        {
            case ContadorAtual.globoOcular:
                cont = guardCont[ContadorAtual.globoOcular];
                
                MaskedTexture[] m = S.GetMaskedTexDbWithId(SectionDataBase.globoOcular);

                retorno = m[cont].coresEditaveis.Length > 0;
                material = mesh.material;
                umaCor = material.GetColor(m[cont].coresEditaveis[0].materialColorTarget.ToString());
                MessageAgregator<MsgChangeColorPicker>.AddListener(OnChangeColorPicker);
                //UiEventAgregator.AddListener(UIEventKey.changeColorPicker,OnChangeColorPicker);
            break;
            #region Removido
            //case ContadorAtual.cabelo:
            //    #region originalRemovido
            //    //cont  = guardCont[ContadorAtual.cabelo];

            //    //sm = S.GetDbMeshWithId(SectionDataBase.cabelo);

            //    //retorno = sm[cont].coresEditaveis.Length > 0;

            //    //GameObject G = partes_b[cont].atual;
            //    //int indexOfSM = partes_b[cont].contador;
            //    #endregion

            //    GetSimpleChangebleElements(cAtual, out scm, out gg);

            //    retorno = scm.coresEditaveis.Length > 0;
            //    if (scm.coresEditaveis.Length > 0)
            //    {
            //        material = GetMaterialForChangeColor(gg, scm.coresEditaveis[0]);

            //        umaCor = material.GetColor(scm.coresEditaveis[0].materialColorTarget.ToString());

            //        UiEventAgregator.AddListener(UIEventKey.changeColorPicker, OnChangeColorPicker);
            //    }
            //break;
            #endregion
            case ContadorAtual.cabelo:
            case ContadorAtual.nariz:
            case ContadorAtual.queixo:

                GetSimpleChangebleElements(cAtual,out scm,out gg);

                retorno = scm.coresEditaveis.Length > 0;

                if (scm.coresEditaveis.Length > 0)
                {
                    
                    material = GetMaterialForChangeColor(gg,scm.coresEditaveis[0]);

                    if (scm.coresEditaveis[0].registro == RegistroDeCores.skin)
                    {
                        skinColor = material.GetColor(scm.coresEditaveis[0].ColorTargetName);
                        umaCor = skinColor;
                        ChangeSkinColor();
                    }
                    MessageAgregator<MsgChangeColorPicker>.AddListener(OnChangeColorPicker);
                    //UiEventAgregator.AddListener(UIEventKey.changeColorPicker, OnChangeColorPicker);
                }
            break;
            case ContadorAtual.torso:

            break;

        };

        return retorno;
    }

    Material GetMaterialForChangeColor(GameObject G,ColorContainer cc)
    {
        Material retorno = null;


        if (cc.meshOrChildren == MainOrChildren.parentObject)
            G = G.transform.parent.gameObject;

        if (cc.meshOrChildren == MainOrChildren.childrenObject)
        {
            retorno = G.GetComponentInChildren<Renderer>().materials[cc.indiceDoMaterialAlvo];
        } else if (cc.meshOrChildren == MainOrChildren.mainObject)
        {
            retorno = G.GetComponent<Renderer>().materials[cc.indiceDoMaterialAlvo];
        }

        return retorno;
    }

    void GetSimpleChangebleElements(ContadorAtual cAtual,out SimpleChangebleMesh sm,out GameObject gg)
    {
        int cont = guardCont[cAtual];
        SectionDataBase sdb = StringForEnum.GetEnum(cAtual.ToString(), SectionDataBase.nariz);
        SimpleChangebleMesh[] sms = S.GetDbMeshWithId(sdb);

        gg = partes_b[cont].atual;
        int indexOfsm = partes_b[cont].contador;

        sm = sms[indexOfsm];


    }

    void GetCombinedChangebleElements(ContadorAtual cAtual, out CombinedChangebleMesh sm, out GameObject gg)
    {
        int cont = guardCont[cAtual];
        SectionDataBase sdb = StringForEnum.GetEnum(cAtual.ToString(), SectionDataBase.nariz);
        CombinedChangebleMesh[] sms = S.GetCombinedMeshDbWithID(sdb);

        gg = partesComTamanho_b[cont].atual;
        int indexOfsm = partesComTamanho_b[cont].contador;

        sm = sms[indexOfsm];

    }

    void ChangeSkinColor()
    {
        for (int i = 0; i < partes_b.Count; i++)
        {
            SimpleChangebleMesh scm;
            GameObject G;
            GetSimpleChangebleElements(partes_b[i].id, out scm, out G);

            for (int j = 0; j < scm.coresEditaveis.Length; j++)
            {
                if (scm.coresEditaveis[j].registro == RegistroDeCores.skin)
                {
                    Material material = GetMaterialForChangeColor(G, scm.coresEditaveis[j]);
                    material.SetColor(scm.coresEditaveis[j].ColorTargetName, skinColor);
                }
            }
        }

        for (int i = 0; i < partesComTamanho_b.Count; i++)
        {
            CombinedChangebleMesh scm;
            GameObject G;
            GetCombinedChangebleElements(partesComTamanho_b[i].id, out scm, out G);

            for (int j = 0; j < scm.coresEditaveis.Length; j++)
            {
                if (scm.coresEditaveis[j].registro == RegistroDeCores.skin)
                {
                    Material material = GetMaterialForChangeColor(G, scm.coresEditaveis[j]);
                    material.SetColor(scm.coresEditaveis[j].ColorTargetName, skinColor);
                }
            }
        } 
    }

    void OnChangeColorPicker(MsgChangeColorPicker e)
    {
        Material material=null;
        GameObject G;
        SimpleChangebleMesh scm;
        Color c = e.color;
        switch (cAtual)
        {
            case ContadorAtual.globoOcular:

                int cont = guardCont[ContadorAtual.globoOcular];

                MaskedTexture[] m = S.GetMaskedTexDbWithId(SectionDataBase.globoOcular);

                material = mesh.material;
                material.SetColor(m[cont].coresEditaveis[0].materialColorTarget.ToString(), c);
                umaCor = c;
                //m[cont].coresEditaveis[0].color = c;
            break;
            case ContadorAtual.cabelo:
                

                GetSimpleChangebleElements(cAtual,out scm, out G);
                material = GetMaterialForChangeColor(G,scm.coresEditaveis[0]);

                material.SetColor(scm.coresEditaveis[0].materialColorTarget.ToString(), c);
                umaCor = c;
            break;
            case ContadorAtual.nariz:
            case ContadorAtual.queixo:
                GetSimpleChangebleElements(cAtual, out scm, out G);
                material = GetMaterialForChangeColor(G, scm.coresEditaveis[0]);

                material.SetColor(scm.coresEditaveis[0].ColorTargetName, c);
                skinColor = c;
                if (scm.coresEditaveis[0].registro == RegistroDeCores.skin)
                    ChangeSkinColor();
            break;
        }
    }

    void VerifyChangePart()
    {
        int change = 0;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            change = 1;

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            change = -1;
        }

        ContadorAtual ant = cAtual;
        cAtual = (ContadorAtual)ContadorCiclico.Contar(change, (int)cAtual, System.Enum.GetValues(typeof(ContadorAtual)).Length);

        if (cAtual != ant)
        {
            umLabel.text = cAtual.ToString();
        }
    }

    void GloboOcular(int change)
    {
        int cont = guardCont[ContadorAtual.globoOcular];

        MaskedTexture[] m = S.GetMaskedTexDbWithId(SectionDataBase.globoOcular);
        cont = ContadorCiclico.Contar(change, cont, m.Length);
        Material material = mesh.material;
        material.SetTexture(textureTarget, m[cont].baseTex);
        material.SetTexture(textureMaskTarget, m[cont].maskTex);

        guardCont[ContadorAtual.globoOcular] = cont;
    }

    #region antigo_Update
    void Update_b()
    {
        Material material = mesh.material;
        
        int change = 0;
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            change = 1;

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            change = -1;
        }

        ConjuntoDasPartes cdp = partes[(int)cAtual];

        if (change != 0)
        {
            cdp.contador = ContadorCiclico.Contar(change, cdp.contador, cdp.possiveis.Length);
            MudarMesh(ref cdp.atual,cdp.possiveis,cdp.contador);
        }

        change = 0;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            change = 1;

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            change = -1;
        }

        if (change != 0)
        {
            cAtual =  (ContadorAtual)ContadorCiclico.Contar(change,(int)cAtual, partes.Count);
        }

        change = 0;

        if (Input.GetKeyDown(KeyCode.W))
        {
            change = 1;

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            change = -1;
        }

        if (change != 0)
        {
            tamanho = ContadorCiclico.Contar(change, tamanho, 5);

            MudarMeshComTamanho();
        }
    }
    #endregion

    void ChangeCombinedMesh(int change)
    {
        ConjuntoDasPartesComTamanhoCombinado_b c = ConjuntoCombinadoPorID(cAtual);
        ConjuntoDasPartesComTamanhoCombinado_b c_2 = ConjuntoCombinadoPorID(c.combinadoCom);
        
        SectionDataBase meuDb = StringForEnum.GetEnum<SectionDataBase>(cAtual.ToString());
        CombinedChangebleMesh[] m = S.GetCombinedMeshDbWithID(meuDb);
        meuDb = StringForEnum.GetEnum<SectionDataBase>(c.combinadoCom.ToString());
        CombinedChangebleMesh[] m2 = S.GetCombinedMeshDbWithID(meuDb);
        
        c.contador = ContadorCiclico.Contar(change, c.contador, m.Length);
        Color[] guard_1 = GuardColor(c.atual, m[c.contador].coresEditaveis);
        Color[] guard_2 = GuardColor(c_2.atual, m2[c_2.contador].coresEditaveis);
        MudarMesh(ref c.atual, m[c.contador].mesh);
        MudarMesh(ref c_2.atual, GetMeshCombinedWithId(m2,m[c.contador].combinedWithId[0]));

        SetRememberedColors(guard_1, m[c.contador].coresEditaveis, c.atual);
        SetRememberedColors(guard_2, m2[c_2.contador].coresEditaveis, c_2.atual);
    }

    GameObject GetMeshCombinedWithId(CombinedChangebleMesh[] m2,int id)
    {
        GameObject retorno = null;
        for (int i = 0; i < m2.Length; i++)
        {
            if (m2[i].toCombineId == id)
                retorno = m2[i].mesh;
        }

        return retorno;
    }

    void MudarMeshComTamanho()
    {
        ConjuntoDasPartesComTamanhoCombinado c = partesComTamanho[0];
        ConjuntoDasPartesComTamanhoCombinado c_2 = partesComTamanho[1];

        MudarMesh(ref c.atual, c.possiveis[c.contador].tamanhosDeObjetos[tamanho].objeto);
        MudarMesh(ref c_2.atual, c_2.possiveis[c_2.contador].tamanhosDeObjetos[
            c.possiveis[c.contador].tamanhosDeObjetos[tamanho].CombinadoCom
            ].objeto);
    }

    void MudarMesh(ref GameObject atualG, GameObject[] objetosV, int contador)
    {
        MudarMesh(ref atualG, objetosV[contador]);
    }

    void MudarMesh(ref GameObject atualG,GameObject objetosV)
    {
        Transform atual = atualG.transform;
        Transform novo = Instantiate(objetosV, atual.parent).transform;

        novo.gameObject.SetActive(true);

        novo.localPosition = atual.localPosition;
        novo.localRotation = atual.localRotation;
        SkinnedMeshRenderer sNovo = novo.GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer sAtual = atual.GetComponent<SkinnedMeshRenderer>();

        if (sNovo && sAtual)
        {
            sNovo.bones = sAtual.bones;
            sNovo.rootBone = sAtual.rootBone;
        }

        Destroy(atualG);

        atualG = novo.gameObject;
    }

    void TrocaMesh(int change)
    {
        SectionDataBase meuDb = StringForEnum.GetEnum(cAtual.ToString(), SectionDataBase.cabelo);
        SimpleChangebleMesh[] m = S.GetDbMeshWithId(meuDb);
        
        int index = guardCont[cAtual];
        Color[] C = GuardColor(partes_b[index].atual, m[partes_b[index].contador].coresEditaveis);
        partes_b[index].contador = ContadorCiclico.Contar(change, partes_b[index].contador,m.Length);
        MudarMesh(ref partes_b[index].atual, m[partes_b[index].contador].mesh);
        SetRememberedColors(C, m[partes_b[index].contador].coresEditaveis, partes_b[index].atual);
    }

    Color[] GuardColor(GameObject G, ColorContainer[] ccs)
    {
        Color[] retorno = new Color[ccs.Length];
        Material material;
        
        for (int i = 0; i < retorno.Length; i++)
        {
            material = GetMaterialForChangeColor(G, ccs[i]);
            retorno[i] = material.GetColor(ccs[i].ColorTargetName);
        }
        return retorno;
    }

    void SetRememberedColors(Color[] C, ColorContainer[] ccs, GameObject G)
    {
        Material material;

        for (int i = 0; i < ccs.Length; i++)
        {
            material = GetMaterialForChangeColor(G, ccs[i]);
            material.SetColor(ccs[i].ColorTargetName, C[i]);
        }
    }


    #region suprimido

    /*
    void MudarNariz()
    {
        Transform atual = narizAtual.transform;
        Transform novo = Instantiate(possiveisNarizes[contadorDeNariz],atual.parent).transform;

        novo.gameObject.SetActive(true);

        novo.localPosition = atual.localPosition;
        novo.localRotation = atual.localRotation;

        Destroy(atual.gameObject);

        narizAtual = novo.gameObject;
    }*/
    /*
    void EfetuarMudancaVisual(ContadorAtual id)
    {

        switch (id)
        {
            case ContadorAtual.Nariz:
            case ContadorAtual.Queixo:

            break;
        }
        Transform atual = cabeloAtual.transform;
        Transform novo = Instantiate(possiveisCabelos[contadorDeCabelo], atual.parent).transform;
        novo.gameObject.SetActive(true);
       

        novo.localPosition = atual.localPosition;
        novo.localRotation = atual.localRotation;
        //novo.GetComponentInChildren<SkinnedMeshRenderer>().bones = atual.GetComponentInChildren<SkinnedMeshRenderer>().bones;

        Destroy(atual.gameObject);

        cabeloAtual = novo.gameObject;
    }*/
    #endregion
}

public struct MsgChangeColorPicker:IMessageBase
{
    public Color color;
}