using System;
using System.Collections.Generic;
using Commons;
using Data;
using Domain.Data.GameData;
using States;
using UnityEngine;
using TypedMovement = Commons.TypedMovement;

namespace Scripts
{
    /// <summary>
    /// 角色的属性内，包含所有角色的组件、属性等，并且通过管理器进行管理。
    /// 此类对象不进行参数传递，使用Uid通过管理器进行访问。
    /// 参数格式以下划线加名称进行命名，下划线后采用驼峰式。
    /// 访问变量删除下划线并首字母大写，外部调用通常必须通过访问器进行调用（除非你能明确是否需要逻辑处理）
    /// </summary>
    public class GDChaPlayer:GDCharacter
    {
        
        #region Unity组件
        /// <summary>
        /// 角色RigidBody
        /// </summary>
        private Rigidbody _rigidbody;
        public Rigidbody Rigidbody
        {
            get => _rigidbody;
            set => _rigidbody = value;
        }
        

        /// <summary>
        /// 动画控制器
        /// </summary>
        private Animator _animator;
        public Animator Animator
        {
            get => _animator;
            set => _animator = value;
        }
        /// <summary>
        /// 角色碰撞器
        /// </summary>
        private CapsuleCollider _collider;
        public CapsuleCollider Collider
        {
            get => _collider;
            set => _collider = value;
        }

        private Transform _attackRangeUI;

        public Transform AttackRangeUI
        {
            get => _attackRangeUI;
            set => _attackRangeUI = value;
        }
        #endregion

        #region 移动相关属性
        /// <summary>
        /// 角色移动速度（目前不设置加速度）
        /// </summary>
        private float _moveSpeed;
        public float MoveSpeed
        {
            get => _moveSpeed;
            set => _moveSpeed = value;
        }

        private float _moveAcceleration;

        public float Height
        {
            get => _collider.height;
        }

        public Vector3 TargetCenter
        {
            get
            {
                var position = Transform.position;
                return new Vector3(position.x, Height/2, position.z);
            }
        }
        public float MoveAcceleration
        {
            get => _moveAcceleration;
            set => _moveAcceleration = value;
        }
        /// <summary>
        /// 角色旋转移动速度
        /// </summary>
        private float _rotateSpeed;
        public float RotateSpeed
        {
            get => _rotateSpeed;
            set => _rotateSpeed = value;
        }

        /// <summary>
        /// 角色移动的目标位置
        /// </summary>
        private Vector3 _targetMovePosition;
        public Vector3 TargetMovePosition
        {
            get => _targetMovePosition;
            set => _targetMovePosition = value;
        }



        /// <summary>
        /// 角色下一步的移动位置，用于寻路算法
        /// </summary>
        private Vector3 _nextMovePosition;
        public Vector3 NextMovePosition
        {
            get => _nextMovePosition;
            set => _nextMovePosition = value;
        }
        /// <summary>
        /// 移动类型
        /// </summary>
        private TypedMovement _typedMovement;

        public TypedMovement TypedMovement
        {
            get => _typedMovement;
            set => _typedMovement = value;
        }
        /// <summary>
        /// 鼠标输入的向量
        /// </summary>
        private Vector3 _inputVector;
        public Vector3 InputVector
        {
            get => _inputVector;
            set => _inputVector = value;
        }

        private bool _isMoving;

        public bool IsMoving
        {
            get => _isMoving;
            set => _isMoving = value;
        }
        private bool _isCrouch;

        public bool IsCrouch
        {
            get => _isCrouch;
            set => _isCrouch = value;
        }
        private bool _isSprinting;

        public bool IsSprinting
        {
            get => _isSprinting;
            set => _isSprinting = value;
        }

        private bool _isInjured;

        public bool IsInjured
        {
            get => _isInjured;
            set => _isInjured = value;
        }

        private bool _isCasting;

        public bool IsCasting
        {
            get => _isCasting;
            set => _isCasting = value;
        }
        #endregion

        #region 攻击属性

        /// <summary>
        /// 物理攻击
        /// </summary>
        private AAttackDamage _attackDamage;
        /// <summary>
        /// 法术攻击
        /// </summary>
        private AAbilityPower _abilityPower;

        /// <summary>
        /// 物理抗性（护甲）
        /// </summary>
        private AArmorResistance _armorResistance;
        /// <summary>
        /// 魔法抗性（法抗）
        /// </summary>
        private AMagicResistance _magicResistance;

        /// <summary>
        /// 暴击
        /// </summary>
        private ACriticalStrike _criticalStrike;

        /// <summary>
        /// 攻击速度
        /// </summary>
        private AAttackSpeed _attackSpeed;
        /// <summary>
        /// 特效
        /// </summary>
        private AONHitEffects _onHitEffects;

        /// <summary>
        /// 物理穿甲
        /// </summary>
        private AArmorPenetration _armorPenetration;

        /// <summary>
        /// 魔法穿透
        /// </summary>
        private AMagicPenetration _magicPenetration;

        /// <summary>
        /// 生命恢复（百分比）
        /// </summary>
        private AHealthRegeneration _healthRegeneration;
        /// <summary>
        /// 法术恢复（百分比）
        /// </summary>
        private AMagicRegeneration _magicRegeneration;

        /// <summary>
        /// 技能急速
        /// </summary>
        private AAbilityHaste _abilityHaste;

        /// <summary>
        /// 移动速度
        /// </summary>
        private AMovement _movement;

