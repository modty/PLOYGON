using System.Collections.Generic;
using ActionPool;
using Attribute.Items;
using Commons;
using Domain.Data.GameData;
using Domain.MessageEntities;
using Loxodon.Framework.Messaging;
using NUnit.Framework;
using Scripts;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace Data
{
    public abstract class GDCharacter:GDBase
    {
        protected Dictionary<TypedAttribute, ABaseAttribute> _attributes;
        public GDCharacter()
        {
            _mGameData = new MGameData(this,null);
            _animState = new MAnimNormalAttack(this);
            _mAttributeChange=new MAttributeChange(this);
            _attributes = new Dictionary<TypedAttribute, ABaseAttribute>();
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
        private GDCharacter _target;

        public GDCharacter Target
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
        public void AddAttribute(TypedAttribute typedAttribute,ABaseAttribute attribute)
        {
            attribute.GameData = this;
            _attributes.Add(typedAttribute,attribute);
        }

        public T GetAttribute<T>(TypedAttribute typedAttribute) where T : class
        {
            return _attributes[typedAttribute] as T;
        } 
    }
}