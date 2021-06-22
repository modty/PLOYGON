using ActionPool;
using Commons;
using Domain.MessageEntities;
using Loxodon.Framework.Messaging;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace Data
{
    public class GameData
    {
        private long _uid;

        public GameData()
        {
            _mGameData = new MGameData(this,null);
            _animState = new MAnimNormalAttack(this);
            _mAttributeChange=new MAttributeChange(this);
        }

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
        protected Messenger _messenger=Messenger.Default;
        #region 监听

        private MGameData _mGameData;
        protected MAttributeChange _mAttributeChange;
        protected MAnimNormalAttack _animState;

        #endregion
        private GameData _target;

        public GameData Target
        {
            get => _target;
            set
            {
                // 设置目标不能重复设置、不能以自己为目标
                if (_target==null||_target.Uid!=value.Uid)
                {
                    _mGameData.GameData = value;
                    _target = value;
                    _messenger.Publish(TypedUIElements.PlayerTarget.ToString(),_mGameData);
                }
            }
        }
    }
}