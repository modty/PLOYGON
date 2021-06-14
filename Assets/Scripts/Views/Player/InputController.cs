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

    public PlayerData data;
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
        _movementState = new MovementState(data);
        _animationState = new AnimationState(data);
        _sceneUIState = new SceneUIState();
        _normalAttackState = new NormalAttackState(data);
        _skillAreaState = new SkillAreaState(data);
        _mouse=MouseController.Get();
        InitMessageObjs();
        _gameData.GameData = data;
        messenger.Publish(TypedUIElements.PlayerMes.ToString(),_gameData);

    }
    #region 消息通信

    private MInput _mInput;
    private MMouseTarget _mMouseTarget;
    private MMovement _mMovement;
    private MGameData _gameData;
    #endregion
    
    private void InitMessageObjs()
    {
        _mInput = new MInput(this);
        _mMouseTarget = new MMouseTarget(this);
        _mMovement = new MMovement(this);
        _gameData = new MGameData(this,null);
    }
    // Update is called once per frame
    void Update()
    {
        data.MoveSpeed = moveSpeed;
        _animationState.Update();
        _skillAreaState.Update();

        if (Input.GetKeyDown(KeyCode.D))
        {
            data.Health.UpdateCurrentValue(-1000);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            data.Health.UpdateCurrentValue(1000);
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            GameData gameData = _mouse.GameData;
            if (gameData != null)
            {
                _mMouseTarget.MousePosition = _mouse.MousePosition;
                _mMouseTarget.GameData = gameData;

                // 地面暂时不加入点击选择
                if (!(gameData is FloorAttribute))
                {
                    data.Target = gameData;
                }
                // 鼠标点击到（可移动位置）
                if (gameData.CanMoved&&_mouse.MousePosition!=Vector3.zero)
                {

                    /*EventCenter.Broadcast(TypedInputActions.OnKeyDown_Mouse1_Walkable.ToString(),
                        true,_mouse.MousePosition);*/
                    messenger.Publish(TypedInputActions.OnKeyDown_Mouse1_Walkable.ToString(), _mMouseTarget);
                }
                else
                {

                    /*EventCenter.Broadcast(TypedInputActions.OnKeyDown_Mouse1_Target.ToString(), gameData);*/
                    messenger.Publish(TypedInputActions.OnKeyDown_Mouse1_Target.ToString(), _mMouseTarget);

                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameData gameData = _mouse.GameData;
            _mInput.ForceAttack = false;
            if (gameData != null)
            {
                _mMouseTarget.MousePosition = _mouse.MousePosition;
                _mMouseTarget.GameData = gameData;
                // 地面暂时不加入点击选择
                if (!(gameData is FloorAttribute))
                {
                    data.Target = gameData;
                }
                // 鼠标点击到（可移动位置）
                if (gameData.CanMoved&&_mouse.MousePosition!=Vector3.zero)
                {
                    messenger.Publish(TypedInputActions.OnKeyDown_Mouse0_Walkable.ToString(), _mMouseTarget);
                }
                else
                {
                    messenger.Publish(TypedInputActions.OnKeyDown_Mouse0_Target.ToString(), _mMouseTarget);
                }
                messenger.Publish(TypedInputActions.ForceAttack.ToString(), _mInput);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _mInput.ForceAttack = true;
            messenger.Publish(TypedInputActions.ForceAttack.ToString(), _mInput);
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
