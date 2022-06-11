using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FayvitBasicTools
{
    public class BytesTransform
    {

        public static byte[] ToBytes(object o)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(ms, o);

            return ms.ToArray();
        }

        public static T ToObject<T>(byte[] b)
        {
            MemoryStream ms = new MemoryStream(b);
            BinaryFormatter bf = new BinaryFormatter();
            return (T)bf.Deserialize(ms);
        }
    }
}