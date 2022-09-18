using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using FayvitMessageAgregator;
using FayvitBasicTools;
using CustomizationSpace;

namespace CustomizationSpace
{

    public class SectionCustomizationManager : MonoBehaviour
    {
        [ArrayElementTitle("id"), SerializeField] private List<SimpleMesh> malhas;
        [ArrayElementTitle("id"), SerializeField] private List<CombinedMesh> malhasComb;
        [ArrayElementTitle("id"), SerializeField] private List<CustomizationTextures> texturasE;
        [ArrayElementTitle("id"), SerializeField] private List<ColorAssignements> colorAssign;

        public bool masculino = true;

        [SerializeField] private SectionDataBaseContainer sdbc;
        private SectionDataBase transportSDB;
        private int transportInt;

        public List<ColorAssignements> ColorAssign { get => colorAssign; }
        public ColorContainerStruct GuardOriginalColor { get; private set; }
        public RegistroDeCores GetTargetColorReg
        {
            get => GetColorAssignById(transportSDB).coresEditaveis[transportInt].coresEditaveis.registro;
        }

        private void Start()
        {
            sdbc = GetComponent<SectionDataBaseContainer>();
            if (sdbc == null)
                sdbc = FindObjectOfType<SectionDataBaseContainer>();
            SetStarterColorAssing();

            #region testeGridSuggestionSuprimido
            //cdbManager = new ColorDbManager(mySuggestionColors,myGetColor);
            //transportInt = 0;
            //transportSDB = SectionDataBase.@base;
            //EventAgregator.AddListener(EventKey.changeColorPicker, OnChangeColorPicker);
            //MessageAgregator<UiDeOpcoesChangeMessage>.AddListener(OnUiChange);
            #endregion

            #region mainCOlors
            //myGetColor.transform.parent.gameObject.SetActive(true);


            //CR_SupportSingleton.Instance.InvokeOnEndFrame(gameObject,() =>
            //{
            //    Color[] C = myGetColor.RoletaDeCores();
            //    mySuggestionColors.StartHud((int x) => { }, C);
            //    myGetColor.transform.parent.gameObject.SetActive(false);
            //    List<float4> f4 = new List<float4>();
            //    for (int i = 0; i < C.Length; i++)
            //    {
            //        f4.Add(new float4(C[i].r, C[i].g, C[i].b, C[i].a));
            //    }

            //    LoadAndSaveGame.SalvarArquivo("testeCustomCharacter/DateColors/mainColors.crs", f4, Application.dataPath);
            //});
            #endregion
        }

        private void Update()
        {
            /*cdbManager.Update();*/
        }



        #region privateNonVoid

        ColorAssignements GetColorAssignements(SectionDataBase member)
        {
            ColorAssignements retorno = null;
            for (int i = 0; i < colorAssign.Count; i++)
            {
                if (colorAssign[i].id == member)
                    retorno = colorAssign[i];
            }

            return retorno;
        }

        GameObject GetGameObjectWithParentID(SectionDataBase id)
        {
            GameObject retorno = null;
            bool foi;

            do
            {
                foi = true;
                for (int i = 0; i < texturasE.Count; i++)
                {
                    if (texturasE[i].id == id)
                    {
                        MaskedTexture[] m = sdbc.GetMaskedTexDbWithId(texturasE[i].id);
                        id = m[texturasE[i].contador].meshParent;
                        foi = false;
                    }
                }
            } while (!foi);

            for (int i = 0; i < malhas.Count; i++)
                if (malhas[i].id == id)
                    retorno = malhas[i].atual;

            for (int i = 0; i < malhasComb.Count; i++)
                if (malhasComb[i].id == id)
                    retorno = malhasComb[i].atual;

            return retorno;
        }

        Color FindAnotherReg(ColorContainer cc, Color forDefault)
        {
            foreach (var cs in colorAssign)
            {
                foreach (var ccs in cs.coresEditaveis)
                {
                    if (ccs.coresEditaveis != null && cc.registro == ccs.coresEditaveis.registro && cc != ccs.coresEditaveis)
                    {
                        forDefault = ccs.cor;
                    }
                }
            }

            return forDefault;
        }

