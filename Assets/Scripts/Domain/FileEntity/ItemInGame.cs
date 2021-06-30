using Domain.FileEntity;
using Domain.Services.IService;
using Loxodon.Framework.Contexts;
using Scripts.Commons.Utils;
using UnityEngine;

namespace Items
{
    /// <summary>
    /// 游戏运行后的物品数据
    /// </summary>
    public class ItemInGame
    {
        private ResourceService _resourceService;
        private FileBase _item;
        /// <summary>
        /// 物品所绑定的信息（如：名称、描述、图标等）。
        /// 通过深度复制FileEntity获得
        /// </summary>
        public FileBase Item
        {
            get => _item;
            set => _item = value;
        }

        /// <summary>
        /// 物品中包含的物品，如背包、礼包、仓库、宝箱等。如果其中某个下标处为null，则代表不包含物品。
        /// </summary>
        private ItemInGame[] _containItems;

        public ItemInGame[] ContainItems
        {
            get => _containItems;
            set => _containItems = value;
        }

        /// <summary>
        /// 物品图标，加载一次后保存，避免重复IO
        /// </summary>
        private Sprite _icon;
        public Sprite Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = _resourceService.LoadSpriteByIO(_item.icon);
                }
                return _icon;
            }
            set => _icon = value;
        }

        private int _stackCount;
        /// <summary>
        /// 堆叠数量，如果在背包中，需要显示数量的时候调用
        /// </summary>
        public int StackCount
        {
            get => _stackCount;
            set => _stackCount = value;
        }
        /// <summary>
        /// 物品容量，仓库、背包、礼包等具有容量的物品，表示物品中最大可存放的物品数。
        /// </summary>
        public int Capacity
        {
            get { return Item.capacity; }
            set
            {
                _containItems=new ItemInGame[value];
                Item.capacity = value;
            }
        }

        public int MaxStackCount
        {
            set => _item.maxStackSize = value;
            get => _item.maxStackSize;
        }
        public string Name
        {
            get
            {
                return _item.name_cn;
            }
        }
        public string TypeName
        {
            get
            {
                return _item.name_cn;
            }
        }
        public int Price
        {
            get
            {
                return _item.price;
            }
        }

        public long Uid
        {
            get { return _item.uid; }
        }
        /// <summary>
        /// 返回特殊物品的描述
        /// </summary>
        /// <returns></returns>
        public virtual string GetDescription()
        {
            return string.Format("<color={0}>{1}</color>", "#00ff00ff", Name);
        }

        public ItemInGame()
        {
            _containItems = new ItemInGame[10];
        }

        public ItemInGame(long belongId,FileBase item)
        {
            _resourceService = Context.GetApplicationContext().GetContainer().Resolve<ResourceService>();
            // 深度克隆对象
            this.Item = ItemUtil.Clone(item);
            // 所属的物品（上一级物品。如：背包中的物品上一级为背包）
            this.Item.BelongTo = belongId;
            // 加载图标
            this.Icon = _resourceService.LoadSpriteByIO(item.icon);
            // 根据容量，初始化
            _containItems=new ItemInGame[item.capacity];
            // 初始化数量为1
            StackCount = 1;
        }

        public bool Use()
        {
            return true;
        }
    }
}

