using System;
using System.Collections.Generic;
using ActionPool;
using Scripts;
using UnityEngine;

public class SceneUIController:MonoBehaviour
{
    [SerializeField]
    private GameObject _clickMovePointer;

    [SerializeField] private Transform _sceneUIParent;
    private void Start()
    {
        EventCenter.AddListener<bool,Vector3>(TypedInputActions.OnKeyDown_Mouse1_Walkable.ToString(),OnClickMouseRightWalkable);
    }
    
    
    /// <summary>
    /// 鼠标右键点击移动平台，进行移动，移动只需提供位置即可。
    /// </summary>
    private void OnClickMouseRightWalkable(bool isNewTarget,Vector3 position)
    {
        Instantiate(_clickMovePointer, _sceneUIParent).transform.position=position;
    }
}
