using System;
using Domain.Services.IService;
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
            _container.Register<InputService>(new InputService());
            _container.Register<CharacterService>(new CharacterService());
            _container.Register<MouseService>(new MouseService());
            _container.Register<CombatUIService>(new CombatUIService());
            
        }
        private void Start()
        {
            RegisterServices();
            InputService inputService = _container.Resolve<InputService>();
            inputService.Start();
            CharacterService characterService = _container.Resolve<CharacterService>();
            characterService.Start();
            MouseService mouseService = _container.Resolve<MouseService>();
            mouseService.Start();
            CombatUIService combatUIService = _container.Resolve<CombatUIService>();
            combatUIService.Start();
        }
    }
}