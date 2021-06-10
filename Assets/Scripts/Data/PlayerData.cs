using Commons;
using Data;
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
    public class PlayerAttribute:GameData
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
        private AttackDamage _attackDamage;
        /// <summary>
        /// 法术攻击
        /// </summary>
        private AbilityPower _abilityPower;

        /// <summary>
        /// 物理抗性（护甲）
        /// </summary>
        private ArmorResistance _armorResistance;
        /// <summary>
        /// 魔法抗性（法抗）
        /// </summary>
        private MagicResistance _magicResistance;

        /// <summary>
        /// 暴击
        /// </summary>
        private CriticalStrike _criticalStrike;

        /// <summary>
        /// 攻击速度
        /// </summary>
        private AttackSpeed _attackSpeed;
        /// <summary>
        /// 特效
        /// </summary>
        private ONHitEffects _onHitEffects;

        /// <summary>
        /// 物理穿甲
        /// </summary>
        private ArmorPenetration _armorPenetration;

        /// <summary>
        /// 魔法穿透
        /// </summary>
        private MagicPenetration _magicPenetration;

        /// <summary>
        /// 生命恢复（百分比）
        /// </summary>
        private HealthRegeneration _healthRegeneration;
        /// <summary>
        /// 法术恢复（百分比）
        /// </summary>
        private MagicRegeneration _magicRegeneration;

        /// <summary>
        /// 技能急速
        /// </summary>
        private AbilityHaste _abilityHaste;

        /// <summary>
        /// 移动速度
        /// </summary>
        private Movement _movement;

        /// <summary>
        /// 生命偷取
        /// </summary>
        private LifeSteal _lifeSteal;
        /// <summary>
        /// 全能吸血
        /// </summary>
        private Ominivamp _ominivamp;

        /// <summary>
        /// 每5秒生命值恢复
        /// </summary>
        private HealthRegon _healthRegon;

        /// <summary>
        /// 每5秒法力恢复
        /// </summary>
        private ResourceRegon _resourceRegon;

        /// <summary>
        /// 韧性
        /// </summary>
        private Tenacity _tenacity;

        private Health _health;

        public Health Health
        {
            get => _health;
            set
            {
                _health = value;
                _health.GameData = this;
            }
        }

        public AttackDamage AttackDamage
        {
            get => _attackDamage;
            set
            {
                _attackDamage = value;
                _attackDamage.GameData = this;
            }
        }

        public AbilityPower AbilityPower
        {
            get => _abilityPower;
            set => _abilityPower = value;
        }

        public ArmorResistance ArmorResistance
        {
            get => _armorResistance;
            set => _armorResistance = value;
        }

        public MagicResistance MagicResistance
        {
            get => _magicResistance;
            set => _magicResistance = value;
        }

        public CriticalStrike CriticalStrike
        {
            get => _criticalStrike;
            set => _criticalStrike = value;
        }

        public AttackSpeed AttackSpeed
        {
            get => _attackSpeed;
            set => _attackSpeed = value;
        }

        public ONHitEffects ONHitEffects
        {
            get => _onHitEffects;
            set => _onHitEffects = value;
        }

        public ArmorPenetration ArmorPenetration
        {
            get => _armorPenetration;
            set => _armorPenetration = value;
        }

        public MagicPenetration MagicPenetration
        {
            get => _magicPenetration;
            set => _magicPenetration = value;
        }

        public HealthRegeneration HealthRegeneration
        {
            get => _healthRegeneration;
            set => _healthRegeneration = value;
        }

        public MagicRegeneration MagicRegeneration
        {
            get => _magicRegeneration;
            set => _magicRegeneration = value;
        }

        public AbilityHaste AbilityHaste
        {
            get => _abilityHaste;
            set => _abilityHaste = value;
        }

        public Movement Movement
        {
            get => _movement;
            set => _movement = value;
        }

        public LifeSteal LifeSteal
        {
            get => _lifeSteal;
            set => _lifeSteal = value;
        }

        public Ominivamp Ominivamp
        {
            get => _ominivamp;
            set => _ominivamp = value;
        }

        public HealthRegon HealthRegon
        {
            get => _healthRegon;
            set => _healthRegon = value;
        }

        public ResourceRegon ResourceRegon
        {
            get => _resourceRegon;
            set => _resourceRegon = value;
        }
        
        public Tenacity Tenacity
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
            set => _weaponType = value;
        }
        #endregion
        
        private TypedCharacterState _state;
        public TypedCharacterState State
        {
            get => _state;
            set => _state = value;
        }


        #region 动画控制相关

        private float _normalAttackAnimSpeed;

        public float NormalAttackAnimSpeed
        {
            get => _normalAttackAnimSpeed;
            set => _normalAttackAnimSpeed = value;
        }

        #endregion
        #region Actions

        private MovementState _movementState;

        public MovementState MovementState
        {
            get => _movementState;
            set => _movementState = value;
        }

        #endregion
        public PlayerAttribute(GameObject gameObject):base()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            Transform = gameObject.GetComponent<Transform>();
            _animator = gameObject.GetComponent<Animator>();
            _collider = gameObject.GetComponent<CapsuleCollider>();
            CanMoved = false;
        }

    }
}