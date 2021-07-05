using ActionPool;
using Commons;
using Domain.MessageEntities;
using Loxodon.Framework.Messaging;
using Scripts;
using Scripts.Commons;
using UnityEngine;

namespace States
{
    
    
    /// <summary>
    /// 管理角色动画
    /// </summary>
    public class AnimationState:BaseState
    {
        private GDChaPlayer _gdChaPlayer;

        private Animator _animator;
        private Rigidbody _rigidbody;
        
        public AnimationState()
        {
        }

        /// <summary>
        /// 引用属性必须在构造器中提前获取引用。
        /// </summary>
        /// <param name="gdChaPlayer"></param>
        public AnimationState(GDChaPlayer gdChaPlayer)
        {
            _gdChaPlayer = gdChaPlayer;
            // 获取需要属性
            _animator = gdChaPlayer.Animator;
            _rigidbody = gdChaPlayer.Rigidbody;
            RegistInputActions();
            StartAction();
        }
        
        protected override void DoUpdate()
        {
            MoveSpeedSet();
        }

        #region MoveSpeedSet()方法变量

        private float moveSpeed;
        #endregion
        private void MoveSpeedSet()
        {
            if (_gdChaPlayer.IsMoving)
            {
                moveSpeed = _gdChaPlayer.MoveSpeed;
                if (moveSpeed > 0)
                {
                    _animator.SetBool(Constants_Anim.Moving_Bool,true);
                    _animator.SetFloat(Constants_Anim.VelocityZ_Float,moveSpeed);
                    if (moveSpeed > 6)
                    {
                        _animator.SetFloat(Constants_Anim.Charge_Float,(moveSpeed-6)/10);
                    }
                }
                else
                {
                    _animator.SetBool(Constants_Anim.Moving_Bool,false);
                    _animator.SetFloat(Constants_Anim.VelocityZ_Float,0);
                }

            }
            else
            {
                _animator.SetBool(Constants_Anim.Moving_Bool,false);
                _animator.SetFloat(Constants_Anim.VelocityZ_Float,0);
            }
            
        }

        #region 订阅引用

        private ISubscription<MAnimNormalAttack> _subscriptionAnimNormalAttack;
        private ISubscription<MAnimNormalAttack> _subscriptionOnWeaponChange;
        private ISubscription<MAnimNormalAttack> _subscriptionAnimNormalAttackStop;
        

        #endregion
        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegistInputActions()
        {
            _subscriptionAnimNormalAttack=_messenger.Subscribe<MAnimNormalAttack>(TypedInputActions.AnimNormalAttack.ToString(), (message) =>
            {
                _animator.SetBool(Constants_Anim.StopAction_bool,message.Stop);
                NormalAttack(message.WeaponType,message.Action);
            });
            _subscriptionAnimNormalAttackStop=_messenger.Subscribe<MAnimNormalAttack>(TypedInputActions.AnimNormalAttackStop.ToString(), (message) =>
            {
                _animator.SetBool(Constants_Anim.StopAction_bool,message.Stop);
            });
           
            _subscriptionOnWeaponChange = _messenger.Subscribe<MAnimNormalAttack>(Constants_Event.PlayerWeaponChange,
                (message) =>
                {
                    _animator.SetTrigger(Constants_Anim.InstantSwitchTrigger);
                    _animator.SetInteger(Constants_Anim.Weapon_Int,message.WeaponType);
                });

        }

        private void NormalAttack(int weapon,int action)
        {
            _animator.SetFloat(Constants_Anim.NormalAttackSpeed_Float,_gdChaPlayer.NormalAttackAnimSpeed);
            _animator.SetInteger(Constants_Anim.Weapon_Int,weapon);
            _animator.SetInteger(Constants_Anim.Action_Int,action);
            _animator.SetTrigger(Constants_Anim.AttackTrigger);
        }
    }
}