using FayvitSupportSingleton;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets.ScriptsLadoB.FayvitAdressable
{
    public class AddressablesResourcesManager
    {
        static Dictionary<InfoCommandTexture, List<GameObject>> refInfoCommandTexture = new Dictionary<InfoCommandTexture, List<GameObject>>();
        static Dictionary<InfoCommandTexture, AsyncOperationHandle<Sprite>> infoCommandOperations = new Dictionary<InfoCommandTexture, AsyncOperationHandle<Sprite>>();
        static Dictionary<string, List<GameObject>> refPrefabs = new Dictionary<string, List<GameObject>>();
        static Dictionary<string, AsyncOperationHandle<GameObject>> prefabsOperations = new Dictionary<string, AsyncOperationHandle<GameObject>>();

        public static void SetSprite(InfoCommandTexture i, GameObject spriteContainer, System.Action<Sprite> onLoadSprite)
        {
            if (infoCommandOperations.ContainsKey(i) && infoCommandOperations[i].IsValid())
            {
                if (infoCommandOperations[i].IsDone)
                    onLoadSprite(infoCommandOperations[i].Result);
                else
                    infoCommandOperations[i].Completed += v =>
                    {
                        onLoadSprite(v.Result);
                        if (!refInfoCommandTexture.ContainsKey(i))
                            refInfoCommandTexture.Add(i, new List<GameObject>() { spriteContainer });
                    };

                VerifyLoadSprite(i);
            }
            else
            {
                AsyncOperationHandle<Sprite> loadOperation = Addressables.LoadAssetAsync<Sprite>(i.ToString());
                infoCommandOperations.Add(i, loadOperation);
                loadOperation.Completed += v =>
                {
                    onLoadSprite(v.Result);
                    refInfoCommandTexture.Add(i, new List<GameObject>() { spriteContainer });
                };

                VerifyLoadSprite(i);
            }
        }

        internal static void OtimizePrefabs(string p, GameObject cA)
        {
            if (refPrefabs.ContainsKey(p))
                refPrefabs[p].Add(cA);
            else
                refPrefabs.Add(p, new List<GameObject>() { cA });

            VerifyLoadPrefabs(p);
        }

        private static void VerifyLoadPrefabs(string last)
        {
            SupportSingleton.Instance.InvokeInSeconds(() =>
            {
                foreach (var v in refPrefabs.Keys.ToList())
                {
                    if (v != last)
                    {
                        bool remover = true;
                        foreach (var g in refPrefabs[v])
                        {
                            remover &= (g == null);
                        }

                        if (remover)
                        {

                            Addressables.Release(prefabsOperations[v]);
                            prefabsOperations.Remove(v);
                            refPrefabs.Remove(v);

                        }
                    }
                }

                Resources.UnloadUnusedAssets();
            }, 90);
        }

        internal static GameObject InstantGet(string v, System.Action<GameObject> onLoad = null)
        {
            if (prefabsOperations.ContainsKey(v))
            {
                onLoad?.Invoke(prefabsOperations[v].Result);
                return prefabsOperations[v].WaitForCompletion();
            }
            else
            {
                AsyncOperationHandle<GameObject> loadOperation = Addressables.LoadAssetAsync<GameObject>(v);
                loadOperation.Completed += vv =>
                {
                    onLoad?.Invoke(vv.Result);
                };
                prefabsOperations.Add(v, loadOperation);

                return loadOperation.WaitForCompletion();
            }
        }

        public static void VerifyLoadSprite(InfoCommandTexture last)
        {
            SupportSingleton.Instance.InvokeOnEndFrame(() =>
            {
                foreach (var v in refInfoCommandTexture.Keys.ToList())
                {
                    if (v != last)
                    {
                        bool remover = true;
                        foreach (var g in refInfoCommandTexture[v])
                        {
                            remover &= !(g != null && g.activeSelf && g.GetComponent<UnityEngine.UI.Image>().sprite == infoCommandOperations[v].Result);
                        }

                        if (remover)
                        {

                            Addressables.Release(infoCommandOperations[v]);
                            infoCommandOperations.Remove(v);
                            refInfoCommandTexture.Remove(v);

                        }
                    }
                }

                Resources.UnloadUnusedAssets();
            });
        }
    }
}