using System;

namespace Domain.Data.FileData
{
    /// <summary>
    /// FileData基类。
    /// </summary>
    [Serializable]
    public abstract class FDBase
    {
        /// <summary>
        /// 物品类型ID
        /// 所有物品都应该有ID
        /// </summary>
        public long id;
        
        /// <summary>
        /// 物品名称
        /// 所有物品都应该有名称
        /// </summary>
        public string title;
        /// <summary>
        /// 物品简介
        /// 所有物品都应该有简介
        /// </summary>
        public string description;
        /// <summary>
        /// 物品图标。
        /// 为""代表没有图标
        /// </summary>
        public string icon;
        /// <summary>
        /// 物品价格。
        /// </summary>
        public int price;
        /// <summary>
        /// 物品最大堆叠数。
        /// 默认值为1。为0时代表物品不能放置在背包、仓库等容器中。
        /// 一般而言，消耗品都能够进行堆叠。部分特殊消耗品最大堆叠数为1
        /// </summary>
        public int maxStackSize;
        /// <summary>
        /// 物品容量（能够作为容器使用时生效）
        /// 为0时代表不能存储物品
        /// 注：消耗品也能具有
        /// </summary>
        public int capacity;
    }
}