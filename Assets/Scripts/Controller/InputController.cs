using System;
using System.Collections;
using System.Collections.Generic;
using ActionPool;
using Actions;
using Commons;
using Data;
using Scripts;
using UnityEngine;

public class InputController : MonoBehaviour
{
    /// <summary>
    /// 临时链接
    /// </summary>
    public GameObject modelObject;

    private PlayerAttribute _attribute;
    public GameObject skillRange;
    private MovementAction _movementAction;
    private AnimationAction _animationAction;
    private SceneUIAction _sceneUIAction;
    private NormalAttackAction _normalAttackAction;
    private SkillAreaAction _skillAreaAction;
    public float moveSpeed=5f;

    private MouseController _mouse;
    // Start is called before the first frame update
    void Start()
    {
        _attribute = new PlayerAttribute(modelObject);
        _attribute.RotateSpeed = 3600f;
        _attribute.AttackRange = 4f;
        _attribute.MoveAcceleration = 12;
        _attribute.WeaponType = TypedWeapon.Unarmed;
        _attribute.AttackRangeUI = skillRange.transform;
        _movementAction = new MovementAction(_attribute);
        _animationAction = new AnimationAction(_attribute);
        _sceneUIAction = new SceneUIAction();
        _normalAttackAction = new NormalAttackAction(_attribute);
        _skillAreaAction = new SkillAreaAction(_attribute);
        _mouse=MouseController.Get();
    }

    // Update is called once per frame
    void Update()
    {
        _attribute.MoveSpeed = moveSpeed;
        _animationAction.Update();
        _skillAreaAction.Update();
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            GameData gameData = _mouse.GameData;
            if (gameData != null)
            {
                // 鼠标点击到（可移动位置）
                if (gameData.CanMoved&&_mouse.MousePosition!=Vector3.zero)
                {
                    EventCenter.Broadcast(TypedInputActions.OnKeyDown_Mouse1_Walkable.ToString(),
                        true,_mouse.MousePosition);
                }
                else
                {
                    EventCenter.Broadcast(TypedInputActions.OnKeyDown_Mouse1_Target.ToString(), gameData);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventCenter.Broadcast(TypedInputActions.OnForceAttack.ToString());
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            EventCenter.Broadcast(TypedInputActions.OffForceAttack.ToString());
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EventCenter.Broadcast(TypedInputActions.NormalAttack.ToString(),1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EventCenter.Broadcast(TypedInputActions.NormalAttack.ToString(),2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EventCenter.Broadcast(TypedInputActions.NormalAttack.ToString(),3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            EventCenter.Broadcast(TypedInputActions.NormalAttack.ToString(),4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            EventCenter.Broadcast(TypedInputActions.NormalAttack.ToString(),5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            EventCenter.Broadcast(TypedInputActions.NormalAttack.ToString(),6);
        }
        
    }

    private void FixedUpdate()
    {
        _normalAttackAction.Update();
        _movementAction.Update();
    }
}
