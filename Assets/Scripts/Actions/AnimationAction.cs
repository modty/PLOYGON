using System;
using ActionPool;
using Scripts;
using Scripts.Commons;
using UnityEngine;

namespace Actions
{
    
    
    /// <summary>
    /// 管理角色动画
    /// </summary>
    public class AnimationAction:BaseAction
    {
        private PlayerAttribute _player;

        private Animator _animator;
        private Rigidbody _rigidbody;
        
        public AnimationAction()
        {
        }

        /// <summary>
        /// 引用属性必须在构造器中提前获取引用。
        /// </summary>
        /// <param name="player"></param>
        public AnimationAction(PlayerAttribute player)
        {
            _player = player;
            // 获取需要属性
            _animator = player.Animator;
            _rigidbody = player.Rigidbody;
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
            if (_player.IsMoving)
            {
                moveSpeed = _player.MoveSpeed;
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
        
        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegistInputActions()
        {
            EventCenter.AddListener<int,int>(TypedInputActions.AnimNormalAttack.ToString(),NormalAttack);
        }

        private void NormalAttack(int weapon,int action)
        {
            _animator.SetFloat(Constants_Anim.NormalAttackSpeed_Float,_player.NormalAttackAnimSpeed);
            _animator.SetInteger(Constants_Anim.Weapon_Int,weapon);
            _animator.SetInteger(Constants_Anim.Action_Int,action);
            _animator.SetTrigger(Constants_Anim.AttackTrigger);
        }
    }
}