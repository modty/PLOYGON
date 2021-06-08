using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CombatText : MonoBehaviour {


    [SerializeField]
    private Text text;

    private void Awake()
    {
        speed_xz=Random.Range(.5f,.7f);
        speed_y=Random.Range(2f,2f);
        speed_y_acc = 4;
    }


    // X轴运动速度
    private float speed_xz;
    // T轴运动速度
    private float speed_y;
    // Y轴加速度
    private float speed_y_acc;

    private int _direction;
    // Use this for initialization
	void Start ()
    {
        StartCoroutine(FadeOut());
        _direction = 1;
    }

    public int Direction
    {
        get => _direction;
        set
        {
            _direction = value;
            speed_xz *= _direction;
        }
    }
	// Update is called once per frame
	void Update ()
    {
        Vector3 position = transform.position;
        position.x += speed_xz * Time.deltaTime;
        position.y += speed_y * Time.deltaTime;
        //Vector3 newPosition =position+ new Vector3(0, speed_y, speed_xz).normalized *Time.deltaTime;
        transform.position = position;
        speed_y -= speed_y_acc * Time.deltaTime;
        
	}

    public IEnumerator FadeOut()
    {
        float startAlpha = text.color.a;


        float progress = 0.0f;

        while (progress < 1.0)
        {
            Color tmp = text.color;

            tmp.a = Mathf.Lerp(startAlpha, 0, progress*progress);

            text.color = tmp;

            progress +=  Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);

   
    }
}
