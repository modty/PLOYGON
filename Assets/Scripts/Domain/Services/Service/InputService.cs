using System;
using Commons;
using Commons.Constants;
using Domain.Contexts;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Messaging;
using Scripts;
using UnityEngine;

namespace Domain.Services.IService
{
    /// <summary>
    /// 用户输入服务
    /// </summary>
    public class InputService:BaseService
    {
        InputController _inputController;
        /// <summary>
        /// 初始化获取引用，使其不生效
        /// </summary>
        public InputService()
        {
            _inputController = InputController.Instance;
        }

        public new void Start()
        {
            _inputController.enabled = true;
        }

        public new void Stop()
        {
            _inputController.enabled = false;
        }

        /// <summary>
        /// 设置当前控制的角色
        /// </summary>
        /// <param name="uid">角色上下文键值</param>
        public void SetControlledCharacter(long uid)
        {
            // 获得玩家属性
            _inputController.PlayerContext = Context.GetContext<CharacterContext>("Character:"+uid);
            _messenger.Publish(TypedUIElements.PlayerMes.ToString(),_inputController.PlayerContext.GetContainer().Resolve<PlayerData>(Constants_Context.PlayerData));
        }
        
    }
}