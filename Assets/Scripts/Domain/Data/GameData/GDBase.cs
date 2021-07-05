using System;
using Commons;
using Domain.Data.ActionData;
using Domain.Data.FileData;
using Domain.Services.IService;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Messaging;
using UnityEngine;

namespace Domain.Data.GameData
{
    /// <summary>
    /// 游戏运行后数据对象的基类。包含通用的对象引用
    /// </summary>
    public abstract class GDBase
    {
        #region 基础属性

        protected long _id;
        /// <summary>
        /// 物品ID，标志物品类型ID，如苹果、香蕉、蝙蝠等详细对象。
        /// </summary>
        public long ID
        {
            get => _id;
            set => _id = value;
        }
        protected string _belongTo;
        public string BelongTo
        {
            get => _belongTo;
            set => _belongTo = value;
        }
        private string _uid;
        /// <summary>
        /// 物品唯一ID，标志游戏运行后物品唯一ID。如：苹果1，苹果2，装备1，装备2等。
        /// </summary>
        public string Uid
        {
            get => _uid;
            set => _uid = value;
        }
        protected int _maxStackSize;
        protected int _price;
        protected int _capacity;

        protected int _stackCount;

        public int StackCount
        {
            get => _stackCount;
            set => _stackCount = value;
        }
        public int MAXStackSize
        {
            get => _maxStackSize;
            set => _maxStackSize = value;
        }

        public int Price
        {
            get => _price;
            set => _price = value;
        }

        public int Capacity
        {
            get => _capacity;
            set => _capacity = value;
        }


        private TypedInteract _typedInteract;
        public TypedInteract TypedInteract
        {
            get => _typedInteract;
            set => _typedInteract = value;
        }
        protected Sprite _icon;

        public Sprite Icon
        {
            get => _icon;
            set => _icon = value;
        }
        protected string _title;

        public string Title
        {
            get => _title;
            set => _title = value;
        }
        #endregion


        #region 外部引用
        /// <summary>
        /// 消息通信引用获取，只会在子类中使用。
        /// </summary>
        protected Messenger _messenger;

        /// <summary>
        /// FileData服务引用获取
        /// </summary>
        protected FDService _fdService;
        protected ResourceService _resourceService;

        #endregion

        
        #region 使用逻辑
        /// <summary>
        /// 是否能够使用
        /// </summary>
        /// <returns></returns>
        public abstract bool CanUse();

        /// <summary>
        /// 将当前物品作用到目标上
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public abstract bool Use(GDBase target);

        /// <summary>
        /// 判断iData是否可以用于自己。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract bool CanReceiveUse(GDBase data);
        #endregion
        
        /// <summary>
        /// 构造器只能被子类初始化
        /// </summary>
        protected GDBase()
        {
            _messenger=Messenger.Default;
            _fdService = Context.GetApplicationContext().GetContainer().Resolve<FDService>();
            _resourceService = Context.GetApplicationContext().GetContainer().Resolve<ResourceService>();
        }
    }
}