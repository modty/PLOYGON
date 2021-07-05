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

        private GDChaPlayer _gdChaPlayer;

        private Transform _transform;
        private MovementState _movementState;

        private GDCharacter _target;
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
        private float normalAttack_Anim_timer;
        private float normalAttack_Anim_timer_max;

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
        public NormalAttackState(GDChaPlayer gdChaPlayer):base()
        {
            _gdChaPlayer = gdChaPlayer;
            _transform = _gdChaPlayer.Transform;
            _movementState = gdChaPlayer.MovementState;
            RegistInputActions();
            // 设置攻击前摇为0.7f，攻击后摇为0.7f，每次攻击需要1.4f，攻速度0.71，即每秒攻击0.71次，
            normalAttack_Anim_timer = .8f;
            normalAttack_Anim_timer_max = .8f;


            normalAttack_cooling = 0;
            normalAttack_ratio = 1;
            isNormalAttackCoolDown = true;
            _gdChaPlayer.NormalAttackAnimSpeed=1f;
            InitMessageObjs();
        }


        #region 消息通信

        private MInput _mInput;
        private MMouseTarget _mMouseTarget;
        private MMovement _mMovement;
        private MAnimNormalAttack _mAnimNormalAttack;
        #endregion

        private void InitMessageObjs()
        {
            _mInput = new MInput(this);
            _mMouseTarget = new MMouseTarget(this);
            _mMovement = new MMovement(this);
            _mAnimNormalAttack = new MAnimNormalAttack(this);
        }

        private float tempTimer;
        protected override void DoUpdate()
        {
            timer +=Time.time -timeTemp;
            timeTemp = Time.time;
            float distance = (_target.Transform.position - _transform.position).magnitude;
            // 如果角色正在移动，且进入攻击距离，停止
            if (distance < _gdChaPlayer.AttackRange)
            {
                if (_gdChaPlayer.IsMoving)
                {
                    
                    // 确保帧同步
                    _messenger.Publish(TypedInputActions.StopMove.ToString(),_mMovement);

                }
                switch (attackState)
                {
                    case 0:
                        StartAction();
                        break;
                    case 1:
                        //Debug.Log(Time.time-tempTimer);
                        NormalAttack(1);
                        //tempTimer = Time.time;
                        timer = 0;
                        attackState = 2;
                        break;
                    case 2:
                        if (timer - normalAttack_Anim_timer > -.005f)
                        {
                            _mAnimNormalAttack.Stop = true;
                            _messenger.Publish(TypedInputActions.AnimNormalAttackStop.ToString(),_mAnimNormalAttack);
                            timer = 0;
                            attackState = 3;
                            if (normalAttack_cooling <= .005f)
                            {
                                attackState = 1;
                                DoUpdate();
                            }
                        }
                        break;
                    case 3:
                        if (timer - normalAttack_cooling > -.005f)
                        {
                            timer = 0;
                            attackState = 1;
                            DoUpdate();
                        }
                        break;
                }
            }
            // 没有移动且角色离开攻击范围
            else if(!_gdChaPlayer.IsMoving&&distance > _gdChaPlayer.AttackRange)
            {
                _mMovement.TargetPosition = _target.Transform.position;
                _messenger.Publish(TypedInputActions.MoveTo.ToString(),_mMovement);
            }
            
          
        }

        /// <summary>
        /// 更新攻击速度，更新时必须保证触发攻击占整个普攻过程的比例不变，最大攻速设定为5，避免动画和实际数据的不匹配。
        /// </summary>
        /// <param name="at">每秒攻击次数</param>
        public void UpdateAttackSpeed()
        {
            float at = _gdChaPlayer.NormalAttackAnimSpeed;
            float frequency=1/at;
            // 需要加速，普攻冷却设置为0
            if (at > 1)
            {
                _gdChaPlayer.NormalAttackAnimSpeed = at;
                normalAttack_cooling = 0;
            }
            // 不需要加速
            else
            {
                // 动画不变
                _gdChaPlayer.NormalAttackAnimSpeed = 1;
                float frequency_static = frequency-normalAttack_Anim_timer_max;
                frequency = normalAttack_Anim_timer_max;
                normalAttack_cooling = frequency_static;
            }
            normalAttack_Anim_timer = frequency;
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

            if (action == (int) TypedWeapon.TwoHandBow)
            {
                SoundManager.Instance.PlayPullingStringBack();
            }
            right = !right;
            _mAnimNormalAttack.Stop = false;
            _mAnimNormalAttack.WeaponType = (int)_gdChaPlayer.WeaponType;
            _mAnimNormalAttack.Action = action;
            _messenger.Publish(TypedInputActions.AnimNormalAttack.ToString(),_mAnimNormalAttack);
        }

        #region 订阅引用

        private ISubscription<MMouseTarget> onMouse1Walkable;
        private ISubscription<MAttributeChange> onAttackSpeedChange;
        private ISubscription<MMouseTarget> onMouse1Target;
        private ISubscription<MMouseTarget> onMouse0Target;
        private ISubscription<MMouseTarget> onMouse0Walkable;
        private ISubscription<MInput> onForceAttack;
        private ISubscription<MInput> onNormalAttack;
        private ISubscription<MInput> onStopAttack;
        private ISubscription<MMovement> onStopMove;
        private ISubscription<MMovement> onMoveTo;

        #endregion
        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegistInputActions()
        {
            onMouse0Target=_messenger.Subscribe<MMouseTarget>(
                TypedInputActions.OnKeyDown_Mouse0_Target.ToString(),
                (message) =>
                {
                    GDCharacter gdCharacter = message.GdCharacter;

                    // 非强制攻击和点击自己直接退出
                    if (!_forceAttack || _gdChaPlayer.Uid.Equals(gdCharacter.Uid))
                    {
                        return;
                    }
                    if(_target!=null&&_target.Uid == gdCharacter.Uid) return;
                    switch (attackState)
                    {
                        case 0:
                        case 1:
                            attackState = 1;
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                    }

                    _inputAttack = true;
                    _target = gdCharacter;
                    timeTemp =Time.time;
                    CheckActionState();
                });
            onMouse1Target=_messenger.Subscribe<MMouseTarget>(
                TypedInputActions.OnKeyDown_Mouse1_Target.ToString(),
                (message) =>
                {
                    GDCharacter gdCharacter = message.GdCharacter;
                    // 点击自己直接退出
                    // 点击自己直接退出
                    if(_gdChaPlayer.Uid.Equals(gdCharacter.Uid)) return;
                    timer = 0;
                    isAttacked = false;
                    isAttackAnim = false;
                    StopAction();
                });

            onForceAttack=_messenger.Subscribe<MInput>(
                TypedInputActions.ForceAttack.ToString(),
                (message) =>
                {
                    _forceAttack = message.ForceAttack;
                    
                });
            onStopAttack=_messenger.Subscribe<MInput>(
                TypedInputActions.StopAttack.ToString(),
                (message) =>
                {
                    _target = null;
                    timer = 0;
                    isAttacked = false;
                    isAttackAnim = false;
                    StopAction();
                });
            onNormalAttack=_messenger.Subscribe<MInput>(
                TypedInputActions.NormalAttack.ToString(),
                (message) =>
                {
                    NormalAttack(message.MormalAttack);
                });
            
            // 点击地面，停止当前动作
            onMouse1Walkable=_messenger.Subscribe<MMouseTarget>(
                TypedInputActions.OnKeyDown_Mouse1_Walkable.ToString(),
                (message) =>
                {
                    StopAction();
                    _mAnimNormalAttack.Stop = true;
                    _messenger.Publish(TypedInputActions.AnimNormalAttackStop.ToString(),_mAnimNormalAttack);
                    _target = null;
                });
            onAttackSpeedChange = _messenger.Subscribe<MAttributeChange>(
                TypedAttribute.AttackSpeed.ToString(),
                (message) =>
                {
                    UpdateAttackSpeed();
                });
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