using System;
using Commons;
using Commons.Constants;
using Domain.Contexts;
using Domain.MessageEntities;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Messaging;
using Loxodon.Framework.Services;
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

        #region 订阅引用

        private ISubscription<MGameData> OnControlledCharacter_Change;

        #endregion
        /// <summary>
        /// 初始化获取引用，使其不生效
        /// </summary>
        public InputService(IServiceContainer container):base(container)
        {
            _inputController = InputController.Instance;
            RegistSubscibes();
        }

        private void RegistSubscibes()
        {
            OnControlledCharacter_Change=_messenger.Subscribe<MGameData>(Constants_Event.ControlledCharacter, (gameData) =>
            {
                SetControlledCharacter(gameData.GameData as GDChaPlayer);
            });
        }
        protected override void OnStart(IServiceContainer container)
        {
            _inputController.enabled = true;

        }

        protected override void OnStop(IServiceContainer container)
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
            _messenger.Publish(TypedUIElements.PlayerMes.ToString(),_inputController.PlayerContext.GetContainer().Resolve<GDChaPlayer>(Constants_Context.PlayerData));
        }

        private void SetControlledCharacter(GDChaPlayer gdChaPlayer)
        {
            _inputController.GdChaPlayer = gdChaPlayer;
        }
    }
}