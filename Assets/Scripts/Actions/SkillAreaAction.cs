using ActionPool;
using Scripts;
using UnityEngine;

namespace Actions
{
    /// <summary>
    /// 角色的攻击范围
    /// </summary>
    public class SkillAreaAction:BaseAction
    {
        private PlayerAttribute _player;

        private Transform _playerAttackRange; 
        private Transform _transform; 
        public SkillAreaAction(PlayerAttribute playerAttribute)
        {
            _player = playerAttribute;
            _playerAttackRange = _player.AttackRangeUI;
            _transform = _player.Transform;
            RegistInputActions();
        }

        protected override void DoUpdate()
        {
            _playerAttackRange.position = _transform.position;
        }
        
        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegistInputActions()
        {
            EventCenter.AddListener(TypedInputActions.OnForceAttack.ToString(), () =>
            {
                float size = _player.AttackRange * 2;
                Debug.Log(_playerAttackRange);
                _playerAttackRange.localScale = new Vector3(size, 1, size);
                _playerAttackRange.position = _transform.position;
                StartAction();
                _playerAttackRange.gameObject.SetActive(true);
            });
            EventCenter.AddListener(TypedInputActions.OffForceAttack.ToString(), () =>
            {
                _playerAttackRange.gameObject.SetActive(false);
                StopAction();
            });
        }

    }
}