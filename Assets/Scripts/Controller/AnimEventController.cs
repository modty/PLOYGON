using UnityEngine;

namespace Scripts
{
    public class AnimEventController:MonoBehaviour
    {
        
        public void Hit()
        {
            SoundManager.Instance.Play();
        }
    }
}