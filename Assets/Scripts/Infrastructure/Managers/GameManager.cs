using System;
using Domain.FileEntity;
using Domain.Services.IService;
using Items;
using Loxodon.Framework.Binding;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Services;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// 游戏管理
    /// </summary>
    public class GameManager:MonoBehaviour
    {
        private ApplicationContext _context;
        private IServiceContainer _container;

        private void Awake()
        {
            // 创建全局上下文
            _context = Context.GetApplicationContext();
            //获得上下文中的服务容器
            _container = _context.GetContainer();
        }



        /// <summary>
        /// 注册服务
        /// </summary>
        private void RegisterServices()
        {
            // 注册输入服务
            _container.Register<ResourceService>(new ResourceService(_container));
            _container.Register<InputService>(new InputService(_container));
            _container.Register<CharacterService>(new CharacterService(_container));
            _container.Register<MouseService>(new MouseService(_container));
            _container.Register<CombatUIService>(new CombatUIService(_container));
            _container.Register<PrefabService>(new PrefabService(_container));
            _container.Register<BaseDataService>(new BaseDataService(_container));
            _container.Register<GameUIService>(new GameUIService(_container));
        }
        private void Start()
        {
            RegisterServices();
            StartAllService();
        }

        private void StartAllService()
        {
            // 先加载数据、资源相关服务
            CombatUIService combatUIService = _container.Resolve<CombatUIService>();
            GameUIService gameUIService = _container.Resolve<GameUIService>();
            PrefabService prefabService = _container.Resolve<PrefabService>();
            ResourceService resourceService = _container.Resolve<ResourceService>();
            BaseDataService baseDataService = _container.Resolve<BaseDataService>();
            MouseService mouseService = _container.Resolve<MouseService>();
            InputService inputService = _container.Resolve<InputService>();
           
            CharacterService characterService = _container.Resolve<CharacterService>();
            
            combatUIService.Start();
            gameUIService.Start();
            prefabService.Start();
            resourceService.Start();
            baseDataService.Start();
            mouseService.Start();
            inputService.Start();
            
            // 加载角色
            characterService.Start();
        }
    }
}