        Material GetMaterialForChangeElement(GameObject G, SignatureBase sb)
        {
            Material retorno = null;

            if (sb.meshOrChildren == MainOrChildren.parentObject)
                G = G.transform.parent.gameObject;

            if (sb.meshOrChildren == MainOrChildren.childrenObject)
            {
                retorno = G.GetComponentInChildren<Renderer>().materials[sb.indiceDoMaterialAlvo];
            }
            else if (sb.meshOrChildren == MainOrChildren.mainObject)
            {
                retorno = G.GetComponent<Renderer>().materials[sb.indiceDoMaterialAlvo];
            }

            return retorno;
        }

        SimpleMesh GetMeshInListById(SectionDataBase sdb)
        {
            for (int i = 0; i < malhas.Count; i++)
            {
                if (malhas[i].id == sdb)
                    return malhas[i];
            }

            return null;
        }

        CombinedMesh GetCombinedMeshInListById(SectionDataBase sdb)
        {
            for (int i = 0; i < malhasComb.Count; i++)
            {
                if (malhasComb[i].id == sdb)
                    return malhasComb[i];
            }

            return null;
        }

        CustomizationTextures GetCustomizationTexInListByID(SectionDataBase sdb)
        {
            for (int i = 0; i < texturasE.Count; i++)
            {
                if (texturasE[i].id == sdb)
                    return texturasE[i];
            }

            return null;
        }

        CustomizationIdentity GetCustomizationIdentity(SectionDataBase index)
        {

            foreach (var m in malhas)
                if (m.id == index)
                    return m;

            foreach (var c in malhasComb)
                if (c.id == index)
                    return c;

            foreach (var t in texturasE)
                if (t.id == index)
                    return t;

            return null;
            //if (ce is SimpleChangebleMesh)
            //{
            //    retorno = GetMeshInListById(sdb);
            //}
            //else if (ce is CombinedChangebleMesh)
            //{
            //    retorno = GetCombinedMeshInListById(sdb);
            //}
            //else if (ce is MaskedTexture)
            //{
            //    retorno = GetCustomizationTexInListByID(sdb);
            //}

            //return retorno;
        }

        Color[] GuardColor(GameObject G, ColorContainer[] ccs)
        {
            Color[] retorno = new Color[ccs.Length];
            Material material;

            for (int i = 0; i < retorno.Length; i++)
            {
                material = GetMaterialForChangeElement(G, ccs[i]);
                retorno[i] = material.GetColor(ccs[i].ColorTargetName);
            }
            return retorno;
        }

        GameObject GetMeshCombinedWithId(CombinedChangebleMesh[] m2, int id)
        {
            GameObject retorno = null;
            for (int i = 0; i < m2.Length; i++)
            {
                if (m2[i].toCombineId == id)
                    retorno = m2[i].mesh;
            }

            return retorno;
        }

        GameObject GetSignaturedGO(SectionDataBase sdb)
        {
            GameObject G;
            SignatureBase[] sbs;
            SignatureBase[] sbs2;

            SetSignaturesAndGO(sdb, out G, out sbs, out sbs2);

            return G;
        }
        #endregion


        #region privateVoid

        void SetSignatureDbAndGO(SectionDataBase sdb, out GameObject G, out SignatureBase[] sb)
        {
            SignatureBase[] nonUseSb;
            G = null;
            sb = null;

            SetSignaturesAndGO(sdb, out G, out nonUseSb, out sb);
        }

        void SetLocalSignatureAndGO(SectionDataBase sdb, out SignatureBase[] sb, out GameObject G)
        {
            SignatureBase[] nonUseSb;
            G = null;
            sb = null;

            SetSignaturesAndGO(sdb, out G, out sb, out nonUseSb);
        }

