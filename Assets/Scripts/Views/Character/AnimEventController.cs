using System;
using Commons;
using Domain.MessageEntities;
using Loxodon.Framework.Messaging;
using Managers;
using UnityEngine;

namespace Scripts
{
    public class AnimEventController:MonoBehaviour
    {
        public GameObject combatText;
        public Transform combatTextParent;

        public PlayerData _player;
        private Messenger _messenger;
        private void Awake()
        {
            _messenger=Messenger.Default;
            _mCombatTextCreate = new MCombatTextCreate(this);
        }

        private float timer=0;
        private MCombatTextCreate _mCombatTextCreate;
        public void Hit()
        {
            if(_player.Target==null||_player.Target.Uid==_player.Uid) return;
            //Debug.Log(Time.time-timer);
            timer = Time.time;
            SoundManager.Instance.Play();
            var position = _player.Target.Transform.position;
            position.y = .4f;
            _mCombatTextCreate.Position = position;
            _mCombatTextCreate.Text = _player.AttackDamage.CurrentValue().ToString();
            _mCombatTextCreate.Type = SCTTYPE.DAMAGE;
            _mCombatTextCreate.Direction = position.x > _player.Transform.position.x;
            _messenger.Publish(Constants_Event.CombatTextCreate,_mCombatTextCreate);
        }
    }
}