using UnityEngine;
using System.Collections;
using GameJolt.API;
using FayvitBasicTools;
using FayvitMessageAgregator;
using System.Collections.Generic;
using CustomizationSpace;

namespace MyJoltSpace
{
    public class SaveAndLoadInJolt
    {
        public static bool estaCarregado = false;
        private static float ultimoGlobalSave;
        public static void SavedaGalera(CustomizationContainerDates ccd)
        {
            if (Mathf.Abs(ultimoGlobalSave - Time.time) > 30 || ultimoGlobalSave == 0)
            {
                ultimoGlobalSave = Time.time;
                DataStore.Get("1234", true, (string S2) =>
                {
                    SaveDatesForJolt S;
                    if (!string.IsNullOrEmpty(S2))
                    {
                        S = BytesTransform.ToObject<SaveDatesForJolt>(JsonUtility.FromJson<preJSON>(S2).b);
                    }
                    else
                    {
                        Debug.Log("string nula do Jolt");
                        S = new SaveDatesForJolt()
                        {
                            ccds = new ToSaveCustomizationContainer()
                            {
                                ccds = new List<CustomizationContainerDates>()
                            }
                        };
                    }

                    ccd.Sid = GameJoltAPI.Instance.CurrentUser.Name + " :" + ccd.Sid;
                    S.ccds.ccds.Add(ccd);

                    Debug.Log("salvou: " + GameJoltAPI.Instance.CurrentUser.ID.ToString());

                    byte[] sb = BytesTransform.ToBytes(S);
                    preJSON pre = new preJSON()
                    {
                        numeroDePersonagens = S.NumeroDePersonagens.x,
                        numeroFeminino = S.NumeroDePersonagens.y,
                        numeroMasculino = S.NumeroDePersonagens.z,
                        b = sb
                    };

                    DataStore.Set("1234",
                            JsonUtility.ToJson(pre), true,
                           (bool f) =>
                           {
                               if (f)
                                   Debug.Log("foi para o global corretamente");
                               else
                                   Debug.Log("falha ao salvar no global");
                           });
                });
            }
            else
            {
                Debug.Log("condição do temppo");
            }
        }
            

        public static void Save()
        {
            if (SaveDatesForJolt.instance.ccds.ccds != null && GameJoltAPI.Instance.CurrentUser != null)
            {
                Debug.Log("salvou: " + GameJoltAPI.Instance.CurrentUser.ID.ToString());
                SaveDatesForJolt s = SaveDatesForJolt.instance;
                byte[] sb = BytesTransform.ToBytes(SaveDatesForJolt.instance);
                preJSON pre = new preJSON()
                {
                    numeroDePersonagens = s.NumeroDePersonagens.x,
                    numeroFeminino = s.NumeroDePersonagens.y,
                    numeroMasculino = s.NumeroDePersonagens.z,
                    b = sb
                };

                DataStore.Set(GameJoltAPI.Instance.CurrentUser.ID.ToString(),
                        JsonUtility.ToJson(pre), true,
                       Acertou);
            }
            else
            {
                MessageAgregator<MsgFinishTrySaveInJolt>.Publish(new MsgFinishTrySaveInJolt());
            }
        }

        public static void Load()
        {
            if (GameJoltAPI.Instance.CurrentUser != null)
            {

                DataStore.Get(GameJoltAPI.Instance.CurrentUser.ID.ToString(), true, (string S2) =>
                {
                    if (!string.IsNullOrEmpty(S2))
                    {

                        Debug.Log("Dados Carregados do Jolt");
                        SaveDatesForJolt.instance = BytesTransform.ToObject<SaveDatesForJolt>(JsonUtility.FromJson<preJSON>(S2).b);
                        //SaveDatesForJolt.SetSavesWithBytes();
                    }
                    else
                    {
                        Debug.Log("string nula do Jolt");
                        SaveDatesForJolt.instance = new SaveDatesForJolt()
                        {
                            ccds = new ToSaveCustomizationContainer()
                            {
                                ccds = new List<CustomizationContainerDates>()
                            },
                            lccds = new ListToSaveCustomizationContainer()
                            {
                                dccd = new Dictionary<string, List<CustomizationContainerDates>>()
                            }
                        };
                    }

                    GameObject.FindObjectOfType<JoltLoginManager>().StartCoroutine(Carregado());
                });
            }
        }

        static void Acertou(bool foi)
        {
            MessageAgregator<MsgFinishTrySaveInJolt>.Publish(new MsgFinishTrySaveInJolt()
            {
                result = foi
            });

            if (foi)
            {
                Debug.Log("Deu certo load jolt");
            }
            else
                Debug.Log("algo errado");
        }

        static IEnumerator Carregado()
        {
            yield return new WaitForEndOfFrame();
            estaCarregado = true;
        }

        public static void SaveList(string dicKey, CustomizationContainerDates ccd)
        {
            if (SaveDatesForJolt.instance.lccds.dccd != null)
            {
                if (SaveDatesForJolt.instance.lccds.dccd.ContainsKey(dicKey))
                    SaveDatesForJolt.instance.lccds.dccd[dicKey].Add(ccd);
                else
                    SaveDatesForJolt.instance.lccds.dccd.Add(dicKey, new List<CustomizationContainerDates>() { ccd });

                Save();
            }
        }

        public static void SaveMember(CustomizationContainerDates ccd)
        {
            Debug.Log("Raw save");
            if (SaveDatesForJolt.instance.ccds.ccds != null)
            {
                Debug.Log("instance content");
                SaveDatesForJolt.instance.ccds.ccds.Add(ccd);
                SaveAndLoadInJolt.Save();
                SaveAndLoadInJolt.SavedaGalera(ccd);
            }
        }

        public static void CreateList(string dicKey)
        {
            if (!SaveDatesForJolt.instance.lccds.dccd.ContainsKey(dicKey))
            {
                SaveDatesForJolt.instance.lccds.dccd.Add(dicKey, new List<CustomizationContainerDates>());
            }
        }
    }

    [System.Serializable]
    public class preJSON
    {
        public int numeroDePersonagens;
        public int numeroMasculino;
        public int numeroFeminino;
        public byte[] b;
    }

    [System.Serializable]
    public struct SaveDatesForJolt
    {
        public ToSaveCustomizationContainer ccds;
        public ListToSaveCustomizationContainer lccds;

        public static SaveDatesForJolt instance;

        public Vector3Int NumeroDePersonagens{
            get{
                Vector3Int V = new Vector3Int(ccds.ccds.Count, 0, 0);
                for (int i = 0; i < V.x; i++)
                {
                    if (ccds.ccds[i].PersBase == PersonagemBase.feminino)
                        V.y++;
                    else
                        V.z++;
                }

                if(lccds.dccd!=null)
                    foreach (var v in lccds.dccd)
                    {
                        V.x += v.Value.Count;
                        for (int i = 0; i < v.Value.Count; i++)
                            if (v.Value[i].PersBase == PersonagemBase.feminino)
                                V.y++;
                            else
                                V.z++;
                    }
                return V;
            }
        }
    }

    public struct MsgFinishTrySaveInJolt : IMessageBase
    {
        public bool result;
    }

}