        void SetStarterColorAssing()
        {
            for (int i = 0; i < malhas.Count; i++)
            {
                UpdateColorAssingOfID(malhas[i]);
            }

            for (int i = 0; i < malhasComb.Count; i++)
            {
                UpdateColorAssingOfID(malhasComb[i]);
            }

            for (int i = 0; i < texturasE.Count; i++)
            {
                MaskedTexture[] m = sdbc.GetMaskedTexDbWithId(texturasE[i].id);
                GameObject G = GetGameObjectWithParentID(m[texturasE[i].contador].meshParent);
                UpdateColorAssingOfID(texturasE[i].id, G, m[texturasE[i].contador].coresEditaveis);
            }
        }

        void UpdateColorAssingOfID(CombinedMesh m)
        {
            CombinedChangebleMesh scm = sdbc.GetCombinedMeshDbWithID(m.id)[m.contador];
            UpdateColorAssingOfID(m.id, m.atual, scm.coresEditaveis);
        }

        void UpdateColorAssingOfID(SimpleMesh m)
        {
            SimpleChangebleMesh scm = sdbc.GetDbMeshWithId(m.id)[m.contador];
            UpdateColorAssingOfID(m.id, m.atual, scm.coresEditaveis);
        }

        void UpdateColorAssingOfID(SectionDataBase id, GameObject G, ColorContainer[] ccs)
        {

            int intTarget = -1;
            for (int i = 0; i < colorAssign.Count; i++)
            {
                if (colorAssign[i].id == id)
                {
                    intTarget = i;
                }
            }

            if (intTarget == -1)
            {
                intTarget = colorAssign.Count;
                colorAssign.Add(new ColorAssignements());
            }

            ColorAssignements target = colorAssign[intTarget];
            target.id = id;
            target.coresEditaveis = new ColorContainerStruct[ccs.Length];

            for (int i = 0; i < ccs.Length; i++)
            {

                Material material = GetMaterialForChangeElement(G, ccs[i]);
                //Debug.Log(G.name+" : "+target.id);


                Color corParaInserir = material.GetColor(ccs[i].ColorTargetName);
                if (ccs[i].registro != RegistroDeCores.registravel)
                {
                    corParaInserir = FindAnotherReg(ccs[i], corParaInserir);
                    material.SetColor(ccs[i].ColorTargetName, corParaInserir);
                }
                target.coresEditaveis[i] = new ColorContainerStruct()
                {
                    coresEditaveis = new ColorContainer()
                    {
                        indiceDoMaterialAlvo = ccs[i].indiceDoMaterialAlvo,
                        registro = ccs[i].registro,
                        materialColorTarget = ccs[i].materialColorTarget,
                        meshOrChildren = ccs[i].meshOrChildren
                    },
                    cor = corParaInserir

                };
            }

        }

        void SetRememberedColors(Color[] C, ColorContainer[] ccs, GameObject G)
        {
            Material material;

            for (int i = 0; i < ccs.Length; i++)
            {
                material = GetMaterialForChangeElement(G, ccs[i]);
                material.SetColor(ccs[i].ColorTargetName, C[i]);
            }
        }

        void MudarMesh(ref GameObject atualG, GameObject[] objetosV, int contador)
        {
            MudarMesh(ref atualG, objetosV[contador]);
        }

        void MudarMesh(ref GameObject atualG, GameObject objetosV)
        {
            //LogType.
            Debug.unityLogger.logEnabled = false;// Desabilitei o Log pq o unity reclamava de ossos e tecidos nessa parte
            Transform atual = atualG.transform;
            Transform novo = Instantiate(objetosV, atual.parent).transform;

            novo.gameObject.SetActive(true);
            Debug.unityLogger.logEnabled = true;

            novo.localPosition = atual.localPosition;
            novo.localRotation = atual.localRotation;
            SkinnedMeshRenderer sNovo = novo.GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer sAtual = atual.GetComponent<SkinnedMeshRenderer>();

            if (sNovo && sAtual)
            {
                sNovo.bones = sAtual.bones;
                sNovo.rootBone = sAtual.rootBone;
            }

            if (Application.isPlaying)
            {
                //Debug.Log("Application yes playing: ");
                Destroy(atualG);

            }
            else
            {
                DestroyImmediate(atualG);
                //Debug.Log("Application not playing");
            }

            atualG = novo.gameObject;
        }

