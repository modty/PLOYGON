using System.Collections;
using System.Collections.Generic;
using Scripts;
using UnityEngine;

namespace Views
{
    /// <summary>
    /// A projectile shot with a ranged weapon
    /// </summary>

    public class Projectile : MonoBehaviour
    {
        public int damage = 1;
        public float speed = 1f;
        public float duration = 10f;


        [HideInInspector]
        public Vector3 dir;

        [HideInInspector]
        public PlayerData shooter;

        private Vector3 curve_dir = Vector3.zero;
        private float curve_dist = 0f;
        private float timer = 0f;

        void Start()
        {
            SoundManager.Instance.PlayArrowFlyingPast();
        }

        void Update()
        {
            if (curve_dist > 0.01f && (timer * speed) < curve_dist)
            {
                //Initial curved dir (only in freelook mode)
                float value = Mathf.Clamp01(timer * speed / curve_dist);
                Vector3 cdir = (1f - value) * curve_dir + value * dir;
                transform.position += cdir * speed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(cdir.normalized, Vector2.up);
            }
            else
            {
                //Regular dir
                transform.position += dir * speed * Time.deltaTime;
                transform.rotation = Quaternion.LookRotation(dir.normalized, Vector2.up);
            }

            timer += Time.deltaTime;
            if (timer > duration)
                Destroy(gameObject);
        }

        public void SetInitialCurve(Vector3 dir, float dist = 10f)
        {
            curve_dir = dir;
            curve_dist = dist * 1.25f; //Add offset for more accuracy
        }

        private void OnTriggerEnter(Collider collision)
        {
            PlayerData playerData = collision.GetComponent<DataController>().GameData as PlayerData;
            if (playerData!=null&&playerData.Uid != shooter.Uid)
            {
                SoundManager.Instance.PlayArrowImpactflesh();
                Debug.Log(collision.name);
                Destroy(gameObject);
            }
        }
    }

}