using Data;
using Loxodon.Framework.Messaging;
using UnityEngine;

namespace Domain.MessageEntities
{
    /// <summary>
    /// 鼠标右键点击信息
    /// </summary>
    public class MouseClickRight:MessageBase
    {
        private Vector3 _position;
        private GameData _clickObject;
        public MouseClickRight(object sender,Vector3 position,GameData clickObject) : base(sender)
        {
            _position = position;
            _clickObject = clickObject;
        }

        public Vector3 Position
        {
            get => _position;
            set => _position = value;
        }

        public GameData ClickObject
        {
            get => _clickObject;
            set => _clickObject = value;
        }
    }
}