using Data;
using Loxodon.Framework.Messaging;
using UnityEngine;

namespace Domain.MessageEntities
{
    /// <summary>
    /// 鼠标处对象信息
    /// </summary>
    public class MMouseTarget:MessageBase
    {
        /// <summary>
        /// 鼠标位置
        /// </summary>
        private Vector3 _mousePosition;
        /// <summary>
        /// 鼠标处对象
        /// </summary>
        private GDCharacter _gdCharacter;

        public MMouseTarget(object sender,Vector3 mousePosition,GDCharacter gdCharacter) : base(sender)
        {
            _mousePosition = mousePosition;
            _gdCharacter = gdCharacter;
        }
        public MMouseTarget(object sender) : base(sender)
        {
        }
        public Vector3 MousePosition
        {
            get => _mousePosition;
            set => _mousePosition = value;
        }

        public GDCharacter GdCharacter
        {
            get => _gdCharacter;
            set => _gdCharacter = value;
        }
    }
}