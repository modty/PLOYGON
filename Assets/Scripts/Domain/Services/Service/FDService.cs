using System.Collections.Generic;
using Domain.Data.FileData;
using Loxodon.Framework.Services;
using Scripts.Commons.Utils;
using UnityEngine;

namespace Domain.Services.IService
{
    public class FDService:BaseService
    {
        private string consumblePotionDir = "File/Data/Consumable/Potion.json";
        private string qpuipmentBackpackDir = "File/Data/Equipment/Backpack.json";
        /// <summary>
        /// 消耗品字典
        /// </summary>
        private Dictionary<long,FDConPotion> consumblePotionDic;
        private Dictionary<long,FDEquBackpack> qpuipmentBackpackDic;
        
        /// <summary>
        /// 资源加载服务引用
        /// </summary>
        private ResourceService _resourceService;
        
        public FDService(IServiceContainer container) : base(container)
        {
            _resourceService = container.Resolve<ResourceService>();
        }

        /// <summary>
        /// 服务启动后加载数据
        /// </summary>
        /// <param name="container"></param>
        protected override void OnStart(IServiceContainer container)
        {
            consumblePotionDic = _resourceService.LoadJSON< Dictionary<long,FDConPotion>>(true,consumblePotionDir);
            qpuipmentBackpackDic = _resourceService.LoadJSON< Dictionary<long,FDEquBackpack>>(true,qpuipmentBackpackDir);
        }

        protected override void OnStop(IServiceContainer container)
        {
            
        }

        /// <summary>
        /// 获取FileData数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FDBase GetFD(long id)
        {
            int type = ItemUtil.GetItemType(id);
            FDBase item = null;
            switch (type)
            {
                case 0:
                    item=qpuipmentBackpackDic[id];
                    break;
                case 1:
                    break;
                case 2:
                    item=consumblePotionDic[id];
                    break;
            }
            return item;
        }
    }
}