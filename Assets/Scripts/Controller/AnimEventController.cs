using Managers;
using UnityEngine;

namespace Scripts
{
    public class AnimEventController:MonoBehaviour
    {
        public GameObject combatText;
        public Transform combatTextParent;


        public PlayerAttribute _player;
        public void Hit()
        {
            SoundManager.Instance.Play();

            var position = _player.Target.Transform.position;
            position.y = .4f;
            CombatTextManager.Instance.CreateText(position,"100",SCTTYPE.DAMAGE,false,position.x>_player.Transform.position.x);
        }
    }
}