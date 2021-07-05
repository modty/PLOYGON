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

        public GDChaPlayer gdChaPlayer;

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
            GDChaPlayer target = gdChaPlayer.Target as GDChaPlayer;
            var position = gdChaPlayer.Transform.position;
            var position2 = rightHand.position;
            arrow.transform.position = position2;
            if (target != null)
            {
                var position1 = target.TargetCenter;
                projectile.transform.rotation = Quaternion.LookRotation(position1-position.normalized, Vector3.up);
                projectile.shooter = gdChaPlayer;
                projectile.dir = position1-position2;
            }

            projectile.damage = (int) gdChaPlayer.GetAttribute<AAttackDamage>(TypedAttribute.AttackDamage).CurrentValue;

        }
        public void Hit()
        {
            if(gdChaPlayer.Target==null||gdChaPlayer.Target.Uid==gdChaPlayer.Uid) return;
            GDChaPlayer target=gdChaPlayer.Target as GDChaPlayer;
            Debug.Log(Time.time-timer);
            timer = Time.time;
            SoundManager.Instance.Play();
            int damage = (int) gdChaPlayer.GetAttribute<AAttackDamage>(TypedAttribute.AttackDamage).CurrentValue;
            target?.GetAttribute<AHealth>(TypedAttribute.Health).UpdateCurrentValue(-damage);
        }
    }
}