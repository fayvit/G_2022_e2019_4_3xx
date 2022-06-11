using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace FayvitSave
{
    public class RawLoadAndSave
    {
        public static void SalvarNoPlayerPrefs(string id,object conteudo)
        {
            byte[] b = FayvitBasicTools.BytesTransform.ToBytes(conteudo);
            preJSON pre = new preJSON() { b = b };
            PlayerPrefs.SetString(id, JsonUtility.ToJson(pre));
            PlayerPrefs.Save();
        }

        public static T CarregarDoPlayerPrefs<T>(string id)
        {
            preJSON pre = JsonUtility.FromJson<preJSON>(PlayerPrefs.GetString(id));
            T retorno = default;
            if(pre!=null)
                if(pre.b!=null)
                    retorno = FayvitBasicTools.BytesTransform.ToObject<T>(pre.b);
            return retorno;
        }

        public static void SalvarArquivo(string nomeArquivo, object conteudo, string mainPath = "")
        {
            if (string.IsNullOrEmpty(mainPath))
                mainPath = Application.persistentDataPath;

            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                FileStream file = File.Create(mainPath + "/" + nomeArquivo);
                bf.Serialize(file, conteudo);
                file.Close();
            }
            catch (IOException e)
            {
                Debug.Log(e.StackTrace);
                Debug.LogWarning("Save falhou");
            }
        }

        public static T CarregarArquivo<T>(string nomeArquivo, string mainPath = "")
        {
            if (string.IsNullOrEmpty(mainPath))
                mainPath = Application.persistentDataPath;

            object retorno = null;
            if (File.Exists(mainPath + "/" + nomeArquivo))
            {
                //Debug.Log(mainPath);
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(mainPath + "/" + nomeArquivo, FileMode.Open);
                retorno = bf.Deserialize(file);
                file.Close();
            }

            return (T)retorno;
        }
    }
}