        void VerifyMaskedSubChanges(ChangebleElement ce)
        {
            if (ce.subsection.Length > 0)
            {
                for (int i = 0; i < ce.subsection.Length; i++)
                {
                    Debug.Log(ce.subsection[i] + " são subsections");
                    ChangebleElement[] ces = sdbc.GetChangebleElementWithId(ce.subsection[i]);
                    CustomizationIdentity cId = GetCustomizationIdentity(ce.subsection[i]);
                    ChangeTextureElementTo(ce.subsection[i], ces as MaskedTexture[], cId.contador);
                }
            }
        }

        void SetAssignedColors(SectionDataBase sdb, ColorContainer[] ccs, GameObject G)
        {
            Material material;
            for (int i = 0; i < ccs.Length; i++)
            {
                ColorAssignements ca = GetColorAssignById(sdb);
                if (ca.coresEditaveis.Length > i)
                {
                    material = GetMaterialForChangeElement(G, ccs[i]);

                    material.SetColor(ccs[i].ColorTargetName, ca.coresEditaveis[i].cor);
                }
            }
        }

        void SetSignaturesAndGO(SectionDataBase sdb, out GameObject G, out SignatureBase[] sb, out SignatureBase[] dbSb)
        {
            ChangebleElement[] ces = sdbc.GetChangebleElementWithId(sdb);
            sb = null;
            dbSb = null;
            G = null;

            if (ces[0] is CombinedChangebleMesh)
            {
                CombinedMesh cm = GetCombinedMeshInListById(sdb);
                dbSb = ces[cm.contador].coresEditaveis;
                sb = GetColorAssignById(sdb).VetorDeCoresEditaveis;
                G = cm.atual;

            }
            else if (ces[0] is SimpleChangebleMesh)
            {
                SimpleMesh sm = GetMeshInListById(sdb);
                dbSb = ces[sm.contador].coresEditaveis;
                sb = GetColorAssignById(sdb).VetorDeCoresEditaveis;
                G = sm.atual;
            }
            else if (ces[0] is MaskedTexture)
            {
                CustomizationTextures ct = GetCustomizationTexInListByID(sdb);
                MaskedTexture m = ces[ct.contador] as MaskedTexture;
                G = GetGameObjectWithParentID(m.meshParent);
                dbSb = ces[ct.contador].coresEditaveis;
                sb = GetColorAssignById(sdb).VetorDeCoresEditaveis;
            }
        }
        #endregion


        #region publicNonVoid

        public int GetActiveIndexOf(SectionDataBase index)
        {
            foreach (var m in malhas)
                if (m.id == index)
                    return m.contador;

            foreach (var c in malhasComb)
                if (c.id == index)
                    return c.contador;

            foreach (var t in texturasE)
                if (t.id == index)
                    return t.contador;

            return -1;
        }

        public Color GetColorOfMember(SectionDataBase member, int inIndex)
        {
            //Debug.Log(member + " : " + inIndex);

            ColorAssignements C = GetColorAssignById(member, inIndex);
            return C.coresEditaveis[inIndex].cor;
        }

        public ColorAssignements GetColorAssignById(SectionDataBase member, int inIndex = -1)
        {
            ColorAssignements retorno = GetColorAssignements(member);

            if (retorno == null || retorno.coresEditaveis.Length <= inIndex)
            {
                SignatureBase[] sb;
                GameObject G;
                SetSignatureDbAndGO(member, out G, out sb);

                ColorContainer[] cc = sb as ColorContainer[];
                UpdateColorAssingOfID(member, G, cc);

                retorno = retorno ?? GetColorAssignements(member);
            }


            return retorno;
        }

