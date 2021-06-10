using ActionPool;
using Commons;
using UnityEngine;

namespace Data
{
    public class GameData
    {
        private long _uid;

        public long Uid
        {
            get => _uid;
            set => _uid = value;
        }

        private TypedInteract _typedInteract;
        public TypedInteract TypedInteract
        {
            get => _typedInteract;
            set => _typedInteract = value;
        }



        /// <summary>
        /// 角色是否能在上面移动
        /// </summary>
        private bool _canMoved;

        public bool CanMoved
        {
            get => _canMoved;
            set => _canMoved = value;
        }


        /// <summary>
        /// 角色Transform
        /// </summary>
        private Transform _transform;
        public Transform Transform
        {
            get => _transform;
            set => _transform = value;
        }
        private GameData _target;

        public GameData Target
        {
            get => _target;
            set
            {
                if (_target==null||!_target.Uid.Equals(Uid))
                {
                    _target = value;
                    EventCenter.Broadcast("UIElement:"+TypedUIElements.PlayerTarget,_target);
                }
            }
        }
    }
}