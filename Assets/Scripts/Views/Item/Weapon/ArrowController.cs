using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField]
    private Transform arrow;
    public float speed = 15f;
    public float duration = 10f;
    private Vector3 _targetPosition;
    public GameObject flash;
    public AudioClip arrowFlyingPast;
    public AudioClip arrowHit;
    public Vector3 TargetPosition
    {
        get => _targetPosition;
        set => _targetPosition = value;
    }
    private float timer = 0f;
    private Rigidbody _rigidbody;
    // Start is called before the first frame update
    void Start ()
    {
        if (flash != null)
        {
            
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs == null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
    }

    void FixedUpdate ()
    {
        if (speed != 0)
        {
            transform.position=Vector3.MoveTowards(transform.position, _targetPosition,speed*Time.deltaTime);
        }
    }

    
    public void SetInitialCurve(Vector3 dir, float dist = 10f)
    {
        _targetPosition = dir;
    }

}