        public CustomizationSpace.CustomizationContainerDates GetCustomDates()
        {
            CustomizationContainerDates ccd = new CustomizationContainerDates();
            ccd.SetDates(
                masculino ? PersonagemBase.masculino : PersonagemBase.feminino,
                malhas, malhasComb, texturasE, colorAssign);
            return ccd;
        }

        public Dictionary<RegistroDeCores, Color> VerifyColorReg()
        {
            Dictionary<RegistroDeCores, Color> retorno = new Dictionary<RegistroDeCores, Color>();
            foreach (var c in colorAssign)
            {
                foreach (var cc in c.coresEditaveis)
                    if (cc.coresEditaveis.registro != RegistroDeCores.registravel
                        &&
                        cc.coresEditaveis.registro != RegistroDeCores.skin
                        )
                    {
                        if (!retorno.ContainsKey(cc.coresEditaveis.registro))
                        {
                            retorno.Add(cc.coresEditaveis.registro, cc.cor);
                        }
                    }
            }

            return retorno;
        }
        #endregion


        #region publicVoid

        public void TrocaMesh(SectionDataBase meuDb, SimpleChangebleMesh[] m, int change)
        {
            SimpleMesh sMesh = GetMeshInListById(meuDb);
            //Color[] C = GuardColor(sMesh.atual, m[sMesh.contador].coresEditaveis);
            sMesh.contador = ContadorCiclico.Contar(change, sMesh.contador, m.Length);
            TrocaMeshTo(meuDb, m, sMesh.contador);
            //SetRememberedColors(C, m[sMesh.contador].coresEditaveis, sMesh.atual);
        }

        public void TrocaMeshTo(SectionDataBase meuDb, SimpleChangebleMesh[] m, int to)
        {
            SimpleMesh sMesh = GetMeshInListById(meuDb);
            sMesh.contador = to;
            MudarMesh(ref sMesh.atual, m[sMesh.contador].mesh);
            VerifyMaskedSubChanges(m[sMesh.contador]);
            SetAssignedColors(meuDb, m[sMesh.contador].coresEditaveis, sMesh.atual);
        }

        public void ChangeCombinedMesh(SectionDataBase meuDb, CombinedChangebleMesh[] m, CombinedChangebleMesh[] m2, int change)
        {
            CombinedMesh c = GetCombinedMeshInListById(meuDb);
            CombinedMesh c_2 = GetCombinedMeshInListById(c.combinadoCom);

            c.contador = ContadorCiclico.Contar(change, c.contador, m.Length);
            c_2.contador = m[c.contador].combinedWithId[0];

            MudarMesh(ref c.atual, m[c.contador].mesh);
            MudarMesh(ref c_2.atual, GetMeshCombinedWithId(m2, m[c.contador].combinedWithId[0]));

            VerifyMaskedSubChanges(m[c.contador]);
            VerifyMaskedSubChanges(m2[c_2.contador]);

            SetAssignedColors(meuDb, m[c.contador].coresEditaveis, c.atual);
            SetAssignedColors(c.combinadoCom, m2[c_2.contador].coresEditaveis, c_2.atual);
        }

        public void ChangeTextureElementTo(SectionDataBase sdb, MaskedTexture[] m, int to)
        {
            CustomizationTextures ct = GetCustomizationTexInListByID(sdb);

            if (ct == null)
            {
                Debug.Log("CustomizationTexture não encontrado para : " + sdb);
                return;
            }
            ct.contador = to;
            MaskedTexture mm = m[to];
            ITextureContainerElement tce = sdbc.GetChangebleElementWithId(mm.meshParent)[GetActiveIndexOf(mm.meshParent)] as ITextureContainerElement;
            TextureContainer tc = tce.GetTexContainerByID(sdb);
            GameObject G = GetGameObjectWithParentID(mm.meshParent);
            Material material = GetMaterialForChangeElement(G, tc);
            material.SetTexture(tc.BaseIdName, mm.baseTex);
            material.SetTexture(tc.MaskedIdName, mm.maskTex);
            SetAssignedColors(sdb, mm.coresEditaveis, G);

        }

