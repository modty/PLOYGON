using System;
using System.Collections.Generic;
using Loxodon.Framework.Services;
using UnityEngine;

namespace Domain.Services.IService
{
    /// <summary>
    /// 阈值体服务，用于加载、缓存游戏中的预制体。
    ///     当预制体长期未使用时，采用淘汰算法进行回收
    /// </summary>
    public class PrefabService:BaseService
    {
        /// <summary>
        /// 用于加载预制体
        /// </summary>
        private ResourceService _resource;

        
        public PrefabService(IServiceContainer container) : base(container)
        {
            _resource = container.Resolve<ResourceService>();
        }

        public GameObject Get(String url)
        {
            /*GameObject gameObject = null;
            if (!cache.TryGetValue(url,out gameObject))
            {
                gameObject = _resource.LoadPrefabByResources(url);
                if (gameObject != null)
                {
                    cache[url] = gameObject;
                }
            }*/
            return _resource.LoadPrefabByResources(url);
        }
        protected override void OnStart(IServiceContainer container)
        {
        }


        protected override void OnStop(IServiceContainer container)
        {
        }
    }
}