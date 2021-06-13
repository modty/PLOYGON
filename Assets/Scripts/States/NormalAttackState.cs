using System;
using ActionPool;
using Commons;
using Data;
using Domain.MessageEntities;
using Loxodon.Framework.Messaging;
using Scripts;
using UnityEngine;

namespace States
{
    /// <summary>
    /// 普通攻击
    /// </summary>
    public class NormalAttackState:BaseState
    {
        // 强制攻击指令
        private bool _forceAttack;
        // 攻击输入
        private bool _inputAttack;

        private PlayerAttribute _player;

        private Transform _transform;
        private MovementState _movementState;

        private GameData _target;
        /// <summary>
        /// 攻击计时，不使用Time.fixedDeltaTime
        /// </summary>
        private float timer;
        
        /// <summary>
        /// 用户验证每次攻击间隔
        /// </summary>
        private float duration;
        /// <summary>
        /// 是否进入攻击动画
        /// </summary>
        private bool isAttackAnim;
        
        
        /// <summary>
        /// 是否攻击
        /// </summary>
        private bool isAttacked;

        /// <summary>
        /// 动画前摇
        /// </summary>
        private float normalAttack_Anim_preTimer_base;
        private float normalAttack_Anim_preTimer;
        /// <summary>
        /// 动画后摇
        /// </summary>
        private float normalAttack_Anim_aftTimer_base;
        private float normalAttack_Anim_aftTimer;
        /// <summary>
        /// 攻击冷却
        /// </summary>
        private float normalAttack_cooling;

        private float normalAttack_ratio;

        private float timeTemp;
        private bool isNewEnter;
        private bool isNormalAttackCoolDown;
        /// <summary>
        /// 标记当前攻击状态
        ///     0：停止
        ///     1：调用攻击动画，并计时
        ///     2：计时完毕，事实攻击，并计时
        ///     3：动画播放完毕，计时，等待冷却
        ///     4：冷却完毕，设置状态为0
        /// </summary>
        private int attackState;
        public NormalAttackState(PlayerAttribute playerAttribute):base()
        {
            _player = playerAttribute;
            _transform = _player.Transform;
            _movementState = playerAttribute.MovementState;
            RegistInputActions();
            // 设置攻击前摇为0.7f，攻击后摇为0.7f，每次攻击需要1.4f，攻速度0.71，即每秒攻击0.71次，
            normalAttack_Anim_aftTimer_base = 0.58f;
            normalAttack_Anim_aftTimer_base = normalAttack_Anim_aftTimer;

            normalAttack_Anim_preTimer_base = 0.42f;
            normalAttack_Anim_preTimer = normalAttack_Anim_preTimer_base;

            normalAttack_cooling = 0;
            normalAttack_ratio = normalAttack_Anim_preTimer;
            isNormalAttackCoolDown = true;
            _player.AttackSpeed.UpdateCurrentValue(2f);
            InitMessageObjs();

        }


        #region 消息通信

        private InputMessage _inputMessage;
        private MouseTargetMessage _mouseTargetMessage;
        private MovementMessage _movementMessage;
        private AnimNormalAttack _animNormalAttack;
        #endregion

        private void InitMessageObjs()
        {
            _inputMessage = new InputMessage(this);
            _mouseTargetMessage = new MouseTargetMessage(this);
            _movementMessage = new MovementMessage(this);
            _animNormalAttack = new AnimNormalAttack(this);
        }

        protected override void DoUpdate()
        {
            timer +=Time.time -timeTemp;
            timeTemp = Time.time;
            float distance = (_target.Transform.position - _transform.position).magnitude;
            // 如果角色正在移动，且进入攻击距离，停止
            if (distance < _player.AttackRange)
            {
                if (_player.IsMoving)
                {
                    // 确保帧同步
                    _messenger.Publish(TypedInputActions.StopMove.ToString(),_movementMessage);
                }
                switch (attackState)
                {
                    case 0:
                        StartAction();
                        break;
                    case 1:
                        NormalAttack(1);
                        timer = 0;
                        attackState = 2;
                        break;
                    case 2:
                        if (timer - normalAttack_Anim_preTimer > -.005f)
                        {
                            if (_player.Target != null)
                            {
                                PlayerAttribute target=_player.Target as PlayerAttribute;
                                target?.Health.UpdateCurrentValue(-(int)_player.AttackDamage.CurrentValue());
                            }
                            timer = 0;
                            attackState = 3;
                        }
                        break;
                    case 3:
                        if (timer - normalAttack_Anim_aftTimer > -.005f)
                        {
                            timer = 0;
                            attackState = 4;
                            if (normalAttack_cooling < .005f)
                            {
                                attackState = 1;
                            }
                        }
                        break;
                    case 4:
                        if (timer - normalAttack_cooling > -.005f)
                        {
                            timer = 0;
                            attackState = 1;
                        }
                        break;
                }
            }
            // 没有移动且角色离开攻击范围
            else if(!_player.IsMoving&&distance > _player.AttackRange)
            {
                _movementMessage.TargetPosition = _target.Transform.position;
                _messenger.Publish(TypedInputActions.MoveTo.ToString(),_movementMessage);
            }
            
          
        }

