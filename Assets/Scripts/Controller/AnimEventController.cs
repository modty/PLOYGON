using Managers;
using UnityEngine;

namespace Scripts
{
    public class AnimEventController:MonoBehaviour
    {
        public GameObject combatText;
        public Transform combatTextParent;
        public void Hit()
        {
            SoundManager.Instance.Play();
            CombatTextManager.Instance.CreateText(transform.position,"100",SCTTYPE.DAMAGE,false);
        }
    }
}