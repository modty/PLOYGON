using System;
using System.Collections.Generic;
using ActionPool;
using Commons;
using Data;
using Domain.MessageEntities;
using Loxodon.Framework.Messaging;
using Scripts;
using States;
using UnityEngine;
using AnimationState = States.AnimationState;

public class InputController : MonoBehaviour
{
    /// <summary>
    /// 临时链接
    /// </summary>
    public GameObject modelObject;

    public PlayerAttribute _attribute;
    private MovementState _movementState;
    private AnimationState _animationState;
    private SceneUIState _sceneUIState;
    private NormalAttackState _normalAttackState;
    private SkillAreaState _skillAreaState;
    public float moveSpeed=5f;
    public AnimEventController eventController;
    private MouseController _mouse;
    private IDisposable subscription;
    private IDisposable chatroomSubscription;
    private Messenger messenger;
    private void Awake()
    {
        messenger = Messenger.Default;
    }

    // Start is called before the first frame update
    void Start()
    {
        _movementState = new MovementState(_attribute);
        _animationState = new AnimationState(_attribute);
        _sceneUIState = new SceneUIState();
        _normalAttackState = new NormalAttackState(_attribute);
        _skillAreaState = new SkillAreaState(_attribute);
        _mouse=MouseController.Get();
        EventCenter.Broadcast("UIElement:"+TypedUIElements.PlayerMes,(GameData)_attribute);
        InitMessageObjs();

    }

    #region 消息通信

    private InputMessage _inputMessage;
    private MouseTargetMessage _mouseTargetMessage;
    private MovementMessage _movementMessage;
    #endregion
    
    private void InitMessageObjs()
    {
        _inputMessage = new InputMessage(this);
        _mouseTargetMessage = new MouseTargetMessage(this);
        _movementMessage = new MovementMessage(this);
    }
    // Update is called once per frame
    void Update()
    {
        _attribute.MoveSpeed = moveSpeed;
        _animationState.Update();
        _skillAreaState.Update();

        if (Input.GetKeyDown(KeyCode.D))
        {
            _attribute.Health.UpdateCurrentValue(-1000);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _attribute.Health.UpdateCurrentValue(1000);
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            GameData gameData = _mouse.GameData;
            if (gameData != null)
            {
                _mouseTargetMessage.MousePosition = _mouse.MousePosition;
                _mouseTargetMessage.GameData = gameData;

                // 地面暂时不加入点击选择
                if (!(gameData is FloorAttribute))
                {
                    _attribute.Target = gameData;
                }
                // 鼠标点击到（可移动位置）
                if (gameData.CanMoved&&_mouse.MousePosition!=Vector3.zero)
                {

                    /*EventCenter.Broadcast(TypedInputActions.OnKeyDown_Mouse1_Walkable.ToString(),
                        true,_mouse.MousePosition);*/
                    messenger.Publish(TypedInputActions.OnKeyDown_Mouse1_Walkable.ToString(), _mouseTargetMessage);
                }
                else
                {

                    /*EventCenter.Broadcast(TypedInputActions.OnKeyDown_Mouse1_Target.ToString(), gameData);*/
                    messenger.Publish(TypedInputActions.OnKeyDown_Mouse1_Target.ToString(), _mouseTargetMessage);

                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameData gameData = _mouse.GameData;
            if (gameData != null)
            {
                _mouseTargetMessage.MousePosition = _mouse.MousePosition;
                _mouseTargetMessage.GameData = gameData;
                // 地面暂时不加入点击选择
                if (!(gameData is FloorAttribute))
                {
                    _attribute.Target = gameData;
                }
                // 鼠标点击到（可移动位置）
                if (gameData.CanMoved&&_mouse.MousePosition!=Vector3.zero)
                {
                    messenger.Publish(TypedInputActions.OnKeyDown_Mouse0_Walkable.ToString(), _mouseTargetMessage);
                }
                else
                {
                    /*EventCenter.Broadcast(TypedInputActions.OnKeyDown_Mouse1_Target.ToString(), gameData);*/
                    messenger.Publish(TypedInputActions.OnKeyDown_Mouse0_Target.ToString(), _mouseTargetMessage);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _inputMessage.ForceAttack = true;
            messenger.Publish(TypedInputActions.ForceAttack.ToString(), _inputMessage);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _inputMessage.ForceAttack = false;
            messenger.Publish(TypedInputActions.ForceAttack.ToString(), _inputMessage);
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
        _normalAttackState.Update();
        _movementState.Update();
    }
}