        /// <summary>
        /// 更新攻击速度，更新时必须保证触发攻击占整个普攻过程的比例不变，最大攻速设定为5，避免动画和实际数据的不匹配。
        /// </summary>
        /// <param name="at">每秒攻击次数</param>
        public void UpdateAttackSpeed(float at)
        {
            float frequency=1/at;
            // 需要加速，普攻冷却设置为0
            if (at > 1)
            {
                _player.NormalAttackAnimSpeed = at;
                normalAttack_cooling = 0;
            }
            // 不需要加速
            else
            {
                // 动画不变
                _player.NormalAttackAnimSpeed = 1;
                float frequency_static = frequency-1;
                frequency = 1;
                normalAttack_cooling = frequency_static;
            }

            normalAttack_Anim_preTimer = frequency * normalAttack_ratio;
            normalAttack_Anim_aftTimer = frequency-normalAttack_Anim_preTimer;
        }

        private bool right;
        private void NormalAttack(int action)
        {
            // 确保面向目标
            if (_target != null)
            {
                _transform.rotation = Quaternion.LookRotation(_target.Transform.position-_transform.position);
            }
            if(right)
            {
                action = 5;
            }
            right = !right;
            _animNormalAttack.WeaponType = (int)_player.WeaponType;
            _animNormalAttack.Action = action;
            _messenger.Publish(TypedInputActions.AnimNormalAttack.ToString(),_animNormalAttack);
        }

        #region 订阅引用

        private ISubscription<MouseTargetMessage> onMouse1Walkable;
        private ISubscription<MouseTargetMessage> onMouse1Target;
        private ISubscription<MouseTargetMessage> onMouse0Target;
        private ISubscription<MouseTargetMessage> onMouse0Walkable;
        private ISubscription<InputMessage> onForceAttack;
        private ISubscription<InputMessage> onNormalAttack;
        private ISubscription<InputMessage> onStopAttack;
        private ISubscription<MovementMessage> onStopMove;
        private ISubscription<MovementMessage> onMoveTo;

        #endregion
        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegistInputActions()
        {
            onMouse0Target=_messenger.Subscribe<MouseTargetMessage>(
                TypedInputActions.OnKeyDown_Mouse0_Target.ToString(),
                (message) =>
                {
                    GameData gameData = message.GameData;

                    // 非强制攻击和点击自己直接退出
                    if (!_forceAttack || _player.Uid.Equals(gameData.Uid))
                    {
                        return;
                    }
                    if(_target!=null&&_target.Uid == gameData.Uid) return;
                    switch (attackState)
                    {
                        case 0:
                        case 1:
                        case 2:
                            attackState = 1;
                            break;
                        case 3:
                            break;
                        case 4:
                            break;
                    }

                    _inputAttack = true;
                    _target = gameData;
                    timeTemp =Time.time;
                    CheckActionState();
                });
            onMouse1Target=_messenger.Subscribe<MouseTargetMessage>(
                TypedInputActions.OnKeyDown_Mouse1_Target.ToString(),
                (message) =>
                {
                    GameData gameData = message.GameData;
                    // 点击自己直接退出
                    // 点击自己直接退出
                    if(_player.Uid.Equals(gameData.Uid)) return;
                    timer = 0;
                    isAttacked = false;
                    isAttackAnim = false;
                    StopAction();
                });

            onForceAttack=_messenger.Subscribe<InputMessage>(
                TypedInputActions.ForceAttack.ToString(),
                (message) =>
                {
                    _forceAttack = message.ForceAttack;
                    
                });
            onStopAttack=_messenger.Subscribe<InputMessage>(
                TypedInputActions.StopAttack.ToString(),
                (message) =>
                {
                    _target = null;
                    timer = 0;
                    isAttacked = false;
                    isAttackAnim = false;
                    StopAction();
                });
            onNormalAttack=_messenger.Subscribe<InputMessage>(
                TypedInputActions.NormalAttack.ToString(),
                (message) =>
                {
                    NormalAttack(message.MormalAttack);
                });
            
            // 点击地面，停止当前动作
            onMouse1Walkable=_messenger.Subscribe<MouseTargetMessage>(
                TypedInputActions.OnKeyDown_Mouse1_Walkable.ToString(),
                (message) =>
                {
                    StopAction();
                    _target = null;
                });
            
            EventCenter.AddListener<float>(Constants_Event.AttributeChange+":"+_player.Uid+":"+TypedAttribute.AttackSpeed,UpdateAttackSpeed);
        }



        private void CheckActionState()
        {
            if (_inputAttack && _forceAttack&&stop)
            {
                // 准备攻击
                StartAction();
            }
        }
    }
}