using System;
using System.Collections.Generic;
using ActionPool;
using Commons;
using Commons.Constants;
using Data;
using Domain.Contexts;
using Domain.Data;
using Domain.Data.ActionData;
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
            _gdChaPlayer = _playerContext.Get<GDChaPlayer>(Constants_Context.PlayerData);
        }
    }

    private GDChaPlayer _gdChaPlayer;

    public GDChaPlayer GdChaPlayer
    {
        get => _gdChaPlayer;
        set => _gdChaPlayer = value;
    }

    private MouseController _mouse;
    private IDisposable subscription;
    private IDisposable chatroomSubscription;
    private Messenger messenger;

    
    private void Awake()
    {
        _instance = this;
        messenger = Messenger.Default;
        InitMessageObjs();
    }

    private void Start()
    {
        _mouse=MouseController.Instance;
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
        _gameData = new MGameData(this,_gdChaPlayer);
    }
    // Update is called once per frame
    void Update()
    {
        if(_gdChaPlayer==null) return;
        if (Input.GetKeyDown(KeyCode.D))
        {
            _gdChaPlayer.GetAttribute<AHealth>(TypedAttribute.Health).UpdateCurrentValue(-1000);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _gdChaPlayer.GetAttribute<AHealth>(TypedAttribute.Health).UpdateCurrentValue(1000);
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            GDCharacter gdCharacter = _mouse.GdCharacter;
            if (gdCharacter != null)
            {
                _mMouseTarget.MousePosition = _mouse.MousePosition;
                _mMouseTarget.GdCharacter = gdCharacter;

                // 地面暂时不加入点击选择
                if (!(gdCharacter is DFloor))
                {
                    _gdChaPlayer.Target = gdCharacter;
                }
                // 鼠标点击到（可移动位置）
                if (gdCharacter.CanMoved&&_mouse.MousePosition!=Vector3.zero)
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
            GDCharacter gdCharacter = _mouse.GdCharacter;
            _mInput.ForceAttack = false;
            if (gdCharacter != null)
            {
                _mMouseTarget.MousePosition = _mouse.MousePosition;
                _mMouseTarget.GdCharacter = gdCharacter;
                // 地面暂时不加入点击选择
                if (!(gdCharacter is DFloor))
                {
                    _gdChaPlayer.Target = gdCharacter;
                }
                // 鼠标点击到（可移动位置）
                if (gdCharacter.CanMoved&&_mouse.MousePosition!=Vector3.zero)
                {
                    messenger.Publish(TypedInputActions.OnKeyDown_Mouse0_Walkable.ToString(), _mMouseTarget);
                }
                else
                {
                    if (gdCharacter.Uid != _gdChaPlayer.Uid)
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
            GDChaPlayer gdChaPlayer = _gameData.GameData as GDChaPlayer;
            gdChaPlayer.AttackRange=1.2f;
            gdChaPlayer.WeaponType = TypedWeapon.Unarmed;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GDChaPlayer gdChaPlayer = _gameData.GameData as GDChaPlayer;
            gdChaPlayer.AttackRange=4.5f;
            gdChaPlayer.WeaponType = TypedWeapon.TwoHandBow;
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
        
    }
}
