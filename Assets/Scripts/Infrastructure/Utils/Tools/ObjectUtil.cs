using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Tools
{
    public class ObjectUtil
    {
        /// <summary>
        /// 采用序列化的方式，深度克隆一个对象
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public  static  T  Clone<T>(T  obj)
        {
            Stream objectStream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(objectStream, obj);
            objectStream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(objectStream);
        }
    }
}