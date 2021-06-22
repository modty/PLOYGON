using System;
using System.Collections.Generic;
using ActionPool;
using Commons;
using Commons.Constants;
using Data;
using Domain.Contexts;
using Domain.MessageEntities;
using Loxodon.Framework.Messaging;
using Scripts;
using States;
using UnityEngine;
using AnimationState = States.AnimationState;

public class InputController : MonoBehaviour
{

    private static InputController _instance;

    public static InputController Instance
    {
        get => _instance;
    }

    private CharacterContext _playerContext;

    public CharacterContext PlayerContext
    {
        get => _playerContext;
        set
        {
            _playerContext = value;
            _playerData = _playerContext.Get<PlayerData>(Constants_Context.PlayerData);
        }
    }

    private PlayerData _playerData;


    private MouseController _mouse;
    private IDisposable subscription;
    private IDisposable chatroomSubscription;
    private Messenger messenger;

    
    private void Awake()
    {
        _instance = this;
        messenger = Messenger.Default;
        _mouse=MouseController.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {

        _mouse=MouseController.Instance;
        InitMessageObjs();
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
        _gameData = new MGameData(this,_playerData);
    }
    // Update is called once per frame
    void Update()
    {
        if(_playerData==null) return;

        if (Input.GetKeyDown(KeyCode.D))
        {
            _playerData.Health.UpdateCurrentValue(-1000);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _playerData.Health.UpdateCurrentValue(1000);
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
                    _playerData.Target = gameData;
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
                    _playerData.Target = gameData;
                }
                // 鼠标点击到（可移动位置）
                if (gameData.CanMoved&&_mouse.MousePosition!=Vector3.zero)
                {
                    messenger.Publish(TypedInputActions.OnKeyDown_Mouse0_Walkable.ToString(), _mMouseTarget);
                }
                else
                {
                    if (gameData.Uid != _playerData.Uid)
                    {
                        messenger.Publish(TypedInputActions.OnKeyDown_Mouse0_Target.ToString(), _mMouseTarget);
                    }
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
            PlayerData playerData = _gameData.GameData as PlayerData;
            playerData.AttackRange=1.2f;
            playerData.WeaponType = TypedWeapon.Unarmed;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerData playerData = _gameData.GameData as PlayerData;
            playerData.AttackRange=4.5f;
            playerData.WeaponType = TypedWeapon.TwoHandBow;
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
}
