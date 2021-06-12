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
        /// 攻击前摇
        /// </summary>
        private float normalAttack_preTimer;

        /// <summary>
        /// 动画前摇
        /// </summary>
        private float normalAttack_Anim_preTimer;
        /// <summary>
        /// 动画后摇
        /// </summary>
        private float normalAttack_Anim_aftTimer;
        /// <summary>
        /// 攻击后摇
        /// </summary>
        private float normalAttack_aftTimer;

        private float normalAttack_ratio;
        /// <summary>
        /// 攻击后摇具有上限，初始化时的值
        /// </summary>
        private float normalAttack_aftTimer_max;
        private float timeTemp;
        
        public NormalAttackState(PlayerAttribute playerAttribute):base()
        {
            _player = playerAttribute;
            _transform = _player.Transform;
            _movementState = playerAttribute.MovementState;
            RegistInputActions();
            
            // 设置攻击前摇为0.7f，攻击后摇为0.7f，每次攻击需要1.4f，攻速度0.71，即每秒攻击0.71次，
            normalAttack_preTimer = 0f;
            normalAttack_aftTimer = 0f;
            normalAttack_Anim_aftTimer = 0.58f;
            normalAttack_Anim_preTimer = 0.42f;
            normalAttack_aftTimer_max = .42f;
            normalAttack_ratio = normalAttack_Anim_preTimer;
            _player.AttackSpeed.UpdateCurrentValue(1.5f);
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
            if(_target==null) return;
            float currentTime = Time.time;
            timer +=currentTime -timeTemp;
            timeTemp=currentTime;
            float distance = (_target.Transform.position - _transform.position).magnitude;
            // 如果角色正在移动，且进入攻击距离，停止
            if (distance < _player.AttackRange)
            {
                if (timer < -5)
                {
                    timer = 0;
                }
                _messenger.Publish(TypedInputActions.StopMove.ToString(),_movementMessage);
                // 进入攻击动画
                if (isAttackAnim)
                {
                    // 进行攻击（为了匹配动画，使用动画的事件进行处理）
                    if (isAttacked)
                    {
                        // 动画播放结束
                        if (normalAttack_Anim_aftTimer-timer <.005f)
                        {
                            //Debug.Log("3：动画后摇结束，等待静态后摇："+timer);
                            isAttackAnim = false;
                            timer = 0;
                            // 没有攻击静态攻击后摇，直接退出，不进入下一帧计算，需要补充时间
                            if (normalAttack_aftTimer < .005f)
                            {
                                //Debug.Log("4：没有静态后摇，直接等待静态前摇："+timer);
                                isAttacked = false;
                                DoUpdate();
                            }
                        }
                    }
                    // 没有攻击
                    else
                    {
                        if (normalAttack_Anim_preTimer-timer <.005f)
                        {
                            //Debug.Log("2：动画前摇结束，进行攻击："+timer);
                            //Debug.Log("攻击：间隔："+(Time.time - duration)+"|||||"+normalAttack_preTimer+"："+normalAttack_Anim_preTimer+"："+normalAttack_Anim_aftTimer+"："+normalAttack_aftTimer);
                            if (_player.Target != null)
                            {
                                PlayerAttribute target=_player.Target as PlayerAttribute;
                                target?.Health.UpdateCurrentValue(-(int)_player.AttackDamage.CurrentValue());
                            }
                            duration = currentTime;
                            // 攻击过了
                            isAttacked = true;
                            timer = 0;
                        }
                    }
                   
                }
                else
                {
                    // 攻击后摇
                    if (isAttacked)
                    {
                        // 静态攻击后摇过去，可以进行攻击（注：攻击结束不用nextTimer，不然会多一个Time.fixDetalTime的时间）
                        if (normalAttack_aftTimer-timer <.005f)
                        {
                            //Debug.Log("4：动画前摇结束，攻击完毕："+timer);
                            isAttacked = false;
                            isAttackAnim = false;
                            timer = 0;
                        }
                       
                    }
                    // 攻击前摇
                    else
                    {
                        // 如果没有静态攻击前摇，不进行时间重置
                        if (normalAttack_preTimer <=.005f)
                        {
                            //Debug.Log("1:没有静态前摇，直接等待动画前摇："+timer);
                            NormalAttack(1);
                            isAttackAnim = true;
                            DoUpdate();
                        }
                        else if (normalAttack_preTimer-timer <.005f)
                        {
                            //Debug.Log("1：静态前摇结束，等待动画前摇："+timer);
                            // 调用动画
                            NormalAttack(1);
                            // 标记进入动画
                            isAttackAnim = true;
                            timer = 0;
                        }
                    }
                    
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
            // 需要加速，静态前后摇均设置为0
            if (at > 1)
            {
                _player.NormalAttackAnimSpeed = at;
                normalAttack_preTimer = 0;
                normalAttack_aftTimer = 0;
            }
            // 不需要加速
            else
            {
                // 动画不变
                _player.NormalAttackAnimSpeed = 1;
                float frequency_static = frequency-1;
                frequency = 1;
                if (normalAttack_aftTimer < normalAttack_aftTimer_max)
                {
                    float hit = normalAttack_aftTimer_max - normalAttack_aftTimer;
                    if (hit < frequency_static)
                    {
                        normalAttack_aftTimer = normalAttack_aftTimer_max;
                        frequency_static -= hit;
                        if (frequency_static > 0)
                        {
                            normalAttack_preTimer += frequency_static;
                        }
                    }
                    else
                    {
                        normalAttack_aftTimer +=frequency_static;
                    }
                }
              
            }

            normalAttack_Anim_preTimer = frequency * normalAttack_ratio;
            normalAttack_Anim_aftTimer = frequency-normalAttack_Anim_preTimer;
        }

        private bool right;
        private void NormalAttack(int action)
        {
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
                    // 点击自己直接退出
                    if(!_forceAttack||_player.Uid.Equals(gameData.Uid)||(_target!=null&&_target.Uid.Equals(gameData.Uid))) return;
                    timeTemp =Time.time;
                    // 设置为负数，表示第一次选中角色，等待进行移动，移动完毕（攻击距离够）时设置为0
                    timer = -10;
                    duration = Time.time;
                    _inputAttack = true;
                    isAttacked = false;
                    isAttackAnim = false;
                    CheckActionState();
                    _target = gameData;
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
                    if (_forceAttack)
                    {
                        CheckActionState();
                    }
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
                    timer = 0;
                    isAttacked = false;
                    isAttackAnim = false;
                    StopAction();
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