        public void ChangeTextureElement(SectionDataBase sdb, MaskedTexture[] m, int change)
        {

            CustomizationTextures ct = GetCustomizationTexInListByID(sdb);

            ct.contador = ContadorCiclico.Contar(change, ct.contador, m.Length);

            ChangeTextureElementTo(sdb, m, ct.contador);


        }

        public void SetCustomDates(CustomizationContainerDates ccd)
        {
            List<CustomizationIdentity> malhas;
            List<CustomizationIdentity> malhasComb;
            List<CustomizationIdentity> texturasE;
            List<ColorAssignements> colorAssign;

            ccd.GetDates(out malhas, out malhasComb, out texturasE, out colorAssign);
            this.colorAssign = colorAssign;

            foreach (CustomizationIdentity s in malhas)
            {
                SimpleChangebleMesh[] scm = sdbc.GetDbMeshWithId(s.id);
                SimpleMesh c2 = GetMeshInListById(s.id);
                c2.contador = s.contador;
                MudarMesh(ref c2.atual, scm[s.contador].mesh);

            }
            foreach (CustomizationIdentity c in malhasComb)
            {
                CombinedChangebleMesh[] ccm = sdbc.GetCombinedMeshDbWithID(c.id);
                CombinedMesh c2 = GetCombinedMeshInListById(c.id);
                c2.contador = c.contador;
                MudarMesh(ref c2.atual, ccm[c.contador].mesh);
                c2.contador = c.contador;
            }

            foreach (CustomizationIdentity ct in texturasE)
            {
                MaskedTexture[] mt = sdbc.GetMaskedTexDbWithId(ct.id);
                ChangeTextureElementTo(ct.id, mt, ct.contador);
            }

            //SetColorsByAssign(colorAssign);
            foreach (ColorAssignements ca in colorAssign)
                for (int i = 0; i < ca.coresEditaveis.Length; i++)
                {
                    transportInt = i;
                    transportSDB = ca.id;

                    ApplyColor(ca.coresEditaveis[i].cor);
                }
        }

        public void SetColorsByAssign(List<ColorAssignements> receivedCA)
        {
            ColorAssignements baseAssign = null;
            foreach (ColorAssignements ca in receivedCA)
            {
                int cont = 0;
                baseAssign = ca.id == SectionDataBase.@base ? ca : baseAssign;

                for (int i = 0; i < ca.coresEditaveis.Length; i++)
                {

                    GameObject G;
                    SignatureBase[] sbs;
                    SignatureBase[] dbSbs;

                    SetSignaturesAndGO(ca.id, out G, out sbs, out dbSbs);

                    if (cont >= sbs.Length)
                        break;
                    SignatureBase sb = sbs[cont];

                    if (ca.coresEditaveis[i].coresEditaveis.registro != RegistroDeCores.skin
                        && (sb as ColorContainer).registro != RegistroDeCores.skin)
                    {

                        transportInt = cont;
                        transportSDB = ca.id;

                        ApplyColor(ca.coresEditaveis[i].cor);
                        cont++;
                    }
                }
            }

            transportInt = 0;
            transportSDB = SectionDataBase.@base;

            ApplyColor(baseAssign.coresEditaveis[0].cor);
        }

        public void StartChangeColor(SectionDataBase sdb, int inIndex, ColorContainerStruct guardColor)
        {
            transportSDB = sdb;
            transportInt = inIndex;

            GuardOriginalColor = guardColor.CcsClone();

        }

