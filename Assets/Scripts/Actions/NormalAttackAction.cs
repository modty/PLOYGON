using System;
using ActionPool;
using Data;
using Scripts;
using UnityEngine;

namespace Actions
{
    /// <summary>
    /// 普通攻击
    /// </summary>
    public class NormalAttackAction:BaseAction
    {
        // 强制攻击指令
        private bool _forceAttack;
        // 攻击输入
        private bool _inputAttack;

        private PlayerAttribute _player;

        private Transform _transform;
        private MovementAction _movementAction;

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
        /// 动画加速倍率。
        /// 普通攻击冷却缩减规则如下：
        ///     1、当静态前摇/后摇大于0时，缩减静态前后摇（平均分配）。
        ///     2、当静态前后摇为0时，加速动作进行缩短。
        /// </summary>
        private float animMultiply;
        
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
        
        public NormalAttackAction(PlayerAttribute playerAttribute):base()
        {
            _player = playerAttribute;
            _transform = _player.Transform;
            _movementAction = playerAttribute.MovementAction;
            RegistInputActions();
            
            // 设置攻击前摇为0.7f，攻击后摇为0.7f，每次攻击需要1.4f，攻速度0.71，即每秒攻击0.71次，
            normalAttack_preTimer = 0f;
            normalAttack_aftTimer = 0f;
            normalAttack_Anim_aftTimer = 0.4f;
            normalAttack_Anim_preTimer = 0.6f;
            normalAttack_aftTimer_max = .4f;
            normalAttack_ratio = normalAttack_Anim_preTimer;
            UpdateAttackSpeed(0.8f);

        }
        protected override void DoUpdate()
        {
            if(_target==null) return;
            float currentTime = Time.time;
            timer +=currentTime -timeTemp;
            timeTemp=currentTime;
            float distance = (_target.Transform.position - _transform.position).magnitude;
            // 如果角色正在移动，且进入攻击距离，停止
            if (distance <= _player.AttackRange)
            {
                if (_player.IsMoving)
                {
                    // 确保帧同步，不使用EventCenter
                    _movementAction.StopMove();
                }

                // 进入攻击动画
                if (isAttackAnim)
                {
                    // 进行攻击（为了匹配动画，使用动画的事件进行处理）
                    if (isAttacked)
                    {
                        // 动画播放结束
                        if (Math.Abs(timer - normalAttack_Anim_aftTimer)<.01f)
                        {
                            //Debug.Log("3：动画后摇结束，等待静态后摇："+timer);
                            
                            isAttackAnim = false;
                            timer = 0;
                            // 没有攻击静态攻击后摇，直接退出，不进入下一帧计算，需要补充时间
                            if (normalAttack_aftTimer < .01f)
                            {
                                isAttacked = false;
                            }
                        }
                    }
                    // 没有攻击
                    else
                    {
                        if (Math.Abs(timer - normalAttack_Anim_preTimer)<.01f)
                        {
                            //Debug.Log("2：动画前摇结束，进行攻击："+timer);
                            Debug.Log("攻击：间隔："+(Time.time - duration)+"|||||"+normalAttack_preTimer+"："+normalAttack_Anim_preTimer+"："+normalAttack_Anim_aftTimer+"："+normalAttack_Anim_preTimer);
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
                        if (Math.Abs(timer - normalAttack_aftTimer)<.01f)
                        {
                            isAttacked = false;
                            isAttackAnim = false;
                            timer = 0;
                        }
                       
                    }
                    // 攻击前摇
                    else
                    {
                        // 如果没有静态攻击前摇，不进行时间重置
                        if (normalAttack_preTimer <.01f)
                        {
                            NormalAttack(1);
                            isAttackAnim = true;
                        }
                        else if (Math.Abs(timer - normalAttack_preTimer)<.01f)
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
                EventCenter.Broadcast(TypedInputActions.MoveTo.ToString(),_target.Transform.position);
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
                animMultiply = at;
                normalAttack_preTimer = 0;
                normalAttack_aftTimer = 0;
            }
            // 不需要加速
            else
            {
                // 动画不变
                animMultiply = 1;
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

        private void NormalAttack(int action)
        {
            EventCenter.Broadcast(TypedInputActions.AnimNormalAttack.ToString(),(int)_player.WeaponType,action);
        }
        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegistInputActions()
        {
            timer = 0;
            isAttacked = false;
            isAttackAnim = false;
            EventCenter.AddListener<GameData>(TypedInputActions.OnKeyDown_Mouse1_Target.ToString(),(gameData)=>
            {
                // 初始化时需要设置为Time.fixedDeltaTime,因为Action需要下一次调用才刷新
                timer = 0;
                timeTemp =Time.time;
                duration = Time.time;
                _inputAttack = true;
                isAttacked = false;
                isAttackAnim = false;
                CheckActionState();
                _target = gameData;
            });
            EventCenter.AddListener(TypedInputActions.OnForceAttack.ToString(),()=>
            {
                _forceAttack = true;
                CheckActionState();
            });
            EventCenter.AddListener(TypedInputActions.OffForceAttack.ToString(),()=>
            {
                _forceAttack = false;
            });
            EventCenter.AddListener(TypedInputActions.StopAttack.ToString(), () =>
            {
                _target = null;
                timer = 0;
                isAttacked = false;
                isAttackAnim = false;
                StopAction();
            });
            EventCenter.AddListener<int>(TypedInputActions.NormalAttack.ToString(),NormalAttack);
        }



        private void CheckActionState()
        {
            Debug.Log(_inputAttack+"--"+_forceAttack+"--"+stop);
            if (_inputAttack && _forceAttack&&stop)
            {
                // 准备攻击
                StartAction();
            }
        }
    }
}