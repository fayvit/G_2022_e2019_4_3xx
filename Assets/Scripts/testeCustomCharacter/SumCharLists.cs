using System.Collections.Generic;
using Criatures2021;
using FayvitSave;

public class SumCharLists
{
    public static Dictionary<string, List<CustomizationContainerDates>> Dccd {
        get
        {
            
            Dictionary<string, List<CustomizationContainerDates>> dccd = new Dictionary<string, List<CustomizationContainerDates>>();
            ListToSaveCustomizationContainer.Instance.Load();
            Dictionary<string, List<CustomizationContainerDates>> dccd2 = ListToSaveCustomizationContainer.Instance.dccd;
            foreach (var v in dccd2.Keys)
            {
                dccd[v] = dccd2[v];
            }

            dccd2 = MyGameController.PersonagensDeListas;

            foreach (var v in dccd2.Keys)
            {
                dccd[v] = dccd2[v];
            }

            return dccd;
        }
    }

    public static List<CustomizationContainerDates> Lccd
    {
        get {
            List<CustomizationContainerDates> lccd = new List<CustomizationContainerDates>();
            ToSaveCustomizationContainer.Instance.Load();
            List<CustomizationContainerDates> lccd2 = ToSaveCustomizationContainer.Instance.ccds;
            foreach (var v in lccd2)
                lccd.Add(v);

            lccd2 = MyGameController.PersonagensNaoListados;

            foreach (var v in lccd2)
                lccd.Add(v);

            return lccd;
        }
    }

    public static void SaveSumChars()
    {
        Dictionary<string, List<CustomizationContainerDates> > dccd = Dccd;
        ListToSaveCustomizationContainer.Instance.dccd.Clear();

        foreach (var v in dccd.Keys)
        {
            ListToSaveCustomizationContainer.Instance.dccd[v] = dccd[v];
        }

        ListToSaveCustomizationContainer.Instance.SaveLoaded();

        List<CustomizationContainerDates> lccd = Lccd;
        ToSaveCustomizationContainer.Instance.ccds.Clear();

        foreach (var v in lccd)
        {
            ToSaveCustomizationContainer.Instance.ccds.Add(v);
        }

        ToSaveCustomizationContainer.Instance.SaveLoaded();

    }

    public static void SalvarBlocoDeNotas()
    {
        byte[] b = FayvitBasicTools.BytesTransform.ToBytes(ToSaveCustomizationContainer.Instance.ccds);
        preJSON pre = new preJSON() { b = b };
        string caminho = "C:/Users/User/Desktop/lista.pers";
        System.IO.StreamWriter file = System.IO.File.CreateText(caminho);
        file.Write(UnityEngine.JsonUtility.ToJson(pre));
        file.Close();

        b = FayvitBasicTools.BytesTransform.ToBytes(ListToSaveCustomizationContainer.Instance.dccd);
        pre = new preJSON() { b = b };
        caminho = "C:/Users/User/Desktop/dicionario.pers";
        file = System.IO.File.CreateText(caminho);
        file.Write(UnityEngine.JsonUtility.ToJson(pre));
        UnityEngine.Debug.Log(pre.ToString());
        file.Close();
    }
}
