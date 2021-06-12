using Data;
using Loxodon.Framework.Messaging;
using UnityEngine;

namespace Domain.MessageEntities
{
    /// <summary>
    /// 鼠标处对象信息
    /// </summary>
    public class MouseTargetMessage:MessageBase
    {
        /// <summary>
        /// 鼠标位置
        /// </summary>
        private Vector3 _mousePosition;
        /// <summary>
        /// 鼠标处对象
        /// </summary>
        private GameData _gameData;

        public MouseTargetMessage(object sender,Vector3 mousePosition,GameData gameData) : base(sender)
        {
            _mousePosition = mousePosition;
            _gameData = gameData;
        }
        public MouseTargetMessage(object sender) : base(sender)
        {
        }
        public Vector3 MousePosition
        {
            get => _mousePosition;
            set => _mousePosition = value;
        }

        public GameData GameData
        {
            get => _gameData;
            set => _gameData = value;
        }
    }
}