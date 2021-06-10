using System;
using ActionPool;
using Data;
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
            UpdateAttackSpeed(2.5f);

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
                if (_player.IsMoving)
                {
                    // 确保帧同步，不使用EventCenter
                    _movementState.StopMove();
                }
                // 进入攻击动画
                if (isAttackAnim)
                {
                    // 进行攻击（为了匹配动画，使用动画的事件进行处理）
                    if (isAttacked)
                    {
                        // 动画播放结束
                        if (normalAttack_Anim_aftTimer-timer <.005f)
                        {
                            Debug.Log("3：动画后摇结束，等待静态后摇："+timer);
                            isAttackAnim = false;
                            timer = 0;
                            // 没有攻击静态攻击后摇，直接退出，不进入下一帧计算，需要补充时间
                            if (normalAttack_aftTimer < .005f)
                            {
                                Debug.Log("4：没有静态后摇，直接等待静态前摇："+timer);
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
                            Debug.Log("2：动画前摇结束，进行攻击："+timer);
                            Debug.Log("攻击：间隔："+(Time.time - duration)+"|||||"+normalAttack_preTimer+"："+normalAttack_Anim_preTimer+"："+normalAttack_Anim_aftTimer+"："+normalAttack_aftTimer);
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
                            Debug.Log("4：动画前摇结束，攻击完毕："+timer);
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
                            Debug.Log("1:没有静态前摇，直接等待动画前摇："+timer);
                            NormalAttack(1);
                            isAttackAnim = true;
                            DoUpdate();
                        }
                        else if (normalAttack_preTimer-timer <.005f)
                        {
                            Debug.Log("1：静态前摇结束，等待动画前摇："+timer);
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
            EventCenter.Broadcast(TypedInputActions.AnimNormalAttack.ToString(),(int)_player.WeaponType,action);
        }
        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegistInputActions()
        {
            EventCenter.AddListener<GameData>(TypedInputActions.OnKeyDown_Mouse0_Target.ToString(),(gameData)=>
            {
                Debug.Log("目标:"+_player+"--"+gameData);
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

            EventCenter.AddListener<GameData>(TypedInputActions.OnKeyDown_Mouse1_Target.ToString(),(gameData)=>
            {
                // 点击自己直接退出
                if(_player.Uid.Equals(gameData.Uid)) return;
                timer = 0;
                isAttacked = false;
                isAttackAnim = false;
                StopAction();
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
            
            // 点击地面，停止当前动作
            EventCenter.AddListener<bool,Vector3>(TypedInputActions.OnKeyDown_Mouse1_Walkable.ToString(),(isNew,position)=>
            {
                timer = 0;
                isAttacked = false;
                isAttackAnim = false;
                StopAction();
            });
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