using Managers;
using UnityEngine;

namespace Scripts
{
    public class AnimEventController:MonoBehaviour
    {
        public GameObject combatText;
        public Transform combatTextParent;


        public PlayerAttribute _player;

        private float timer=0;
        public void Hit()
        {
            if(_player.Target==null||_player.Target.Uid==_player.Uid) return;
            //Debug.Log(Time.time-timer);
            timer = Time.time;
            SoundManager.Instance.Play();
            var position = _player.Target.Transform.position;
            position.y = .4f;
            CombatTextManager.Instance.CreateText(position,_player.AttackDamage.CurrentValue().ToString(),SCTTYPE.DAMAGE,false,position.x>_player.Transform.position.x);
        }
    }
}