        /// <summary>
        /// 生命偷取
        /// </summary>
        private ALifeSteal _lifeSteal;
        /// <summary>
        /// 全能吸血
        /// </summary>
        private AOminivamp _ominivamp;

        /// <summary>
        /// 每5秒生命值恢复
        /// </summary>
        private AHealthRegon _healthRegon;

        /// <summary>
        /// 每5秒法力恢复
        /// </summary>
        private AResourceRegon _resourceRegon;

        /// <summary>
        /// 韧性
        /// </summary>
        private ATenacity _tenacity;


        private Transform _leftHand;
        private Transform _rightHand;

        public Transform LeftHand
        {
            get => _leftHand;
            set => _leftHand = value;
        }

        public Transform RightHand
        {
            get => _rightHand;
            set => _rightHand = value;
        }

       
        public AAbilityPower AbilityPower
        {
            get => _abilityPower;
            set => _abilityPower = value;
        }

        public AArmorResistance ArmorResistance
        {
            get => _armorResistance;
            set => _armorResistance = value;
        }

        public AMagicResistance MagicResistance
        {
            get => _magicResistance;
            set => _magicResistance = value;
        }

        public ACriticalStrike CriticalStrike
        {
            get => _criticalStrike;
            set => _criticalStrike = value;
        }
        public AONHitEffects ONHitEffects
        {
            get => _onHitEffects;
            set => _onHitEffects = value;
        }

        public AArmorPenetration ArmorPenetration
        {
            get => _armorPenetration;
            set => _armorPenetration = value;
        }

        public AMagicPenetration MagicPenetration
        {
            get => _magicPenetration;
            set => _magicPenetration = value;
        }

        public AHealthRegeneration HealthRegeneration
        {
            get => _healthRegeneration;
            set => _healthRegeneration = value;
        }

        public AMagicRegeneration MagicRegeneration
        {
            get => _magicRegeneration;
            set => _magicRegeneration = value;
        }

        public AAbilityHaste AbilityHaste
        {
            get => _abilityHaste;
            set => _abilityHaste = value;
        }

        public AMovement Movement
        {
            get => _movement;
            set => _movement = value;
        }

        public ALifeSteal LifeSteal
        {
            get => _lifeSteal;
            set => _lifeSteal = value;
        }

        public AOminivamp Ominivamp
        {
            get => _ominivamp;
            set => _ominivamp = value;
        }

        public AHealthRegon HealthRegon
        {
            get => _healthRegon;
            set => _healthRegon = value;
        }

        public AResourceRegon ResourceRegon
        {
            get => _resourceRegon;
            set => _resourceRegon = value;
        }
        
        public ATenacity Tenacity
        {
            get => _tenacity;
            set => _tenacity = value;
        }

        /// <summary>
        /// 攻击距离
        /// </summary>
        private float _attackRange;
        public float AttackRange
        {
            get => _attackRange;
            set => _attackRange = value;
        }

        private TypedWeapon _weaponType;

        public TypedWeapon WeaponType
        {
            get => _weaponType;
            set
            {
                if (!_weaponType.Equals(value))
                {
                    _weaponType = value;
                    _animState.WeaponType = (int)value;              
                    _messenger.Publish(Constants_Event.PlayerWeaponChange,_animState);
                }
            }
        }
        #endregion
        
        #region 动画控制相关

        private float _normalAttackAnimSpeed;

        public float NormalAttackAnimSpeed
        {
            get => _normalAttackAnimSpeed;
            set
            {

                if (Math.Abs(value - _normalAttackAnimSpeed) > .005f)
                {
                    _mAttributeChange.TypedAttribute = TypedAttribute.AttackSpeed;
                    _normalAttackAnimSpeed = value;
                    _messenger.Publish(TypedAttribute.AttackSpeed.ToString(),_mAttributeChange);
                }
            }
        }

        #endregion
        
        #region 相关逻辑状态
        
        private TypedCharacterState _state;
        public TypedCharacterState State
        {
            get => _state;
            set => _state = value;
        }
        private MovementState _movementState;

        public MovementState MovementState
        {
            get => _movementState;
            set => _movementState = value;
        }

        #endregion

        #region 角色物品（快捷栏、背包）
        /// <summary>
        /// 背包快捷栏装备情况。默认为容量为5，如果需要容量扩充，UI也需要相应改变。
        /// 如果数组中的值为null，则代表当前位置没有装备背包。
        /// </summary>
        private GDBase[] bagBarShortCutItems;

        public GDBase[] BagBarShortCutItems
        {
            get => bagBarShortCutItems;
            set => bagBarShortCutItems = value;
        }

        /// <summary>
        /// 当前打开的背包
        /// </summary>
        private int _bagOpenIndex;
        public int BagOpenIndex
        {
            get => _bagOpenIndex;
            set => _bagOpenIndex = value;
        }

        #endregion
        public GDChaPlayer(GameObject gameObject):base()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            Transform = gameObject.GetComponent<Transform>();
            _animator = gameObject.GetComponent<Animator>();
            _collider = gameObject.GetComponent<CapsuleCollider>();
            CanMoved = false;
            _attributes = new Dictionary<TypedAttribute, ABaseAttribute>();
        }

        public override bool CanUse()
        {
            return false;
        }

        public override bool Use(GDBase target)
        {
            return false;
        }

        public override bool CanReceiveUse(GDBase data)
        {
            return true;
        }
    }
}