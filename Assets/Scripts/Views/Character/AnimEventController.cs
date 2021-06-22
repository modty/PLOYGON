using System;
using Commons;
using Domain.MessageEntities;
using Loxodon.Framework.Messaging;
using Managers;
using UnityEngine;
using Views;

namespace Scripts
{
    public class AnimEventController:MonoBehaviour
    {
        public GameObject combatText;
        public Transform combatTextParent;

        public PlayerData _player;

        private Messenger _messenger;

        [SerializeField]
        private Transform leftHand;
        [SerializeField]
        private Transform rightHand;
        
        
        private void Awake()
        {
            _messenger=Messenger.Default;
            _mCombatTextCreate = new MCombatTextCreate(this);

        }

        private float timer=0;
        private MCombatTextCreate _mCombatTextCreate;

        public void Shoot()
        {
            GameObject arrow = PrefabsManager.Instance.Arrow;
            Projectile projectile = arrow.GetComponent<Projectile>();
            SoundManager.Instance.PlayReleasingStringBow();
            PlayerData target = _player.Target as PlayerData;
            var position = _player.Transform.position;
            var position2 = rightHand.position;
            arrow.transform.position = position2;
            if (target != null)
            {
                var position1 = target.TargetCenter;
                projectile.transform.rotation = Quaternion.LookRotation(position1-position.normalized, Vector3.up);
                projectile.shooter = _player;
                projectile.dir = position1-position2;
            }

            projectile.damage = (int) _player.AttackDamage.CurrentValue();
            
        }
        public void Hit()
        {
            if(_player.Target==null||_player.Target.Uid==_player.Uid) return;
            PlayerData target=_player.Target as PlayerData;
            Debug.Log(Time.time-timer);
            timer = Time.time;
            SoundManager.Instance.Play();
            var position = _player.Target.Transform.position;
            position.y = .4f;
            target?.Health.UpdateCurrentValue(-(int)_player.AttackDamage.CurrentValue());
            _mCombatTextCreate.Position = position;
            _mCombatTextCreate.Text = _player.AttackDamage.CurrentValue().ToString();
            _mCombatTextCreate.Type = SCTTYPE.DAMAGE;
            _mCombatTextCreate.Direction = position.x > _player.Transform.position.x;
            _messenger.Publish(Constants_Event.CombatTextCreate,_mCombatTextCreate);
        }
    }
}