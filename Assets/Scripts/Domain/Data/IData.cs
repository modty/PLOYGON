using Attribute.Items;
using Loxodon.Framework.Messaging;
using UnityEngine;

namespace Domain.Data
{
    [System.Serializable]
    public abstract class IData: ScriptableObject
    {
        /// <summary>
        /// 消息引用，只在本类中使用，因此为私有变量
        /// </summary>
        protected Messenger _messenger;
        
        protected long _uid;
        public long UId
        {
            get => _uid;
            set => _uid = value;
        }
        protected IData()
        {
            _messenger=Messenger.Default;
        }

        #region 物品使用逻辑
        /// <summary>
        /// 是否能够使用
        /// </summary>
        /// <returns></returns>
        protected abstract bool CanUse();
        public void Use()
        {
            if (CanUse())
            {
                DoUse();
            }
        }
        /// <summary>
        /// 处理使用逻辑
        /// </summary>
        protected abstract void DoUse();
        /// <summary>
        /// 判断iData是否可以用于自己。
        /// </summary>
        /// <param name="iData"></param>
        /// <returns></returns>
        public abstract bool CanReceiveUse(DataBase iData);
        

        #endregion
    }
}