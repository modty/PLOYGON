using System;

namespace Domain.FileEntity
{
    /// <summary>
    /// 所有物品父类
    /// </summary>
    [Serializable]
    public abstract class FileBase
    {
        public long uid;
        public string name_cn;
        public string name_us;
        public string description_cn;
        public string icon;
        public int maxStackSize;
        public int price;
        public int capacity;
        private long _belongTo;
        public long BelongTo
        {
            get => _belongTo;
            set => _belongTo = value;
        }
        protected FileBase()
        {
        }
    }
}