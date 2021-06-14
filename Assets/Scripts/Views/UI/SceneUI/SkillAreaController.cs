using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAreaController : MonoBehaviour
{
    public Transform parent;

    // Update is called once per frame
    void Update()
    {
        var position = parent.position;
        transform.position = new Vector3(position.x,.2f,position.z);
    }
}
