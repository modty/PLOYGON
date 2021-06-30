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

        }

        private float timer=0;

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

            projectile.damage = (int) _player.GetAttribute<AAttackDamage>(TypedAttribute.AttackDamage).CurrentValue;

        }
        public void Hit()
        {
            if(_player.Target==null||_player.Target.Uid==_player.Uid) return;
            PlayerData target=_player.Target as PlayerData;
            Debug.Log(Time.time-timer);
            timer = Time.time;
            SoundManager.Instance.Play();
            int damage = (int) _player.GetAttribute<AAttackDamage>(TypedAttribute.AttackDamage).CurrentValue;
            target?.GetAttribute<AHealth>(TypedAttribute.Health).UpdateCurrentValue(-damage);
        }
    }
}