        public void EndChangeColor(bool effective)
        {
            if (!effective)
            {
                GetColorAssignById(transportSDB).coresEditaveis[transportInt] = GuardOriginalColor;
                ApplyColor(GuardOriginalColor.cor);
            }
            /*
            EventAgregator.RemoveListener(EventKey.changeColorPicker, OnChangeColorPicker);
            myGetColor.transform.parent.gameObject.SetActive(false);

            if (!efetive)
                ApplyColor(guardOriginalColor);
                */


        }

        public void ApplyColor(Color c)
        {
            SectionDataBase sdb = transportSDB;
            int inIndex = transportInt;

            GameObject G;
            SignatureBase[] sbs;
            SignatureBase[] dbSbs;

            SetSignaturesAndGO(sdb, out G, out sbs, out dbSbs);

            if (inIndex >= dbSbs.Length)
                return;
            SignatureBase sb = sbs[inIndex];

            Material material = GetMaterialForChangeElement(G, sb);

            if (dbSbs.Length > inIndex)
                material.SetColor(((ColorContainer)dbSbs[inIndex]).materialColorTarget.ToString(), c);

            ColorContainerStruct[] ccs = GetColorAssignById(sdb).coresEditaveis;

            if (ccs.Length > inIndex)
                ccs[inIndex].cor = c;

            if (((ColorContainer)sb).registro != RegistroDeCores.registravel)
                ChangeRegColor(c, ((ColorContainer)sb).registro);

            MessageAgregator<MsgApplyColor>.Publish(new MsgApplyColor() { c = c });

        }

        public void ChangeRegColor(Color skinColor, RegistroDeCores registrado)
        {
            #region old
            //    for (int i = 0; i < malhas.Count; i++)
            //    {
            //        GameObject G;
            //        SignatureBase[] sbs;

            //        SetSignatureAndGO(malhas[i].id, out sbs, out G);

            //        ColorContainer[] scm = sbs as ColorContainer[];

            //        for (int j = 0; j < scm.Length; j++)
            //        {
            //            if (scm[j].registro == registrado)
            //            {
            //                Material material = GetMaterialForChangeElement(G, scm[j]);
            //                material.SetColor(scm[j].ColorTargetName, skinColor);
            //            }
            //        }
            //    }

            //    for (int i = 0; i < malhasComb.Count; i++)
            //    {
            //        SignatureBase[] sc;
            //        GameObject G;
            //        SetSignatureAndGO(malhasComb[i].id, out sc, out G);

            //        ColorContainer[] scm = sc as ColorContainer[];

            //        for (int j = 0; j < scm.Length; j++)
            //        {
            //            if (scm[j].registro == registrado)
            //            {
            //                Material material = GetMaterialForChangeElement(G, scm[j]);
            //                material.SetColor(scm[j].ColorTargetName, skinColor);
            //            }
            //        }
            //    }
            #endregion

            foreach (ColorAssignements ca in colorAssign)
            {

                for (int i = 0; i < ca.coresEditaveis.Length; i++)
                {
                    ColorContainer ccs = ca.coresEditaveis[i].coresEditaveis;

                    if (ccs.registro == registrado)
                    {
                        GameObject G;
                        SignatureBase[] dbSb;

                        SetSignatureDbAndGO(ca.id, out G, out dbSb);
                        if (dbSb.Length > i)
                        {
                            Material material = GetMaterialForChangeElement(G, dbSb[i]);
                            material.SetColor(((ColorContainer)dbSb[i]).ColorTargetName, skinColor);

                            ca.coresEditaveis[i].cor = skinColor;
                        }
                    }
                }
            }
        }

        public void ChangeColorReg(RegistroDeCores r)
        {
            SectionDataBase sdb = transportSDB;
            int inIndex = transportInt;

            //GameObject G;
            //SignatureBase[] sbs;

            //SetSignatureAndGO(sdb, out sbs, out G);

            //if (inIndex >= sbs.Length)
            //    return;
            //SignatureBase sb = sbs[inIndex];

            //((ColorContainer)sb).registro = r;

            GetColorAssignById(sdb).coresEditaveis[inIndex].coresEditaveis.registro = r;

        }

