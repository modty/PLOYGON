using System.Collections.Generic;
using Data;
using Domain.FileEntity;
using Loxodon.Framework.Services;
using Scripts.Commons.Utils;
using UnityEngine;

namespace Domain.Services.IService
{
    public class BaseDataService:BaseService
    {
        private string UNKNOWN = "unknown";
        private string equipmentDir = "File/JSON/Items/Equipment.json";
        private string qualityColorDir = "File/JSON/Items/QualityColor.json";
        private string equipmentTypeDir = "Resources/Items/EquipmentType.json";
        private string spellDir = "Resources/Items/Spells.json";
        private string consumableDir = "File/JSON/Items/Consumable.json";
        private Dictionary<int, Quality> qualityColorDic; 
        private Dictionary<long,Consumable> consumableDic;
        private Dictionary<long,Equipment> equipmentDic;
        private ResourceService _resourceService;
        public BaseDataService(IServiceContainer container) : base(container)
        {
            _resourceService = container.Resolve<ResourceService>();
        }
        /// <summary>
        /// 加载物品品质颜色
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string GetItemQualityColor(long uid)
        {
            int qualityId = ItemUtil.GetQuality(uid);
            return qualityColorDic[qualityId].color;
        }

        /// <summary>
        /// 加载物品的显示资源（物品名）
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public string GetItemName(long uid)
        {
            string res = UNKNOWN;
            switch (ItemUtil.GetItemType(uid))
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    res = consumableDic[uid].icon;
                    break;
            }

            return res;
        }
        
        /// <summary>
        /// 比较两个uid是否唯一，所有比较都必须调用此方法，方便后续更改比较方法，或者通过其他方法比较等
        /// </summary>
        /// <param name="uid1"></param>
        /// <param name="uid2"></param>
        /// <returns></returns>
        public bool CompareUniqueId(long uid1,long uid2)
        {
            return uid1 == uid2;
        }

        public Sprite GetIcon(long uid)
        {
            string dir = UNKNOWN;
            switch (ItemUtil.GetItemType(uid))
            {
                case 0:
                    dir = new EquipmentInGame(equipmentDic[uid]).icon;
                    break;
                case 1:
                    break;
                case 2:
                    dir = new ConsumableInGame(consumableDic[uid]).icon;
                    break;
            }

            Sprite sprite = _resourceService.LoadSpriteByIO(dir);
            return sprite;
        }

        /// <summary>
        /// 获取物品的堆叠数量
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public int GetStackSize(long uid)
        {
            int stackSize = 0;
            switch (ItemUtil.GetItemType(uid))
            {
                case 0:
                    break;
            }
            return stackSize;
        }
        public FileBase GetItem(long uid,int type)
        {
            FileBase item = null;
            switch (type)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    item=consumableDic[uid];
                    break;
            }
            return item;
        }
        
        public FileBase GetItem(long uid)
        {
            int type = ItemUtil.GetItemType(uid);
            FileBase item = null;
            switch (type)
            {
                case 0:
                    item=new EquipmentInGame(equipmentDic[uid]);
                    break;
                case 1:
                    break;
                case 2:
                    item=new ConsumableInGame(consumableDic[uid]);
                    break;
            }
            return item;
        }

        public Quality GetQuality(long uid)
        {
            return qualityColorDic[ItemUtil.GetQuality(uid)];
        }
        protected override void OnStart(IServiceContainer container)
        {
            consumableDic = _resourceService.LoadJSON< Dictionary<long,Consumable>>(true,consumableDir);
            qualityColorDic = _resourceService.LoadJSON<Dictionary<int, Quality>>(true,qualityColorDir);
            equipmentDic = _resourceService.LoadJSON<Dictionary<long, Equipment>>(true, equipmentDir);
        }

        protected override void OnStop(IServiceContainer container)
        {
            
        }
    }
}