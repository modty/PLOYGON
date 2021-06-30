using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Scripts.Commons.Utils
{
    public class ItemUtil
    {
        public static int CalculateValue(int[] values)
        {
            if (values.Length == 2)
            {
                if (values[0] == values[1]) return values[0];
                return new Random(Guid.NewGuid().GetHashCode()).Next(values[0],values[1]);
            }
            return -1;
        }
        /// <summary>
        /// 物品基础ID
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static int GetItemId(long uid)
        {
            return (int) (uid% 1000);
        }

    
        /// <summary>
        /// 深度克隆一个对象
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
        /// <summary>
        /// 物品类别
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static int GetItemType(long uid)
        {
            return (int) ((uid / 1000000000000) % 1000);
        }
        /// <summary>
        /// 物品品质
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static int GetQuality(long uid)
        {
            return (int) ((uid / 1000000000) % 1000);
        }
        /// <summary>
        /// 物品用途
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static int GetUseType(long uid)
        {
        
            return (int) ((uid / 1000000) % 1000);
        }
    }
}