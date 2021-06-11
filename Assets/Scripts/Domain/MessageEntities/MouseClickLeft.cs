using System.Numerics;
using Data;
using Loxodon.Framework.Messaging;

namespace Domain.MessageEntities
{
    /// <summary>
    /// 鼠标左键点击信息
    /// </summary>
    public class MouseClickLeft:MessageBase
    {
        private Vector3 _position;
        private GameData _clickObject;
        public MouseClickLeft(object sender,Vector3 position,GameData clickObject) : base(sender)
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