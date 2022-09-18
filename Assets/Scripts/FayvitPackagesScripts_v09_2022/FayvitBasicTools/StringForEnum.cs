using UnityEngine;
using System.Collections;

namespace FayvitBasicTools
{
    public class StringForEnum
    {
        public static T GetEnum<T>(string s, T original)
        {
            T chave = original;
            if (s != "")
            {
                try
                {
                    chave = (T)System.Enum.Parse(typeof(T), s);
                }
                catch (System.ArgumentException e)
                {
                    Debug.LogError("string para enum: " + typeof(T) + " invalida no enum \n" + e.StackTrace);
                }
            }

            return chave;
        }

        public static T GetEnum<T>(string s)
        {
            T chave = default;
            if (!string.IsNullOrEmpty(s))
            {
                try
                {
                    chave = (T)System.Enum.Parse(typeof(T), s);
                }
                catch (System.ArgumentException e)
                {
                    Debug.LogError("string para enum: " + typeof(T) +" "+ s + " invalida no enum \n" + e.StackTrace);
                }
            }

            return chave;
        }
    }
}