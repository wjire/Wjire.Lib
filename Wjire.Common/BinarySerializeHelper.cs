using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Wjire.Common
{
    /// <summary>
    /// 二进制序列化helper
    /// </summary>
    public static class BinarySerializeHelper
    {

        /// <summary>
        /// 二进制序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] SerializeToBytes(object obj)
        {
            return SerializeToMemoryStream(obj).ToArray();
        }


        /// <summary>
        /// 二进制序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static MemoryStream SerializeToMemoryStream(object obj)
        {
            MemoryStream memory = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memory, obj);
            return memory;
        }


        /// <summary>
        /// 二进制反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memory"></param>
        /// <returns></returns>
        public static T DeserializeFrom<T>(Stream memory)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (T)formatter.Deserialize(memory);
        }
    }
}
