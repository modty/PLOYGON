using ActionPool;
using Domain.MessageEntities;
using Loxodon.Framework.Messaging;
using Scripts;
using UnityEngine;

namespace States
{
    /// <summary>
    /// 角色的攻击范围
    /// </summary>
    public class SkillAreaState:BaseState
    {
        private PlayerData _player;

        private Transform _playerAttackRange; 
        private Transform _transform; 
        public SkillAreaState(PlayerData playerData)
        {
            _player = playerData;
            _playerAttackRange = _player.AttackRangeUI;
            _transform = _player.Transform;
            RegistInputActions();
        }

        protected override void DoUpdate()
        {
            _playerAttackRange.position = _transform.position;
        }
        #region 订阅引用

        private ISubscription<MMouseTarget> onMouse1Walkable;
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
            
            onForceAttack=_messenger.Subscribe<MInput>(TypedInputActions.ForceAttack.ToString(), (message) =>
            {
                if (message.ForceAttack)
                {
                    float size = _player.AttackRange * 2;
                    _playerAttackRange.localScale = new Vector3(size, .1f, size);
                    _playerAttackRange.position = _transform.position;
                    StartAction();
                    _playerAttackRange.gameObject.SetActive(true);
                }
                else
                {
                    _playerAttackRange.gameObject.SetActive(false);
                    StopAction();
                }
               
            });
        }

    }
}