        public void ChangeColorRegIfEqual(RegistroDeCores r, Color remembered, Color targetColor)
        {
            ColorContainer C = GetColorAssignById(transportSDB).coresEditaveis[transportInt].coresEditaveis;
            if (C.registro == r)
            {
                C.registro = RegistroDeCores.registravel;
                ApplyColor(remembered);
            }
            else
            {
                C.registro = r;
                ApplyColor(targetColor);


            }
        }
        #endregion

    }

    public struct MsgApplyColor : IMessageBase
    {
        public Color c;
    }

    #region Subclass
    [System.Serializable]
    public class ColorAssignements
    {
        public SectionDataBase id;
        [ArrayElementTitle("coresEditaveis.materialColorTarget")]
        public ColorContainerStruct[] coresEditaveis;

        public ColorContainer[] VetorDeCoresEditaveis
        {
            get
            {
                List<ColorContainer> cc = new List<ColorContainer>();
                foreach (var ccs in coresEditaveis)
                {
                    cc.Add(ccs.coresEditaveis);
                }

                return cc.ToArray();
            }
        }
    }

    [System.Serializable]
    public struct ColorContainerStruct : System.ICloneable
    {
        public ColorContainer coresEditaveis;
        public Color cor;

        public ColorContainerStruct CcsClone()
        {
            return (ColorContainerStruct)Clone();
        }

        public object Clone()
        {
            return new ColorContainerStruct()
            {
                cor = cor,
                coresEditaveis = new ColorContainer()
                {
                    indiceDoMaterialAlvo = coresEditaveis.indiceDoMaterialAlvo,
                    materialColorTarget = coresEditaveis.materialColorTarget,
                    meshOrChildren = coresEditaveis.meshOrChildren,
                    registro = coresEditaveis.registro
                }
            };
        }
    }

    [System.Serializable]
    public struct SerializableColorAssignements
    {
        public SectionDataBase id;
        public SerializableColorContainerStruct[] coresEditaveis;

        public SerializableColorAssignements(ColorAssignements colorAss)
        {
            id = colorAss.id;
            coresEditaveis = new SerializableColorContainerStruct[colorAss.coresEditaveis.Length];
            for (int i = 0; i < colorAss.coresEditaveis.Length; i++)
            {
                coresEditaveis[i] = new SerializableColorContainerStruct(colorAss.coresEditaveis[i]);
            }
        }

        public ColorAssignements GetCAss
        {
            get
            {
                ColorContainerStruct[] ccs = new ColorContainerStruct[coresEditaveis.Length];
                for (int i = 0; i < coresEditaveis.Length; i++)
                {
                    ccs[i] = coresEditaveis[i].GetCCS;
                }

                return new ColorAssignements()
                {
                    id = id,
                    coresEditaveis = ccs
                };
            }
        }
    }

    [System.Serializable]
    public struct SerializableColorContainerStruct
    {
        public ColorContainer coresEditaveis;
        public float4 cor;

        public SerializableColorContainerStruct(ColorContainerStruct C)
        {
            coresEditaveis = C.coresEditaveis;
            cor = new float4(C.cor.r, C.cor.g, C.cor.b, C.cor.a);
        }

        public ColorContainerStruct GetCCS
        {
            get
            {
                return new ColorContainerStruct()
                {
                    cor = new Color(cor.x, cor.y, cor.z, cor.w),
                    coresEditaveis = coresEditaveis
                };
            }
        }

    }

    [System.Serializable]
    public class CombinedMesh : CustomizationIdentity
    {
        public GameObject atual;
        public SectionDataBase combinadoCom;
    }

    [System.Serializable]
    public class SimpleMesh : CustomizationIdentity
    {
        public GameObject atual;
    }

    [System.Serializable]
    public class CustomizationTextures : CustomizationIdentity { }

    [System.Serializable]
    public class CustomizationIdentity
    {
        public SectionDataBase id;
        public int contador = 0;
    }
    #endregion
}